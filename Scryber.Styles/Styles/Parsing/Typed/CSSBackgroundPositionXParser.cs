using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSBackgroundPositionXParser : CSSThicknessValueParser
    {
        public CSSBackgroundPositionXParser()
            : base(CSSStyleItems.BackgroundPositionX, StyleKeys.BgXPosKey)
        {
        }
    }
}
