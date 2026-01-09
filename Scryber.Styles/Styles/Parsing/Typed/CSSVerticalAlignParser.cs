using System;
using Scryber.Html;
using Scryber.Drawing;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSVerticalAlignParser : CSSStyleValueParser
    {
        public CSSVerticalAlignParser()
            : base(CSSStyleItems.VerticalAlign)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool success = true;
            VerticalAlignment align;

            if (!reader.ReadNextValue())
            {
                align = VerticalAlignment.Top;
                success = false;
            }
            else
            {
                var val = reader.CurrentTextValue;

                if(IsExpression(val))
                {
                    success = AttachExpressionBindingHandler(onStyle, StyleKeys.PositionVAlignKey, val, DoConvertVAlign);
                }
                else if(TryParseVAlign(val, out align))
                {
                    onStyle.SetValue(StyleKeys.PositionVAlignKey, align);
                    success = true;
                }
                else success = false;
            }

            return success;
        }

        protected bool DoConvertVAlign(StyleBase style, object value, out VerticalAlignment align)
        {
            if(null == value)
            {
                align = VerticalAlignment.Top;
                return false;
            }
            else if(value is VerticalAlignment v)
            {
                align = v;
                return true;
            }
            else if(TryParseVAlign(value.ToString(), out align))
            {
                return true;
            }
            else
            {
                align = VerticalAlignment.Top;
                return false;
            }    
        }


        public static bool TryParseVAlign(string value, out VerticalAlignment align)
        {
            bool success = true;

            switch (value)
            {
                case "top":
                    align = VerticalAlignment.Top;
                    break;
                case "bottom":
                    align = VerticalAlignment.Bottom;
                    break;
                case "middle":
                    align = VerticalAlignment.Middle;
                    break;
                case "baseline":
                    align = VerticalAlignment.Baseline;
                    break;
                default:
                    align = VerticalAlignment.Top;
                    success = false;
                    break;
            }

            return success;
        }
    }
}
