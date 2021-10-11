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
        public override Unit Width { get => base.Width; set => base.Width = value; }

        [PDFAttribute("height")]
        public override Unit Height { get => base.Height; set => base.Height = value; }

        [PDFAttribute("x")]
        public override Unit X { get => base.X; set => base.X = value; }

        [PDFAttribute("y")]
        public override Unit Y { get => base.Y; set => base.Y = value; }

        [PDFAttribute("rx")]
        public Unit CornerRadiusX { get { return this.Style.Border.CornerRadius; } set { this.Style.Border.CornerRadius = value; } }

        [PDFAttribute("ry")]
        public Unit CornerRadiusY { get { return this.Style.Border.CornerRadius; } set { this.Style.Border.CornerRadius = value; } }

        // stroke

        [PDFAttribute("stroke")]
        public override Color StrokeColor { get => base.StrokeColor; set => base.StrokeColor = value; }

        [PDFAttribute("stroke-width")]
        public override Unit StrokeWidth { get => base.StrokeWidth; set => base.StrokeWidth = value; }

        [PDFAttribute("stroke-linecap")]
        public LineCaps StrokeLineCap
        {
            get { return this.Style.Stroke.LineCap; }
            set { this.Style.Stroke.LineCap = value; }
        }

        [PDFAttribute("stroke-dasharray")]
        public override Dash StrokeDashPattern { get => base.StrokeDashPattern; set => base.StrokeDashPattern = value; }



        // fill

        [PDFAttribute("fill")]
        public override Color FillColor { get => base.FillColor; set => base.FillColor = value; }


        //
        // .ctor
        //

        public SVGRect()
        {
        }


        protected override void BuildPath(GraphicsPath path, Point[] points, Style style, bool end)
        {
            if (this.CornerRadiusX != Unit.Zero)
                this.BuildRoundRectPath(path, points, style);
            else
                base.BuildPath(path, points, style, end);
        }

        private void BuildRoundRectPath(GraphicsPath path, Point[] points, Style style)
        {
            throw new NotImplementedException("Round rectangle points are not currently supported");
        }
    }
}
