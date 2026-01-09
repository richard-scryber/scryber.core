using System;
using Scryber.Drawing;
using Scryber.Html;
using Scryber.Text;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// 
    /// </summary>
    public class CSSWhiteSpaceParser : CSSStyleValueParser
    {
        public CSSWhiteSpaceParser()
            : base(CSSStyleItems.WhiteSpace)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool success = true;
            Text.WordWrap wrap;
            bool preserve;

            if (!reader.ReadNextValue())
            {
                wrap = WordWrap.Auto;
                preserve = false;
                success = false;
            }
            else if(IsExpression(reader.CurrentTextValue))
            {
                success = this.AttachExpressionBindingHandler(onStyle, StyleKeys.TextWordWrapKey, reader.CurrentTextValue, DoConvertWhitespace);
            }
            else if(TryParseWhitespace(reader.CurrentTextValue, out wrap, out preserve))
            {
                onStyle.SetValue(StyleKeys.TextWordWrapKey, wrap);
                onStyle.SetValue(StyleKeys.TextWhitespaceKey, preserve);
                success = true;
            }
            else
            {
                success = false;
            }
            return success;
        }

        protected bool DoConvertWhitespace(StyleBase style, object value, out WordWrap wrap)
        {
            if (null == value)
            {
                wrap = WordWrap.Auto;
                return false;
            }
            else if (TryParseWhitespace(value.ToString(), out wrap, out bool preserve))
            {
                style.SetValue(StyleKeys.TextWhitespaceKey, preserve);
                return true;
            }
            else
                return false;
        }


        public static bool TryParseWhitespace(string value, out WordWrap wrap, out bool preserve)
        {
            bool success;

            switch (value.ToLower())
            {
                case "normal":
                    wrap = WordWrap.Auto;
                    preserve = false;
                    success = true;
                    break;
                case "pre":
                    wrap = WordWrap.NoWrap;
                    preserve = true;
                    success = true;
                    break;
                case "nowrap":
                    wrap = WordWrap.NoWrap;
                    preserve = false;
                    success = true;
                    break;
                case "pre-wrap":
                    wrap = WordWrap.Auto;
                    preserve = true;
                    success = true;
                    break;
                case "pre-line":
                    wrap = WordWrap.Auto;
                    preserve = false;
                    success = true;
                    break;
                default:
                    wrap = WordWrap.Auto;
                    preserve = false;
                    success = false; //not supported
                    break;
            }

            return success;
        }
    }
}
