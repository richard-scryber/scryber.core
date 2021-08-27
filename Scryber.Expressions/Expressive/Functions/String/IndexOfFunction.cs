using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Scryber.Expressive.Expressions;

namespace Scryber.Expressive.Functions.String
{
    public class IndexOfFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name
        {
            get
            {
                return "IndexOf";
            }
        }

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
            this.ValidateParameterCount(parameters, -1, 2);

            object value = parameters[0].Evaluate(variables);

            if (value is null)
            {
                return null;
            }
            else if (value is string)
            {
                //String comparison
                //Can use an optional start index parameter

                string text = value.ToString();

                value = parameters[1].Evaluate(variables);

                if (value is null)
                {
                    return null;
                }

                string match = value.ToString();

                if (parameters.Length > 2)
                {
                    int start = Convert.ToInt32(parameters[2].Evaluate(variables));
                    return text.IndexOf(match, start, context.EqualityStringComparison);
                }
                else
                {
                    return text.IndexOf(match, context.EqualityStringComparison);
                }
            }
            else if (value is IEnumerable col)
            {
                //Array (or collection) comparison
                
                var index = 0;
                
                value = parameters[1].Evaluate(variables);

                foreach (var p in col)
                {
                    object innerValue = p;
                    if(innerValue is IExpression)
                    {
                        innerValue = (innerValue as IExpression).Evaluate(variables);
                    }

                    if(null != innerValue)
                    {
                        if(innerValue is string innerStr)
                        {
                            if (value is string valueStr)
                            {
                                if (innerStr.Equals(valueStr, context.EqualityStringComparison))
                                {
                                    return index;
                                }
                            }
                            //else they are not the same as one is a string
                        }
                        else if(value.Equals(innerValue))
                        {
                            return index;
                        }
                    }
                    index++;
                }
                return -1;
            }
            else if (value is IComparable)
            {
                var comp = (value as IComparable);

                value = parameters[1].Evaluate(variables);

                if (comp.CompareTo(value) == 0)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        }

        #endregion
    }
}
