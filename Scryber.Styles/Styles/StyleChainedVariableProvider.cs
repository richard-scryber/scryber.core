using System;
using Scryber.Expressive;


namespace Scryber.Styles
{
    /// <summary>
    /// Implements the Expressive IVariableProvider to look in the StyleVariableSet, or if not found, the next IVariableProvider
    /// </summary>
    public class ChainedStyleVariableProvider : IVariableProvider
    {

        private IVariableProvider _next;
        private StyleVariableSet _variables;

        /// <summary>
        /// The next variable provider to fall back to
        /// </summary>
        public IVariableProvider NextProvider
        {
            get { return _next; }
        }

        /// <summary>
        /// The current set of variables
        /// </summary>
        public StyleVariableSet StyleVariables
        {
            get { return _variables; }
        }

        public ChainedStyleVariableProvider(StyleVariableSet vars, IVariableProvider next)
        {
            this._variables = vars;
            this._next = next;
        }

        /// <summary>
        /// Implementation of the IVariableProvider interface
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(string variableName, out object value)
        {
            StyleVariable styleVar;

            if (null != this._variables && this._variables.TryGetValue(variableName, out styleVar))
            {
                value = styleVar.Value;
                return true;
            }
            else if (null != this._next)
            {
                return this._next.TryGetValue(variableName, out value);
            }
            else
            {
                value = null;
                return false;
            }
        }
    }
}
