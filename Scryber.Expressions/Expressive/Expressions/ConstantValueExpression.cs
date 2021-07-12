using System.Collections.Generic;

namespace Scryber.Expressive.Expressions
{
    internal class ConstantValueExpression : IExpression
    {
        private readonly object value;

        internal ConstantValueExpression(object value)
        {
            this.value = value;
        }

        #region IExpression Members

        public object Evaluate(IDictionary<string, object> variables)
        {
            return this.value;
        }

        #endregion
    }
}
