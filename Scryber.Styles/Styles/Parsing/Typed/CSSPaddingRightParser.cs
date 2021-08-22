using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSPaddingRightParser : CSSThicknessValueParser
    {
        public CSSPaddingRightParser()
            : base(CSSStyleItems.PaddingRight, StyleKeys.PaddingRightKey)
        {
        }
    }
}
