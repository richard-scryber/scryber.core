using System;
using System.Collections.Generic;
using System.Globalization;

namespace Scryber.Expressive
{
    internal static class ExtensionMethods
    {
        internal static bool IsArithmeticOperator(this string source)
        {
            return string.Equals(source, "+", StringComparison.Ordinal) ||
                string.Equals(source, "-", StringComparison.Ordinal) || string.Equals(source, "\u2212", StringComparison.Ordinal) ||
                string.Equals(source, "/", StringComparison.Ordinal) || string.Equals(source, "\u00f7", StringComparison.Ordinal) ||
                string.Equals(source, "*", StringComparison.Ordinal) || string.Equals(source, "\u00d7", StringComparison.Ordinal) ||
                string.Equals(source, "+", StringComparison.Ordinal) ||
                string.Equals(source, "+", StringComparison.Ordinal) ||
                string.Equals(source, "+", StringComparison.Ordinal);
        }

        internal static bool IsNumeric(this string source, CultureInfo cultureInfo)
        {
            if (source.StartsWith("0x"))
            {
                try
                {
                    int val = Convert.ToInt32(source, 16);
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            }
            else if (source.StartsWith("0b"))
            {
                try
                {
                    int val = Convert.ToInt32(source.Substring(2), 2);
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            }
            else
                return double.TryParse(source, NumberStyles.Any, cultureInfo, out _);
        }

        internal static T PeekOrDefault<T>(this Queue<T> queue)
        {
            return (queue.Count > 0) ? queue.Peek() : default(T);
        }

        internal static string SubstringUpTo(this string source, int startIndex, char character)
        {
            if (startIndex != 0)
            {
                string inner = source.Substring(startIndex);

                return inner.Substring(0, inner.IndexOf(character) + 1);
            }

            return source.Substring(startIndex, source.IndexOf(character) + 1);
        }
    }
}
