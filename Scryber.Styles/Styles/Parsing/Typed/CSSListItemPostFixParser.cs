using System;
namespace Scryber.Styles.Parsing.Typed
{
    public class CSSListItemPostFixParser : CSSStringParser
    {
        public CSSListItemPostFixParser()
            : base("-pdf-li-postfix", StyleKeys.ListPostfixKey)
        {
        }
    }
}
