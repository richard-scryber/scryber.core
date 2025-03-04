using System;
using System.Collections.Generic;
using System.Threading;
using Scryber.Drawing;
using Scryber.PDF.Resources;
using Scryber.Svg.Components;

namespace Scryber.Svg.Layout;


public class SVGLinearReflectingGradientCalculator : SVGLinearGradientCalculator
{

    public SVGLinearReflectingGradientCalculator(GradientSpreadMode mode, GradientUnitType unitType, Unit x1, Unit y1,
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
            if (index % 2 == 0)
                this.AddRepeatingStops(index, repetitions, stops, colors);
            else
                this.AddReflectedStops(index, repetitions, stops, colors);

            index += 1.0;
        }
        
        return new GradientLinearDescriptor(colors, this.CalculatedAngle);

    }

    private void AddRepeatingStops(double index, double repetitions, SVGLinearGradientStopList stops, List<GradientColor> colors)
    {
        var firstStop = stops[0];
        
        double offset = GetStopOffset(firstStop);
        double baseOffset = index / repetitions;
        
        if(offset > 0.0)
            colors.Add(new GradientColor(GetStopColor(firstStop), baseOffset, GetStopOpacity(firstStop)));

        
        foreach (var stop in stops)
        {
            offset = GetStopOffset(stop);
            
            //true offset within the repetitions
            offset = (offset / repetitions) + baseOffset;
            
            colors.Add(new GradientColor(GetStopColor(stop), offset, GetStopOpacity(stop)));
        }

        var repeatMax = (1.0 + index) / repetitions;
        if (offset < repeatMax)
        {
            //we need to pad this gradient to the end of the repetition
            
            var lastStop = stops[stops.Count - 1];
            offset = (1 / repetitions) + baseOffset;
            
            colors.Add(new GradientColor(GetStopColor(lastStop), offset, GetStopOpacity(lastStop)));
        }
    }
    
    private void AddReflectedStops(double index, double repetitions, SVGLinearGradientStopList stops, List<GradientColor> colors)
    {
        var lastStop = stops[stops.Count - 1];
        
        double offset = GetStopOffset(lastStop);
        double baseOffset = index / repetitions;
        double factor = 1 / repetitions; // space for a full loop. E.g. 2 repetitions means the stop loop will take up 1/2 length of the full gradient.
        
        if(offset > 0.0) //we are padding so add the color to zero point.
            colors.Add(new GradientColor(GetStopColor(lastStop), baseOffset, GetStopOpacity(lastStop)));

        for (int i = stops.Count - 1; i >= 0; i--)
        {
            var stop = stops[i];

            //reversing
            offset = 1 - GetStopOffset(stop);
            
            //true offset within the repetitions
            offset = (offset * factor) + baseOffset;
            
            colors.Add(new GradientColor(GetStopColor(stop), offset, GetStopOpacity(stop)));
        }

        var repeatMax = (1.0 + index) / repetitions;
        if (offset < repeatMax)
        {
            //we need to pad this gradient to the end of the repetition
            
            var firstStop = stops[0];
            offset = baseOffset + (1 * factor);
            
            colors.Add(new GradientColor(GetStopColor(firstStop), offset, GetStopOpacity(lastStop)));
        }
    }
}