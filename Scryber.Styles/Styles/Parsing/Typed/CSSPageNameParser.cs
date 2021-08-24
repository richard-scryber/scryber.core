using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSPageNameParser : CSSStyleValueParser
    {
        public CSSPageNameParser() : base(CSSStyleItems.PageGroupName)
        {
        }

        protected override bool DoSetStyleValue(Style style, CSSStyleItemReader reader)
        {
            if (reader.MoveToNextValue() && !string.IsNullOrEmpty(reader.CurrentTextValue))
            {
                string val = reader.CurrentTextValue;

                if (IsExpression(val))
                {
                    return AttachExpressionBindingHandler(style, StyleKeys.PageNameGroupKey, val, DoConvertPageName);
                }
                else
                {
                    style.PageStyle.PageNameGroup = reader.CurrentTextValue;
                    return true;
                }
            }
            else
                return false;
        }

        protected bool DoConvertPageName(StyleBase style, object value, out string name)
        {
            if(null == value)
            {
                name = string.Empty;
                return false;
            }
            else
            {
                name = value.ToString();
                return !string.IsNullOrEmpty(name);
            }
        }
    }
}
