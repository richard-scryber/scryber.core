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

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
            this.ValidateParameterCount(parameters, -1, 1);
            
            IList<decimal> values = new List<decimal>();

            foreach (var p in parameters)
            {
                var value = p.Evaluate(variables);
                value = Comparison.ExtractAnyJsonValue(value);

                if (Collections.TryIsCollection(value, out var enumerable))
                {
                    foreach (var item in enumerable)
                    {
                        var inner = Comparison.ExtractAnyJsonValue(item);
                        values.Add(Convert.ToDecimal(inner));
                    }
                }
                else
                {
                    values.Add(Convert.ToDecimal(value));
                }
            }

            var groups = values.GroupBy(v => v);
            //TODO: This can be improved, as we just need the max count
            int maxCount = groups.Max(g => g.Count());
            return groups.First(g => g.Count() == maxCount).Key;
        }

        #endregion
    }
}
