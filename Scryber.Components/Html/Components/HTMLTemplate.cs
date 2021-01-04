using System;
namespace Scryber.Html.Components
{
    [PDFParsableComponent("template")]
    public class HTMLTemplate : Scryber.Data.ForEach
    {
        [PDFTemplate()]
        [PDFElement()]
        public IPDFTemplate TemplateContent
        {
            get { return base.Template; }
            set { base.Template =value; }
        }

        [PDFAttribute("data-bind", BindingOnly = true)]
        public object DataBindValue
        {
            get { return base.Value; }
            set { base.Value = value; }
        }

        public HTMLTemplate()
        {
        }
    }
}
