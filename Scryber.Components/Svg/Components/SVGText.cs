using System;
using Scryber.Drawing;
using Scryber.Styles;
using Scryber.Components;
using Scryber.PDF;
using Scryber.PDF.Layout;
using Scryber.Text;


namespace Scryber.Svg.Components
{
    [PDFParsableComponent("text")]
    public class SVGText : SVGBase, IPDFViewPortComponent
    {

        [PDFAttribute("width")]
        public override Unit Width { 
            get => Unit.Zero;
            set
            {
                ;
            }
        }

        /// <summary>
        /// Not supported
        /// </summary>
        [PDFAttribute("height")]
        public override Unit Height { 
            get => Unit.Zero;
            set
            {
                ;
            } 
        }

        [PDFAttribute("x")]
        public override Unit X {
            get
            {
                StyleValue<Unit> value;
                if (this.Style.TryGetValue(StyleKeys.SVGGeometryXKey, out value))
                    return value.Value(this.Style);
                else
                    return Unit.Zero;
            }
            set
            {
                this.Style.SetValue(StyleKeys.SVGGeometryXKey, value);
            }
        }

        [PDFAttribute("y")]
        public override Unit Y {
            get
            {
                StyleValue<Unit> value;
                if (this.Style.TryGetValue(StyleKeys.SVGGeometryYKey, out value))
                    return value.Value(this.Style);
                else
                    return Unit.Zero;
            }
            set
            {
                this.Style.SetValue(StyleKeys.SVGGeometryYKey, value);
            }
        }
        
        [PDFAttribute("dx")]
        public Unit DeltaX {
            get
            {
                StyleValue<Unit> value;
                if (this.Style.TryGetValue(StyleKeys.SVGGeometryDeltaXKey, out value))
                    return value.Value(this.Style);
                else
                    return Unit.Zero;
            }
            set
            {
                this.Style.SetValue(StyleKeys.SVGGeometryDeltaXKey, value);
            }
        }

        [PDFAttribute("dy")]
        public Unit DeltaY {
            get
            {
                StyleValue<Unit> value;
                if (this.Style.TryGetValue(StyleKeys.SVGGeometryDeltaYKey, out value))
                    return value.Value(this.Style);
                else
                    return Unit.Zero;
            }
            set
            {
                this.Style.SetValue(StyleKeys.SVGGeometryDeltaYKey, value);
            }
        }

        private Unit _textLength = Unit.Auto;
        
        [PDFAttribute("textLength")]
        public Unit TextLength
        {
            get
            {
                return _textLength;
            }
            set
            {
                _textLength = value;
            }
        }

        private TextLengthAdjustType _lengthAdjust = TextLengthAdjustType.Spacing;

        [PDFAttribute("lengthAdjust")]
        public TextLengthAdjustType LengthAdjust
        {
            get { return _lengthAdjust; }
            set { _lengthAdjust = value; }
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

        [PDFAttribute("letter-spacing")]
        public Unit LetterSpacing
        {
            get
            {
                StyleValue<Unit> spacing;
                if (this.Style.TryGetValue(StyleKeys.TextCharSpacingKey, out spacing))
                    return spacing.Value(this.Style);
                else
                {
                    return Unit.Zero;
                }
            }
            set
            {
                this.Style.SetValue(StyleKeys.TextCharSpacingKey, value);
            }
        }

        public void RemoveLetterSpacing()
        {
            this.Style.RemoveValue(StyleKeys.TextCharSpacingKey);
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

        /// <summary>
        /// Set to the block that contains this text - so it can be referred to later.
        /// </summary>
        internal PDFLayoutBlock TextBlock { get; set; }

        public SVGText() : base(ObjectTypes.Text)
        {
        }


        protected override Style GetBaseStyle()
        {
            var style = base.GetBaseStyle();
            style.Position.DisplayMode = DisplayMode.Block;
            style.Position.PositionMode = PositionMode.Absolute;
            style.Padding.Right = 4;
            style.Text.WrapText = Text.WordWrap.NoWrap;
            style.Text.PositionFromBaseline = true;
            style.Text.Leading = Unit.Auto;
            style.Text.CharacterSpacing = 0;
            style.Text.WordSpacing = 0;
            style.Text.Decoration = TextDecoration.None;
            style.Text.FirstLineInset = 0;
            style.Text.PreserveWhitespace = false;
            return style;
        }


        #region IPDFViewPortComponent Members

        IPDFLayoutEngine IPDFViewPortComponent.GetEngine(IPDFLayoutEngine parent, PDFLayoutContext context, Style style)
        {
            return this.CreateLayoutEngine(parent, context, style);
        }

        protected virtual IPDFLayoutEngine CreateLayoutEngine(IPDFLayoutEngine parent, PDFLayoutContext context, Style style)
        {
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
