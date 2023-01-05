using System;
using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSContentParser : CSSStyleValueParser
    {
        public CSSContentParser()
            : base(CSSStyleItems.Content)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool result = true;
            ContentDescriptor full = null;
            bool bound = false;

            while (reader.ReadNextValue())
            {
                string value = reader.CurrentTextValue;

                ContentDescriptor parsed;
                if (IsExpression(value))
                {
                    if (null != full)
                        throw new ArgumentOutOfRangeException("Cannot bind individual transforms within multiple operations. Consider using a concat expression to build the string");

                    result &= AttachExpressionBindingHandler(onStyle, StyleKeys.ContentTextKey, value, DoConvertContent);
                    bound = true;
                }
                else if (DoConvertContent(onStyle, value, out parsed))
                {
                    if(bound)
                        throw new ArgumentOutOfRangeException("Cannot bind individual transforms within multiple operations. Consider using a concat expression to build the string");

                    if (null == full)
                        full = parsed;
                    else
                        full.Append(parsed);

                    result = true;
                }
                else
                    result = false;
            }

            if (null != full)
            {
                onStyle.SetValue(StyleKeys.ContentTextKey, full);
                return result;
            }
            else
            {
                return false;
            }
        }


        protected bool DoConvertFullContent(StyleBase style, object value, out ContentDescriptor descriptor)
        {
            if(null == value)
            {
                descriptor = null;
                return false;
            }
            var content = value.ToString().Trim();
            descriptor = null;
            bool parsed = false;

            while(string.IsNullOrEmpty(content) == false)
            {
                var one = ContentDescriptor.Parse(content);
                if(null != one)
                {
                    content = content.Substring(0, one.Value.Length).Trim();
                    parsed = true;
                }
            }


            return parsed;

        }


        protected bool DoConvertContent(StyleBase style, object value, out ContentDescriptor descriptor)
        {
            if(null == value)
            {
                descriptor = null;
                return false;
            }
            else
            {
                descriptor = ContentDescriptor.Parse(value.ToString());
                return true;
            }
        }


    }
}
