namespace Scryber.Svg.Components;
using Scryber.Drawing;
using Scryber.Styles;

[PDFParsableComponent("polygon")]
public class SVGPolygon : SVGPolyLine
{


    public SVGPolygon(): base()
    { }

    protected override GraphicsPath CreatePath(Size available, Style fullstyle)
    {
        GraphicsPath path = base.CreatePath(available, fullstyle);
        path.ClosePath(true);

        return path;
    }
}