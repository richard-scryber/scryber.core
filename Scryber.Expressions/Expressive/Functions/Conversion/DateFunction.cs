using Scryber.Expressive.Expressions;
using System;
using System.Globalization;

namespace Scryber.Expressive.Functions.Conversion
{
    public class DateFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name => "Date";

        public override object Evaluate(IExpression[] parameters, Context context)
        {
            this.ValidateParameterCount(parameters, -1, 1);

            var objectToConvert = parameters[0].Evaluate(this.Variables);
            
            // No point converting if there is nothing to convert.
            if (objectToConvert is null) { return null; }

            // Safely check for a format parameter.
            if (parameters.Length > 1 &&
                objectToConvert is string dateString)
            {
                var format = parameters[1].Evaluate(this.Variables);

                if (format is string formatString)
                {
                    return DateTime.ParseExact(dateString, formatString, context.CurrentCulture);
                }
            }

            return Convert.ToDateTime(objectToConvert, context.CurrentCulture);
        }

        #endregion
    }
}
