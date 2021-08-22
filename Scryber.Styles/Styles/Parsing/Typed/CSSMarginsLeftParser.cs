using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSMarginsLeftParser : CSSThicknessValueParser
    {
        public CSSMarginsLeftParser()
            : base(CSSStyleItems.MarginsLeft, StyleKeys.MarginsLeftKey)
        {
        }
    }
}
