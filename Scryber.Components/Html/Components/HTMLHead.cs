using System;
using System.Collections.Generic;
using System.Text;
using Scryber.Components;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("head")]
    public class HTMLHead : PDFContainerComponent
    {
        
        [PDFElement("title")]
        public string Title { get; set; }

        [PDFArray()]
        [PDFElement("")]
        public PDFComponentList Contents
        {
            get { return base.InnerContent; }
            set { base.InnerContent = value; }
        }

        public HTMLHead() : this((PDFObjectType)"htmH")
        {

        }

        public HTMLHead(PDFObjectType type): base(type)
        {

        }
    }
}
