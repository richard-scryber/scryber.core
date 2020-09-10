using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Styles;

namespace Scryber.Html.Components
{
    /// <summary>
    /// Explicit sub class of the PDFDiv under the HTML Namespace that can be styled independantly
    /// </summary>
    [PDFParsableComponent("div")]
    public class HTMLDiv : Scryber.Components.Div
    {
        [PDFAttribute("class")]
        public override string StyleClass { get => base.StyleClass; set => base.StyleClass = value; }

        [PDFAttribute("style")]
        public override PDFStyle Style { get => base.Style; set => base.Style = value; }
        public HTMLDiv()
            : base()
        {
        }
    }
}
