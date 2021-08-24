using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSPageBreakInsideParser : CSSBreakInsideParser
    {
        public CSSPageBreakInsideParser()
            : base(CSSStyleItems.PageBreakInside, StyleKeys.OverflowActionKey, StyleKeys.OverflowSplitKey)
        {
        }
    }
}
