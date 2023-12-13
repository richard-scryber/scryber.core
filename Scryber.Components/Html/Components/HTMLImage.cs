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

        
        [PDFAttribute("data-allow-missing-images")]
        public override bool AllowMissingImages { get => base.AllowMissingImages; set => base.AllowMissingImages = value; }

        [PDFAttribute("data-min-scale")]
        public override double MinimumScaleReduction { get => base.MinimumScaleReduction; set => base.MinimumScaleReduction = value; }

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
        public override Unit Width
        {
            get => base.Width;
            set => base.Width = value;
        }
        
        [PDFAttribute("height")]
        public override Unit Height
        {
            get => base.Height;
            set => base.Height = value;
        }

        [PDFAttribute("alt")]
        public string AlternateName
        {
            get;set;
        }

        public HTMLImage()
            : this(HTMLObjectTypes.Image)
        {
        }

        protected HTMLImage(ObjectType type): base(type)
        { }


        public override string MapPath(string path)
        {
            //Override for data images as urls - where System.Uri.IsWellFormedUriString

            if (!string.IsNullOrEmpty(path) && path.StartsWith("data:image/"))
                return path;
            else
                return base.MapPath(path);
        }

        public override string MapPath(string source, out bool isfile)
        {
            return base.MapPath(source, out isfile);
        }


        protected override Style GetBaseStyle()
        {
            var style = base.GetBaseStyle();
            style.Position.PositionMode = PositionMode.Inline;
            return style;
        }

    }

}
