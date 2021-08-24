using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSColumnBreakBeforeParser : CSSBreakBeforeAfterParser
    {

        public CSSColumnBreakBeforeParser()
            : base(CSSStyleItems.BreakBefore, StyleKeys.ColumnBreakBeforeKey)
        {
        }

        
    }
}
