using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSZIndexParser : CSSStyleAttributeParser<int>
    {
        public CSSZIndexParser()
            : base(CSSStyleItems.ZIndex, StyleKeys.PositionZIndexKey)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            if (reader.ReadNextValue())
            {
                if (IsExpression(reader.CurrentTextValue))
                {
                    return this.AttachExpressionBindingHandler(onStyle, this.StyleAttribute, reader.CurrentTextValue, DoConvertZIndex);
                }
                else if (DoConvertZIndex(onStyle, reader.CurrentTextValue, out int index))
                {
                    this.SetValue(onStyle, index);
                    return true;
                }
            }
            return false;
        }

        protected bool DoConvertZIndex(StyleBase style, object value, out int index)
        {
            if (value == null || value.ToString() == "auto")
            {
                index = 0;
                return true;
            }
            else if (value is int i)
            {
                index = i;
                return true;
            }
            else if (ParseInteger(value.ToString(), out index))
            {
                return true;
            }
            else
            {
                index = 0;
                return false;
            }
        }
    }
}
