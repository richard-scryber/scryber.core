using System;
using Scryber.Drawing;
using Scryber.Styles;
using Scryber.Components;
using Scryber.Html.Components;

namespace Scryber.Svg.Components
{
    [PDFParsableComponent("a")]
    public class SVGAnchor : HTMLAnchor
    {

        public SVGAnchor()
        {
        }


        protected override Style GetBaseStyle()
        {
            var style = base.GetBaseStyle();
            style.Fill.RemoveColor();
            style.Text.RemoveDecoration();
            return style;
        }
    }
}
