using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSLeftParser : CSSUnitStyleParser
    {
        public CSSLeftParser()
            : base(CSSStyleItems.Left, StyleKeys.PositionXKey)
        {
        }

    }
}
