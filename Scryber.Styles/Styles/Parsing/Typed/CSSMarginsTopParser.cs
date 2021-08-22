using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSMarginsTopParser : CSSThicknessValueParser
    {
        public CSSMarginsTopParser()
            : base(CSSStyleItems.MarginsTop, StyleKeys.MarginsTopKey)
        {
        }
    }
}
