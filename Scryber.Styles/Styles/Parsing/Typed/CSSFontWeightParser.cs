using System;
using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Parses and sets the components italic option based on the CSS names
    /// </summary>
    public class CSSFontWeightParser : CSSStyleValueParser
    {
        public CSSFontWeightParser()
            : base(CSSStyleItems.FontWeight)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool bold = true;
            bool result = true;
            if (reader.ReadNextValue())
            {
                var str = reader.CurrentTextValue;
                if (IsExpression(str))
                {
                    result = AttachExpressionBindingHandler(onStyle, StyleKeys.FontBoldKey, str, DoConvertFontWeight);
                }
                else if (TryGetFontWeight(reader.CurrentTextValue, out bold))
                {
                    onStyle.SetValue(StyleKeys.FontBoldKey, bold);
                }
                else
                    result = false;
            }
            else
                result = false;

            return result;
        }

        public bool DoConvertFontWeight(StyleBase onStyle, object value, out bool fontBold)
        {
            if(null == value)
            {
                fontBold = false;
                return false;
            }
            else if(value is bool bold)
            {
                fontBold = bold;
                return true;
            }
            else if(TryGetFontWeight(value.ToString(), out bold))
            {
                fontBold = bold;
                return true;
            }
            else
            {
                fontBold = false;
                return false;
            }
        }

        public static bool IsFontWeight(string value)
        {
            bool italic;
            return TryGetFontWeight(value, out italic);
        }

        public static bool TryGetFontWeight(string value, out bool bold)
        {
            switch (value.ToLower())
            {
                case ("bold"):
                case ("bolder"):
                    bold = true;
                    return true;

                case ("normal"):
                case ("lighter"):
                    bold = false;
                    return true;

                case ("100"):
                case ("200"):
                case ("300"):
                case ("400"):
                case ("500"):
                    bold = false;
                    return true;

                case ("600"):
                case ("700"):
                case ("800"):
                case ("900"):
                    bold = true;
                    return true;
                default:
                    bold = false;
                    return false;

            }
        }

    }
}
