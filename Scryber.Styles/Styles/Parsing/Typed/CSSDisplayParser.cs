using System;
using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Parses and sets the components text decoration option based on the CSS names
    /// </summary>
    public class CSSDisplayParser : CSSEnumStyleParser<PositionMode>
    {
        public CSSDisplayParser()
            : base(CSSStyleItems.Display, StyleKeys.PositionModeKey)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool result = true;
            PositionMode display;
            if (reader.ReadNextValue())
            {
                string value = reader.CurrentTextValue;

                if (IsExpression(value))
                {
                    result = AttachExpressionBindingHandler(onStyle, this.StyleAttribute, value, DoConvertPosition);
                }
                if (TryGetPositionEnum(value, out display))
                {
                    this.SetValue(onStyle, display);
                    result = true;
                }
            }
            else
                result = false;

            return result;
        }


        protected bool DoConvertPosition(StyleBase style, object value, out PositionMode position)
        {
            if(null == value)
            {
                position = PositionMode.Block;
                return false;
            }
            else if(value is PositionMode p)
            {
                position = p;
                return true;
            }
            else if(TryGetPositionEnum(value.ToString(), out position))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool TryGetPositionEnum(string value, out PositionMode display)
        {
            switch (value.ToLower())
            {
                case ("inline"):
                    display = PositionMode.Inline;
                    return true;
                case("inline-block"):
                    display = PositionMode.InlineBlock;
                    return true;
                case ("block"):
                    display = PositionMode.Block;
                    return true;
                case ("none"):
                    display = PositionMode.Invisible;
                    return true;
                default:
                    display = PositionMode.Block;
                    return false;

            }
        }

    }
}
