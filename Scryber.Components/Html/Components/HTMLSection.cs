using System;
using Scryber.Components;
using Scryber.Styles;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("section")]
    public class HTMLSection : Section
    {

        [PDFAttribute("class")]
        public override string StyleClass { get => base.StyleClass; set => base.StyleClass = value; }

        [PDFAttribute("style")]
        public override Style Style { get => base.Style; set => base.Style = value; }

        [PDFElement("")]
        [PDFArray(typeof(Component))]
        public override ComponentList Contents => base.Contents;

        [PDFElement("header")]
        [PDFTemplate()]
        public override IPDFTemplate Header { get => base.Header; set => base.Header = value; }

        [PDFElement("footer")]
        [PDFTemplate()]
        public override IPDFTemplate Footer { get => base.Footer; set => base.Footer = value; }

        /// <summary>
        /// Global Html hidden attribute used with xhtml as hidden='hidden'
        /// </summary>
        [PDFAttribute("hidden")]
        public string Hidden
        {
            get
            {
                if (this.Visible)
                    return string.Empty;
                else
                    return "hidden";
            }
            set
            {
                if (string.IsNullOrEmpty(value) || value != "hidden")
                    this.Visible = true;
                else
                    this.Visible = false;
            }
        }

        [PDFAttribute("title")]
        public override string OutlineTitle
        {
            get => base.OutlineTitle;
            set => base.OutlineTitle = value;
        }

        public HTMLSection()
        {
        }
    }
}
