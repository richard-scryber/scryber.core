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

        public CSSFillColourParser(string css, PDFStyleKey<PDFColor> key, PDFStyleKey<double> opacity)
            : base(css, key, opacity)
        { }

    }
}
