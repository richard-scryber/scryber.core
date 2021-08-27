using Scryber.Expressive.Expressions;
using System;
using System.Collections.Generic;

namespace Scryber.Expressive.Functions.Date
{
    public class MinutesBetweenFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name => "MinutesBetween";

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
            this.ValidateParameterCount(parameters, 2, 2);

            var startObject = parameters[0].Evaluate(variables);
            var endObject = parameters[1].Evaluate(variables);

            if (startObject is null || endObject is null) { return null; }

            var start = Convert.ToDateTime(startObject, context.CurrentCulture);
            var end = Convert.ToDateTime(endObject, context.CurrentCulture);

            return (end - start).TotalMinutes;
        }

        #endregion
    }
}
