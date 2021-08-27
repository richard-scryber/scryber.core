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

            var result = parameters[0].Evaluate(variables);

            if (result is IEnumerable enumerableResult)
            {
                result = Min(enumerableResult, context);
            }

            // Null means we should bail out.
            if (result is null)
            {
                return null;
            }

            // Skip the first item in the list as it has already been evaluated.
            foreach (var value in parameters.Skip(1))
            {
                var evaluatedValue = value.Evaluate(variables);

                if (evaluatedValue is IEnumerable enumerable)
                {
                    evaluatedValue = Min(enumerable, context);
                }

                result = Comparison.CompareUsingMostPreciseType(result, evaluatedValue, context) < 0
                    ? result
                    : evaluatedValue;

                // Null means we should bail out.
                if (result is null)
                {
                    return null;
                }
            }

            return result;
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
