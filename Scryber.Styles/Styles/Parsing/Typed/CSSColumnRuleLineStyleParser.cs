using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSColumnRuleLineStyleParser : CSSBorderStyleParser
    {
        public CSSColumnRuleLineStyleParser() : base(CSSStyleItems.ColumnRuleStyle, StyleKeys.ColumnRuleStyleKey,
            StyleKeys.ColumnRuleDashKey)
        {
            
        }
        
    }
}