using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Styles;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("b")]
    public class HTMLBoldSpan : Scryber.Components.BoldSpan
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

        public HTMLBoldSpan()
            : this(HTMLObjectTypes.Bold)
        {
        }

        protected HTMLBoldSpan(ObjectType type): base(type)
        { }
    }

    [PDFParsableComponent("strong")]
    public class HTMLStrong : HTMLBoldSpan
    {

        public HTMLStrong() : this(HTMLObjectTypes.Strong)
        { }

        protected HTMLStrong(ObjectType type): base(type)
        { }
    }
}
