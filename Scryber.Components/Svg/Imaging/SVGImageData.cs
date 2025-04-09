using System;
using System.Runtime.CompilerServices;
using Scryber.Components;
using Scryber.Drawing;
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
        private SVGCanvas _svgCanvas = null;
        private PDFLayoutBlock _layout = null;

        /// <summary>
        /// Gets the loaded canvas for this SVG Image data
        /// </summary>
        public SVGCanvas Canvas
        {
            get { return _svgCanvas; }
        }

        /// <summary>
        /// Gets the pdf layout associated with this SVG Image data (if any)
        /// </summary>
        public PDFLayoutBlock Layout
        {
            get{ return _layout; }
        }
        
        
        #region ILayoutComponent properties
        
        public string ID { get; set; }
        public string ElementName { get; set; }
        public IDocument Document { get; private set; }
        public IComponent Parent { get; set; }
        
        #endregion
        
        public SVGPDFImageData(string source, SVGCanvas canvas, int w, int h) 
            : base(ObjectTypes.ImageData, source, w, h)
        {
            _svgCanvas = canvas ?? throw new ArgumentNullException(nameof(canvas));
            _svgCanvas.IsDiscreetSVG = true;
        }

       
       
        public override PDFObjectRef Render(PDFName name, IStreamFilter[] filters, ContextBase context, PDFWriter writer)
        {
            var renderContext = (PDFRenderContext)context;
            
            var imgRef = this.SetUpImage(name, filters, renderContext, writer);
            
            if (_layoutRef == null)
            {
                this._layoutRef = this.DoRenderLayoutToPDF(name, filters, renderContext, writer);
            }
            
            this.DoRenderImageToPDF(name, _layoutName, _layoutRef, filters, renderContext, writer);
            
            this.ReleaseImage(imgRef, renderContext, writer);
            
            return imgRef;
        }

        protected virtual PDFObjectRef DoRenderLayoutToPDF(PDFName imageName, IStreamFilter[] filters, PDFRenderContext context, PDFWriter writer)
        {
            var  prevOffset = context.Offset;
            PDFObjectRef oref = null;
            PDFName canvasName = null;
            
            if (null != _layout)
            {
                try
                {
                    
                    context.Offset = new Point(0.0, 0.0);
                    _layout.PagePosition = Point.Empty;
                    
                    //Removing the clipping
                    //_layout.Position.OverflowAction = OverflowAction.None;
                    _layout.Position.ClipInset = Thickness.Empty();
                    
                    var bounds = _layout.TotalBounds;
                    bounds.Location = Point.Empty;
                    _layout.TotalBounds = bounds;

                    Document doc = (Document)context.Document;

                    //Take the positioned region from the block and render that instead?
                    oref = _layout.OutputToPDF(context, writer);

                    foreach (var rsrc in doc.SharedResources)
                    {
                        if (rsrc is PDFLayoutXObjectResource xobj)
                        {
                            var renderer = xobj.Renderer;
                            if (renderer != null && renderer.Owner == this._svgCanvas)
                            {
                                canvasName = renderer.OutputName;
                            }
                        }
                    }

                    if (null != canvasName)
                    {
                        this._layoutName = canvasName;
                    }
                    else
                    {
                        throw new PDFRenderException(
                            "No resource could be found for the LayoutXObject that matches the referenced image SVG canvas for " +
                            this.SourcePath);
                    }
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

        protected virtual void DoRenderImageToPDF(PDFName imageName, PDFName layoutName, PDFObjectRef layoutRef, IStreamFilter[] filters,
            PDFRenderContext context, PDFWriter writer)
        {
            //Render the image as a graphics stream 
            
            
            //TODO: Calculate the transformation matrix
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
            var len =writer.EndStream();
            
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
            
            writer.BeginDictionaryEntry("BBox");
            writer.BeginArray();
            writer.WriteRealS(0.0);
            writer.WriteRealS(0.0);
            writer.WriteRealS(context.Space.Width.PointsValue);
            writer.WriteRealS(context.Space.Height.PointsValue);
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

        public override Size GetSize()
        {
            if (null != this.Canvas)
            {
                var sz = new Size(this.Canvas.Width, this.Canvas.Height);
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
            if(null != this.Loaded)
                this.Loaded(this, new LoadEventArgs(context));
        }

        
        public void Init(InitContext context)
        {
            this.Document = context.Document;
            if(null != this._svgCanvas)
                _svgCanvas.Init(context);
            
            this.OnInitialized(context);
            
        }

        public void Load(LoadContext context)
        {
            this.Document = context.Document;
            if(null != this._svgCanvas)
                _svgCanvas.Load(context);
            
            this.OnLoaded(context);
        }
        
        
        #endregion
        
        public string MapPath(string source)
        {
            var service = ServiceProvider.GetService<IPathMappingService>();
            if (null == service) return source;
            
            
            bool isfile = false;
            
            if (!string.IsNullOrEmpty(this.SourcePath))
            {
                return service.MapPath(ParserLoadType.Generation,source, this.SourcePath, out isfile);
            }
            else
            {
                return service.MapPath(ParserLoadType.Generation, source, string.Empty, out isfile);
            }
        }

        public Size GetRequiredSizeForLayout(Size available, LayoutContext context, Style appliedstyle)
        {
            var newSize = available;
            
            if (appliedstyle.TryGetValue(StyleKeys.SizeWidthKey, out var width))
            {
                newSize.Width = width.Value(appliedstyle);
                if (appliedstyle.TryGetValue(StyleKeys.SizeHeightKey, out var height))
                {
                    newSize.Height = height.Value(appliedstyle);
                }
                else
                {
                    //TODO: Scale height Proportionally
                }
            }
            else if (appliedstyle.TryGetValue(StyleKeys.SizeHeightKey, out var height))
            {
                //TODO: Scale width proportionally.
            }

            if (null == this._layout)
            {
                this._layout = DoLayoutCanvas(context, appliedstyle);
            }
            
            return newSize;
        }
        
        public override Size GetRequiredSizeForRender(Size available, ContextBase context)
        {
            var orig = this.GetSize();
            
            //We are rendering 1: 1 from the Image to the Canvas, so we now scale by the ratio.
            var scaleX = available.Width.PointsValue / orig.Width.PointsValue;
            var scaleY = available.Height.PointsValue / orig.Height.PointsValue;
            available = new Size(scaleX, scaleY);
            
            return available;
        }
        
        public override Point GetRequiredOffsetForRender(Point offset, ContextBase context)
        {
            var renderContext = (PDF.PDFRenderContext)context;
            var orig = this.GetSize();
            var scaleY = renderContext.Space.Height.PointsValue / orig.Height.PointsValue;
            offset.Y += renderContext.Space.Height;
            return offset;
        }

        public void SetRenderSizes(Rect content, Rect border, Rect total, Style style)
        {
            
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
                
                //Push the style and then manually get the full style for layout.
                
                layoutContext.StyleStack.Push(applied);
                
                var full = layoutContext.StyleStack.GetFullStyle(this._svgCanvas, 
                    new Size(SVGCanvas.DefaultWidth, SVGCanvas.DefaultHeight), 
                    DoGetParentSize, 
                    new Size(Font.DefaultFontSize, Font.DefaultFontSize * 0.5), 
                    Font.DefaultFontSize);
                
                //Create a positioned region within the current page on the current block
                //where all the content will be laid out.
                
                var pg = layoutContext.DocumentLayout.CurrentPage;
                var open = pg.LastOpenBlock();
                var posRegion = open.BeginNewPositionedRegion(full.CreatePostionOptions(false), pg, this._svgCanvas, full, false, false);
                
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
                
                
                if(null == layout)
                    throw new PDFLayoutException("Could not find the layout black for the rendered");

                if (!layout.IsClosed)
                    layout.Close();
                
                //bool updateSize = true;
                //open.CurrentRegion.RemoveItem(layout, updateSize);

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