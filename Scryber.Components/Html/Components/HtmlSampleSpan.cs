using System;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.Html.Components
{

    [PDFParsableComponent("samp")]
    public class HTMLSampleSpan : HTMLSpan
    {
        public HTMLSampleSpan() : this(HTMLObjectTypes.Sample)
        { }

        protected HTMLSampleSpan(ObjectType type) : base(type)
        { }

        protected override Style GetBaseStyle()
        {
            var style = base.GetBaseStyle();
            style.Font.FontFamily = (FontSelector)"monospace";
            return style;
        }

    }
}
