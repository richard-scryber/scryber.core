using Scryber.Expressive.Expressions;
using System;
using System.Collections.Generic;

namespace Scryber.Expressive.Functions.Mathematical
{
    public class LogFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name { get { return "Log"; } }

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
            this.ValidateParameterCount(parameters, 2, 2);

            return Math.Log(Convert.ToDouble(parameters[0].Evaluate(variables)), Convert.ToDouble(parameters[1].Evaluate(variables)));
        }

        #endregion
    }
}
