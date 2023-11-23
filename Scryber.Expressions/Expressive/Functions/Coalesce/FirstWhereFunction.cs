using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scryber.Expressive.Expressions;

namespace Scryber.Expressive.Functions.Coalesce
{
	public class FirstWhereFunction : FunctionBase
	{

        public override string Name
		{
			get { return "FirstWhere"; }
		}

        public FirstWhereFunction()
		{
		}

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
            this.ValidateParameterCount(parameters, 2, 2);

            IExpression each = parameters[0];
            IExpression check = parameters[1];

            object value = each.Evaluate(variables);
            object matched = null;
            IEnumerable entries;

            if (Helpers.Collections.TryIsCollection(value, out entries))
            {
                foreach (var item in entries)
                {
                    CurrentDataExpression.SetCurrentData(item, variables);
                    var match = check.Evaluate(variables);
                    if (match is bool b && b)
                    {
                        matched = item;
                        break;
                    }
                }

                return matched;
            }
            else
            {
                CurrentDataExpression.SetCurrentData(value, variables);
                var match = check.Evaluate(variables);
                if (match is bool b && b)
                    return  value;
                else
                    return null;
            }
        }

    }
}

