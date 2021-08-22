using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSMarginsAllParser : CSSThicknessAllParser
    {
        public CSSMarginsAllParser()
            : base(CSSStyleItems.Margins,
                  StyleKeys.MarginsAllKey,
                  StyleKeys.MarginsLeftKey,
                  StyleKeys.MarginsTopKey,
                  StyleKeys.MarginsRightKey,
                  StyleKeys.MarginsBottomKey)
        {

        }
    }
}
