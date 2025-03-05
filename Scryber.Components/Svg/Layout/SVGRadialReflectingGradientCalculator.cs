using Scryber.Drawing;
using System;
using Scryber.Svg.Components;

namespace Scryber.Svg.Layout;

public class SVGRadialReflectingGradientCalculator : SVGRadialGradientCalculator
{
    
    
    public SVGRadialReflectingGradientCalculator(GradientSpreadMode mode, GradientUnitType unitType, Point firstCentre, Unit firstRadius, Point secondCentre, Unit secondRadius)
        : base(mode, unitType, firstCentre, firstRadius, secondCentre, secondRadius)
    {
    }

    public override GradientRadialDescriptor CreateDescriptor(SVGGradientStopList stops)
    {
        throw new NotImplementedException();
    }
}