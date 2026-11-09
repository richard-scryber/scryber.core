using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSBottomParser : CSSUnitStyleParser
    {
        public CSSBottomParser()
            : base(CSSStyleItems.Bottom, StyleKeys.PositionBottomKey)
        {
        }
    }
}
