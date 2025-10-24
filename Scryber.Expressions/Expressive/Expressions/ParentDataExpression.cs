using System;
using System.Collections.Generic;

namespace Scryber.Expressive.Expressions
{
    public class ParentDataExpression : IExpression
    {

        public const string ParentDataVariableName = "[_ParentData_]";

        private Context context;

        public ParentDataExpression(Context context)
        {
            this.context = context;
        }

       
        #region IExpression Members

        /// <inheritdoc />
        public object Evaluate(IDictionary<string, object> variables)
        {
            object value;
            if (variables.TryGetValue(ParentDataVariableName, out value))
                return value;
            else
                return null;
        }

        #endregion

        public static void SetCurrentData(object value, IDictionary<string, object> variables)
        {
            variables[ParentDataVariableName] = value;
        }

        public static object GetCurrentData(IDictionary<string, object> variables)
        {
            object value = null;
            if (variables != null && variables.TryGetValue(ParentDataVariableName, out value))
                return value;
            else
                return null;
        }
    }
}
