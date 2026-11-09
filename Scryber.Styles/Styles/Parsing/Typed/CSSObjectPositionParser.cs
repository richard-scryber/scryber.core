using System;
using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Parses the object-position css property - one or two values that are keywords (left, center, right, top, bottom), lengths or percentages.
    /// The default is 50% 50% (centered).
    /// </summary>
    public class CSSObjectPositionParser : CSSStyleValueParser
    {
        public CSSObjectPositionParser()
            : base(CSSStyleItems.ObjectPosition)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            if (!reader.ReadNextValue())
                return false;

            var first = reader.CurrentTextValue;
            string second = reader.ReadNextValue() ? reader.CurrentTextValue : null;

            //more than 2 values are not supported
            if (reader.ReadNextValue())
                return false;

            if (IsExpression(first) || (null != second && IsExpression(second)))
            {
                //expressions are evaluated to the same single or double value
                var x = first;
                var y = second ?? first;
                return AttachExpressionBindingHandler(onStyle, StyleKeys.SizeObjectPositionXKey, x, DoConvertXPosition)
                       && AttachExpressionBindingHandler(onStyle, StyleKeys.SizeObjectPositionYKey, y, DoConvertYPosition);
            }

            if (TryParsePosition(first, second, out var ux, out var uy))
            {
                onStyle.SetValue(StyleKeys.SizeObjectPositionXKey, ux);
                onStyle.SetValue(StyleKeys.SizeObjectPositionYKey, uy);
                return true;
            }

            return false;
        }

        protected bool DoConvertXPosition(StyleBase style, object value, out ObjectPosition result)
        {
            return DoConvertAxis(value, true, out result);
        }

        protected bool DoConvertYPosition(StyleBase style, object value, out ObjectPosition result)
        {
            return DoConvertAxis(value, false, out result);
        }

        private bool DoConvertAxis(object value, bool horizontal, out ObjectPosition result)
        {
            result = ObjectPosition.Center;
            if (null == value)
                return false;
            else if (value is ObjectPosition p)
            {
                result = p;
                return true;
            }

            var parts = value.ToString().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0 || parts.Length > 2)
                return false;

            if (!TryParsePosition(parts[0], parts.Length > 1 ? parts[1] : null, out var ux, out var uy))
                return false;

            result = horizontal ? ux : uy;
            return true;
        }

        /// <summary>
        /// Parses one or two position values, in either order if they are keywords (e.g. 'top left' is the same as 'left top')
        /// </summary>
        public static bool TryParsePosition(string first, string second, out ObjectPosition x, out ObjectPosition y)
        {
            x = ObjectPosition.Center;
            y = ObjectPosition.Center;

            first = first.Trim().ToLowerInvariant();
            second = second?.Trim().ToLowerInvariant();

            if (null == second)
            {
                //a single value: a vertical keyword sets only the vertical, otherwise it is the horizontal
                if (first == "top" || first == "bottom")
                    return TryParseValue(first, out y);
                else
                    return TryParseValue(first, out x);
            }

            bool firstVertical = first == "top" || first == "bottom";
            bool secondHorizontal = second == "left" || second == "right";

            if (firstVertical || secondHorizontal)
            {
                //the keywords are the other way around
                if (first == "left" || first == "right" || second == "top" || second == "bottom")
                    return false;

                return TryParseValue(first, out y) && TryParseValue(second, out x);
            }
            else
            {
                if (first == "top" || first == "bottom" || second == "left" || second == "right")
                    return false;

                return TryParseValue(first, out x) && TryParseValue(second, out y);
            }
        }

        private static bool TryParseValue(string value, out ObjectPosition position)
        {
            switch (value)
            {
                case "left":
                case "top":
                    position = ObjectPosition.Start;
                    return true;
                case "center":
                    position = ObjectPosition.Center;
                    return true;
                case "right":
                case "bottom":
                    position = ObjectPosition.End;
                    return true;
                default:
                    position = ObjectPosition.Center;
                    if (!Unit.TryParse(value, out var unit))
                        return false;

                    if (unit.Units == PageUnits.Percent)
                        position = new ObjectPosition(unit.Value);
                    else if (!unit.IsRelative && !Unit.IsAutoValue(unit))
                        position = new ObjectPosition(unit);
                    else
                        return false; //other relative units (em etc.) are not supported

                    return true;
            }
        }
    }
}
