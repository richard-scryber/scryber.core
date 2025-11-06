using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSHyphenateMinLengthParser : CSSIntStyleParser
    {

        public CSSHyphenateMinLengthParser() : base(CSSStyleItems.HyphenateLimitLength,
            StyleKeys.TextHyphenationMinLength)
        {
            
        }

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