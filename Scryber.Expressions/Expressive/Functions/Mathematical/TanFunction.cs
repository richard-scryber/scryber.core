using Scryber.Expressive.Expressions;
using System;
using System.Collections.Generic;

namespace Scryber.Expressive.Functions.Mathematical
{
    public class TanFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name { get { return "Tan"; } }

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
            this.ValidateParameterCount(parameters, 1, 1);

            return Math.Tan(Convert.ToDouble(parameters[0].Evaluate(variables)));
        }

        #endregion
    }
}
