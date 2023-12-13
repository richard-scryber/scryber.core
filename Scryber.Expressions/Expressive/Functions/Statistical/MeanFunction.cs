using Scryber.Expressive.Expressions;
using Scryber.Expressive.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Scryber.Expressive.Functions.Statistical
{
    public class MeanFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name { get { return "Mean"; } }

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
            this.ValidateParameterCount(parameters, -1, 1);

            int count = 0;
            object result = 0;

            foreach (var value in parameters)
            {
                int increment = 1;
                object evaluatedValue = value.Evaluate(variables);
                IEnumerable enumerable = evaluatedValue as IEnumerable;

                if (enumerable != null)
                {
                    int enumerableCount = 0;
                    object enumerableSum = 0;

                    foreach (var item in enumerable)
                    {
                        enumerableCount++;
                        enumerableSum = Numbers.Add(enumerableSum, item, variables);
                    }

                    increment = enumerableCount;
                    evaluatedValue = enumerableSum;
                }

                result = Numbers.Add(result, evaluatedValue, variables);
                count += increment;
            }

            return Convert.ToDouble(result) / count;
        }

        #endregion
    }
}
