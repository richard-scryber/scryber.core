using System;
using System.Collections;
using System.Collections.Generic;
using Scryber.Expressive.Expressions;
using Scryber.Expressive.Helpers;

namespace Scryber.Expressive.Functions.Coalesce
{
	public class EachOfFunction : FunctionBase
	{
        public override string Name
		{
			get
			{
				return "EachOf";
			}
		}

        public EachOfFunction()
		{
		}

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
			this.ValidateParameterCount(parameters, 2, 2);

			object value;
			IExpression lookup = parameters[1];

			IExpression each = parameters[0];

			value = each.Evaluate(variables);

			ArrayList all = new ArrayList();
			IEnumerable enumerate;

			if(Helpers.Collections.TryIsCollection(value, out enumerate))
			{
				foreach (var item in enumerate)
				{

					CurrentDataExpression.SetCurrentData(item, variables);
					var one = lookup.Evaluate(variables);

					if(null != one)
					{
						if (one is string s)
						{
							if (!string.IsNullOrEmpty(s))
								all.Add(s);
						}
						else if (one is bool b)
						{
							if (b)
							{
								all.Add(one);
							}
						}
						else if (!(one is DBNull))
						{
							all.Add(one);
						}
					}
					
				}
			}

			return all;
        }
    }
}

