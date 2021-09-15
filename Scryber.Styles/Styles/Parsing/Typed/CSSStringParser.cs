using System;
namespace Scryber.Styles.Parsing.Typed
{
    public class CSSStringParser : CSSStyleValueParser
    {
        public PDFStyleKey<string> StyleKey { get; protected set; }

        public CSSStringParser(string name, PDFStyleKey<string> styleKey)
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
                    style.SetValue(this.StyleKey, val.ToString());
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
