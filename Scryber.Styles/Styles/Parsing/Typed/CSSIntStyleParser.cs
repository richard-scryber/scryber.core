using System;
namespace Scryber.Styles.Parsing.Typed
{
	public class CSSIntStyleParser : CSSStyleAttributeParser<int>
	{
		public CSSIntStyleParser(string itemKey, StyleKey<int> attr)
			: base(itemKey, attr)
		{
		}


        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool result = false;
            if (reader.ReadNextValue())
            {
                string value = reader.CurrentTextValue;

                int parsed;
                if (IsExpression(value))
                {
                    result = AttachExpressionBindingHandler(onStyle, this.StyleAttribute, value, this.DoConvertUnit);
                }
                if (this.DoConvertUnit(onStyle, value, out parsed))
                {
                    onStyle.SetValue(this.StyleAttribute, parsed);
                    result = true;
                }
            }
            return result;
        }

        protected virtual bool DoConvertUnit(StyleBase onStyle, object value, out int result)
        {
            if (null == value)
            {
                result = 0;
                return false;
            }
            else if (value is int)
            {
                result = (int)value;
                return true;
            }
            else if (int.TryParse(value.ToString(), out result))
                return true;
            else
                return false;
        }
    }
}

