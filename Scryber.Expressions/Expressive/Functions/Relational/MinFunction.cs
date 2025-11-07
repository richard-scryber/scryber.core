using System;
using Scryber.Expressive.Expressions;
using Scryber.Expressive.Helpers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Scryber.Expressive.Functions.Relational
{
    public class MinFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name => "Min";

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
            this.ValidateParameterCount(parameters, -1, 1);

            object min = null;
            object result;
            
            for (var i = 0; i < parameters.Length; i++)
            {
                var param = parameters[i];

                result = param.Evaluate(variables);
                result = Comparison.ExtractAnyJsonValue(result);

                if (result != null)
                {
                    if (Collections.TryIsCollection(result, out var ienm))
                    {
                        foreach (var innerResult in ienm)
                        {
                            var innerValue = Comparison.ExtractAnyJsonValue(innerResult);
                            if (null == min)
                            {
                                min = innerValue;
                            }
                            else if (null == innerValue)
                            {
                                //ignore nulls
                            }
                            else if (innerValue is string innerStr)
                            {
                                if (System.String.Compare(min.ToString(), innerStr, context.EqualityStringComparison) > 0)
                                {
                                    min = innerValue;
                                }
                            }
                            else if(innerValue is IComparable comp)
                            {
                                if (Comparison.CompareUsingMostPreciseType(min, comp, context) > 0)
                                    min = comp;
                            }
                        }
                    }
                    else //not a collection
                    {
                        if (null == min)
                        {
                            min = result;
                        }
                        else if (result is string str)
                        {
                            if (System.String.Compare(min.ToString(), str, context.EqualityStringComparison) > 0)
                            {
                                min = str;
                            }
                        }
                        else if (result is IComparable comp)
                        {
                            if (Comparison.CompareUsingMostPreciseType(min, comp, context) > 0)
                                min = comp;
                        }

                    }
                }
            }

            return min;
        }

        #endregion

        private static object Min(IEnumerable enumerable, Context context)
        {
            object enumerableResult = null;

            foreach (var item in enumerable)
            {
                // Null means we should bail out.
                if (item is null)
                {
                    return null;
                }

                if (enumerableResult is null)
                {
                    enumerableResult = item;
                    continue;
                }

                enumerableResult = Comparison.CompareUsingMostPreciseType(enumerableResult, item, context) < 0
                    ? enumerableResult
                    : item;
            }

            return enumerableResult;
        }
    }
}
