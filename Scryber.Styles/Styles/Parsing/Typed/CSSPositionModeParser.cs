using System;
using Scryber.Text;
using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSPositionModeParser : CSSEnumStyleParser<PositionMode>
    {
        public CSSPositionModeParser() : base(CSSStyleItems.PositionModeType, StyleKeys.PositionModeKey)
        { }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool result = true;
            PositionMode found = PositionMode.Block;
            if (reader.ReadNextValue())
            {
                var val = reader.CurrentTextValue;

                if (IsExpression(val))
                {
                    result = AttachExpressionBindingHandler(onStyle, this.StyleAttribute, val, DoConvertPositionMode);
                }
                else if (TryGetPositionMode(val, out found))
                {
                    this.SetValue(onStyle, found);
                    result = true;
                }
                else
                    result = false;
            }
            else
                result = false;

            return result;
        }

        protected bool DoConvertPositionMode(StyleBase style, object value, out PositionMode mode)
        {
            if (null == value)
            {
                mode = PositionMode.Block;
                return false;
            }
            else if (value is PositionMode m)
            {
                mode = m;
                return true;
            }
            else if (TryGetPositionMode(value.ToString(), out mode))
            {
                return true;
            }
            else
                return false;

        }

        public static bool TryGetPositionMode(string value, out PositionMode mode)
        {
            mode = PositionMode.Block;
            if (string.IsNullOrEmpty(value))
                return false;

            switch (value.ToLower())
            {
                case ("relative"):
                    mode = PositionMode.Relative;
                    return true;

                case ("absolute"):
                    mode = PositionMode.Absolute;
                    return true;

                default:
                    return false;
            }
        }

    }
}
