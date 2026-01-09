using System;
using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSColumnGapParser : CSSUnitStyleParser
    {
        public CSSColumnGapParser()
            : base(CSSStyleItems.ColumnGap, StyleKeys.ColumnAlleyKey)
        {
        }


        protected override bool DoConvertUnit(StyleBase onStyle, object value, out Unit result)
        {
            if (null != value && value is string str &&
                str.Equals("normal", StringComparison.InvariantCultureIgnoreCase))
            {
                result = ColumnWidths.UndefinedWidth;
                return true;
            }
            else
                return base.DoConvertUnit(onStyle, value, out result);

        }
    }
}
