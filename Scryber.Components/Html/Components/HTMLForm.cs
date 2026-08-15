using System;
using Scryber.Components;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("form")]
    public class HTMLForm : Form
    {
        public HTMLForm() : this(HTMLObjectTypes.Form)
        {
        }

        protected HTMLForm(ObjectType type) : base(type)
        { }
    }
}
