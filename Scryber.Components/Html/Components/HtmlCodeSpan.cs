using System;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("code")]
    public class HTMLCodeSpan : HTMLSpan
    {
        public HTMLCodeSpan()
        {
        }

        protected override Style GetBaseStyle()
        {
            var style = base.GetBaseStyle();
            style.Font.FontFamily = (PDFFontSelector)"monospace";
            return style;
        }
    }
}
