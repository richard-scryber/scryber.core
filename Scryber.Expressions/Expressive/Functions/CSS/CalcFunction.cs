using System;
using System.Collections.Generic;
using Scryber.Expressive.Expressions;

namespace Scryber.Expressive.Functions.CSS
{
    public class CalcFunction : FunctionBase
    {

        public override string Name { get { return "Calc"; } }

        public CalcFunction()
        {
        }

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
            this.ValidateParameterCount(parameters, 1, 1);
            object value = parameters[0].Evaluate(variables);
            return value;
        }
    }
}
