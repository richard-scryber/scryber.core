using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Scryber.PDF.Graphics;
using Scryber.Styles;

namespace Scryber.Svg.Components;

using Scryber.Drawing;

[PDFParsableComponent("linearGradient")]
public class SVGLinearGradient : SVGFillBase, IStyledComponent, ICloneable
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

    [PDFAttribute("x1")]
    public Unit X1 
    {
        get
        {
            if (this.HasStyle)
                return this.Style.GetValue(StyleKeys.SVGGeometryGradientX1Key, Unit.Zero);
            else
                return Unit.Zero;
        }
        set
        {
            this.Style.SetValue(StyleKeys.SVGGeometryGradientX1Key, value);
        } 
    }

    [PDFAttribute("x2")]
    public Unit X2 {
        get
        {
            if (this.HasStyle)
                return this.Style.GetValue(StyleKeys.SVGGeometryGradientX2Key, new Unit(1.0));
            else
                return new Unit(1.0);
        }
        set
        {
            this.Style.SetValue(StyleKeys.SVGGeometryGradientX2Key, value);
        } 
    }

    [PDFAttribute("y1")]
    public Unit Y1 {
        get
        {
            if (this.HasStyle)
                return this.Style.GetValue(StyleKeys.SVGGeometryGradientY1Key, Unit.Zero);
            else
                return Unit.Zero;
        }
        set
        {
            this.Style.SetValue(StyleKeys.SVGGeometryGradientY1Key, value);
        } 
    }

    [PDFAttribute("y2")]
    public Unit Y2{
        get
        {
            if (this.HasStyle)
                return this.Style.GetValue(StyleKeys.SVGGeometryGradientY2Key, new Unit(1.0));
            else
                return new Unit(1.0);
        }
        set
        {
            this.Style.SetValue(StyleKeys.SVGGeometryGradientY2Key, value);
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
    
    

    private SVGLinearGradientStopList _stops;
    
    [PDFElement("")]
    [PDFArray(typeof(SVGLinearGradientStop))]
    public SVGLinearGradientStopList Stops
    {
        get
        {
            if (null == this._stops)
                this._stops = new SVGLinearGradientStopList(this.InnerContent);
            return this._stops;
        }
    }
    
    public SVGLinearGradient() : this(ObjectTypes.LinearGradient)
    {
    }

    protected SVGLinearGradient(ObjectType type) : base(type)
    {
        this.X1 = Unit.Zero;
        this.X2 = new Unit(1.0);
        this.Y1 = Unit.Zero;
        this.Y2 = new Unit(1.0);
        this.SpreadMode = GradientSpreadMode.Pad;
        this.GradientUnits = GradientUnitType.ObjectBoundingBox;
    }

    public override PDFBrush CreateBrush(Rect totalBounds)
    {
        var descriptor = this.CreateDescriptor(totalBounds);
        return new PDFGradientLinearBrush(descriptor);
    }


    protected virtual GradientLinearDescriptor CreateDescriptor(Rect totalbounds)
    {
        Unit x1 = Unit.Zero;
        Unit x2 = new Unit(1.0);
        Unit y1 = Unit.Zero;
        Unit y2 = new Unit(1.0);
        
        GradientSpreadMode mode = GradientSpreadMode.Pad;
        GradientUnitType type = GradientUnitType.ObjectBoundingBox;
        
        if (!string.IsNullOrEmpty(this.TemplateHref) && this.TemplateHref.StartsWith("url("))
        {
            var href = this.TemplateHref.Substring(4);
            if (href.EndsWith(")"))
            {
                href = href.Substring(0, href.Length - 1);
                var grad = this.FindDocumentComponentById(href) as SVGLinearGradient;
                if (null != grad)
                {
                    x1 = grad.X1;
                    x2 = grad.X2;
                    y1 = grad.Y1;
                    y2 = grad.Y2;
                    mode = grad.SpreadMode;
                    type = grad.GradientUnits;
                }
            }
        }

        StyleValue<Unit> value;
        StyleValue<GradientSpreadMode> spread;
        StyleValue<GradientUnitType> units;
        
        if (this.HasStyle)
        {
            if (this.Style.TryGetValue(StyleKeys.SVGGeometryGradientX1Key, out value))
                x1 = value.Value(this.Style);
            if (this.Style.TryGetValue(StyleKeys.SVGGeometryGradientX2Key, out value))
                x2 = value.Value(this.Style);
            
            if (this.Style.TryGetValue(StyleKeys.SVGGeometryGradientY1Key, out value))
                y1 = value.Value(this.Style);
            if (this.Style.TryGetValue(StyleKeys.SVGGeometryGradientY2Key, out value))
                y2 = value.Value(this.Style);
            
            if (this.Style.TryGetValue(StyleKeys.SVGGeometryGradientSpreadModeKey, out spread))
                mode = spread.Value(this.Style);
            
            if (this.Style.TryGetValue(StyleKeys.SVGGeometryGradientUnitKey, out units))
                type = units.Value(this.Style);
        }

        Scryber.Drawing.GradientLinearDescriptor descriptor = new GradientLinearDescriptor();
        descriptor.Angle = this.GetGradientAngle(x1, x2, y1, y2, type, totalbounds);
        descriptor.Repeating = (mode == GradientSpreadMode.Repeat);
        List<GradientColor> colors = new List<GradientColor>(this.Stops.Count);
        
        if (this.Stops.Count > 1)
        {
            
            Unit offset = Unit.Zero;
            
            
            foreach (var stop in this.Stops)
            {
                offset = stop.Offset;
                if (offset.IsRelative)
                {
                    if (offset.Units == PageUnits.Percent)
                        offset = new Unit(offset.Value / 100.0);
                    else
                        continue; //we can only use % relative values
                }

                GradientColor color = new GradientColor(stop.StopColor, offset.PointsValue, stop.StopOpacity);
                colors.Add(color);
            }

            if (offset.Value < 1.0)
            {
                var last = this.Stops[this.Stops.Count - 1];
                GradientColor lastColor = new GradientColor(last.StopColor, 1.0, last.StopOpacity);
                colors.Add(lastColor);
            }
        }
        else
        {
            var stop = this.Stops[0];
            GradientColor one = new GradientColor(stop.StopColor, 0, stop.StopOpacity);
            colors.Add(one);
            GradientColor two = new GradientColor(stop.StopColor, 1.0, stop.StopOpacity);
            colors.Add(two);
        }
        
        descriptor.Colors = colors;

        return descriptor;
        
        
    }

    private double GetGradientAngle(Unit x1, Unit x2, Unit y1, Unit y2, GradientUnitType type, Rect bounds)
    {
        //TODO: consider the type of object bounds and make relative to that.
        
        if (x1.IsRelative)
        {
            if (x1.Units == PageUnits.Percent)
                x1 = new Unit(x1.Value / 100.0);
            else
            {
                x1 = Unit.Zero;
            }
        }
        
        if (x2.IsRelative)
        {
            if (x1.Units == PageUnits.Percent)
                x1 = new Unit(x1.Value / 100.0);
            else
            {
                x1 = Unit.Zero;
            }
        }
        if (y1.IsRelative)
        {
            if (x1.Units == PageUnits.Percent)
                x1 = new Unit(x1.Value / 100.0);
            else
            {
                x1 = Unit.Zero;
            }
        }
        if (y2.IsRelative)
        {
            if (x1.Units == PageUnits.Percent)
                x1 = new Unit(x1.Value / 100.0);
            else
            {
                x1 = Unit.Zero;
            }
        }
        
        
        var deltax = x2 - x1;
        var deltay = y2 - y1;

        var radians = Math.Atan2(deltay.Value, deltax.Value);
        var degrees = radians * (180.0 / Math.PI);
        
        //zero = vertical
        return degrees + 90;
    }


    public virtual SVGLinearGradient Clone()
    {
        var clone = (SVGLinearGradient)this.MemberwiseClone();
        
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