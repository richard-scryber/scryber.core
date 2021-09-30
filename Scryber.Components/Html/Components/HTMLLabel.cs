using System;
using Scryber.Styles;
using Scryber.PDF;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("label")]
    public class HTMLLabel : Scryber.Components.Span
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

        [PDFAttribute("data-content")]
        public string DataContent
        {
            get; set;
        }

        public HTMLLabel()
        {
        }


        protected override void OnPreLayout(PDFLayoutContext context)
        {
            if (!string.IsNullOrEmpty(this.DataContent))
            {
                this.Contents.Clear();
                this.Contents.Add(new Scryber.Components.TextLiteral(this.DataContent));
            }
            base.OnPreLayout(context);
        }
    }
}
