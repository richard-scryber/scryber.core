using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

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

        /// <summary>
        /// If the value is a System.Text.Json.JsonElement or a NewtonSoft.Hson.Linq.JToken, then a native value will be extracted where possible.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static object ExtractAnyJsonValue(object value)
        {
            if (value is Newtonsoft.Json.Linq.JToken atoken)
            {
                switch (atoken.Type)
                {
                    case(JTokenType.Boolean):
                        value = atoken.Value<bool>();
                        break;
                    case(JTokenType.Bytes):
                        value = atoken.Value<long>();
                        break;
                    case(JTokenType.Date):
                        value = atoken.Value<DateTime>();
                        break;
                    case(JTokenType.Array):
                        break;
                    case(JTokenType.Float):
                        value = atoken.Value<double>();
                        break;
                    case(JTokenType.Integer):
                        value = atoken.Value<int>();
                        break;
                    case(JTokenType.Null):
                        value = null;
                        break;
                    case(JTokenType.String):
                        value = atoken.Value<string>();
                        break;
                }
            }
#if NET6_0_OR_GREATER
            else if (value is System.Text.Json.JsonElement jele)
            {
                switch (jele.ValueKind)
                {
                    case(System.Text.Json.JsonValueKind.False):
                    case(System.Text.Json.JsonValueKind.True):
                        value = jele.GetBoolean(); 
                        break;
                    case(System.Text.Json.JsonValueKind.Null):
                        value = null;
                        break;
                    case(System.Text.Json.JsonValueKind.Number):
                        value = jele.GetDouble();
                        break;
                    case(System.Text.Json.JsonValueKind.String):
                        value = jele.GetString();
                        break;
                    default:
                        break;
                }
            }
#endif
            return value;
        }

        internal static int CompareUsingMostPreciseType(object a, object b, Context context)
        {
            a = ExtractAnyJsonValue(a);
            b = ExtractAnyJsonValue(b);
            
            
            var mostPreciseType = GetMostPreciseType(a?.GetType(), b?.GetType());

            if (mostPreciseType == typeof(string))
            {
                string astr;
                string bstr;
                if (a == null && b == null)
                    return 0;
                
                if (a == null)
                    return -1;
                
                if(b == null)
                    return 1;
                   

                if (a is string str1)
                    astr = str1;
                else if (a is IConvertible conv1)
                    astr = conv1.ToString(context.CurrentCulture);
                else
                    astr = (string)Convert.ChangeType(a, TypeCode.String, context.CurrentCulture);

                if (b is string str2)
                    bstr = str2;
                else if (b is IConvertible conv2)
                    bstr = conv2.ToString(context.CurrentCulture);
                else
                    bstr = (string)Convert.ChangeType(a, TypeCode.String, context.CurrentCulture);
                
                return string.Compare(astr, bstr, context.EqualityStringComparison);
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

            return -1;
        }
    }
}
