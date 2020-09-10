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

        public HTMLMeta() : base((PDFObjectType)"htmM")
        { }
    }
}
