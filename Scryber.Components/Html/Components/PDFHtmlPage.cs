using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Components;
using Scryber.Styles;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("Page")]
    public class PDFHtmlPage : Scryber.Components.PDFPage
    {
        [PDFAttribute("source")]
        public string Source
        {
            get { return this._fragment.Source; }
            set { this._fragment.Source = value; }
        }

        [PDFElement("Content", IgnoreInnerTags =true)]
        public System.Xml.XmlNode HtmlContents
        {
            get { return this._fragment.XHTMLContents; }
            set { this._fragment.XHTMLContents = value; }
        }

        [PDFParserIgnore()]
        public override PDFComponentList Contents => base.Contents;

        private PDFHtmlFragment _fragment;

        public PDFHtmlPage(): base((PDFObjectType)"hmlP")
        {
            _fragment = new PDFHtmlFragment();
            this.InnerContent.Add(_fragment);
        }


        public override IPDFLayoutEngine GetEngine(IPDFLayoutEngine parent, PDFLayoutContext context, PDFStyle style)
        {
            return new Scryber.Layout.LayoutEnginePage(this, parent);
        }

        /// <summary>
        /// Overrides the base behaviour to add the overflow action of new page to this elements style.
        /// </summary>
        /// <returns></returns>
        protected override Scryber.Styles.PDFStyle GetBaseStyle()
        {
            Scryber.Styles.PDFStyle flat = base.GetBaseStyle();
            flat.Overflow.Action = Scryber.Drawing.OverflowAction.NewPage;

            return flat;
        }
    }
}
