using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSMaxHeightParser : CSSUnitStyleParser
    {
        public CSSMaxHeightParser()
            : base(CSSStyleItems.MaximumHeight, StyleKeys.SizeMaximumHeightKey)
        {
        }

    }
}
