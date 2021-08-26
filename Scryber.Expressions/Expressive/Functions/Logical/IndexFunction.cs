using Scryber.Expressive.Expressions;
using Scryber.Expressive.Helpers;
using System;


namespace Scryber.Expressive.Functions.Relational
{
    public class IndexFunction : FunctionBase
    {

        public const string CurrentIndexVariableName = "[CurrentIndex]";

        public override string Name { get { return "Index"; } }

        public override object Evaluate(IExpression[] parameters, Context context)
        {
            this.ValidateParameterCount(parameters, 0, 0);

            object value;
            if (this.Variables.TryGetValue(CurrentIndexVariableName, out value))
                return value;
            else
                return -1;
        }
    }
}
