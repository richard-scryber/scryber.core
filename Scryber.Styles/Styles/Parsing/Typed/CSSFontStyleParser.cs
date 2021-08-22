using System;
using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Parses and sets the components italic option based on the CSS names
    /// </summary>
    public class CSSFontStyleParser : CSSStyleValueParser
    {
        public CSSFontStyleParser()
            : base(CSSStyleItems.FontStyle)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool italic = true;
            bool result = true;


            if (reader.ReadNextValue())
            {
                var str = reader.CurrentTextValue;
                if (IsExpression(str))
                {
                    result = AttachExpressionBindingHandler(onStyle, StyleKeys.FontItalicKey, str, DoConvertFontStyle);
                }
                else if (TryGetFontStyle(str, out italic))
                {
                    onStyle.SetValue(StyleKeys.FontItalicKey, italic);
                    result = true;
                }
                else
                    result = false;
            }
            else
                result = false;

            return result;
        }

        protected bool DoConvertFontStyle(StyleBase style, object value, out bool fontStyle)
        {
            if(null == value)
            {
                fontStyle = false;
                return false;
            }
            else if(value is bool ital)
            {
                fontStyle = ital;
                return true;
            }
            else if(TryGetFontStyle(value.ToString(), out ital))
            {
                fontStyle = ital;
                return true;
            }
            else
            {
                fontStyle = false;
                return false;
            }    
        }

        public static bool IsFontStyle(string value)
        {
            bool italic;
            return TryGetFontStyle(value, out italic);
        }

        public static bool TryGetFontStyle(string value, out bool italic)
        {
            switch (value.ToLower())
            {
                case ("italic"):
                case ("oblique"):
                    italic = true;
                    return true;

                case ("normal"):
                    italic = false;
                    return true;

                default:
                    italic = false;
                    return false;

            }
        }

    }
}
