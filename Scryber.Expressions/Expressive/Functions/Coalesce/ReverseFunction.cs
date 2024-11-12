using System;
using System.Collections;
using System.Collections.Generic;
using Scryber.Expressive.Expressions;
using Scryber.Expressive.Helpers;
using Scryber.PDF.Native;

namespace Scryber.Expressive.Functions.Coalesce
{
	public class ReverseFunction : FunctionBase
	{
        public override string Name
		{
			get
			{
				return "Reverse";
			}
		}

        public ReverseFunction()
		{
		}

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
			this.ValidateParameterCount(parameters, -1, 1);

			var list = new ResultList();
			Helpers.Collections.DoForEach(parameters, variables, context, (val, vars, cont) =>
			{
				if (Helpers.Collections.TryIsCollection(val, out var collection))
				{
					var total = Helpers.Collections.DoForEach(collection, variables, context, (innerVal, innerVars, innerContext) =>
					{
						list.Add(innerVal);
						return 1;
					});
					return total;
				}
				else
				{
					list.Add(val);
					return 1;
				}
			});
			list.Reverse();
			return list.ToString();

			
        }

    }

}

