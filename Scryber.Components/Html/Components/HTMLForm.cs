using System;
using Scryber.Components;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("form")]
    public class HTMLForm : Form
    {
        [PDFAttribute("action")]
        public string Action { get; set; }

        [PDFAttribute("method")]
        public string Method { get; set; }

        public HTMLForm() : this(HTMLObjectTypes.Form)
        {
        }

        protected HTMLForm(ObjectType type) : base(type)
        { }
    }
}
