using System;
using Scryber.Drawing;
using System.Collections.Generic;
using Scryber.Components;
using Scryber.Styles;

namespace Scryber.Svg.Components;

[PDFParsableComponent("stop")]
public class SVGLinearGradientStop : Component, IStyledComponent, ICloneable
{

    private Style _style;
    private Style _cachedFullStyle;

    [PDFAttribute("style")]
    public Style Style
    {
        get
        {
            if (null == this._style)
                _style = new Style();
            return _style;
        }
    }
    

    public bool HasStyle
    {
        get
        {
            if (null == this._style || this._style.HasValues == false)
                return false;
            else
            {
                return true;
            }
        }
    }
    
    


    [PDFAttribute("offset")]
    public Unit Offset {
        get
        {
            if (this.HasStyle)
                return this.Style.GetValue(StyleKeys.SVGGradientStopOffsetKey, Unit.Zero);
            else
            {
                return Unit.Zero;
            }
        }
        set
        {
            this.Style.SetValue(StyleKeys.SVGGradientStopOffsetKey, value);
        }
    } 
    
    [PDFAttribute("stop-color")]
    public Color StopColor {
        get
        {
            if (this.HasStyle)
                return this.Style.GetValue(StyleKeys.SVGGradientStopColorKey, StandardColors.Black);
            else
            {
                return StandardColors.Black;
            }
        }
        set
        {
            this.Style.SetValue(StyleKeys.SVGGradientStopColorKey, value);
        }
        
    }

    [PDFAttribute("stop-opacity")]
    public double StopOpacity
    {
        get
        {
            if (this.HasStyle)
                return this.Style.GetValue(StyleKeys.SVGGradientStopOpacityKey, 1.0);
            else
            {
                return 1.0;
            }
        }
        set
        {
            this.Style.SetValue(StyleKeys.SVGGradientStopOpacityKey, value);
        }
    }
    
    

    public SVGLinearGradientStop() : this(ObjectTypes.LinearGradientStop)
    {
        
    }

    protected SVGLinearGradientStop(ObjectType type)
        : base(type)
    {
    }

    protected override void OnPreLayout(LayoutContext context)
    {
        base.OnPreLayout(context);
        
        if(this.HasStyle)
            context.StyleStack.Push(this.Style);
        
        this._cachedFullStyle = context.StyleStack.BuildFullStyle(this);
        
        if(this.HasStyle)
            context.StyleStack.Pop();
    }

    public bool TryGetFullStyle(out Style fullStyle)
    {
        if (null == this._cachedFullStyle)
        {
            fullStyle = null;
            return false;
        }
        else
        {
            fullStyle = this._cachedFullStyle;
            return true;
        }
    }

    public virtual SVGLinearGradientStop Clone()
    {
        var clone = (SVGLinearGradientStop)this.MemberwiseClone();
        if (this.HasStyle)
        {
            this.Style.MergeInto(clone.Style);
        }

        return clone;
    }

    
    object ICloneable.Clone()
    {
        return Clone();
    }
    
}

public class SVGLinearGradientStopList : ComponentWrappingList<SVGLinearGradientStop>
{
    public SVGLinearGradientStopList(ComponentList innerList) : base(innerList)
    {
    }
}