using System;
using Scryber.Expressive.Expressions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scryber.Expressive.Helpers;

namespace Scryber.Expressive.Functions.Statistical
{
    public class ModeFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name { get { return "Mode"; } }

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, ExpressionContext context)
        {
            this.ValidateParameterCount(parameters, -1, 1);
            
            //numbers (and text that reads as a number) are compared as numbers, any other value as it is (e.g. text)
            IList<object> values = new List<object>();

            foreach (var p in parameters)
            {
                var value = p.Evaluate(variables);
                value = Comparison.ExtractAnyJsonValue(value);

                if (Collections.TryIsCollection(value, out var enumerable))
                {
                    foreach (var item in enumerable)
                    {
                        var inner = Comparison.ExtractAnyJsonValue(item);
                        values.Add(ToModeValue(inner, context));
                    }
                }
                else
                {
                    values.Add(ToModeValue(value, context));
                }
            }

            if (values.Count == 0)
                return null;

            var groups = values.GroupBy(v => v);
            //TODO: This can be improved, as we just need the max count
            int maxCount = groups.Max(g => g.Count());
            return groups.First(g => g.Count() == maxCount).Key;
        }

        private static object ToModeValue(object value, ExpressionContext context)
        {
            switch (value)
            {
                case null:
                    return null;
                case string text:
                    if (decimal.TryParse(text, System.Globalization.NumberStyles.Number, context.DecimalCurrentCulture, out var parsed))
                        return parsed;
                    return text;
                case bool:
                case DateTime:
                    return value;
                case IConvertible:
                    try
                    {
                        return Convert.ToDecimal(value, context.DecimalCurrentCulture);
                    }
                    catch (InvalidCastException)
                    {
                        return value;
                    }
                    catch (FormatException)
                    {
                        return value;
                    }
                default:
                    return value;
            }
        }

        #endregion
    }
}
