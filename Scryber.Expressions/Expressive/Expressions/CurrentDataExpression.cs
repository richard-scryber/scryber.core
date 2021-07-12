using System;
using System.Collections.Generic;

namespace Scryber.Expressive.Expressions
{
    public class CurrentDataExpression : IExpression
    {
        private Context context;

        public CurrentDataExpression(Context context)
        {
            this.context = context;
        }

       
        #region IExpression Members

        /// <inheritdoc />
        public object Evaluate(IDictionary<string, object> variables)
        {
            return context.CurrentDataContext;
        }

        #endregion
    }
}
