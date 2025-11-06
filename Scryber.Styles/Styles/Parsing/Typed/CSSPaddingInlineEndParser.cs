using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSPaddingInlineEndParser : CSSThicknessValueParser
    {
        public CSSPaddingInlineEndParser()
            : base(CSSStyleItems.PaddingInlineEnd, StyleKeys.PaddingInlineEnd)
        {
        }
    }
}
