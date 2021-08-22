using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSMarginsRightParser : CSSThicknessValueParser
    {
        public CSSMarginsRightParser()
            : base(CSSStyleItems.MarginsRight, StyleKeys.MarginsRightKey)
        {
        }
    }
}
