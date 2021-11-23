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

        public HTMLSection() : base()
        {
        }

        protected override void OnDataBound(DataContext context)
        {
            this.AddDataContent(this.DataContent, context);
            base.OnDataBound(context);
        }

        protected override Style GetBaseStyle()
        {
            var style = base.GetBaseStyle();
            style.PageStyle.BreakBefore = true;

            return style;
        }
    }
}
