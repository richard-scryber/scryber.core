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

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            // Call base to set ColumnAlleyKey (used by multi-column layout) and
            // also set FlexColumnGapKey so that gap + column-gap declarations
            // compete on the same key and CSS order determines the winner.
            bool result = base.DoSetStyleValue(onStyle, reader);
            if (result && onStyle.IsValueDefined(StyleKeys.ColumnAlleyKey))
                onStyle.SetValue(StyleKeys.FlexColumnGapKey,
                    onStyle.GetValue(StyleKeys.ColumnAlleyKey, Unit.Zero));
            return result;
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
