using Scryber.Svg.Components;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.Svg.Layout;

public class SVGGradientCalculator
{

    protected double GetStopOffset(SVGGradientStop stop)
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

    protected Color GetStopColor(SVGGradientStop stop)
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
    
    protected double GetStopOpacity(SVGGradientStop stop)
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

    protected static Unit ToNonRelative(Unit unit)
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
    
    protected static Point ToNonRelative(Point pt)
    {
        return new Point(ToNonRelative(pt.X), ToNonRelative(pt.Y));
    }
    
}