using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Defines the 
    /// </summary>
    public class CSSColumnRuleParser : CSSBorderSideParser
    {

        public CSSColumnRuleParser() : base(CSSStyleItems.ColumnRule, StyleKeys.ColumnRuleWidthKey,
            StyleKeys.ColumnRuleColorKey, StyleKeys.ColumnRuleStyleKey, StyleKeys.ColumnRuleDashKey)
        {
            
        }
    }
}