using Scryber.Expressive.Expressions;
using System.Collections;
using System.Collections.Generic;

namespace Scryber.Expressive.Functions.Mathematical
{
    public class CountFunction : FunctionBase
    {
        #region FunctionBase Members

        /// <inheritdoc />
        public override string Name => "Count";

        /// <inheritdoc />
        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
            this.ValidateParameterCount(parameters, -1, 1);

            var count = 0;

            foreach (var value in parameters)
            {
                var increment = 1;
                var evaluatedValue = value.Evaluate(variables);
                IEnumerable ienum;

                if (Helpers.Collections.TryIsCollection(evaluatedValue, out ienum) && ienum is ICollection col)
                {
                    increment = col.Count;
                }

                count += increment;
            }

            return count;
        }

        #endregion
    }
}
