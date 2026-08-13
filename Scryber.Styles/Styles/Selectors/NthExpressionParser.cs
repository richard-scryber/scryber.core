using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Scryber.Styles.Selectors
{
    /// <summary>
    /// Parses the small CSS 'An+B' micro-syntax used inside :nth-child(), :nth-last-child(),
    /// :nth-of-type(), and :nth-last-of-type() - e.g. 'odd', 'even', '3', '2n', '2n+1', '-n+3'.
    /// </summary>
    public static class NthExpressionParser
    {
        private static readonly Regex AnBPattern = new Regex(
            @"^(?<a>[+-]?\d*n)(?<b>[+-]\d+)?$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Attempts to parse an An+B expression, returning the a and b coefficients such that
        /// the formula matches indexes where index == (a * n) + b for some integer n >= 0.
        /// </summary>
        public static bool TryParse(string raw, out int a, out int b)
        {
            a = 0;
            b = 0;

            if (string.IsNullOrWhiteSpace(raw))
                return false;

            string value = raw.Trim();

            if (string.Equals(value, "odd", StringComparison.OrdinalIgnoreCase))
            {
                a = 2;
                b = 1;
                return true;
            }

            if (string.Equals(value, "even", StringComparison.OrdinalIgnoreCase))
            {
                a = 2;
                b = 0;
                return true;
            }

            //Whitespace is permitted around the sign in the CSS spec (e.g. "2n + 1") - normalise it away.
            string compact = Regex.Replace(value, @"\s+", "");

            var match = AnBPattern.Match(compact);
            if (match.Success)
            {
                string aPart = match.Groups["a"].Value; // e.g. "2n", "-n", "n", "+n"
                string bPart = match.Groups["b"].Value; // e.g. "+1", "-3", or empty

                string coeff = aPart.Substring(0, aPart.Length - 1); // strip the trailing 'n'

                if (string.IsNullOrEmpty(coeff) || coeff == "+")
                    a = 1;
                else if (coeff == "-")
                    a = -1;
                else if (!int.TryParse(coeff, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out a))
                    return false;

                if (!string.IsNullOrEmpty(bPart))
                {
                    if (!int.TryParse(bPart, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out b))
                        return false;
                }

                return true;
            }

            //A bare integer: a = 0, b = that integer (matches only the single index == b).
            if (int.TryParse(compact, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out int justB))
            {
                a = 0;
                b = justB;
                return true;
            }

            return false;
        }
    }
}
