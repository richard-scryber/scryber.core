using System;
using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Parses the object-fit css property - fill (default), contain, cover, none or scale-down
    /// </summary>
    public class CSSObjectFitParser : CSSStyleValueParser
    {
        public CSSObjectFitParser()
            : base(CSSStyleItems.ObjectFit)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            if (!reader.ReadNextValue())
                return false;

            var val = reader.CurrentTextValue;

            if (IsExpression(val))
                return AttachExpressionBindingHandler(onStyle, StyleKeys.SizeObjectFitKey, val, DoConvertFit);

            if (TryParseFit(val, out var fit))
            {
                onStyle.SetValue(StyleKeys.SizeObjectFitKey, fit);
                return true;
            }

            return false;
        }

        protected bool DoConvertFit(StyleBase style, object value, out ObjectFit fit)
        {
            if (null == value)
            {
                fit = ObjectFit.Fill;
                return false;
            }
            else if (value is ObjectFit f)
            {
                fit = f;
                return true;
            }
            else
                return TryParseFit(value.ToString(), out fit);
        }

        public static bool TryParseFit(string value, out ObjectFit fit)
        {
            switch (value.Trim().ToLowerInvariant())
            {
                case "fill":
                    fit = ObjectFit.Fill;
                    return true;
                case "contain":
                    fit = ObjectFit.Contain;
                    return true;
                case "cover":
                    fit = ObjectFit.Cover;
                    return true;
                case "none":
                    fit = ObjectFit.None;
                    return true;
                case "scale-down":
                    fit = ObjectFit.ScaleDown;
                    return true;
                default:
                    fit = ObjectFit.Fill;
                    return false;
            }
        }
    }
}
