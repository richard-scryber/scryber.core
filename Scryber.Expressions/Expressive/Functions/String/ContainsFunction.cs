using System.Collections.Generic;
using Scryber.Expressive.Expressions;

namespace Scryber.Expressive.Functions.String
{
    public class ContainsFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name
        {
            get
            {
                return "Contains";
            }
        }

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
            this.ValidateParameterCount(parameters, 2, 2);

            // TODO: evaluate (no pun intended) whether a cast is correct here.
            var text = (string)parameters[0].Evaluate(variables);
            var value = (string)parameters[1].Evaluate(variables);

            if (value is null)
            {
                return false;
            }

            return text?.IndexOf(value, context.EqualityStringComparison) >= 0;
        }

        #endregion
    }
}
