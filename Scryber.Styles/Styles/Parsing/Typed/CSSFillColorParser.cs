using System;
using Scryber.Html;
using Scryber.Drawing;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSFillColourParser : CSSColorStyleParser
    {
        public CSSFillColourParser()
            : this(CSSStyleItems.FillColor, StyleKeys.FillColorKey, StyleKeys.FillOpacityKey)
        {
        }

        public CSSFillColourParser(string css, StyleKey<PDFColor> key, StyleKey<double> opacity)
            : base(css, key, opacity)
        { }

    }
}
