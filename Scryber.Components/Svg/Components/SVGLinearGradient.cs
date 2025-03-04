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
    }

    public override PDFBrush CreateBrush(Rect totalBounds)
    {
        var descriptor = this.CreateDescriptor(totalBounds);
        return new PDFGradientLinearBrush(descriptor);
    }


    /// <summary>
    /// Returns a linear gradient descriptor
    /// </summary>
    /// <param name="totalbounds"></param>
    /// <returns></returns>
    protected virtual GradientLinearDescriptor CreateDescriptor(Rect totalbounds)
    {
        Unit x1 = Unit.Zero;
        Unit x2 = new Unit(1.0);
        Unit y1 = Unit.Zero;
        Unit y2 = Unit.Zero;
        
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
        
        //None of the style values are inherited so we should be ok with just collecting the direct values - TBC
        var style = this.GetAppliedStyle();
        
        if (null != style)
        {
            if (style.TryGetValue(StyleKeys.SVGGeometryGradientX1Key, out value))
                x1 = value.Value(this.Style);
            if (style.TryGetValue(StyleKeys.SVGGeometryGradientX2Key, out value))
                x2 = value.Value(this.Style);
            
            if (style.TryGetValue(StyleKeys.SVGGeometryGradientY1Key, out value))
                y1 = value.Value(this.Style);
            if (style.TryGetValue(StyleKeys.SVGGeometryGradientY2Key, out value))
                y2 = value.Value(this.Style);
            
            if (style.TryGetValue(StyleKeys.SVGGeometryGradientSpreadModeKey, out spread))
                mode = spread.Value(this.Style);
            
            if (style.TryGetValue(StyleKeys.SVGGeometryGradientUnitKey, out units))
                type = units.Value(this.Style);
        }

        var calc = CreateGradientCalculator(mode, type, x1, x2, y1, y2);
            
        return calc.CreateDescriptor(this.Stops);
        

        // Scryber.Drawing.GradientLinearDescriptor descriptor = new GradientLinearDescriptor();
        //
        // descriptor.Angle = 0.0; // this.GetGradientAngle(x1, x2, y1, y2, type, out double length, out double maxLen);
        // //get the length of a unit box
        // var maxLen = 1.0;// PDFLinearShadingPattern.GetMaxLengthBoundingBox(new Rect(x1, y1, x2 - x1, y2-y1), descriptor.Angle, out double patternStartOffset, out Point maxStart, out Point maxEnd).PointsValue;
        // var length = 1.0; //TODO calculate the descriptor length // PDFLinearShadingPattern.GetMaxLengthBoundingBox(new Rect(x1, y1, x2 - x1, y2 - y1), descriptor.Angle, out Point actStart, out Point actEnd).PointsValue;
        //
        // if (length <= 0)
        //     length = maxLen;
        //
        // descriptor.Repeating = false; //we add our own stops based on the mode.
        // List<GradientColor> colors = new List<GradientColor>(this.Stops.Count);
        //
        //
        //
        // if (this.Stops.Count > 1)
        // {
        //     int count = 1;
        //     double factor = 1.0;
        //     if (mode == GradientSpreadMode.Repeat)
        //     {
        //         factor = maxLen / length;
        //         count = (int)Math.Ceiling(Math.Abs(length / maxLen));
        //     }
        //     
        //     Unit offset = Unit.Zero;
        //     var total = Unit.Zero;
        //     var index = 0.0;
        //     while (index < count)
        //     {
        //         //Add a first color if the first stop is great than zero.
        //         var first = this.Stops[0];
        //         offset = first.Offset;
        //         if (offset.IsRelative)
        //         {
        //             if (offset.Units == PageUnits.Percent)
        //                 offset = new Unit(offset.Value / 100.0);
        //             else
        //             {
        //                 offset = 0;
        //             }
        //         }
        //
        //         double distance;
        //
        //         if (offset > 0.0)
        //         {
        //             distance = index / count;
        //             colors.Add(new GradientColor(first.StopColor, distance, first.StopOpacity));
        //         }
        //             
        //
        //         foreach (var stop in this.Stops)
        //         {
        //             offset = stop.Offset;
        //             if (offset.IsRelative)
        //             {
        //                 if (offset.Units == PageUnits.Percent)
        //                     offset = new Unit(offset.Value / 100.0);
        //                 else
        //                     continue; //we can only use % relative values
        //             }
        //
        //             
        //             distance = (index + offset.PointsValue) / count;
        //
        //             
        //             
        //             GradientColor color = new GradientColor(stop.StopColor, Math.Min(distance, 1.0), stop.StopOpacity);
        //             colors.Add(color);
        //
        //             if (distance >= 1.0)
        //             {
        //                 index = count;
        //                 break;
        //             }
        //         }
        //
        //         if (mode == GradientSpreadMode.Pad || mode == GradientSpreadMode.Repeat)
        //         {
        //             if (offset.Value < 1.0)
        //             {
        //                 var last = this.Stops[this.Stops.Count - 1];
        //                 distance = (index + 1.0) / count;
        //                 GradientColor lastColor = new GradientColor(last.StopColor, distance, last.StopOpacity);
        //                 colors.Add(lastColor);
        //             }
        //             
        //         }
        //         else if (mode == GradientSpreadMode.Reflect && total < maxLen)
        //         {
        //             //TODO: reverse the order and go again
        //         }
        //         else
        //         {
        //             //add the length to the total, so can we continue on if needed
        //             total += length;
        //         }
        //
        //         index++;
        //     }
        // }
        // else if(this.Stops.Count == 1)
        // {
        //     var stop = this.Stops[0];
        //     GradientColor one = new GradientColor(stop.StopColor, 0, stop.StopOpacity);
        //     colors.Add(one);
        //     GradientColor two = new GradientColor(stop.StopColor, 1.0, stop.StopOpacity);
        //     colors.Add(two);
        // }
        // else
        // {
        //     //No stops defined so we just go black
        //     GradientColor one = new GradientColor(StandardColors.Black, 0, 1.0);
        //     colors.Add(one);
        //     GradientColor two = new GradientColor(StandardColors.Black, 1.0, 1.0);
        //     colors.Add(two);
        // }
        //
        // descriptor.Colors = colors;
        //
        // return descriptor;
        
        
    }

    private SVGLinearGradientCalculator CreateGradientCalculator(GradientSpreadMode mode, GradientUnitType type, Unit x1, Unit x2, Unit y1, Unit y2)
    {
        switch (mode)
        {
            case GradientSpreadMode.Pad:
                return new SVGLinearPaddedGradientCalculator(mode, type, x1, y1, x2, y2);
            
            case GradientSpreadMode.Repeat:
                return new SVGLinearRepeatingGradientCalculator(mode, type, x1, y1, x2, y2);
            
            case GradientSpreadMode.Reflect:
                return new SVGLinearReflectingGradientCalculator(mode, type, x1, y1, x2, y2);
            
            default:
                    throw new NotImplementedException();
                break;
        }
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