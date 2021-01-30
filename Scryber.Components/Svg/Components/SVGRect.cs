using System;
using Scryber.Styles;
using Scryber.Drawing;

namespace Scryber.Svg.Components
{
    [PDFParsableComponent("rect")]
    public class SVGRect : Scryber.Components.Rectangle
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

        [PDFAttribute("title")]
        public override string OutlineTitle
        {
            get => base.OutlineTitle;
            set => base.OutlineTitle = value;
        }

        //
        //style attributes
        //

        // position

        [PDFAttribute("width")]
        public override PDFUnit Width { get => base.Width; set => base.Width = value; }

        [PDFAttribute("height")]
        public override PDFUnit Height { get => base.Height; set => base.Height = value; }

        [PDFAttribute("x")]
        public override PDFUnit X { get => base.X; set => base.X = value; }

        [PDFAttribute("y")]
        public override PDFUnit Y { get => base.Y; set => base.Y = value; }

        [PDFAttribute("rx")]
        public PDFUnit CornerRadiusX { get { return this.Style.Border.CornerRadius; } set { this.Style.Border.CornerRadius = value; } }

        [PDFAttribute("ry")]
        public PDFUnit CornerRadiusY { get { return this.Style.Border.CornerRadius; } set { this.Style.Border.CornerRadius = value; } }

        // stroke

        [PDFAttribute("stroke")]
        public override PDFColor StrokeColor { get => base.StrokeColor; set => base.StrokeColor = value; }

        [PDFAttribute("stroke-width")]
        public override PDFUnit StrokeWidth { get => base.StrokeWidth; set => base.StrokeWidth = value; }

        [PDFAttribute("stroke-linecap")]
        public LineCaps StrokeLineCap
        {
            get { return this.Style.Stroke.LineCap; }
            set { this.Style.Stroke.LineCap = value; }
        }

        [PDFAttribute("stroke-dasharray")]
        public override PDFDash StrokeDashPattern { get => base.StrokeDashPattern; set => base.StrokeDashPattern = value; }



        // fill

        [PDFAttribute("fill")]
        public override PDFColor FillColor { get => base.FillColor; set => base.FillColor = value; }


        //
        // .ctor
        //

        public SVGRect()
        {
        }


        protected override void BuildPath(PDFGraphicsPath path, PDFPoint[] points, Style style, bool end)
        {
            if (this.CornerRadiusX != PDFUnit.Zero)
                this.BuildRoundRectPath(path, points, style);
            else
                base.BuildPath(path, points, style, end);
        }

        private void BuildRoundRectPath(PDFGraphicsPath path, PDFPoint[] points, Style style)
        {
            throw new NotImplementedException("Round rectangle points are not currently supported");
        }
    }
}
