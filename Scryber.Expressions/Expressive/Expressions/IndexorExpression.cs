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
                else if(rhs is IList list)
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
                else if(rhs is IDictionary dict)
                {
                    return DoGetDictionaryValue(dict, rhs.ToString());
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
