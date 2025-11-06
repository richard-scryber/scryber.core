using System;
using System.Collections;
using System.Collections.Generic;
using Scryber.Expressive.Expressions;
using Scryber.Expressive.Helpers;

namespace Scryber.Expressive.Functions.Relational
{
	public class MaxOfFunction : FunctionBase
	{
        public override string Name
		{
			get
			{
				return "MaxOf";
			}
		}

        public MaxOfFunction()
		{
		}

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
			this.ValidateParameterCount(parameters, 2, 2);

			object value;
			IExpression lookup = parameters[1];

			IExpression each = parameters[0];

			value = each.Evaluate(variables);

			object max = null;

			if(Helpers.Collections.TryIsCollection(value, out IEnumerable enumerate))
			{
				foreach (var item in enumerate)
				{

					CurrentDataExpression.SetCurrentData(item, variables);
					var one = lookup.Evaluate(variables);

					if (null == max)
					{
						max = one;
					}
					else if (null == one)
					{
						continue;
					}
                    else if (max is string || one is string)
                    {
	                    if (string.Compare(max.ToString(), one.ToString(), context.EqualityStringComparison) < 0)
		                    max = one.ToString();
                    }
                    else
                        max = Comparison.CompareUsingMostPreciseType(max, one, context) > 0 ? max : one;

					//if (one is IComparable compare)
					//{
					//	if (null == max)
					//		max = compare;
					//	else
					//		max = (max.CompareTo(compare) < 0 ? compare : max);
					//}
					
					
				}
			}
			else
			{
				CurrentDataExpression.SetCurrentData(value, variables);
				max = lookup.Evaluate(variables);
			}

			return max;
        }
    }
}

