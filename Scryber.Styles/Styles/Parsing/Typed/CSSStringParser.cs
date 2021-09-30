using System;
namespace Scryber.Styles.Parsing.Typed
{
    public class CSSStringParser : CSSStyleValueParser
    {
        public StyleKey<string> StyleKey { get; protected set; }

        public CSSStringParser(string name, StyleKey<string> styleKey)
            : base(name)
        {
            this.StyleKey = styleKey;
        }

        protected override bool DoSetStyleValue(Style style, CSSStyleItemReader reader)
        {
            bool success = false;
            if(!reader.ReadNextValue())
            {
                return success;
            }
            else
            {
                var val = reader.CurrentTextValue;

                if(IsExpression(val))
                {
                    success = AttachExpressionBindingHandler(style, this.StyleKey, val, DoConvertString);
                }
                else if(!string.IsNullOrEmpty(val))
                {
                    //remove any spaces at the start and end
                    string str = val.ToString().Trim();

                    //if in quotes then remove the quotes (single or double)

                    if (str.StartsWith("'") && str.EndsWith("'"))
                        str = str.Substring(1, str.Length - 2);

                    else if (str.StartsWith("\"") && str.EndsWith("\""))
                        str = str.Substring(1, str.Length - 2);

                    style.SetValue(this.StyleKey, str);
                    success = true;
                }
                else
                {
                    success = false;
                }

                return success;
            }    
        }


        protected bool DoConvertString(StyleBase style, object value, out string converted)
        {
            if(null == value)
            {
                converted = null;
                return false;
            }
            else
            {
                converted = value.ToString();
                return true;
            }
        }
    }
}
