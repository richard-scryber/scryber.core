using System;
using Scryber.Components;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("input")]
    public class HTMLInput : FormInputField
    {
        

        public HTMLInput() : this(HTMLObjectTypes.FormInput)
        {
        }

        protected HTMLInput(ObjectType type): base(type)
        { }
    }
}
