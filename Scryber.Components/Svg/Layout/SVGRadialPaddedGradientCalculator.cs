using Scryber.Drawing;
using System;
using System.Collections.Generic;
using Scryber.Svg.Components;

namespace Scryber.Svg.Layout;

public class SVGRadialPaddedGradientCalculator : SVGRadialGradientCalculator
{
    
    
    public SVGRadialPaddedGradientCalculator(GradientSpreadMode mode, GradientUnitType unitType, Point firstCentre, Unit firstRadius, Point secondCentre, Unit secondRadius)
        : base(mode, unitType, firstCentre, firstRadius, secondCentre, secondRadius)
    {
    }

    public override GradientRadialDescriptor CreateDescriptor(SVGGradientStopList stops)
    {
        var descriptor = new GradientSVGRadialDescriptor(new List<GradientColor>());
        
        descriptor.StartCenter = this.ToNonRelative(this.FirstCentre);
        descriptor.EndCenter = this.ToNonRelative(this.SecondCentre);

        descriptor.StartRadius = this.ToNonRelative(this.FirstRadius);
        descriptor.EndRadius = this.ToNonRelative(this.SecondRadius);


        for (var i = 0; i < stops.Count; i++)
        {
            var stop = stops[i];
            var color = GetStopColor(stop);
            var offset = GetStopOffset(stop);
            var opacity = GetStopOpacity(stop);
            
            descriptor.Colors.Add(new GradientColor(color, offset, opacity));
        }


        return descriptor;
    }
}