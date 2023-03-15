using System;
using Scryber.Html;
using Scryber.Drawing;
using System.Security;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Parses the number of required characters in a word before a split for hypenation
    /// </summary>
    public class CSSHyphensMinBeforeParser : CSSIntStyleParser
    {
        public CSSHyphensMinBeforeParser() : base(CSSStyleItems.HyphenationMinBefore, StyleKeys.TextHyphenationMinBeforeBreak) { }
    }

    /// <summary>
    /// Parses the number of required characters in a word after a split for hypenation
    /// </summary>
    public class CSSHyphensMinAfterParser : CSSIntStyleParser
    {
        public CSSHyphensMinAfterParser() : base(CSSStyleItems.HyphenationMinAfter, StyleKeys.TextHyphenationMinAfterBreak) { }
    }

    /// <summary>
    /// Parses the character that will be appended as a hyphen to any breaking words
    /// </summary>
    public class CSSHyphensCharAppendParser : CSSCharStyleParser
    {
        public CSSHyphensCharAppendParser() : base(CSSStyleItems.HyphenationMinAfter, StyleKeys.TextHyphenationCharAppend) { }
    }

    /// <summary>
    /// Parses the hypens css enumeration - supporting auto and none
    /// </summary>
    public class CSSHyphensParser : CSSStyleValueParser
    {
        public CSSHyphensParser()
            : base(CSSStyleItems.Hyphenation)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool success = true;
            Text.WordHyphenation hyphenate;
            if (!reader.ReadNextValue())
            {
                hyphenate = Text.WordHyphenation.None;
                success = false;
            }
            else
            {
                var val = reader.CurrentTextValue;

                if(IsExpression(val))
                {
                    success = AttachExpressionBindingHandler(onStyle, StyleKeys.TextWordHyphenation, val, DoConvertHyphenation);
                }

                else if(TryParseHyphenation(val, out hyphenate))
                {
                    onStyle.SetValue(StyleKeys.TextWordHyphenation, hyphenate);
                    success = true;
                }
                
            }
            return success;
        }

        protected bool DoConvertHyphenation(StyleBase style, object value, out Text.WordHyphenation hyphen)
        {
            if(null == value)
            {
                hyphen = Text.WordHyphenation.None;
                return false;
            }
            else if(value is Text.WordHyphenation h)
            {
                hyphen = h;
                return true;
            }
            else if(TryParseHyphenation(value.ToString(), out hyphen))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected static bool TryParseHyphenation(string value, out Text.WordHyphenation hyphen)
        {
            bool success = true;

            switch (value.ToLower())
            {
                case "auto":
                    hyphen = Text.WordHyphenation.Auto;
                    break;
                case "none":
                    hyphen = Text.WordHyphenation.None;
                    break;
                default:
                    hyphen = Text.WordHyphenation.None;
                    success = false;
                    break;
            }

            return success;
        }
    }
}
