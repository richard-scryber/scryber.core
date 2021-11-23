using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Styles;

namespace Scryber.Html.Components
{

    [PDFParsableComponent("main")]
    public class HTMLMain : HTMLHeadFootContainer
    {

        public HTMLMain()
            : this((ObjectType)"htMa")
        {
        }
        
        protected HTMLMain(ObjectType type)
            : base(type)
        {}
    }
}
