using Scryber.Expressive.Expressions.Binary;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

#if NET6_0

using System.Text.Json;

#endif

namespace Scryber.Expressive.Expressions
{
    public class PropertyExpression : BinaryExpressionBase
    {
        public PropertyExpression(IExpression left, IExpression right, Context context)
            : base(EnsureLeftExpression(left, context), right, context)
        {
        }

        private static IExpression EnsureLeftExpression(IExpression left, Context context)
        {
            if(null == left)
            {
                left = new CurrentDataExpression(context);
            }
            return left;
        }

        

        protected override object EvaluateImpl(object lhsResult, IExpression rightHandSide, IDictionary<string, object> variables)
        {
            if (null != rightHandSide)
            {
                string rhs;
                if (rightHandSide is VariableExpression varExpr)
                {
                    rhs = varExpr.variableName;
                }
                else
                {
                    rhs = rightHandSide.Evaluate(variables).ToString();
                }

                if (string.IsNullOrEmpty(rhs))
                {
                    return null;
                }
                else
                {
                    return DoGetMyValue(lhsResult, rhs, this.Context);
                }
            }
            else
            {
                return null;
            }
        }


        private object DoGetMyValue(object parent, string name, Context context)
        {
            

            if(null == parent)
            {
                throw new ArgumentNullException(nameof(parent));
            }

            //Do the JSON object checks first

            if (parent is Newtonsoft.Json.Linq.JObject jobject)
            {
                if (jobject.ContainsKey(name) == false)
                    return null;

                var result = jobject.GetValue(name);
                return GetJTokenValue(result);
            }

#if NET6_0

            //New to .net 6
            else if (parent is JsonElement jelement)
            {
                JsonElement result;
                if (!jelement.TryGetProperty(name, out result))
                    return null;
                else
                    return GetJsonElementValue(result);
                
            }

#endif
            //check for dynamic type
            else if (parent is System.Dynamic.ExpandoObject)
            {
                object found;
                IDictionary<string, object> expando = parent as System.Dynamic.ExpandoObject;
                if (expando.TryGetValue(name, out found))
                {
                    return found;
                }
                else
                {
                    return null; //As we are dynamic, let's be generous and not throw an error.
                }
            }
            else
            {
                //standard .net object reflection
                var type = parent.GetType();

                PropertyInfo pi = null;
                FieldInfo fi = null;

                if (TryGetProperty(type, name, context.IsCaseInsensitiveParsingEnabled, out pi))
                {
                    return pi.GetValue(parent, null);
                }

                else if (TryGetField(type, name, context.IsCaseInsensitiveParsingEnabled, out fi))
                {
                    return fi.GetValue(parent);
                }

                else if (parent is ICustomTypeDescriptor)
                {
                    var properties = (parent as ICustomTypeDescriptor).GetProperties();
                    var prop = properties.Find(name, Context.IsCaseInsensitiveParsingEnabled);

                    if (null != prop)
                    {
                        return prop.GetValue(parent);
                    }
                    else
                    {
                        return null;
                    }
                }

                else
                {
                    return null;
                    // being kind and just not returning a value
                    //throw new InvalidOperationException("Could not extract the property '" + name + "' from the object, as it is not a supported type for property access");
                }

            }
        }

#if NET6_0

        public static object GetJsonElementValue(JsonElement result)
        {
            switch (result.ValueKind)
            {
                case JsonValueKind.Array:
                case JsonValueKind.Object:
                    return result;
                case JsonValueKind.Null:
                    return null;
                case JsonValueKind.False:
                case JsonValueKind.True:
                    return result.GetBoolean();
                case JsonValueKind.Number:
                    return result.GetDouble();
                default:
                    return result.GetString();
            }
        }

#endif

        public static object GetJTokenValue(Newtonsoft.Json.Linq.JToken result)
        {
            object value;
            switch (result.Type)
            {
                case Newtonsoft.Json.Linq.JTokenType.Array:
                case Newtonsoft.Json.Linq.JTokenType.Object:
                    value = result;
                    break;
                case Newtonsoft.Json.Linq.JTokenType.Null:
                case Newtonsoft.Json.Linq.JTokenType.None:
                    value = null;
                    break;
                case Newtonsoft.Json.Linq.JTokenType.Integer:
                case Newtonsoft.Json.Linq.JTokenType.Float:
                    value = ((double)result);
                    break;
                case Newtonsoft.Json.Linq.JTokenType.Boolean:
                    value = ((bool)result);
                    break;
                case Newtonsoft.Json.Linq.JTokenType.String:
                case Newtonsoft.Json.Linq.JTokenType.Uri:
                    value = ((string)result);
                    break;
                case Newtonsoft.Json.Linq.JTokenType.Guid:
                    value = ((Guid)result);
                    break;
                case Newtonsoft.Json.Linq.JTokenType.Date:
                    value = ((DateTime)result);
                    break;
                case Newtonsoft.Json.Linq.JTokenType.Bytes:
                    value = ((byte[])result);
                    break;
                case Newtonsoft.Json.Linq.JTokenType.TimeSpan:
                    value = ((TimeSpan)result);
                    break;
                default:
                    value = result.ToString(Newtonsoft.Json.Formatting.None);
                    break;
            }
            return value;
        }

        private static bool TryGetProperty(Type fortype, string name, bool ignoreCase, out PropertyInfo found)
        {
            found = fortype.GetProperty(name);

            if (null == found)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private static bool TryGetField(Type fortype, string name, bool ignoreCase, out FieldInfo found)
        {
            found = fortype.GetField(name);
            if (null == found)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
    }
}
