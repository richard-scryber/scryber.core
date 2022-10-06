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
#if NET6_0_OR_GREATER
			else if (value is System.Text.Json.JsonElement jele && jele.ValueKind == System.Text.Json.JsonValueKind.Array)
			{
				var all = new ArrayList(jele.GetArrayLength());

				foreach (var inner in jele.EnumerateArray())
				{
					all.Add(inner);
				}
				collection = all;
				return true;
			}

#endif
			else if (value is Newtonsoft.Json.Linq.JToken jt && jt.Type == Newtonsoft.Json.Linq.JTokenType.Array)
			{
				collection = (IEnumerable)value;
				return true;
			}
			else if(value is Newtonsoft.Json.Linq.JToken)
			{
				collection = null;
				return false;
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

		public static int DoForEach(IEnumerable parameters, IDictionary<string, object> variables, Context context, Func<object, IDictionary<string, object>, Context, int> callback)
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
				else if(item is Newtonsoft.Json.Linq.JToken token)
				{
					if (token.Type == Newtonsoft.Json.Linq.JTokenType.Array)
					{
						var all = ((Newtonsoft.Json.Linq.JArray)token);
						foreach (var inner in all)
						{
							count += callback(inner, variables, context);
						}
					}
					else
					{
						object value = PropertyExpression.GetJTokenValue(token);
						count += callback(value, variables, context);
					}
                }
#if NET6_0_OR_GREATER

				else if(item is System.Text.Json.JsonElement element)
				{

				}
#endif

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

		private static int InnerDoForEach(object item, IDictionary<string, object> variables, Context context, Func<object, IDictionary<string,object>, Context, int> callback)
		{
			int count = 0;

            if (item is string)
            {
                count += callback(item, variables, context);
            }
            else if (item is Newtonsoft.Json.Linq.JToken token)
            {
                if (token.Type == Newtonsoft.Json.Linq.JTokenType.Array)
                {
                    var all = ((Newtonsoft.Json.Linq.JArray)token);
                    foreach (var inner in all)
                    {
						var one = PropertyExpression.GetJTokenValue(inner);
                        count += callback(one, variables, context);
                    }
                }
                else
                {
                    object value = PropertyExpression.GetJTokenValue(token);
                    count += callback(value, variables, context);
                }
            }
#if NET6_0_OR_GREATER

            else if (item is System.Text.Json.JsonElement element)
            {
				if (element.ValueKind == System.Text.Json.JsonValueKind.Array)
				{
					int entries = element.GetArrayLength();
					for (var i = 0; i < entries; i++)
					{
						var one = element[i];
						object value = PropertyExpression.GetJsonElementValue(one);
						count += callback(value, variables, context);
					}
				}
				else
				{
					object value = PropertyExpression.GetJsonElementValue(element);
					count += callback(value, variables, context);
				}

            }
#endif

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

