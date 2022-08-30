using Scryber.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("form")]
    public class HTMLForm : Scryber.Components.Form
    {
        [PDFAttribute("class")]
        public override string StyleClass { get => base.StyleClass; set => base.StyleClass = value; }

        [PDFAttribute("style")]
        public override Style Style { get => base.Style; set => base.Style = value; }

        [PDFAttribute("data-content")]
        public string DataContent { get; set; }

        public HTMLForm() : base()
        {
        }
    }
}
