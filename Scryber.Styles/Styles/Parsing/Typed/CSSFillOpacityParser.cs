using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{

    public class CSSFillOpacityParser : CSSOpacityParser
    {
        public CSSFillOpacityParser()
            : this(CSSStyleItems.FillOpacity, StyleKeys.FillOpacityKey)
        {
        }

        protected CSSFillOpacityParser(string attr, StyleKey<double> styleKey)
            : base(attr, styleKey)
        {

        }
    }
}
