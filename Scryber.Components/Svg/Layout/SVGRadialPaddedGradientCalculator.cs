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
        var descriptor = new GradientRadialDescriptor(new List<GradientColor>());
        
        //descriptor.StartCenter = this.ToNonRelative(this.FirstCentre);
        //descriptor.EndCenter = this.ToNonRelative(this.SecondCentre);

        //descriptor.StartRadius = this.ToNonRelative(this.FirstRadius);
        //descriptor.EndRadius = this.ToNonRelative(this.SecondRadius);

        descriptor.Colors.Add(new GradientColor(GetStopColor(stops[0]), 0.0, 1.0));
        descriptor.Colors.Add(new GradientColor(GetStopColor(stops[1]), 1.0, 1.0));


        return descriptor;
    }
}