using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSSVGGradientStopOpacity : CSSOpacityParser
    {

        public CSSSVGGradientStopOpacity() : this(CSSStyleItems.GradientStopOpacity, StyleKeys.SVGGradientStopOpacityKey)
        {
            
        }

        protected CSSSVGGradientStopOpacity(string attr, StyleKey<double> key)
            : base(attr, key)
        {
            
        }
        
    }
}