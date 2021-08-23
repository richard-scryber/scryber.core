using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSMaxWidthParser : CSSUnitStyleParser
    {
        public CSSMaxWidthParser()
            : base(CSSStyleItems.MaximumWidth, StyleKeys.SizeMaximumWidthKey)
        {
        }

    }
}
