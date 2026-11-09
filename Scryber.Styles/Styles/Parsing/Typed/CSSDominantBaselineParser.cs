using System;
using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
	public class CSSDominantBaselineParser : CSSEnumStyleParser<DominantBaseline>
    {
        public CSSDominantBaselineParser()
            : base(CSSStyleItems.DominantBaselineType, StyleKeys.DominantBaselineKey)
        {
        }


        protected override bool DoConvertEnum(StyleBase onStyle, object value, out DominantBaseline result)
        {
            //Dominant baseline values have a dash (-) in then that we replace with an underscore.
            if (value is string str && str.IndexOf('-') > -1)
                value = str.Replace('-', '_');

            return base.DoConvertEnum(onStyle, value, out result);
        }

    }
}

