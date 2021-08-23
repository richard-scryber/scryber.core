using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSMinHeightParser : CSSUnitStyleParser
    {
        public CSSMinHeightParser()
            : base(CSSStyleItems.MinimumHeight, StyleKeys.SizeMinimumHeightKey)
        {
        }

    }
}
