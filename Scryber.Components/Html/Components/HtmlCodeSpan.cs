using System;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("code")]
    public class HTMLCodeSpan : HTMLSpan
    {
        public HTMLCodeSpan() : this(HTMLObjectTypes.Code)
        {
        }

        protected HTMLCodeSpan(ObjectType type): base(type)
        { }

        protected override Style GetBaseStyle()
        {
            var style = base.GetBaseStyle();
            style.Font.FontFamily = (FontSelector)"monospace";
            return style;
        }
    }
}
