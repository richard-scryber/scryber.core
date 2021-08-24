using System;
using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSStrokeLineCapParser : CSSStyleValueParser
    {
        public CSSStrokeLineCapParser() : base(CSSStyleItems.StrokeLineCap)
        { }

        protected override bool DoSetStyleValue(Style style, CSSStyleItemReader reader)
        {
            var key = StyleKeys.StrokeEndingKey;
            bool result = false;
            LineCaps cap;

            if (reader.ReadNextValue())
            {
                var val = reader.CurrentTextValue.ToLower();
                if (IsExpression(val))
                {
                    result = AttachExpressionBindingHandler(style, key, val, DoConvertLineCap);
                }
                else if(TryParseLineCap(val.ToLower(), out cap))
                {
                    style.SetValue(key, cap);
                    result = true;
                }
                else
                {
                    result = false;
                }
            }


            return result;
        }

        protected bool DoConvertLineCap(StyleBase style, object value, out LineCaps cap)
        {
            if(null == value)
            {
                cap = LineCaps.Butt;
                return false;
            }
            else if(value is LineCaps c)
            {
                cap = c;
                return true;
            }
            else if(TryParseLineCap(value.ToString(), out cap))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool TryParseLineCap(string val, out LineCaps cap)
        {
            bool result;

            switch (val)
            {
                case ("butt"):
                    cap = LineCaps.Butt;
                    result = true;
                    break;
                case ("round"):
                    cap = LineCaps.Round;
                    result = true;
                    break;
                case ("square"):
                    cap = LineCaps.Square;
                    result = true;
                    break;
                default:
                    cap = LineCaps.Butt;
                    result = false;
                    break;
            }

            return result;
        }
    }
}
