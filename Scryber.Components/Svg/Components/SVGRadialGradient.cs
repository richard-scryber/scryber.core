using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Xml.Schema;
using Scryber.PDF.Graphics;
using Scryber.PDF.Resources;
using Scryber.Styles;
using Scryber.Svg.Layout;

namespace Scryber.Svg.Components;

using Scryber.Drawing;

[PDFParsableComponent("radialGradient")]
public class SVGRadialGradient : SVGFillBase, IStyledComponent, ICloneable
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

    
    /// <summary>
    /// Gets or sets the X Position of the start of the Radial gradient circle. Default is 50%, shape centre
    /// </summary>
    [PDFAttribute("fx")]
    public Unit FirstX 
    {
        get
        {
            if (this.HasStyle)
                return this.Style.GetValue(StyleKeys.SVGGeometryGradientFirstXKey, Unit.Percent(50));
            else
                return  Unit.Percent(50);
        }
        set
        {
            this.Style.SetValue(StyleKeys.SVGGeometryGradientFirstXKey, value);
        } 
    }

    /// <summary>
    /// Gets or sets the X Position of the end of the Radial Gradint circle. Default is 50%, shape centre
    /// </summary>
    [PDFAttribute("cx")]
    public Unit SecondX {
        get
        {
            if (this.HasStyle)
                return this.Style.GetValue(StyleKeys.SVGGeometryGradientSecondXKey, Unit.Percent(50));
            else
                return Unit.Percent(50);
        }
        set
        {
            this.Style.SetValue(StyleKeys.SVGGeometryGradientSecondXKey, value);
        } 
    }

    /// <summary>
    /// Gets or sets the Y Position of the start of the Radial Gradient. Default is 50%, shape centre
    /// </summary>
    [PDFAttribute("fy")]
    public Unit FirstY {
        get
        {
            if (this.HasStyle)
                return this.Style.GetValue(StyleKeys.SVGGeometryGradientFirstYKey, Unit.Percent(50));
            else
                return Unit.Percent(50);
        }
        set
        {
            this.Style.SetValue(StyleKeys.SVGGeometryGradientFirstYKey, value);
        } 
    }

    /// <summary>
    /// Gets or sets the Y position of the end of the Radial Gradient circle. Default is 50%, shape centre
    /// </summary>
    [PDFAttribute("cy")]
    public Unit SecondY{
        get
        {
            if (this.HasStyle)
                return this.Style.GetValue(StyleKeys.SVGGeometryGradientSecondYKey, Unit.Percent(50));
            else
                return Unit.Percent(50);
        }
        set
        {
            this.Style.SetValue(StyleKeys.SVGGeometryGradientSecondYKey, value);
        } 
    }
    
    /// <summary>
    /// Gets or sets the radius of the start of the circle perscribed by the radial gradient such that 0% stop will be at this radius. Default is 0%
    /// </summary>
    [PDFAttribute("fr")]
    public Unit FirstRadius {
        get
        {
            if (this.HasStyle)
                return this.Style.GetValue(StyleKeys.SVGGeometryGradientFirstRadiusKey, Unit.Percent(0));
            else
                return Unit.Percent(0);
        }
        set
        {
            this.Style.SetValue(StyleKeys.SVGGeometryGradientFirstRadiusKey, value);
        } 
    }

    /// <summary>
    /// Gets or sets the radius of the end of the circle perscribed by the radial gradient such that the 100% stop will be at this radius. Default is 100%
    /// </summary>
    [PDFAttribute("r")]
    public Unit SecondRadius{
        get
        {
            if (this.HasStyle)
                return this.Style.GetValue(StyleKeys.SVGGeometryGradientSecondRadiusKey, Unit.Percent(50));
            else
                return Unit.Percent(50);
        }
        set
        {
            this.Style.SetValue(StyleKeys.SVGGeometryGradientSecondRadiusKey, value);
        } 
    }

    [PDFAttribute("spreadMethod")]
    public GradientSpreadMode SpreadMode{
        get
        {
            if (this.HasStyle)
                return this.Style.GetValue(StyleKeys.SVGGeometryGradientSpreadModeKey, GradientSpreadMode.Pad);
            else
                return GradientSpreadMode.Pad;
        }
        set
        {
            this.Style.SetValue(StyleKeys.SVGGeometryGradientSpreadModeKey, value);
        } 
    }
    
    public GraphicFillMode FillMode{
        get
        {
            if (this.HasStyle)
                return this.Style.GetValue(StyleKeys.GraphicFillModeKey, GraphicFillMode.Winding);
            else
                return GraphicFillMode.Winding;
        }
        set
        {
            this.Style.SetValue(StyleKeys.GraphicFillModeKey, value);
        } 
    }
    
    /// <summary>
    /// Not currently supported
    /// </summary>
    [PDFAttribute("gradientUnits")]
    public GradientUnitType GradientUnits{
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
    
    

    private SVGGradientStopList _stops;
    
    [PDFElement("")]
    [PDFArray(typeof(SVGGradientStop))]
    public SVGGradientStopList Stops
    {
        get
        {
            if (null == this._stops)
                this._stops = new SVGGradientStopList(this.InnerContent);
            return this._stops;
        }
    }
    
    public SVGRadialGradient() : this(ObjectTypes.RadialGradient)
    {
    }

    protected SVGRadialGradient(ObjectType type) : base(type)
    {
    }

    public override PDFBrush CreateBrush(Rect totalBounds)
    {
        var descriptor = this.CreateDescriptor(totalBounds);
        return new PDFGradientRadialBrush(descriptor);
    }


    /// <summary>
    /// Returns a linear gradient descriptor
    /// </summary>
    /// <param name="totalbounds"></param>
    /// <returns></returns>
    protected virtual GradientRadialDescriptor CreateDescriptor(Rect totalbounds)
    {
        var fifyPC = Unit.Percent(50);
        Unit cx = fifyPC;
        Unit cy = fifyPC;
        Unit fx = fifyPC;
        Unit fy = fifyPC;

        Unit fr = Unit.Zero;
        Unit cr = fifyPC;
        
        GradientSpreadMode mode = GradientSpreadMode.Pad;
        GradientUnitType type = GradientUnitType.ObjectBoundingBox;
        
        if (!string.IsNullOrEmpty(this.TemplateHref) && this.TemplateHref.StartsWith("url("))
        {
            var href = this.TemplateHref.Substring(4);
            if (href.EndsWith(")"))
            {
                href = href.Substring(0, href.Length - 1);
                var grad = this.FindDocumentComponentById(href) as SVGRadialGradient;
                if (null != grad)
                {
                    fx = grad.FirstX;
                    fy = grad.FirstY;
                    fr = grad.FirstRadius;
                    
                    cx = grad.SecondX;
                    cy = grad.SecondY;
                    cr = grad.SecondRadius;
                    
                    mode = grad.SpreadMode;
                    type = grad.GradientUnits;
                }
            }
        }

        StyleValue<Unit> value;
        StyleValue<GradientSpreadMode> spread;
        StyleValue<GradientUnitType> units;
        
        //None of the style values are inherited so we should be ok with just collecting the direct values - TBC
        var style = this.GetAppliedStyle();
        
        if (null != style)
        {
            if (style.TryGetValue(StyleKeys.SVGGeometryGradientFirstXKey, out value))
                fx = value.Value(this.Style);
            if (style.TryGetValue(StyleKeys.SVGGeometryGradientFirstYKey, out value))
                fy = value.Value(this.Style);
            
            if (style.TryGetValue(StyleKeys.SVGGeometryGradientSecondXKey, out value))
                cx = value.Value(this.Style);
            if (style.TryGetValue(StyleKeys.SVGGeometryGradientSecondYKey, out value))
                cy = value.Value(this.Style);
            
            if (style.TryGetValue(StyleKeys.SVGGeometryGradientFirstRadiusKey, out value))
                fr = value.Value(this.Style);
            if (style.TryGetValue(StyleKeys.SVGGeometryGradientSecondRadiusKey, out value))
                cr = value.Value(this.Style);
            
            if (style.TryGetValue(StyleKeys.SVGGeometryGradientSpreadModeKey, out spread))
                mode = spread.Value(this.Style);
            
            if (style.TryGetValue(StyleKeys.SVGGeometryGradientUnitKey, out units))
                type = units.Value(this.Style);
        }

        var calc = CreateGradientCalculator(mode, type, fx, fy, fr, cx, cy, cr);
            
        return calc.CreateDescriptor(this.Stops);
        

        
        
    }

    private SVGRadialGradientCalculator CreateGradientCalculator(GradientSpreadMode mode, GradientUnitType type, Unit fx, Unit fy, Unit fr, Unit cx, Unit cy, Unit cr)
    {
        switch (mode)
        {
            case GradientSpreadMode.Pad:
                return new SVGRadialPaddedGradientCalculator(mode, type, new Point(fx, fy), fr, new Point (cx, cy), cr);
            
            case GradientSpreadMode.Repeat:
                return new SVGRadialRepeatingGradientCalculator(mode, type, new Point(fx, fy), fr, new Point (cx, cy), cr);
            
            case GradientSpreadMode.Reflect:
                return new SVGRadialReflectingGradientCalculator(mode, type, new Point(fx, fy), fr, new Point (cx, cy), cr);
            
            default:
                    throw new NotSupportedException("The gradient mode " + mode + " is not supported.");
        }
    }
    

    public virtual SVGRadialGradient Clone()
    {
        var clone = (SVGRadialGradient)this.MemberwiseClone();
        
        foreach (var s in this.Stops)
        {
            clone.Stops.Add(s.Clone());
        }

        if (!string.IsNullOrEmpty(this.ID) && null != this.Document)
            clone.ID = this.Document.GetIncrementID(ObjectTypes.LinearGradient);
        
        return clone;

    }

    object ICloneable.Clone()
    {
        return this.Clone();
    }
    
    
}