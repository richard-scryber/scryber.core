using Scryber.Styles;
using Scryber.Drawing;
using Scryber.Svg.Components;

namespace Scryber.Svg.Imaging;

/// <summary>
/// Implements the sizing for an SVG Image Reference when the referenced image has both a ViewPort and explicit width and or height.
/// </summary>
public class SVGImageDataVPAndWHSizer : SVGImageDataSizer
{

    public SVGImageDataVPAndWHSizer(SVGCanvas forCanvas, Style appliedStyle, LayoutContext context)
        : base(forCanvas, appliedStyle, context)
    {
    }
}