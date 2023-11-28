using System;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.Html.Components
{
    /// <summary>
    /// Implements the HTML block element address, with an output of italic
    /// </summary>
    [PDFParsableComponent("aside")]
    public class HTMLAside : Scryber.Html.Components.HTMLDiv
    {


        public HTMLAside() : this(HTMLObjectTypes.Address)
        {
        }

        protected HTMLAside(ObjectType type): base(type)
        { }

        /// <summary>
        /// default output style the same as the base.
        /// </summary>
        /// <returns></returns>
        protected override Style GetBaseStyle()
        {
            return base.GetBaseStyle();
        }
    }
}
