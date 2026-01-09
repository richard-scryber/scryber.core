using System;
using Scryber.Html;
using Scryber.Drawing;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Parses and sets the components overflow option based on the CSS names
    /// </summary>
    public class CSSOverflowActionParser : CSSEnumStyleParser<OverflowAction>
    {
        public CSSOverflowActionParser()
            : base(CSSStyleItems.Overflow, StyleKeys.OverflowActionKey)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool result = true;
            OverflowAction over;
            if (reader.ReadNextValue())
            {
                string value = reader.CurrentTextValue;

                if (IsExpression(value))
                {
                    result = this.AttachExpressionBindingHandler(onStyle, this.StyleAttribute, value, DoConvertOverflow);
                }
                else if (TryGetOverflowEnum(value, out over))
                {
                    this.SetValue(onStyle, over);
                    result = true;
                }
                else
                    result = false;
            }
            else
                result = false;

            return result;
        }

        protected bool DoConvertOverflow(StyleBase style, object value, out OverflowAction over)
        {
            if (null == value)
            {
                over = OverflowAction.None;
                return false;
            }
            else if (value is OverflowAction action)
            {
                over = action;
                return true;
            }
            else if (TryGetOverflowEnum(value.ToString(), out over))
            {
                return true;
            }
            else
                return false;
        }

        public static bool IsOverflowEnum(string value)
        {
            OverflowAction over;
            return TryGetOverflowEnum(value, out over);
        }

        public static bool TryGetOverflowEnum(string value, out OverflowAction over)
        {
            switch (value.ToLower())
            {
                case ("auto"):
                case ("scroll"):
                case("new-page"):
                    over = OverflowAction.NewPage;
                    return true;

                case ("visible"):
                    over = OverflowAction.Visible;
                    return true;

                case ("hidden"):
                case("truncate"):
                    over = OverflowAction.Truncate;
                    return true;

                case ("clip"):
                    over = OverflowAction.Clip;
                    return true;

                default:
                    over = OverflowAction.None;
                    return false;

            }
        }

    }
}
