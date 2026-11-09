using Scryber.Drawing;
using System;
using Scryber.Svg.Components;
using System.Collections.Generic;

namespace Scryber.Svg.Layout;

public class SVGRadialReflectingGradientCalculator : SVGRadialGradientCalculator
{
    
    
    public SVGRadialReflectingGradientCalculator(GradientSpreadMode mode, GradientUnitType unitType, Point firstCentre, Unit firstRadius, Point secondCentre, Unit secondRadius)
        : base(mode, unitType, firstCentre, firstRadius, secondCentre, secondRadius)
    {
    }

    public override GradientRadialDescriptor CreateDescriptor(SVGGradientStopList stops)
    {
         var descriptor = new GradientSVGRadialDescriptor(new List<GradientColor>());
        
        descriptor.StartCenter = ToNonRelative(this.FirstCentre);
        descriptor.EndCenter = ToNonRelative(this.SecondCentre);

        descriptor.StartRadius = ToNonRelative(this.FirstRadius);
        descriptor.EndRadius = ToNonRelative(this.SecondRadius);

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

            if (loop % 2 == 0)
                this.AddRepeatStops(descriptor, stops, baseOffset, count);
            else
                this.AddReflectedStops(descriptor, stops, baseOffset, count);
            
            
            
            
            baseOffset += factor;
        }

        descriptor.EndRadius = factor * count;


        return descriptor;
    }

    protected virtual double AddRepeatStops(GradientRadialDescriptor descriptor, SVGGradientStopList stops,
        double baseOffset, double count)

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
        
        return loopOffset;
    }
    
    protected virtual double AddReflectedStops(GradientRadialDescriptor descriptor, SVGGradientStopList stops,
        double baseOffset, double count)

    {
        double loopOffset = 0.0;
        double offset = 0.0;
        Color color = StandardColors.Black;
        double opacity = 1.0;

        for (var i = stops.Count - 1; i >= 0; i--)
        {
            var stop = stops[i];



            color = GetStopColor(stop);
            opacity = GetStopOpacity(stop);

            offset = 1 - GetStopOffset(stop);

            if (i == (stops.Count - 1) && offset > 0.0)
            {
                //pad the start with the stop color
                descriptor.Colors.Add(new GradientColor(color, baseOffset, opacity));
            }

            loopOffset = offset / count;
            loopOffset += baseOffset;

            descriptor.Colors.Add(new GradientColor(color, loopOffset, opacity));
        }

        if (offset > 0.0)
        {
            //we need to pad the gradient out to 1.0
            loopOffset = 1 / count;
            loopOffset += baseOffset;
            descriptor.Colors.Add(new GradientColor(color, loopOffset, opacity));
        }
        
        return loopOffset;
    }
}