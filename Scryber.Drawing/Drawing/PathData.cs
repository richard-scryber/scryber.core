/*  Copyright 2012 PerceiveIT Limited
 *  This file is part of the Scryber library.
 *
 *  You can redistribute Scryber and/or modify 
 *  it under the terms of the GNU Lesser General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 * 
 *  Scryber is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 * 
 *  You should have received a copy of the GNU Lesser General Public License
 *  along with Scryber source code in the COPYING.txt file.  If not, see <http://www.gnu.org/licenses/>.
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Scryber.Drawing
{
    #region public abstract class PathData

    /// <summary>
    /// Base class for each of the path information parts that make up a complex graphics path.
    /// </summary>
    public abstract class PathData
    {
        private PathDataType _type;

        /// <summary>
        /// The type of path operation this represents.
        /// </summary>
        public PathDataType Type { get { return _type; } }

        public PathData(PathDataType type)
        {
            this._type = type;
        }

        public abstract void FillAllPoints(List<Point> points);

        public virtual PathData Clone()
        {
            return (PathData)this.MemberwiseClone();
        }

        public virtual bool UpdateAdornmentInfo(PathData previous, PathData next, PathAdornmentInfo info,
            AdornmentPlacements placement)
        {

            info.Location = Point.Empty;
            Point start, end;
            double angle;
            if (placement == AdornmentPlacements.Start)
            {

                    start = previous.GetLocation(null, AdornmentPlacements.End);
                    if (info.ExplicitAngle.HasValue)
                    {
                        end = Point.Empty;
                        angle = info.ExplicitAngle.Value;
                    }
                    else
                    {
                        end = this.GetLocation(previous, AdornmentPlacements.Start);
                        angle = this.GetAngle(previous, start, end, AdornmentPlacements.Start, info.ReverseAngleAtStart);
                    }
                
                
            }
            else
            {

                start = previous?.GetLocation(previous, AdornmentPlacements.End) ?? Point.Empty;
                
                //if we are a close then get the next start location, otherwize our end location.
                if (this.Type == PathDataType.Close)
                {
                    if (null == next)
                        end = Point.Empty;
                    else
                        end = next.GetLocation(this, AdornmentPlacements.Start);
                }
                else
                {
                    end = this.GetLocation(previous, AdornmentPlacements.End);
                }
                

                if (info.ExplicitAngle.HasValue)
                    angle = info.ExplicitAngle.Value;
                else
                    angle = this.GetAngle(previous, start, end, placement, false);
            }

            if (placement == AdornmentPlacements.End)
                info.Location = end;
            else
                info.Location = start;
            
            info.AngleRadians = angle;
            return true;
        }

        public virtual Point GetLocation(PathData previous, AdornmentPlacements placement)
        {
            return Point.Empty;
        }

        public virtual double? GetStartAngle(PathData previous, Point start, bool reversed = false)
        {
            var end = this.GetLocation(previous, AdornmentPlacements.End);
            return this.GetAngle(previous, start, end, AdornmentPlacements.Start, reversed);
        }

        public virtual double? GetEndAngle(PathData prev, Point end, PathData next, bool reversed = false)
        {
            var start = prev.GetLocation(null, AdornmentPlacements.End);
            return GetAngle(prev, start, end, AdornmentPlacements.End, reversed);
        }
        
        

        public virtual double GetAngle(PathData previous, Point start, Point end, AdornmentPlacements placement, bool reverseAngle)
        {
            var a = end.X.PointsValue - start.X.PointsValue;
            var o = end.Y.PointsValue - start.Y.PointsValue;
            double angle;

            if (a == 0.0 && o == 0.0)
                angle = 0.0;
            
            else if (placement == AdornmentPlacements.Start)
            {
                angle = Math.Atan(o / a);

                if (a < 0)
                {
                    if (reverseAngle)
                        angle = angle + (2 * Math.PI);
                    else
                        angle = angle + Math.PI;
                }
                else if(reverseAngle)
                {
                    angle = Math.PI + angle;
                }

            }
            else
            {
                angle = Math.Atan(o / a);
                
                if (a < 0)
                {
                    angle = angle + Math.PI;
                }
            }
            
            return angle;
        }
    }

    #endregion

    #region public class PathMoveData : PathData

    public class PathMoveData : PathData
    {
        public Point MoveTo { get; set; }

        public PathMoveData() : base(PathDataType.Move) { }

        public override void FillAllPoints(List<Point> points)
        {
            points.Add(this.MoveTo);
        }
        

        public override Point GetLocation(PathData previous, AdornmentPlacements placement)
        {
            return this.MoveTo;
        }

        public override bool UpdateAdornmentInfo(PathData previous, PathData next, PathAdornmentInfo info, AdornmentPlacements placement)
        {
            return base.UpdateAdornmentInfo(previous, next, info, placement);
        }
    }

    #endregion

    #region public class PathSubPathData : PathData

    public class PathSubPathData : PathData
    {
        public Path InnerPath { get; set; }

        public PathSubPathData() : base(PathDataType.SubPath) { }

        public override void FillAllPoints(List<Point> points)
        {
            if (null != this.InnerPath)
                this.InnerPath.FillAllPoints(points);
        }

        public override PathData Clone()
        {
            
            var sub = (PathSubPathData)base.Clone();
            
            if (null != this.InnerPath)
                sub.InnerPath = this.InnerPath.Clone();
            
            return sub;
        }
    }

    #endregion

    #region public class PathLineData : PathData

    public class PathLineData : PathData
    {
        public Point LineTo { get; set; }

        public PathLineData() : base(PathDataType.Line) { }

        public override void FillAllPoints(List<Point> points)
        {
            points.Add(LineTo);
        }

        

        public override Point GetLocation(PathData previous, AdornmentPlacements placement)
        {
            return this.LineTo;
        }

        public override double? GetStartAngle(PathData previous, Point start, bool reversed = false)
        {
            var end = this.LineTo;
            var angle = this.GetAngle(previous, start, end, AdornmentPlacements.Start, reversed);
            return angle;
        }

        public override bool UpdateAdornmentInfo(PathData previous, PathData next, PathAdornmentInfo info, AdornmentPlacements placement)
        {
            if (placement == AdornmentPlacements.Middle && previous.Type == PathDataType.Move)
            {
                var start = previous.GetLocation(null, AdornmentPlacements.End);
                var end = this.GetLocation(null, AdornmentPlacements.End);
                var angle = info.ExplicitAngle ?? previous.GetAngle(previous, start, end, AdornmentPlacements.End, false);

                info.Location = start;
                info.AngleRadians = angle;
                return true;
            }
            else
            {
                return base.UpdateAdornmentInfo(previous, next, info, placement);
            }
        }
    }

    #endregion

    #region public class PathRectData : PathData

    public class PathRectData : PathData
    {
        public Rect Rect { get; set; }

        public PathRectData() : base(PathDataType.Rect) { }

        public override void FillAllPoints(List<Point> points)
        {
            points.Add(this.Rect.Location);
            points.Add(new Point(this.Rect.X + this.Rect.Width, this.Rect.Y));
            points.Add(new Point(this.Rect.X + this.Rect.Width, this.Rect.Y + this.Rect.Height));
            points.Add(new Point(this.Rect.X, this.Rect.Y + this.Rect.Height));
        }
    }

    #endregion

    #region public class PathBezierCurveData : PathData

    public class PathBezierCurveData : PathData
    {
        public Point[] Points { get; private set; }

        public Point EndPoint { get { return this.Points[0]; } }
        public Point StartHandle { get { return this.Points[1]; } }
        public Point EndHandle { get { return this.Points[2]; } }

        public bool HasStartHandle { get; private set; }

        public bool HasEndHandle { get; private set; }

        public PathBezierCurveData(Point end, Point startHandle, Point endHandle, bool hasStart, bool hasEnd) : base(PathDataType.Bezier) 
        {
            this.Points = new Point[] { end, startHandle, endHandle };
            this.HasEndHandle = hasEnd;
            this.HasStartHandle = hasStart;
        }

        public override void FillAllPoints(List<Point> points)
        {
            points.AddRange(this.Points);
        }
        
        public override double? GetStartAngle(PathData previous, Point start, bool reversed = false)
        {
            var angle = this.GetAngle(previous, start, this.StartHandle, AdornmentPlacements.Start, reversed);
            return angle;
        }
        
        
        public override double? GetEndAngle(PathData prev, Point end, PathData next, bool reversed = false)
        {
            
            var start  = this.HasEndHandle ? this.EndHandle : prev.GetLocation(null, AdornmentPlacements.End);
            return GetAngle(prev, start, end, AdornmentPlacements.End, reversed);
        }

        public override Point GetLocation(PathData previous, AdornmentPlacements placement)
        {
            if (placement == AdornmentPlacements.End)
                return this.EndPoint;
            else
                return base.GetLocation(previous, placement);
        }

        public override bool UpdateAdornmentInfo(PathData previous, PathData next, PathAdornmentInfo info, AdornmentPlacements placement)
        {
            
            info.Location = Point.Empty;

            Point start;
            Point end;
            double angle;
            if (placement == AdornmentPlacements.Start)
            {
                start = previous?.GetLocation(previous, AdornmentPlacements.End) ?? Point.Empty;
                if (this.HasStartHandle)
                {
                    end = this.StartHandle;
                }
                else if (this.HasEndHandle)
                {
                    end = this.EndHandle;
                }
                else
                {
                    end = this.EndPoint;
                }

                if (info.ExplicitAngle.HasValue)
                    angle = info.ExplicitAngle.Value;
                else
                    angle = this.GetAngle(previous, start, end, placement, info.ReverseAngleAtStart);
            }
            else
            {
                end = this.EndPoint;
                
                if (this.HasEndHandle)
                {
                    start = this.EndHandle;
                }
                else if (this.HasStartHandle)
                {
                    start = this.StartHandle;
                }
                else
                {
                    start = previous?.GetLocation(previous, AdornmentPlacements.End) ?? Point.Empty;
                }

                if (info.ExplicitAngle.HasValue)
                    angle = info.ExplicitAngle.Value;
                else
                    angle = this.GetAngle(previous, start, end, placement, false);
            }



            if (placement == AdornmentPlacements.Start)
            {
                info.Location = start;
            }
            else
            {
                info.Location = end;
            }
            info.AngleRadians = angle;
            
            return true;
        }
    }

    #endregion

    #region public class PathArcData : PathData

    /// <summary>
    /// Describes an eliptical arc based on the path current position, x and y radii, and the end point. 
    /// Also allows for rotation, sweep direction and large or small paths
    /// </summary>
    public class PathArcData : PathData
    {
        /// <summary>
        /// Gets or sets the X Radius of an the ellipse that describes this arc.
        /// </summary>
        public Unit RadiusX { get; set; }

        /// <summary>
        /// Gets or Sets the Y Radius of an ellipse that describes this arc
        /// </summary>
        public Unit RadiusY { get; set; }

        /// <summary>
        /// Gets or sets the rotation angle of the ellipse that describes the arc.
        /// </summary>
        public Unit XAxisRotation { get; set; }

        /// <summary>
        /// Gets or sets the Arc size option within the ellipse - large 
        /// </summary>
        public PathArcSize ArcSize { get; set; }

        /// <summary>
        /// Gets or sets the sweep direction of the arc along the ellipse path
        /// </summary>
        public PathArcSweep ArcSweep { get; set; }

        /// <summary>
        /// Gets or sets the end point of the arc on the ellipse circumference.
        /// </summary>
        public Point EndPoint { get; set; }


        public PathArcData()
            : base(PathDataType.Arc)
        {
        }

        public override void FillAllPoints(List<Point> points)
        {
            points.Add(this.EndPoint);
        }

        public override double? GetStartAngle(PathData previous, Point start, bool reversed = false)
        {
            var quad = PathDataHelper.GetBezierCurvesForArc(start, this).First();
            var angle = GetAngle(previous, start, quad.StartHandle, AdornmentPlacements.Start, reversed);
            
            return angle;
        }

        public override bool UpdateAdornmentInfo(PathData previous, PathData next, PathAdornmentInfo info, AdornmentPlacements placement)
        {
            
            info.Location = Point.Empty;
            Point start = previous == null ? Point.Empty : previous.GetLocation(null, AdornmentPlacements.End);
            Point end;
            double angle;

            PathBezierCurveData[] myquads;
            
            if (placement == AdornmentPlacements.Start)
            {
                if (info.ExplicitAngle.HasValue)
                    angle = info.ExplicitAngle.Value;
                else
                {
                    var quad = PathDataHelper.GetBezierCurvesForArc(start, this).First();
                    end = quad.StartHandle;
                    angle = GetAngle(previous, start, end, placement, info.ReverseAngleAtStart);
                }

                info.Location = start;
                info.AngleRadians = angle;
                return true;
            }
            else if(placement == AdornmentPlacements.End)
            {
                var last = PathDataHelper.GetBezierCurvesForArc(start, this).Last();
                end = last.EndPoint;
                start = last.EndHandle;
                
                if (info.ExplicitAngle.HasValue)
                    angle = info.ExplicitAngle.Value;
                else
                    angle = GetAngle(previous, start, end, placement, false);

                info.Location = end;
                info.AngleRadians = angle;
                return true;
            }
            else
            {
                var all = PathDataHelper.GetBezierCurvesForArc(start, this);
                var last = all.Last();
                end = last.EndPoint;
                start = last.EndHandle;
                
                if (info.ExplicitAngle.HasValue)
                    angle = info.ExplicitAngle.Value;
                else
                {
                    angle = GetAngle(previous, start, end, placement, false);
                }

                info.Location = end;
                info.AngleRadians = angle;
                if (null != next)
                {
                    var nextAngle = next.GetStartAngle(this, end);
                    
                    if (nextAngle.HasValue)
                    {
                        var diff = info.AngleRadians - nextAngle.Value;
                        diff /= 2;
                        info.AngleRadians -= diff;
                    }
                }

                return true;
            }
            
        }
        
    }

    #endregion

    #region public class PathQuadraticCurve : PathData

    /// <summary>
    /// Defines a quadratic curve (one control point) from the current position to the End Point.
    /// </summary>
    public class PathQuadraticCurve : PathData
    {
        /// <summary>
        /// Gets the control point for this quadratic curve
        /// </summary>
        public Point ControlPoint { get; set; }

        /// <summary>
        /// Gets or sets the end point for the quadratic curve
        /// </summary>
        public Point EndPoint { get; set; }

        public PathQuadraticCurve()
            : base(PathDataType.Quadratic)
        {
        }

        public override void FillAllPoints(List<Point> points)
        {
            points.Add(this.ControlPoint);
            points.Add(this.EndPoint);
        }

        public override bool UpdateAdornmentInfo(PathData previous, PathData next, PathAdornmentInfo info, AdornmentPlacements placement)
        {
            Point start = previous == null ? Point.Empty : previous.GetLocation(null, AdornmentPlacements.End);
            Point end;
            double angle;
            
            if (placement == AdornmentPlacements.Middle)
            {
                var last = PathDataHelper.GetBezierCurvesForQuadratic(start, this).Last();
                end = last.EndPoint;
                start = last.EndHandle;
                angle = this.GetAngle(previous, start, end, AdornmentPlacements.Middle, false);

                info.AngleRadians = angle;
                info.Location = end;
                
                if (null != next)
                {
                    var nextAngle = next.GetStartAngle(this, end);
                    
                    if (nextAngle.HasValue)
                    {
                        var diff = info.AngleRadians - nextAngle.Value;
                        diff /= 2;
                        info.AngleRadians -= diff;
                    }
                }
                
                return true;
            }
            else if (placement == AdornmentPlacements.End)
            {
                var quads = PathDataHelper.GetBezierCurvesForQuadratic(start, this).ToArray(); 
                var last = quads[quads.Length - 1];
                end = last.EndPoint;
                start = last.EndHandle;
                angle = this.GetAngle(previous, start, end, AdornmentPlacements.End, false);

                info.AngleRadians = angle;
                info.Location = end;
                
                if (null != next)
                {
                    var nextAngle = next.GetStartAngle(this, end);
                    
                    if (nextAngle.HasValue)
                    {
                        var diff = info.AngleRadians - nextAngle.Value;
                        diff /= 2;
                        info.AngleRadians -= diff;
                    }
                }
                
                return true;
            }
            else
            {
                var result = base.UpdateAdornmentInfo(previous, next, info, placement);
                return result;
            }
        }
        
        public override double? GetStartAngle(PathData previous, Point start, bool reversed = false)
        {
            var quad = PathDataHelper.GetBezierCurvesForQuadratic(start, this).First();
            var angle = GetAngle(previous, start, quad.StartHandle, AdornmentPlacements.Start, reversed);
            
            return angle;
        }

        public override Point GetLocation(PathData previous, AdornmentPlacements placement)
        {
            if (placement == AdornmentPlacements.End)
            {
                return this.EndPoint;
            }
            else
            {
                return base.GetLocation(previous, placement);
            }
        }
    }

    #endregion

    #region public class PathCloseData : PathData

    public class PathCloseData : PathData
    {
        public PathCloseData()
            : base(PathDataType.Close)
        {
        }

        public override void FillAllPoints(List<Point> points)
        {
            
        }
        
        public override bool UpdateAdornmentInfo(PathData previous, PathData next, PathAdornmentInfo info,
            AdornmentPlacements placement)
        {

            info.Location = Point.Empty;
            Point start, end;
            double angle;
            if (placement == AdornmentPlacements.Start)
            {

                    start = previous.GetLocation(null, AdornmentPlacements.End);
                    if (info.ExplicitAngle.HasValue)
                    {
                        end = Point.Empty;
                        angle = info.ExplicitAngle.Value;
                    }
                    else
                    {
                        end = this.GetLocation(previous, AdornmentPlacements.Start);
                        angle = this.GetAngle(previous, start, end, AdornmentPlacements.Start, info.ReverseAngleAtStart);
                    }
                    info.Location = start;
            
                    info.AngleRadians = angle;
                    return true;
                
            }
            else
            {
                

                start = previous?.GetLocation(previous, AdornmentPlacements.End) ?? Point.Empty;
                
                //if we are a close then get the next start location, otherwize our end location.
                if (null == next)
                    end = Point.Empty;
                else
                    end = next.GetLocation(this, AdornmentPlacements.Start);
                
                
                

                if (info.ExplicitAngle.HasValue)
                    angle = info.ExplicitAngle.Value;
                else if (null != previous)
                {
                    if (start == end) //our close meets at the same point so we go back to the previous path data completely.
                    {
                        return previous.UpdateAdornmentInfo(null, this, info, AdornmentPlacements.End);
                    }
                    else
                    {
                        angle = previous.GetAngle(previous, start, end, placement, info.ReverseAngleAtStart);
                    }
                }
                else
                {
                    angle = 0.0;
                }

            
                
                info.Location = end;
            
                info.AngleRadians = angle;
                return true;
            }
                
        }

        public override Point GetLocation(PathData previous, AdornmentPlacements placement)
        {
            if (placement == AdornmentPlacements.Start)
                return base.GetLocation(previous, placement);
            else if(null != previous)
            {
                return previous.GetLocation(null, AdornmentPlacements.End);
            }
            else
            {
                return Point.Empty;
            }
        }
    }
    

    #endregion
}
