using System;
namespace Scryber.Styles.Parsing.Typed
{
	public class CSSCharStyleParser : CSSStyleAttributeParser<char>
	{
		public CSSCharStyleParser(string itemKey, StyleKey<char> attr)
			: base(itemKey, attr)
		{
		}


        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool result = false;
            if (reader.ReadNextValue())
            {
                string value = reader.CurrentTextValue;

                char parsed;
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

        protected virtual bool DoConvertUnit(StyleBase onStyle, object value, out char result)
        {
            if (null == value)
            {
                result = ' ';
                return false;
            }
            else if (value is char)
            {
                result = (char)value;
                return true;
            }
            else
            {
                string val = value.ToString();
                if (string.IsNullOrEmpty(val))
                {
                    result = ' ';
                    return false;
                }
                else
                {
                    result = val[0];
                    return true;
                }
            }
        }
    }
}

