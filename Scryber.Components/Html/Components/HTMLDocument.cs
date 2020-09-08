using System;
using System.Collections.Generic;
using System.Text;
using Scryber.Components;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("html")]
    public class HTMLDocument : PDFDocument
    {
        [PDFElement("head")]
        public HTMLHead Head { get; set; }

        private HTMLBody _body;

        [PDFElement("body")]
        public HTMLBody Body
        {
            get { return this._body; }
            set
            {
                if (null != this._body)
                    this.Pages.Remove(this._body);

                this._body = value;

                if (null != this._body)
                    this.Pages.Add(_body);
            }
        }

        [PDFParserIgnore()]
        public override PDFPageList Pages { get { return base.Pages; } }
    }
}
