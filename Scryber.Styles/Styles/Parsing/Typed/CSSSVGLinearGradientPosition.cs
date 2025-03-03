using Scryber.Html;
using Scryber.Drawing;

namespace Scryber.Styles.Parsing.Typed
{
    public abstract class CSSSVGLinearGradientPosition : CSSUnitStyleParser
    {
        public CSSSVGLinearGradientPosition(string styleAttr, StyleKey<Unit> styleKey) : base(styleAttr, styleKey) { }
    }

    public class CSSSVGLinearGradientX1Position : CSSSVGLinearGradientPosition
    {
         public CSSSVGLinearGradientX1Position() : base(CSSStyleItems.GradientX1Position, StyleKeys.SVGGeometryGradientX1Key) { }
    }
    
    public class CSSSVGLinearGradientX2Position : CSSSVGLinearGradientPosition
    {
        public CSSSVGLinearGradientX2Position() : base(CSSStyleItems.GradientX2Position, StyleKeys.SVGGeometryGradientX2Key) { }
    }
    
    public class CSSSVGLinearGradientY1Position : CSSSVGLinearGradientPosition
    {
        public CSSSVGLinearGradientY1Position() : base(CSSStyleItems.GradientY1Position, StyleKeys.SVGGeometryGradientY1Key) { }
    }
    
    public class CSSSVGLinearGradientY2Position : CSSSVGLinearGradientPosition
    {
        public CSSSVGLinearGradientY2Position() : base(CSSStyleItems.GradientY2Position, StyleKeys.SVGGeometryGradientY2Key) { }
    }

    public class CSSSVGLinearGradientSpreadMode : CSSEnumStyleParser<GradientSpreadMode>
    {
        public CSSSVGLinearGradientSpreadMode() : base(CSSStyleItems.GradientSpreadMode, StyleKeys.SVGGeometryGradientSpreadModeKey) { }
        
    }
}