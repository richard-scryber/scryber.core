using System;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{

    /// <summary>
    /// Parses the text-transform css property - none, uppercase, lowercase and capitalize
    /// </summary>
    public class CSSTextTransformParser : CSSStyleValueParser
    {

        public CSSTextTransformParser()
            : base(CSSStyleItems.TextTransform)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            if (!reader.ReadNextValue())
                return false;

            var val = reader.CurrentTextValue;

            if (IsExpression(val))
                return AttachExpressionBindingHandler(onStyle, StyleKeys.TextTransformKey, val, DoConvertTransform);

            if (TryParseTransform(val, out var transform))
            {
                onStyle.SetValue(StyleKeys.TextTransformKey, transform);
                return true;
            }

            return false;
        }

        protected bool DoConvertTransform(StyleBase style, object value, out Text.TextTransform transform)
        {
            if (null == value)
            {
                transform = Text.TextTransform.None;
                return false;
            }
            else if (value is Text.TextTransform t)
            {
                transform = t;
                return true;
            }
            else
                return TryParseTransform(value.ToString(), out transform);
        }

        protected static bool TryParseTransform(string value, out Text.TextTransform transform)
        {
            switch (value.Trim().ToLowerInvariant())
            {
                case "none":
                    transform = Text.TextTransform.None;
                    return true;
                case "uppercase":
                    transform = Text.TextTransform.Uppercase;
                    return true;
                case "lowercase":
                    transform = Text.TextTransform.Lowercase;
                    return true;
                case "capitalize":
                    transform = Text.TextTransform.Capitalize;
                    return true;
                default:
                    transform = Text.TextTransform.None;
                    return false;
            }
        }
    }
}
