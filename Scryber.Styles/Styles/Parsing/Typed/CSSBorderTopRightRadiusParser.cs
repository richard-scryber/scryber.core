using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSBorderTopRightRadiusParser : CSSUnitStyleParser
    {
        public CSSBorderTopRightRadiusParser() : base(CSSStyleItems.BorderTopRightRadius, StyleKeys.BorderTopRightRadiusKey)
        { }
    }
}
