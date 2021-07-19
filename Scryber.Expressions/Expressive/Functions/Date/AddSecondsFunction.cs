using Scryber.Expressive.Expressions;
using System;

namespace Scryber.Expressive.Functions.Date
{
    public class AddSecondsFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name => "AddSeconds";

        public override object Evaluate(IExpression[] parameters, Context context)
        {
            this.ValidateParameterCount(parameters, 2, 2);

            var dateObject = parameters[0].Evaluate(this.Variables);
            var secondsObject = parameters[1].Evaluate(this.Variables);

            if (dateObject is null || secondsObject is null) { return null; }

            var date = Convert.ToDateTime(dateObject, context.CurrentCulture);
            var seconds = Convert.ToDouble(secondsObject, context.CurrentCulture);

            return date.AddSeconds(seconds);
        }

        #endregion
    }
}
