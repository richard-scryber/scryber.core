using System;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("mark")]
    public class HTMLMarkedSpan : HTMLSpan
    {

        private static Color MarkColor = (Color)Html.CSSColors.Names2Colors["mark"];

        public HTMLMarkedSpan() : this(HTMLObjectTypes.Quoted)
        {
        }

        protected HTMLMarkedSpan(ObjectType type): base(type)
        { }

        protected override Style GetBaseStyle()
        {
            if (string.IsNullOrEmpty(this.ElementName))
                this.ElementName = "mark";

            var style = base.GetBaseStyle();

            style.Background.Color = MarkColor;
            //style.Text.Decoration = Text.TextDecoration.Underline;

            return style;
        }
    }

    
}
