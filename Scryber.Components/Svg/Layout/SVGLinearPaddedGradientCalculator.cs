using Scryber.Drawing;
using Scryber.Svg.Components;
using System;
using System.Collections.Generic;

namespace Scryber.Svg.Layout;

public class SVGLinearPaddedGradientCalculator : SVGLinearGradientCalculator
{

    public SVGLinearPaddedGradientCalculator(GradientSpreadMode mode, GradientUnitType unitType, Unit x1, Unit y1,
        Unit x2, Unit y2) : base(mode, unitType, x1, y1, x2, y2)
    {

    }

    protected override GradientLinearDescriptor DoCreateDescriptor(SVGLinearGradientStopList stops)
    {
        List<GradientColor> colors = new List<GradientColor>(stops.Count);
        
        var first = stops[0];
        
        double distance = ToNonRelative(first.Offset).PointsValue;
        GradientColor color;
        
        //padded so we add something from 0 to the first actual stop
        if (distance > 0.0)
        {
            color = new GradientColor(first.StopColor, 0.0, first.StopOpacity);
        }

        foreach (var stop in stops)
        {
            distance = ToNonRelative(stop.Offset).PointsValue;
            color = new GradientColor(stop.StopColor, Math.Min(distance, 1.0), stop.StopOpacity);
            colors.Add(color);
            
            if(distance > 1.0)
                break;
        }

        if (distance < 1.0)
        {
            var last = stops[stops.Count - 1];
            color = new GradientColor(last.StopColor, 1.0, last.StopOpacity);
            colors.Add(color);
        }

        return new GradientLinearDescriptor(colors, this.CalculatedAngle);
        
        
    }
}