using System;
using Scryber.Expressive.Expressions;

namespace Scryber.Expressive.Functions.CSS
{
    public class VarFunction : FunctionBase
    {

        public override string Name
        {
            get { return "Var"; }
        }

        public VarFunction()
        {
        }

        public override object Evaluate(IExpression[] parameters, Context context)
        {
            this.ValidateParameterCount(parameters, -1, 1);
            object value = parameters[0].Evaluate(this.Variables);

            //Allow the pass through of an optional second parameter with a fallback value

            if (null == value && parameters.Length > 1)
                value = parameters[1].Evaluate(this.Variables);

            return value;
        }
    }
}
