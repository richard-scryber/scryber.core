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
    }

    #endregion
}
