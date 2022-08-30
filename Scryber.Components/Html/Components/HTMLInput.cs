using System;
using Scryber.Components;
using Scryber.Styles;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("input")]
    public class HTMLInput : FormInputField
    {
        [PDFAttribute("class")]
        public override string StyleClass
        {
            get => base.StyleClass;
            set => base.StyleClass = value;
        }

        [PDFAttribute("style")]
        public override Style Style
        {
            get => base.Style;
            set => base.Style = value;
        }

        public HTMLInput() :base()
        {
        }
    }
}
