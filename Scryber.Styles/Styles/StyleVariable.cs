using System;
using System.Collections.Generic;

namespace Scryber.Styles
{
    
    public class StyleVariable
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public StyleVariable()
        {
        }
    }

    public class StyleVariableSet : Dictionary<string, StyleVariable>
    {
        
    }
}
