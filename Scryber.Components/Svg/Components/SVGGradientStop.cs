using System;
using Scryber.Drawing;
using System.Collections.Generic;
using Scryber.Components;
using Scryber.Styles;

namespace Scryber.Svg.Components;

[PDFParsableComponent("stop")]
public class SVGGradientStop : Component, IStyledComponent, ICloneable
{

    private Style _style;
    private Style _cachedFullStyle;
    
    [PDFAttribute("class")]
    public override string StyleClass { get => base.StyleClass; set => base.StyleClass = value; }

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
    
    

    public SVGGradientStop() : this(ObjectTypes.GradientStop) { }
    

    protected SVGGradientStop(ObjectType type)
        : base(type)
    {
    }

    protected override void OnPreLayout(LayoutContext context)
    {
        base.OnPreLayout(context);
        
        var style = this.GetAppliedStyle();
        
        if(null != style)
            context.StyleStack.Push(style);
        
        this._cachedFullStyle = context.StyleStack.BuildComponentStyle(this);
        
        if(null != style)
            context.StyleStack.Pop();
    }

    public override Style GetAppliedStyle()
    {
        var style = base.GetAppliedStyle();

        return style;
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

    protected override void OnDataBinding(DataContext context)
    {
        base.OnDataBinding(context);
    }
    
    
    public virtual SVGGradientStop Clone()
    {
        var clone = (SVGGradientStop)this.MemberwiseClone();
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




public class SVGGradientStopList : ComponentWrappingList<SVGGradientStop>
{
    public SVGGradientStopList(ComponentList innerList) : base(innerList)
    {
    }
}
