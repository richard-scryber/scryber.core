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
using Scryber.PDF;
using Scryber.PDF.Native;
using Scryber.PDF.Resources;
using Scryber.Drawing;

namespace Scryber.PDF.Graphics
{
    public partial class PDFGraphics
    {
        
        public void DrawElipse(PDFPen pen, Rect rect)
        {
            this.DrawElipse(pen, rect.X, rect.Y, rect.Width, rect.Height);
        }

        public void DrawElipse(PDFPen pen, Point pos, Size size)
        {
            this.DrawElipse(pen, pos.X, pos.Y, size.Width, size.Height);
        }

        public void DrawElipse(PDFPen pen, Unit x, Unit y, Unit width, Unit height)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            Rect bounds = new Rect(x, y, width, height);

            this.SaveGraphicsState();
            pen.SetUpGraphics(this, bounds);

            OutputElipsePoints(x, y, width, height);
            this.RenderCloseStrokePathOp();

            pen.ReleaseGraphics(this, bounds);

            this.RestoreGraphicsState();
            
        }

        public void FillElipse(PDFBrush brush, Rect rect)
        {
            this.FillElipse(brush, rect.Location, rect.Size);
        }

        public void FillElipse(PDFBrush brush, Point pos, Size size)
        {
            this.FillElipse(brush, pos.X, pos.Y, size.Width, size.Height);
        }

        public void FillElipse(PDFBrush brush, Unit x, Unit y, Unit width, Unit height)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");

            Rect bounds = new Rect(x, y, width, height);

            this.SaveGraphicsState();
            brush.SetUpGraphics(this, bounds);

            OutputElipsePoints(x, y, width, height);
            this.RenderFillPathOp();

            brush.ReleaseGraphics(this, bounds);

            this.RestoreGraphicsState();

        }

        private void OutputElipsePoints(Unit x, Unit y, Unit width, Unit height)
        {
            PDFReal left = x.RealValue;
            PDFReal right = (x + width).RealValue;
            PDFReal top = y.RealValue;
            PDFReal bottom = (y + height).RealValue;
            PDFReal hcentre = (left + (width.RealValue / (PDFReal)2.0));
            PDFReal vcentre = (top + (height.RealValue / (PDFReal)2.0));
            PDFReal xhandle = ((width.RealValue / (PDFReal)2.0) * (PDFReal)CircularityFactor);
            PDFReal yhandle = ((height.RealValue / (PDFReal)2.0) * (PDFReal)CircularityFactor);

            this.RenderMoveTo(left, vcentre);
            //top left quadrant
            this.RenderBezierCurveTo(hcentre, top, left, vcentre - yhandle, hcentre - xhandle, top);
            //top right quadrant
            this.RenderBezierCurveTo(right, vcentre, hcentre + xhandle, top, right, vcentre - yhandle);
            //bottom right quadrant
            this.RenderBezierCurveTo(hcentre, bottom, right, vcentre + yhandle, hcentre + xhandle, bottom);
            //bottom left quadrant
            this.RenderBezierCurveTo(left, vcentre, hcentre - xhandle, bottom, left, vcentre + yhandle);
        }

        public void DrawQuadrants(PDFPen pen, Rect rect, Quadrants sides)
        {
            this.DrawQuadrants(pen, rect.Location, rect.Size, sides);
        }

        public void DrawQuadrants(PDFPen pen, Point pos, Size size, Quadrants sides)
        {
            this.DrawQuadrants(pen, pos.X, pos.Y, size.Width, size.Height, sides);
        }

        public void DrawQuadrants(PDFPen pen, Unit x, Unit y, Unit width, Unit height, Quadrants sides)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");

            Rect bounds = new Rect(x, y, width, height);

            this.SaveGraphicsState();
            pen.SetUpGraphics(this, bounds);

            OutputQuadrantPoints(x, y, width, height, sides);
            this.RenderStrokePathOp();

            pen.ReleaseGraphics(this, bounds);

            this.RestoreGraphicsState();
        }

        private void OutputQuadrantPoints(Unit x, Unit y, Unit width, Unit height, Quadrants sides)
        {
            PDFReal left = x.RealValue;
            PDFReal right = (x + width).RealValue;
            PDFReal top = y.RealValue;
            PDFReal bottom = (y + height).RealValue;
            PDFReal hcentre = (left + (width.RealValue / (PDFReal)2.0));
            PDFReal vcentre = (top + (height.RealValue / (PDFReal)2.0));
            PDFReal xhandle = ((width.RealValue / (PDFReal)2.0) * (PDFReal)CircularityFactor);
            PDFReal yhandle = ((height.RealValue / (PDFReal)2.0) * (PDFReal)CircularityFactor);

            bool requiresmove;
            if ((sides & Quadrants.TopLeft) > 0)
            {
                this.RenderMoveTo(left, vcentre);
                //top left quadrant
                this.RenderBezierCurveTo(hcentre, top, left, vcentre - yhandle, hcentre - xhandle, top);
                requiresmove = false;
            }
            else
                requiresmove = true;

            if ((sides & Quadrants.TopRight) > 0)
            {
                if (requiresmove)
                    this.RenderMoveTo(hcentre, top);
                //top right quadrant
                this.RenderBezierCurveTo(right, vcentre, hcentre + xhandle, top, right, vcentre - yhandle);
                requiresmove = false;
            }
            else
                requiresmove = true;

            if ((sides & Quadrants.BottomRight) > 0)
            {
                if (requiresmove)
                    this.RenderMoveTo(right, vcentre);
                //bottom right quadrant
                this.RenderBezierCurveTo(hcentre, bottom, right, vcentre + yhandle, hcentre + xhandle, bottom);
                requiresmove = false;
            }
            else
                requiresmove = true;

            if ((sides & Quadrants.BottomLeft) > 0)
            {
                if (requiresmove)
                    this.RenderMoveTo(hcentre, bottom);
                //bottom left quadrant
                this.RenderBezierCurveTo(left, vcentre, hcentre - xhandle, bottom, left, vcentre + yhandle);
            }
        }

        public void FillQuadrants(PDFBrush brush, Rect rect, Quadrants sides)
        {
            this.FillQuadrants(brush, rect.Location, rect.Size, sides);
        }

        public void FillQuadrants(PDFBrush brush, Point pos, Size size, Quadrants sides)
        {
            this.FillQuadrants(brush, pos.X, pos.Y, size.Width, size.Height, sides);
        }

        public void FillQuadrants(PDFBrush brush, Unit x, Unit y, Unit width, Unit height, Quadrants sides)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");

            Rect bounds = new Rect(x, y, width, height);

            this.SaveGraphicsState();
            brush.SetUpGraphics(this, bounds);

            OutputQuadrantShapes(x, y, width, height, sides);
            this.RenderFillPathOp();

            brush.ReleaseGraphics(this, bounds);

            this.RestoreGraphicsState();
        }

        private void OutputQuadrantShapes(Unit x, Unit y, Unit width, Unit height, Quadrants sides)
        {
            PDFReal left = x.RealValue;
            PDFReal right = (x + width).RealValue;
            PDFReal top = y.RealValue;
            PDFReal bottom = (y + height).RealValue;
            PDFReal hcentre = (left + (width.RealValue / (PDFReal)2.0));
            PDFReal vcentre = (top + (height.RealValue / (PDFReal)2.0));
            PDFReal xhandle = ((width.RealValue / (PDFReal)2.0) * (PDFReal)CircularityFactor);
            PDFReal yhandle = ((height.RealValue / (PDFReal)2.0) * (PDFReal)CircularityFactor);

            if ((sides & Quadrants.TopLeft) > 0)
            {
                this.RenderMoveTo(left, vcentre);
                //top left quadrant
                this.RenderBezierCurveTo(hcentre, top, left, vcentre - yhandle, hcentre - xhandle, top);
                this.RenderContinuationLine(hcentre, vcentre);
                this.RenderContinuationLine(left, vcentre);
            }
            

            if ((sides & Quadrants.TopRight) > 0)
            {
                this.RenderMoveTo(hcentre, top);
                //top right quadrant
                this.RenderBezierCurveTo(right, vcentre, hcentre + xhandle, top, right, vcentre - yhandle);
                this.RenderContinuationLine(hcentre, vcentre);
                this.RenderContinuationLine(hcentre, top);
            }
            
            if ((sides & Quadrants.BottomRight) > 0)
            {
                this.RenderMoveTo(right, vcentre);
                //bottom right quadrant
                this.RenderBezierCurveTo(hcentre, bottom, right, vcentre + yhandle, hcentre + xhandle, bottom);
                this.RenderContinuationLine(hcentre, vcentre);
                this.RenderContinuationLine(right, vcentre);
            }

            if ((sides & Quadrants.BottomLeft) > 0)
            {
                this.RenderMoveTo(hcentre, bottom);
                //bottom left quadrant
                this.RenderBezierCurveTo(left, vcentre, hcentre - xhandle, bottom, left, vcentre + yhandle);
                this.RenderContinuationLine(hcentre, vcentre);
                this.RenderContinuationLine(hcentre, bottom);
            }
        }


        public void DrawCurve(Point start, Point end, Point starthandle, Point endhandle)
        {
            this.RenderMoveTo(start.X, start.Y);
            this.RenderBezierCurveTo(end.X, end.Y, starthandle.X, starthandle.Y, endhandle.X, endhandle.Y);
        }

        public void DrawContinuationCurve(Point end, Point starthandle, Point endhandle)
        {
            this.RenderBezierCurveTo(end.X, end.Y, starthandle.X, starthandle.Y, endhandle.X, endhandle.Y);
        }


        public void DrawLine(Unit x1, Unit y1, Unit x2, Unit y2)
        {
            this.RenderMoveTo(x1, y1);
            this.RenderLineTo(x2, y2);
            this.RenderStrokePathOp();
        }

        public void DrawLine(Point start, Point end)
        {
            this.RenderMoveTo(start.X, start.Y);
            this.RenderLineTo(end.X, end.Y);
            this.RenderStrokePathOp();
        }


        public void FillPath(PDFBrush brush, Point location, GraphicsPath path)
        {
            if (null == path)
                throw new ArgumentNullException("path");

            if (null == brush)
                throw new ArgumentNullException("brush");

            this.SaveGraphicsState();
            this.OutputPath(brush, null, location, path);
            this.RestoreGraphicsState();
        }

        public void DrawPath(PDFPen pen, Point location, GraphicsPath path)
        {
            if (null == path)
                throw new ArgumentNullException("path");

            if (null == pen)
                throw new ArgumentNullException("pen");

            this.SaveGraphicsState();
            this.OutputPath(null, pen, location, path);
            this.RestoreGraphicsState();
        }

        public void FillAndStrokePath(PDFBrush brush, PDFPen pen, Point location, GraphicsPath path)
        {
            if (null == path)
                throw new ArgumentNullException("path");
            if (null == pen)
                throw new ArgumentNullException("pen");
            if (null == brush)
                throw new ArgumentNullException("brush");

            this.SaveGraphicsState();
            this.OutputPath(brush, pen, location, path);
            this.RestoreGraphicsState();
        }

        private void OutputPath(PDFBrush brush, PDFPen pen, Point location, GraphicsPath path)
        {
            Rect bounds = new Rect(path.Bounds.X + location.X, path.Bounds.Y + location.Y, path.Bounds.Width, path.Bounds.Height);
            
            if (null != brush)
                brush.SetUpGraphics(this, bounds);
            if (null != pen)
                pen.SetUpGraphics(this, bounds);

            
            PathAdornmentInfo info = null;
                        
            if (path.HasAdornments)
            {
                info = new PathAdornmentInfo(location, 0, brush, pen);
                path.OutputAdornments(this, info, this.Context, AdornmentOrder.Before);

            }
            
            
            if (null != path.PathMatrix)
            {
                this.SetTransformationMatrix(path.PathMatrix, false, true);
            }

            Point cursor = Point.Empty;

            foreach (Path p in path.SubPaths)
            {
                RenderPathData(location, p, ref cursor);
            }

            if (null != brush && null != pen)
                this.RenderFillAndStrokePathOp(path.Mode == GraphicFillMode.EvenOdd);
            else if (null != brush)
                this.RenderFillPathOp(path.Mode == GraphicFillMode.EvenOdd);
            else if (null != pen)
                this.RenderStrokePathOp();

            //Transformation matrix will be released with restore graphics state
            
            

            if (null != info)
            {
                path.OutputAdornments(this, info, this.Context, AdornmentOrder.After);
            }
            
            if (null != brush)
                brush.ReleaseGraphics(this, bounds);
            if (null != pen)
                pen.ReleaseGraphics(this, bounds);
        }

        private void RenderPathData(Point location, Path p, ref Point cursor)
        {
            if (null == p)
                return;
            foreach (PathData data in p.Operations)
            {
                this.RenderPathOp(data, location, ref cursor);
            }

            //if (p.Closed)
            //    RenderCloseStrokePathOp();
        }

        private void RenderPathOp(PathData data, Point location, ref Point cursor)
        {
            switch (data.Type)
            {
                case PathDataType.Move:
                    PathMoveData move = (PathMoveData)data;
                    cursor = move.MoveTo;
                    this.RenderMoveTo(move.MoveTo.X + location.X, move.MoveTo.Y + location.Y);
                    break;
                case PathDataType.Line:
                    PathLineData line = (PathLineData)data;
                    cursor = line.LineTo;
                    this.RenderLineTo(line.LineTo.X + location.X, line.LineTo.Y + location.Y);
                    break;
                case PathDataType.Rect:
                    PathRectData rect = (PathRectData)data;
                    cursor = rect.Rect.Location;
                    this.RenderRectangle(rect.Rect.X + location.X, rect.Rect.Y + location.Y, rect.Rect.Width, rect.Rect.Height);
                    break;
                case PathDataType.SubPath:
                    PathSubPathData sub = (PathSubPathData)data;
                    this.RenderPathData(location, sub.InnerPath, ref cursor);
                    break;
                case PathDataType.Bezier:
                    PathBezierCurveData bez = (PathBezierCurveData)data;
                    cursor = bez.EndPoint;
                    if (bez.HasStartHandle && bez.HasEndHandle)
                    {
                        this.RenderBezierCurveTo(bez.EndPoint.X + location.X, bez.EndPoint.Y + location.Y,
                                         bez.StartHandle.X + location.X, bez.StartHandle.Y + location.Y,
                                         bez.EndHandle.X + location.X, bez.EndHandle.Y + location.Y);
                    }
                    else if (bez.HasStartHandle)
                    {
                        this.RenderBezierCurveToWithStartHandleOnly(bez.EndPoint.X + location.X, bez.EndPoint.Y + location.Y,
                                         bez.StartHandle.X + location.X, bez.StartHandle.Y + location.Y);
                    }
                    else if (bez.HasEndHandle)
                    {
                        this.RenderBezierCurveToWithEndHandleOnly(bez.EndPoint.X + location.X, bez.EndPoint.Y + location.Y,
                                         bez.EndHandle.X + location.X, bez.EndHandle.Y + location.Y);
                    }
                    else
                        this.RenderLineTo(bez.Points[2].X, bez.Points[2].Y);
                    
                    break;
                case PathDataType.Arc:
                    IEnumerable<PathBezierCurveData> segments;
                    PathArcData arc = (PathArcData)data;
                    segments = PathDataHelper.GetBezierCurvesForArc(cursor, arc);
                    foreach (PathBezierCurveData segment in segments)
                    {
                        this.RenderBezierCurveTo(segment.EndPoint.X + location.X, 
                                                 segment.EndPoint.Y + location.Y, 
                                                 segment.StartHandle.X + location.X, 
                                                 segment.StartHandle.Y + location.Y, 
                                                 segment.EndHandle.X + location.X, 
                                                 segment.EndHandle.Y + location.Y);
                    }
                    cursor = arc.EndPoint;
                    break;

                case PathDataType.Quadratic:
                    IEnumerable<PathBezierCurveData> quadSegments;
                    PathQuadraticCurve quad = (PathQuadraticCurve)data;
                    quadSegments = PathDataHelper.GetBezierCurvesForQuadratic(cursor, quad);
                    foreach (PathBezierCurveData quadSeg in quadSegments)
                    {
                        this.RenderBezierCurveTo(quadSeg.EndPoint.X + location.X, quadSeg.EndPoint.Y + location.Y, 
                                                 quadSeg.StartHandle.X + location.X, quadSeg.StartHandle.Y + location.Y,
                                                 quadSeg.EndHandle.X + location.X, quadSeg.EndHandle.Y + location.Y);
                    }
                    cursor = quad.EndPoint;
                    break;

                case PathDataType.Close:
                    this.RenderClosePathOp();
                    cursor = Point.Empty;
                    break;
                    
                default:
                    throw new ArgumentOutOfRangeException("data.Type");

            }
        }
        
        
    }
}
