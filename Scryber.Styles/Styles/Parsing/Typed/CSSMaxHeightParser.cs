using System;
using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSMaxHeightParser : CSSUnitStyleParser
    {
        public CSSMaxHeightParser()
            : base(CSSStyleItems.MaximumHeight, StyleKeys.SizeMaximumHeightKey)
        {
        }

        protected override bool DoConvertUnit(StyleBase onStyle, object value, out Unit result)
        {
            if (null != value && value is string str && str.Equals("none", StringComparison.InvariantCultureIgnoreCase))
            {
                result = Unit.Auto;
                return true;
            }
            return base.DoConvertUnit(onStyle, value, out result);
        }
    }
}
