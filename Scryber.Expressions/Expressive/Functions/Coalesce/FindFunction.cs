using System;
using System.Collections;
using System.Collections.Generic;
using Scryber.Expressive.Expressions;
using Scryber.Expressive.Helpers;
using Scryber.PDF.Native;

namespace Scryber.Expressive.Functions.Coalesce
{
	[Obsolete("The function 'find' needs implementing with a reference to the root adding to the items dictionary")]
	public class FindFunction : FunctionBase
	{
        public override string Name
		{
			get
			{
				return "Find";
			}
		}

        public FindFunction()
		{
		}

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, ExpressionContext context)
        {
			this.ValidateParameterCount(parameters, 2, 1);

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

			return list.ToArray();

			
        }

    }

}

