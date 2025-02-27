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
        Unit x2, Unit y2) 
    {
        this.Mode = mode;
        this.UnitType = unitType; //Not used
        
        var bounds = GetAbsoluteRect(x1, y1, x2, y2);
        
        double hypotenuse, longestSide;
        this.CalculatedAngle = GetGradientAngle(bounds.X, bounds.Y, bounds.Width, bounds.Height, out hypotenuse, out longestSide);
        this.GradientBounds = GetWrappingRect(bounds);
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
    
    
    /// <summary>
    /// Converts a rect that may have a negative width or height to one that always has a positive width and height (adjusting the x and y positions accordingly)
    /// </summary>
    /// <param name="bounds">The bounds to wrap</param>
    /// <returns></returns>
    private static Rect GetWrappingRect(Rect bounds)
    {
        var wrapped = bounds;

        if (bounds.Width < 0)
        {
            wrapped.X += bounds.Width;
            wrapped.Width = 0 - bounds.Width;
        }

        if (bounds.Height < 0)
        {
            wrapped.Y += bounds.Height;
            wrapped.Height = 0 - bounds.Height;
        }
        return wrapped;
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
    
    protected double GetGradientAngle(Unit x, Unit y, Unit width, Unit height, out double hypotenuse, out double longestSide)
    {
        //TODO: consider the type of object bounds and make relative to that when mode == UserSpaceOnUse .
        
        var deltax = width.PointsValue;
        var deltay = height.PointsValue;

        var radians = Math.Atan2(deltay, deltax);
        var degrees = radians * (180.0 / Math.PI);
        hypotenuse = Math.Sqrt((deltax * deltax) + (deltay * deltay));
        
        //zero = horizontal
        
        while (degrees < 0)
            degrees += 360;
        
        while (degrees > 360)
            degrees -= 360;

        if (deltax == 0)
        {
            longestSide = hypotenuse;
        }
        else if (deltay == 0)
        {
            longestSide = hypotenuse;
        }
        else
        {
            var cos = 1/Math.Cos(radians);
            var sin = 1/Math.Sin(radians);
            longestSide = Math.Max(cos, sin);
        }

        return degrees;
    }

    protected double GetUnitGradientLength(Rect bounds, double angle)
    {
        angle = Math.Round(angle, 2);
        
        while(angle < 0.0)
            angle += 360.0;
        
        while(angle > 360.0)
            angle -= 360.0;
        
        if(angle == 0.0 || angle == 180.0)
            return bounds.Width.PointsValue;
        
        
        if (angle == 90.0 || angle == 270.0 || angle == 0.0 || angle == 180.0)
            return 1.0;
        else if(angle== 45.0 || angle == 315.0 || angle == 135.0 || angle == 225.0)
            return Math.Sqrt(2.0) / 2;
        else
        {
            GradientLinearDescriptor.CalculateOptimumCoords(bounds, angle, out var tl, out var br);
            var len = CalculateLineLength(tl, br);
            return len;
        }
    }

    

    public static double CalculateLineLength(Point pt1, Point pt2)
    {
        var x = pt1.X.PointsValue - pt2.X.PointsValue;
        var y = pt1.Y.PointsValue - pt2.Y.PointsValue;
        var sq = (x * x) + (y * y);
        return Math.Sqrt(sq);
        
    }
    protected Point GetUnitStartPoint(Rect bounds, double angle, double length)
    {
        return new Point(bounds.X + length * Math.Cos(angle), bounds.Y + length * Math.Sin(angle));
    }
}