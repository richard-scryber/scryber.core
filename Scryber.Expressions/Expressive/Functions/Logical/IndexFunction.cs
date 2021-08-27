using Scryber.Expressive.Expressions;
using Scryber.Expressive.Helpers;
using System;
using System.Collections.Generic;

namespace Scryber.Expressive.Functions.Relational
{
    public class IndexFunction : FunctionBase
    {

        public const string CurrentIndexVariableName = "[CurrentIndex]";

        public override string Name { get { return "Index"; } }

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
            this.ValidateParameterCount(parameters, 0, 0);

            object value;
            if (variables.TryGetValue(CurrentIndexVariableName, out value))
                return value;
            else
                return -1;
        }
    }
}
