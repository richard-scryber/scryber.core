using Scryber.Drawing;
using Scryber.Styles;
using Scryber.PDF.Graphics;

namespace Scryber.Svg.Components;

public class SVGPattern : SVGFillBase, IStyledComponent
{
    
    private Style _style;

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
            this.Style.SetValue(StyleKeys.SizeHeightKey, value);
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
    public Unit X
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
    public Unit Y {
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
    
    

    public SVGPattern() : this(ObjectTypes.GraphicPattern)
    {
        
    }

    protected SVGPattern(ObjectType type) : base(type)
    {
        
    }


    protected virtual Scryber.Drawing.GraphicPatternDescriptor CreateDescriptor(Rect totalBounds)
    {
        return null;
        
    }
    
    public override PDFBrush CreateBrush(Rect totalBounds)
    {
        var descriptor = this.CreateDescriptor(totalBounds);
        return new PDFGraphicPatternBrush(descriptor);
    }
}