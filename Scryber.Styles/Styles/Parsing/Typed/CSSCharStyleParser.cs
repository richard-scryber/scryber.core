using System;
namespace Scryber.Styles.Parsing.Typed
{
	public class CSSCharStyleParser : CSSStyleAttributeParser<char>
	{
        public bool AllowQuotes { get; set; }

        public CSSCharStyleParser(string itemKey, StyleKey<char> attr, bool allowQuotes)
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
                else if (AllowQuotes && val.Length == 3)
                {
                    if (val[0] == '\'' && val[val.Length - 1] == '\'')
                    {
                        result = val[1];
                        return true;
                    }
                    else if (val[0] == '"' && val[val.Length - 1] == '"')
                    {
                        result = val[1];
                        return true;
                    }
                    else
                    {
                        result = ' ';
                        return false;
                    }
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

