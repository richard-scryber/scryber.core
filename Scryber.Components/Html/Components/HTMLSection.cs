using System;
using Scryber.Components;
using Scryber.Styles;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("section")]
    public class HTMLSection : HTMLHeadFootContainer
    {
        

        public HTMLSection() : this(HTMLObjectTypes.Section)
        {
        }

        protected HTMLSection(ObjectType type) : base(type)
        { }

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
