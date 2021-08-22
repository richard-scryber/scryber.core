using System;
using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSBackgroundPositionYParser : CSSThicknessValueParser
    {
        public CSSBackgroundPositionYParser()
            : base(CSSStyleItems.BackgroundPositionY, StyleKeys.BgYPosKey)
        {
        }
    }
}
