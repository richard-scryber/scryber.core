using System;
using System.Collections.Generic;

namespace Scryber.Expressive.Expressions
{
    internal class FunctionExpression : IExpression
    {
        private readonly ExecFunction function;
        private readonly string name;
        private readonly IExpression[] parameters;
        private readonly ExpressionContext context;

        internal FunctionExpression(string name, ExecFunction function, ExpressionContext context, IExpression[] parameters)
        {
            this.name = name;
            this.function = function;
            this.parameters = parameters;
            this.context = context;
        }

        #region IExpression Members

        public object Evaluate(IDictionary<string, object> variables)
        {
            return this.function(this.parameters, variables, this.context);
        }

        #endregion
    }
}
