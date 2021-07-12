using Scryber.Expressive.Expressions;
using System;

namespace Scryber.Expressive.Functions.Date
{
    internal sealed class AddDaysFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name => "AddDays";

        public override object Evaluate(IExpression[] parameters, Context context)
        {
            this.ValidateParameterCount(parameters, 2, 2);

            var dateObject = parameters[0].Evaluate(this.Variables);
            var daysObject = parameters[1].Evaluate(this.Variables);

            if (dateObject is null || daysObject is null) { return null; }

            var date = Convert.ToDateTime(dateObject, context.CurrentCulture);
            var days = Convert.ToDouble(daysObject, context.CurrentCulture);

            return date.AddDays(days);
        }

        #endregion
    }
}
