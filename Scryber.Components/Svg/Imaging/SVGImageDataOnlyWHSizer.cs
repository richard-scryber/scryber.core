using Scryber.Styles;
using Scryber.Drawing;
using Scryber.Svg.Components;

namespace Scryber.Svg.Imaging;

/// <summary>
/// Implements the sizing for an SVG Image Reference when the referenced image has only explicit width and or height.
/// </summary>
public class SVGImageDataOnlyWHSizer : SVGImageDataSizer
{

    public SVGImageDataOnlyWHSizer(SVGCanvas forCanvas, Style appliedStyle, LayoutContext context)
        : base(forCanvas, appliedStyle, context)
    {
    }
}