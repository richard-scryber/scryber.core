using System;
using System.Collections.Generic;

namespace Scryber.Expressive.Helpers
{
    internal static class Comparison
    {
        private static readonly Type[] CommonTypes = {
            typeof(DateTime), // If it can be interpreted as a DateTime use that.
            typeof(decimal),  // Decimal is stored as 96 bits of value, plus a sign, plus an exponent
            typeof(double),   // Double is stored as a 64 bit floating point
            typeof(long),     // 64 bit signed integer
            typeof(int),      // 32 bit signed integer
            typeof(bool),     // Process booleans before strings
            typeof(string),   // If it's not anything else, it can be a string.
        };

        internal static int CompareUsingMostPreciseType(object a, object b, Context context)
        {
            var mostPreciseType = GetMostPreciseType(a?.GetType(), b?.GetType());

            if (mostPreciseType == typeof(string))
            {
                return string.Compare(
                    (string)Convert.ChangeType(a, mostPreciseType, context.CurrentCulture),
                    (string)Convert.ChangeType(b, mostPreciseType, context.CurrentCulture),
                    context.EqualityStringComparison);
            }

            return Comparison.Compare(a, b, mostPreciseType, context);
        }

        private static Type GetMostPreciseType(Type a, Type b)
        {
            if (a == b)
            {
                return a; // If they're the same type, just return one of them.
            }
            foreach (var t in Comparison.CommonTypes)
            {
                if (a == t || b == t)
                {
                    return t;
                }
            }

            return a;
        }

        private static int Compare(object lhs, object rhs, Type mostPreciseType, Context context)
        {
            // If at least one is null then the check is simple.
            if (lhs is null && rhs is null)
            {
                return 0;
            }

            if (lhs is null)
            {
                return -1;
            }

            if (rhs is null)
            {
                return 1;
            }

            var lhsType = lhs.GetType();
            var rhsType = rhs.GetType();

            if (lhsType == rhsType)
            {
                return Comparer<object>.Default.Compare(lhs, rhs);
            }

            // Attempt to convert using the mostPreciseType first.
            try
            {
                if (lhsType == mostPreciseType)
                {
                    rhs = Convert.ChangeType(rhs, mostPreciseType, context.CurrentCulture);
                }
                else
                {
                    lhs = Convert.ChangeType(lhs, mostPreciseType, context.CurrentCulture);
                }

                return Comparer<object>.Default.Compare(lhs, rhs);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception)
#pragma warning restore CA1031 // Do not catch general exception types
            {

            }

            // Attempt to convert the RHS to match the LHS.
            try
            {
                return Comparer<object>.Default.Compare(lhs, Convert.ChangeType(rhs, lhsType, context.CurrentCulture));
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception)
#pragma warning restore CA1031 // Do not catch general exception types
            {

            }

            // Attempt to convert the LHS to match the RHS.
            try
            {
                return Comparer<object>.Default.Compare(lhs, Convert.ChangeType(rhs, lhsType, context.CurrentCulture));
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception)
#pragma warning restore CA1031 // Do not catch general exception types
            {

            }

            // Failing that resort to a string.
            try
            {
                return string.Compare(
                    (string)Convert.ChangeType(lhs, typeof(string), context.CurrentCulture),
                    (string)Convert.ChangeType(rhs, typeof(string), context.CurrentCulture),
                    context.EqualityStringComparison);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception)
#pragma warning restore CA1031 // Do not catch general exception types
            {

            }

            return 0;
        }
    }
}
