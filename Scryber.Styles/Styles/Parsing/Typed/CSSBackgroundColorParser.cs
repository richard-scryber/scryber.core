using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Parses and sets the background values for a component based on the shorthand css background property
    /// </summary>
    public class CSSBackgroundColorParser : CSSColorStyleParser
    {
        public CSSBackgroundColorParser()
            : base(CSSStyleItems.BackgroundColor, StyleKeys.BgColorKey, StyleKeys.BgOpacityKey)
        {
        }

    }
}
