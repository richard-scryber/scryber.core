using Scryber.Drawing;
using Scryber.Styles;
using Scryber.Components;
using Scryber.PDF;
using Scryber.PDF.Graphics;
using Scryber.PDF.Resources;
using Scryber.Svg.Layout;

namespace Scryber.Svg.Components;

[PDFParsableComponent("pattern")]
public class SVGPattern : SVGFillBase, IStyledComponent, IPDFViewPortComponent
{
    
    private Style _style;
    private SVGCanvas _svgCanvas;

    [PDFAttribute("style")]
    public Style Style
    {
        get
        {
            if (null == this._style)
                this._style = new Style();
            return this._style;
        }
    }

    public bool HasStyle
    {
        get
        {
            return null != this._style && this._style.HasValues;
        }
    }

    [PDFAttribute("class")]
    public override string StyleClass 
    { 
        get => base.StyleClass; 
        set => base.StyleClass = value;
    }

    [PDFAttribute("height")]
    public Unit PatternHeight
    {
        get
        {
            if (this.HasStyle)
                return this.Style.GetValue(StyleKeys.SizeHeightKey, Unit.Zero);
            else
            {
                return Unit.Zero;
            }
        }
        set
        {
            this.Style.SetValue(StyleKeys.SizeHeightKey, value);
        }
        
    }
    
    [PDFAttribute("width")]
    public Unit PatternWidth
    {
        get
        {
            if (this.HasStyle)
                return this.Style.GetValue(StyleKeys.SizeWidthKey, Unit.Zero);
            else
            {
                return Unit.Zero;
            }
        }
        set
        {
            this.Style.SetValue(StyleKeys.SizeWidthKey, value);
        }
        
    }
    
    
    #region public PDFRect ViewBox {get; set;}

    [PDFAttribute("viewBox")]
    public Rect ViewBox
    {
        get
        {
            if (this.HasStyle)
                return this.Style.GetValue(StyleKeys.PositionViewPort, Rect.Empty);
            else
                return Rect.Empty;
        }
        set
        {
            this.Style.SetValue(StyleKeys.PositionViewPort, value);
        }
    }

    public void RemoveViewBox()
    {
        this.Style.RemoveValue(StyleKeys.PositionViewPort);
    }

    #endregion

    [PDFAttribute("x")]
    public Unit OffsetX
    {
        get
        {
            if (this.HasStyle)
                return this.Style.GetValue(StyleKeys.SVGGeometryXKey, Unit.Zero);
            else
                return Unit.Zero;
        }
        set
        {
            this.Style.SetValue(StyleKeys.SVGGeometryXKey, value);
        } 
    }
    
    [PDFAttribute("y")]
    public Unit OffsetY {
        get
        {
            if (this.HasStyle)
                return this.Style.GetValue(StyleKeys.SVGGeometryYKey, Unit.Zero);
            else
                return Unit.Zero;
        }
        set
        {
            this.Style.SetValue(StyleKeys.SVGGeometryYKey, value);
        } 
    }
    
    /// <summary>
    /// Not currently supported
    /// </summary>
    [PDFAttribute("patternUnits")]
    public GradientUnitType PatternUnits{
        get
        {
            if (this.HasStyle)
                return this.Style.GetValue(StyleKeys.SVGGeometryGradientUnitKey, GradientUnitType.ObjectBoundingBox);
            else
                return GradientUnitType.ObjectBoundingBox;
        }
        set
        {
            this.Style.SetValue(StyleKeys.SVGGeometryGradientUnitKey, value);
        } 
    }

    [PDFAttribute("href")]
    public string TemplateHref { get; set; }
    
    /// <summary>
    /// Not currently supported
    /// </summary>
    [PDFAttribute("patternContentUnits")]
    public GradientUnitType PatternContentUnits{
        get
        {
            if (this.HasStyle)
                return this.Style.GetValue(StyleKeys.SVGGeometryGradientUnitKey, GradientUnitType.ObjectBoundingBox);
            else
                return GradientUnitType.ObjectBoundingBox;
        }
        set
        {
            this.Style.SetValue(StyleKeys.SVGGeometryGradientUnitKey, value);
        } 
    }
    
    private GradientUnitType _patternContentUnits = GradientUnitType.ObjectBoundingBox;


    [PDFElement("")]
    [PDFArray(typeof(Component))]
    public ComponentList Contents
    {
        get { return this._svgCanvas.Contents; }
    }

    
    /// <summary>
    /// Gets the inner canvas to draw contents of the pattern on.
    /// </summary>
    public SVGCanvas InnerCanvas
    {
        get
        {
            return this._svgCanvas;
        }
    }
    
    
    

    public string XObjectRendererKey
    {
        get;
        set;
    }

    public SVGPattern() : this(ObjectTypes.GraphicPattern)
    {
        
    }

    protected SVGPattern(ObjectType type) : base(type)
    {
        this._svgCanvas = new SVGCanvas();
    }

    

    private PDFGraphicTilingPattern _tilingPattern = null;
    private Style _fullStyle = null;
    private int tileIndex = 1;

    

    
    
    
    /// <summary>
    /// Overrides the base method to set up the pattern
    /// </summary>
    /// <param name="totalBounds"></param>
    /// <returns></returns>
    public override PDFBrush CreateBrush(Rect totalBounds)
    {

        var descriptorKey = GraphicTilingPatternDescriptor.GetResourceKey(this.UniqueID);
        var layoutKey = PDFPatternLayoutResource.GetLayoutResourceKey(this.UniqueID);
        
        return new PDFGraphicPatternBrush(this, descriptorKey, layoutKey);
    }
    
    //
    // event and lifecycle overrides to pass through to the canvas
    // 
        
    protected override void DoInitChildren(InitContext context)
    {
        this._svgCanvas.Init(context);
        base.DoInitChildren(context);
    }

    protected override void DoLoadChildren(LoadContext context)
    {
        this._svgCanvas.Load(context);
        base.DoLoadChildren(context);
    }
    

    protected override void DoDataBindChildren(DataContext context)
    {
        this._svgCanvas.DataBind(context);
        
        base.DoDataBindChildren(context);
    }

    

    protected override void DoCloseLayoutArtefacts(PDFLayoutContext context, PDFArtefactRegistrationSet artefacts, Style fullstyle)
    {
        this._svgCanvas.CloseLayoutArtefacts(context, artefacts, fullstyle);
        base.DoCloseLayoutArtefacts(context, artefacts, fullstyle);
    }

    public IPDFLayoutEngine GetEngine(IPDFLayoutEngine parent, PDFLayoutContext context, Style fullstyle)
    {
        this._fullStyle = fullstyle;
        this._svgCanvas.ID = this.ID;
        
        
        return new LayoutEngineSVGPattern(parent, this, this._svgCanvas, context);
    }
}