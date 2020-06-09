/*  Copyright 2012 PerceiveIT Limited
 *  This file is part of the Scryber library.
 *
 *  You can redistribute Scryber and/or modify 
 *  it under the terms of the GNU Lesser General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 * 
 *  Scryber is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 * 
 *  You should have received a copy of the GNU Lesser General Public License
 *  along with Scryber source code in the COPYING.txt file.  If not, see <http://www.gnu.org/licenses/>.
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
namespace Scryber.Configuration
{
    /// <summary>
    /// Static (shared) accessor class for the Scryber configuration section
    /// </summary>
    /// <remarks>PDFX supports the use of configuration for tracing and font-mappings by adding a &lt;Scryber&gt section to the config file.
    /// Register the section using 
    /// &ltSection name="Scryber" 
    ///         type="Scryber.Configuration.PDFXConfigurationSection, Scryber.Configuration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe"/&gt;
    /// In the sections element of the config file</remarks>
    [Obsolete("Use the IScryberConfigurationService interface with the services provider", true)]
    public static class ScryberConfiguration
    {
        public const string ScryberConfigGroupKey = "scryber";
        private const string ScryberTracingConfigSectionKey = ScryberConfigGroupKey + "/" + "tracing";
        private const string ScryberFontsConfigSectionKey = ScryberConfigGroupKey + "/" + "fonts";
        private const string ScryberGenerationConfigSectionKey = ScryberConfigGroupKey + "/" + "generation";
        private const string ScryberRenderOptionConfigSectionKey = ScryberConfigGroupKey + "/" + "rendering";

        internal const bool DefaultUseSystemFonts = true;
        internal const bool DefaultUseFontSubstitution = false;
        internal const string DefaultFontName = "Sans-Serif";

        //
        // Tracing
        //

        #region public static TracingSection TracingConfigSection {get;}

        private static TracingConfigurationSection _tracingSection;

        /// <summary>
        /// Gets the configuration section for the tracing element
        /// </summary>
        [Obsolete("Use the configuration service", true)]
        public static TracingConfigurationSection TracingConfigSection
        {
            get
            {
                if(null == _tracingSection)
                    _tracingSection = ConfigurationManager.GetSection(ScryberTracingConfigSectionKey) as TracingConfigurationSection;
                if (null == _tracingSection)
                    _tracingSection = new TracingConfigurationSection();
                return _tracingSection;
            }
        }

        #endregion

        #region public static TraceRecordLevel GetTraceLevel()

        /// <summary>
        /// Gets the configured trace level
        /// </summary>
        /// <returns></returns>
        [Obsolete("Use the configuration service", true)]
        public static TraceRecordLevel GetTraceLevel()
        {
            return TracingConfigSection.TraceLevel;
        }

        #endregion

        #region public static PDFTraceLog GetLog()


        /// <summary>
        /// Gets any configured trace log(s) recording at the configured level
        /// </summary>
        /// <param name="level"></param>
        /// <returns>The configured log. If no log is configured, then a new PDFTraceLog that does nothing will be returned</returns>
        [Obsolete("Use the configuration service", true)]
        public static PDFTraceLog GetLog()
        {
            return TracingConfigSection.GetLog();
        }

        #endregion

        #region public static PDFTraceLog GetLog(TraceRecordLevel level)

        /// <summary>
        /// Gets any configured trace log(s) recording at the specified level
        /// </summary>
        /// <param name="level"></param>
        /// <returns>The configured log. If no log is configured, then a new PDFTraceLog that does nothing will be returned</returns>
        [Obsolete("Use the configuration service", true)]
        public static PDFTraceLog GetLog(TraceRecordLevel level)
        {
            TracingConfigurationSection section = TracingConfigSection;
            if (null == section)
                return new Logging.DoNothingTraceLog(level);
            else
                return section.GetLog(level);
        }

        #endregion

        //
        // Fonts
        //

        #region private static FontMappingSection FontConfigSection {get;}

        /// <summary>
        /// Gets the FontMappingSection from the configuration file
        /// </summary>
        [Obsolete("Use the configuration service", true)]
        private static FontsConfigurationSection FontConfigSection
        {
            get
            {
                return ConfigurationManager.GetSection(ScryberFontsConfigSectionKey) as FontsConfigurationSection;
            }
        }

        #endregion

        #region public static FontMappingCollection GetExplictFontMappings()

        /// <summary>
        /// Gets the collection of fonts explicitly declared in the configuration file
        /// </summary>
        /// <returns></returns>
        [Obsolete("Use the configuration service", true)]
        public static FontMappingCollection GetExplictFontMappings()
        {
            FontsConfigurationSection config = FontConfigSection;
            if (null == config)
                return null; 
            else
                return config.FontNames;
        }

        #endregion

        #region internal static string GetFontDefaultDirectory()

        /// <summary>
        /// Returns the default directory to scan for fonts that can be used in PDF files
        /// </summary>
        /// <returns></returns>
        [Obsolete("Use the configuration service", true)]
        public static string GetFontDefaultDirectory()
        {
            FontsConfigurationSection config = FontConfigSection;
            if (null == config)
                return string.Empty;
            else
                return config.DefaultDirectory;
        }

        #endregion

        #region public static bool UseSubstituteFonts()

        /// <summary>
        /// Returns true if a substitute font should be used when the actual
        /// font cannot be found - same family no style, or if still not found - Courier
        /// </summary>
        /// <returns></returns>
        [Obsolete("Use the configuration service", true)]
        public static bool UseSubstituteFonts()
        {
            FontsConfigurationSection config = FontConfigSection;
            if (null == config)
                return DefaultUseFontSubstitution; 
            else
                return config.UseFontSubstitution;
        }

        #endregion

        #region internal static bool UseSystemFonts()



        /// <summary>
        /// Returns true if the local system fonts can be used in PDF's. False if only explicit fonts can be used
        /// </summary>
        /// <returns></returns>
        [Obsolete("Use the configuration service", true)]
        public static bool UseSystemFonts()
        {
            FontsConfigurationSection config = FontConfigSection;
            if (null == config)
                return DefaultUseSystemFonts; 
            else
                return config.UseSystemFonts;
        }

        #endregion

        #region public static string GetDefaultFont()

        /// <summary>
        /// Returns the default font to use in rendering documents
        /// </summary>
        /// <returns></returns>
        [Obsolete("Use the configuration service", true)]
        public static string GetDefaultFont()
        {
            FontsConfigurationSection config = FontConfigSection;
            if (null == config)
                return DefaultFontName;
            else
                return config.DefaultFontName;
        }

        #endregion

        //
        // Rendering
        //

        #region public static RenderOptionsConfigurationSection RenderOptions {get;}

        /// <summary>
        /// Gets the rendering options section from the configuration
        /// </summary>
        [Obsolete("Use the configuration service", true)]
        public static RenderOptionsConfigurationSection RenderOptions
        {
            get
            {
                RenderOptionsConfigurationSection config = ConfigurationManager.GetSection(ScryberRenderOptionConfigSectionKey) as RenderOptionsConfigurationSection;
                if (null == config)
                    config = new RenderOptionsConfigurationSection();
                return config;
            }
        }

        #endregion

        //
        // Generator Extensions
        //

        #region private static GenerationConfigurationSection GeneratorSection {get;}


        /// <summary>
        /// Gets the GenerationConfigurationSection.
        /// </summary>
        [Obsolete("Use the configuration service", true)]
        private static GenerationConfigurationSection GeneratorSection
        {
            get
            {
                GenerationConfigurationSection genSection = ConfigurationManager.GetSection(ScryberGenerationConfigSectionKey) as GenerationConfigurationSection;
                if (null == genSection)
                    genSection = GenerationConfigurationSection.Default;

                return genSection;
            }
        }

        #endregion

        #region public static ParserReferenceMissingAction ParserMissingReferenceAction

        /// <summary>
        /// Gets the action a parser should take if a referenced file or resource cannot be found.
        /// </summary>
        [Obsolete("Use the configuration service", true)]
        public static ParserReferenceMissingAction ParserMissingReferenceAction
        {
            get
            {
                GenerationConfigurationSection section = GeneratorSection;
                return section.MissingReferenceAction;
            }
        }

        #endregion

        #region public static string GetAssemblyNamespaceForXmlNamesapce(string xmlNamesapce)

        /// <summary>
        /// Gets the configured runtime Assembly and Namespace definition for a specific XML Namespace. Or null (Nothing) if it is not defined.
        /// </summary>
        /// <param name="xmlNamesapce"></param>
        /// <returns></returns>
        [Obsolete("Use the configuration service", true)]
        public static string GetAssemblyNamespaceForXmlNamesapce(string xmlNamesapce)
        {
            SchemaMappingElement extension;
            GenerationConfigurationSection section = GeneratorSection;
            if (null == section)
                return null;
            else
            {
                SchemaMappingCollection col = section.Mappings;
                if (null == col)
                    return null;
                else if (col.TryGetMapping(xmlNamesapce, out extension))
                {
                    return extension.RuntimeNamespace + ", " + extension.RuntimeAssembly;
                }
            }
            return null;

        }

        #endregion

        #region public static string GetXmlNamespaceForAssemblyNamespace(string fullAssemblyNameSpace)

        /// <summary>
        /// Returns any xml namespace declared in the config, for a specific namespace and assembly combination
        /// </summary>
        /// <param name="fullAssemblyNameSpace">Should be in the format 'NAME.SPACE, ASSEMBLY.NAME [Optional Full Assembly Name]' - case sensitive</param>
        /// <returns>The xml namespace or null if not found.</returns>
        [Obsolete("Use the configuration service", true)]
        public static string GetXmlNamespaceForAssemblyNamespace(string fullAssemblyNameSpace)
        {
            if (string.IsNullOrEmpty(fullAssemblyNameSpace))
                throw new ArgumentNullException("fullAssemblyNameSpace");
            int nsIndex = fullAssemblyNameSpace.IndexOf(',');
            if (nsIndex < 0)
                throw new ArgumentOutOfRangeException("fullAssemblyNameSpace", "The namespace and assembly name could not be extracted from the full assembly namespace. Should be in the format 'NAME.SPACE, ASSEMBLY.NAME [Optional Full Assembly Name]'");
            string ns = fullAssemblyNameSpace.Substring(0, nsIndex).Trim();
            string assm = fullAssemblyNameSpace.Substring(nsIndex + 1).Trim();

            return GetXmlNamespaceForAssemblyNamespace(ns, assm);
        }

        /// <summary>
        /// Returns any xml namespace declared in the config, for a specific namespace and assembly combination
        /// </summary>
        /// <param name="ns">The namespace - case sensitive </param>
        /// <param name="assembly">The assembly (as per config)</param>
        /// <returns>The xml namespace or null if not found.</returns>
        [Obsolete("Use the configuration service", true)]
        public static string GetXmlNamespaceForAssemblyNamespace(string ns, string assembly)
        {
            GenerationConfigurationSection section = GeneratorSection;
            if (null == section || null == section.Mappings || section.Mappings.Count == 0)
                return null;
            foreach (SchemaMappingElement mapping in GeneratorSection.Mappings)
            {
                if (mapping.RuntimeNamespace.Equals(ns) && mapping.RuntimeAssembly.Equals(assembly))
                    return mapping.XmlNamespace;
            }

            //not found
            return null;
        }

        #endregion

        #region public static BindingFactoryElementCollection GetExplicitBindingFactories()

        /// <summary>
        /// Returns all the configured Binding Factory elements
        /// </summary>
        /// <returns></returns>
        [Obsolete("Use the configuration service", true)]
        public static BindingFactoryElementCollection GetExplicitBindingFactories()
        {
            GenerationConfigurationSection gen = GeneratorSection;
            if (null != gen)
            {
                BindingFactoryElementCollection elements = gen.ExpressionBinders;
                return elements;
            }
            return null;
        }

        #endregion

        #region public static bool TryGetBinder(string prefix, out IPDFBindingExpressionFactory binder)

        /// <summary>
        /// Attempts to retrieve a binding expression factory for a predefined prefix. Returns an instance of the binder and true if found.
        /// Otherwise null and false;
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="binder"></param>
        /// <returns></returns>
        [Obsolete("Use the configuration service", true)]
        public static bool TryGetBinder(string prefix, out IPDFBindingExpressionFactory binder)
        {
            GenerationConfigurationSection section = GeneratorSection;
            if (null == section)
            {
                binder = null;
                return false;
            }

            BindingFactoryElementCollection col = section.ExpressionBinders;
            BindingFactoryElement ele = null;
            if (col == null || col.Count == 0)
            {
                binder = null;
                return false;
            }
            else if (col.TryGetBindingElement(prefix, out ele))
            {
                binder = ele.GetFactory();
                return null != binder;
            }
            else
            {
                binder = null;
                return false;
            }
        }

        #endregion
    }
}
