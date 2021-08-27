using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Scryber.Expressive.Expressions;

namespace Scryber.Expressive.Functions.String
{
    public class ConcatFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name
        {
            get
            {
                return "Concat";
            }
        }

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
            //We can accept 1 parameter that is an array
            this.ValidateParameterCount(parameters, -1, 1);

            StringBuilder sb = new StringBuilder();

            //Recursive method if some parameters are arrays
            Evaluate(sb, parameters, variables, context);

            //return the final full string
            return sb.ToString();
        }

        protected virtual void Evaluate(StringBuilder sb, IEnumerable parameters, IDictionary<string, object> variables, Context context)
        {
            foreach (var p in parameters)
            {
                object value;

                if (p is IExpression expression)
                {
                    value = expression.Evaluate(variables);
                }
                else
                {
                    value = p;
                }

                if (null == value)
                {
                    continue;
                }
                else if(value is string)
                {
                    sb.Append(value);
                }
                else if(value is IEnumerable enumerate)
                {
                    Evaluate(sb, enumerate, variables, context);
                }
                else
                {
                    string s = value.ToString();
                    sb.Append(s);
                }

            }
        }

        #endregion
    }
}
