using Scryber.Expressive.Expressions;
using System;

namespace Scryber.Expressive.Functions.Conversion
{
    public class LongFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name => "Long";

        public override object Evaluate(IExpression[] parameters, Context context)
        {
            this.ValidateParameterCount(parameters, 1, 1);

            var objectToConvert = parameters[0].Evaluate(this.Variables);

            // No point converting if there is nothing to convert.
            if (objectToConvert is null) { return null; }

            return Convert.ToInt64(objectToConvert, context.CurrentCulture);
        }

        #endregion
    }
}
