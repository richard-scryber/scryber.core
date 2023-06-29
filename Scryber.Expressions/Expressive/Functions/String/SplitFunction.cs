using System.Collections.Generic;
using Scryber.Expressive.Expressions;

namespace Scryber.Expressive.Functions.String
{
    public class SplitFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name
        {
            get
            {
                return "Split";
            }
        }

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
            this.ValidateParameterCount(parameters, -1, 2);

            object value = parameters[0].Evaluate(variables);
            var splitOn = parameters[1].Evaluate(variables);

            if (value is null) { return null; }
            if (splitOn is null) { return new string[] { value.ToString() }; }

            string text = value.ToString();
           
            if (!string.IsNullOrEmpty(text))
            {
                var all = text.Split(splitOn.ToString()[0]);
                return all;
            }
            else
            {
                return new string[] { };
            }
            
            
        }

        #endregion
    }
}
