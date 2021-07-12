using Scryber.Expressive.Expressions;
using Scryber.Expressive.Helpers;
using System;
using System.Collections;

namespace Scryber.Expressive.Functions.Statistical
{
    internal class MeanFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name { get { return "Mean"; } }

        public override object Evaluate(IExpression[] parameters, Context context)
        {
            this.ValidateParameterCount(parameters, -1, 1);

            int count = 0;
            object result = 0;

            foreach (var value in parameters)
            {
                int increment = 1;
                object evaluatedValue = value.Evaluate(Variables);
                IEnumerable enumerable = evaluatedValue as IEnumerable;

                if (enumerable != null)
                {
                    int enumerableCount = 0;
                    object enumerableSum = 0;

                    foreach (var item in enumerable)
                    {
                        enumerableCount++;
                        enumerableSum = Numbers.Add(enumerableSum, item);
                    }

                    increment = enumerableCount;
                    evaluatedValue = enumerableSum;
                }

                result = Numbers.Add(result, evaluatedValue);
                count += increment;
            }

            return Convert.ToDouble(result) / count;
        }

        #endregion
    }
}
