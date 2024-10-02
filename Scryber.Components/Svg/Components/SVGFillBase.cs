using Scryber.PDF.Graphics;
using Scryber.Drawing;
namespace Scryber.Svg.Components;

/// <summary>
/// Base class for the linear gradient, radial gradient and pattern elements in SVG
/// </summary>
public abstract class SVGFillBase : Scryber.Components.ContainerComponent
{
    
    public SVGFillBase(ObjectType type): base(type)
    {}


    public abstract PDFBrush CreateBrush(Rect totalBounds);

}