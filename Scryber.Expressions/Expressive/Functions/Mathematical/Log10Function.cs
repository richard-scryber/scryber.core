using Scryber.Expressive.Expressions;
using System;

namespace Scryber.Expressive.Functions.Mathematical
{
    internal class Log10Function : FunctionBase
    {
        #region FunctionBase Members

        public override string Name { get { return "Log10"; } }

        public override object Evaluate(IExpression[] parameters, Context context)
        {
            this.ValidateParameterCount(parameters, 1, 1);

            return Math.Log10(Convert.ToDouble(parameters[0].Evaluate(Variables)));
        }

        #endregion
    }
}
