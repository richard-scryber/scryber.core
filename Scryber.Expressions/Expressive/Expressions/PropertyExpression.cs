using Scryber.Expressive.Expressions.Binary;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Scryber.Expressive.Exceptions;

using System.Text.Json;

namespace Scryber.Expressive.Expressions
{
    public class PropertyExpression : BinaryExpressionBase
    {
        public PropertyExpression(IExpression left, IExpression right, ExpressionContext context)
            : base(EnsureLeftExpression(left, context), right, context)
        {
        }

        private static IExpression EnsureLeftExpression(IExpression left, ExpressionContext context)
        {
            if(null == left)
            {
                left = new CurrentDataExpression(context);
            }
            return left;
        }

        public override object Evaluate(IDictionary<string, object> variables)
        {
            if (null == this.rightHandSide && this.leftHandSide is CurrentDataExpression)
            {
                //special case where we are just a '.'
                var result = this.leftHandSide.Evaluate(variables);
                return result;
            }
            else
                return base.Evaluate(variables);
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
                else if (null == lhsResult)
                {
                    //A null root variable (e.g. 'model' in model.missing.value) is reported.
                    //A null part way along the path (e.g. 'missing') is not an error - the result is just null.
                    if (IsRootExpression(this.leftHandSide))
                        throw new NullRootVariableException(rhs);

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


        private static bool IsRootExpression(IExpression expression)
        {
            return expression is VariableExpression
                   || expression is CurrentDataExpression
                   || expression is ParentDataExpression
                   || expression is SelfVariableExpression
                   || expression is ParentVariableExpression;
        }

        private object DoGetMyValue(object parent, string name, ExpressionContext context)
        {
            if (parent is IDictionary dict)
            {
                if (dict.Contains(name))
                    return NormalizeDictionaryValue(dict[name]);
                //fall back to the default implementation if not found
            }

            return GetPropertyValue(parent, name, context.IsCaseInsensitiveParsingEnabled);
        }

        // A plain IDictionary (e.g. Dictionary<string,object> built by a host
        // application from deserialized JSON) is returned above without going
        // through GetPropertyValue's JObject/JsonElement handling below - so a
        // JToken/JsonElement stored as one of its values would otherwise reach
        // callers (and functions like double()/string()) unwrapped, unlike the
        // exact same JSON value reached via nested property access. Apply the
        // same normalization here so a dictionary's own values behave
        // identically to a JObject/JsonElement's properties.
        private static object NormalizeDictionaryValue(object value)
        {
            return Helpers.JsonValues.ToNative(value);
        }
        
        public static object GetPropertyValue(object parent, string name, bool caseInSensitive)
        {

            if(null == parent)
            {
                throw new ArgumentNullException(nameof(parent));
            }

            //Do the JSON object checks first
            if (Helpers.JsonValues.TryGetProperty(parent, name, out var jsonValue))
            {
                return jsonValue;
            }
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

                if (TryGetProperty(type, name, caseInSensitive, out pi))
                {
                    return pi.GetValue(parent, null);
                }

                else if (TryGetField(type, name, caseInSensitive, out fi))
                {
                    return fi.GetValue(parent);
                }

                else if (parent is ICustomTypeDescriptor)
                {
                    var properties = (parent as ICustomTypeDescriptor).GetProperties();
                    var prop = properties.Find(name, caseInSensitive);

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

        /// <summary>
        /// Returns the native value of a System.Text.Json element (objects and arrays are returned as they are)
        /// </summary>
        public static object GetJsonElementValue(JsonElement result)
        {
            return Helpers.JsonValues.ToNative(result);
        }

        /// <summary>
        /// Returns the native value of a Newtonsoft token (objects and arrays are returned as they are)
        /// </summary>
        public static object GetJTokenValue(Newtonsoft.Json.Linq.JToken result)
        {
            return Helpers.JsonValues.ToNative(result);
        }

        private static bool TryGetProperty(Type fortype, string name, bool ignoreCase, out PropertyInfo found)
        {
            BindingFlags flags = ignoreCase ? (BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) : BindingFlags.Default;
            found = fortype.GetProperty(name, flags);

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
            BindingFlags flags = ignoreCase ? (BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) : BindingFlags.Default;
            found = fortype.GetField(name, flags);
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
