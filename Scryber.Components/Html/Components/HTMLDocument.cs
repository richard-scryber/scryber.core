using System;
using System.Collections.Generic;
using System.Text;
using Scryber.Components;
using Scryber.Styles;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("html")]
    public class HTMLDocument : Document
    {

        private HTMLHead _head;

        [PDFElement("head")]
        public HTMLHead Head
        {
            get { return this._head; }
            set
            {
                if (null != _head)
                    this.InnerContent.Remove(_head);

                this._head = value;

                if (null != this._head)
                    this.InnerContent.Add(_head);
            }
        }


        [PDFAttribute("lang")]
        public string Language
        {
            get;
            set;
        }

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
        public override PageList Pages { get { return base.Pages; } }


        public override PDFStyle GetAppliedStyle(Component forComponent, PDFStyle baseStyle)
        {
            var applied = base.GetAppliedStyle(forComponent, baseStyle);
            if (null != this.Head)
                applied = this.Head.GetAppliedStyle(forComponent, applied);

            return applied;
        }
    }
}
