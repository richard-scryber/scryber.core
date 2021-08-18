using System;
using Scryber.Expressive.Expressions;

namespace Scryber.Expressive.Functions.CSS
{
    public class CalcFunction : FunctionBase
    {

        public override string Name { get { return "Calc"; } }

        public CalcFunction()
        {
        }

        public override object Evaluate(IExpression[] parameters, Context context)
        {
            this.ValidateParameterCount(parameters, 1, 1);
            object value = parameters[0].Evaluate(this.Variables);
            return value;
        }
    }
}
