using Scryber.Expressive.Expressions;
using System;
using System.Collections.Generic;

namespace Scryber.Expressive.Functions.String
{
    public class PadLeftFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name
        {
            get
            {
                return "PadLeft";
            }
        }

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
            this.ValidateParameterCount(parameters, -1, 2);

            object value = parameters[0].Evaluate(variables);
            object length = parameters[1].Evaluate(variables);

            if (value is null || length is null) { return null; }

            string text = null;
            if (value is string)
            {
                text = (string)value;
            }
            else
            {
                text = value.ToString();
            }

            int totalLength = Convert.ToInt32(length);

            var third = (parameters.Length > 2) ? parameters[2].Evaluate(variables) : null;
            
            char character = ' ';

            if (third is char)
            {
                character = (char)third;
            }
            else if (third is string)
            {
                character = (char)((string)third)[0];
            }
            
            return text.PadLeft(totalLength, character);
        }

        #endregion
    }
}
