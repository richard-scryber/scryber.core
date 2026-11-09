using System;
using Scryber.Expressive.Expressions;

namespace Scryber.Expressive.Helpers
{
    /// <summary>
    /// Helpers for the functions that look up a property, or check a condition, on each item in a collection
    /// </summary>
    public static class Lookups
    {
        /// <summary>
        /// Resolves the lookup parameter for a collection function.
        /// If the parameter is a text constant such as 'name' (or 'address.city') then it is treated as the property
        /// of the current item, the same as .name (or .address.city). Any other expression is returned as it is.
        /// </summary>
        public static IExpression Resolve(IExpression lookup, ExpressionContext context)
        {
            if (lookup is ConstantValueExpression constant && constant.Evaluate(null) is string name)
            {
                name = name.Trim();
                if (name.StartsWith("."))
                    name = name.Substring(1);

                if (name.Length == 0)
                    return lookup;

                IExpression resolved = new CurrentDataExpression(context);
                foreach (var part in name.Split('.'))
                {
                    if (part.Length == 0)
                        return lookup;

                    resolved = new PropertyExpression(resolved, new ConstantValueExpression(part), context);
                }
                return resolved;
            }

            return lookup;
        }
    }
}
