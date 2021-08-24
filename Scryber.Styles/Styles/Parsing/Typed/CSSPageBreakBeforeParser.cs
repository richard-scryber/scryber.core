using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSPageBreakBeforeParser : CSSBreakBeforeAfterParser
    {

        public CSSPageBreakBeforeParser()
            : base(CSSStyleItems.PageBreakBefore, StyleKeys.PageBreakBeforeKey)
        {
        }


    }
}
