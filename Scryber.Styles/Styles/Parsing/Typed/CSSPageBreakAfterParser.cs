using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSPageBreakAfterParser : CSSPageBreakBeforeAfterParser
    {

        public CSSPageBreakAfterParser()
            : base(CSSStyleItems.PageBreakAfter, StyleKeys.PageBreakAfterKey)
        {
        }


    }
}
