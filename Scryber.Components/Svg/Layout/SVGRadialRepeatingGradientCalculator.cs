using Scryber.Drawing;
using System;
using Scryber.Svg.Components;
using System.Collections.Generic;

namespace Scryber.Svg.Layout;

public class SVGRadialRepeatingGradientCalculator : SVGRadialGradientCalculator
{
    
    
    public SVGRadialRepeatingGradientCalculator(GradientSpreadMode mode, GradientUnitType unitType, Point firstCentre, Unit firstRadius, Point secondCentre, Unit secondRadius)
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

        double factor;
        if (this.SecondRadius.IsRelative)
        {
            if(this.SecondRadius.Units == PageUnits.Percent)
                factor = this.SecondRadius.Value / 100d;
            else
                factor = this.SecondRadius.Value;
        }
        else
        {
            factor = this.SecondRadius.Value;
        }

        if(factor <= 0.0) // invalid factor, return to default of 0.5
            factor = 0.5d;
        
        
        
        
        var count = Math.Ceiling(1.0 / factor);
        var baseOffset = 0.0;

        for (int loop = 0; loop < count; loop++)
        {
            double loopOffset = 0.0;
            double offset = 0.0;
            Color color = StandardColors.Black;
            double opacity = 1.0;
            
            for (var i = 0; i < stops.Count; i++)
            {
                var stop = stops[i];
                
                
                
                color = GetStopColor(stop);
                opacity = GetStopOpacity(stop);
                
                offset = GetStopOffset(stop);

                if (i == 0 && offset > 0.0)
                {
                    //pad the start with the stop color
                    descriptor.Colors.Add(new GradientColor(color, loopOffset, opacity));
                }
                
                loopOffset = offset / count;
                loopOffset += baseOffset;

                descriptor.Colors.Add(new GradientColor(color, loopOffset, opacity));
            }

            if (offset < 1.0)
            {
                //we need to pad the gradient out to 1.0
                loopOffset = 1 / count;
                loopOffset += baseOffset;
                descriptor.Colors.Add(new GradientColor(color, loopOffset, opacity));
            }
            
            baseOffset += factor;
        }

        descriptor.EndRadius = factor * count;


        return descriptor;
    }
}