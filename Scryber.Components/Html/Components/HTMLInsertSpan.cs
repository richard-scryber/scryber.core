using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Styles;

namespace Scryber.Html.Components
{

    [PDFParsableComponent("ins")]
    public class HTMLInsertSpan : HTMLUnderlinedSpan
    {

        [PDFAttribute("cite")]
        public string Citation { get; set; }

        [PDFAttribute("datetime")]
        public string DateTime { get; set; }

        public HTMLInsertSpan(): this(HTMLObjectTypes.Insert)
        { }

        protected HTMLInsertSpan(ObjectType type) : base(type)
        { }
    }
}
