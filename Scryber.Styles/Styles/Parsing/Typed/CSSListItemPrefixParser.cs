using System;
namespace Scryber.Styles.Parsing.Typed
{
    public class CSSListItemPrefixParser : CSSStringParser
    {
        public CSSListItemPrefixParser()
            : base("-pdf-li-prefix", StyleKeys.ListPrefixKey)
        { 
        }
    }
}
