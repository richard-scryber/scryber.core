using Scryber.Expressive.Expressions;
using System;

namespace Scryber.Expressive.Functions.Date
{
    public class AddMonthsFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name => "AddMonths";

        public override object Evaluate(IExpression[] parameters, Context context)
        {
            this.ValidateParameterCount(parameters, 2, 2);

            var dateObject = parameters[0].Evaluate(this.Variables);
            var monthsObject = parameters[1].Evaluate(this.Variables);

            if (dateObject is null || monthsObject is null) { return null; }

            var date = Convert.ToDateTime(dateObject, context.CurrentCulture);
            var months = Convert.ToInt32(monthsObject, context.CurrentCulture);

            return date.AddMonths(months);
        }

        #endregion
    }
}
