using System;
namespace Scryber.Styles.Parsing.Typed
{
    public class CSSListItemGroupParser : CSSStringParser
    {
        public CSSListItemGroupParser()
            : base("-pdf-li-group", StyleKeys.ListGroupKey)
        {
        }
    }
}
