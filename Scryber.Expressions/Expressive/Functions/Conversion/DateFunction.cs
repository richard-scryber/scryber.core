using Scryber.Expressive.Expressions;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Scryber.Expressive.Functions.Conversion
{
    public class DateFunction : FunctionBase
    {
        
        private static readonly DateTime EpochDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        #region FunctionBase Members

        public override string Name => "Date";

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, ExpressionContext context)
        {
            if (parameters.Length == 0)
                return DateTime.Now;

            var objectToConvert = parameters[0].Evaluate(variables);
            
            // No point converting if there is nothing to convert.
            if (objectToConvert is null) { return null; }

            // Safely check for a format parameter.
            if (parameters.Length > 1 &&
                objectToConvert is string dateString)
            {
                var format = parameters[1].Evaluate(variables);

                if (format is string formatString)
                {
                    //read in the current culture (e.g. 'Januar' for de-DE), then the invariant culture
                    //so English month and day names and AM/PM are still read on any machine or browser.
                    if (DateTime.TryParseExact(dateString, formatString, context.CurrentCulture, DateTimeStyles.None, out var parsed))
                        return parsed;
                    if (DateTime.TryParseExact(dateString, formatString, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsed))
                        return parsed;

                    //neither, so fail with the error for the current culture
                    return DateTime.ParseExact(dateString, formatString, context.CurrentCulture);
                }
            }
            else if(objectToConvert is int i)
            {
                var timespan = new TimeSpan(i * TimeSpan.TicksPerMillisecond);
                var date = EpochDate;
                date = date.Add(timespan);
                return date;
            }
            else if(objectToConvert is long l)
            {
                var timespan = new TimeSpan(l * TimeSpan.TicksPerMillisecond);
                var date = EpochDate;
                date = date.Add(timespan);
                return date;
            }
            else if(long.TryParse(objectToConvert.ToString(), out l))
            {
                var timespan = new TimeSpan(l * TimeSpan.TicksPerMillisecond);
                var date = EpochDate;
                date = date.Add(timespan);
                return date;
            }

            //text without a format is read in the current culture, then the invariant culture
            if (objectToConvert is string text)
            {
                if (DateTime.TryParse(text, context.CurrentCulture, DateTimeStyles.None, out var parsed))
                    return parsed;
                if (DateTime.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsed))
                    return parsed;
            }

            return Convert.ToDateTime(objectToConvert, context.CurrentCulture);
        }

        #endregion
    }
}
