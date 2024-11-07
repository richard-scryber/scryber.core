using Scryber.Expressive.Expressions;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace Scryber.Expressive.Functions.String
{
    public class RegexSwapFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name { get { return "Swap"; } }

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
            this.ValidateParameterCount(parameters, 3, 3);
            
            var input = parameters[0].Evaluate(variables) as string;
            var pattern = parameters[1].Evaluate(variables) as string;
            var replacement = parameters[2].Evaluate(variables) as string;
            
            if (string.IsNullOrEmpty(pattern))
                return null;
            else if (string.IsNullOrEmpty(input))
                return null;
            
            else
            {
                if (string.IsNullOrEmpty(replacement))
                    replacement = string.Empty;
                
                var all= Regex.Replace(input, pattern, replacement);

                return all;
            }
        }

        #endregion
    }
}