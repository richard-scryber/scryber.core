#define SEARCH_CURRENT_OBJECT_PROPERTIES

using System;
using System.Collections.Generic;

namespace Scryber.Expressive.Expressions
{
    internal class VariableExpression : IExpression
    {
        public readonly string variableName;
        public readonly bool isCaseInSensitive;

        internal VariableExpression(string variableName, bool caseInSensitive)
        {
            this.variableName = variableName;
            this.isCaseInSensitive = caseInSensitive;
        }

        #region IExpression Members

        /// <inheritdoc />
        public object Evaluate(IDictionary<string, object> variables)
        {
            if (variables is null)
                return null;
            
            if(!variables.TryGetValue(this.variableName, out var variableValue))
            {
                #if SEARCH_CURRENT_OBJECT_PROPERTIES

                if (variables.TryGetValue(CurrentDataExpression.CurrentDataVariableName, out var curr))
                {
                    variableValue =
                        PropertyExpression.GetPropertyValue(curr, this.variableName, this.isCaseInSensitive);
                    return variableValue;

                }
                
                #endif
                
                //not found
                return null;
            }

            // Check to see if we have to referred to another expression.
            if (variableValue is IExpression expression)
            {
                return expression.Evaluate(variables);
            }

            return variableValue;
        }

        #endregion
    }
    
    internal class SelfVariableExpression : IExpression
    {
        public object Evaluate(IDictionary<string, object> variables)
        {
            var result = variables[CurrentDataExpression.CurrentDataVariableName];
            return result;
        }
    }
}
