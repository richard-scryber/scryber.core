using Scryber.Expressive.Expressions;
using System;

namespace Scryber.Expressive.Functions.Date
{
    public class AddMinutesFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name => "AddMinutes";

        public override object Evaluate(IExpression[] parameters, Context context)
        {
            this.ValidateParameterCount(parameters, 2, 2);

            var dateObject = parameters[0].Evaluate(this.Variables);
            var minutesObject = parameters[1].Evaluate(this.Variables);

            if (dateObject is null || minutesObject is null) { return null; }

            var date = Convert.ToDateTime(dateObject, context.CurrentCulture);
            var days = Convert.ToDouble(minutesObject, context.CurrentCulture);

            return date.AddMinutes(days);
        }

        #endregion
    }
}
