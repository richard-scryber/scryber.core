using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSBorderColorParser : CSSColorStyleParser
    {

        public CSSBorderColorParser()
            : base(CSSStyleItems.BorderColor, StyleKeys.BorderColorKey, StyleKeys.BorderOpacityKey)
        {
        }
    }

    public class CSSBorderLeftColorParser : CSSColorStyleParser
    {

        public CSSBorderLeftColorParser()
            : base(CSSStyleItems.BorderLeftColor, StyleKeys.BorderLeftColorKey, null)
        {
        }
    }

    public class CSSBorderTopColorParser : CSSColorStyleParser
    {

        public CSSBorderTopColorParser()
            : base(CSSStyleItems.BorderTopColor, StyleKeys.BorderTopColorKey, null)
        {
        }
    }

    public class CSSBorderBottomColorParser : CSSColorStyleParser
    {

        public CSSBorderBottomColorParser()
            : base(CSSStyleItems.BorderBottomColor, StyleKeys.BorderBottomColorKey, null)
        {
        }
    }

    public class CSSBorderRightColorParser : CSSColorStyleParser
    {

        public CSSBorderRightColorParser()
            : base(CSSStyleItems.BorderRightColor, StyleKeys.BorderRightColorKey, null)
        {
        }
    }
}
