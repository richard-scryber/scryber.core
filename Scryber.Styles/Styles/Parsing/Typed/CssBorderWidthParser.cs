using System;
using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Overrides the standard CSSUnitStyleParser to support the 'thin', 'medium' and 'thick' values
    /// </summary>
    public class CSSBorderWidthParser : CSSUnitStyleParser
    {
        private static readonly Unit ThinSize = (Unit)0.2;
        private static readonly Unit MediumSize = (Unit)1.0;
        private static readonly Unit ThickSize = (Unit)3.0;

        public CSSBorderWidthParser()
            : this(CSSStyleItems.BorderWidth, StyleKeys.BorderWidthKey)
        {

        }

        public CSSBorderWidthParser(string attr, StyleKey<Unit> styleKey)
            : base(attr, styleKey)
        {

        }

        

        protected override bool DoConvertUnit(StyleBase onStyle, object value, out Unit result)
        {
            bool success = false;

            if(null == value)
            {
                result = Unit.Empty;
                return false;
            }
            var str = value.ToString();

            if (string.Equals("thin", str, StringComparison.OrdinalIgnoreCase))
            {
                result = ThinSize;
                success = true;
            }
            else if (string.Equals("medium", str, StringComparison.OrdinalIgnoreCase))
            {
                result = MediumSize;
                success = true;
            }
            else if (string.Equals("thick", str, StringComparison.OrdinalIgnoreCase))
            {
                result = ThickSize;
                success = true;
            }
            else
                success = base.DoConvertUnit(onStyle, value, out result);

            return success;
        }
    }

    public class CSSBorderLeftWidthParser : CSSBorderWidthParser
    {
        public CSSBorderLeftWidthParser() : base(CSSStyleItems.BorderLeftWidth, StyleKeys.BorderLeftWidthKey)
        {
        }
    }

    public class CSSBorderTopWidthParser : CSSBorderWidthParser
    {
        public CSSBorderTopWidthParser() : base(CSSStyleItems.BorderTopWidth, StyleKeys.BorderTopWidthKey)
        {
        }
    }

    public class CSSBorderRightWidthParser : CSSBorderWidthParser
    {
        public CSSBorderRightWidthParser() : base(CSSStyleItems.BorderRightWidth, StyleKeys.BorderRightWidthKey)
        {
        }
    }

    public class CSSBorderBottomWidthParser : CSSBorderWidthParser
    {
        public CSSBorderBottomWidthParser() : base(CSSStyleItems.BorderBottomWidth, StyleKeys.BorderBottomWidthKey)
        {
        }
    }
}
