using System;
using System.Drawing.Text;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Parses a value into an enumeration value for a PDFStyleItem
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CSSEnumStyleParser<T> : CSSStyleAttributeParser<T> where T : struct
    {

        private const bool IgnoreCase = true;
        private Type _enumType;

        public CSSEnumStyleParser(string styleItemKey, PDFStyleKey<T> pdfAttr)
            : base(styleItemKey, pdfAttr)
        {
            _enumType = typeof(T);

            if (_enumType.IsEnum == false)
                throw new InvalidCastException("The type " + typeof(T).FullName + " is not an enumeration");
        }


        /// <summary>
        /// Overrides the base implementation to either set the value of the enumeration or attach an expression to the
        /// style for evaluation later on.
        /// </summary>
        /// <param name="onStyle">The style we are parsing for</param>
        /// <param name="reader">The style item reader</param>
        /// <returns>True the value was parsed or the expression was bound</returns>
        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool success = false;
            T result;

            if (reader.ReadNextValue())
            {
                if (IsExpression(reader.CurrentTextValue))
                {
                    this.AttachExpressionBindingHandler(onStyle, this.StyleAttribute, reader.CurrentTextValue, this.DoConvertEnum);
                }
                else if (this.DoConvertEnum(onStyle, reader.CurrentTextValue, out result))
                {
                    this.SetValue(onStyle, result);
                    success = true;
                }
            }
            return success;
        }

        /// <summary>
        /// Supports the conversion of the value to
        /// </summary>
        /// <param name="onStyle"></param>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected virtual bool DoConvertEnum(StyleBase onStyle, object value, out T result)
        {
            bool success = true;
            if (null == value)
            {
                success = false;
                result = default;
            }
            else if (value is T)
                result = (T)value;
            else if (value is string)
                success = Enum.TryParse<T>(value as string, IgnoreCase, out result);
            else
            {
                var str = (value).ToString();
                success = Enum.TryParse<T>(str, IgnoreCase, out result);
            }
            return success;
        }

    }
}
