using System;
namespace Scryber.Html.Components
{

    [PDFParsableComponent("nav")]
    public class HTMLNav : HTMLDiv
    {
        public HTMLNav() : this(HTMLObjectTypes.Nav)
        {
        }

        protected HTMLNav(ObjectType type) : base(type)
        { }
    }
}
