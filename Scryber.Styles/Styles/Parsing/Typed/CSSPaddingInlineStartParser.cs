using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSPaddingInlineStartParser : CSSThicknessValueParser
    {
        public CSSPaddingInlineStartParser()
            : base(CSSStyleItems.PaddingInlineStart, StyleKeys.PaddingInlineStart)
        {
        }
    }
}
