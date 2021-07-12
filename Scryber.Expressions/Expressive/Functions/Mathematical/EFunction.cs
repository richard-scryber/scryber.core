using Scryber.Expressive.Expressions;
using Scryber.Expressive.Helpers;
using System;


namespace Scryber.Expressive.Functions.Mathematical
{
    internal class EFunction : FunctionBase
    {
        public override string Name { get { return "E"; } }

        public override object Evaluate(IExpression[] parameters, Context context)
        {
            this.ValidateParameterCount(parameters, 0, 0);
            return Math.E;
        }
    }
}