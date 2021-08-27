using Scryber.Expressive.Expressions;
using Scryber.Expressive.Helpers;
using System;
using System.Collections.Generic;

namespace Scryber.Expressive.Functions.Mathematical
{
    public class PIFunction : FunctionBase
    {
        public override string Name { get { return "PI"; } }

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
            this.ValidateParameterCount(parameters, 0, 0);
            return Math.PI;
        }
    }
}
