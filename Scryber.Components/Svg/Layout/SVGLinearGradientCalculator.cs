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
            CalculateOptimumCoords(bounds, angle, out var tl, out var br);
            var len = CalculateLineLength(tl, br);
            return len;
        }
    }

    /// <summary>
    /// Given the boundary and an angle - we calculate the best 2 coordinates a linear gradient path should go for, to.
    /// NOTE one of the points may be outside of the bounds so that the linear gradient will cover the entire area.
    /// </summary>
    /// <param name="bounds">The bounds of the shape that will be covered with the gradient</param>
    /// <param name="angle">The angle of the gradient in degrees - where zero is horizontal and rotates clockwise as the value increases - using cartesian co-ordinates</param>
    /// <param name="p1">Set to the starting point of the line</param>
    /// <param name="p2">Set to the ending point of the line</param>
    /// <returns>True if the coordinates could be calculated</returns>
    /// <remarks>
    /// We calculate the shortest length of line from a corner of the rectangle bounds (based on the actual angle itself) to a point beyond the
    /// perimiter where a perpendicular line would pass through the opposite corner of the rectangle.
    /// Eg.. For 115 degrees we start on the top right corner and come back on our selves passing the bottom of the bounds until reaching a point below there
    /// where the perpendicular line (angle 25 degrees) would intersect with both the left bottom of the boundsand the gradient line.
    /// </remarks>
    public static bool CalculateOptimumCoords(Rect bounds, double angle, out Point p1, out Point p2)
    {
        while(angle < 0.0)
            angle += 360.0;
        
        while(angle > 360.0)
            angle -= 360.0;
        
        p1 = Point.Empty;
        p2 = Point.Empty;

        double radians = angle * (Math.PI / 180.0);
        double m1 = Math.Tan(radians);
        
        double x1;
        double y1;
        //y = mx + c
        // c = y - mx;
        double c1;
        double x2, y2;
        
        bool result = false;

        if (angle == 360.0 || angle == 0)
        {
            p1 = new Point(bounds.X, bounds.Y);
            p2 = new Point(bounds.X + bounds.Width, bounds.Y);
            result = true;
        }
        else if (angle > 270.0) 
        {
            //between 360 and 270
            //top left corner down to the bottom right.
            p1 = new Point(bounds.X, bounds.Y + bounds.Height);
            x1 = p1.X.PointsValue;
            y1 = p1.Y.PointsValue;
            c1 = y1 - (m1 * x1);
            
            var m2 = -(1 / m1);

            
            //perpendicular line must pass through bottom right corner
            x2 = bounds.X.PointsValue + bounds.Width.PointsValue;
            y2 = bounds.Y.PointsValue;
            
            var c2 = y2 - (m2 * x2);

            double xi = (c2 - c1) / (m1 - m2);
            double yi = (m2 * xi) + c2;
            p2 = new Point(Math.Round(xi, 10), Math.Round(yi, 10));
            
            result = true;
        }
        else if (angle == 270.0)
        {
            p1 = new Point(bounds.X, bounds.Y + bounds.Height);
            p2 = new Point(bounds.X, bounds.Y);
            result = true;
        }
        else if (angle > 180.0)
        {
            //between 270 and 180
            //top right corner down to the bottom left.
            p1 = new Point(bounds.X + bounds.Width, bounds.Y + bounds.Height);
            x1 = p1.X.PointsValue;
            y1 = p1.Y.PointsValue;
            c1 = y1 - (m1 * x1);
            
            var m2 = -(1 / m1);

            
            //perpendicular line must pass through bottom left corner
            x2 = bounds.X.PointsValue;
            y2 = bounds.Y.PointsValue;
            
            var c2 = y2 - (m2 * x2);

            double xi = (c2 - c1) / (m1 - m2);
            double yi = (m2 * xi) + c2;
            p2 = new Point(Math.Round(xi, 10), Math.Round(yi, 10));
            
            result = true;
        }
        else if (angle == 180.0)
        {
            p1 = new Point(bounds.X + bounds.Width, bounds.Y);
            p2 = new Point(bounds.X, bounds.Y);
            result = true;
        }
        else if (angle > 90.0)
        {
            //between 180 and 90
            //bottom right corner up to the top left.
            p1 = new Point(bounds.X + bounds.Width, bounds.Y);
            x1 = p1.X.PointsValue;
            y1 = p1.Y.PointsValue;
            c1 = y1 - (m1 * x1);
            
            var m2 = -(1 / m1);

            
            //perpendicular line must pass through top left corner
            x2 = bounds.X.PointsValue;
            y2 = bounds.Y.PointsValue + bounds.Height.PointsValue;
            
            var c2 = y2 - (m2 * x2);

            double xi = (c2 - c1) / (m1 - m2);
            double yi = (m2 * xi) + c2;
            p2 = new Point(Math.Round(xi, 10), Math.Round(yi, 10));
            
            result = true;

        }
        else if (angle == 90.0)
        {
            p1 = new Point(bounds.X, bounds.Y);
            p2 = new Point(bounds.X, bounds.Y + bounds.Height);
            result = true;
        }
        else //< 90
        {
            //from bottom left corner
            
            x1 = bounds.X.PointsValue;
            y1 = bounds.Y.PointsValue;
            
            c1 = bounds.Y.PointsValue - (m1 * x1);
            //perpendicular line must pass through far right corner
            var m2 = -(1 / m1);
            y2 = bounds.Y.PointsValue + bounds.Height.PointsValue;
            x2 = bounds.X.PointsValue + bounds.Width.PointsValue;

            var c2 = y2 - (m2 * x2) ; //20
            
            
            //calculate the intersection of the two lines.
            double xi, yi;
            //y = m1x + c1
            //y = m2x + c2
            //x = (c2 - c1)/(m1 - m2)
            
            xi = (c2 - c1) / (m1 - m2);
            yi = (m2 * xi) + c2;
            
            

            p1 = new Point(bounds.X, bounds.Y);
            p2 = new Point(Math.Round(xi, 10), Math.Round(yi, 10));
            
            result = true;
        }
        
        return result;
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