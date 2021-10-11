using System;
using Scryber.Html;
using Scryber.Drawing;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSOverflowYParser : CSSStyleValueParser
    {
        public CSSOverflowYParser() : base(CSSStyleItems.OverflowY)
        {

        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool result = false;

            if (reader.ReadNextValue())
            {
                var val = reader.CurrentTextValue;

                if (IsExpression(val))
                {
                    result = this.AttachExpressionBindingHandler(onStyle, StyleKeys.ClipTopKey, val, DoConvertOverflowY);
                    result &= this.AttachExpressionBindingHandler(onStyle, StyleKeys.ClipBottomKey, val, DoConvertOverflowY);
                }
                else if (TryParseOverflow(val, out bool setValue))
                {
                    if (setValue)
                    {
                        onStyle.SetValue(StyleKeys.ClipTopKey, (Unit)0.1);
                        onStyle.SetValue(StyleKeys.ClipBottomKey, (Unit)0.1);
                    }
                    else
                    {
                        onStyle.RemoveValue(StyleKeys.ClipTopKey);
                        onStyle.RemoveValue(StyleKeys.ClipBottomKey);
                    }
                }

            }

            return result;

        }

        protected bool DoConvertOverflowY(StyleBase style, object value, out Unit clipping)
        {
            if (null == value)
            {
                clipping = Unit.Empty;
                return false;
            }
            else if (TryParseOverflow(value.ToString(), out bool clip))
            {
                if (!clip)
                {
                    //We actually perform the removal of the values from the style
                    //And then return false so no values are set.

                    style.RemoveValue(StyleKeys.ClipTopKey);
                    style.RemoveValue(StyleKeys.ClipBottomKey);
                    clipping = Unit.Empty;
                    return false;
                }
                else
                {
                    clipping = (Unit)0.1;
                    return true;
                }
            }
            else
            {
                clipping = Unit.Empty;
                return false;
            }
        }

        public static bool TryParseOverflow(string value, out bool setClip)
        {
            bool parsed;

            switch (value)
            {
                case ("hidden"):
                    setClip = true;
                    parsed = true;
                    break;

                case ("auto"):
                case ("initial"):
                case ("visible"):
                    setClip = false;
                    parsed = true;
                    break;

                default:
                    setClip = false;
                    parsed = false;
                    break;
            }
            return parsed;
        }
    }
}
