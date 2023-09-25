using System;
using System.Collections;
using System.Collections.Generic;
using Scryber.Expressive.Expressions;
using Scryber.Expressive.Helpers;

namespace Scryber.Expressive.Functions.Statistical
{
	public class AverageOfFunction : FunctionBase
	{
        public override string Name
		{
			get
			{
				return "AverageOf";
			}
		}

        public AverageOfFunction()
		{
		}

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
			this.ValidateParameterCount(parameters, 2, 2);

			object value;
			IExpression lookup = parameters[1];

			IExpression each = parameters[0];

			value = each.Evaluate(variables);

			int count = 0;

            object total = 0;

            if (value is IEnumerable enumerate)
            {
                foreach (var item in enumerate)
                {
                    CurrentDataExpression.SetCurrentData(item, variables);
                    var one = lookup.Evaluate(variables);
                    total = Numbers.Add(total ?? 0, one ?? 0);
                    count++;
                }
            }

            return Numbers.Divide(total, (double)count);
        }
    }
}

