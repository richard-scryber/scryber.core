using System;
using System.Collections;
using System.Collections.Generic;
using Scryber.Expressive.Expressions;
using Scryber.Expressive.Helpers;

namespace Scryber.Expressive.Functions.Coalesce
{
	public class SortByFunction : FunctionBase
	{
        public override string Name
		{
			get
			{
				return "SortBy";
			}
		}

        public SortByFunction()
		{
		}

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
            this.ValidateParameterCount(parameters, -1, 2);

            object value;
            
            IExpression each = parameters[0];
            IExpression lookup = parameters[1];

            bool ascending = true;

            if (parameters.Length > 2)
            {
                var direction = parameters[2].Evaluate(variables);
                if (direction != null && direction.ToString().ToLower() == "desc")
                    ascending = false;
            }


            value = each.Evaluate(variables);
            IEnumerable enumerate;
            SortedList all = new SortedList(new SortByComparer(context, ascending));

            if (Helpers.Collections.TryIsCollection(value, out enumerate))
            {
                foreach (var item in enumerate)
                {
                    CurrentDataExpression.SetCurrentData(item, variables);
                    var one = lookup.Evaluate(variables);
                    all.Add(one, item);
                }
            }

            return all.Values;

        }

        private class SortByComparer : IComparer
        {
            private Context _context;
            private bool _ascending;

            public SortByComparer(Context context, bool ascending)
            {
                _context = context;
                _ascending = ascending;
            }

            public int Compare(object one, object two)
            {
                if (null == one && null == two)
                    return 0;
                if (null == one)
                    return -1;
                else if (null == two)
                    return 1;
                else
                {
                    var result = Comparison.CompareUsingMostPreciseType(one, two, _context);
                    if (!_ascending)
                        result = -(result);

                    return result;
                }

            }
        }
    }

}

