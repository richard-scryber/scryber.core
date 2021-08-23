using System;
using Scryber.Html;
using Scryber.Drawing;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Parses and sets the components italic option based on the CSS names
    /// </summary>
    public class CSSLetterSpacingParser : CSSStyleValueParser
    {
        public static readonly PDFUnit NormalSpacing = PDFUnit.Zero;

        public CSSLetterSpacingParser()
            : base(CSSStyleItems.LetterSpacing)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            PDFUnit size = PDFUnit.Zero;
            bool result = true;
            if (reader.ReadNextValue())
            {
                var value = reader.CurrentTextValue;
                if (IsExpression(value))
                {
                    result = AttachExpressionBindingHandler(onStyle, StyleKeys.TextCharSpacingKey, value, DoConvertLetterSpacing);
                }
                else if (TryGetLetterSpacing(reader.CurrentTextValue, out size))
                {
                    onStyle.SetValue(StyleKeys.TextCharSpacingKey, size);
                    result = true;
                }
            }
            else
                result = false;

            return result;
        }

        protected bool DoConvertLetterSpacing(StyleBase style, object value, out PDFUnit spacing)
        {
            if(null == value)
            {
                spacing = PDFUnit.Empty;
                return false;
            }
            else if(value is PDFUnit unit)
            {
                spacing = unit;
                return true;
            }
            else if(TryGetLetterSpacing(value.ToString(), out spacing))
            {
                return true;
            }
            else
            {
                spacing = PDFUnit.Empty;
                return false;
            }

        }

        public static bool TryGetLetterSpacing(string value, out PDFUnit size)
        {
            switch (value.ToLower())
            {
                case ("normal"):
                    size = NormalSpacing;
                    return true;

                default:

                    if (CSSStyleValueParser.ParseCSSUnit(value, out size))
                        return true;
                    else
                        return false;

            }
        }

    }
}
