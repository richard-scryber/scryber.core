using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSColumnRuleColorParser : CSSColorStyleParser
    {

        public CSSColumnRuleColorParser() : base(CSSStyleItems.ColumnRuleColor,
            StyleKeys.ColumnRuleColorKey, StyleKeys.ColumnRuleOpacityKey)
        {
            
        }
        
    }
}