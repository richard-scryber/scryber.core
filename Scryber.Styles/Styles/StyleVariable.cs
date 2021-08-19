using System;
using System.Collections.Generic;

namespace Scryber.Styles
{

    /// <summary>
    /// A simple wrapper for a variable in a css style with a name and a value
    /// </summary>
    public class StyleVariable
    {
        /// <summary>
        /// Gets the css name (usually prefixed with --).
        /// </summary>
        public string CssName { get; private set; }

        private string _normal;

        /// <summary>
        /// Gets the normalized name (with any -- prefix removed)
        /// </summary>
        public string NormalizedName
        {
            get
            {
                if(null == _normal)
                {
                    if (this.CssName.StartsWith("--"))
                        _normal = this.CssName.Substring(2);
                    else
                        _normal = this.CssName;
                }
                return _normal;
            }
        }

        /// <summary>
        /// Gets or sets the value of the variable
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Creates a new css style variable with the specified name and value 
        /// </summary>
        /// <param name="cssName">The immutable name of the variable</param>
        /// <param name="value"></param>
        public StyleVariable(string cssName, string value)
        {
            this.CssName = cssName ?? throw new ArgumentNullException(nameof(cssName));
            this.Value = value;
        }
    }


    public class StyleVariableSet : Dictionary<string, StyleVariable>
    {
        public void Add(StyleVariable variable)
        {
            this.Add(variable.CssName, variable);
        }
    }
}
