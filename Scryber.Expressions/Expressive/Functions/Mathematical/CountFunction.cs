using Scryber.Expressive.Expressions;
using System.Collections;

namespace Scryber.Expressive.Functions.Mathematical
{
    internal class CountFunction : FunctionBase
    {
        #region FunctionBase Members

        /// <inheritdoc />
        public override string Name => "Count";

        /// <inheritdoc />
        public override object Evaluate(IExpression[] parameters, Context context)
        {
            this.ValidateParameterCount(parameters, -1, 1);

            var count = 0;

            foreach (var value in parameters)
            {
                var increment = 1;
                var evaluatedValue = value.Evaluate(Variables);

                if (evaluatedValue is ICollection collection)
                {
                    increment = collection.Count;
                }

                count += increment;
            }

            return count;
        }

        #endregion
    }
}
