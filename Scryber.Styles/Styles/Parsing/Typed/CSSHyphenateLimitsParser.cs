using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Encapsultes the 3 values for length, min-before and min-after for hyphenation. Can be 1, 2 or 3 values separated by a space.
    /// </summary>
    public class CSSHyphenateLimitsParser : CSSStyleValueParser
    {
        public CSSHyphenateLimitsParser() : base(CSSStyleItems.HyphenateLimitChars)
        {
            
        }

        protected override bool DoSetStyleValue(Style style, CSSStyleItemReader reader)
        {
            int count = 0;
            int failed = 0;

            while (reader.ReadNextValue())
            {
                count++;

                if (IsExpression(reader.CurrentTextValue))
                {
                    if (count == 1) // first is always the length of the word
                    {
                        if (!AttachExpressionBindingHandler(style, StyleKeys.TextHyphenationMinLength, reader.CurrentTextValue, DoConvertHyphenValue))
                            failed++;
                    }
                    else if (count == 2)
                    {
                        if (!AttachExpressionBindingHandler(style, StyleKeys.TextHyphenationMinBeforeBreak,  reader.CurrentTextValue, DoConvertHyphenValue))
                            failed++;
                    }
                    else if (count == 3)
                    {
                        if (!AttachExpressionBindingHandler(style, StyleKeys.TextHyphenationMinAfterBreak,  reader.CurrentTextValue, DoConvertHyphenValue))
                            failed++;
                    }

                }
                else
                {
                    if (count == 1)
                    {
                        if (DoConvertHyphenValue(style, reader.CurrentTextValue, out var length))
                        {
                            style.SetValue(StyleKeys.TextHyphenationMinLength, length);
                        }
                        else
                            failed++;
                    }
                    else if(count == 2)

                    {
                        if (DoConvertHyphenValue(style, reader.CurrentTextValue, out var before))
                        {
                            style.SetValue(StyleKeys.TextHyphenationMinBeforeBreak, before);
                        }
                        else
                            failed++;

                    }
                    else if (count == 3)
                    {
                        if (DoConvertHyphenValue(style, reader.CurrentTextValue, out var after))
                        {
                            style.SetValue(StyleKeys.TextHyphenationMinAfterBreak, after);
                        }
                        else
                            failed++;
                    }
                    
                    //Ignore anything after 3 values.
                }

            }

            return count > 0 && failed == 0;
        }

        protected bool DoConvertHyphenValue(StyleBase style, object value, out int length)
        {
            length = CSSHyphensParser.AutoValue;

            if (null == value)
                return false;
            else if (value.ToString() == "auto")
            {
                return true; //OK
            }
            else if (value is int converted)
            {
                length = converted;
                return true;
            }           
            else if (int.TryParse(value.ToString(), out length))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        
    }
}