using Scryber.Expressive.Expressions;
using System;
using System.Collections.Generic;

namespace Scryber.Expressive.Functions.Mathematical
{
    public class RandomFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name { get { return "Random"; } }
        
        
        private static readonly Random _random = new Random();

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
            this.ValidateParameterCount(parameters, -1, -1);

            object min = parameters.Length > 0 ? parameters[0].Evaluate(variables) : 0.0;
            object max = parameters.Length > 1 ? parameters[1].Evaluate(variables) : 1.0;

            var random = _random;

            if (min is int && max is int)
            {
                return random.Next((int)min, (int)max);
            }
            else if (min is double || max is double)
            {
                var value = random.NextDouble();
                var typedMin = Convert.ToDouble(min);
                var scale = Convert.ToDouble(max) - typedMin;

                return typedMin + (scale * value);
            }
            else if(min is decimal || max is decimal)
            {
                var value = random.NextDouble();
                var typedMin = Convert.ToDouble(min);
                var scale = Convert.ToDouble(max) - typedMin;

                return typedMin + (scale * value);
            }

            return null;
        }

        #endregion
    }
}
