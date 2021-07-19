using Scryber.Expressive.Expressions;
using System;

namespace Scryber.Expressive.Functions.Date
{
    public class AddHoursFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name => "AddHours";

        public override object Evaluate(IExpression[] parameters, Context context)
        {
            this.ValidateParameterCount(parameters, 2, 2);

            var dateObject = parameters[0].Evaluate(this.Variables);
            var hoursObject = parameters[1].Evaluate(this.Variables);

            if (dateObject is null || hoursObject is null) { return null; }

            var date = Convert.ToDateTime(dateObject, context.CurrentCulture);
            var hours = Convert.ToDouble(hoursObject, context.CurrentCulture);

            return date.AddHours(hours);
        }

        #endregion
    }
}
