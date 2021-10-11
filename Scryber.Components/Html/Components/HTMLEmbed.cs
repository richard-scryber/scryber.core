using System;
using Scryber.Components;
using Scryber.Styles;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("embedded")]
    [PDFRemoteParsableComponent("embed", SourceAttribute = "src")]
    public class HTMLEmbed : VisualComponent, IInvisibleContainer
    {

        [PDFAttribute("class")]
        public override string StyleClass { get => base.StyleClass; set => base.StyleClass = value; }

        [PDFAttribute("style")]
        public override Style Style { get => base.Style; set => base.Style = value; }

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

        [PDFElement("")]
        [PDFArray(typeof(Component))]
        public ComponentList Contents
        {
            get { return base.InnerContent; }
        }


        public HTMLEmbed() : base((ObjectType)"htem")
        {
        }
    }
}
