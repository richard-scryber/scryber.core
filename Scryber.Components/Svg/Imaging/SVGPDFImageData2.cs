using Scryber.Drawing;
using Scryber.PDF;
using Scryber.PDF.Native;
using Scryber.Styles;
using Scryber.Svg.Components;

namespace Scryber.Svg.Imaging;

public class SVGPDFImageData2 : ImageVectorData, ILayoutComponent
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

    public bool IsLaidOut
    {
        get;
        private set;
    }

    //
    // .ctor
    //
    
    public SVGPDFImageData2(string source, SVGCanvas forCanvas) 
        : this(ObjectTypes.ImageData, source, forCanvas)
    {
    }

    protected SVGPDFImageData2(ObjectType type, string source , SVGCanvas forCanvas) 
        : base(type, source)
    {
        this.Canvas = forCanvas ?? throw new System.ArgumentNullException(nameof(forCanvas));
        this.Canvas.IsDiscreetSVG = true;
    }

    public bool EnsureLaidOut(Size available, LayoutContext context, Style appliedstyle)
    {
        if (!IsLaidOut)
        {
            this.IsLaidOut = this.DoLayoutCanvas(available, context, appliedstyle);
        }

        return IsLaidOut;
    }
    
    public override Size GetSize()
    {
        throw new System.NotImplementedException();
    }

    

    public override void ResetFilterCache()
    {
        
    }


    protected virtual bool DoLayoutCanvas(Size available, LayoutContext context, Style appliedstyle)
    {
        var result = false;

        var engine = new SVGPDFImageDataLayoutEngine();
        
        var sizer = new SVGImageDataSizer(this.Canvas, available, appliedstyle, context);
        
        var block = engine.TryLayoutCanvas(this.Canvas, sizer, context, appliedstyle);

        if (block != null)
            result = true;


        return result;
    }
    

    public Size GetRequiredSizeForLayout(Size available, LayoutContext context, Style appliedstyle)
    {
        throw new System.NotImplementedException();
    }

    public void SetRenderSizes(Rect content, Rect border, Rect total, Style style)
    {
        throw new System.NotImplementedException();
    }
    
    
    public override PDFObjectRef Render(PDFName name, IStreamFilter[] filters, ContextBase context, PDFWriter writer)
    {
        throw new System.NotImplementedException();
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