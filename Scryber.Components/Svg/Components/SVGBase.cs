using System;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.PDF.Native;
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
        public string StrokeLineCap
        {
            get
            {
                if (this.Style.IsValueDefined(StyleKeys.StrokeEndingKey))
                    return this.Style.Stroke.LineCap.ToString().ToLower();
                else
                    return LineCaps.Square.ToString().ToLower();
            }
            set
            {
                LineCaps cap;
                if (Enum.TryParse<LineCaps>(value, true, out cap))
                    this.Style.Stroke.LineCap = cap;
                else
                    this.Style.RemoveValue(StyleKeys.StrokeEndingKey);
            }
        }

        [PDFAttribute("stroke-linejoin")]
        public string StrokeLineJoin
        {
            get
            {
                if (this.Style.IsValueDefined(StyleKeys.StrokeJoinKey))
                    return this.Style.Stroke.LineJoin.ToString().ToLower();
                else
                    return LineJoin.Bevel.ToString().ToLower();
            }
            set
            {
                LineJoin join;
                if (Enum.TryParse<LineJoin>(value, true, out join))
                    this.Style.Stroke.LineJoin = join;
                else
                    this.Style.RemoveValue(StyleKeys.StrokeJoinKey);
            }
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
        public override double FillOpacity { get => base.FillOpacity; set => base.FillOpacity = value; }

        // fill

        [PDFAttribute("fill")]
        public override PDFColor FillColor
        {
            get => base.FillColor;
            set => base.FillColor = value;
        }

        [PDFAttribute("font-family")]
        public override FontSelector FontFamily
        {
            get => base.FontFamily;
            set => base.FontFamily = value;
        }

        [PDFAttribute("font-size")]
        public override PDFUnit FontSize
        {
            get => base.FontSize;
            set => base.FontSize = value;
        }

        

        // font


        public SVGBase(ObjectType type)
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
