using System;
using Scryber.Html;
using Scryber.Drawing;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Parses and sets the components background repeat option based on the CSS names
    /// </summary>
    public class CSSBackgroundRepeatParser : CSSEnumStyleParser<PatternRepeat>
    {
        public CSSBackgroundRepeatParser()
            : base(CSSStyleItems.BackgroundRepeat, StyleKeys.BgRepeatKey)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool result = true;
            PatternRepeat repeat;

            if (reader.ReadNextValue())
            {
                var str = reader.CurrentTextValue;
                if (IsExpression(str))
                {
                    result = this.AttachExpressionBindingHandler(onStyle, this.StyleAttribute, str, DoConvertRepeatEnum);
                }
                else if (TryGetRepeatEnum(reader.CurrentTextValue, out repeat))
                {
                    this.SetValue(onStyle, repeat);
                    result = true;
                }
                else
                    result = false;
            }
            else
                result = false;

            return result;
        }

        protected bool DoConvertRepeatEnum(StyleBase styleBase, object value, out PatternRepeat repeat)
        {
            if (null == value)
            {
                repeat = PatternRepeat.None;
                return false;
            }
            else if (value is PatternRepeat repeat1)
            {
                repeat = repeat1;
                return true;
            }
            else if (TryGetRepeatEnum(value.ToString(), out repeat))
            {
                return true;
            }
            else
                return false;
        }

        public static bool IsRepeatEnum(string value)
        {
            PatternRepeat repeat;
            return TryGetRepeatEnum(value, out repeat);
        }

        public static bool TryGetRepeatEnum(string value, out PatternRepeat repeat)
        {
            switch (value.ToLower())
            {
                case ("repeat"):
                    repeat = PatternRepeat.RepeatBoth;
                    return true;

                case ("repeat-x"):
                    repeat = PatternRepeat.RepeatX;
                    return true;

                case ("repeat-y"):
                    repeat = PatternRepeat.RepeatY;
                    return true;

                case ("no-repeat"):
                    repeat = PatternRepeat.None;
                    return true;

                default:
                    repeat = PatternRepeat.RepeatBoth;
                    return false;

            }
        }

    }
}
