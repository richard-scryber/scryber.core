using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scryber.Html.Components
{

    [PDFParsableComponent("a")]
    public class HTMLAnchor : Scryber.Components.PDFLink
    {
        public HTMLAnchor()
            : base()
        {
        }


        public override string MapPath(string path)
        {
            return base.MapPath(path);
        }
    }
}
