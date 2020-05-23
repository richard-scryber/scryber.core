using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scryber.Generation
{
    /// <summary>
    /// Represents the Outlets (properties and variables) and Actions (event handlers) on a specified controller class
    /// </summary>
    public class ParserControllerDefinition
    {
        public ParserControllerOutletList Outlets { get; private set; }

        public ParserControllerActionList Actions { get; private set; }

        public Type ControllerType { get; private set; }

        public string ControllerTypeName { get; private set; }


        public ParserControllerDefinition(string name, Type type)
        {
            this.ControllerType = type;
            this.ControllerTypeName = name;

            this.Outlets = new ParserControllerOutletList();
            this.Actions = new ParserControllerActionList();
        }

        

    }
}
