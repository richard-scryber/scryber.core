using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSPaddingAllParser : CSSThicknessAllParser
    {
        public CSSPaddingAllParser()
            : base(CSSStyleItems.Padding, StyleKeys.PaddingAllKey, StyleKeys.PaddingLeftKey, StyleKeys.PaddingTopKey, StyleKeys.PaddingRightKey, StyleKeys.PaddingBottomKey)
        {

        }
    }
}
