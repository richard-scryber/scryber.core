using Scryber.Expressive.Expressions;
using System;
using System.Collections.Generic;

namespace Scryber.Expressive.Functions.Date
{
    public class HourOfFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name => "HourOf";

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
            this.ValidateParameterCount(parameters, 1, 1);

            var dateObject = parameters[0].Evaluate(variables);

            if (dateObject is null) { return null; }

            var date = Convert.ToDateTime(dateObject, context.CurrentCulture);

            return date.Hour;
        }

        #endregion
    }
}
