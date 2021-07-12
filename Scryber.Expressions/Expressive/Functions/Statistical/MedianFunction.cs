using Scryber.Expressive.Expressions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Scryber.Expressive.Functions.Statistical
{
    internal class MedianFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name { get { return "Median"; } }

        public override object Evaluate(IExpression[] parameters, Context context)
        {
            this.ValidateParameterCount(parameters, -1, 1);

            IList<decimal> decimalValues = new List<decimal>();

            foreach (var p in parameters)
            {
                var value = p.Evaluate(this.Variables);

                if (value is IEnumerable enumerable)
                {
                    foreach (var item in enumerable)
                    {
                        decimalValues.Add(Convert.ToDecimal(item));
                    }
                }
                else
                {
                    decimalValues.Add(Convert.ToDecimal(value));
                }
            }

            return Median(decimalValues.ToArray());
        }

        #endregion

        private static decimal Median(IEnumerable<decimal> xs)
        {
            var ys = xs.OrderBy(x => x).ToList();
            var mid = (ys.Count - 1) / 2.0;
            return (ys[(int)(mid)] + ys[(int)(mid + 0.5)]) / 2;
        }
    }
}
