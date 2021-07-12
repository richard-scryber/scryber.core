using Scryber.Expressive.Expressions;
using System;

namespace Scryber.Expressive.Functions.Date
{
    internal sealed class HourOfFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name => "HourOf";

        public override object Evaluate(IExpression[] parameters, Context context)
        {
            this.ValidateParameterCount(parameters, 1, 1);

            var dateObject = parameters[0].Evaluate(this.Variables);

            if (dateObject is null) { return null; }

            var date = Convert.ToDateTime(dateObject, context.CurrentCulture);

            return date.Hour;
        }

        #endregion
    }
}
