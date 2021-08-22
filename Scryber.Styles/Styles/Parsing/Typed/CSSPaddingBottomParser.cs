using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSPaddingBottomParser : CSSThicknessValueParser
    {
        public CSSPaddingBottomParser()
            : base(CSSStyleItems.PaddingBottom, StyleKeys.PaddingBottomKey)
        {
        }
    }
}
