using System;
using Scryber.Styles;
using Scryber.Components;
using Scryber.Drawing;

namespace Scryber.Svg.Components
{
    [PDFParsableComponent("svg")]
    public class SVGCanvas : Scryber.Components.Canvas
    {

        [PDFAttribute("class")]
        public override string StyleClass { get => base.StyleClass; set => base.StyleClass = value; }

        [PDFAttribute("style")]
        public override Style Style { get => base.Style; set => base.Style = value; }

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

        [PDFElement("")]
        [PDFArray(typeof(Component))]
        public override ComponentList Contents
        {
            get { return base.Contents; }
        }

        [PDFAttribute("title")]
        public override string OutlineTitle
        {
            get => base.OutlineTitle;
            set => base.OutlineTitle = value;
        }

        //style attributes

        [PDFAttribute("width")]
        public override PDFUnit Width { get => base.Width; set => base.Width = value; }

        [PDFAttribute("height")]
        public override PDFUnit Height { get => base.Height; set => base.Height = value; }


        public SVGCanvas()
        {
        }
    }
}
