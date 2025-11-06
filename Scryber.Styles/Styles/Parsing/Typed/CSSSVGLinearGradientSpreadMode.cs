using Scryber.Html;
using Scryber.Drawing;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSSVGLinearGradientSpreadMode : CSSEnumStyleParser<GradientSpreadMode>
    {
        public CSSSVGLinearGradientSpreadMode() : base(CSSStyleItems.GradientSpreadMode, StyleKeys.SVGGeometryGradientSpreadModeKey) { }
        
    }
}