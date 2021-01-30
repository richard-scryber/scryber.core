using System;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.Native;
using Scryber.Styles;

namespace Scryber.Svg.Components
{
    public abstract class SVGBase : VisualComponent, ICloneable
    {
        [PDFAttribute("class")]
        public override string StyleClass
        {
            get => base.StyleClass;
            set => base.StyleClass = value;
        }

        [PDFAttribute("style")]
        public override Style Style
        {
            get => base.Style;
            set => base.Style = value;
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

        public SVGBase(PDFObjectType type)
            : base(type)
        {
        }


        public virtual SVGBase Clone()
        {
            var c = this.MemberwiseClone() as SVGBase;
            c.Parent = null;
            if(this.Style.HasValues)
            {
                c.Style = new Style();
                this.Style.MergeInto(c.Style);
            }
            
            return c;
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }
    }
}
