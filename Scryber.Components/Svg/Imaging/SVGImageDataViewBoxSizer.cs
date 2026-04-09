using Scryber.Styles;
using Scryber.Drawing;
using Scryber.Svg.Components;

namespace Scryber.Svg.Imaging;

public class SVGImageDataViewBoxSizer : SVGImageDataSizer
{
    public SVGImageDataViewBoxSizer(SVGCanvas forCanvas, Style appliedStyle, LayoutContext context)
        : base(forCanvas, appliedStyle, context)
    {
    }
}