using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{

    public class CSSFillOpacityParser : CSSOpacityParser
    {
        public CSSFillOpacityParser()
            : base(CSSStyleItems.FillOpacity, StyleKeys.FillOpacityKey)
        {
        }
    }
}
