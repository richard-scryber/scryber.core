using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSMarginsBottomParser : CSSThicknessValueParser
    {
        public CSSMarginsBottomParser()
            : base(CSSStyleItems.MarginsBottom, StyleKeys.MarginsBottomKey)
        {
        }
    }
}
