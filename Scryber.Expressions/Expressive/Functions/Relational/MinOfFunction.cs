﻿using System;
using System.Collections;
using System.Collections.Generic;
using Scryber.Expressive.Expressions;
using Scryber.Expressive.Helpers;

namespace Scryber.Expressive.Functions.Relational
{
	public class MinOfFunction : FunctionBase
	{
        public override string Name
		{
			get
			{
				return "MinOf";
			}
		}

        public MinOfFunction()
		{
		}

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
			this.ValidateParameterCount(parameters, 2, 2);

			object value;
			IExpression lookup = parameters[1];

			IExpression each = parameters[0];

			value = each.Evaluate(variables);

			IComparable min = null;

			if(value is IEnumerable enumerate)
			{
				foreach (var item in enumerate)
				{

					CurrentDataExpression.SetCurrentData(item, variables);
					var one = lookup.Evaluate(variables);

					if(one is IComparable compare)
					{
						if (null == min)
							min = compare;
						else
							min = (min.CompareTo(compare) > 0 ? compare : min);
					}
					
				}
			}

			return min;
        }
    }
}

