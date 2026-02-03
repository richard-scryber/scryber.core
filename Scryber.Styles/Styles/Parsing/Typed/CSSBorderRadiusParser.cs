using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSBorderRadiusParser : CSSThicknessAllParser
    {
        public CSSBorderRadiusParser() : base(CSSStyleItems.BorderRadius, StyleKeys.BorderCornerRadiusKey, StyleKeys.BorderBottomLeftRadiusKey, StyleKeys.BorderTopLeftRadiusKey, StyleKeys.BorderTopRightRadiusKey, StyleKeys.BorderBottomRightRadiusKey)
        { }
    }
}
