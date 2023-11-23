using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Styles;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("i")]
    public class HTMLItalicSpan : Scryber.Components.ItalicSpan
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

        public HTMLItalicSpan()
            : this(HTMLObjectTypes.Italic)
        {
        }

        protected HTMLItalicSpan(ObjectType type): base(type)
        {

        }
    }

    [PDFParsableComponent("em")]
    public class HTMLEmphasis : HTMLItalicSpan
    {

        public HTMLEmphasis(): this(HTMLObjectTypes.Emphasis)
        { }

        protected HTMLEmphasis(ObjectType type): base(type)
        {
        }
    }
}
