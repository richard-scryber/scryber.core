using System;
namespace Scryber.Drawing
{
    /// <summary>
    /// Defines a single gradient color value with opacity and distance that can be used in a gradient
    /// </summary>
    public class GradientColor
    {
        //[Obsolete("PDF does not support opacity in gradients but retaining incase we can make it work", false)]
        public double? Opacity { get; set; }

        /// <summary>
        /// The offset of the color from the start (10%, 30% etc).
        /// </summary>
        public double? Distance { get; set; }


        /// <summary>
        /// The actual color value to be used in the gradient
        /// </summary>
        public Color Color { get; set; }

        public GradientColor(Color color) : this(color, null, null)
        {

        }

        public GradientColor(Color color, double? distance, double? opacity)
        {
            this.Color = color;
            this.Distance = distance;
            this.Opacity = opacity;
        }



        /// <summary>
        /// The length of the stop from the start of the gradient line (e.g. 112pt), if it was given as a length rather than a percentage.
        /// The distance is null until the length of the gradient line is known.
        /// </summary>
        public Unit? Length { get; set; }

        public static bool TryParse(string value, out GradientColor color)
        {
            return TryParse(value, null, out color);
        }

        /// <summary>
        /// Parses a single color stop e.g. red, #FF0000 20%, rgba(255,0,0,0.5) 112pt.
        /// If the stop is a length and the lengthBasis (in points) is known, then the distance is set as a percentage of that basis.
        /// </summary>
        public static bool TryParse(string value, double? lengthBasis, out GradientColor color)
        {
            color = null;
            if (null == value)
                return false;

            var parts = SplitOutsideParentheses(value.Trim());
            if (parts.Count < 1 || parts.Count > 2)
                return false;

            if (!TryParseStopColor(parts[0], out var colVal, out var opacity))
                return false;

            color = new GradientColor(colVal, null, opacity);

            if (parts.Count == 2 && !TryParseStopDistance(parts[1], lengthBasis, color))
            {
                color = null;
                return false;
            }

            return true;
        }

        private static bool TryParseStopColor(string value, out Color color, out double? opacity)
        {
            opacity = null;

            if (value.StartsWith("rgba(", StringComparison.OrdinalIgnoreCase))
                return Color.TryParseRGBA(value, out color, out opacity);

            if (value.StartsWith("hsl", StringComparison.OrdinalIgnoreCase))
                return Color.TryParseHSL(value, out color, out opacity);

            if (Html.CSSColors.Names2Colors.TryGetValue(value, out var colHex))
                value = colHex;

            return Color.TryParse(value, out color);
        }

        private static bool TryParseStopDistance(string value, double? lengthBasis, GradientColor color)
        {
            double distV;

            if (value.EndsWith("%"))
            {
                if (!double.TryParse(value.Substring(0, value.Length - 1), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out distV))
                    return false;

                color.Distance = distV;
                return true;
            }

            //a plain number is a percentage
            if (double.TryParse(value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out distV))
            {
                color.Distance = distV;
                return true;
            }

            if (Unit.TryParse(value, out var length) && !length.IsRelative)
            {
                color.Length = length;
                if (lengthBasis.HasValue && lengthBasis.Value > 0)
                    color.Distance = (length.PointsValue / lengthBasis.Value) * 100.0;
                return true;
            }

            //not a known distance, so leave it to be distributed with the others
            return true;
        }

        private static System.Collections.Generic.List<string> SplitOutsideParentheses(string value)
        {
            var parts = new System.Collections.Generic.List<string>();
            var current = new System.Text.StringBuilder();
            int depth = 0;

            foreach (var c in value)
            {
                if (c == '(')
                    depth++;
                else if (c == ')')
                    depth--;

                if (depth == 0 && char.IsWhiteSpace(c))
                {
                    if (current.Length > 0)
                        parts.Add(current.ToString());
                    current.Clear();
                }
                else
                    current.Append(c);
            }

            if (current.Length > 0)
                parts.Add(current.ToString());

            return parts;
        }
    }
}
