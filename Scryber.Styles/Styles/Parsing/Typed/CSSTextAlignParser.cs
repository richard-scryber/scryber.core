using System;
using Scryber.Html;
using Scryber.Drawing;
using System.Security;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSTextAlignParser : CSSStyleValueParser
    {
        public CSSTextAlignParser()
            : base(CSSStyleItems.TextAlign)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool success = true;
            HorizontalAlignment align;
            if (!reader.ReadNextValue())
            {
                align = HorizontalAlignment.Left;
                success = false;
            }
            else
            {
                var val = reader.CurrentTextValue;

                if(IsExpression(val))
                {
                    success = AttachExpressionBindingHandler(onStyle, StyleKeys.PositionHAlignKey, val, DoConvertHAlign);
                }

                else if(TryParseHAlign(val, out align))
                {
                    onStyle.SetValue(StyleKeys.PositionHAlignKey, align);
                    success = true;
                }
                else
                {
                    success = false;
                }
                
            }
            return success;
        }

        protected bool DoConvertHAlign(StyleBase style, object value, out HorizontalAlignment align)
        {
            if(null == value)
            {
                align = HorizontalAlignment.Left;
                return false;
            }
            else if(value is HorizontalAlignment h)
            {
                align = h;
                return true;
            }
            else if(TryParseHAlign(value.ToString(), out align))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected static bool TryParseHAlign(string value, out HorizontalAlignment align)
        {
            bool success = true;

            switch (value.ToLower())
            {
                case "left":
                    align = HorizontalAlignment.Left;
                    break;
                case "right":
                    align = HorizontalAlignment.Right;
                    break;
                case "center":
                    align = HorizontalAlignment.Center;
                    break;
                case "justify":
                    align = HorizontalAlignment.Justified;
                    break;
                default:
                    align = HorizontalAlignment.Left;
                    success = false;
                    break;
            }

            return success;
        }
    }
}
