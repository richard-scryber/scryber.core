using System;
namespace Scryber.Styles.Parsing.Typed
{
    public class CSSListItemAlignmentParser : CSSEnumStyleParser<HorizontalAlignment>
    {
        public CSSListItemAlignmentParser()
            : base("-pdf-li-align", StyleKeys.ListAlignmentKey)
        {
        }
    }
}
