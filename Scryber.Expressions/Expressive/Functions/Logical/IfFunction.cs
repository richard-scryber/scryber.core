using Scryber.Expressive.Expressions;
using System;
using System.Collections.Generic;

namespace Scryber.Expressive.Functions.Logical
{
    public class IfFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name { get { return "If"; } }

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
            this.ValidateParameterCount(parameters, 3, 3);

            bool condition = Convert.ToBoolean(parameters[0].Evaluate(variables));

            return condition ? parameters[1].Evaluate(variables) : parameters[2].Evaluate(variables);
        }

        #endregion
    }
}
