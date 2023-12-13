using System;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("q")]
    public class HTMLQuotedSpan : HTMLSpan
    {
        public HTMLQuotedSpan() : this(HTMLObjectTypes.Quoted)
        {
        }

        protected HTMLQuotedSpan(ObjectType type): base(type)
        { }

        protected override Style GetBaseStyle()
        {
            if (string.IsNullOrEmpty(this.ElementName))
                this.ElementName = "q";

            var style = base.GetBaseStyle();
            return style;
        }
    }

    
}
