using System;
using System.Runtime.CompilerServices;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.Expressive;
using Scryber.PDF;
using Scryber.PDF.Graphics;
using Scryber.PDF.Layout;
using Scryber.PDF.Native;
using Scryber.PDF.Resources;
using Scryber.Styles;
using Scryber.Svg.Components;

namespace Scryber.Svg.Imaging
{

    /// <summary>
    /// Represents a single discreet SVG image (canvas), loaded from another source. 
    /// </summary>
    public class SVGPDFImageData : ImageData, ILayoutComponent
    {

        private PDFObjectRef _renderRef = null;
        private PDFObjectRef _layoutRef = null;
        private PDFName _layoutName = null;




        #region public Rect? ImgXObjectBBox

        private Rect? _imgXObjectBBox = null;

        /// <summary>
        /// If set to a value then returns the bounding box for the imageXObject that will be output to
        /// render the SVGCanvas, or returns null
        /// </summary>
        public Rect? ImgXObjectBBox
        {
            get { return _imgXObjectBBox; }
            private set { _imgXObjectBBox = value; }
        }

        #endregion

        #region public SVGCanvas Canvas {get; private set;}

        private SVGCanvas _svgCanvas = null;

        /// <summary>
        /// Gets the canvas for this SVG Image data that is parsed from the loaded content
        /// </summary>
        public SVGCanvas Canvas
        {
            get { return _svgCanvas; }
            private set { _svgCanvas = value; }
        }

        #endregion

        #region public SVGImage Image {get; private set;}

        private PDFLayoutBlock _layout = null;

        /// <summary>
        /// Gets the pdf layout associated with this SVG Image data (if any)
        /// </summary>
        public PDFLayoutBlock Layout
        {
            get { return _layout; }
            private set { _layout = value; }
        }

        #endregion


        #region ILayoutComponent properties

        string IComponent.ID { get; set; }
        string IComponent.ElementName { get; set; }

        private IDocument _document;

        IDocument IComponent.Document
        {
            get { return this._document; }
        }

        public IComponent Parent { get; set; }

        #endregion

        public SVGPDFImageData(string source, SVGCanvas canvas, int w, int h)
            : base(ObjectTypes.ImageData, source, w, h)
        {
            _svgCanvas = canvas ?? throw new ArgumentNullException(nameof(canvas));
            _svgCanvas.IsDiscreetSVG = true;
        }




        /// <summary>
        /// Returns the defined image size on the referenced canvas
        /// </summary>
        /// <returns></returns>
        public override Size GetSize()
        {
            if (null != this.Canvas && this.ImgXObjectBBox.HasValue)
            {

                var sz = this.ImgXObjectBBox.Value.Size;
                return sz;
            }

            var baseSize = base.GetSize();
            return baseSize;
        }

        public override void ResetFilterCache()
        {

        }


        //
        // ILayoutComponent
        //

        #region Init and Load implementation

        public event InitializedEventHandler Initialized;
        public event LoadedEventHandler Loaded;

        protected virtual void OnInitialized(InitContext context)
        {
            if (null != this.Initialized)
                this.Initialized(this, new InitEventArgs(context));
        }

        protected virtual void OnLoaded(LoadContext context)
        {
            if (null != this.Loaded)
                this.Loaded(this, new LoadEventArgs(context));
        }


        public void Init(InitContext context)
        {
            this._document = context.Document;

            if (null != this._svgCanvas)
                _svgCanvas.Init(context);

            this.OnInitialized(context);

        }

        public void Load(LoadContext context)
        {
            this._document = context.Document;

            if (null != this._svgCanvas)
                _svgCanvas.Load(context);

            this.OnLoaded(context);
        }


        #endregion

        #region MapPath

        public string MapPath(string source)
        {
            var service = ServiceProvider.GetService<IPathMappingService>();
            if (null == service) return source;


            bool isfile = false;

            if (!string.IsNullOrEmpty(this.SourcePath))
            {
                return service.MapPath(ParserLoadType.Generation, source, this.SourcePath, out isfile);
            }
            else
            {
                return service.MapPath(ParserLoadType.Generation, source, string.Empty, out isfile);
            }
        }

        #endregion

        public Size GetRequiredSizeForLayout(Size available, LayoutContext context, Style appliedstyle)
        {
            var newSize = available;
            var canvasStyle = this.Canvas.GetAppliedStyle();
            
            if (appliedstyle.TryGetValue(StyleKeys.SizeWidthKey, out var width))
            {
                newSize.Width = width.Value(appliedstyle);
                if (appliedstyle.TryGetValue(StyleKeys.SizeHeightKey, out var height))
                {
                    newSize.Height = height.Value(appliedstyle);
                }
                else
                {
                    //Scale height Proportionally if we can
                    if (canvasStyle.IsValueDefined(StyleKeys.SizeWidthKey) && canvasStyle.IsValueDefined(StyleKeys.SizeHeightKey))
                    {
                        var canvasWidth = canvasStyle.GetValue(StyleKeys.SizeWidthKey, Unit.Empty).PointsValue;
                        var canvasHeight = canvasStyle.GetValue(StyleKeys.SizeHeightKey, Unit.Empty).PointsValue;
                        var scale = newSize.Width.PointsValue / canvasWidth;
                        
                        newSize.Height = canvasHeight * scale;
                    }
                    else if (canvasStyle.IsValueDefined(StyleKeys.PositionViewPort))
                    {
                        var canvasViewPort = canvasStyle.GetValue(StyleKeys.PositionViewPort, Rect.Empty);
                        var scale = newSize.Width.PointsValue / canvasViewPort.Width.PointsValue;
                        
                        newSize.Height = canvasViewPort.Height.PointsValue * scale;
                    }
                }
            }
            else if (appliedstyle.TryGetValue(StyleKeys.SizeHeightKey, out var height))
            {
                newSize.Height = height.Value(appliedstyle);
                
                if (canvasStyle.IsValueDefined(StyleKeys.SizeWidthKey) && canvasStyle.IsValueDefined(StyleKeys.SizeHeightKey))
                {
                    var canvasWidth = canvasStyle.GetValue(StyleKeys.SizeWidthKey, Unit.Empty).PointsValue;
                    var canvasHeight = canvasStyle.GetValue(StyleKeys.SizeHeightKey, Unit.Empty).PointsValue;
                    var scale = newSize.Height.PointsValue / canvasHeight;
                    
                    newSize.Width = canvasWidth * scale;
                }
                else if (canvasStyle.IsValueDefined(StyleKeys.PositionViewPort))
                {
                    var canvasViewPort = canvasStyle.GetValue(StyleKeys.PositionViewPort, Rect.Empty);
                    var scale = newSize.Height.PointsValue / canvasViewPort.Height.PointsValue;
                    
                    newSize.Width = canvasViewPort.Width.PointsValue * scale;
                }
            }
            else if (null != this.Canvas)
            {
               
                bool hasCanvasSize = false;
                if (canvasStyle.IsValueDefined(StyleKeys.SizeWidthKey))
                {
                    newSize.Width = canvasStyle.GetValue(StyleKeys.SizeWidthKey, Unit.Zero);
                    hasCanvasSize = true;
                    if (canvasStyle.IsValueDefined(StyleKeys.SizeHeightKey))
                    {
                        newSize.Height = canvasStyle.GetValue(StyleKeys.SizeHeightKey, Unit.Zero);
                    }
                    else
                    {
                        //Should this be propotional
                    }
                }
                else if (canvasStyle.IsValueDefined(StyleKeys.SizeHeightKey))
                {
                    hasCanvasSize = true;
                    newSize.Height = canvasStyle.GetValue(StyleKeys.SizeHeightKey, Unit.Zero);
                    //Should width be calculated as proportional
                }
                else if (canvasStyle.IsValueDefined(StyleKeys.PositionViewPort))
                {
                    var canvasViewPort = canvasStyle.GetValue(StyleKeys.PositionViewPort, Rect.Empty);
                    
                    if(canvasViewPort.IsEmpty)
                        canvasViewPort = new Rect(0, 0, SVGCanvas.DefaultWidth, SVGCanvas.DefaultHeight);
                    
                    var scaleW = available.Width.PointsValue / canvasViewPort.Width.PointsValue;
                    //var scaleH = available.Height.PointsValue / canvasViewPort.Height.PointsValue;
                    //var minScale = Math.Min(scaleH, scaleW);
                    
                    newSize.Width = canvasViewPort.Width.PointsValue * scaleW;
                    newSize.Height = canvasViewPort.Height.PointsValue * scaleW;
                }

                if (hasCanvasSize)
                {
                    if (newSize.Height > available.Height)
                    {
                        var scale = newSize.Height.PointsValue / available.Height.PointsValue;
                        newSize.Height = available.Height;
                        newSize.Width = newSize.Width / scale;
                    }
                }
                
            }

            if (null == this._layout)
            {
                this._layout = DoLayoutCanvas(context, appliedstyle);
            }

            return newSize;
        }

        public override Rect? GetClippingRect(Point offset, Size available, ContextBase context)
        {
            return new Rect(offset.X, offset.Y, available.Width, available.Height);
        }

        public override Size GetRequiredSizeForRender(Point offset, Size available, ContextBase context)
        {
            var orig = this.GetSize();

            //We are rendering 1: 1 from the Image to the Canvas, so we now scale by the ratio.
            var scaleX = available.Width.PointsValue / orig.Width.PointsValue;
            var scaleY = available.Height.PointsValue / orig.Height.PointsValue;

            ViewPortAspectRatio
                aspectRatio =
                    this.Canvas
                        .PreserveAspectRatio; //We assume this is not set on a style as we are a referenced image and therefore independent

            if (aspectRatio.Align == AspectRatioAlign.None)
            {
                available = new Size(scaleX, scaleY);
            }
            else if (aspectRatio.Meet != AspectRatioMeet.Slice)
            {
                var min = Math.Min(scaleX, scaleY);
                available = new Size(min, min);
            }
            else
            {
                var max = Math.Max(scaleX, scaleY);
                available = new Size(max, max);
            }

            return available;
        }

        public override Point GetRequiredOffsetForRender(Point offset, Size available, ContextBase context)
        {
            var orig = this.GetSize();

            var scaleX = available.Width.PointsValue / orig.Width.PointsValue;
            var scaleY = available.Height.PointsValue / orig.Height.PointsValue;

            offset.Y += available.Height;

            ViewPortAspectRatio
                aspectRatio =
                    this.Canvas
                        .PreserveAspectRatio; //We assume this is not set on a style as we are a referenced image and therefore independent

            if (aspectRatio.Align == AspectRatioAlign.None)
            {
                //Do nothing as we are not preserving aspect ratio
            }
            else if (aspectRatio.Meet == AspectRatioMeet.Meet)
            {
                offset = UpdateOffsetForMeetScale(offset, available, scaleX, scaleY, orig, aspectRatio, context);
            }
            else
            {
                offset = UpdateOffsetForSliceScale(offset, available, scaleX, scaleY, orig, aspectRatio, context);
            }
            //
            // this._imgXObjectBBox = this.UpdateImageBoundingBoxForAspectRatio(offset, available, scaleX, scaleY, orig, aspectRatio, context);


            return offset;
        }

        private Point UpdateOffsetForMeetScale(Point offset, Size available, double scaleX, double scaleY,
            Size original, ViewPortAspectRatio aspectRatio, ContextBase context)
        {
            if (scaleX > scaleY)
            {
                var used = original.Width * scaleY;
                var space = available.Width - used;

                switch (aspectRatio.Align)
                {
                    case AspectRatioAlign.xMinYMax:
                    case AspectRatioAlign.xMinYMid:
                    case AspectRatioAlign.xMinYMin:

                        break;

                    case AspectRatioAlign.xMidYMin:
                    case AspectRatioAlign.xMidYMax:
                    case AspectRatioAlign.xMidYMid:
                        offset.X += space / 2.0;
                        break;
                    case AspectRatioAlign.xMaxYMax:
                    case AspectRatioAlign.xMaxYMid:
                    case AspectRatioAlign.xMaxYMin:
                        offset.X += space;
                        break;

                    case AspectRatioAlign.None:
                        throw new ArgumentOutOfRangeException("aspectRatio",
                            "The value of none in preserve aspect ratio should be handled elsewhere.");

                    default:
                        //Do nothing
                        break;
                }
            }
            else if (scaleX < scaleY)
            {
                var used = original.Height * scaleX;
                var space = available.Height - used;

                switch (aspectRatio.Align)
                {
                    case AspectRatioAlign.xMinYMin:
                    case AspectRatioAlign.xMidYMin:
                    case AspectRatioAlign.xMaxYMin:
                        offset.Y -= space;
                        break;
                    case AspectRatioAlign.xMinYMid:
                    case AspectRatioAlign.xMidYMid:
                    case AspectRatioAlign.xMaxYMid:
                        offset.Y -= space / 2.0;
                        break;
                    case AspectRatioAlign.xMinYMax:
                    case AspectRatioAlign.xMidYMax:
                    case AspectRatioAlign.xMaxYMax:
                        //Already at the bottom, so do nothing
                        break;
                    case AspectRatioAlign.None:
                        throw new ArgumentOutOfRangeException("aspectRatio",
                            "The value of none in preserve aspect ratio should be handled elsewhere.");

                    default:
                        break;
                }
            }

            return offset;
        }

        private Point UpdateOffsetForSliceScale(Point offset, Size available, double scaleX, double scaleY,
            Size original, ViewPortAspectRatio aspectRatio, ContextBase context)
        {
            if (scaleX > scaleY)
            {
                var used = original.Height * scaleX;
                var extra = used - available.Height;

                switch (aspectRatio.Align)
                {
                    case AspectRatioAlign.xMinYMin:
                    case AspectRatioAlign.xMidYMin:
                    case AspectRatioAlign.xMaxYMin:
                        offset.Y += extra;
                        break;
                    case AspectRatioAlign.xMinYMid:
                    case AspectRatioAlign.xMidYMid:
                    case AspectRatioAlign.xMaxYMid:
                        offset.Y += extra / 2.0;
                        break;
                    case AspectRatioAlign.xMinYMax:
                    case AspectRatioAlign.xMidYMax:
                    case AspectRatioAlign.xMaxYMax:
                        //Do nothing
                        break;

                    default:
                        break;
                }
            }
            else if (scaleX < scaleY)
            {
                var used = original.Width * scaleY;
                var extra = used - available.Width;

                switch (aspectRatio.Align)
                {
                    case(AspectRatioAlign.xMinYMin):
                    case(AspectRatioAlign.xMinYMid):
                    case(AspectRatioAlign.xMinYMax):
                        //No change as left aligned
                        break;
                    case(AspectRatioAlign.xMidYMin):
                    case(AspectRatioAlign.xMidYMid):
                    case(AspectRatioAlign.xMidYMax):
                        offset.X -= extra / 2.0;
                        break;
                    case(AspectRatioAlign.xMaxYMin):
                    case(AspectRatioAlign.xMaxYMid):
                    case(AspectRatioAlign.xMaxYMax):
                        offset.X -= extra;
                        break;
                    default:
                        break;
                    
                }
            }

            return offset;
        }
        
        protected virtual Rect UpdateImageBoundingBoxForAspectRatio(Point offset, Size available, double scaleX, double scaleY,
            Size original, ViewPortAspectRatio aspectRatio, ContextBase context)
        {
            if (!this.ImgXObjectBBox.HasValue)
                this.ImgXObjectBBox = new Rect(0, 0, original.Width, original.Height);

            var bbox = this.ImgXObjectBBox.Value;
            
            if (aspectRatio.Meet == AspectRatioMeet.Slice)
            {
                if (scaleX > scaleY)
                {
                    var used = original.Height * scaleX;
                    var vsize = available.Height / scaleX; //The height of the canvas that will be visible
                    var vspace = original.Height - vsize;

                    switch (aspectRatio.Align)
                    {
                        case AspectRatioAlign.xMinYMin:
                        case AspectRatioAlign.xMidYMin:
                        case AspectRatioAlign.xMaxYMin:
                            //The top of the image is drawn
                            bbox.Y = bbox.Height - vsize;
                            bbox.Y += 1.0; //No idea, but there is a position issue
                            break;
                        case AspectRatioAlign.xMinYMid:
                        case AspectRatioAlign.xMidYMid:
                        case AspectRatioAlign.xMaxYMid:
                            bbox.Y = (vspace / 2.0) + vsize;
                            bbox.Height = vsize * scaleX;
                            
                            bbox.Y += 1.0; //No idea, but there is a position issue
                            bbox.Height += 1.0;
                            break;
                        case AspectRatioAlign.xMinYMax:
                        case AspectRatioAlign.xMidYMax:
                        case AspectRatioAlign.xMaxYMax:
                            bbox.Height = vsize;
                            
                            bbox.Height += 1.0;
                            break;

                        default:
                            break;
                    }
                    
                }
                else if (scaleX < scaleY)
                {
                    
                }
            }

            return bbox;
        }

        public void SetRenderSizes(Rect content, Rect border, Rect total, Style style)
        {
            //We do nothing here as it is pre-calculated
        }

        protected virtual PDFLayoutBlock DoLayoutCanvas(LayoutContext context, Style style)
        {
            PDFLayoutContext layoutContext = (PDFLayoutContext)context;
            var prevStack = context.StyleStack;
            var prevItems = context.Items;
            PDFLayoutItem layout = null;
            try
            {
                //Clear the styles and the items so nothing will affect the declared style.

                var baseStyle = SVGCanvas.GetDefaultBaseStyle();
                context.StyleStack = new StyleStack(baseStyle);
                context.Items = new ItemCollection(_svgCanvas);


                var applied = this._svgCanvas.GetAppliedStyle();

                //We set the position mode as fixed with a offset of zero, zero
                //so it does not affect the flow of the rest of the page 

                applied.SetValue(StyleKeys.PositionModeKey, PositionMode.Fixed);
                applied.SetValue(StyleKeys.PositionXKey, Unit.Zero);
                applied.SetValue(StyleKeys.PositionYKey, Unit.Zero);

                Rect viewport = Rect.Empty;
                Unit width = SVGCanvas.DefaultWidth;
                Unit height = SVGCanvas.DefaultHeight;

                if (applied.TryGetValue(StyleKeys.PositionViewPort, out var found))
                {
                    viewport = found.Value(applied);
                    width = viewport.Width;
                    height = viewport.Height;
                }

                if (_svgCanvas.Style.IsValueDefined(StyleKeys.SizeWidthKey))
                {
                    width = _svgCanvas.Style.GetValue(StyleKeys.SizeWidthKey, Unit.Auto);
                }


                if (_svgCanvas.Style.IsValueDefined(StyleKeys.SizeHeightKey))
                {
                    height = _svgCanvas.Style.GetValue(StyleKeys.SizeHeightKey, Unit.Auto);
                }

                applied.SetValue(StyleKeys.SizeHeightKey, height);
                applied.SetValue(StyleKeys.SizeWidthKey, width);


                //Push the style and then manually get the full style for layout.

                layoutContext.StyleStack.Push(applied);

                var full = layoutContext.StyleStack.GetFullStyle(this._svgCanvas,
                    new Size(width, height),
                    DoGetParentSize,
                    new Size(Font.DefaultFontSize, Font.DefaultFontSize * 0.5),
                    Font.DefaultFontSize);

                //Create a positioned region within the current page on the current block
                //where all the content will be laid out.

                var pg = layoutContext.DocumentLayout.CurrentPage;
                var open = pg.LastOpenBlock();
                var posRegion = open.BeginNewPositionedRegion(full.CreatePostionOptions(false), pg, this._svgCanvas,
                    full, false, false);

                //Then we can use the default SVG engine and layout the content.

                using (var engine = (this._svgCanvas as IPDFViewPortComponent).GetEngine(null, layoutContext, full))
                {
                    engine.Layout(layoutContext, full);
                }

                //finally we clean up popung the stack
                //removing the positioned region itself from the layout
                //and closing if needed.

                layoutContext.StyleStack.Pop();
                var parent = (PDFLayoutBlock)posRegion.Parent as PDFLayoutBlock;
                parent.PositionedRegions.Remove(posRegion);

                layout = posRegion.Contents[0];


                if (null == layout)
                    throw new PDFLayoutException("Could not find the layout black for the rendered");

                if (!layout.IsClosed)
                    layout.Close();

                this._imgXObjectBBox = new Rect(0, 0, width, height);


            }
            catch (PDFLayoutException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new PDFLayoutException(
                    "Could not bind the canvase for the SVG Image " + (this.SourcePath ?? "[UNKNOWN PATH]"), e);
            }
            finally
            {
                context.StyleStack = prevStack;
                context.Items = prevItems;
            }

            return layout as PDFLayoutBlock;
        }

        private Size DoGetParentSize(IComponent forcomponent, Style withstyle, PositionMode withposition)
        {
            return new Size(SVGCanvas.DefaultWidth, SVGCanvas.DefaultHeight);
        }

        //
        //Render
        //


        public override PDFObjectRef Render(PDFName name, IStreamFilter[] filters, ContextBase context,
            PDFWriter writer)
        {
            var renderContext = (PDFRenderContext)context;

            var imgORef = this.SetUpImage(name, filters, renderContext, writer);

            if (_layoutRef == null)
            {
                this._layoutRef = this.DoRenderLayoutToPDF(name, filters, renderContext, writer);
            }

            this.DoRenderImageMetaData(name, _layoutName, _layoutRef, filters, renderContext, writer);

            this.ReleaseImage(imgORef, renderContext, writer);

            return imgORef;
        }

        protected virtual PDFObjectRef DoRenderLayoutToPDF(PDFName imageName, IStreamFilter[] filters,
            PDFRenderContext context, PDFWriter writer)
        {
            var prevOffset = context.Offset;
            PDFObjectRef oref = null;
            PDFName canvasName = null;

            if (null != _layout)
            {
                try
                {

                    context.Offset = new Point(0.0, 0.0);
                    _layout.PagePosition = Point.Empty;

                    var bounds = _layout.TotalBounds;
                    bounds.Location = Point.Empty;
                    _layout.TotalBounds = bounds;
                    
                    _layout.IsInlineContent = false;



                    oref = _layout.OutputToPDF(context, writer);
                    this._layoutName = AssertGetCanvasRenderName(context, this.Canvas);
                }
                catch (Exception ex)
                {
                    if (context.Conformance == ParserConformanceMode.Lax)
                        context.TraceLog.Add(TraceLevel.Error, "SVG Image",
                            "Could not output the SVG Image as a canvas.", ex);
                    else
                    {
                        throw new Scryber.PDFRenderException(
                            "The SVG Image " + this.SourcePath +
                            " could not be rendered as a canvas. See the inner exception for more details", ex);
                    }
                }
                finally
                {
                    context.Offset = prevOffset;
                }
            }

            return oref;
        }

        #region protected virtual PDFName AssertGetCanvasRenderName(PDFRenderContext context, SVGCanvas canvas)

        /// <summary>
        /// Attempts to extract the name of the PDFName of the shared resource for this images' canvas. If found then it is returned.
        /// If not then an exceptiopn will be thrown - as the name is required to render the do action on this images graphics stream.
        /// </summary>
        /// <param name="context">The current render context, that contains the document with the shared resource</param>
        /// <param name="canvas">The canvas to match for in the resources</param>
        /// <returns>The PDFName (e.g. /canv1) of the shared resource</returns>
        /// <exception cref="PDFRenderException">Thrown if the canvas</exception>
        protected virtual PDFName AssertGetCanvasRenderName(PDFRenderContext context, SVGCanvas canvas)
        {
            Document doc = (Document)context.Document;
            PDFName canvasName = null;
            foreach (var rsrc in doc.SharedResources)
            {
                if (rsrc is PDFLayoutXObjectResource xobj)
                {
                    var renderer = xobj.Renderer;
                    if (renderer != null && renderer.Owner == canvas)
                    {
                        canvasName = renderer.OutputName;
                    }
                }
            }

            if (null == canvasName)
            {
                throw new PDFRenderException(
                    "No resource could be found for the LayoutXObject that matches the referenced image SVG canvas for " +
                    this.SourcePath);
            }

            return canvasName;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageName"></param>
        /// <param name="layoutName"></param>
        /// <param name="layoutRef"></param>
        /// <param name="filters"></param>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        protected virtual void DoRenderImageMetaData(PDFName imageName, PDFName layoutName, PDFObjectRef layoutRef,
            IStreamFilter[] filters,
            PDFRenderContext context, PDFWriter writer)
        {
            //Render the image as a graphics stream 

            var matrixComponents = DoGetCanvasToImageMatrix(context, _svgCanvas, _layout);
            if (null != matrixComponents && matrixComponents.Length == 6)
            {
                for (var i = 0; i < 6; i++)
                {
                    writer.WriteRealS(matrixComponents[i]);
                }
            
                writer.WriteOpCodeS(PDFOpCode.GraphTransformMatrix);
            
                for (var i = 0; i < 6; i++)
                {
                    writer.WriteRealS(matrixComponents[i]);
                }
            
                writer.WriteOpCodeS(PDFOpCode.TxtTransformMatrix);
            }
            
            writer.WriteNameS(layoutName.Value);
            writer.WriteOpCodeS(PDFOpCode.XobjPaint);
            var len = writer.EndStream();

            writer.BeginDictionary();
            writer.WriteDictionaryNameEntry("Type", "XObject");
            writer.WriteDictionaryNameEntry("Subtype", "Form");
            writer.BeginDictionaryEntry("Resources");

            writer.BeginDictionary();
            writer.BeginDictionaryEntry("XObject");

            writer.BeginDictionary();
            writer.WriteDictionaryObjectRefEntry(layoutName.Value, layoutRef);

            writer.EndDictionary();
            writer.EndDictionaryEntry();

            writer.EndDictionary(); //Resource
            writer.EndDictionaryEntry();
            
            
            var bbox = _imgXObjectBBox.Value;
            
            writer.BeginDictionaryEntry("BBox");
            writer.BeginArray();
            writer.WriteRealS(bbox.X.PointsValue);
            writer.WriteRealS(bbox.Y.PointsValue);
            writer.WriteRealS(bbox.Width.PointsValue);
            writer.WriteRealS(bbox.Height.PointsValue);
            writer.EndArray();
            writer.EndDictionaryEntry();

            writer.WriteDictionaryNumberEntry("Length", len);
            writer.EndDictionary(); //XObject

        }

        

    protected virtual double[] DoGetCanvasToImageMatrix(PDFRenderContext context, SVGCanvas canvas,
            PDFLayoutBlock block)
        {
            PDFTransformationMatrix matrix = PDFTransformationMatrix.Identity();
            //matrix.SetTranslation(context.Offset.X, context.Offset.Y);
            var comp = matrix.Components;
            
            return comp;
        }

        /// <summary>
        /// Begins the image XObject and the image stream that will set the transform matrix and call the Canvas XObject do
        /// </summary>
        /// <param name="imageName"></param>
        /// <param name="filters"></param>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        protected virtual PDFObjectRef SetUpImage(PDFName imageName, IStreamFilter[] filters, PDFRenderContext context, PDFWriter writer)
        {
            var imgRef = writer.BeginObject(imageName.Value);
            writer.BeginStream(imgRef, filters);
            
            return imgRef;
        }

        protected virtual void ReleaseImage(PDFObjectRef imgRef, PDFRenderContext context, PDFWriter writer)
        {
            writer.EndObject();
        }
        
        #region IDisposable implementation

        public void Dispose()
        {
            this.Dispose(true);
        }

        ~SVGPDFImageData()
        {
            this.Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                _svgCanvas?.Dispose();
        }
        
        #endregion
        
        
    }

}