using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSMarginsInlineEndParser : CSSThicknessValueParser
    {
        public CSSMarginsInlineEndParser()
            : base(CSSStyleItems.MarginInlineEnd, StyleKeys.MarginsInlineStart)
        {
        }
    }
}
