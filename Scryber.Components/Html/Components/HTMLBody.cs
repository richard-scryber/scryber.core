using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Components;
using Scryber.Styles;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("body")]
    public class HTMLBody : Scryber.Components.Section
    {

        
        public HTMLBody()
            : base()
        {
        }

        [PDFAttribute("class")]
        public override string StyleClass { get => base.StyleClass; set => base.StyleClass = value; }

        [PDFAttribute("style")]
        public override PDFStyle Style { get => base.Style; set => base.Style = value; }

        [PDFElement("")]
        [PDFArray(typeof(Component))]
        public override ComponentList Contents => base.Contents;
    }
}
