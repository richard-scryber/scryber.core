using System.Collections;
using System.Collections.Generic;
using Scryber.Expressive.Expressions;
using Scryber.Expressive.Helpers;

namespace Scryber.Expressive.Functions.Logical
{
    public class InFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name { get { return "In"; } }

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
            this.ValidateParameterCount(parameters, -1, 2);

            var found = false;

            var parameter = parameters[0].Evaluate(variables);

            // Goes through any values, and stop whe one is found
            for (var i = 1; i < parameters.Length; i++)
            {
                var result = parameters[i].Evaluate(variables);
                if(Compare(result, parameter, context))
                {
                    found = true;
                    break;
                }    
            }

            return found;
        }

        #endregion

        private bool Compare(object result, object compareto, Context context)
        {

            if (result is string str)
            {
                if (null == compareto)
                    return str == null;
                else
                    return str.CompareTo(compareto.ToString()) == 0;
            }
            else if (result is IEnumerable resultEnum)
            {
                foreach (var item in resultEnum)
                {
                    if (Compare(item, compareto, context))
                        return true;
                }
            }
            if (Comparison.CompareUsingMostPreciseType(compareto, result, context) == 0)
            {
                return true;
            }
            else
                return false;
        }
    }
}
