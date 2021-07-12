using System;
using System.Collections.Generic;

namespace Scryber.Expressive.Expressions
{
    internal class FunctionExpression : IExpression
    {
        private readonly Func<IExpression[], IDictionary<string, object>, object> function;
        private readonly string name;
        private readonly IExpression[] parameters;

        internal FunctionExpression(string name, Func<IExpression[], IDictionary<string, object>, object> function, IExpression[] parameters)
        {
            this.name = name;
            this.function = function;
            this.parameters = parameters;
        }

        #region IExpression Members

        public object Evaluate(IDictionary<string, object> variables)
        {
            return this.function(this.parameters, variables);
        }

        #endregion
    }
}
