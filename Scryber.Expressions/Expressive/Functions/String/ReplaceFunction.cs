using System.Collections.Generic;
using Scryber.Expressive.Expressions;

namespace Scryber.Expressive.Functions.String
{
    public class ReplaceFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name
        {
            get
            {
                return "Replace";
            }
        }

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
            this.ValidateParameterCount(parameters, -1, 3);

            object value = parameters[0].Evaluate(variables);
            var replace = parameters[1].Evaluate(variables);
            var replacement = parameters[2].Evaluate(variables);

            if (value is null) { return null; }
            if (replace is null) { return value.ToString(); }
            if(replacement is null) { replacement = ""; }

            string text = value.ToString();
           
            if (!string.IsNullOrEmpty(text))
            {
#if NETSTANDARD2_0_OR_GREATER
                var replaced = text.Replace(replace.ToString(), replacement.ToString());
#else
                var replaced = text.Replace(replace.ToString(), replacement.ToString(), context.EqualityStringComparison);
#endif
                return replaced;
            }
            else
            {
                return string.Empty;
            }
            
            
        }

#endregion
    }
}
