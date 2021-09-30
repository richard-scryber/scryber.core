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
    /// <summary>
    /// Contains path drawing operations
    /// </summary>
    [PDFParsableValue()]
    public class PDFGraphicsPath : PDFObject
    {
        
        private List<Path> _paths = new List<Path>();
        private Stack<Path> _stack = new Stack<Path>();
        private PDFRect _bounds;
        private PDFPoint _cursor;
        private PDFPoint _lasthandle;
        private GraphicFillMode _mode = GraphicFillMode.Winding;

        public GraphicFillMode Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        public List<Path> Paths
        {
            get { return _paths; }
        }

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

        public PDFPoint Cursor
        {
            get { return _cursor; }
            set { _cursor = value; }
        }

        public PDFPoint LastHandle
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

        /// <summary>
        /// Gets the bounds of this path
        /// </summary>
        public PDFRect Bounds
        {
            get { return _bounds; }
        }

        /// <summary>
        /// Creates a new empty graphics path, ready to start adding operations to.
        /// </summary>
        public PDFGraphicsPath()
            : this(PDFObjectTypes.GraphicsPath)
        {

        }

        /// <summary>
        /// Creates a new empty graphics path, ready to start adding operations to.
        /// </summary>
        protected PDFGraphicsPath(ObjectType type)
            : base(type)
        {
            Path p = new Path();
            _paths = new List<Path>();
            _paths.Add(p);

            _stack = new Stack<Path>();
            _stack.Push(p);
            _bounds = PDFRect.Empty;
        }

        public void MoveTo(PDFPoint start)
        {
            PathMoveData move = new PathMoveData();
            move.MoveTo = start;
            CurrentPath.Add(move);
            IncludeInBounds(start);
            Cursor = start;
        }

        public void MoveBy(PDFPoint delta)
        {
            PathMoveData move = new PathMoveData();
            PDFPoint pos = ConvertDeltaToActual(delta);
            move.MoveTo = pos;
            CurrentPath.Add(move);
            IncludeInBounds(pos);
            Cursor = pos;
        }

        public void LineTo(PDFPoint end)
        {
            PathLineData line = new PathLineData();
            line.LineTo = end;
            CurrentPath.Add(line);
            IncludeInBounds(end);
            Cursor = end;
        }

        public void LineFor(PDFPoint delta)
        {
            PathLineData line = new PathLineData();
            PDFPoint end = ConvertDeltaToActual(delta);
            line.LineTo = end;
            CurrentPath.Add(line);
            IncludeInBounds(end);
            Cursor = end;
        }

        public void VerticalLineTo(PDFUnit y)
        {
            PathLineData line = new PathLineData();
            PDFPoint end = new PDFPoint(this.Cursor.X, y);
            line.LineTo = end;
            CurrentPath.Add(line);
            IncludeInBounds(end);
            Cursor = end;
        }

        public void VerticalLineFor(PDFUnit dy)
        {
            PathLineData line = new PathLineData();
            PDFPoint end = ConvertDeltaToActual(0, dy);
            line.LineTo = end;
            CurrentPath.Add(line);
            IncludeInBounds(end);
            Cursor = end;
        }

        public void HorizontalLineTo(PDFUnit x)
        {
            PathLineData line = new PathLineData();
            PDFPoint end = new PDFPoint(x, this.Cursor.Y);
            line.LineTo = end;
            CurrentPath.Add(line);
            IncludeInBounds(end);
            Cursor = end;
        }

        public void HorizontalLineFor(PDFUnit dx)
        {
            PathLineData line = new PathLineData();
            PDFPoint end = ConvertDeltaToActual(dx, 0);
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

        public void QuadraticCurveTo(PDFPoint end, PDFPoint handle)
        {
            PathQuadraticCurve arc = new PathQuadraticCurve() { EndPoint = end, ControlPoint = handle };

            CurrentPath.Add(arc);
            IncludeInBounds(end);
            IncludeInBounds(handle);
            Cursor = end;
            LastHandle = handle;
        }


        public void QuadraticCurveFor(PDFPoint endDelta, PDFPoint handleDelta)
        {
            PDFPoint end = ConvertDeltaToActual(endDelta);
            PDFPoint handle = ConvertDeltaToActual(handleDelta);
            PathQuadraticCurve arc = new PathQuadraticCurve() { EndPoint = end, ControlPoint = handle };

            CurrentPath.Add(arc);
            IncludeInBounds(end);
            IncludeInBounds(handle);
            Cursor = end;
            LastHandle = handle;
        }

        public void SmoothQuadraticCurveFor(PDFPoint endDelta)
        {
            PDFPoint end = ConvertDeltaToActual(endDelta);

            if (this.LastHandle == PDFPoint.Empty)
            {
                this.LineTo(end);
            }
            else
            {
                PDFPoint handle = GetReflectedLastHandle();
                this.QuadraticCurveTo(end, handle);
            }
        }

        public void SmoothQuadraticCurveTo(PDFPoint end)
        {
            if (this.LastHandle == PDFPoint.Empty)
            {
                this.LineTo(end);
            }
            else
            {
                PDFPoint handle = GetReflectedLastHandle();
                this.QuadraticCurveTo(end, handle);
            }

        }

        private PDFPoint GetReflectedLastHandle()
        {
            PDFUnit handlex = new PDFUnit((Cursor.X.PointsValue - LastHandle.X.PointsValue));
            PDFUnit handley = new PDFUnit((Cursor.Y.PointsValue - LastHandle.Y.PointsValue));
            PDFPoint handle = new PDFPoint(handlex, handley);
            handle = ConvertDeltaToActual(handle);
            return handle;
        }

        public void CubicCurveTo(PDFPoint end, PDFPoint handleStart, PDFPoint handleEnd)
        {
            PathBezierCurveData arc = new PathBezierCurveData(end, handleStart, handleEnd, true, true);
            CurrentPath.Add(arc);
            IncludeInBounds(end);
            IncludeInBounds(handleStart);
            IncludeInBounds(handleEnd);

            Cursor = end;
            LastHandle = handleEnd;
        }

        public void SmoothCubicCurveTo(PDFPoint end, PDFPoint handleEnd)
        {
            if (this.LastHandle == PDFPoint.Empty)
            {
                CubicCurveToWithHandleEnd(end, handleEnd);
            }
            else
            {
                PDFPoint handleStart = GetReflectedLastHandle();
                CubicCurveTo(end, handleStart, handleEnd);
            }
        }

        public void SmoothCubicCurveFor(PDFPoint endDelta, PDFPoint handleEndDelta)
        {
            PDFPoint end = ConvertDeltaToActual(endDelta);
            PDFPoint handleEnd = ConvertDeltaToActual(handleEndDelta);

            if (this.LastHandle == PDFPoint.Empty)
            {
                CubicCurveToWithHandleEnd(end, handleEnd);
            }
            else
            {
                PDFPoint handleStart = GetReflectedLastHandle();
                CubicCurveTo(end, handleStart, handleEnd);
            }
        }

        public void CubicCurveToWithHandleStart(PDFPoint end, PDFPoint handleStart)
        {
            PathBezierCurveData arc = new PathBezierCurveData(end, handleStart, PDFPoint.Empty, true, false);
            CurrentPath.Add(arc);
            IncludeInBounds(end);
            IncludeInBounds(handleStart);

            Cursor = end;
            LastHandle = PDFPoint.Empty;
        }

        public void CubicCurveToWithHandleEnd(PDFPoint end, PDFPoint handleEnd)
        {
            PathBezierCurveData arc = new PathBezierCurveData(end, PDFPoint.Empty, handleEnd, false, true);
            CurrentPath.Add(arc);
            IncludeInBounds(end);
            IncludeInBounds(handleEnd);

            Cursor = end;
            LastHandle = handleEnd;
        }


        public void CubicCurveFor(PDFPoint delta, PDFPoint deltaHandleStart, PDFPoint deltaHandleEnd)
        {
            PDFPoint end = ConvertDeltaToActual(delta);
            PDFPoint handleStart = ConvertDeltaToActual(deltaHandleStart);
            PDFPoint handleEnd = ConvertDeltaToActual(deltaHandleEnd);

            PathBezierCurveData arc = new PathBezierCurveData(end, handleStart, handleEnd, true, true);
            CurrentPath.Add(arc);
            IncludeInBounds(end);
            IncludeInBounds(handleStart);
            IncludeInBounds(handleEnd);

            Cursor = end;
            LastHandle = handleEnd;
        }

        public void CubicCurveForWithHandleStart(PDFPoint delta, PDFPoint deltaHandleStart)
        {
            PDFPoint end = ConvertDeltaToActual(delta);
            PDFPoint handleStart = ConvertDeltaToActual(deltaHandleStart);
            
            PathBezierCurveData arc = new PathBezierCurveData(end, handleStart, PDFPoint.Empty, true, false);
            CurrentPath.Add(arc);
            IncludeInBounds(end);
            IncludeInBounds(handleStart);

            Cursor = end;
            LastHandle = PDFPoint.Empty;
        }

        public void CubicCurveForWithHandleEnd(PDFPoint delta, PDFPoint deltaHandleEnd)
        {
            PDFPoint end = ConvertDeltaToActual(delta);
            PDFPoint handleEnd = ConvertDeltaToActual(deltaHandleEnd);

            PathBezierCurveData arc = new PathBezierCurveData(end, PDFPoint.Empty, handleEnd, false, true);
            CurrentPath.Add(arc);
            IncludeInBounds(end);
            IncludeInBounds(handleEnd);

            Cursor = end;
            LastHandle = handleEnd;
        }



        internal void ArcTo(PDFUnit rx, PDFUnit ry, double ang, PathArcSize size, PathArcSweep sweep, PDFPoint end)
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

        internal void ArcFor(PDFUnit rx, PDFUnit ry, double ang, PathArcSize size, PathArcSweep sweep, PDFPoint enddelta)
        {
            PDFPoint end = ConvertDeltaToActual(enddelta);
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

        private void IncludeInBounds(PDFPoint pt)
        {
            if (_bounds.Width < pt.X)
                _bounds.Width = pt.X;
            if (_bounds.Height < pt.Y)
                _bounds.Height = pt.Y;
        }

        private PDFPoint ConvertDeltaToActual(PDFPoint delta)
        {
            return new PDFPoint(this.Cursor.X + delta.X, this.Cursor.Y + delta.Y);
        }

        private PDFPoint ConvertDeltaToActual(PDFUnit dx, PDFUnit dy)
        {
            return new PDFPoint(this.Cursor.X + dx, this.Cursor.Y + dy);
        }

        public PDFPoint[] GetAllPoints()
        {
            List<PDFPoint> pts = new List<PDFPoint>();
            foreach (Path path in this.Paths)
            {
                path.FillAllPoints(pts);
            }
            return pts.ToArray();
        }

        public static PDFGraphicsPath Parse(string value)
        {
            PDFGraphicsPath path = new PDFGraphicsPath();
            if (string.IsNullOrEmpty(value))
                return path;

            PDFSVGPathDataParser parser = new PDFSVGPathDataParser(true, null);
            parser.ParseSVG(path, value);
            
            return path;
        }

        

    }
}
