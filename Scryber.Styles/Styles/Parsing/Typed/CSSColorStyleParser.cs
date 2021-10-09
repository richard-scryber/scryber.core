using System;
using Scryber.Drawing;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Parses a css color value into a PDFColor object for a PDFStyleKey
    /// </summary>
    public class CSSColorStyleParser : CSSStyleAttributeParser<Color>
    {
        private StyleKey<double> _opacity;

        public StyleKey<double> OpacityStyleKey
        {
            get { return _opacity; }
            protected set { _opacity = value; }
        }



        public CSSColorStyleParser(string styleItemKey, StyleKey<Color> pdfAttr, StyleKey<double> opacity)
            : base(styleItemKey, pdfAttr)
        {
            _opacity = opacity;
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool result = false;
            Color color;

            if (reader.ReadNextValue())
            {
                if (IsExpression(reader.CurrentTextValue))
                {
                    result = this.AttachExpressionBindingHandler(onStyle, this.StyleAttribute, reader.CurrentTextValue, this.DoConvertColor);
                }
                else if (this.DoConvertColor(onStyle, reader.CurrentTextValue, out color))
                {
                    onStyle.SetValue(this.StyleAttribute, color);
                    result = true;
                }
            }

            return result;
        }

        protected virtual bool DoConvertOpacity(StyleBase onStyle, object value, out double opacity)
        {
            Color parsed;
            
            if (null == value)
            {
                opacity = 1.0;
                return false;
            }
            else if (ParseCSSColor(value.ToString(), out parsed, out double? op))
            {
                opacity = op ?? 1.0;
                return op.HasValue;
            }
            else
            {
                opacity = 1.0;
                return false;
            }
        }

        protected virtual bool DoConvertColor(StyleBase onStyle, object value, out Color result)
        {
            if (null == value)
            {
                result = Color.Transparent;
                return false;
            }
            else if (value is Color)
            {
                result = (Color)value;
                return true;
            }
            else if (ParseCSSColor(value.ToString(), out result, out double? opacity))
            {
                if (onStyle.Immutable == false)
                {
                    if (opacity.HasValue && null != this._opacity)
                        onStyle.SetValue(this._opacity, opacity.Value);
                }
                return true;
            }
            else
            {
                result = Color.Transparent;
                return false;
            }
        }

    }
}
