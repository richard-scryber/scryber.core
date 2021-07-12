using Scryber.Expressive.Expressions;
using Scryber.Expressive.Helpers;
using System.Collections;

namespace Scryber.Expressive.Functions.Mathematical
{
    internal class SumFunction : FunctionBase
    {
        #region IFunction Members

        public override string Name { get { return "Sum"; } }

        public override object Evaluate(IExpression[] parameters, Context context)
        {
            this.ValidateParameterCount(parameters, -1, 1);

            object result = 0;

            foreach (var value in parameters)
            {
                object evaluatedValue = value.Evaluate(Variables);
                IEnumerable enumerable = evaluatedValue as IEnumerable;

                if (enumerable != null)
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
