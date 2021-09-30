using System;
namespace Scryber.Html.Components
{
    [PDFParsableComponent("if")]
    public class HTMLIf : Scryber.Data.If
    {
        [PDFAttribute("data-test")]
        public override bool Test
        {
            get => base.Test;
            set => base.Test = value;
        }

        [PDFTemplate]
        [PDFElement("")]
        [PDFAttribute("data-template")]
        public override ITemplate Template { get => base.Template; set => base.Template = value; }


        public HTMLIf() : base()
        {
        }


    }
}
