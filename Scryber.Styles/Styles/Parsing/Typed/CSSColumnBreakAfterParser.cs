using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSColumnBreakAfterParser : CSSColumnBreakBeforeAfterParser
    {

        public CSSColumnBreakAfterParser()
            : base(CSSStyleItems.BreakAfter, StyleKeys.ColumnBreakAfterKey)
        {
        }


    }
}
