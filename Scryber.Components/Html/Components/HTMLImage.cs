using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("img")]
    public class HTMLImage : Scryber.Components.Image
    {
        [PDFAttribute("class")]
        public override string StyleClass { get => base.StyleClass; set => base.StyleClass = value; }

        [PDFAttribute("style")]
        public override Style Style { get => base.Style; set => base.Style = value; }

        [PDFAttribute("data-img")]
        public override PDFImageData Data { get => base.Data; set => base.Data = value; }

        [PDFAttribute("data-allow-missing-images")]
        public override bool AllowMissingImages { get => base.AllowMissingImages; set => base.AllowMissingImages = value; }

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

        [PDFAttribute("align")]
        public FloatMode Align
        {
            get
            {
                return this.Style.Position.Float;
            }
            set
            {
                this.Style.Position.Float = value;
            }
        }

        [PDFAttribute("title")]
        public override string OutlineTitle
        {
            get => base.OutlineTitle;
            set => base.OutlineTitle = value;
        }

        [PDFAttribute("width")]
        public override PDFUnit Width
        {
            get => base.Width;
            set => base.Width = value;
        }
        
        [PDFAttribute("height")]
        public override PDFUnit Height
        {
            get => base.Height;
            set => base.Height = value;
        }

        public HTMLImage()
            : base()
        {
        }

    }

}
