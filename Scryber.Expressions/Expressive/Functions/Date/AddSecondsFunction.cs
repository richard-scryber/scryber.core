using Scryber.Expressive.Expressions;
using System;
using System.Collections.Generic;

namespace Scryber.Expressive.Functions.Date
{
    public class AddSecondsFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name => "AddSeconds";

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
            this.ValidateParameterCount(parameters, 2, 2);

            var dateObject = parameters[0].Evaluate(variables);
            var secondsObject = parameters[1].Evaluate(variables);

            if (dateObject is null || secondsObject is null) { return null; }

            var date = Convert.ToDateTime(dateObject, context.CurrentCulture);
            var seconds = Convert.ToDouble(secondsObject, context.CurrentCulture);

            return date.AddSeconds(seconds);
        }

        #endregion
    }
}
