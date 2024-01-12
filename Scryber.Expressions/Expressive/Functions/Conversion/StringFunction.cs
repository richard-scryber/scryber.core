using System.Collections.Generic;
using Scryber.Expressive.Expressions;

namespace Scryber.Expressive.Functions.Conversion
{
    public class StringFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name => "String";

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
            this.ValidateParameterCount(parameters, -1, 1);

            var objectToConvert = parameters[0].Evaluate(variables);

            // No point converting if there is nothing to convert.
            if (objectToConvert is null) { return null; }
            
            // Safely check for a format parameter.
            if (parameters.Length > 1)
            {
                var first = parameters[1].Evaluate(variables);

                string formatString;
                System.Globalization.CultureInfo cultureInfo;

                if (parameters.Length > 2)
                {
                    var second = parameters[2].Evaluate(variables);

                    if (second is System.Globalization.CultureInfo)
                        cultureInfo = second as System.Globalization.CultureInfo;

                    else if (second is string)
                        cultureInfo = System.Globalization.CultureInfo.GetCultureInfo(second as string);

                    else
                        cultureInfo = null;
                }
                else
                    cultureInfo = context.CurrentCulture;

                if (first is string)
                {
                    formatString = first as string;
                }
                else
                    formatString = "";


                string converted;

                if (objectToConvert is System.IFormattable conv)
                    converted = conv.ToString(formatString, cultureInfo);
                else
                {
                    if (!string.IsNullOrEmpty(formatString))
                        formatString = $"{{0:{formatString}}}";
                    else
                        formatString = $"{{0}}";

                    converted = string.Format(cultureInfo, formatString, objectToConvert);
                }

                //HACK:replace any narrow non-breaking space with a standard non-breaking space.
                converted = converted.Replace((char)8239, (char)160); 
                return converted;
            }

            return objectToConvert.ToString();
        }

        #endregion
    }
}
