using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSPaddingTopParser : CSSThicknessValueParser
    {
        public CSSPaddingTopParser()
            : base(CSSStyleItems.PaddingTop, StyleKeys.PaddingTopKey)
        {
        }
    }
}
