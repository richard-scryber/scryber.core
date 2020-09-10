using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Styles;

namespace Scryber.Html.Components
{

    [PDFParsableComponent("td")]
    public class HTMLTableCell : Scryber.Components.PDFTableCell
    {
        [PDFAttribute("class")]
        public override string StyleClass { get => base.StyleClass; set => base.StyleClass = value; }

        [PDFAttribute("style")]
        public override PDFStyle Style { get => base.Style; set => base.Style = value; }

        public HTMLTableCell()
            : base()
        {
        }
    }

    [PDFParsableComponent("th")]
    public class HTMLTableHeaderCell : Scryber.Components.PDFTableHeaderCell
    {
        [PDFAttribute("class")]
        public override string StyleClass { get => base.StyleClass; set => base.StyleClass = value; }

        [PDFAttribute("style")]
        public override PDFStyle Style { get => base.Style; set => base.Style = value; }

        public HTMLTableHeaderCell()
            : base()
        {
        }
    }

}
