using System;
using System.Collections;
using System.Collections.Generic;
using Scryber.Expressive.Expressions;

namespace Scryber.Expressive.Helpers
{
	public static class Collections
	{

		public static bool TryIsCollection(object value, out IEnumerable collection)
		{
			if (value is string)
			{
				collection = null;
				return false;
			}
			else if (JsonValues.IsJson(value))
			{
				//only a JSON array is a collection (a JObject or JValue is also enumerable)
				return JsonValues.TryGetArray(value, out collection);
			}
			else if (value is IEnumerable enumerable)
			{
				collection = enumerable;
				return true;
			}
			else
			{
				collection = null;
				return false;
			}
		}

		public static int DoForEach(IEnumerable parameters, IDictionary<string, object> variables, ExpressionContext context, Func<object, IDictionary<string, object>, ExpressionContext, int> callback)
		{
			int count = 0;

			foreach (var item in parameters)
			{
				if(item is IExpression expr)
				{
					var value = expr.Evaluate(variables);

					count += InnerDoForEach(value, variables, context, callback);
				}
				else if(item is string)
				{
					count += callback(item, variables, context);
				}
				else if (JsonValues.TryGetArray(item, out var jsonItems))
				{
					//the entries of a JSON array, as JSON values
					foreach (var inner in jsonItems)
					{
						count += callback(inner, variables, context);
					}
				}
				else if (JsonValues.IsJson(item))
				{
					count += callback(JsonValues.ToNative(item), variables, context);
				}
				else if(item is IEnumerable enumerate)
				{
					foreach (var inner in enumerate)
					{
						count += callback(inner, variables, context);
					} 
				}
				else
				{
					count += callback(item, variables, context);
				}
			}

			return count;

		}

		private static int InnerDoForEach(object item, IDictionary<string, object> variables, ExpressionContext context, Func<object, IDictionary<string,object>, ExpressionContext, int> callback)
		{
			int count = 0;

            if (item is string)
            {
                count += callback(item, variables, context);
            }
            else if (JsonValues.TryGetArray(item, out var jsonItems))
            {
                //the native values of a JSON array
                foreach (var inner in jsonItems)
                {
                    count += callback(JsonValues.ToNative(inner), variables, context);
                }
            }
            else if (JsonValues.IsJson(item))
            {
                count += callback(JsonValues.ToNative(item), variables, context);
            }
            else if (item is IEnumerable enumerate)
            {
                foreach (var inner in enumerate)
                {
                    count += callback(inner, variables, context);
                }
            }
            else
            {
                count += callback(item, variables, context);
            }

			return count;
        }
	}
}

