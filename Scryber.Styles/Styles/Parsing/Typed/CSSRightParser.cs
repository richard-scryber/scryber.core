using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSRightParser : CSSUnitStyleParser
    {
        public CSSRightParser()
            : base(CSSStyleItems.Right, StyleKeys.PositionRightKey)
        {
        }

    }
}
