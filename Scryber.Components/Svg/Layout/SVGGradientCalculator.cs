using Scryber.Svg.Components;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.Svg.Layout;

public class SVGGradientCalculator
{

    protected double GetStopOffset(SVGLinearGradientStop stop)
    {
        Unit value;
        if (stop.TryGetFullStyle(out Style style))
        {
            value = style.GetValue(StyleKeys.SVGGradientStopOffsetKey, Unit.Zero);
        }
        else
        {
            value = stop.Offset;
        }

        return ToNonRelative(value).PointsValue;
    }

    protected Color GetStopColor(SVGLinearGradientStop stop)
    {
        Color value;
        if (stop.TryGetFullStyle(out Style style))
        {
            value = style.GetValue(StyleKeys.SVGGradientStopColorKey, StandardColors.Black);
        }
        else
        {
            value = stop.StopColor;
        }
        
        return value;
    }
    
    protected double GetStopOpacity(SVGLinearGradientStop stop)
    {
        Unit value;
        if (stop.TryGetFullStyle(out Style style))
        {
            value = style.GetValue(StyleKeys.SVGGradientStopOffsetKey, 1.0);
        }
        else
        {
            value = stop.Offset;
        }
        
        return ToNonRelative(value).PointsValue;
    }

    protected Unit ToNonRelative(Unit unit)
    {
        if (unit.IsRelative)
        {
            if (unit.Units == PageUnits.Percent)
                unit = new Unit(unit.Value / 100.0);
            else 
                unit = Unit.Zero;
        }
        return unit;
    }
    
}