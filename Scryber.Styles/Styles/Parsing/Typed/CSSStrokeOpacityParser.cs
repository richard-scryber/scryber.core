using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSStrokeOpacityParser : CSSOpacityParser
    {
        public CSSStrokeOpacityParser() : base(CSSStyleItems.StrokeOpacity, StyleKeys.StrokeOpacityKey)
        { }
    }
}
