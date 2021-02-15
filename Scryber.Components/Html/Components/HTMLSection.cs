using System;
using Scryber.Components;
using Scryber.Styles;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("section")]
    public class HTMLSection : HTMLHeadFootContainer
    {
        [PDFAttribute("data-content")]
        public string DataContent { get; set; }

        public HTMLSection()
        {
        }

        protected override void OnDataBinding(PDFDataContext context)
        {
            this.AddDataContent(this.DataContent, context);
            base.OnDataBinding(context);
        }

        protected override Style GetBaseStyle()
        {
            var style = base.GetBaseStyle();
            style.PageStyle.BreakBefore = true;

            return style;
        }
    }
}
