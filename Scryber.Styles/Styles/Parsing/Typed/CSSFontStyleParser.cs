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
            Drawing.FontStyle italic = Drawing.FontStyle.Regular;
            bool result = true;


            if (reader.ReadNextValue())
            {
                var str = reader.CurrentTextValue;
                if (IsExpression(str))
                {
                    result = AttachExpressionBindingHandler(onStyle, StyleKeys.FontStyleKey, str, DoConvertFontStyle);
                }
                else if (TryGetFontStyle(str, out italic))
                {
                    onStyle.SetValue(StyleKeys.FontStyleKey, italic);
                    result = true;
                }
                else
                    result = false;
            }
            else
                result = false;

            return result;
        }

        protected bool DoConvertFontStyle(StyleBase style, object value, out Drawing.FontStyle fontStyle)
        {
            if(null == value)
            {
                fontStyle = Drawing.FontStyle.Regular;
                return false;
            }
            else if(value is bool ital)
            {
                fontStyle = Drawing.FontStyle.Italic;
                return true;
            }
            else if(TryGetFontStyle(value.ToString(), out fontStyle))
            {
                return true;
            }
            else
            {
                fontStyle = Drawing.FontStyle.Regular;
                return false;
            }    
        }

        public static bool IsFontStyle(string value)
        {
            Drawing.FontStyle italic;
            return TryGetFontStyle(value, out italic);
        }

        public static bool TryGetFontStyle(string value, out Drawing.FontStyle italic)
        {
            switch (value.ToLower())
            {
                case ("italic"):
                    italic = Drawing.FontStyle.Italic;
                    return true;

                case ("oblique"):
                    italic = Drawing.FontStyle.Oblique;
                    return true;

                case ("normal"):
                    italic = Drawing.FontStyle.Regular;
                    return true;

                default:
                    italic = Drawing.FontStyle.Regular;
                    return false;

            }
        }

    }
}
