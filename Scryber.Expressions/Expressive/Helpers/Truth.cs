using System;
using System.Globalization;

namespace Scryber.Expressive.Helpers
{
    /// <summary>
    /// The truth of a value in a condition - for the ? : operator, &amp;&amp;, || and !, and the if() function
    /// </summary>
    public static class Truth
    {
        /// <summary>
        /// Null, false, zero and an empty string are false. A string is true unless it is empty or reads as 'false'.
        /// Any other value is converted as a boolean if it can be, otherwise it is true (e.g. a date, an object or a collection).
        /// </summary>
        public static bool IsTrue(object value)
        {
            switch (value)
            {
                case null:
                    return false;
                case bool b:
                    return b;
                case string s:
                    if (s.Length == 0)
                        return false;
                    if (bool.TryParse(s, out var parsed))
                        return parsed;
                    return true;
                case IConvertible convertible:
                    try
                    {
                        return convertible.ToBoolean(CultureInfo.InvariantCulture);
                    }
                    catch (InvalidCastException)
                    {
                        return true;
                    }
                    catch (FormatException)
                    {
                        return true;
                    }
                default:
                    return true;
            }
        }
    }
}
