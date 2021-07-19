using Scryber.Expressive.Expressions;
using Scryber.Expressive.Helpers;
using System;


namespace Scryber.Expressive.Functions.Relational
{
    public class IndexFunction : FunctionBase
    {
        public override string Name { get { return "Index"; } }

        public override object Evaluate(IExpression[] parameters, Context context)
        {
            this.ValidateParameterCount(parameters, 0, 0);
            return context.CurrentDataIndex < 0 ? null : context.CurrentDataIndex;
        }
    }
}
