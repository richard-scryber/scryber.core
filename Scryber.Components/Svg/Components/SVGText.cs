using System;
using Scryber.Drawing;
using Scryber.Styles;
using Scryber.Components;
using Scryber.PDF;
using Scryber.PDF.Layout;


namespace Scryber.Svg.Components
{
    [PDFParsableComponent("text")]
    public class SVGText : SVGBase, IPDFViewPortComponent
    {

        [PDFAttribute("width")]
        public override Unit Width { get => base.Width; set => base.Width = value; }

        [PDFAttribute("height")]
        public override Unit Height { get => base.Height; set => base.Height = value; }

        [PDFAttribute("x")]
        public override Unit X { get => base.X; set => base.X = value; }

        [PDFAttribute("y")]
        public override Unit Y {
            get => base.Y;
            set => base.Y = value;
        }


        [PDFAttribute("text-anchor")]
        public TextAnchor TextAnchor
        {
            get
            {
                StyleValue<TextAnchor> value;
                if (this.Style.TryGetValue(StyleKeys.TextAnchorKey, out value))
                    return value.Value(this.Style);
                else
                    return TextAnchor.Start;
            }
            set
            {
                this.Style.SetValue(StyleKeys.TextAnchorKey, value);
            }
        }

        public void RemoveTextAnchor()
        {
            this.Style.RemoveValue(StyleKeys.TextAnchorKey);
        }

        [PDFAttribute("dominant-baseline")]
        public DominantBaseline DominantBaseline
        {
            get
            {
                StyleValue<DominantBaseline> value;
                if (this.Style.TryGetValue(StyleKeys.DominantBaselineKey, out value))
                    return value.Value(this.Style);
                else
                    return DominantBaseline.Auto;

            }
            set
            {
                if (value == DominantBaseline.Auto)
                    this.RemoveDominantBaseline();
                else
                    this.Style.SetValue(StyleKeys.DominantBaselineKey, value);
            }
        }

        public void RemoveDominantBaseline()
        {
            this.Style.RemoveValue(StyleKeys.DominantBaselineKey);
        }

        private TextLiteralList _inner;

        [PDFElement()]
        [PDFArray(typeof(TextLiteral))]
        public TextLiteralList Content
        {
            get
            {
                if (null == _inner)
                    _inner = new TextLiteralList(this, this.InnerContent);
                return _inner;
            }
        }


        public SVGText() : base(ObjectTypes.Text)
        {
        }


        protected override Style GetBaseStyle()
        {
            var style = base.GetBaseStyle();
            style.Position.PositionMode = PositionMode.Block;
            style.Padding.Right = 4;
            style.Text.WrapText = Text.WordWrap.NoWrap;
            style.Text.PositionFromBaseline = true;
            return style;
        }


        #region IPDFViewPortComponent Members

        IPDFLayoutEngine IPDFViewPortComponent.GetEngine(IPDFLayoutEngine parent, PDFLayoutContext context, Style style)
        {
            return this.CreateLayoutEngine(parent, context, style);
        }

        protected virtual IPDFLayoutEngine CreateLayoutEngine(IPDFLayoutEngine parent, PDFLayoutContext context, Style style)
        {
            //TODO: Support the use of measurement from the baseline and the text positioning content
            // e.g. text-anchor, text-length
            //https://developer.mozilla.org/en-US/docs/Web/SVG/Element/text

            return new Scryber.Svg.Layout.TSpanLayoutEngine(this, parent);
        }

        #endregion
    }

    public class TextLiteralList : ComponentWrappingList<TextLiteral>
    {
        public TextLiteralList(Component owner, ComponentList inner) : base(inner)
        {

        }
    }


    
}
