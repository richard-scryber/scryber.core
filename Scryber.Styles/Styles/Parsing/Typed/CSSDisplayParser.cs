using System;
using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Parses and sets the components text decoration option based on the CSS names
    /// </summary>
    public class CSSDisplayParser : CSSEnumStyleParser<DisplayMode>
    {
        public CSSDisplayParser()
            : base(CSSStyleItems.Display, StyleKeys.PositionDisplayKey)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool result = true;
            DisplayMode display;
            if (reader.ReadNextValue())
            {
                string value = reader.CurrentTextValue;

                if (IsExpression(value))
                {
                    result = AttachExpressionBindingHandler(onStyle, this.StyleAttribute, value, DoConvertPosition);
                }
                else if (TryGetDisplayEnum(value, out display))
                {
                    this.SetValue(onStyle, display);
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            else
                result = false;

            return result;
        }


        protected bool DoConvertPosition(StyleBase style, object value, out DisplayMode display)
        {
            if(null == value)
            {
                display = DisplayMode.Block;
                return false;
            }
            else if(value is DisplayMode p)
            {
                display = p;
                return true;
            }
            else if(TryGetDisplayEnum(value.ToString(), out display))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool TryGetDisplayEnum(string value, out DisplayMode display)
        {
            switch (value.ToLower())
            {
                case ("inline"):
                    display = DisplayMode.Inline;
                    return true;
                case("inline-block"):
                    display = DisplayMode.InlineBlock;
                    return true;
                case ("block"):
                    display = DisplayMode.Block;
                    return true;
                case("table-cell"):
                    display = DisplayMode.TableCell;
                    return true;
                case ("none"):
                    display = DisplayMode.Invisible;
                    return true;
                default:
                    display = DisplayMode.Block;
                    return false;

            }
        }

    }
}
