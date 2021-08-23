using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSMinWidthParser : CSSUnitStyleParser
    {
        public CSSMinWidthParser()
            : base(CSSStyleItems.MinimumWidth, StyleKeys.SizeMinimumWidthKey)
        {
        }

    }
}
