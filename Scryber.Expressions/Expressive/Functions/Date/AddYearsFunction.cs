using Scryber.Expressive.Expressions;
using System;

namespace Scryber.Expressive.Functions.Date
{
    public class AddYearsFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name => "AddYears";

        public override object Evaluate(IExpression[] parameters, Context context)
        {
            this.ValidateParameterCount(parameters, 2, 2);

            var dateObject = parameters[0].Evaluate(this.Variables);
            var yearsObject = parameters[1].Evaluate(this.Variables);

            if (dateObject is null || yearsObject is null) { return null; }

            var date = Convert.ToDateTime(dateObject, context.CurrentCulture);
            var years = Convert.ToInt32(yearsObject, context.CurrentCulture);

            return date.AddYears(years);
        }

        #endregion
    }
}
