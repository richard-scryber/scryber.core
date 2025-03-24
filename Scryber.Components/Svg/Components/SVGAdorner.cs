using Scryber.PDF.Graphics;
using Scryber.PDF.Native;
using Scryber.Drawing;

namespace Scryber.Svg.Components;

/// <summary>
/// Base class for SVG components that adorn (enhance) existing components
/// </summary>
public abstract class SVGAdorner : Scryber.Components.ContainerComponent, IPathAdorner
{

    public SVGAdorner(ObjectType type) : base(type)
    {
        
    }
    
    public abstract AdornmentOrientationValue Orientation { get; set; }

    public abstract PDFName OutputAdornment(PDFGraphics toGraphics, PathAdornmentInfo info, ContextBase context);
}