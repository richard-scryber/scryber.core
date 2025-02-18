using Scryber.Drawing;
using System;
using Scryber.Svg.Components;

namespace Scryber.Svg.Layout;

/// <summary>
/// Base class for the padded, repeating and reverse calculators that calculate and return the GradientLinearDescriptor to be used by grandent brushes
/// </summary>
public abstract class SVGLinearGradientCalculator
{
    public GradientSpreadMode Mode { get; private set; }
    
    public GradientUnitType UnitType { get; private set; }
    
    public Rect GradientBounds { get; private set; }
    
    public double CalculatedAngle { get; private set; }
    
    

    public SVGLinearGradientCalculator(GradientSpreadMode mode, GradientUnitType unitType, Unit x1, Unit y1,
        Unit x2, Unit y2) : this(mode, unitType, GetAbsoluteRect(x1, y1, x2, y2))
    {
    }
    
    protected SVGLinearGradientCalculator(GradientSpreadMode mode, GradientUnitType units, Rect bounds)
    {
        this.Mode = mode;
        this.UnitType = units;
        this.GradientBounds = bounds;
        
        double hypotenuse, longestSide;
        this.CalculatedAngle = GetGradientAngle(bounds.X, bounds.Y, bounds.Width, bounds.Height, out hypotenuse, out longestSide);
    }


    public virtual GradientLinearDescriptor CreateDescriptor(SVGLinearGradientStopList stops)
    {
        if (null == stops || stops.Count < 2)
            return null;
        else
            return this.DoCreateDescriptor(stops);
    }

    protected abstract GradientLinearDescriptor DoCreateDescriptor(SVGLinearGradientStopList stops);



    private static Rect GetAbsoluteRect(Unit x1, Unit y1, Unit x2, Unit y2)
    {
        x1 = ToNonRelative(x1);
        x2 = ToNonRelative(x2);
        y1 = ToNonRelative(y1);
        y2 = ToNonRelative(y2);
        
        return new Rect(x1, y1, x2 - x1, y2 - y1);

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
    
    private double GetGradientAngle(Unit x, Unit y, Unit width, Unit height, out double hypotenuse, out double longestSide)
    {
        //TODO: consider the type of object bounds and make relative to that when mode == UserSpaceOnUse .
        
        var deltax = width.PointsValue;
        var deltay = height.PointsValue;

        var radians = Math.Atan2(deltay, deltax);
        var degrees = radians * (180.0 / Math.PI);
        hypotenuse = Math.Sqrt((deltax * deltax) + (deltay * deltay));
        
        //zero = vertical
        degrees = degrees + 90;
        
        if (degrees < 0)
            degrees += 360;
        else if (degrees > 360)
            degrees -= 360;

        if (deltax == 0)
        {
            longestSide = 1.0;
        }
        else if (deltay == 0)
        {
            longestSide = 1.0;
        }
        else
        {
            var cos = 1/Math.Cos(radians);
            var sin = 1/Math.Sin(radians);
            longestSide = Math.Min(cos, sin);
        }

        return degrees;
    }
}