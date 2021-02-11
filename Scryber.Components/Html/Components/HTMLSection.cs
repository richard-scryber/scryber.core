using System;
using Scryber.Components;
using Scryber.Styles;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("section")]
    public class HTMLSection : HTMLHeadFootContainer
    {
        public HTMLSection()
        {
        }


        protected override Style GetBaseStyle()
        {
            var style = base.GetBaseStyle();
            style.PageStyle.BreakBefore = true;

            return style;
        }
    }
}
