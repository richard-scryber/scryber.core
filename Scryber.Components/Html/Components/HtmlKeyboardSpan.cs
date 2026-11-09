using System;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.Html.Components
{

    [PDFParsableComponent("kbd")]
    public class HTMLKeyboardSpan : HTMLSpan
    {
        public HTMLKeyboardSpan() : this(HTMLObjectTypes.Keyboard)
        { }

        protected HTMLKeyboardSpan(ObjectType type) : base(type)
        { }

        protected override Style GetBaseStyle()
        {
            var style = base.GetBaseStyle();
            style.Font.FontFamily = (FontSelector)"monospace";
            return style;
        }

    }
}
