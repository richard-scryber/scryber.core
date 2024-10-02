using System;
using Scryber.Drawing;
using Scryber.Styles;
using Scryber.Components;
namespace Scryber.Svg.Components
{
    [PDFParsableComponent("tspan")]
    public class SVGTextSpan : TextLiteral, IStyledComponent
    {

        #region public PDFStyle Style {get;set;} + public bool HasStyle{get;}

        private Style _style;

        /// <summary>
        /// Gets the applied style for this page Component
        /// </summary>
        [PDFAttribute("style")]
        public virtual Style Style
        {
            get
            {
                if (_style == null)
                {
                    _style = new Style();
                    _style.Priority = Style.DirectStylePriority;
                }
                return _style;
            }
            set
            {
                this._style = value;
            }
        }

        /// <summary>
        /// Gets the flag to indicate if this page Component has style 
        /// information associated with it.
        /// </summary>
        public virtual bool HasStyle
        {
            get { return this._style != null && this._style.HasValues; }
        }

        #endregion
        

        [PDFAttribute("class")]
        public override string StyleClass { get => base.StyleClass; set => base.StyleClass = value; }
        
        

        [PDFAttribute("x")]
        public Unit X {
            get
            {
                StyleValue<Unit> value;
                if (this.HasStyle && this.Style.TryGetValue(StyleKeys.SVGGeometryXKey, out value))
                    return value.Value(this.Style);
                else
                    return Unit.Auto;
            }
            set
            {
                this.Style.SetValue(StyleKeys.SVGGeometryXKey, value);
            }
        }

        [PDFAttribute("y")]
        public Unit Y {
            get
            {
                StyleValue<Unit> value;
                if (this.HasStyle && this.Style.TryGetValue(StyleKeys.SVGGeometryYKey, out value))
                    return value.Value(this.Style);
                else
                    return Unit.Auto;
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
                if (this.HasStyle && this.Style.TryGetValue(StyleKeys.SVGGeometryDeltaXKey, out value))
                    return value.Value(this.Style);
                else
                    return Unit.Auto;
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
                if (this.HasStyle && this.Style.TryGetValue(StyleKeys.SVGGeometryDeltaYKey, out value))
                    return value.Value(this.Style);
                else
                    return Unit.Auto;
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
        
         [PDFAttribute("stroke")]
        public Color StrokeColor
        {
            get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.StrokeColorKey, StandardColors.Transparent);
                else
                {
                    return StandardColors.Transparent;
                }
            }
            set
            {
                this.Style.SetValue(StyleKeys.StrokeColorKey, value);
            }
        }


        [PDFAttribute("stroke-width")]
        public Unit StrokeWidth
        {
            get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.StrokeWidthKey, Unit.Empty);
                else
                    return Unit.Empty;
            }
            set
            { 
                this.Style.SetValue(StyleKeys.StrokeWidthKey, value);
            }
        }

        [PDFAttribute("stroke-linecap")]
        public string StrokeLineCap
        {
            get
            {
                if (this.HasStyle  && this.Style.IsValueDefined(StyleKeys.StrokeEndingKey))
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
                if (this.HasStyle && this.Style.IsValueDefined(StyleKeys.StrokeJoinKey))
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
        public Dash StrokeDashPattern
        {
            get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.StrokeDashKey, Dash.None);
                else
                    return Dash.None;
            }
            set
            {
                this.Style.SetValue(StyleKeys.StrokeDashKey, value);
            }
        }



        [PDFAttribute("stroke-opacity")]
        public double StrokeOpacity
        {
            get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.StrokeOpacityKey, 1.0);
                else
                {
                    return 1.0;
                }
            }
            set
            {
                this.Style.SetValue(StyleKeys.StrokeOpacityKey, value);
            }
        }

        [PDFAttribute("fill-opacity")]
        public double FillOpacity
        {
            get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.FillOpacityKey, 1.0);
                else
                {
                    return 1.0;
                }
            }
            set
            {
                this.Style.SetValue(StyleKeys.FillOpacityKey, value);
            }
        }

        // fill

        [PDFAttribute("fill")]
        public SVGFillValue FillColor
        {
            get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.SVGFillKey, SVGFillColorValue.Black);
                else
                {
                    return SVGFillColorValue.Black;
                }
            }
            set
            {
                this.Style.SetValue(StyleKeys.SVGFillKey, value);
            }
        }

        [PDFAttribute("font-family")]
        public FontSelector FontFamily
        {
            get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.FontFamilyKey, null);
                else
                {
                    return null;
                }
            }
            set
            {
                this.Style.SetValue(StyleKeys.FontFamilyKey, value);
            }
        }

        [PDFAttribute("font-size")]
        public Unit FontSize
        {
            get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.FontSizeKey, Unit.Auto);
                else
                {
                    return Unit.Auto;
                }
            }
            set
            {
                this.Style.SetValue(StyleKeys.FontSizeKey, value);
            }
        }
        
        [PDFAttribute("font-weight")]
        public int FontWeight
        {
            get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.FontWeightKey, FontWeights.Regular);
                else
                {
                    return FontWeights.Regular;
                }
            }
            set
            {
                this.Style.SetValue(StyleKeys.FontWeightKey, value);
            }
        }

        [PDFAttribute("font-style")]
        public Scryber.Drawing.FontStyle FontStyle
        {
            get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.FontStyleKey, Scryber.Drawing.FontStyle.Regular);
                else
                {
                    return Scryber.Drawing.FontStyle.Regular;
                }
            }
            set
            {
                this.Style.SetValue(StyleKeys.FontStyleKey, value);
            }
        }
        
        
        public SVGTextSpan() : base()
        {

        }
        

    }
}
