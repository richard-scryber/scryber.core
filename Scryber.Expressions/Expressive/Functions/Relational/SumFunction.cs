using Scryber.Expressive.Expressions;
using Scryber.Expressive.Helpers;
using System.Collections;
using System.Collections.Generic;

namespace Scryber.Expressive.Functions.Relational
{
    public class SumFunction : FunctionBase
    {
        #region IFunction Members

        public override string Name { get { return "Sum"; } }

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
            this.ValidateParameterCount(parameters, -1, 1);

            object result = 0;

            foreach (var value in parameters)
            {
                object evaluatedValue = value.Evaluate(variables);
                IEnumerable enumerable;

                if (Helpers.Collections.TryIsCollection(evaluatedValue, out enumerable))
                {
                    object enumerableSum = 0;
                    foreach (var item in enumerable)
                    {
                        // When summing we don't want to bail out early with a null value.
                        enumerableSum = Numbers.Add(enumerableSum ?? 0, item ?? 0);
                    }
                    evaluatedValue = enumerableSum;
                }
                
                // When summing we don't want to bail out early with a null value.
                result = Numbers.Add(result ?? 0, evaluatedValue ?? 0);
            }

            return result;
        }

        #endregion
    }
}
