using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSStrokeWidthParser : CSSUnitStyleParser
    {
        public CSSStrokeWidthParser() : base(CSSStyleItems.StrokeWidth, StyleKeys.StrokeWidthKey)
        { }
    }
}
