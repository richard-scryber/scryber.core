using System;
using Scryber.Html;
using Scryber.Drawing;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Parses or binds the line height supporting propotional based on current fontsize if set.
    /// TODO:improve this to evaluation time
    /// </summary>
    public class CSSFontLineHeightParser : CSSUnitStyleParser
    {
        public CSSFontLineHeightParser()
            : base(CSSStyleItems.FontLineHeight, StyleKeys.TextLeadingKey)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            Unit absolute;
            bool result = false;

            if (reader.ReadNextValue())
            {
                var str = reader.CurrentTextValue;

                if(IsExpression(str))
                {
                    result = AttachExpressionBindingHandler(onStyle, StyleKeys.TextLeadingKey, str, DoConvertLineHeight);
                }
                else if(TryParseLineHeight(onStyle, str, out absolute))
                {
                    onStyle.SetValue(StyleKeys.TextLeadingKey, absolute);
                    result = true;
                }
            }
            return result;

        }

        protected bool DoConvertLineHeight(StyleBase onStyle, object value, out Unit result)
        {
            if(null == value)
            {
                result = Unit.Zero;
                return false;
            }
            else if(value is Unit unit)
            {
                result = unit;
                return true;
            }
            else if(TryParseLineHeight(onStyle, value.ToString(), out result))
            {
                return true;
            }
            else
            {
                result = Unit.Zero;
                return false;
            }    
        }


        public static bool TryParseLineHeight(StyleBase onStyle, string value, out Unit height)
        {
            double proportional;

            //special case of a single double value - is proportional to the current font size if set.
            if (double.TryParse(value, out proportional))
            {
                StyleValue<Unit> fsize;
                if (onStyle.TryGetValue(StyleKeys.FontSizeKey, out fsize))
                {
                    height = fsize.Value(onStyle) * proportional;
                    return true;
                }

            }

            //otherwise treat as an absolute unit.
            if (ParseCSSUnit(value, out height))
            {
                return true;
            }

            else
                return false;
        }
    }
}
