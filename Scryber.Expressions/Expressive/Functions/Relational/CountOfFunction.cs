using System;
using System.Collections;
using System.Collections.Generic;
using Scryber.Expressive.Expressions;
using Scryber.Expressive.Helpers;

namespace Scryber.Expressive.Functions.Relational
{
	public class CountOfFunction : FunctionBase
	{
        public override string Name
		{
			get
			{
				return "CountOf";
			}
		}

        public CountOfFunction()
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

			if(Helpers.Collections.TryIsCollection(value, out IEnumerable enumerate))
			{
				foreach (var item in enumerate)
				{

					CurrentDataExpression.SetCurrentData(item, variables);
					var one = lookup.Evaluate(variables);
					if (null == one)
					{
						//skip
					}
					else if (one is bool b && !b)
					{
						//skip
					}
					else
					{
						count += 1;
					}
				}
			}

			return count;
        }
    }
}

