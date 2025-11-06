using Scryber.Html;


namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Parses the number of required characters in a word after a split for hypenation
    /// </summary>
    public class CSSHyphensMinAfterParser : CSSIntStyleParser
    {
        public CSSHyphensMinAfterParser() : base(CSSStyleItems.HyphenationMinAfter, StyleKeys.TextHyphenationMinAfterBreak) { }
        
        
        protected override bool DoConvertUnit(StyleBase onStyle, object value, out int result)
        {
            if (null == value)
            {
                result = 0;
                return false;
            }
            
            if (value.ToString() == "auto")
            {
                value = CSSHyphensParser.AutoValue;
            }
            
            return base.DoConvertUnit(onStyle, value, out result);
        }
    }
}