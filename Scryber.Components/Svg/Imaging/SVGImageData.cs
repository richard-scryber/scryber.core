using System;
using Scryber.Drawing;
using Scryber.PDF;
using Scryber.PDF.Layout;
using Scryber.PDF.Native;
using Scryber.Styles;
using Scryber.Svg.Components;

namespace Scryber.Svg.Imaging
{

    public class SVGPDFImageData : ImageData, ILayoutComponent, IBindableComponent
    {
        private PDFObjectRef _renderRef = null;
        private SVGCanvas _svgCanvas = null;
        private PDFLayoutItem _layout = null;
        
        public SVGPDFImageData(string source, SVGCanvas canvas, int w, int h) 
            : base(ObjectTypes.ImageData, source, w, h)
        {
            _svgCanvas = canvas ?? throw new ArgumentNullException(nameof(canvas));
            _svgCanvas.IsDiscreetSVG = true;
        }

       

        public override PDFObjectRef Render(PDFName name, IStreamFilter[] filters, ContextBase context, PDFWriter writer)
        {
            if(_renderRef == null)
                _renderRef = this.DoRenderToPDF(name, filters, context, writer);
            return _renderRef;
        }

        protected virtual PDFObjectRef DoRenderToPDF(PDFName name, IStreamFilter[] filters, ContextBase context, PDFWriter writer)
        {
            var renderContext = (PDFRenderContext)context;
            
            PDFObjectRef oref = null;

            if (null != _layout)
            {
                //oref = _layout.OutputToPDF(renderContext, writer);
            }

            return oref;
        }

        protected PDFLayoutItem LayoutCanvas(PDFLayoutContext layoutContext, Style style)
        {
            using(var engine = (this._svgCanvas as IPDFViewPortComponent).GetEngine(null, layoutContext, style))
            {
                engine.Layout(layoutContext, style);
            }

            return layoutContext.DocumentLayout.CurrentPage.LastOpenBlock();
        }

        public override Size GetSize()
        {
            return base.GetSize();
        }

        public override void ResetFilterCache()
        {
            
        }
        
        

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
        
        //
        // ILayoutComponent
        //

        public event InitializedEventHandler Initialized;
        public event LoadedEventHandler Loaded;
        public void Init(InitContext context)
        {
            this.Document = context.Document;
            if(null != this._svgCanvas)
                _svgCanvas.Init(context);
            
        }

        public void Load(LoadContext context)
        {
            this.Document = context.Document;
            if(null != this._svgCanvas)
                _svgCanvas.Load(context);
        }
        

        public string ID { get; set; }
        public string ElementName { get; set; }
        public IDocument Document { get; private set; }
        public IComponent Parent { get; set; }
        
        
        public string MapPath(string source)
        {
            var service = ServiceProvider.GetService<IPathMappingService>();
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
            if (null == this._layout)
            {
                this._layout = this.DoLayoutCanvas(context, appliedstyle);
            }
            return available;
        }

        public void SetRenderSizes(Rect content, Rect border, Rect total, Style style)
        {
            
        }

        protected virtual PDFLayoutItem DoLayoutCanvas(LayoutContext context, Style style)
        {
            PDFLayoutContext layoutContext = (PDFLayoutContext)context;
            var prevStack = context.StyleStack;
            var prevItems = context.Items;
            PDFLayoutItem layout = null;
            try
            {
                var baseStyle = SVGCanvas.GetDefaultBaseStyle();
                context.StyleStack = new StyleStack(baseStyle);
                context.Items = new ItemCollection(_svgCanvas);

                var pg = layoutContext.DocumentLayout.CurrentPage;
                var open = pg.LastOpenBlock();

                var applied = this._svgCanvas.GetAppliedStyle();
                layoutContext.StyleStack.Push(applied);
                
                var full = layoutContext.StyleStack.GetFullStyle(this._svgCanvas, 
                    new Size(SVGCanvas.DefaultWidth, SVGCanvas.DefaultHeight), 
                    DoGetParentSize, 
                    new Size(Font.DefaultFontSize, Font.DefaultFontSize * 0.5), 
                    Font.DefaultFontSize);
                
                using (var engine = (this._svgCanvas as IPDFViewPortComponent).GetEngine(null, layoutContext, full))
                {
                    engine.Layout(layoutContext, full);
                }
                
                layoutContext.StyleStack.Pop();
                layout = open.CurrentRegion.Contents[open.CurrentRegion.Contents.Count - 1];

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
            return layout;
        }

        private Size DoGetParentSize(IComponent forcomponent, Style withstyle, PositionMode withposition)
        {
            return new Size(SVGCanvas.DefaultWidth, SVGCanvas.DefaultHeight);
        }
        

        public event DataBindEventHandler DataBinding;
        
        
        public event DataBindEventHandler DataBound;
        public void DataBind(DataContext context)
        {
            
            if (null != this._svgCanvas)
            {
                try
                {
                    //Create a new context with an empty data stack for binding
                    context = new DataContext(new ItemCollection(this), context.TraceLog, context.PerformanceMonitor,
                        context.Document, context.Format);
                    
                    this._svgCanvas.DataBind(context);
                }
                catch (PDFDataException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    throw new PDFDataException(
                        "Could not bind the canvase for the SVG Image " + (this.SourcePath ?? "[UNKNOWN PATH]"), e);
                }
            }

        }
    }

}