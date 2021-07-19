using Scryber.Expressive.Expressions;
using System;

namespace Scryber.Expressive.Functions.Date
{
    public class AddMillisecondsFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name => "AddMilliseconds";

        public override object Evaluate(IExpression[] parameters, Context context)
        {
            this.ValidateParameterCount(parameters, 2, 2);

            var dateObject = parameters[0].Evaluate(this.Variables);
            var millisecondsObject = parameters[1].Evaluate(this.Variables);

            if (dateObject is null || millisecondsObject is null) { return null; }

            var date = Convert.ToDateTime(dateObject, context.CurrentCulture);
            var milliseconds = Convert.ToDouble(millisecondsObject, context.CurrentCulture);

            return date.AddMilliseconds(milliseconds);
        }

        #endregion
    }
}
