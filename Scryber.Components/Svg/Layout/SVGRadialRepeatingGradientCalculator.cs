using Scryber.Drawing;
using System;
using Scryber.Svg.Components;

namespace Scryber.Svg.Layout;

public class SVGRadialRepeatingGradientCalculator : SVGRadialGradientCalculator
{
    
    
    public SVGRadialRepeatingGradientCalculator(GradientSpreadMode mode, GradientUnitType unitType, Point firstCentre, Unit firstRadius, Point secondCentre, Unit secondRadius)
        : base(mode, unitType, firstCentre, firstRadius, secondCentre, secondRadius)
    {
    }

    public override GradientRadialDescriptor CreateDescriptor(SVGGradientStopList stops)
    {
        throw new NotImplementedException();
    }
}