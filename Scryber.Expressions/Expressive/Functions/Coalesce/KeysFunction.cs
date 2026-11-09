using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Scryber.Expressive.Expressions;

namespace Scryber.Expressive.Functions.Coalesce
{
    /// <summary>
    /// Returns the keys of a dictionary, or the names of the properties of an object (including json objects),
    /// as an array of strings that can be counted, sorted and enumerated.
    /// </summary>
    public class KeysFunction : FunctionBase
    {
        public override string Name
        {
            get { return "Keys"; }
        }

        public KeysFunction()
        {
        }

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, ExpressionContext context)
        {
            this.ValidateParameterCount(parameters, 1, 1);

            var value = parameters[0].Evaluate(variables);
            return GetKeys(value);
        }

        /// <summary>
        /// Gets the keys of a dictionary, or property names of an object. A null value, a string, a number, or a collection has no keys.
        /// </summary>
        public static string[] GetKeys(object value)
        {
            var keys = new List<string>();

            if (null == value || value is string || value.GetType().IsPrimitive)
            {
                //nothing
            }
            else if (value is IDictionary dict)
            {
                foreach (var key in dict.Keys)
                    keys.Add(ToKeyString(key));
            }
            else if (value is Newtonsoft.Json.Linq.JObject jobj)
            {
                foreach (var prop in jobj.Properties())
                    keys.Add(prop.Name);
            }
            else if (value is System.Text.Json.JsonElement jele)
            {
                if (jele.ValueKind == System.Text.Json.JsonValueKind.Object)
                {
                    foreach (var prop in jele.EnumerateObject())
                        keys.Add(prop.Name);
                }
            }
            else if (value is IEnumerable<KeyValuePair<string, object>> pairs)
            {
                //e.g. an ExpandoObject
                foreach (var pair in pairs)
                    keys.Add(pair.Key);
            }
            else if (value is IEnumerable)
            {
                if (!AddReadOnlyDictionaryKeys(value, keys))
                {
                    //any other collection has no keys
                }
            }
            else
            {
                foreach (var prop in value.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (prop.CanRead && prop.GetIndexParameters().Length == 0)
                        keys.Add(prop.Name);
                }
            }

            return keys.ToArray();
        }

        private static bool AddReadOnlyDictionaryKeys(object value, List<string> keys)
        {
            foreach (var face in value.GetType().GetInterfaces())
            {
                if (face.IsGenericType && face.GetGenericTypeDefinition() == typeof(IReadOnlyDictionary<,>))
                {
                    var prop = face.GetProperty("Keys");
                    if (prop?.GetValue(value) is IEnumerable all)
                    {
                        foreach (var key in all)
                            keys.Add(ToKeyString(key));
                        return true;
                    }
                }
            }
            return false;
        }

        private static string ToKeyString(object key)
        {
            return Convert.ToString(key, CultureInfo.InvariantCulture) ?? string.Empty;
        }
    }
}
