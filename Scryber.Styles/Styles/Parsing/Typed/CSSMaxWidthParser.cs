using System;
using Scryber.Html;
using Scryber.Drawing;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSMaxWidthParser : CSSUnitStyleParser
    {
        public CSSMaxWidthParser()
            : base(CSSStyleItems.MaximumWidth, StyleKeys.SizeMaximumWidthKey)
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
