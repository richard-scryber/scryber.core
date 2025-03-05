using Scryber.Drawing;
using Scryber.Svg.Components;
using System;
using System.Collections.Generic;
using Scryber.PDF.Resources;

namespace Scryber.Svg.Layout;

public class SVGLinearPaddedGradientCalculator : SVGLinearGradientCalculator
{

    public SVGLinearPaddedGradientCalculator(GradientSpreadMode mode, GradientUnitType unitType, Unit x1, Unit y1,
        Unit x2, Unit y2) : base(mode, unitType, x1, y1, x2, y2)
    {

    }
    
    private static Rect _UnitBounds = new Rect(0, 0, 1, 1);

    protected override GradientLinearDescriptor DoCreateDescriptor(SVGGradientStopList stops)
    {
        //The unit length describes a full box length
        var angle = this.CalculatedAngle;
        GradientLinearDescriptor.CalculateOptimumCoords(_UnitBounds, angle, out Point unitStartPoint, out Point unitEndPoint);
        var unitLength = GradientLinearDescriptor.GetLength(unitStartPoint, unitEndPoint).PointsValue;
        
        //var unitLength = PDFLinearShadingPattern.GetMaxLengthBoundingBox(_UnitBounds, this.CalculatedAngle, out double unitStart, 
        //    out Point unitStartPoint, out Point unitEndPoint).PointsValue;
        
        //The bounding length is the length of the actual gradient based on any prescribed bounds.
        GradientLinearDescriptor.CalculateOptimumCoords(this.GradientBounds, angle, out Point startPoint, out Point endPoint);
        var length = GradientLinearDescriptor.GetLength(startPoint, endPoint).PointsValue;
        
        // var length  = PDFLinearShadingPattern.GetMaxLengthBoundingBox(this.GradientBounds, this.CalculatedAngle, out double start,
        //     out Point startPt, out Point endPt).PointsValue;

        var patternStartX = startPoint.X - unitStartPoint.X;
        var patternStartY = startPoint.Y - unitStartPoint.Y;
        var patternStart = GetPatternStart(unitStartPoint, startPoint, angle);
        
        var max = length + patternStart;
        var factor = length / unitLength;
        
        List<GradientColor> colors = new List<GradientColor>(stops.Count);
        
        var first = stops[0];

        GradientColor color;
        
        if (patternStart > 0.0)
        {
            color = new GradientColor(GetStopColor(first), 0.0, GetStopOpacity(first));
            colors.Add(color);
        }


        double distance = GetStopOffset(first);// ToNonRelative(first.Offset).PointsValue + patternStart;
        
        
        //padded so we add something from 0 to the first actual stop
        if (distance > 0.0)
        {
            color = new GradientColor(GetStopColor(first), patternStart, GetStopOpacity(first));
        }

        foreach (var stop in stops)
        {
            distance = GetStopOffset(stop); // ToNonRelative(stop.Offset).PointsValue;
            distance *= factor;
            distance += patternStart;
            
            color = new GradientColor(GetStopColor(stop), Math.Min(distance, 1.0), GetStopOpacity(stop));
            colors.Add(color);
            
            if(distance > max)
                break;
        }

        if (distance < Math.Min(max, 1.0))
        {
            var last = stops[stops.Count - 1];
            color = new GradientColor(GetStopColor(last), Math.Min(max, 1.0), GetStopOpacity(last));
            colors.Add(color);
        }

        if (max < 1.0)
        {
            var last = stops[stops.Count - 1];
            color = new GradientColor(GetStopColor(last), 1.0, GetStopOpacity(last));
            colors.Add(color);
        }

        return new GradientLinearDescriptor(colors, this.CalculatedAngle);
        
    }

    protected double GetPatternStart(Point unitStartPoint, Point startPoint, double angle)
    {
        if (startPoint == unitStartPoint)
            return 0.0;
        else
        {
            var maxX = Unit.Max(unitStartPoint.X, startPoint.X);
            var maxY = Unit.Max(unitStartPoint.Y, startPoint.Y);
            var minX = Unit.Min(unitStartPoint.X, startPoint.X);
            var minY = Unit.Min(unitStartPoint.Y, startPoint.Y);
            
            var x = (maxX - minX).PointsValue;
            var y = (maxY - minY).PointsValue;
            return Math.Sqrt(x * x + y * y);
        }
    }
}