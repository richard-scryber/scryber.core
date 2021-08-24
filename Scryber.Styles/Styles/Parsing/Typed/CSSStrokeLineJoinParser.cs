using System;
using Scryber.Html;
using Scryber.Drawing;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSStrokeLineJoinParser : CSSStyleValueParser
    {
        public CSSStrokeLineJoinParser() : base(CSSStyleItems.StrokeLineCap)
        { }

        protected override bool DoSetStyleValue(Style style, CSSStyleItemReader reader)
        {
            var key = StyleKeys.StrokeJoinKey;
            var result = false;

            if (reader.ReadNextValue())
            {
                var val = reader.CurrentTextValue;
                LineJoin join;
                if (IsExpression(val))
                {
                    result = AttachExpressionBindingHandler(style, key, val, DoConvertLineJoin);
                }
                else if (TryParseLineJoin(val.ToLower(), out join))
                {
                    style.SetValue(key, join);
                    result = true;
                }
                else
                    result = false;
            }

            return result;
        }

        protected bool DoConvertLineJoin(StyleBase style, object value, out LineJoin join)
        {
            if(null == value)
            {
                join = LineJoin.Bevel;
                return false;
            }
            else if(value is LineJoin j)
            {
                join = j;
                return true;
            }
            else if(TryParseLineJoin(value.ToString(), out join))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool TryParseLineJoin(string value, out LineJoin join)
        {
            bool result;

            switch (value)
            {
                case ("bevel"):
                    join = LineJoin.Bevel;
                    result = true;
                    break;
                case ("round"):
                    join = LineJoin.Round;
                    result = true;
                    break;
                case ("mitre"):
                    join = LineJoin.Mitre;
                    result = true;
                    break;
                default:
                    join = LineJoin.Bevel;
                    result = false;
                    break;
            }
            return result;
        }
    }
}
