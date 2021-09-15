using System;
namespace Scryber.Styles.Parsing.Typed
{
    public class CSSListConcatenationParser : CSSStyleValueParser
    {
        public CSSListConcatenationParser()
            : base("-pdf-list-concat")
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool success = true;
            bool concat;
            if (!reader.ReadNextValue())
            {
                concat = false;
                success = false;
            }
            else
            {
                var val = reader.CurrentTextValue;

                if (IsExpression(val))
                {
                    success = AttachExpressionBindingHandler(onStyle, StyleKeys.ListConcatKey, val, DoConvertConcatenation);
                }

                else if (DoConvertConcatenation(onStyle, val, out concat))
                {
                    onStyle.SetValue(StyleKeys.ListConcatKey, concat);
                    success = true;
                }

            }
            return success;
        }

        protected bool DoConvertConcatenation(StyleBase style, object value, out bool concat)
        {
            if (null == value)
            {
                concat = false;
                return false;
            }
            else if (value is bool)
            {
                concat = (bool)value;
                return true;
            }
            else
            {
                string str = value.ToString();

                if(string.IsNullOrEmpty(str))
                {
                    concat = false;
                    return false;
                }
                if (string.Equals("concatenate", str, StringComparison.OrdinalIgnoreCase))
                {
                    concat = true;
                    return true;
                }
                else if(string.Equals("true", str, StringComparison.OrdinalIgnoreCase))
                {
                    concat = true;
                    return true;
                }
                else if (string.Equals("1", str, StringComparison.Ordinal))
                {
                    concat = true;
                    return true;
                }
                else //any other value is considered valid but not to concatenate.
                {
                    concat = false;
                    return true;
                }
            }
        }
    }
}
