using System;
using Scryber.Html;
using Scryber.Drawing;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSFillColorParser : CSSColorStyleParser
    {
        public CSSFillColorParser()
            : this(CSSStyleItems.FillColor, StyleKeys.FillColorKey, StyleKeys.FillOpacityKey)
        {
        }

        public CSSFillColorParser(string css, StyleKey<Color> key, StyleKey<double> opacity)
            : base(css, key, opacity)
        { }

    }
}
