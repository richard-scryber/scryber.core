using System;
using Scryber.Drawing;
using Scryber.PDF;
using Scryber.PDF.Layout;
using Scryber.PDF.Native;
using Scryber.Styles;
using Scryber.Svg.Components;

namespace Scryber.Svg.Imaging;

/// <summary>
/// Implements the ImageData class for a referenced SVG Image tag.
/// </summary>
public class SVGPDFImageData : ImageVectorData, ILayoutComponent
{
    
    #region ILayoutComponent Properties
    
    private string _id;
    private string _elementName;
    private IDocument _document;
    private IComponent _parent;
    
    public string ID
    {
        get => _id;
        set => _id = value;
    }

    public string ElementName
    {
        get => _elementName;
        set => _elementName = value;
    }

    public IDocument Document => _document;

    public IComponent Parent
    {
        get => _parent;
        set => _parent = value;
    }
    
    #endregion
    
    #region ILayoutComponent Init and Load Implementation
    
    public event InitializedEventHandler Initialized;
    public event LoadedEventHandler Loaded;

    protected virtual void OnInitialized(InitContext context)
    {
        if(null != Initialized)
            this.Initialized(this, new InitEventArgs(context));
    }
    public void Init(InitContext context)
    {
        this.OnInitialized(context);
    }

    protected virtual void OnLoaded(LoadContext context)
    {
        if(null != Loaded)
            this.Loaded(this, new LoadEventArgs(context));
    }

    public void Load(LoadContext context)
    {
        this.OnLoaded(context);
    }
    
    #endregion

    /// <summary>
    /// Gets the complete SVG canvas for this Image.
    /// </summary>
    public SVGCanvas Canvas { get; protected set; }

    /// <summary>
    /// Gets the block that has been laid out for the content of the SVG image
    /// </summary>
    public PDFLayoutBlock LayoutBlock { get; private set; }

    /// <summary>
    /// Flay that returns true if this image has successfully been laid out.
    /// </summary>
    public bool IsLaidOut
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets the image sizer (used to independently calculate sizes of images based on viewboxes, proportions and explicit widths and heights)
    /// </summary>
    public SVGImageDataSizer Sizer
    {
        get; 
        private set;
    }

    /// <summary>
    /// Gets the bounds of the inner XObject
    /// </summary>
    public Rect? ImgXObjectBBox
    {
        get
        {
            if(null == this.Sizer)
                return null;
            else
            {
                return new Rect(Point.Empty, this.Sizer.GetLayoutSize());
            }
        }
    }
    
    /// <summary>
    /// Overrides the base method, to check with the sizer (Or false if no sizer is set)
    /// </summary>
    public override bool AllowLayoutOverflow
    {
        get { return this.Sizer != null ? this.Sizer.AllowLayoutOverflow : false; }
    }

    //
    // .ctor
    //
    
    public SVGPDFImageData(string source, SVGCanvas forCanvas) 
        : this(ObjectTypes.ImageData, source, forCanvas)
    {
    }

    protected SVGPDFImageData(ObjectType type, string source , SVGCanvas forCanvas) 
        : base(type, source)
    {
        this.Canvas = forCanvas ?? throw new System.ArgumentNullException(nameof(forCanvas));
        this.Canvas.IsDiscreetSVG = true;
    }

    //
    // implementation
    //
    
    public bool EnsureLaidOut(Size available, LayoutContext context, Style appliedstyle)
    {
        if (!IsLaidOut)
        {
            this.IsLaidOut = this.DoLayoutCanvas(available, context);
        }

        return IsLaidOut;
    }
    
    public override Size GetSize()
    {
        if(null == this.Sizer)
            return new Size(SVGCanvas.DefaultWidth, SVGCanvas.DefaultHeight);
        else
        {
            return this.Sizer.GetLayoutSize();
        }
    }

    

    public override void ResetFilterCache()
    {
        
    }


    protected virtual bool DoLayoutCanvas(Size available, LayoutContext context)
    {
        var result = false;

        var engine = new SVGPDFImageDataLayoutEngine();

        //always start with a clean applied style, as we are going to render the SVG any outer document styles are applied to the actual image container.
        var appliedstyle = this.Canvas.GetAppliedStyle();
        
        var sizer =  SVGImageDataSizer.CreateSizingStrategy(this.Canvas, appliedstyle, context);
        
        var block = engine.TryLayoutCanvas(this.Canvas, sizer, context, appliedstyle);

        if (block != null)
            result = true;

        this.LayoutBlock = block;
        this.Sizer = sizer;
        
        return result;
    }
    

    public Size GetRequiredSizeForLayout(Size available, LayoutContext context, Style appliedstyle)
    {
        if (!this.IsLaidOut)
        {
            this.IsLaidOut = this.DoLayoutCanvas(available, context);
        }

        // The sizer gives us the SVG's intrinsic size (from viewBox / explicit SVG dims / defaults).
        
        var naturalSize = this.Sizer.GetLayoutSize();
        Size rendered = this.Sizer.GetOutputSizeForLayout(naturalSize, available, appliedstyle, context);
        return rendered;
    }

    public override Rect? GetClippingRect(Point offset, Size available, ContextBase context)
    {
        return this.Sizer.GetClippingRect(offset, available, context);
    }

    public override Size GetRequiredSizeForRender(Point offset, Size available, ContextBase context)
    {
        return this.Sizer.GetRenderScaleForContent(offset, available, context);
    }

    public override Point GetRequiredOffsetForRender(Point offset, Size available, ContextBase context)
    {
        return this.Sizer.GetRenderOffsetForContent(offset, available, context);
    }

    public void SetRenderSizes(Rect content, Rect border, Rect total, Style style)
    {
        
    }
    
    
    public override PDFObjectRef Render(PDFName name, IStreamFilter[] filters, ContextBase context, PDFWriter writer)
    {
        SVGPDFImageDataRenderer renderer = null;
        PDFObjectRef rendererRef = null;
        
        try
        {
            PDFRenderContext renderContext = (PDFRenderContext)context;
            renderer = new SVGPDFImageDataRenderer(this.Canvas, this.LayoutBlock, this.Sizer);
            
            rendererRef = renderer.SetUpImage(name, filters, writer, renderContext);
            
            renderer.OutputToPDF(renderContext);

            renderer.ReleaseImage(name);
        }
        catch (Exception e)
        {
            throw;
        }
        finally
        {
            if (null != renderer)
            {
                renderer.Dispose();
            }
        }

        return rendererRef;
    }
    
    #region ILayoutComponent Map Path Implementation

    /// <summary>
    /// Converts a relative path to an absolute path. As we are discreet we use our source path as the reference,
    /// rather than any parent or documents source path.
    /// </summary>
    /// <param name="source">The path to map</param>
    /// <returns>A full absolute path</returns>
    public string MapPath(string source)
    {
        var service = ServiceProvider.GetService<IPathMappingService>();
        if(service == null) return source;

        var parentPath = this.SourcePath ?? "";
        
        return service.MapPath(ParserLoadType.Generation, source, parentPath, out bool isfile);
    }

    #endregion
    
    #region  IDisposable Support for ILayoutComponent
    
    public void Dispose()
    {
        this.Dispose(true);
    }

    protected virtual void Dispose(bool disposing)
    {
        
    }
    
    #endregion
    
}