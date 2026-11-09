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

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, ExpressionContext context)
        {
            this.ValidateParameterCount(parameters, -1, 1);

            object value;
            
            IExpression each = parameters[0];
            IExpression lookup;

            bool ascending = true;

            if (parameters.Length == 1)
            {
                //just a collection of plain values (e.g. keys, strings or numbers) sorted by the values themselves.
                lookup = new CurrentDataExpression(context);
            }
            else if (parameters.Length == 2 && IsDirection(parameters[1], variables, out var onlyDirection))
            {
                //sortBy(values, 'desc')
                lookup = new CurrentDataExpression(context);
                ascending = onlyDirection;
            }
            else
            {
                lookup = Helpers.Lookups.Resolve(parameters[1], context);

                if (parameters.Length > 2)
                {
                    var direction = parameters[2].Evaluate(variables);
                    if (direction != null && direction.ToString().ToLower() == "desc")
                        ascending = false;
                }
            }


            value = each.Evaluate(variables);
            IEnumerable enumerate;
            //A list of the keys and items, so items with the same key are all kept (in their original order).
            var all = new List<KeyValuePair<object, object>>();

            if (Helpers.Collections.TryIsCollection(value, out enumerate))
            {
                foreach (var item in enumerate)
                {
                    CurrentDataExpression.SetCurrentData(item, variables);
                    var one = lookup.Evaluate(variables);
                    all.Add(new KeyValuePair<object, object>(one, item));
                }
            }

            var comparer = new SortByComparer(context, ascending);
            var sorted = new List<KeyValuePair<object, object>>(System.Linq.Enumerable.OrderBy(all, kvp => kvp.Key, comparer));

            var array = new object[sorted.Count];
            for (var i = 0; i < sorted.Count; i++)
                array[i] = sorted[i].Value;
            return array;

        }

        /// <summary>
        /// Returns true if the expression is a text constant of 'asc' or 'desc', and sets ascending accordingly.
        /// </summary>
        private static bool IsDirection(IExpression expression, IDictionary<string, object> variables, out bool ascending)
        {
            ascending = true;

            //Only a constant text value is a direction. A property such as .desc is a lookup.
            if (expression is ConstantValueExpression && expression.Evaluate(variables) is string text)
            {
                if (string.Equals(text, "desc", StringComparison.OrdinalIgnoreCase))
                {
                    ascending = false;
                    return true;
                }
                else if (string.Equals(text, "asc", StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        private class SortByComparer : IComparer, IComparer<object>
        {
            private ExpressionContext _context;
            private bool _ascending;

            public SortByComparer(ExpressionContext context, bool ascending)
            {
                _context = context;
                _ascending = ascending;
            }

            int IComparer<object>.Compare(object one, object two)
            {
                return this.Compare(one, two);
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

