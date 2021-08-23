using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSTopParser : CSSUnitStyleParser
    {
        public CSSTopParser()
            : base(CSSStyleItems.Top, StyleKeys.PositionYKey)
        {
        }
    }
}
