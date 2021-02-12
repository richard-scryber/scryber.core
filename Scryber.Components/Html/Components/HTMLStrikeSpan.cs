using System;
using Scryber.Styles;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("strike")]
    public class HTMLStrikeSpan : HTMLSpan
    {
        public HTMLStrikeSpan()
        {
        }

        protected override Style GetBaseStyle()
        {
            var style = base.GetBaseStyle();
            style.Text.Decoration = Text.TextDecoration.StrikeThrough;
            return style;
        }
    }

    [PDFParsableComponent("del")]
    public class HTMLDelSpan : HTMLStrikeSpan
    {

    }
}
