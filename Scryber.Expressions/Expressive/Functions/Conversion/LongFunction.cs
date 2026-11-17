using Scryber.Expressive.Expressions;
using System;
using System.Collections.Generic;

namespace Scryber.Expressive.Functions.Conversion
{
    public class LongFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name => "Long";

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, ExpressionContext context)
        {
            this.ValidateParameterCount(parameters, 1, 1);

            var objectToConvert = parameters[0].Evaluate(variables);

            // No point converting if there is nothing to convert.
            if (objectToConvert is null) { return null; }
            
            if (objectToConvert is string str)
            {
                if (str.Length > 2)
                {
                    if (str.StartsWith("0x")) // hex conversion
                        return Convert.ToInt64(str, 16);
                    else if (str.StartsWith("0b"))
                        return Convert.ToInt64(str.Substring(2), 2);
                }
            }
            else if (objectToConvert is DateTime dt)
                return (long)(dt.Ticks / TimeSpan.TicksPerMillisecond);

            
            //text with a fraction (e.g. '10.6' from json) is read as a number (invariant), then rounded as a number is
            if (objectToConvert is string text && !long.TryParse(text, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out _)
                && double.TryParse(text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var number))
                return Convert.ToInt64(number);

            return Convert.ToInt64(objectToConvert, context.CurrentCulture);
        }

        #endregion
    }
}
