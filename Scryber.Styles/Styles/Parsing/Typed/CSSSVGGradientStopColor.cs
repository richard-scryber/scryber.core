using Scryber.Html;
using System;
using Scryber.Drawing;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSSVGGradientStopColor : CSSColorStyleParser
    {

        public CSSSVGGradientStopColor() : this(CSSStyleItems.GradientStopColor, StyleKeys.SVGGradientStopColorKey,
            StyleKeys.SVGGradientStopOpacityKey)
        {
            
        }

        protected CSSSVGGradientStopColor(string attr, StyleKey<Color> colorKey, StyleKey<double> opacityKey)
            : base(attr, colorKey, opacityKey)
        {
            
        }

        
    }
}