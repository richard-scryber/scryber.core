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
using Scryber.PDF.Graphics;
using System.Text;

namespace Scryber.Drawing
{
    /// <summary>
    /// Contains path drawing operations
    /// </summary>
    [PDFParsableValue()]
    public class GraphicsPath : ITypedObject, ICloneable
    {
        
        private List<Path> _paths = new List<Path>();
        private Stack<Path> _stack = new Stack<Path>();
        private PathMultiAdornment _addornments; 
        private Point _cursor;
        private Point _lasthandle;
        private GraphicFillMode _mode = GraphicFillMode.Winding;
        
        //set the top left and bottom right to opposite values.
        private Point _topLeft = new Point(Double.MaxValue, Double.MaxValue);
        private Point _bottomRight = new Point(0.0, 0.0); 

        public GraphicFillMode Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        // public List<Path> Paths
        // {
        //     get { return _paths; }
        // }

        public IEnumerable<Path> SubPaths
        {
            get { return _paths; }
        }

        public Path CurrentPath
        {
            get
            {
                if (_stack.Count == 0)
                    return null;
                else
                    return _stack.Peek();
            }
        }

        public Point Cursor
        {
            get { return _cursor; }
            set { _cursor = value; }
        }

        public Point LastHandle
        {
            get { return _lasthandle; }
            set { _lasthandle = value; }
        }

        /// <summary>
        /// Returns true if this path has an open and current path that can be added to.
        /// </summary>
        public bool HasCurrentPath
        {
            get { return this._stack.Count > 0; }
        }

        public bool HasAdornments
        {
            get { return this._addornments != null; }
        }

        public void OutputAdornments(PDFGraphics graphics, PathAdornmentInfo info, ContextBase context,
            AdornmentOrder currentOrder)
        {
            if (null != this._addornments)
            {
                
                var cursor = info.Location;
                var start = cursor;
                
                foreach (var subPath in this.SubPaths)
                {
                    var count = subPath.Operations.Count;
                    for (var i = 0; i < count; i++)
                    {
                        var op = subPath.Operations[i];
                        var end = cursor;
                        
                        if (i == 0)
                        {
                            //Do nothing except update the cursor
                            cursor = op.GetLocation(start, end, AdornmentPlacements.Start);
                        }
                        else if (i == 1)
                        {
                            info.Location = op.GetLocation(start, end, AdornmentPlacements.Start);
                            info.AngleRadians = op.GetAngle(start, end); //auto-start-reverse
                            cursor = this._addornments.EnsureAdornments(graphics, info, context, currentOrder, AdornmentPlacements.Start);
                        }

                        if (i > 0 && i < count - 1)
                        {
                            // mid point
                            info.Location = op.GetLocation(start, end, AdornmentPlacements.Middle);
                            info.AngleRadians = op.GetAngle(start, end); //auto-start-reverse
                            cursor = this._addornments.EnsureAdornments(graphics, info, context, currentOrder, AdornmentPlacements.Middle);
                        }

                        if (i == count - 1)
                        {
                            //last
                            info.Location = op.GetLocation(start, end, AdornmentPlacements.End);
                            info.AngleRadians = op.GetAngle(start, end); //auto-start-reverse
                            cursor = this._addornments.EnsureAdornments(graphics, info, context, currentOrder, AdornmentPlacements.End);
                        }

                        start = cursor;
                        cursor = end;
                    }
                    
                }
                
            }
        }

        

        /// <summary>
        /// Gets the bounds of this path
        /// </summary>
        public Rect Bounds
        {
            get
            {
                if(this._paths == null || this._paths.Count == 0)
                    return Rect.Empty;
                else if (this._paths.Count == 1 && this._paths[0].Count == 0)
                    return Rect.Empty;
                else
                {
                    var x = _topLeft.X;
                    var y = _topLeft.Y;
                    var w = _bottomRight.X - _topLeft.X;
                    var h = _bottomRight.Y - _topLeft.Y;
                    return new Rect(x, y, w, h);
                }
            }
        }
        
        /// <summary>
        /// Gets or sets any render matrix to be applied to the path data when rendering
        /// </summary>
        public PDFTransformationMatrix PathMatrix { get; set; }

        
        public ObjectType Type
        {
            get { return ObjectTypes.GraphicsPath; }
        }

        /// <summary>
        /// Creates a new empty graphics path, ready to start adding operations to.
        /// </summary>
        public GraphicsPath()
            : this(ObjectTypes.GraphicsPath)
        {

        }

        public void AddAdornment(IPathAdorner adorner, AdornmentOrder order, AdornmentPlacements placements)
        {
            if(null != this._addornments)
                this._addornments.Append(adorner, order, placements);
            else
            {
                this._addornments = new PathMultiAdornment(adorner, order, placements);
            }
                
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        public GraphicsPath Clone()
        {
            var newPath = (GraphicsPath)this.MemberwiseClone();
            
            if (null != this.PathMatrix)
                newPath.PathMatrix = this.PathMatrix.Clone();
            
            if (null != this._paths)
            {
                newPath._paths = new List<Path>(this._paths.Count);
                foreach (var p in this._paths)
                {
                    newPath._paths.Add(p.Clone());
                }
            }

            return newPath;
        }

        /// <summary>
        /// Creates a new empty graphics path, ready to start adding operations to.
        /// </summary>
        protected GraphicsPath(ObjectType type)
        {
            Path p = new Path();
            _paths = new List<Path>();
            _paths.Add(p);

            _stack = new Stack<Path>();
            _stack.Push(p);

        }

        public void MoveTo(Point start)
        {
            PathMoveData move = new PathMoveData();
            move.MoveTo = start;
            CurrentPath.Add(move);
            IncludeInBounds(start);
            Cursor = start;
        }

        public void MoveBy(Point delta)
        {
            PathMoveData move = new PathMoveData();
            Point pos = ConvertDeltaToActual(delta);
            move.MoveTo = pos;
            CurrentPath.Add(move);
            IncludeInBounds(pos);
            Cursor = pos;
        }

        public void LineTo(Point end)
        {
            PathLineData line = new PathLineData();
            line.LineTo = end;
            CurrentPath.Add(line);
            IncludeInBounds(end);
            Cursor = end;
        }

        public void LineFor(Point delta)
        {
            PathLineData line = new PathLineData();
            Point end = ConvertDeltaToActual(delta);
            line.LineTo = end;
            CurrentPath.Add(line);
            IncludeInBounds(end);
            Cursor = end;
        }

        public void VerticalLineTo(Unit y)
        {
            PathLineData line = new PathLineData();
            Point end = new Point(this.Cursor.X, y);
            line.LineTo = end;
            CurrentPath.Add(line);
            IncludeInBounds(end);
            Cursor = end;
        }

        public void VerticalLineFor(Unit dy)
        {
            PathLineData line = new PathLineData();
            Point end = ConvertDeltaToActual(0, dy);
            line.LineTo = end;
            CurrentPath.Add(line);
            IncludeInBounds(end);
            Cursor = end;
        }

        public void HorizontalLineTo(Unit x)
        {
            PathLineData line = new PathLineData();
            Point end = new Point(x, this.Cursor.Y);
            line.LineTo = end;
            CurrentPath.Add(line);
            IncludeInBounds(end);
            Cursor = end;
        }

        public void HorizontalLineFor(Unit dx)
        {
            PathLineData line = new PathLineData();
            Point end = ConvertDeltaToActual(dx, 0);
            line.LineTo = end;
            CurrentPath.Add(line);
            IncludeInBounds(end);
            Cursor = end;
        }

        /// <summary>
        /// Closes the current path (drawing a line from the current point to the start point if the path is not already closed)
        /// And then ends the path if 'end' is true.
        /// </summary>
        public void ClosePath(bool end)
        {
            PathCloseData close = new PathCloseData();
            CurrentPath.Add(close);
            if(end)
                this.EndPath();
        }

        public void QuadraticCurveTo(Point end, Point handle)
        {
            PathQuadraticCurve arc = new PathQuadraticCurve() { EndPoint = end, ControlPoint = handle };

            CurrentPath.Add(arc);
            IncludeInBounds(end);
            IncludeInBounds(handle);
            Cursor = end;
            LastHandle = handle;
        }


        public void QuadraticCurveFor(Point endDelta, Point handleDelta)
        {
            Point end = ConvertDeltaToActual(endDelta);
            Point handle = ConvertDeltaToActual(handleDelta);
            PathQuadraticCurve arc = new PathQuadraticCurve() { EndPoint = end, ControlPoint = handle };

            CurrentPath.Add(arc);
            IncludeInBounds(end);
            IncludeInBounds(handle);
            Cursor = end;
            LastHandle = handle;
        }

        public void SmoothQuadraticCurveFor(Point endDelta)
        {
            Point end = ConvertDeltaToActual(endDelta);

            if (this.LastHandle == Point.Empty)
            {
                this.LineTo(end);
            }
            else
            {
                Point handle = GetReflectedLastHandle();
                this.QuadraticCurveTo(end, handle);
            }
        }

        public void SmoothQuadraticCurveTo(Point end)
        {
            if (this.LastHandle == Point.Empty)
            {
                this.LineTo(end);
            }
            else
            {
                Point handle = GetReflectedLastHandle();
                this.QuadraticCurveTo(end, handle);
            }

        }

        private Point GetReflectedLastHandle()
        {
            Unit handlex = new Unit((Cursor.X.PointsValue - LastHandle.X.PointsValue));
            Unit handley = new Unit((Cursor.Y.PointsValue - LastHandle.Y.PointsValue));
            Point handle = new Point(handlex, handley);
            handle = ConvertDeltaToActual(handle);
            return handle;
        }

        public void CubicCurveTo(Point end, Point handleStart, Point handleEnd)
        {
            PathBezierCurveData arc = new PathBezierCurveData(end, handleStart, handleEnd, true, true);
            CurrentPath.Add(arc);
            IncludeInBounds(end);
            IncludeInBounds(handleStart);
            IncludeInBounds(handleEnd);

            Cursor = end;
            LastHandle = handleEnd;
        }

        public void SmoothCubicCurveTo(Point end, Point handleEnd)
        {
            if (this.LastHandle == Point.Empty)
            {
                CubicCurveToWithHandleEnd(end, handleEnd);
            }
            else
            {
                Point handleStart = GetReflectedLastHandle();
                CubicCurveTo(end, handleStart, handleEnd);
            }
        }

        public void SmoothCubicCurveFor(Point endDelta, Point handleEndDelta)
        {
            Point end = ConvertDeltaToActual(endDelta);
            Point handleEnd = ConvertDeltaToActual(handleEndDelta);

            if (this.LastHandle == Point.Empty)
            {
                CubicCurveToWithHandleEnd(end, handleEnd);
            }
            else
            {
                Point handleStart = GetReflectedLastHandle();
                CubicCurveTo(end, handleStart, handleEnd);
            }
        }

        public void CubicCurveToWithHandleStart(Point end, Point handleStart)
        {
            PathBezierCurveData arc = new PathBezierCurveData(end, handleStart, Point.Empty, true, false);
            CurrentPath.Add(arc);
            IncludeInBounds(end);
            IncludeInBounds(handleStart);

            Cursor = end;
            LastHandle = Point.Empty;
        }

        public void CubicCurveToWithHandleEnd(Point end, Point handleEnd)
        {
            PathBezierCurveData arc = new PathBezierCurveData(end, Point.Empty, handleEnd, false, true);
            CurrentPath.Add(arc);
            IncludeInBounds(end);
            IncludeInBounds(handleEnd);

            Cursor = end;
            LastHandle = handleEnd;
        }


        public void CubicCurveFor(Point delta, Point deltaHandleStart, Point deltaHandleEnd)
        {
            Point end = ConvertDeltaToActual(delta);
            Point handleStart = ConvertDeltaToActual(deltaHandleStart);
            Point handleEnd = ConvertDeltaToActual(deltaHandleEnd);

            PathBezierCurveData arc = new PathBezierCurveData(end, handleStart, handleEnd, true, true);
            CurrentPath.Add(arc);
            IncludeInBounds(end);
            IncludeInBounds(handleStart);
            IncludeInBounds(handleEnd);

            Cursor = end;
            LastHandle = handleEnd;
        }

        public void CubicCurveForWithHandleStart(Point delta, Point deltaHandleStart)
        {
            Point end = ConvertDeltaToActual(delta);
            Point handleStart = ConvertDeltaToActual(deltaHandleStart);
            
            PathBezierCurveData arc = new PathBezierCurveData(end, handleStart, Point.Empty, true, false);
            CurrentPath.Add(arc);
            IncludeInBounds(end);
            IncludeInBounds(handleStart);

            Cursor = end;
            LastHandle = Point.Empty;
        }

        public void CubicCurveForWithHandleEnd(Point delta, Point deltaHandleEnd)
        {
            Point end = ConvertDeltaToActual(delta);
            Point handleEnd = ConvertDeltaToActual(deltaHandleEnd);

            PathBezierCurveData arc = new PathBezierCurveData(end, Point.Empty, handleEnd, false, true);
            CurrentPath.Add(arc);
            IncludeInBounds(end);
            IncludeInBounds(handleEnd);

            Cursor = end;
            LastHandle = handleEnd;
        }



        internal void ArcTo(Unit rx, Unit ry, double ang, PathArcSize size, PathArcSweep sweep, Point end)
        {
            PathArcData arc = new PathArcData() { RadiusX = rx, RadiusY = ry, XAxisRotation = ang, ArcSize = size, ArcSweep = sweep, EndPoint = end };
            CurrentPath.Add(arc);
            
            IEnumerable<PathBezierCurveData> curves = PathDataHelper.GetBezierCurvesForArc(this.Cursor, arc);

            foreach (PathBezierCurveData bez in curves)
            {
                IncludeInBounds(bez.EndPoint);
                if (bez.HasStartHandle)
                    IncludeInBounds(bez.StartHandle);
                if (bez.HasEndHandle)
                    IncludeInBounds(bez.EndHandle);
            }

            Cursor = end;
           
        }

        internal void ArcFor(Unit rx, Unit ry, double ang, PathArcSize size, PathArcSweep sweep, Point enddelta)
        {
            Point end = ConvertDeltaToActual(enddelta);
            PathArcData arc = new PathArcData() { RadiusX = rx, RadiusY = ry, XAxisRotation = ang, ArcSize = size, ArcSweep = sweep, EndPoint = end };
            CurrentPath.Add(arc);

            IEnumerable<PathBezierCurveData> curves = PathDataHelper.GetBezierCurvesForArc(this.Cursor, arc);

            foreach (PathBezierCurveData bez in curves)
            {
                IncludeInBounds(bez.EndPoint);
                if (bez.HasStartHandle)
                    IncludeInBounds(bez.StartHandle);
                if (bez.HasEndHandle)
                    IncludeInBounds(bez.EndHandle);
            }

            Cursor = end;
        }

        /// <summary>
        /// Ends the current path
        /// </summary>
        public void EndPath()
        {
            if (_stack.Count > 0)
            {
                _stack.Pop();
            }
        }

        /// <summary>
        /// Starts a new path
        /// </summary>
        public void BeginPath()
        {
            Path p = new Path();
            _stack.Push(p);
            _paths.Add(p);
        }

        private void IncludeInBounds(Point pt)
        {
            if (pt.X < _topLeft.X)
                _topLeft.X = pt.X;
            if (pt.Y < _topLeft.Y)
                _topLeft.Y = pt.Y;
            if (pt.X > _bottomRight.X)
                _bottomRight.X = pt.X;
            if (pt.Y > _bottomRight.Y)
                _bottomRight.Y = pt.Y;
        }

        private Point ConvertDeltaToActual(Point delta)
        {
            return new Point(this.Cursor.X + delta.X, this.Cursor.Y + delta.Y);
        }

        private Point ConvertDeltaToActual(Unit dx, Unit dy)
        {
            return new Point(this.Cursor.X + dx, this.Cursor.Y + dy);
        }

        public Point[] GetAllPoints()
        {
            List<Point> pts = new List<Point>();
            foreach (Path path in this.SubPaths)
            {
                path.FillAllPoints(pts);
            }
            return pts.ToArray();
        }

        public static GraphicsPath Parse(string value)
        {
            GraphicsPath path = new GraphicsPath();
            if (string.IsNullOrEmpty(value))
                return path;

            var parser = new Scryber.Svg.SVGPathDataParser(true, null);
            parser.ParseSVG(path, value);
            
            return path;
        }

        

    }
}
