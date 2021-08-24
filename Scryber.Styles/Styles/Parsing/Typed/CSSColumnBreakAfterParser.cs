using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSColumnBreakAfterParser : CSSBreakBeforeAfterParser
    {

        public CSSColumnBreakAfterParser()
            : base(CSSStyleItems.BreakAfter, StyleKeys.ColumnBreakAfterKey)
        {
        }


    }
}
