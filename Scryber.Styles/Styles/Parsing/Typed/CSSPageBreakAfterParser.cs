using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSPageBreakAfterParser : CSSBreakBeforeAfterParser
    {

        public CSSPageBreakAfterParser()
            : base(CSSStyleItems.PageBreakAfter, StyleKeys.PageBreakAfterKey)
        {
        }


    }
}
