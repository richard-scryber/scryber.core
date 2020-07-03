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
using Scryber.Binding;

namespace Scryber.Generation
{
    public static class ParserHelper
    {
        internal const string typeAttr = "type-name";
        internal const string codebehindAttr = "code-behind";
        internal const string xmlnsAttr = "xmlns";
        internal const string initMethod = "InitializeComponents";
        internal const string parseMethodName = "ParseComponentAtPath";
        internal const string idAttributeName = "id";
        internal const string idPropertyName = "ID";
        internal const string addMethodName = "Add";
        internal const string loadedSourceProperty = "LoadedSource";
        internal const string loadtypeProperty = "LoadType";
        internal const string remoteSourceTypeAttribute = "type";
        internal const string remoteSourcePathAttribute = "source";
        internal const string mapPathMethod = "MapPath";
        internal const string documentPropertyName = "Document";
        internal const string instantiateOverrideMethodName = "InitializeComponents";
        internal const string bindingContextPropertyName = "Context";
        internal const string evalXPathMethod = "EvalXPathAsString";

        internal const int defaultErrorLevel = 4;

        internal static Type[] knownObjectTypes = new Type[] { typeof(Guid), typeof(DateTime), typeof(TimeSpan), typeof(Type), typeof(System.Uri), typeof(System.Version) };
        
        //internal static Type textLiteralType = typeof(Scryber.Components.PDFTextLiteral);

        internal static char[] trimmingCharacters = new char[] { '\r', '\n', '\t', ' ' };
        internal static string[] specialRootAttributes = new string[] { xmlnsAttr, codebehindAttr, typeAttr };
        internal static string[] standardImports = new string[] { "Scryber", "Scryber.Components", "Scryber.Data" };

        public const string BindingStartChar = "{";
        public const string BindingEndChar = "}";
        public const string BindingKeySeparator = ":";
        private const string BindingStartEscape = "{{";

        private const string ItemBindingKey = "item";
        private const string QueryStringBindingKey = "qs";
        private const string XPathBindingKey = "xpath";

        private static object _configlock = new object();
        private static Dictionary<string, IPDFBindingExpressionFactory> _configFactories = null;

        #region private static Dictionary<string, IPDFBindingExpressionFactory> InitFactories()

        /// <summary>
        /// Initializes and returns a dictionary of the standard and 
        /// configured binding expression factories based on the prefix
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, IPDFBindingExpressionFactory> InitFactories()
        {
            Dictionary<string, IPDFBindingExpressionFactory> factories = new Dictionary<string, IPDFBindingExpressionFactory>();

            //let's be thread safe here just so we can be sure 
            lock (_configlock)
            {
                //add the standard factories
                factories[ItemBindingKey] = new BindingItemExpressionFactory();
                //factories[QueryStringBindingKey] = new BindingQueryStringExpressionFactory();
                factories[XPathBindingKey] = new BindingXPathExpressionFactory();

                try
                {
                    var service = ServiceProvider.GetService<IScryberConfigurationService>();
                    var config = service.ParsingOptions.Bindings;
                    if (null != config && config.Count > 0)
                    {
                        foreach (var ele in config)
                        {
                            IPDFBindingExpressionFactory factory = ele.GetFactory();
                            string key = ele.Prefix;

                            //By using the set accessor this will overwrite any of the standard factories previously added.
                            factories[key] = factory;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new System.Exception("Failed to initialize the configured binding expression factories :" + ex.Message, ex);
                }
            }

            return factories;
        }

        #endregion

        #region internal static bool IsBindingExpression(ref string value, out IPDFBindingExpressionFactory bindingfactory, PDFGeneratorSettings settings)

        /// <summary>
        /// Returns true if the value provided is an expression that matches the pattern required for a binding expression.
        /// If it does value will be modified to be just the content of that expression and the bindingFactory will be set to the 
        /// factory that handles these expression types.
        /// </summary>
        /// <param name="value">The string to check if it is a binding expression. If it does match, then it will be modifed </param>
        /// <param name="bindingfactory"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static bool IsBindingExpression(ref string value, out IPDFBindingExpressionFactory bindingfactory, PDFGeneratorSettings settings = null)
        {
            bindingfactory = null;
            if (string.IsNullOrEmpty(value) || value.Length < 4)
            {
                return false;
            }
            else if (!(value.StartsWith(BindingStartChar) && value.EndsWith(BindingEndChar)))
            {
                return false;
            }
            else if (value.StartsWith(BindingStartEscape))
            {
                value = BindingStartChar + value.Substring(BindingStartEscape.Length);
                return false;
            }
            else
            {
                int separatorIndex = value.IndexOf(BindingKeySeparator);
                if (separatorIndex < 0)
                    return false;

                string factoryKey = value.Substring(BindingStartChar.Length, separatorIndex - BindingStartChar.Length);

                //ensure the factories are initialized (and will raise an appropriate exception that can be handled if not.
                //doesn't need to be thread safe as it will simply reset the collection.
                if (null == _configFactories)
                    _configFactories = InitFactories();

                if (_configFactories.TryGetValue(factoryKey, out bindingfactory))
                {
                    int offset = BindingStartChar.Length + factoryKey.Length + BindingKeySeparator.Length;
                    int len = value.Length - (offset + 1);
                    value = value.Substring(offset, len);
                    return true;
                }
                else if (null != settings)
                {
                    settings.TraceLog.Add(TraceLevel.Warning, "XML Parser", string.Format(Errors.BindingPrefixIsNotKnown, factoryKey, value));
                    return false;
                }
                else
                    return false;
                
            }
        }

        #endregion

        #region internal static bool IsSimpleType(ParserPropertyDefinition prop)

        /// <summary>
        /// Returns true if the type is a known simple type (e.g. Int, DateTime, string etc). 
        /// Rather than a complex multivalued type.
        /// </summary>
        /// <param name="prop">The property definition to check the type of</param>
        /// <returns>True if the type is simple (TypeCode != Empty or Object, or is an enum, or is one of the known simple types</returns>
        internal static bool IsSimpleType(ParserPropertyDefinition prop)
        {
            TypeCode tc = Type.GetTypeCode(prop.ValueType);

            if (tc != TypeCode.Empty && tc != TypeCode.Object)
                return true;
            else if (prop.ValueType.IsEnum)
                return true;
            else if (Array.IndexOf<Type>(ParserHelper.knownObjectTypes, prop.ValueType) >= 0)
                return true;
            else
                return false;
        }

        #endregion

        #region private string GetSafeTypeName(string typename)

        /// <summary>
        /// Converts a string into a safe type name by replacing any non valid characters with an underscore
        /// </summary>
        /// <param name="typename"></param>
        /// <returns></returns>
        internal static string GetSafeTypeName(string typename)
        {
            StringBuilder sb = new StringBuilder();
            bool isfirst = true;
            foreach (char c in typename)
            {
                if (char.IsLetter(c))
                {
                    sb.Append(c);
                }
                else if (char.IsDigit(c))
                {
                    if (isfirst)
                        sb.Append("_");
                    else
                        sb.Append(c);
                }
                else
                    sb.Append('_');

                isfirst = false;
            }
            return sb.ToString();
        }

        #endregion

    }
}
