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

    protected override GradientLinearDescriptor DoCreateDescriptor(SVGLinearGradientStopList stops)
    {
        //The unit length describes a full box length
        var unitLength = PDFLinearShadingPattern.GetMaxLengthBoundingBox(_UnitBounds, this.CalculatedAngle, out double unitStart, 
            out Point unitStartPoint, out Point unitEndPoint).PointsValue;
        
        //The bounding length is the length of the actual gradient based on any prescribed bounds.
        var length  = PDFLinearShadingPattern.GetMaxLengthBoundingBox(this.GradientBounds, this.CalculatedAngle, out double start,
            out Point startPt, out Point endPt).PointsValue;
        
        var max = length + start;
        var factor = length / unitLength;
        
        List<GradientColor> colors = new List<GradientColor>(stops.Count);
        
        var first = stops[0];

        GradientColor color;
        
        if (start > 0.0)
        {
            color = new GradientColor(first.StopColor, 0.0, first.StopOpacity);
            colors.Add(color);
        }
        
        
        double distance = ToNonRelative(first.Offset).PointsValue + start;
        
        
        //padded so we add something from 0 to the first actual stop
        if (distance > 0.0)
        {
            color = new GradientColor(first.StopColor, start, first.StopOpacity);
        }

        foreach (var stop in stops)
        {
            distance = ToNonRelative(stop.Offset).PointsValue;
            distance *= factor;
            distance += start;
            
            color = new GradientColor(stop.StopColor, Math.Min(distance, 1.0), stop.StopOpacity);
            colors.Add(color);
            
            if(distance > max)
                break;
        }

        if (distance < Math.Min(max, 1.0))
        {
            var last = stops[stops.Count - 1];
            color = new GradientColor(last.StopColor, Math.Min(max, 1.0), last.StopOpacity);
            colors.Add(color);
        }

        if (max < 1.0)
        {
            var last = stops[stops.Count - 1];
            color = new GradientColor(last.StopColor, 1.0, last.StopOpacity);
            colors.Add(color);
        }

        return new GradientLinearDescriptor(colors, this.CalculatedAngle);
        
        
    }
}