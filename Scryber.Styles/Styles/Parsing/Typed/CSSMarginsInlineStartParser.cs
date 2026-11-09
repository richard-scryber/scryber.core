using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSMarginsInlineStartParser : CSSThicknessValueParser
    {
        public CSSMarginsInlineStartParser()
            : base(CSSStyleItems.MarginInlineStart, StyleKeys.MarginsInlineStart)
        {
        }
    }
}
