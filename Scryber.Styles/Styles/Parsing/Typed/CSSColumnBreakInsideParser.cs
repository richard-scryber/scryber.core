using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSColumnBreakInsideParser :CSSBreakInsideParser
    {
        public CSSColumnBreakInsideParser()
        : base(CSSStyleItems.BreakInside, StyleKeys.OverflowActionKey, StyleKeys.OverflowSplitKey)
        {
        }
    }
}
