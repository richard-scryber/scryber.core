using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSColumnFillParser : CSSEnumStyleParser<ColumnFillMode>
    {
        public CSSColumnFillParser() : base(CSSStyleItems.ColumnFill, StyleKeys.ColumnFillKey)
        {}


        protected override bool DoConvertEnum(StyleBase onStyle, object value, out ColumnFillMode result)
        {
            //for the conversion of balance-all to Balance_All
            
            if(value is string str && str.IndexOf('-') >= 0)
                value = str.Replace("-", "_");
            
            return base.DoConvertEnum(onStyle, value, out result);
        }
    }
    
}