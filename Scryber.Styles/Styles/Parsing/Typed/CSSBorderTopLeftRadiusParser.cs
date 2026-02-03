using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSBorderTopLeftRadiusParser : CSSUnitStyleParser
    {
        public CSSBorderTopLeftRadiusParser() : base(CSSStyleItems.BorderTopLeftRadius, StyleKeys.BorderTopLeftRadiusKey)
        { }
    }
}
