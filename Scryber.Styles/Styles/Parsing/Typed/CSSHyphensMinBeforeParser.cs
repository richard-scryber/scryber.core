using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Parses the number of required characters in a word before a split for hyphenation - an integer value or 'auto'
    /// </summary>
    public class CSSHyphensMinBeforeParser : CSSIntStyleParser
    {
        public CSSHyphensMinBeforeParser() : base(CSSStyleItems.HyphenationMinBefore, StyleKeys.TextHyphenationMinBeforeBreak) { }

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