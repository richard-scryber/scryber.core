using System;
using Scryber.Html;
using Scryber.Drawing;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Parses and sets the background values for a component based on the shorthand css background property
    /// </summary>
    public class CSSBackgroundParser : CSSStyleValueParser
    {
        public CSSBackgroundParser()
            : base(CSSStyleItems.Background)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            var values = new System.Collections.Generic.List<string>();
            while (reader.ReadNextValue())
                values.Add(reader.CurrentTextValue);

            if (values.Count == 0)
                return false;

            //An invalid value is ignored (as css), so check all the parts can be read before changing anything.
            if (!SetBackgroundValues(new Style(), values))
                return false;

            //The shorthand sets all the background values, so any that are not given are back to their initial values (as css).
            ResetBackground(onStyle);

            return SetBackgroundValues(onStyle, values);
        }

        private bool SetBackgroundValues(Style onStyle, System.Collections.Generic.List<string> values)
        {
            var state = new ShorthandState();

            foreach (var value in values)
            {
                state.Count++;

                //a size follows the position after a '/', e.g. center / cover or 0 0/100pt auto
                int slash = value.IndexOf('/');
                if (slash >= 0 && !IsExpression(value) && !IsUrl(value) && !CSSUrlStyleParser.IsGradient(value, out _))
                {
                    if (slash > 0 && !SetBackgroundValue(onStyle, value.Substring(0, slash), state))
                        return false;

                    state.AfterSlash = true;

                    if (slash < value.Length - 1 && !SetBackgroundValue(onStyle, value.Substring(slash + 1), state))
                        return false;
                }
                else if (!SetBackgroundValue(onStyle, value, state))
                    return false;
            }

            return true;
        }

        private class ShorthandState
        {
            public int Count;
            public int Positions;
            public int Sizes;
            public bool AfterSlash;
        }

        /// <summary>
        /// Sets the background values back to their initial values - transparent, no image, repeat, at 0 0 and the natural size.
        /// </summary>
        private static void ResetBackground(Style onStyle)
        {
            onStyle.Background.Color = Color.Transparent;
            onStyle.Background.Opacity = 1.0;
            onStyle.Background.ImageSource = string.Empty;
            onStyle.Background.PatternRepeat = PatternRepeat.RepeatBoth;
            onStyle.Background.PatternXPosition = Unit.Zero;
            onStyle.Background.PatternYPosition = Unit.Zero;
            onStyle.Background.PatternXSize = Unit.Zero;
            onStyle.Background.PatternYSize = Unit.Zero;
        }

        private bool SetBackgroundValue(Style onStyle, string value, ShorthandState state)
        {
            PatternRepeat repeat;
            Unit unit;

            if (CSSUrlStyleParser.IsGradient(value, out var gradient))
            {
                //linear-gradient, radial-gradient etc. are background images
                if (CSSUrlStyleParser.TryGetGradientExpression(gradient, out var gradientExpression))
                    return this.AttachExpressionBindingHandler(onStyle, StyleKeys.BgImgSrcKey, gradientExpression, DoConvertBackgroundImage);

                onStyle.Background.ImageSource = gradient;
                return true;
            }
            else if (IsExpression(value))
            {
                if (state.Count == 1)
                    return this.AttachExpressionBindingHandler(onStyle, StyleKeys.BgColorKey, value, DoConvertBackgroundColor);
                else if (state.Count == 2)
                    return this.AttachExpressionBindingHandler(onStyle, StyleKeys.BgImgSrcKey, value, DoConvertBackgroundImage);
                else if (state.Count == 3)
                    return this.AttachExpressionBindingHandler(onStyle, StyleKeys.BgRepeatKey, value, DoConvertBackgroundRepeat);
                else
                    return false;
            }

            var lower = value.ToLowerInvariant();

            if (state.AfterSlash)
            {
                //background-size, until a value that is not a size
                if (lower == "cover")
                {
                    onStyle.Background.PatternRepeat = PatternRepeat.Fill;
                    state.Sizes++;
                    return true;
                }
                else if (lower == "contain")
                {
                    onStyle.Background.PatternRepeat = PatternRepeat.Contain;
                    state.Sizes++;
                    return true;
                }
                else if (lower == "auto")
                {
                    state.Sizes++;
                    return true;
                }
                else if (Unit.TryParse(value, out unit) && !unit.IsRelative)
                {
                    if (state.Sizes == 0)
                        onStyle.Background.PatternXSize = unit;
                    else
                        onStyle.Background.PatternYSize = unit;
                    state.Sizes++;
                    return true;
                }
                else if (state.Sizes == 0)
                    return false; //a '/' must be followed by a size

                state.AfterSlash = false;
            }

            if (lower == "none")
            {
                //no image - it has already been reset
                return true;
            }
            else if (IsColor(value) || lower.StartsWith("hsl"))
            {
                if (!ParseCSSColor(value, out var color, out var opacity))
                    return false;

                onStyle.Background.Color = color;
                if (opacity.HasValue)
                    onStyle.Background.Opacity = opacity.Value;
                return true;
            }
            else if (IsUrl(value))
            {
                if (!ParseCSSUrl(value, out var url))
                    return false;

                onStyle.Background.ImageSource = url;
                return true;
            }
            else if (CSSBackgroundRepeatParser.TryGetRepeatEnum(value, out repeat))
            {
                onStyle.Background.PatternRepeat = repeat;
                return true;
            }
            else if (lower == "left" || lower == "top")
            {
                //the start of the axis, which is the initial value.
                state.Positions++;
                return true;
            }
            else if (lower == "right" || lower == "bottom" || lower == "center")
            {
                //positions from the end or centre are not supported, so they are accepted and stay at the start.
                state.Positions++;
                return true;
            }
            else if (Unit.TryParse(value, out unit))
            {
                //relative (percentage) positions are not supported, so they stay at the start.
                if (!unit.IsRelative)
                {
                    if (state.Positions == 0)
                        onStyle.Background.PatternXPosition = unit;
                    else
                        onStyle.Background.PatternYPosition = unit;
                }
                state.Positions++;
                return true;
            }
            else if (lower == "scroll" || lower == "fixed" || lower == "local"
                     || lower == "border-box" || lower == "padding-box" || lower == "content-box" || lower == "text")
            {
                //background-attachment, background-origin and background-clip have no meaning for a pdf page, so they are accepted and ignored.
                return true;
            }
            else
                return false;
        }

        protected bool DoConvertBackgroundColor(StyleBase onStyle, object value, out Color result)
        {
            double? opacity;

            if(null == value)
            {
                result = Color.Transparent;
                return false;
            }
            else if(value is Color color)
            {
                result = color;
                return true;
            }
            else if(ParseCSSColor(value.ToString(), out result, out opacity))
            {
                if (opacity.HasValue)
                    onStyle.SetValue(StyleKeys.BgOpacityKey, opacity.Value);
                return true;
            }
            else
            {
                result = Color.Transparent;
                return false;
            }
        }

        protected bool DoConvertBackgroundImage(StyleBase onStyle, object value, out string result)
        {
            if (null == value)
            {
                result = string.Empty;
                return false;
            }
            else
            {
                var str = value.ToString();
                result = str;
                return true;
            }
        }

        protected bool DoConvertBackgroundRepeat(StyleBase onStyle, object value, out PatternRepeat result)
        {
            if (null == value)
            {
                result = PatternRepeat.RepeatBoth;
                return false;
            }
            else if(CSSBackgroundRepeatParser.TryGetRepeatEnum(value.ToString(), out result))
            {
                return true;
            }
            else
            {
                result = PatternRepeat.RepeatBoth;
                return false;
            }
        }
    }
}
