using Scryber.Expressive.Expressions;
using System;
using System.Collections.Generic;

namespace Scryber.Expressive.Functions.Date
{
    public class AddHoursFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name => "AddHours";

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
            this.ValidateParameterCount(parameters, 2, 2);

            var dateObject = parameters[0].Evaluate(variables);
            var hoursObject = parameters[1].Evaluate(variables);

            if (dateObject is null || hoursObject is null) { return null; }

            var date = Convert.ToDateTime(dateObject, context.CurrentCulture);
            var hours = Convert.ToDouble(hoursObject, context.CurrentCulture);

            return date.AddHours(hours);
        }

        #endregion
    }
}
