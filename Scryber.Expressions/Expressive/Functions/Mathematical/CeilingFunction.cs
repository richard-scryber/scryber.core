using Scryber.Expressive.Expressions;
using System;
using System.Collections.Generic;

namespace Scryber.Expressive.Functions.Mathematical
{
    public class CeilingFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name { get { return "Ceiling"; } }

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
            this.ValidateParameterCount(parameters, 1, 1);

            var value = parameters[0].Evaluate(variables);

            if (value is double)
            {
                return Math.Ceiling((double)value);
            }
            else if (value is decimal)
            {
                return Math.Ceiling((decimal)value);
            }
            return Math.Ceiling(Convert.ToDouble(value));
        }

        #endregion
    }
}
