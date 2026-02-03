using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSBorderBottomLeftRadiusParser : CSSUnitStyleParser
    {
        public CSSBorderBottomLeftRadiusParser() : base(CSSStyleItems.BorderBottomLeftRadius, StyleKeys.BorderBottomLeftRadiusKey)
        { }
    }
}
