using System;
using Scryber.Drawing;
using Scryber.Styles;
using Scryber.Components;
using Scryber.PDF;
using Scryber.PDF.Layout;
using Scryber.PDF.Native;
using Scryber.PDF.Resources;
using Scryber.Text;


namespace Scryber.Svg.Components
{
    [PDFParsableComponent("text")]
    public class SVGText : SVGBase, IPDFViewPortComponent, IPDFRenderComponent
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
        
        [PDFAttribute("text-decoration")]
        public string TextDecoration
        {
            get
            {
                StyleValue<TextDecoration> found;
                if (this.HasStyle && this.Style.TryGetValue(StyleKeys.TextDecorationKey, out found))
                {
                    
                    switch (found.Value(this.Style))
                    {
                        case Text.TextDecoration.None:
                            return "none";
                        case Text.TextDecoration.Overline:
                            return "overline";
                        case Text.TextDecoration.Underline:
                            return "underline";
                        case Text.TextDecoration.StrikeThrough:
                            return "line-through";
                        default:
                            return string.Empty;
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                TextDecoration parsed;
                switch (value)
                {
                    case "none":
                        parsed = Text.TextDecoration.None;
                        break;
                    case "overline":
                        parsed = Text.TextDecoration.Overline;
                        break ;
                    case "underline" :
                        parsed = Text.TextDecoration.Underline;
                        break;
                    case "line-through" :
                        parsed = Text.TextDecoration.StrikeThrough;
                        break ;
                    default:
                        return;
                }
                this.Style.SetValue(StyleKeys.TextDecorationKey, parsed);
            }
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

        public SVGText() : this(ObjectTypes.Text)
        {
        }

        protected SVGText(ObjectType type) : base(type)
        {
            
        }

        

        protected override Style GetBaseStyle()
        {
            var style = base.GetBaseStyle();
            //style.Position.DisplayMode = DisplayMode.Block;
            
            //Internal flag to identify that we use the SVGGeometry
            // style.SetValue(StyleKeys.SVGGeometryInUseKey, true);
            //
            // style.Position.PositionMode = PositionMode.Absolute;
            // style.Padding.Right = 4;
            // style.Text.WrapText = Text.WordWrap.NoWrap;
            // style.Text.PositionFromBaseline = true;
            // style.Text.Leading = Unit.Auto;
            // style.Text.CharacterSpacing = 0;
            // style.Text.WordSpacing = 0;
            // style.Text.Decoration = TextDecoration.None;
            // style.Text.FirstLineInset = 0;
            // style.Text.PreserveWhitespace = false;
            return style;
        }

        protected override void SetArrangement(ComponentArrangement arrange, PDFRenderContext context)
        {
            base.SetArrangement(arrange, context);
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


        public override SVGBase Clone()
        {
            var text = base.Clone() as SVGText;
            text.InnerContent = new ComponentList(text, this.Type);
            
            for (int i = 0; i < this.InnerContent.Count; i++)
            {
                var clonable = this.InnerContent[i] as ICloneable;
                if(null == clonable)
                    continue;
                var comp = clonable.Clone() as Component;
                if(null == comp)
                    continue;
                
                text.InnerContent.Add(comp);
            }

            return text;
        }
        
        
        #region IPDFRenderComponent - explicit as usually handled by the layout engine
        

        PDFObjectRef IPDFRenderable.OutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            if (this.Content.Count <= 0)
                return null;

            var style = context.FullStyle;
            var textOptions = style.CreateTextOptions();
            var stroke = style.CreateStrokePen();
            var fill = style.CreateFillBrush();
            var font = style.CreateFont();

            var pos = new Point();
            
            StyleValue<Unit> value;
            if (style.TryGetValue(StyleKeys.SVGGeometryXKey, out value))
                pos.X = value.Value(style);
            else if (style.TryGetValue(StyleKeys.SVGGeometryDeltaYKey, out value))
                pos.X = context.Offset.X + value.Value(style);

            if (style.TryGetValue(StyleKeys.SVGGeometryYKey, out value))
                pos.Y = value.Value(style);
            else if (style.TryGetValue(StyleKeys.SVGGeometryDeltaYKey, out value))
                pos.Y = context.Offset.Y + value.Value(style);

            var bounds = new Rect(pos, context.Space);

            if (this._textLength != Unit.Auto)
                bounds.Width = this.TextLength;
            
            var origOffset = context.Offset;
            var origSize = context.Space;
            context.Graphics.SaveGraphicsState();
            
            PDFObjectRef oref = null;
            
            context.Graphics.BeginText();
            context.Offset = pos;
            
            try
            {
                
                
                if (null != textOptions.Font)
                {
                    if (null == textOptions.Font.Resource)
                        textOptions.Font.SetResourceFont(textOptions.Font.FullName,
                            Document.GetFontResource(textOptions.Font, true, true));
                    
                    textOptions.Font.Resource.RegisterUse(context.Graphics.Container.Resources, this);
                    
                    context.Graphics.SetCurrentFont(textOptions.Font);
                }

                if (null != textOptions.Stroke)
                    textOptions.Stroke.SetUpGraphics(context.Graphics, bounds);

                if (null != textOptions.FillBrush)
                    textOptions.FillBrush.SetUpGraphics(context.Graphics, bounds);
                
                var size = new Size(pos.X, pos.Y);
                context.Graphics.MoveTextCursor(size, true);
            
                
                
                foreach (var literal in this.Content)
                {
                    var reader = literal.CreateReader(context, style);
                    char? charFitted;
                    int fitted;
                    
                    while (reader.Read())
                    {
                        switch (reader.Value.OpType)
                        {
                            case PDFTextOpType.TextContent:
                                var text = (PDFTextDrawOp) reader.Value;
                                var chars = text.Characters;
                                var w = context.Graphics.MeasureString(chars, 0, context.Space, textOptions,
                                    out fitted, out charFitted);
                                if (fitted < chars.Length)
                                    chars = chars.Substring(0, fitted);
                                context.Graphics.FillText(chars);
                                break;
                            default:
                                //do nothing
                            break;
                        }
                    }
                    
                }
            }
            finally
            {
                context.Graphics.EndText();
                context.Graphics.RestoreGraphicsState();
                
                context.Offset = origOffset;
                context.Space = origSize;
            }

            return oref;
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
