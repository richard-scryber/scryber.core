using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSBorderRadiusParser : CSSUnitStyleParser
    {
        public CSSBorderRadiusParser() : base(CSSStyleItems.BorderRadius, StyleKeys.BorderCornerRadiusKey)
        { }
    }
}
