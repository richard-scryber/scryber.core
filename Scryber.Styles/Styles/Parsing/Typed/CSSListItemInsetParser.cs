using System;
namespace Scryber.Styles.Parsing.Typed
{
    public class CSSListItemInsetParser : CSSUnitStyleParser
    {
        public CSSListItemInsetParser()
            : base("-pdf-li-inset", StyleKeys.ListInsetKey)
        {
        }
    }
}
