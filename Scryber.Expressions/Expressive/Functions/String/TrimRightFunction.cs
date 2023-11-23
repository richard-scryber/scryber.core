using System.Collections.Generic;
using Scryber.Expressive.Expressions;

namespace Scryber.Expressive.Functions.String
{
    public class TrimRightFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name
        {
            get
            {
                return "TrimRight";
            }
        }

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
            this.ValidateParameterCount(parameters, 1, 1);

            object value = parameters[0].Evaluate(variables);

            if (value is null)
            {
                return null;
            }

            // Safely handle the text input, if not text then call ToString.
            string text = null;
            if (value is string)
            {
                text = (string)value;
            }
            else
            {
                text = value.ToString();
            }

            return text?.TrimEnd();
            
        }

        #endregion
    }
}
