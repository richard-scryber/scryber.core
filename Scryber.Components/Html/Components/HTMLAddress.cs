using System;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.Html.Components
{
    /// <summary>
    /// Implements the HTML block element address, with an output of italic
    /// </summary>
    [PDFParsableComponent("address")]
    public class HTMLAddress : Scryber.Html.Components.HTMLDiv
    {


        public HTMLAddress() : this(HTMLObjectTypes.Address)
        {
        }

        protected HTMLAddress(ObjectType type): base(type)
        { }

        /// <summary>
        /// default output style is italic
        /// </summary>
        /// <returns></returns>
        protected override Style GetBaseStyle()
        {
            var style = base.GetBaseStyle();
            style.Font.FontItalic = true;
            return style;
        }
    }
}
