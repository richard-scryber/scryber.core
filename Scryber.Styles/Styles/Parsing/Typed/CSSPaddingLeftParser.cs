using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSPaddingLeftParser : CSSThicknessValueParser
    {
        public CSSPaddingLeftParser()
            : base(CSSStyleItems.PaddingLeft, StyleKeys.PaddingLeftKey)
        {
        }
    }
}
