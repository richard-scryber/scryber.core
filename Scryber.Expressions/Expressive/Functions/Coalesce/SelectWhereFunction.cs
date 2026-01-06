using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scryber.Expressive.Expressions;

namespace Scryber.Expressive.Functions.Coalesce
{
	public class SelectWhereFunction : FunctionBase
	{

        public override string Name
		{
			get { return "SelectWhere"; }
		}

        public SelectWhereFunction()
		{
		}

        private static readonly object[] Empty = {};

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
            this.ValidateParameterCount(parameters, 2, 2);

            IExpression each = parameters[0];
            IExpression check = parameters[1];

            object value = each.Evaluate(variables);
            ArrayList matched = new ResultList();
            IEnumerable entries;

            if (Helpers.Collections.TryIsCollection(value, out entries))
            {
                foreach (var item in entries)
                {
                    CurrentDataExpression.SetCurrentData(item, variables);
                    var match = check.Evaluate(variables);
                    if ( match is bool b && b)
                        matched.Add(item);

                }

                return matched.ToArray();
            }
            else
            {
                CurrentDataExpression.SetCurrentData(value, variables);
                var match = check.Evaluate(variables);
                if (match is bool b && b)
                    return new object[] { value };
                else
                    return Empty;
            }
        }

    }
}

