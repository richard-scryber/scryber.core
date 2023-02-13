using System;
using Scryber.Html;
using Scryber.Drawing;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSListStyleParser : CSSStyleValueParser
    {

        public CSSListStyleParser()
            : base(CSSStyleItems.ListStyle)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool result;
            ListNumberingGroupStyle type;
            if (reader.ReadNextValue())
            {
                string val = reader.CurrentTextValue;

                if (IsExpression(val))
                {
                    result = AttachExpressionBindingHandler(onStyle, StyleKeys.ListNumberStyleKey, val, DoConvertListStyle);
                }
                else if (CSSListStyleTypeParser.TryGetListTypeEnum(reader.CurrentTextValue, out type))
                {
                    onStyle.SetValue(StyleKeys.ListNumberStyleKey, type);
                    result = true;
                }
                else
                    result = false;
            }
            else
            {
                result = false;
            }

            //make sure we read to the end of the style value
            while (reader.ReadNextValue())
                ;

            return result;

        }

        protected bool DoConvertListStyle(StyleBase style, object value, out ListNumberingGroupStyle num)
        {
            if (null == value)
            {
                num = ListNumberingGroupStyle.None;
                return false;
            }
            else if (value is ListNumberingGroupStyle g)
            {
                num = g;
                return true;
            }
            else if (CSSListStyleTypeParser.TryGetListTypeEnum(value.ToString(), out num))
            {
                return true;
            }
            else
                return false;
        }
    }
}
