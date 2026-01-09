using System;
using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSOverflowXParser : CSSStyleValueParser
    {
        public CSSOverflowXParser() : base(CSSStyleItems.OverflowX)
        {

        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool result = false;

            if (reader.ReadNextValue())
            {
                var val = reader.CurrentTextValue;

                if(IsExpression(val))
                {
                    result = this.AttachExpressionBindingHandler(onStyle, StyleKeys.ClipLeftKey, val, DoConvertOverflowX);
                    result &= this.AttachExpressionBindingHandler(onStyle, StyleKeys.ClipRightKey, val, DoConvertOverflowX);
                }
                else if(TryParseOverflow(val, out bool setValue))
                {
                    if(setValue)
                    {
                        onStyle.SetValue(StyleKeys.ClipLeftKey, (Unit)0.1);
                        onStyle.SetValue(StyleKeys.ClipRightKey, (Unit)0.1);
                        result = true;
                    }
                    else
                    {
                        onStyle.RemoveValue(StyleKeys.ClipLeftKey);
                        onStyle.RemoveValue(StyleKeys.ClipRightKey);
                        result = true;
                    }
                }
                
            }

            return result;

        }

        protected bool DoConvertOverflowX(StyleBase style, object value, out Unit clipping)
        {
            if(null == value)
            {
                clipping = Unit.Empty;
                return false;
            }
            else if(TryParseOverflow(value.ToString(), out bool clip))
            {
                if(!clip)
                {
                    //We actually perform the removal of the values from the style
                    //And then return false so no values are set.

                    style.RemoveValue(StyleKeys.ClipLeftKey);
                    style.RemoveValue(StyleKeys.ClipRightKey);
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
            bool parsed = false;
            switch (value)
            {
                case ("hidden"):
                case ("clip"):    
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
