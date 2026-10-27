using System;
namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Parses a css color value into a PDFColor object for a PDFStyleKey
    /// </summary>
    public class CSSUrlStyleParser : CSSStyleAttributeParser<string>
    {
        private bool _allowGradients;

        public CSSUrlStyleParser(string styleItemKey, StyleKey<string> pdfAttr, bool allowGradients = true)
            : base(styleItemKey, pdfAttr)
        {
            _allowGradients = allowGradients;
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool result = false;
            string attrvalue;

            if (reader.ReadNextValue())
            {
                if(IsExpression(reader.CurrentTextValue))
                {
                    result = this.AttachExpressionBindingHandler(onStyle, this.StyleAttribute, reader.CurrentTextValue, this.DoConvertWithGradient);
                }
                else if (TryGetGradientExpression(reader.CurrentTextValue, out var gradientExpression))
                {
                    //A gradient that contains var() or calc() values, so needs to be evaluated.
                    result = this.AttachExpressionBindingHandler(onStyle, this.StyleAttribute, gradientExpression, this.DoConvertWithGradient);
                }
                else if(this.DoConvertWithGradient(onStyle, reader.CurrentTextValue, out attrvalue))
                {
                    onStyle.SetValue(this.StyleAttribute, attrvalue);
                    result = true;
                }
            }
            return result;
        }

        protected virtual bool DoConvertWithGradient(StyleBase onStyle, object value, out string result)
        {
            if(null == value)
            {
                result = string.Empty;
                return false;
            }
            var str = value.ToString();

            if (string.Equals("none", str, StringComparison.InvariantCultureIgnoreCase))
            {
                result = string.Empty;
                return true;
            }
            if (IsGradient(str, out result) || ParseCSSUrl(str, out result))
            {
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// If the value is a gradient that contains var() or calc() functions, 
        /// then builds a concat expression that will evaluate those functions within the gradient text.
        /// </summary>
        public static bool TryGetGradientExpression(string value, out string expression)
        {
            expression = null;

            if (!IsGradient(value, out _))
                return false;

            return TryGetEmbeddedExpression(value, out expression);
        }

        public static bool IsGradient(string value, out string finalValue)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (value.StartsWith("linear-gradient(")
                    || value.StartsWith("repeating-linear-gradient(")
                    || value.StartsWith("radial-gradient(")
                    || value.StartsWith("repeating-radial-gradient("))
                {
                    finalValue = value;
                    return true;
                }
            }
            finalValue = null;
            return false;
        }
    }
}
