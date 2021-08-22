using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSColumnCountParser : CSSStyleAttributeParser<int>
    {
        public CSSColumnCountParser()
            : base(CSSStyleItems.ColumnCount, StyleKeys.ColumnCountKey)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            int number;

            if (reader.ReadNextValue())
            {
                if (IsExpression(reader.CurrentTextValue))
                {
                    return this.AttachExpressionBindingHandler(onStyle, this.StyleAttribute, reader.CurrentTextValue, DoConvertColumnCount);
                }
                else if (ParseInteger(reader.CurrentTextValue, out number))
                {
                    this.SetValue(onStyle, number);
                    return true;
                }
            }
            return false;
        }

        protected bool DoConvertColumnCount(StyleBase style, object value, out int count)
        {
            if(null == value)
            {
                count = 1;
                return false;
            }
            else if(value is int i)
            {
                count = i;
                return true;
            }
            else if(ParseInteger(value.ToString(), out count))
            {
                return true;
            }
            else
            {
                count = 1;
                return false;
            }
        }
    }
}
