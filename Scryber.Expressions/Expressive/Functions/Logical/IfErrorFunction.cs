using Scryber.Expressive.Expressions;
using System;
using System.Collections.Generic;

namespace Scryber.Expressive.Functions.Logical
{
    public class IfErrorFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name { get { return "IfError"; } }

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
            this.ValidateParameterCount(parameters, 2, 2);
            object result;

            try
            {
                result = parameters[0].Evaluate(variables);
            }
            catch(Exception ex)
            {
                result = parameters[1].Evaluate(variables);
            }

            return result;
        }

        #endregion
    }
}
