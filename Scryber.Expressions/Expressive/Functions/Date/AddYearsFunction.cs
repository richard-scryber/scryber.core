using Scryber.Expressive.Expressions;
using System;
using System.Collections.Generic;

namespace Scryber.Expressive.Functions.Date
{
    public class AddYearsFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name => "AddYears";

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
            this.ValidateParameterCount(parameters, 2, 2);

            var dateObject = parameters[0].Evaluate(variables);
            var yearsObject = parameters[1].Evaluate(variables);

            if (dateObject is null || yearsObject is null) { return null; }

            var date = Convert.ToDateTime(dateObject, context.CurrentCulture);
            var years = Convert.ToInt32(yearsObject, context.CurrentCulture);

            return date.AddYears(years);
        }

        #endregion
    }
}
