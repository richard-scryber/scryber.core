using System;
using System.Collections.Generic;

namespace Scryber.Options
{


    public class ParsingOptions
    {
        public const string ParsingSection = ScryberOptions.ScryberSectionStub + "Parsing";

        public const string ItemBindingPrefix = "item";
        public const string CalcBindingPrefix = "calc";
        public const string XPathBindingPrefix = "xpath";

        public ParserReferenceMissingAction MissingReferenceAction { get; set; }

        public string DefaultCulture { get; set; }

        public List<NamespaceMappingOption> Namespaces { get; private set; }

        public List<BindingPrefixOption> Bindings { get; set; }


        public Dictionary<MimeType, IParserFactory> Parsers { get; set; }
        

        public ParsingOptions()
        {
            MissingReferenceAction = ParserReferenceMissingAction.RaiseException;
            Namespaces = new List<NamespaceMappingOption>();
            Namespaces.Add(new NamespaceMappingOption() { Source = "http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd",
                                                          Namespace = "Scryber.Components",
                                                          Assembly = "Scryber.Components, Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe"
            });
            Namespaces.Add(new NamespaceMappingOption(){ Source = "http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd",
                                                        Namespace = "Scryber.Data",
                                                        Assembly = "Scryber.Components, Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe"
            });
            Namespaces.Add(new NamespaceMappingOption(){ Source = "http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd",
                                                        Namespace = "Scryber.Styles",
                                                        Assembly = "Scryber.Styles, Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe"
            });
            Namespaces.Add(new NamespaceMappingOption(){ Source = "http://www.w3.org/1999/xhtml",
                                                        Namespace = "Scryber.Html.Components",
                                                        Assembly = "Scryber.Components, Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe"
            });
            Namespaces.Add(new NamespaceMappingOption() { Source= "http://www.w3.org/2000/svg",
                                                        Namespace = "Scryber.Svg.Components",
                                                        Assembly = "Scryber.Components, Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe"
            });

            Bindings = new List<BindingPrefixOption>();
            Bindings.Add(new BindingPrefixOption() { Prefix = ItemBindingPrefix,
                                                     FactoryType = "Scryber.Binding.BindingItemExpressionFactory",
                                                     FactoryAssembly = "Scryber.Generation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe"
            });
            Bindings.Add(new BindingPrefixOption() { Prefix = "@",
                                                     FactoryType = "Scryber.Binding.BindingItemExpressionFactory",
                                                     FactoryAssembly = "Scryber.Generation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe"
            });
            Bindings.Add(new BindingPrefixOption() { Prefix = CalcBindingPrefix,
                                                     FactoryType = "Scryber.Binding.BindingCalcExpressionFactory",
                                                     FactoryAssembly = "Scryber.Generation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe"
            });
            Bindings.Add(new BindingPrefixOption(){ Prefix = XPathBindingPrefix,
                                                    FactoryType = "Scryber.Binding.BindingXPathExpressionFactory",
                                                    FactoryAssembly = "Scryber.Generation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe"
            });

            Parsers = new Dictionary<MimeType, IParserFactory>();
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
            if (string.IsNullOrEmpty(assemblyNamespace) || this.Namespaces == null || this.Namespaces.Count == 0)
                return string.Empty;

            int index = assemblyNamespace.IndexOf(",");
            if (index < 0)
                return assemblyNamespace;
            else
            {
                var ns = assemblyNamespace.Substring(0, index).Trim();
                var assm = assemblyNamespace.Substring(index + 1).Trim();
                return GetXmlNamespaceForAssemblyNamespace(ns, assm);
            }

        }

        /// <summary>
        /// Return the associated XML namespace
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="assm"></param>
        /// <returns></returns>
        public string GetXmlNamespaceForAssemblyNamespace(string ns, string assm)
        {
            if (string.IsNullOrEmpty(assm) || this.Namespaces == null || this.Namespaces.Count == 0)
                return string.Empty;

            for (var i = 0; i < this.Namespaces.Count; i++)
            {
                if (string.Equals(this.Namespaces[i].Namespace, ns) && string.Equals(this.Namespaces[i].Assembly, assm))
                    return this.Namespaces[i].Source;
            }
            //Not found
            return string.Empty;
        }


        public IPDFBindingExpressionFactory GetBindingFactoryForPrefix(string prefix)
        {
            if (string.IsNullOrEmpty(prefix) || this.Bindings == null || this.Bindings.Count == 0)
                return null;

            for(var i = 0; i < this.Bindings.Count; i++)
            {
                if (string.Equals(this.Bindings[i].Prefix, prefix))
                    return this.Bindings[i].GetFactory();
            }

            //Not Found
            return null;
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
        /// We store a local cached version of the factory.
        /// </summary>
        private IPDFBindingExpressionFactory _factory;

        /// <summary>
        /// Gets the factory instance that is specified by this options FactoryType and FactoryAssembly
        /// </summary>
        /// <returns></returns>
        public IPDFBindingExpressionFactory GetFactory()
        {
            if(null == _factory)
                _factory = Utilities.TypeHelper.GetInstance<IPDFBindingExpressionFactory>(this.FactoryType, this.FactoryAssembly, true);

            return _factory;
        }
    }
}
