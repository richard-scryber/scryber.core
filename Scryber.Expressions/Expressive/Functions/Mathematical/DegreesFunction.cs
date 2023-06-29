using Scryber.Expressive.Expressions;
using System;
using System.Collections.Generic;

namespace Scryber.Expressive.Functions.Mathematical
{

    public class DegreesFunction : FunctionBase
    {
        private const double Factor = 180.0 / Math.PI;

        #region FunctionBase Members

        public override string Name { get { return "Deg"; } }

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
            this.ValidateParameterCount(parameters, 1, 1);
            var d = Convert.ToDouble(parameters[0].Evaluate(variables));
            d = d * Factor;
            return d;
        }

        #endregion
    }
}
