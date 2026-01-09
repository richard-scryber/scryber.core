using System;
namespace Scryber.Styles.Parsing.Typed
{
    public class CSSListItemPrefixParser : CSSStringParser
    {
        public CSSListItemPrefixParser()
            : base("-pdf-li-prefix", StyleKeys.ListPrefixKey)
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
