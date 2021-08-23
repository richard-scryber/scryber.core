using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSColumnGapParser : CSSUnitStyleParser
    {
        public CSSColumnGapParser()
            : base(CSSStyleItems.ColumnGap, StyleKeys.ColumnAlleyKey)
        {
        }


    }
}
