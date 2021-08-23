using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSHeightParser : CSSUnitStyleParser
    {
        public CSSHeightParser()
            : base(CSSStyleItems.Height, StyleKeys.SizeHeightKey)
        {
        }

    }
}
