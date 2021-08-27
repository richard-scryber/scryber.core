using Scryber.Expressive.Expressions;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Scryber.Expressive.Functions.String
{
    public class RegexFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name { get { return "Regex"; } }

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
            this.ValidateParameterCount(parameters, 2, 2);
            
            return new Regex(parameters[1].Evaluate(variables) as string).IsMatch(parameters[0].Evaluate(variables) as string);
        }

        #endregion
    }
}
