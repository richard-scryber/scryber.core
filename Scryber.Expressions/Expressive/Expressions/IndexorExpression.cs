using Scryber.Expressive.Expressions.Binary;
using System;
using System.Collections.Generic;
using System.Collections;

namespace Scryber.Expressive.Expressions
{
    public class IndexorExpression : BinaryExpressionBase
    {
        
        internal IndexorExpression(IExpression leftHandSide, IExpression indexExpression, Context context)
            : base(leftHandSide, indexExpression, context)
        {
            
        }

        protected override object EvaluateImpl(object lhsResult, IExpression rightHandSide, IDictionary<string, object> variables)
        {
            if (null != rightHandSide)
            {
                object rhs = rightHandSide.Evaluate(variables).ToString();

                if (null == rhs)
                {
                    return null;
                }
                else if (lhsResult is Array array)
                {
                    if (rhs is int index || int.TryParse(rhs.ToString(), out index))
                    {
                        return DoGetArrayValue(array, index);
                    }
                    else
                    {
                        throw new IndexOutOfRangeException("Could not extract the index of the array from the expression");
                    }

                }
                else if (lhsResult is IList list)
                {
                    if (rhs is int index || int.TryParse(rhs.ToString(), out index))
                    {
                        return DoGetListValue(list, index);
                    }
                    else
                    {
                        throw new IndexOutOfRangeException("Could not extract the index of the list from the expression");
                    }
                }
                else if (lhsResult is IDictionary dict)
                {
                    return DoGetDictionaryValue(dict, rhs.ToString());
                }
#if NET6_0_OR_GREATER

                else if (lhsResult is System.Text.Json.JsonElement jele &&
                    jele.ValueKind == System.Text.Json.JsonValueKind.Array)
                {
                    System.Text.Json.JsonElement value;

                    if (rhs is int index || int.TryParse(rhs.ToString(), out index))
                    {
                        value = jele[index];
                    }
                    else
                        throw new NotSupportedException("The object indexing the array is not based on a number index");

                    switch (value.ValueKind)
                    {
                        case System.Text.Json.JsonValueKind.Array:
                        case System.Text.Json.JsonValueKind.Object:
                            return value;
                        case System.Text.Json.JsonValueKind.Null:
                            return null;
                        case System.Text.Json.JsonValueKind.False:
                        case System.Text.Json.JsonValueKind.True:
                            return value.GetBoolean();
                        case System.Text.Json.JsonValueKind.Number:
                            return value.GetDouble();
                        default:
                            return value.GetString();
                    }
                }
#endif
                else if (lhsResult is Newtonsoft.Json.Linq.JToken jobject &&
                    jobject.Type == Newtonsoft.Json.Linq.JTokenType.Array)
                {
                    Newtonsoft.Json.Linq.JToken result;

                    if (rhs is int index || int.TryParse(rhs.ToString(), out index))
                    {
                        result = ((Newtonsoft.Json.Linq.JArray)jobject)[index];
                    }
                    else
                        throw new NotSupportedException("The object indexing the array is not based on a number index");

                    switch (result.Type)
                    {
                        case Newtonsoft.Json.Linq.JTokenType.Array:
                        case Newtonsoft.Json.Linq.JTokenType.Object:
                            return result;
                        case Newtonsoft.Json.Linq.JTokenType.Null:
                        case Newtonsoft.Json.Linq.JTokenType.None:
                            return null;
                        case Newtonsoft.Json.Linq.JTokenType.Integer:
                        case Newtonsoft.Json.Linq.JTokenType.Float:
                            return ((double)result);
                        case Newtonsoft.Json.Linq.JTokenType.Boolean:
                            return ((bool)result);
                        case Newtonsoft.Json.Linq.JTokenType.String:
                        case Newtonsoft.Json.Linq.JTokenType.Uri:
                            return ((string)result);
                        case Newtonsoft.Json.Linq.JTokenType.Guid:
                            return ((Guid)result);
                        case Newtonsoft.Json.Linq.JTokenType.Date:
                            return ((DateTime)result);
                        case Newtonsoft.Json.Linq.JTokenType.Bytes:
                            return ((byte[])result);
                        case Newtonsoft.Json.Linq.JTokenType.TimeSpan:
                            return ((TimeSpan)result);
                        default:
                            return result.ToString(Newtonsoft.Json.Formatting.None);
                    }
                }
                else
                {
                    throw new NotSupportedException("The object to apply index or key value extraction is not of the supported type of Array, List or Dictionary");
                }
            }
            else
            {
                return null;
            }
        }

        private object DoGetArrayValue(Array array, int index)
        {
            return array.GetValue(index);
        }

        private object DoGetListValue(IList list, int index)
        {
            return list[index];
        }

        private object DoGetDictionaryValue(IDictionary dict, string key)
        {
            if (dict.Contains(key))
            {
                return dict[key];
            }
            else
            {
                return null;
            }
        }
    }
}
