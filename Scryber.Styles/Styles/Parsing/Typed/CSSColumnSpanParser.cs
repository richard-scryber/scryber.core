using System;
using Scryber.Html;
using Scryber.Drawing;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSColumnSpanParser : CSSStyleAttributeParser<int>
    {
        public CSSColumnSpanParser()
            : base(CSSStyleItems.ColumnSpan, StyleKeys.TableCellColumnSpanKey)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            int number;

            if (reader.ReadNextValue())
            {
                string val = reader.CurrentTextValue;

                if (IsExpression(val))
                {
                    return AttachExpressionBindingHandler(onStyle, this.StyleAttribute, val, DoConvertColumnSpan);
                }
                else if (ParseInteger(val, out number) && number >= 1)
                {
                    this.SetValue(onStyle, number);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
                return false;
        }

        protected bool DoConvertColumnSpan(StyleBase style, object value, out int span)
        {
            if(null == value)
            {
                span = 1;
                return false;
            }
            else if(value is int i)
            {
                if(i >= 1)
                {
                    span = i;
                    return true;
                }
                else
                {
                    span = 1;
                    return false;
                }
            }
            else if(ParseInteger(value.ToString(), out span) && span >= 1)
            {
                return true;
            }
            else
            {
                span = 1;
                return false;
            }
        }
    }
}
