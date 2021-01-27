using System;
using Scryber.Styles;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.Native;
using Scryber.Styles.Parsing;

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

        [PDFAttribute("stroke")]
        public override PDFColor StrokeColor
        {
            get => base.StrokeColor;
            set => base.StrokeColor = value;
        }


        [PDFAttribute("stroke-width")]
        public override PDFUnit StrokeWidth
        {
            get => base.StrokeWidth;
            set => base.StrokeWidth = value;
        }

        [PDFAttribute("stroke-linecap")]
        public LineCaps StrokeLineCap
        {
            get { return this.Style.Stroke.LineCap; }
            set { this.Style.Stroke.LineCap = value; }
        }

        [PDFAttribute("stroke-linejoin")]
        public LineJoin StrokeLineJoin
        {
            get { return this.Style.Stroke.LineJoin; }
            set { this.Style.Stroke.LineJoin = value; }
        }

        [PDFAttribute("stroke-dasharray")]
        public override PDFDash StrokeDashPattern
        {
            get => base.StrokeDashPattern;
            set => base.StrokeDashPattern = value;
        }

        [PDFAttribute("stroke-opacity")]
        public override double StrokeOpacity { get => base.StrokeOpacity; set => base.StrokeOpacity = value; }

        [PDFAttribute("fill-opacity")]
        public override PDFReal FillOpacity { get => base.FillOpacity; set => base.FillOpacity = value; }

        // fill

        [PDFAttribute("fill")]
        public override PDFColor FillColor
        {
            get => base.FillColor;
            set => base.FillColor = value;
        }

        #region public PDFRect ViewBox {get; set;}

        [PDFAttribute("viewBox")]
        public PDFRect ViewBox
        {
            get
            {
                StyleValue<PDFRect> rect;
                if (this.Style.TryGetValue(StyleKeys.PositionViewPort, out rect))
                    return rect.Value;
                else
                    return PDFRect.Empty;
            }
            set
            {
                this.Style.SetValue(StyleKeys.PositionViewPort, value);
            }
        }

        public void RemoveViewBox()
        {
            this.Style.RemoveValue(StyleKeys.PositionViewPort);
        }

        #endregion

        #region public SVGAspectRatio PreserveAspectRatio

        [PDFAttribute("preserveAspectRatio")]
        public SVGAspectRatio PreserveAspectRatio
        {
            get
            {
                StyleValue<SVGAspectRatio> aspect;
                if (this.Style.TryGetValue(SVGAspectRatio.AspectRatioStyleKey, out aspect))
                {
                    return aspect.Value;
                }
                else
                    return SVGAspectRatio.Default;
            }
            set
            {
                this.Style.SetValue(SVGAspectRatio.AspectRatioStyleKey, value);
            }
        }

        public void RemoveAspectRatio()
        {
            this.Style.RemoveValue(SVGAspectRatio.AspectRatioStyleKey);
        }

        #endregion

        //style attributes

        [PDFAttribute("width")]
        public override PDFUnit Width { get => base.Width; set => base.Width = value; }

        [PDFAttribute("height")]
        public override PDFUnit Height { get => base.Height; set => base.Height = value; }


        public SVGCanvas()
        {
        }

        protected override Style GetBaseStyle()
        {
            return base.GetBaseStyle();
        }
    }
}
