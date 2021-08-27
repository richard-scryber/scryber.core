using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Scryber.Expressive.Expressions;

namespace Scryber.Expressive.Functions.String
{
    public class JoinFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name
        {
            get
            {
                return "Join";
            }
        }

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
            //We can accept 1 parameter that is an array
            this.ValidateParameterCount(parameters, -1, 1);

            StringBuilder sb = new StringBuilder();
            string separator = null;

            foreach (var p in parameters)
            {
                if (null == separator)
                    separator = (p.Evaluate(variables) ?? "").ToString();

                else
                {
                    Evaluate(separator, sb, p, variables, context);
                }
            }

            //return the final full string
            return sb.ToString();
        }

  
        protected virtual void Evaluate(string separator, StringBuilder sb, object p, IDictionary<string, object> variables, Context context)
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
                return;
            }
            else if (value is string)
            {
                if (sb.Length > 0)
                    sb.Append(separator);

                sb.Append(value);
            }
            else if (value is IEnumerable enumerate)
            {
                foreach (var item in enumerate)
                {
                    Evaluate(separator, sb, item, variables, context);
                }
            }
            else
            {
                string s = value.ToString();

                if (sb.Length > 0)
                    sb.Append(separator);

                sb.Append(s);
            }


        }

        #endregion
    }
}
