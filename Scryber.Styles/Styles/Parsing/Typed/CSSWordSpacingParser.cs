using System;
using Scryber.Html;
using Scryber.Drawing;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Parses and sets the components italic option based on the CSS names
    /// </summary>
    public class CSSWordSpacingParser : CSSStyleValueParser
    {
        public static readonly Unit NormalSpacing = Unit.Zero;

        public CSSWordSpacingParser()
            : base(CSSStyleItems.WordSpacing)
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
                    result = AttachExpressionBindingHandler(onStyle, StyleKeys.TextWordSpacingKey, value, DoConvertWordSpacing);
                }
                else if (TryGetWordSpacing(reader.CurrentTextValue, out size))
                {
                    onStyle.SetValue(StyleKeys.TextWordSpacingKey, size);
                    result = true;
                }
                else
                    result = false;
            }
            else
                result = false;

            return result;
        }

        protected bool DoConvertWordSpacing(StyleBase style, object value, out Unit space)
        {
            if(null == value)
            {
                space = Unit.Empty;
                return false;
            }
            else if(value is Unit unit)
            {
                space = unit;
                return true;
            }
            else if(value is IFormattable)
            {
                var str = ((IFormattable)value).ToString(null, System.Globalization.CultureInfo.InvariantCulture);
                return ParseCSSUnit(str, out space);
            }
            else if(TryGetWordSpacing(value.ToString(), out space))
            {
                return true;
            }
            else
            {
                space = Unit.Empty;
                return false;
            }
        }


        public static bool TryGetWordSpacing(string value, out Unit size)
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
