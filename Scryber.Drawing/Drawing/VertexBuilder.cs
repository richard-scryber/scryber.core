using System;
using System.Collections;
using System.Collections.Generic;

namespace Scryber.Drawing
{

    /// <summary>
    /// Builds a list of vertices (location and angle) for adornments in paths
    /// </summary>
    public class VertexBuilder
    {

        /// <summary>
        /// True if the vertices for the start of the path(s) should be collected (default set from the constructor)
        /// </summary>
        public bool HasStarts { get; protected set; }

        /// <summary>
        /// True if the vertices for the mid points of the path should be collected (default set from the constructor)
        /// </summary>
        public bool HasMids { get; protected set; }

        /// <summary>
        /// True if the vertices for the end of the path(s) should be collected (default set from the constructor)
        /// </summary>
        public bool HasEnds { get; protected set; }

        /// <summary>
        /// If true, then the angle of the start point(s) will be flipped PI radians (180 degrees)
        /// </summary>
        public bool StartIsReversed { get; protected set; }

        /// <summary>
        /// Nullable angle in radians to set the vertexes to (if any). If not set then the angle will be 
        /// </summary>
        public double? ExplicitVertexAngle { get; protected set; }

        /// <summary>
        /// Creates a new vertex builder to can calculate the points for adornments on a graphics path
        /// </summary>
        /// <param name="placements"></param>
        /// <param name="reverseAtStart"></param>
        public VertexBuilder(AdornmentPlacements placements, bool reverseAtStart, double? angle)
            : this((placements & AdornmentPlacements.Start) == AdornmentPlacements.Start,
                (placements & AdornmentPlacements.Middle) == AdornmentPlacements.Middle,
                (placements & AdornmentPlacements.End) == AdornmentPlacements.End,
                reverseAtStart, angle)
        {
        }

        /// <summary>
        /// Creates a new vertex builder to can calculate the points for adornments on a graphics path
        /// </summary>
        /// <param name="hasStarts"></param>
        /// <param name="reverseAtStart"></param>
        public VertexBuilder(bool hasStarts, bool hasMids, bool hasEnds, bool reverseAtStart, double? angle)
        {
            this.HasStarts = hasStarts;
            this.HasMids = hasMids;
            this.HasEnds = hasEnds;
            this.StartIsReversed = reverseAtStart;
            this.ExplicitVertexAngle = angle;
        }


        public IEnumerable<AdornmentVertex> CollectVertices(GraphicsPath forPath)
        {
            List<AdornmentVertex> found = new List<AdornmentVertex>();
            Point cursor = Point.Empty;

            if (!HasStarts && !HasEnds && !HasMids)
                return found;

            foreach (var path in forPath.SubPaths)
            {
                if (path.Operations.Count > 0)
                    this.CollectPathVertices(path, cursor, found);
            }

            return found;
        }

        protected virtual void CollectPathVertices(Path path, Point cursor, List<AdornmentVertex> inVertices)
        {
            if (this.HasStarts)
                this.AddStartVertexAndReturnCursor(path, cursor, inVertices);



            if (this.HasEnds)
                this.AddEndVertextAndReturnCursor(path, cursor, inVertices);
            
            return;

            PathData prev = null;
            PathData curr = path.Operations[0];

            Point location = curr.GetLocation(prev, AdornmentPlacements.Start);
            double angle = 0.0;
            double lastAngle = 0.0;
            bool firstWasMove = false;

            if (this.HasStarts)
            {
                if (curr.Type != PathDataType.Move)
                {
                    angle = curr.GetAngle(null, cursor, location, AdornmentPlacements.Start, this.StartIsReversed);
                    this.AddVertex(location, angle, inVertices);
                }
                else
                {
                    firstWasMove = true;
                }

            }

            for (var i = 1; i < path.Operations.Count; i++)
            {
                cursor = location;
                lastAngle = angle;

                prev = curr;
                curr = path.Operations[i];
                location = curr.GetLocation(prev, AdornmentPlacements.Start);

                if (i == 1 && firstWasMove)
                {
                    //We need to add back in the first vertex which was a move to
                    //and calculate the angle to the second point (taking into account
                    angle = prev.GetAngle(null, cursor, location, AdornmentPlacements.Start, this.StartIsReversed);

                    if (HasStarts)
                    {
                        AddVertex(cursor, angle, inVertices);
                    }
                }
            }
        }

        protected Point AddStartVertexAndReturnCursor(Path path, Point cursor, List<AdornmentVertex> inVertices)
        {
            var curr = path.Operations[0];
            var location = curr.GetLocation(null, AdornmentPlacements.Start);

            if (this.ExplicitVertexAngle.HasValue)
                this.AddVertex(location, this.ExplicitVertexAngle.Value, inVertices);
            else
            {
                double angle = 0.0;
                if (path.Operations.Count > 1)
                {
                    var next = path.Operations[1];
                    if(curr.Type == PathDataType.Move)
                    {
                         angle = next.GetStartAngle(curr, location, this.StartIsReversed) ?? 0.0;
                        
                    }
                    else
                    {
                        angle = GetCombinedAngle(curr, next, location);
                    }
                }
                else
                {
                    //we have no other data than a move to, so the angle is from the origin to the move to.
                    angle = curr.GetAngle(null, Point.Empty, location, AdornmentPlacements.End, this.StartIsReversed);
                }
                
                this.AddVertex(location, angle, inVertices);
            }
            
            


            return cursor;
        }

        protected Point AddEndVertextAndReturnCursor(Path path, Point cursor, List<AdornmentVertex> inVertices)
        {
            var last = path.Operations.Count - 1;
            var curr = path.Operations[last];

            var prev = last > 0 ? path.Operations[last - 1] : null;
            var first = path.Operations[0];
            Double angle;
            Point location;
            
            if (curr.Type == PathDataType.Close)
            {
                location = first.GetLocation(null, AdornmentPlacements.Start);
                angle = GetCombinedAngle(prev, first, location);
            }
            else
            {
                location = curr.GetLocation(prev, AdornmentPlacements.End);
                
                angle = curr.GetEndAngle(prev, location, first) ?? 0.0;
            }

            this.AddVertex(location, angle, inVertices);
            
            return cursor;
        }

        protected double GetCombinedAngle(PathData first, PathData second, Point point)
        {

            var angle1 = first.GetAngle(null, point, point, AdornmentPlacements.End, false);
            var angle2 = second.GetAngle(first, point, point, AdornmentPlacements.Start, false);
            var diff = (angle2 - angle1) / 2.0;
            return angle1 + diff;
        }

        protected virtual void AddVertex(Point pt, double angle, List<AdornmentVertex> toCollection)
        {
            var vertex = new AdornmentVertex(pt, angle);
            toCollection.Add(vertex);
        }
    }

}