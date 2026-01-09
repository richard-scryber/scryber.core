using System;
namespace Scryber.Styles.Parsing.Typed
{
    public class CSSListItemPostFixParser : CSSStringParser
    {
        public CSSListItemPostFixParser()
            : base("-pdf-li-postfix", StyleKeys.ListPostfixKey)
        {
        }
        

        protected override bool DoConvertString(StyleBase style, object value, out string converted)
        {

            if (base.DoConvertString(style, value, out converted))
            {
                if(converted == "none")
                    converted = string.Empty;
                
                return true;
            }
            else
                return false;
        }
    }
}
