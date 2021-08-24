using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSStrokeColorParser : CSSColorStyleParser
    {
        public CSSStrokeColorParser()
            : base(CSSStyleItems.StrokeColor, StyleKeys.StrokeColorKey, StyleKeys.StrokeOpacityKey)
        {

        }
    }
}
