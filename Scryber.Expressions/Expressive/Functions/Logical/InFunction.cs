using Scryber.Expressive.Expressions;
using Scryber.Expressive.Helpers;

namespace Scryber.Expressive.Functions.Logical
{
    internal class InFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name { get { return "In"; } }

        public override object Evaluate(IExpression[] parameters, Context context)
        {
            this.ValidateParameterCount(parameters, -1, 2);

            var found = false;

            var parameter = parameters[0].Evaluate(Variables);

            // Goes through any values, and stop whe one is found
            for (var i = 1; i < parameters.Length; i++)
            {
                if (Comparison.CompareUsingMostPreciseType(parameter, parameters[i].Evaluate(Variables), context) == 0)
                {
                    found = true;
                    break;
                }
            }

            return found;
        }

        #endregion
    }
}
