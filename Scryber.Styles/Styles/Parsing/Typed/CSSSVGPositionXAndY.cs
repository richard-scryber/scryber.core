using Scryber.Html;
using Scryber.Drawing;

namespace Scryber.Styles.Parsing.Typed
{
    public abstract class CSSSVGPosition : CSSUnitStyleParser
    {
        public CSSSVGPosition(string styleAttr, StyleKey<Unit> styleKey) : base(styleAttr, styleKey) { }
    }

    public class CSSSVGx1Position : CSSSVGPosition
    {
         public CSSSVGx1Position() : base(CSSStyleItems.X1Position, StyleKeys.SVGGeometryGradientX1Key) { }
    }
    
    public class CSSSVGx2Position : CSSSVGPosition
    {
        public CSSSVGx2Position() : base(CSSStyleItems.X2Position, StyleKeys.SVGGeometryGradientX2Key) { }
    }
    
    public class CSSSVGy1Position : CSSSVGPosition
    {
        public CSSSVGy1Position() : base(CSSStyleItems.Y1Position, StyleKeys.SVGGeometryGradientY1Key) { }
    }
    
    public class CSSSVGy2Position : CSSSVGPosition
    {
        public CSSSVGy2Position() : base(CSSStyleItems.Y2Position, StyleKeys.SVGGeometryGradientY2Key) { }
    }

    
}