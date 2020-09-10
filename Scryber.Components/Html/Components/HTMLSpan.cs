using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Styles;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("span")]
    public class HTMLSpan : Scryber.Components.PDFSpan
    {
        [PDFAttribute("class")]
        public override string StyleClass { get => base.StyleClass; set => base.StyleClass = value; }

        [PDFAttribute("style")]
        public override PDFStyle Style { get => base.Style; set => base.Style = value; }

        public HTMLSpan()
            : base()
        {
        }
    }
}
