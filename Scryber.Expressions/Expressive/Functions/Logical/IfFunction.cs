using Scryber.Expressive.Expressions;
using System;

namespace Scryber.Expressive.Functions.Logical
{
    internal class IfFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name { get { return "If"; } }

        public override object Evaluate(IExpression[] parameters, Context context)
        {
            this.ValidateParameterCount(parameters, 3, 3);

            bool condition = Convert.ToBoolean(parameters[0].Evaluate(Variables));

            return condition ? parameters[1].Evaluate(Variables) : parameters[2].Evaluate(Variables);
        }

        #endregion
    }
}
