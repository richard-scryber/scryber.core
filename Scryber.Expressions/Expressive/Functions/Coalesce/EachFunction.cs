using System;
using System.Collections;
using System.Collections.Generic;
using Scryber.Expressive.Expressions;
using Scryber.Expressive.Helpers;

namespace Scryber.Expressive.Functions.Coalesce
{
	public class EachFunction : FunctionBase
	{
        public override string Name
		{
			get
			{
				return "Each";
			}
		}

        public EachFunction()
		{
		}

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
			this.ValidateParameterCount(parameters, -1, 1);

			var list = new ResultList();
			Helpers.Collections.DoForEach(parameters, variables, context, (val, vars, cont) =>
			{
				list.Add(val);
				return 1;
			});

			return list.ToArray();

			
        }

    }

}

