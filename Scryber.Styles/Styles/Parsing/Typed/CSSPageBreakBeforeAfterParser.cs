using System;
using System.Reflection;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSPageBreakBeforeAfterParser : CSSStyleAttributeParser<bool>
    {
        public CSSPageBreakBeforeAfterParser(string name, StyleKey<bool> key)
         : base(name, key)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool result;

            if (!reader.ReadNextValue())
                result = false;
            else
            {
                string val = reader.CurrentTextValue;
                bool breakval;

                if (IsExpression(val))
                {
                    result = AttachExpressionBindingHandler(onStyle, this.StyleAttribute, val, DoConvertBreakValue);
                }
                else if (TryParseBreak(val, out breakval))
                {
                    onStyle.SetValue(this.StyleAttribute, breakval);
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            return result;

        }


        protected bool DoConvertBreakValue(StyleBase style, object value, out bool converted)
        {
            if (null == value)
            {
                converted = false;
                return false;
            }
            else if (value is bool b)
            {
                converted = b;
                return true;
            }
            else if (TryParseBreak(value.ToString(), out converted))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool TryParseBreak(string value, out bool breakval)
        {
            bool result;
            switch (value)
            {
                case ("always"):
                case("all"):
                case ("left"):
                case ("right"):
                case("recto"):
                case("verso"):
                    breakval = true;
                    result = true;
                    break;

                case ("avoid"):
                case("auto"):
                    breakval = false;
                    result = true;
                    break;

                default:
                    breakval = false;
                    result = false;
                    break;
            }
            return result;
        }
    }
}
