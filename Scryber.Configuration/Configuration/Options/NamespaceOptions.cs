using System;
namespace Scryber.Configuration.Options
{
    public class NamespaceOptions
    {
        /// <summary>
        /// Gets or sets the namespace being referred to.
        /// e.g. Scryber.Components
        /// </summary>
        public string NamespaceName { get; set; }

        /// <summary>
        /// Gets or sets the full assembly name (inc. version and public key for the assembly
        /// e.g. Scryber.Components, Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe
        /// </summary>
        public string NamespaceAssembly { get; set; }

        /// <summary>
        /// Gets or sets the full mapping value that refers to the specified namespace and assembly
        /// e.g. https://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd
        /// </summary>
        public string NamespaceMapping { get; set; }


        public NamespaceOptions()
        {
        }
    }
}
