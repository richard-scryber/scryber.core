using System;
using System.Reflection;
using Scryber.Drawing;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSColumnBreakBeforeAfterParser : CSSStyleAttributeParser<BreakContentType>
    {
        public CSSColumnBreakBeforeAfterParser(string name, StyleKey<BreakContentType> key)
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
                BreakContentType breakval;

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


        protected bool DoConvertBreakValue(StyleBase style, object value, out BreakContentType converted)
        {
            if (null == value)
            {
                converted = BreakContentType.Auto;
                return false;
            }
            else if (value is BreakContentType b)
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

        public static bool TryParseBreak(string value, out BreakContentType breakval)
        {
            bool result;
            switch (value)
            {
                case("auto"):
                    breakval = BreakContentType.Auto;
                    result = true;
                    break;
                case ("always"):
                case("all"):
                    breakval = BreakContentType.All;
                    result = true;
                    break;
                case("avoid"):
                case("avoid-region"):
                case("avoid-column"):
                case("avoid-page"):
                    breakval = BreakContentType.Avoid;
                    result = true;
                    break;
                case ("region"):
                    breakval  = BreakContentType.Region;
                    result = true;
                    break;
                case ("column"):
                    breakval = BreakContentType.Column;
                    result = true;
                    break;
                case ("page"):
                    breakval  = BreakContentType.Page;
                    result = true;
                    break;
                
                case ("left"):
                    breakval =  BreakContentType.Left;
                    result = true;
                    break;
                case ("right"):
                    breakval = BreakContentType.Right;
                    result = true;
                    break;
                case("Recto"):
                    breakval =  BreakContentType.Recto;
                    result = true;
                    break;
                case("verso"):
                    breakval = BreakContentType.Verso;
                    result = true;
                    break;
                default:
                    breakval = BreakContentType.Auto;
                    result = false;
                    break;
            }
            return result;
        }
    }
}
