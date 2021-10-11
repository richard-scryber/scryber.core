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
        public static readonly Unit XXSmallFontSize = new Unit(6.0, PageUnits.Points);
        public static readonly Unit XSmallFontSize = new Unit(8.0, PageUnits.Points);
        public static readonly Unit SmallFontSize = new Unit(10.0, PageUnits.Points);
        public static readonly Unit MediumFontSize = new Unit(12.0, PageUnits.Points);
        public static readonly Unit LargeFontSize = new Unit(16.0, PageUnits.Points);
        public static readonly Unit XLargeFontSize = new Unit(24.0, PageUnits.Points);
        public static readonly Unit XXLargeFontSize = new Unit(32.0, PageUnits.Points);

        public CSSFontSizeParser()
            : base(CSSStyleItems.FontSize)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            Unit size = Unit.Zero;
            bool result = true;
            if (reader.ReadNextValue())
            {
                var str = reader.CurrentTextValue;
                if (IsExpression(str))
                {
                    result = this.AttachExpressionBindingHandler(onStyle, StyleKeys.FontSizeKey, str, DoConvertFontSize);
                }
                else if (TryGetFontSize(str, out size))
                {
                    onStyle.SetValue(StyleKeys.FontSizeKey, size);
                    result = true;
                }
                else
                    result = false;
            }
            else
                result = false;

            return result;
        }

        protected bool DoConvertFontSize(StyleBase style, object value, out Unit size)
        {
            if(null == value)
            {
                size = Unit.Empty;
                return false;
            }
            else if(value is Unit unit)
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
                size = Unit.Zero;
                return false;
            }
        }

        public static bool IsFontSize(string value)
        {
            Unit size;
            return TryGetFontSize(value, out size);
        }

        public static bool TryGetFontSize(string value, out Unit size)
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
                    size = Unit.Zero;
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
