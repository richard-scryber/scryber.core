using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace Scryber.Expressive.Helpers
{
    /// <summary>
    /// The one place the expression engine reads JSON values - from System.Text.Json (JsonElement) and from Newtonsoft (JToken),
    /// so that both libraries bind in the same way.
    /// Objects and arrays stay as JSON, and every other value is read as its native .NET value: a number is a double,
    /// true and false are a bool, a string is a string, and null is null.
    /// </summary>
    public static class JsonValues
    {
        /// <summary>
        /// Returns true if the value is a System.Text.Json or Newtonsoft JSON value
        /// </summary>
        public static bool IsJson(object value)
        {
            return value is JsonElement || value is JToken;
        }

        /// <summary>
        /// Returns true if the value is a JSON object (with properties)
        /// </summary>
        public static bool IsJsonObject(object value)
        {
            if (value is JsonElement element)
                return element.ValueKind == JsonValueKind.Object;
            else if (value is JToken token)
                return token.Type == JTokenType.Object;
            else
                return false;
        }

        /// <summary>
        /// Returns true if the value is a JSON array
        /// </summary>
        public static bool IsJsonArray(object value)
        {
            if (value is JsonElement element)
                return element.ValueKind == JsonValueKind.Array;
            else if (value is JToken token)
                return token.Type == JTokenType.Array;
            else
                return false;
        }

        /// <summary>
        /// If the value is a JSON value, returns its native value (JSON objects and arrays are returned as they are).
        /// Any other value is returned as it is.
        /// </summary>
        public static object ToNative(object value)
        {
            if (value is JsonElement element)
                return ToNative(element);
            else if (value is JToken token)
                return ToNative(token);
            else
                return value;
        }

        /// <summary>
        /// Returns the native value of the System.Text.Json element (objects and arrays are returned as they are)
        /// </summary>
        public static object ToNative(JsonElement element)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.Array:
                case JsonValueKind.Object:
                    return element;
                case JsonValueKind.Null:
                case JsonValueKind.Undefined:
                    return null;
                case JsonValueKind.False:
                case JsonValueKind.True:
                    return element.GetBoolean();
                case JsonValueKind.Number:
                    return element.GetDouble();
                default:
                    return element.GetString();
            }
        }

        /// <summary>
        /// Returns the native value of the Newtonsoft token (objects and arrays are returned as they are)
        /// </summary>
        public static object ToNative(JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.Array:
                case JTokenType.Object:
                    return token;
                case JTokenType.Null:
                case JTokenType.None:
                case JTokenType.Undefined:
                    return null;
                case JTokenType.Integer:
                case JTokenType.Float:
                    return (double)token;
                case JTokenType.Boolean:
                    return (bool)token;
                case JTokenType.String:
                case JTokenType.Uri:
                    return (string)token;
                case JTokenType.Guid:
                    return (Guid)token;
                case JTokenType.Date:
                    return (DateTime)token;
                case JTokenType.Bytes:
                    return (byte[])token;
                case JTokenType.TimeSpan:
                    return (TimeSpan)token;
                default:
                    return token.ToString(Newtonsoft.Json.Formatting.None);
            }
        }

        /// <summary>
        /// If the parent is a JSON object (or any System.Text.Json element), sets value to the native value of its named property
        /// (or null if it does not have one), and returns true. Otherwise returns false.
        /// </summary>
        public static bool TryGetProperty(object parent, string name, out object value)
        {
            value = null;

            if (parent is JsonElement element)
            {
                //a JSON element that is not an object has no properties
                if (element.ValueKind == JsonValueKind.Object && element.TryGetProperty(name, out var found))
                    value = ToNative(found);

                return true;
            }
            else if (parent is JObject jobject)
            {
                if (jobject.TryGetValue(name, StringComparison.Ordinal, out var found))
                    value = ToNative(found);

                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// If the parent is a JSON array or object, sets value to the native value of the item at the index (for an array),
        /// or of the property with the key (for an object), and returns true. A missing property is null.
        /// Otherwise returns false.
        /// </summary>
        /// <exception cref="NotSupportedException">The parent is a JSON array, and the key is not a number</exception>
        public static bool TryGetIndexed(object parent, string key, out object value)
        {
            value = null;

            if (IsJsonArray(parent))
            {
                if (!int.TryParse(key, out var index))
                    throw new NotSupportedException("The object indexing the array is not based on a number index");

                if (parent is JsonElement element)
                    value = ToNative(element[index]);
                else
                    value = ToNative(((JArray)parent)[index]);

                return true;
            }
            else
                return TryGetProperty(parent, key, out value);
        }

        /// <summary>
        /// If the value is a JSON array, sets items to its entries (as JSON values) and returns true. Otherwise returns false.
        /// </summary>
        public static bool TryGetArray(object value, out IEnumerable items)
        {
            if (value is JsonElement element && element.ValueKind == JsonValueKind.Array)
            {
                items = element.EnumerateArray();
                return true;
            }
            else if (value is JArray array)
            {
                items = array;
                return true;
            }
            else
            {
                items = null;
                return false;
            }
        }

        /// <summary>
        /// If the value is a JSON object, sets keys to its property names and returns true. Otherwise returns false.
        /// </summary>
        public static bool TryGetKeys(object value, out IList<string> keys)
        {
            if (value is JsonElement element && element.ValueKind == JsonValueKind.Object)
            {
                keys = new List<string>();
                foreach (var prop in element.EnumerateObject())
                    keys.Add(prop.Name);
                return true;
            }
            else if (value is JObject jobject)
            {
                keys = new List<string>();
                foreach (var prop in jobject.Properties())
                    keys.Add(prop.Name);
                return true;
            }
            else
            {
                keys = null;
                return false;
            }
        }
    }
}
