using System;
namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Parses a css color value into a PDFColor object for a PDFStyleKey
    /// </summary>
    public class CSSUrlStyleParser : CSSStyleAttributeParser<string>
    {
        private bool _allowGradients;

        public CSSUrlStyleParser(string styleItemKey, PDFStyleKey<string> pdfAttr, bool allowGradients = true)
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
                    this.AttachExpressionBindingHandler(onStyle, this.StyleAttribute, reader.CurrentTextValue, this.DoConvertGradient);
                }
                else if(this.DoConvertGradient(onStyle, reader.CurrentTextValue, out attrvalue))
                {
                    onStyle.SetValue(this.StyleAttribute, attrvalue);
                    result = true;
                }
            }
            return result;
        }

        protected virtual bool DoConvertGradient(StyleBase onStyle, object value, out string result)
        {
            if(null == value)
            {
                result = string.Empty;
                return false;
            }
            var str = value.ToString();

            if (IsGradient(str, out result) || ParseCSSUrl(str, out result))
            {
                return true;
            }
            else
                return false;
        }

        private bool IsGradient(string value, out string finalValue)
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
