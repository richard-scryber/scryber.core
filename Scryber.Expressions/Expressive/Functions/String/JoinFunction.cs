using System;
using System.Collections;
using System.Text;
using Scryber.Expressive.Expressions;

namespace Scryber.Expressive.Functions.String
{
    internal class JoinFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name
        {
            get
            {
                return "Join";
            }
        }

        public override object Evaluate(IExpression[] parameters, Context context)
        {
            //We can accept 1 parameter that is an array
            this.ValidateParameterCount(parameters, -1, 1);

            StringBuilder sb = new StringBuilder();
            string separator = null;

            foreach (var p in parameters)
            {
                if (null == separator)
                    separator = (p.Evaluate(this.Variables) ?? "").ToString();

                else
                {
                    Evaluate(separator, sb, p, context);
                }
            }

            //return the final full string
            return sb.ToString();
        }

  
        protected virtual void Evaluate(string separator, StringBuilder sb, object p, Context context)
        {
            object value;

            if (p is IExpression expression)
            {
                value = expression.Evaluate(Variables);
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
                    Evaluate(separator, sb, item, context);
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
