using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.Html.Components
{

    [PDFParsableComponent("font")]
    public class HTMLFontSpan : Scryber.Components.Span
    {

        [PDFAttribute("class")]
        public override string StyleClass { get => base.StyleClass; set => base.StyleClass = value; }

        [PDFAttribute("style")]
        public override Style Style { get => base.Style; set => base.Style = value; }

        
        [PDFAttribute("color")]
        public override Color FillColor
        {
            get => base.FillColor;
            set => base.FillColor = value;
        }

        [PDFAttribute("face")]
        public override FontSelector FontFamily
        {
            get => base.FontFamily;
            set => base.FontFamily = value;
        }

        [PDFAttribute("size")]
        public override Unit FontSize
        {
            get => base.FontSize;
            set => base.FontSize = value;
        }

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


        public HTMLFontSpan()
            : this(HTMLObjectTypes.FontSpan)
        {
        }

        protected HTMLFontSpan(ObjectType type): base(type)
        { }
    }

}
