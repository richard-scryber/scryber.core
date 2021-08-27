using System.Collections.Generic;
using Scryber.Expressive.Expressions;

namespace Scryber.Expressive.Functions.String
{
    public class EndsWithFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name
        {
            get
            {
                return "EndsWith";
            }
        }

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
            this.ValidateParameterCount(parameters, 2, 2);

            var text = (string)parameters[0].Evaluate(variables);
            var value = (string)parameters[1].Evaluate(variables);

            if (value is null)
            {
                return false;
            }
            
            return text?.EndsWith(value, context.EqualityStringComparison) == true;
        }

        #endregion
    }
}
