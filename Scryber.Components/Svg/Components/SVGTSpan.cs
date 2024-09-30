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

        //TODO: Support the x and y components on the class, if really nescessary - feels overkill.
        

        [PDFAttribute("class")]
        public override string StyleClass { get => base.StyleClass; set => base.StyleClass = value; }

        [PDFAttribute("x")]
        public Unit X {
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
        public Unit Y {
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
                this.Style.SetValue(StyleKeys.SVGGeometryYKey, value);
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
        
        public SVGTextSpan() : base()
        {

        }

    }
}
