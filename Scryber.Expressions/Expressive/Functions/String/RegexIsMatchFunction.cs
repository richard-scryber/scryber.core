using Scryber.Expressive.Expressions;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Scryber.Expressive.Functions.String
{
    public class RegexIsMatchFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name { get { return "IsMatch"; } }

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
            this.ValidateParameterCount(parameters, 2, 2);
            
            var input = parameters[0].Evaluate(variables) as string;
            var pattern = parameters[1].Evaluate(variables) as string;

            if (string.IsNullOrEmpty(pattern))
                return null;
            else if (string.IsNullOrEmpty(input))
                return null;
            else
            {
                return Regex.IsMatch(input, pattern);
            }

            //return Regex.IsMatch(input, pattern);
            //return new Regex(parameters[1].Evaluate(variables) as string).IsMatch(parameters[0].Evaluate(variables) as string);
        }

        #endregion
    }
}
