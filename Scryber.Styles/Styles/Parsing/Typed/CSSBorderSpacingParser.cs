using System;
using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Parses the border-spacing css property - one length for the horizontal and vertical spacing, or two lengths (horizontal then vertical)
    /// </summary>
    public class CSSBorderSpacingParser : CSSUnitStyleParser
    {
        public CSSBorderSpacingParser()
            : base(CSSStyleItems.BorderSpacing, StyleKeys.TableBorderSpacingHorizontalKey)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            if (!reader.ReadNextValue())
                return false;

            var first = reader.CurrentTextValue;
            string second = reader.ReadNextValue() ? reader.CurrentTextValue : null;

            //any other values are invalid
            if (reader.ReadNextValue())
                return false;

            if (null == second)
                second = first;

            return SetSpacing(onStyle, StyleKeys.TableBorderSpacingHorizontalKey, first)
                   && SetSpacing(onStyle, StyleKeys.TableBorderSpacingVerticalKey, second);
        }

        private bool SetSpacing(Style onStyle, StyleKey<Unit> key, string value)
        {
            if (IsExpression(value))
                return AttachExpressionBindingHandler(onStyle, key, value, this.DoConvertUnit);

            if (this.DoConvertUnit(onStyle, value, out var parsed) && (parsed.IsRelative || !Unit.IsAutoValue(parsed)))
            {
                onStyle.SetValue(key, parsed);
                return true;
            }

            return false;
        }
    }
}
