using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSColumnBreakBeforeParser : CSSColumnBreakBeforeAfterParser
    {

        public CSSColumnBreakBeforeParser()
            : base(CSSStyleItems.BreakBefore, StyleKeys.ColumnBreakBeforeKey)
        {
        }

        
    }
}
