using System;
using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSPositionFloatParser : CSSEnumStyleParser<FloatMode>
    {

        public CSSPositionFloatParser()
            : base(CSSStyleItems.Float, StyleKeys.PositionFloat)
        { }
    }
}
