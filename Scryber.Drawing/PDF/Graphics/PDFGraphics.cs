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
    public partial class PDFGraphics : IDisposable
    {

        /// <summary>
        /// Defines the approximate factor for a circle handles using a bezier curve in a calculation
        /// </summary>
        public static readonly double CircularityFactor = 0.55;

        #region public DrawingOrigin Origin {get;set;}

        private DrawingOrigin _origin = DrawingOrigin.TopLeft;

        public DrawingOrigin Origin
        {
            get { return _origin; }
        }

        #endregion

        #region public bool FillModeEvenOdd {get;set;}

        private bool _fillEvenOdd;

        public bool FillModeEvenOdd
        {
            get { return _fillEvenOdd; }
            set { _fillEvenOdd = value; }
        }

        #endregion

        #region protected PDFWriter Writer {get;}

        private PDFWriter _writer;
        public PDFWriter Writer
        {
            get { return _writer; }
        }

        #endregion

        #region public PDFContextBase Context {get;}

        private ContextBase _context;

        /// <summary>
        /// Gets the current context associated with this PDFGraphics
        /// </summary>
        public ContextBase Context
        {
            get { return _context; }
            private set { _context = value; }
        }

        #endregion

        #region public IPDFResourceContainer Container{get;}

        private IResourceContainer _rsrc;
        public IResourceContainer Container
        {
            get { return _rsrc; }
        }

        #endregion

        #region protected bool OwnsWriter {get;}

        private bool _ownswriter;
        protected bool OwnsWriter
        {
            get { return _ownswriter; }
            private set { this._ownswriter = value; }
        }

        #endregion

        #region public PDFSize ContainerSize

        private Size _pageSize;

        public Size ContainerSize
        {
            get { return this._pageSize; }
        }

        #endregion

        #region protected PDFExternalGraphicsState ExternalState {get;}

        private PDFExternalGraphicsState _extState;

        /// <summary>
        /// Gets the external graphics state for this Graphics class
        /// </summary>
        protected PDFExternalGraphicsState ExternalState
        {
            get { return _extState; }
        }

        #endregion

        #region protected .ctor(writer, size, container)

        protected PDFGraphics(PDFWriter writer, Size size, IResourceContainer container)
        {
            this._writer = writer;
            this._pageSize = size;
            this._rsrc = container;
            this._extState = new PDFExternalGraphicsState(container, writer);
        }

        #endregion

        #region internal static PDFGraphics Create(writer, ownswriter, container, origin, size)

        public static PDFGraphics Create(PDFWriter writer, bool ownswriter, IResourceContainer rsrc, DrawingOrigin origin, Size size, ContextBase context)
        {
            if (origin == DrawingOrigin.BottomLeft)
                throw new ArgumentException(Errors.GraphicsOnlySupportsTopDownDrawing, "origin");

            PDFGraphics g = new PDFGraphics(writer, size, rsrc);
            g.OwnsWriter = ownswriter;
            g.Context = context;
            return g;
        }

        #endregion

       
        //private Opacities _currentOpacity = new Opacities() { Fill = 1.0, Stroke = 1.0 };
        

        #region SaveGraphicsState() + RestoreGraphicsState()

        public void SaveGraphicsState()
        {
            //_opacities.Push(_currentOpacity);
            //Opacities newValues = new Opacities() { Fill = _currentOpacity.Fill, Stroke = _currentOpacity.Stroke };
            //_currentOpacity = newValues;
            this.ExternalState.SaveState();
            this.Writer.WriteOpCodeS(PDFOpCode.SaveState);
        }

        public void RestoreGraphicsState()
        {
            // _currState = null;
            this.ExternalState.RestoreState();
            this.Writer.WriteOpCodeS(PDFOpCode.RestoreState);
            //_currentOpacity = _opacities.Pop();
        }

        #endregion

        //private Resources.PDFExtGSState _currState;

        protected PDFExtGSState EnsureExternalState()
        {
            return null;
            //this.ExternalState.EnsureState();
            //if (null == _currState)
            //{
            //    _currState = new Scryber.Resources.PDFExtGSState();
            //    this.Container.Register(_currState);
            //    _currState.Name.WriteData(this.Writer);
            //    this.Writer.WriteOpCodeS(PDFOpCode.GraphApplyState);
            //}
            //return _currState;
        }


        



        #region public SetStrokeColor() + SetFillColor()

        public void SetStrokeColor(Color color)
        {
            switch (color.ColorSpace)
            {
                case ColorSpace.G:
                    this.Writer.WriteOpCodeS(PDFOpCode.ColorStrokeGrayscaleSpace, (PDFReal)color.Gray);
                    break;
                case ColorSpace.CMYK:
                case ColorSpace.RGB:
                    color = color.ToRGB();
                    this.Writer.WriteOpCodeS(PDFOpCode.ColorStrokeRGBSpace, (PDFReal)color.Red, (PDFReal)color.Green, (PDFReal)color.Blue);
                    break;
                case ColorSpace.HSL:
                case ColorSpace.LAB:
                case ColorSpace.Custom:
                default:
                    throw new ArgumentOutOfRangeException("color.ColorSpace", String.Format(Errors.ColorValueIsNotCurrentlySupported, color.ColorSpace));

            }
        }

        public void SetFillColor(Color color)
        {
            switch (color.ColorSpace)
            {
                case ColorSpace.G:
                    this.Writer.WriteOpCodeS(PDFOpCode.ColorFillGrayscaleSpace, (PDFReal)color.Gray);
                    break;
                case ColorSpace.CMYK:
                case ColorSpace.RGB:
                    color = color.ToRGB();
                    this.Writer.WriteOpCodeS(PDFOpCode.ColorFillRGBSpace, (PDFReal)color.Red, (PDFReal)color.Green, (PDFReal)color.Blue);
                    break;
                case ColorSpace.HSL:
                case ColorSpace.LAB:
                case ColorSpace.Custom:
                    throw new NotSupportedException(String.Format(Errors.ColorValueIsNotCurrentlySupported, color.ColorSpace));

                default:
                    throw new ArgumentOutOfRangeException("color.ColorSpace", String.Format(Errors.ColorValueIsNotCurrentlySupported, color.ColorSpace));

            }
        }

        #endregion


        #region SetFillPattern() + ClearFillPattern()

        /// <summary>
        /// If we are rendering with a pattern then we need to use a transformation
        /// matrix when rendering so that the pattern starts at the start of the bounds
        /// </summary>
        protected bool UsePatternFillTransformation = false;

        public void SetFillPattern(PDFName patternName)
        {
            this.Writer.WriteOpCodeS(PDFOpCode.ColorSetFillSpace, (PDFName)"Pattern");
            this.Writer.WriteOpCodeS(PDFOpCode.ColorFillPattern, patternName);
            this.UsePatternFillTransformation = true;
        }

        public void ClearFillPattern()
        {
            this.UsePatternFillTransformation = false;
        }


        #endregion

        #region SetStrokeOpacity() + SetFillOpacity()

        public void SetStrokeOpacity(PDFReal opacity)
        {
            if (opacity.Value < 0.0 || opacity.Value > 1.0)
                throw new ArgumentOutOfRangeException("Opacity values can only be between 0.0 (transparent) and 1.0 (blockout)");
            this.ExternalState.SetStrokeOpacity(opacity);

            //if (opacity.Value != _currentOpacity.Stroke)
            //{
            //    Resources.PDFExtGSState state = this.EnsureExternalState();
            //    state.States[Resources.PDFExtGSState.ColorStrokeOpacity] = opacity;
            //    _currentOpacity.Stroke = opacity.Value;
            //}
        }


        public void SetFillOpacity(PDFReal opacity)
        {
            if (opacity.Value < 0.0 || opacity.Value > 1.0)
                throw new ArgumentOutOfRangeException("Opacity values can only be between 0.0 (transparent) and 1.0 (blockout)");
            
            this.ExternalState.SetFillOpacity(opacity);
            //if(opacity.Value != _currentOpacity.Fill)
            //{
            //    Resources.PDFExtGSState state = this.EnsureExternalState();
            //    state.States[Resources.PDFExtGSState.ColorFillOpactity] = opacity;
            //    _currentOpacity.Fill = opacity.Value;
            //}
        }

        #endregion

        #region SetTransformationMatrix()

        private const int Matrix2DTransformationLength = 6;

        public void SetTransformationMatrix(PDFTransformationMatrix matrix, bool appliesToText, bool appliesToDrawing)
        {
            double[] all = matrix.Components;
            if (all.Length != Matrix2DTransformationLength)
                throw new IndexOutOfRangeException("A 2D transformation matrix can only have 6 values");

            if (appliesToDrawing)
            {
                for (int i = 0; i < Matrix2DTransformationLength; i++)
                {
                    this.Writer.WriteRealS(all[i], "F5");
                }

                this.Writer.WriteOpCodeS(PDFOpCode.GraphTransformMatrix);
            }

            if(appliesToText)
            {
                for (int i = 0; i < Matrix2DTransformationLength; i++)
                {
                    this.Writer.WriteRealS(all[i], "F5");
                }

                this.Writer.WriteOpCodeS(PDFOpCode.TxtTransformMatrix);
            }

        }

        #endregion

        #region protected internal RenderLineWidth() + RenderLineDash() + RenderLineJoin() + RenderLineCap() + RenderLineMitre()

        public void RenderLineWidth(Unit width)
        {
            this.Writer.WriteOpCodeS(PDFOpCode.GraphLineWidth, width.ToPoints().RealValue);
        }

        public void RenderLineDash(Dash dash)
        {
            this.Writer.WriteArrayNumberEntries(true, dash.Pattern);
            this.Writer.WriteOpCodeS(PDFOpCode.GraphDashPattern, (PDFNumber)dash.Phase);
        }

        public void RenderLineJoin(LineJoin linejoin)
        {
            PDFNumber join = (PDFNumber)(int)linejoin;
            Writer.WriteOpCodeS(PDFOpCode.GraphLineJoin, join);
        }

        public void RenderLineCap(LineCaps linecap)
        {
            PDFNumber cap = (PDFNumber)(int)linecap;
            Writer.WriteOpCodeS(PDFOpCode.GraphLineCap, cap);
        }

        public void RenderLineMitre(float mitreLimit)
        {
            PDFReal real = (PDFReal)mitreLimit;
            Writer.WriteOpCodeS(PDFOpCode.GraphMiterLimit, real);
        }

        #endregion

        #region protected RenderStrokePathOp(), RenderFillPathOp(), RenderFillAndStrokePathOp()

        protected void RenderStrokePathOp()
        {
            Writer.WriteOpCodeS(PDFOpCode.GraphStrokePath);
        }

        protected void RenderCloseStrokePathOp()
        {
            Writer.WriteOpCodeS(PDFOpCode.GraphCloseAndStroke);
        }

        protected void RenderFillPathOp()
        {
            this.RenderFillPathOp(this.FillModeEvenOdd);
        }

        protected void RenderFillPathOp(bool evenodd)
        {
            if (evenodd)
                Writer.WriteOpCodeS(PDFOpCode.GraphFillPathEvenOdd);
            else
                Writer.WriteOpCodeS(PDFOpCode.GraphFillPath);
        }

        protected void RenderClosePathOp()
        {
            Writer.WriteOpCodeS(PDFOpCode.GraphClose);
        }



        protected void RenderFillAndStrokePathOp()
        {
            RenderFillAndStrokePathOp(this.FillModeEvenOdd); 
        }

        protected void RenderFillAndStrokePathOp(bool evenodd)
        {
            if (evenodd)
                Writer.WriteOpCodeS(PDFOpCode.GraphFillAndStrokeEvenOdd);
            else
                Writer.WriteOpCodeS(PDFOpCode.GraphFillAndStroke);
        }

        #endregion

        #region protected void RenderMoveTo(PDFUnit x, PDFUnit y) + 1 overload

        protected void RenderMoveTo(Unit x, Unit y)
        {
            this.RenderMoveTo(x.RealValue, y.RealValue);
        }

        protected virtual void RenderMoveTo(PDFReal x, PDFReal y)
        {
            this.Writer.WriteOpCodeS(PDFOpCode.GraphMove, GetXPosition(x), GetYPosition(y));
        }

        #endregion

        #region protected void RenderLineTo(PDFUnit x, PDFUnit y) + 1 overload

        protected void RenderLineTo(Unit x, Unit y)
        {
            this.RenderLineTo(x.RealValue, y.RealValue);
        }

        protected virtual void RenderLineTo(PDFReal x, PDFReal y)
        {
            this.Writer.WriteOpCodeS(PDFOpCode.GraphLineTo, GetXPosition(x), GetYPosition(y));
        }

        #endregion

        #region protected void RenderBezierCurveTo() + 5 overloads

        protected void RenderBezierCurveTo(Unit endX, Unit endY, Unit starthandleX, Unit startHandleY, Unit endhandleX, Unit endhandleY)
        {
            this.RenderBezierCurveTo(endX.RealValue, endY.RealValue, starthandleX.RealValue, startHandleY.RealValue, endhandleX.RealValue, endhandleY.RealValue);
        }

        protected virtual void RenderBezierCurveTo(PDFReal endX, PDFReal endY, PDFReal starthandleX, PDFReal startHandleY, PDFReal endhandleX, PDFReal endhandleY)
        {
            starthandleX = GetXPosition(starthandleX);
            startHandleY = GetYPosition(startHandleY);
            endhandleX = GetXPosition(endhandleX);
            endhandleY = GetYPosition(endhandleY);
            endX = GetXPosition(endX);
            endY = GetYPosition(endY);
            this.Writer.WriteOpCodeS(PDFOpCode.GraphCurve2Handle, starthandleX, startHandleY, endhandleX, endhandleY, endX, endY);
        }


        protected void RenderBezierCurveToWithStartHandleOnly(Unit endX, Unit endY, Unit starthandleX, Unit startHandleY)
        {
            this.RenderBezierCurveToWithStartHandleOnly(endX.RealValue, endY.RealValue, starthandleX.RealValue, startHandleY.RealValue);
        }

        protected virtual void RenderBezierCurveToWithStartHandleOnly(PDFReal endX, PDFReal endY, PDFReal starthandleX, PDFReal startHandleY)
        {
            starthandleX = GetXPosition(starthandleX);
            startHandleY = GetYPosition(startHandleY);
            endX = GetXPosition(endX);
            endY = GetYPosition(endY);
            this.Writer.WriteOpCodeS(PDFOpCode.GraphCurve1HandleBegin, starthandleX, startHandleY, endX, endY);
        }


        protected void RenderBezierCurveToWithEndHandleOnly(Unit endX, Unit endY, Unit endhandleX, Unit endHandleY)
        {
            this.RenderBezierCurveToWithEndHandleOnly(endX.RealValue, endY.RealValue, endhandleX.RealValue, endHandleY.RealValue);
        }

        protected virtual void RenderBezierCurveToWithEndHandleOnly(PDFReal endX, PDFReal endY, PDFReal endhandleX, PDFReal endHandleY)
        {
            endhandleX = GetXPosition(endhandleX);
            endHandleY = GetYPosition(endHandleY);
            endX = GetXPosition(endX);
            endY = GetYPosition(endY);
            this.Writer.WriteOpCodeS(PDFOpCode.GraphCurve1HandleEnd, endhandleX, endHandleY, endX, endY);
        }

        #endregion

        #region protected virtual void RenderRectangle()

        /// <summary>
        /// Actual implementation method to render the operations to append a rectangle shape to the current path
        /// </summary>
        /// <param name="x">The left position</param>
        /// <param name="y">The top position</param>
        /// <param name="width">The width</param>
        /// <param name="height">The height</param>
        protected virtual void RenderRectangle(Unit x, Unit y, Unit width, Unit height)
        {
            var x1 = GetXPosition(x.RealValue);
            var y1 = GetYPosition(y.RealValue);
            var w1 = GetXOffset(width.RealValue);
            var h1 = GetYOffset(height.RealValue);
            
            this.Writer.WriteOpCodeS(PDFOpCode.GraphRect, x1, y1, w1, h1);
        }

        #endregion

        #region protected virtual void RenderLine() + RenderContinuationLine()

        protected virtual void RenderLine(Unit x1, Unit y1, Unit x2, Unit y2)
        {
            this.Writer.WriteOpCodeS(PDFOpCode.GraphMove, GetXPosition(x1), GetYPosition(y1));
            this.Writer.WriteOpCodeS(PDFOpCode.GraphLineTo, GetXPosition(x2), GetYPosition(y2));
        }

        protected virtual void RenderContinuationLine(Unit x, Unit y)
        {
            this.Writer.WriteOpCodeS(PDFOpCode.GraphLineTo, GetXPosition(x), GetYPosition(y));
        }

        protected virtual void RenderContinuationLine(PDFReal x, PDFReal y)
        {
            this.Writer.WriteOpCodeS(PDFOpCode.GraphLineTo, GetXPosition(x), GetYPosition(y));
        }

        #endregion

        #region Translation Offsets

        //Stores a stack of explicit offsets and the current values.

        private Stack<Point> _translationOffsets;
        private PDFReal _currentTranslationXOffset = PDFReal.Zero;
        private PDFReal _currentTranslationYOffset = PDFReal.Zero;

        public PDFReal TranslationXOffset
        {
            get
            {
                return _currentTranslationXOffset;
            }
        }

        public PDFReal TranslationYOffset
        {
            get
            {
                return _currentTranslationYOffset;
            }
        }

        public void SaveTranslationOffset(PDFReal x, PDFReal y)
        {
            if (null == _translationOffsets)
                _translationOffsets = new Stack<Point>();
            else
            {
                x += _currentTranslationXOffset;
                y += _currentTranslationYOffset;
            }
            _translationOffsets.Push(new Point(x.Value, y.Value));
            _currentTranslationXOffset = x;
            _currentTranslationYOffset = y;
        }

        public void RestoreTranslationOffset()
        {
            if (_translationOffsets != null && _translationOffsets.Count > 0)
            {
                _translationOffsets.Pop();
                if (_translationOffsets.Count > 0)
                {
                    var prev = _translationOffsets.Peek();
                    _currentTranslationXOffset = prev.X.RealValue;
                    _currentTranslationYOffset = prev.Y.RealValue;
                }
                else
                {
                    _currentTranslationXOffset = PDFReal.Zero;
                    _currentTranslationYOffset = PDFReal.Zero;
                }
            }
            else
            {
                _currentTranslationXOffset = PDFReal.Zero;
                _currentTranslationYOffset = PDFReal.Zero;
            }
        }

        #endregion

        #region public PDFReal GetXPosition(PDFUnit x) + 4 Overloads

        public PDFReal GetXPosition(Unit ux)
        {
            return ux.RealValue - TranslationXOffset;
        }

        public PDFReal GetXPosition(Unit ux, Unit width)
        {
            return ux.RealValue - TranslationXOffset;
        }

        public PDFReal GetXPosition(Unit ux, PDFReal width)
        {
            return ux.RealValue - TranslationXOffset;
        }

        public PDFReal GetXPosition(PDFReal x, PDFReal width)
        {
            return x - TranslationXOffset;
        }

        public PDFReal GetXPosition(PDFReal x)
        {
            return x - TranslationXOffset;
        }

        public PDFReal GetXPosition(double d)
        {
            return ((PDFReal)d) - TranslationXOffset;
        }

        #endregion

        #region public PDFReal GetYPosition() + 5 overloads

        public PDFReal GetYPosition(Unit uy)
        {
            return (this.ContainerSize.Height.RealValue - TranslationYOffset) - uy.RealValue;
        }

        public PDFReal GetYPosition(Unit uy, PDFReal height)
        {
            return (this.ContainerSize.Height.RealValue - TranslationYOffset) - uy.RealValue - height;
        }

        public PDFReal GetYPosition(PDFReal y)
        {
            return (this.ContainerSize.Height.RealValue - TranslationYOffset) - y;
        }

        public PDFReal GetYPosition(PDFReal y, PDFReal height)
        {
            return (this.ContainerSize.Height.RealValue - TranslationYOffset) - y - height;
        }

        #endregion

        #region public PDFReal GetYOffset() + 1 overload

        public PDFReal GetYOffset(Unit uy)
        {
            return 0 - (uy.RealValue); // TranslationYOffset - uy.RealValue;
        }

        public PDFReal GetYOffset(PDFReal y)
        {
            return 0.0 - y; // TranslationYOffset - y;
        }

        #endregion

        #region public PDFReal GetXOffset() + 1 overloads

        public PDFReal GetXOffset(Unit ux)
        {
            return ux.RealValue; // - TranslationXOffset;
        }

        public PDFReal GetXOffset(PDFReal x)
        {
            return x; // - TranslationXOffset;
        }

        #endregion

        #region public void SetClipRect(PDFRect rectangle) + 3 overloads

        public void SetClipRect(Rect rectangle)
        {
            this.SetClipRect(rectangle.Location, rectangle.Size);
        }

        public void SetClipRect(Point pt, Size sz)
        {
            this.RenderRectangle(pt.X, pt.Y, sz.Width, sz.Height);
            this.Writer.WriteOpCodeS(PDFOpCode.GraphSetClip);
            this.Writer.WriteOpCodeS(PDFOpCode.GraphNoOp);
        }

        public void SetClipRect(Rect rect, Sides sides, Unit cornerradius)
        {
            this.SetClipRect(rect.Location, rect.Size, sides, cornerradius);
        }

        public void SetClipRect(Point pt, Size sz, Sides sides, Unit cornerradius)
        {
            if (cornerradius > Unit.Zero)
            {
                this.DoOutputRoundRectangleWithSidesFill(pt.X, pt.Y, sz.Width, sz.Height, cornerradius, sides);
            }
            else
                this.RenderRectangle(pt.X, pt.Y, sz.Width, sz.Height);

            this.Writer.WriteOpCodeS(PDFOpCode.GraphSetClip);
            this.Writer.WriteOpCodeS(PDFOpCode.GraphNoOp);
        }

        #endregion

        #region public void PaintXObject(Scryber.PDF.Resources.PDFResource rsrc)

        public void PaintXObject(Scryber.PDF.Resources.PDFResource rsrc)
        {
            if (null == rsrc )
                throw new ArgumentNullException("The resource cannot be null");

            var name = rsrc.Name;
            this.PaintXObject(name);
        }

        public void PaintXObject(PDFName name)
        {
            if (null == name || string.IsNullOrEmpty(name.Value))
                throw new ArgumentNullException("The value of name cannot be null");

            this.Writer.WriteOpCodeS(PDFOpCode.XobjPaint, name);
        }

        #endregion

        #region public void PaintImageRef()

        /// <summary>
        /// Writes a reference to the specified image for rendering in the defined position at that size within the current graphics stream
        /// </summary>
        /// <param name="img"></param>
        /// <param name="imgsize"></param>
        /// <param name="pos"></param>
        /// <remarks>This does not include the image within the document or ensure that it is available</remarks>
        public void PaintImageRef(Scryber.PDF.Resources.PDFImageXObject img, Size imgsize, Point pos)
        {
            if (string.IsNullOrEmpty(img.Name.Value))
                throw new ArgumentNullException("img.Name");
            PDFReal width = (PDFReal)(imgsize.Width.RealValue);
            PDFReal height = (PDFReal)(imgsize.Height.RealValue);
            PDFReal x = pos.X.RealValue;
            PDFReal y;
            if (this.Origin == DrawingOrigin.TopLeft)
                //convert the top down to bottom of the page up to the image
                y = ContainerSize.Height.RealValue - pos.Y.RealValue - height; 
            else
                y = pos.Y.RealValue;

            this.Writer.WriteOpCodeS(PDFOpCode.GraphTransformMatrix,
                        width, PDFReal.Zero,
                        PDFReal.Zero, height, x, y);

            this.Writer.WriteOpCodeS(PDFOpCode.XobjPaint, img.Name);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.OwnsWriter && this.Writer != null)
                    this.Writer.Dispose();
            }
        }

        #endregion


    }
}
