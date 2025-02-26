using System;
using System.Collections.Generic;
using System.Threading;
using Scryber.Drawing;
using Scryber.PDF.Resources;
using Scryber.Svg.Components;

namespace Scryber.Svg.Layout;


public class SVGLinearRepeatingGradientCalculator : SVGLinearGradientCalculator
{

    public SVGLinearRepeatingGradientCalculator(GradientSpreadMode mode, GradientUnitType unitType, Unit x1, Unit y1,
        Unit x2, Unit y2) : base(mode, unitType, x1, y1, x2, y2)
    {
        
    }

    protected override GradientLinearDescriptor DoCreateDescriptor(SVGLinearGradientStopList stops)
    {
        List<GradientColor> colors = new List<GradientColor>();
        
        GradientLinearDescriptor.CalculateOptimumCoords(this.GradientBounds, this.CalculatedAngle, out Point start, out Point end);
        var length = GradientLinearDescriptor.GetLength(start, end);
        
        //var length = PDFLinearShadingPattern.GetMaxLengthBoundingBox(this.GradientBounds, this.CalculatedAngle, out double patternStartOffset, out Point start, out Point end);
        
        double scaleFactor = length.PointsValue;
        double repetitions = Math.Ceiling(Math.Abs(1.0 / scaleFactor));

        double index = 0.0;

        //If we are not starting at zero, then push back another repetition
        //TODO: Increate the size of the repetitions until we are beyond 1 then alter the bounds back so we start at the correct place.
        
        while (index < repetitions)
        {
            this.AddRepeatingStops(index, repetitions, stops, colors);

            index += 1.0;
        }
        
        return new GradientLinearDescriptor(colors, this.CalculatedAngle);

    }

    private void AddRepeatingStops(double index, double repetitions, SVGLinearGradientStopList stops, List<GradientColor> colors)
    {
        var firstStop = stops[0];
        double offset = ToNonRelative(firstStop.Offset).PointsValue;
        double baseOffset = index / repetitions;
        
        if(offset > 0.0)
            colors.Add(new GradientColor(firstStop.StopColor, baseOffset, firstStop.StopOpacity));

        
        foreach (var stop in stops)
        {
            offset = ToNonRelative(stop.Offset).PointsValue;
            
            //true offset within the repetitions
            offset = (offset / repetitions) + baseOffset;
            
            colors.Add(new GradientColor(stop.StopColor, offset, stop.StopOpacity));
        }

        var repeatMax = (1.0 + index) / repetitions;
        if (offset < repeatMax)
        {
            //we need to pad this gradient to the end of the repetition
            
            var lastStop = stops[stops.Count - 1];
            offset = (1 / repetitions) + baseOffset;
            
            colors.Add(new GradientColor(lastStop.StopColor, offset, lastStop.StopOpacity));
        }
    }
}