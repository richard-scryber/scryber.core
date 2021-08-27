using Scryber.Expressive.Expressions;
using System;
using System.Collections.Generic;

namespace Scryber.Expressive.Functions.Date
{
    public class AddMillisecondsFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name => "AddMilliseconds";

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
            this.ValidateParameterCount(parameters, 2, 2);

            var dateObject = parameters[0].Evaluate(variables);
            var millisecondsObject = parameters[1].Evaluate(variables);

            if (dateObject is null || millisecondsObject is null) { return null; }

            var date = Convert.ToDateTime(dateObject, context.CurrentCulture);
            var milliseconds = Convert.ToDouble(millisecondsObject, context.CurrentCulture);

            return date.AddMilliseconds(milliseconds);
        }

        #endregion
    }
}
