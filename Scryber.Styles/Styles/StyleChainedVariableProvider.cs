using System;
using Scryber.Drawing;
using Scryber.Expressive;


namespace Scryber.Styles
{
    /// <summary>
    /// Implements the Expressive IVariableProvider to look in the StyleVariableSet, or if not found, the next IVariableProvider
    /// </summary>
    public class StyleChainedVariableProvider : IVariableProvider
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

        /// <summary>
        /// Creates a new Chained variable provider
        /// </summary>
        /// <param name="vars"></param>
        /// <param name="next"></param>
        public StyleChainedVariableProvider(StyleVariableSet vars, IVariableProvider next)
        {
            this._variables = vars ?? throw new ArgumentNullException(nameof(vars));
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
            object nextVal;
            bool result = false;
            value = null;

            //First check the style variables
            if (null != this.StyleVariables && this.StyleVariables.TryGetValue(variableName, out styleVar))
            {
                value = styleVar.Value;
                result = true;
            }

            //Next we check any previous values
            if (null != this._next && this.NextProvider.TryGetValue(variableName, out nextVal))
            {
                value = nextVal;
                result = true;
            }

            return result;
            
        }

        public void AddRelativeDimensions(Size page, Size container, Size font, Unit rootFont, bool useWidth)
        {
            if (null != this.NextProvider)
                this.NextProvider.AddRelativeDimensions(page, container, font, rootFont, useWidth);
        }

        public void AddRelativeCallback(RelativeToAbsoluteDimensionCallback callback)
        {
            if(null != this.NextProvider)
                this.NextProvider.AddRelativeCallback(callback);
        }
    }
}
