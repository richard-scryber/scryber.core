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
        public static readonly Unit NormalSpacing = Unit.Zero;

        public CSSLetterSpacingParser()
            : base(CSSStyleItems.LetterSpacing)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            Unit size = Unit.Zero;
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

        protected bool DoConvertLetterSpacing(StyleBase style, object value, out Unit spacing)
        {
            if(null == value)
            {
                spacing = Unit.Empty;
                return false;
            }
            else if (value is string && (value as string).Equals("normal", StringComparison.InvariantCultureIgnoreCase))
            {
                spacing = NormalSpacing;
                return true;
            }
            else if (TryConvertToUnit(value, out spacing))
            {
                return true;
            }
            else
            {
                spacing = Unit.Empty;
                return false;
            }

        }

        public static bool TryGetLetterSpacing(string value, out Unit size)
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
