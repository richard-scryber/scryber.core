using Scryber.Expressive.Expressions;
using System;
using System.Collections.Generic;

namespace Scryber.Expressive.Functions.Date
{
    public class AddDaysFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name => "AddDays";

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
            this.ValidateParameterCount(parameters, 2, 2);

            var dateObject = parameters[0].Evaluate(variables);
            var daysObject = parameters[1].Evaluate(variables);

            if (dateObject is null || daysObject is null) { return null; }

            var date = Convert.ToDateTime(dateObject, context.CurrentCulture);
            var days = Convert.ToDouble(daysObject, context.CurrentCulture);

            return date.AddDays(days);
        }

        #endregion
    }
}
