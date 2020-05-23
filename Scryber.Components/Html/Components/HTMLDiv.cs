using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scryber.Html.Components
{
    /// <summary>
    /// Explicit sub class of the PDFDiv under the HTML Namespace that can be styled independantly
    /// </summary>
    [PDFParsableComponent("div")]
    public class HTMLDiv : Scryber.Components.PDFDiv
    {
        public HTMLDiv()
            : base()
        {
        }
    }
}
