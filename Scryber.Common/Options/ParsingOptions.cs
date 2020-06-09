using System;
namespace Scryber.Options
{


    public class ParsingOptions
    {
        public const string ParsingSection = ScryberOptions.ScryberSectionStub + "Parsing";


        public ParserReferenceMissingAction MissingReferenceAction { get; set; }

        public string DefaultCulture { get; set; }

        public NamespaceMappingOption[] Namespaces { get; set; }

        public BindingPrefixOption[] Bindings { get; set; }

        public ParsingOptions()
        {
            MissingReferenceAction = ParserReferenceMissingAction.RaiseException;
        }

        private System.Globalization.CultureInfo _defaultCulture;

        public System.Globalization.CultureInfo GetDefaultCulture()
        {
            if (null == _defaultCulture)
            {
                if (!string.IsNullOrEmpty(this.DefaultCulture))
                {
                    _defaultCulture = System.Globalization.CultureInfo.GetCultureInfo(this.DefaultCulture);
                }
            }
            return _defaultCulture;
        }

        public string GetXmlNamespaceForAssemblyNamespace(string assemblyNamespace)
        {
            if (string.IsNullOrEmpty(assemblyNamespace) || this.Namespaces == null || this.Namespaces.Length == 0)
                return string.Empty;

            int index = assemblyNamespace.IndexOf(",");
            if (index < 0)
                return assemblyNamespace;
            else
            {
                var ns = assemblyNamespace.Substring(0, index).Trim();
                var assm = assemblyNamespace.Substring(index + 1).Trim();

                for(var i = 0; i < this.Namespaces.Length; i++)
                {
                    if (string.Equals(this.Namespaces[i].Namespace, ns) && string.Equals(this.Namespaces[i].Assembly, assm))
                        return this.Namespaces[i].Source;
                }
                //Not found
                return string.Empty;

            }

        }


    }

    /// <summary>
    /// Maps an XML Namespace declaration with a specific assembly and namespace
    /// e.g xmlns:pdf=""
    /// </summary>
    public class NamespaceMappingOption
    {

        /// <summary>
        /// Gets or sets the namespace being referred to.
        /// e.g. Scryber.Components
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// Gets or sets the full assembly name (inc. version and public key for the assembly
        /// e.g. Scryber.Components, Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe
        /// </summary>
        public string Assembly { get; set; }

        /// <summary>
        /// Gets or sets the full mapping value that refers to the specified namespace and assembly
        /// e.g. https://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd
        /// </summary>
        public string Source { get; set; }


        public NamespaceMappingOption()
        {
        }
    }

    /// <summary>
    /// Maps a binding expression in an xml file to a binding factory during the parsing of a file
    /// </summary>
    public class BindingPrefixOption
    {
        /// <summary>
        /// The prefix used for the binding expression 
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// The Type name including namespace
        /// e.g. Scryber.XPathBindingExpressionFactory
        /// </summary>
        public string FactoryType { get; set; }

        /// <summary>
        /// The full assembly name that has the type for the factory
        /// e.g. Scryber.Generation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe
        /// </summary>
        public string FactoryAssembly { get; set; }

        /// <summary>
        /// Gets the factory instance that is specified by this options FactoryType and FactoryAssembly
        /// </summary>
        /// <returns></returns>
        public IPDFBindingExpressionFactory GetFactory()
        {
            var factory = Utilities.TypeHelper.GetInstance<IPDFBindingExpressionFactory>(this.FactoryType, this.FactoryAssembly, true);
            return factory;
        }
    }
}
