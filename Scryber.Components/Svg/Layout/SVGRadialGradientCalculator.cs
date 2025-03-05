using Scryber.Drawing;
using System;
using Scryber.Svg.Components;


namespace Scryber.Svg.Layout;

public abstract class SVGRadialGradientCalculator : SVGGradientCalculator
{
    public GradientSpreadMode Mode { get; private set; }
    
    public GradientUnitType UnitType { get; private set; }
    
    public Point FirstCentre { get; private set; }
    
    public Unit FirstRadius { get; private set; }
    
    public Point SecondCentre { get; private set; }
    
    public Unit SecondRadius { get; private set; }
    
    public double CalculatedAngle { get; private set; }


    public SVGRadialGradientCalculator(GradientSpreadMode mode, GradientUnitType unitType, Point firstCentre, Unit firstRadius, Point secondCentre, Unit secondRadius)
    {
        this.Mode = mode;
        this.UnitType = unitType;
        this.FirstCentre = firstCentre;
        this.FirstRadius = firstRadius;
        this.SecondCentre = secondCentre;
        this.SecondRadius = secondRadius;
    }
    
    
    public abstract GradientRadialDescriptor CreateDescriptor(SVGGradientStopList stops);
    
}