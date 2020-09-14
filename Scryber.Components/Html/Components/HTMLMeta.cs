using System;
using System.Collections.Generic;
using System.Text;
using Scryber.Components;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("meta")]
    public class HTMLMeta : Component
    {

        [PDFElement()]
        [PDFAttribute("content")]
        public string Content { get; set; }

        [PDFAttribute("http-equiv")]
        public string HttpEquiv { get; set; }

        [PDFAttribute("charset")]
        public string CharSet { get; set; }

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

        

        public HTMLMeta() : base((PDFObjectType)"htmM")
        { }
    }
}
