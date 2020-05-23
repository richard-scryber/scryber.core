using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scryber.Html.Components
{

    [PDFParsableComponent("td")]
    public class HTMLTableCell : Scryber.Components.PDFTableCell
    {
        public HTMLTableCell()
            : base()
        {
        }
    }

    [PDFParsableComponent("th")]
    public class HTMLTableHeaderCell : Scryber.Components.PDFTableHeaderCell
    {
        public HTMLTableHeaderCell()
            : base()
        {
        }
    }

}
