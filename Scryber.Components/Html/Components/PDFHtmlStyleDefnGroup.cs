using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("CssStyle")]
    public class PDFHtmlStyleDefnGroup : Scryber.Styles.PDFStyleDefn
    {
        
        public PDFHtmlStyleDefnGroup() : base((PDFObjectType)"htmS")
        {
            
        }

        public string Source { get; set; }

        public string Contents { get; set; }


    }
}
