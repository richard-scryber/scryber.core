using System;
using Scryber.Html;
using Scryber.Drawing;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Parses and sets the components italic option based on the CSS names
    /// </summary>
    public class CSSFontSizeParser : CSSStyleValueParser
    {
        public static readonly PDFUnit XXSmallFontSize = new PDFUnit(6.0, PageUnits.Points);
        public static readonly PDFUnit XSmallFontSize = new PDFUnit(8.0, PageUnits.Points);
        public static readonly PDFUnit SmallFontSize = new PDFUnit(10.0, PageUnits.Points);
        public static readonly PDFUnit MediumFontSize = new PDFUnit(12.0, PageUnits.Points);
        public static readonly PDFUnit LargeFontSize = new PDFUnit(16.0, PageUnits.Points);
        public static readonly PDFUnit XLargeFontSize = new PDFUnit(24.0, PageUnits.Points);
        public static readonly PDFUnit XXLargeFontSize = new PDFUnit(32.0, PageUnits.Points);

        public CSSFontSizeParser()
            : base(CSSStyleItems.FontSize)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            PDFUnit size = PDFUnit.Zero;
            bool result = true;
            if (reader.ReadNextValue())
            {
                var str = reader.CurrentTextValue;
                if (IsExpression(str))
                {
                    result = this.AttachExpressionBindingHandler(onStyle, StyleKeys.FontSizeKey, str, DoConvertFontSize);
                }
                else
                {
                    onStyle.SetValue(StyleKeys.FontSizeKey, size);
                    result = true;
                }
            }
            else
                result = false;

            return result;
        }

        protected bool DoConvertFontSize(StyleBase style, object value, out PDFUnit size)
        {
            if(null == value)
            {
                size = PDFUnit.Empty;
                return false;
            }
            else if(value is PDFUnit unit)
            {
                size = unit;
                return true;
            }
            else if(TryGetFontSize(value.ToString(), out size))
            {
                return true;
            }
            else
            {
                size = PDFUnit.Zero;
                return false;
            }
        }

        public static bool IsFontSize(string value)
        {
            PDFUnit size;
            return TryGetFontSize(value, out size);
        }

        public static bool TryGetFontSize(string value, out PDFUnit size)
        {
            switch (value.ToLower())
            {
                case ("medium"):
                    size = MediumFontSize;
                    return true;

                case ("small"):
                    size = SmallFontSize;
                    return true;

                case ("x-small"):
                    size = XSmallFontSize;
                    return true;

                case ("xx-small"):
                    size = XXSmallFontSize;
                    return true;

                case ("large"):
                    size = LargeFontSize;
                    return true;

                case ("x-large"):
                    size = XLargeFontSize;
                    return true;

                case ("xx-large"):
                    size = XXLargeFontSize;
                    return true;

                case ("larger"):
                case ("smaller"):
                    size = PDFUnit.Zero;
                    return false;

                default:

                    if (CSSStyleValueParser.ParseCSSUnit(value, out size))
                        return true;
                    else
                        return false;

            }
        }

    }
}
