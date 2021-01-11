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
using System.Reflection;

namespace Scryber.Generation
{
    public static class ParserDefintionFactory
    {

        //
        // const
        //

        private const char NamespaceSeparator = ',';

        //unqualified hooks
        //so that these elements do no have to qualify the namespace
        //just a bit of a hack to ease notation
        private const string UnqualifiedBold = "B";
        private const string UnqualifiedItalic = "I";
        private const string UnqualifiedLineBreak = "BR";
        private const string UnqualifiedAssembly = "Scryber.Components, Scryber.Components, Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe";


        //
        // inner classes
        //

        #region private class ApplicationDefn : Dictionary<string,AssemblyDefn>

        /// <summary>
        /// A dictionary of all the looked up assemblies in this application
        /// </summary>
        private class ApplicationDefn : Dictionary<string,AssemblyDefn>
        {
        }

        #endregion

        #region private class AssemblyDefn : Dictionary<string, NamespaceDefn>

        /// <summary>
        /// An Assembly with a dictionary of all the looked up namespaces
        /// </summary>
        private class AssemblyDefn : Dictionary<string, NamespaceDefn>
        {
            private System.Reflection.Assembly _assm;

            internal System.Reflection.Assembly InnerAssembly
            {
                get { return _assm; }
                set { _assm = value; }
            }
        }

        #endregion

        #region private class NamespaceDefn : Dictionary<string, Type>

        /// <summary>
        /// A namespace with a dictionary of all the declared types in that namespace with the PDFParserComponent attribute
        /// </summary>
        private class NamespaceDefn : Dictionary<string, Type>
        {
            private Dictionary<string,string> _remotes;

            /// <summary>
            /// Gets a dictionary of the remote types defined in the namespace
            /// mapping the remote type name to the concrete type name
            /// </summary>
            public Dictionary<string,string> RemoteTypes
            {
                get
                {
                    if (null == _remotes)
                        _remotes = new Dictionary<string,string>();
                    return _remotes;
                }
            }
        }

        #endregion

        #region private class ControllerDefn : Dictionary<string, ParserControllerDefinition>

        /// <summary>
        /// A dictionary of type name and their associated controller definitions
        /// </summary>
        private class ControllerDefn : Dictionary<string, ParserControllerDefinition>
        {
        }

        #endregion

        //
        // class variables
        //

        private static ApplicationDefn _application = new ApplicationDefn();
        private static Dictionary<Type, ParserClassDefinition> _typedefinitions;
        private static Dictionary<string, string> _namespaceMappings;
        private static object _applicationlock = new object();
        private static object _typelock = new object();
        private static Dictionary<string, string> _unqualified;
        private static ControllerDefn _controllers = new ControllerDefn();
        private static object _controllerlock = new object();

        //
        // ..ctor
        //

        #region static ParserDefintionFactory()

        /// <summary>
        /// static constructor
        /// </summary>
        static ParserDefintionFactory()
        {
            _unqualified = InitAllowedUnqualifiedEntries();
            _namespaceMappings = InitNamespaceMappings();
            _typedefinitions = new Dictionary<Type, ParserClassDefinition>();
        }

        #endregion

        //
        // internal interface
        //

        #region internal static ParserClassDefinition GetClassDefinition(string elementname, string assemblyQualifiedNamespace, out bool isremote)

        /// <summary>
        /// Gets the ParserClassDefintion for a specific element name in an assemblyQualifiedNamespace.
        /// </summary>
        /// <param name="parsertype"></param>
        /// <returns></returns>
        public static ParserClassDefinition GetClassDefinition(string elementname, string xmlNamespace, bool throwOnNotFound, out bool isremote)
        {
            Type found;
            lock (_applicationlock)
            {
                string assemblyQualifiedNamespace = string.Empty;

                if (string.IsNullOrEmpty(xmlNamespace) == false)
                    assemblyQualifiedNamespace = LookupAssemblyForXmlNamespace(xmlNamespace);
                else
                {
                    //If we dont have a qualified element. Check to see if it's in the list of allowes unqualified elements and if so
                    //set the namespace to that value.

                    string unqualNs;
                    if (_unqualified.TryGetValue(elementname, out unqualNs))
                        assemblyQualifiedNamespace = unqualNs;
                }

                found = UnsafeGetType(elementname, assemblyQualifiedNamespace, throwOnNotFound, out isremote);
            }

            if (null != found)
                return GetClassDefinition(found);
            else
                return null;
        }

        #endregion

        #region internal static string LookupAssemblyForXmlNamespace(string xmlNamespace)

        /// <summary>
        /// Based on the namespace provided in the xml file, looks up any defined runtime assembly in the configuration file.
        /// If not found then the xml namespace itself is returned.
        /// </summary>
        /// <param name="xmlNamespace"></param>
        /// <returns></returns>
        public static string LookupAssemblyForXmlNamespace(string xmlNamespace)
        {
            string assm;
            if (!_namespaceMappings.TryGetValue(xmlNamespace,out assm))
                assm = xmlNamespace;
            return assm;
        }

        #endregion

        #region internal static ParserClassDefinition GetClassDefinition(Type parsabletype)

        /// <summary>
        /// Gets the ParserClassDefintion for a specific type.
        /// </summary>
        /// <param name="parsertype"></param>
        /// <returns></returns>
        public static ParserClassDefinition GetClassDefinition(Type parsabletype)
        {
            if (null == parsabletype)
                throw new ArgumentNullException("parsabletype");

            ParserClassDefinition found;
            lock (_typelock)
            {
                found = UnsafeGetClassDefinitionFromType(parsabletype);
            }
            return found;
        }

        #endregion

        #region internal static bool IsSimpleObjectType(Type type, out PDFValueConverter convert)

        /// <summary>
        /// Returns true if the type is a simple convertable value from a string. 
        /// And sets the converter to the metho that will convert a string to this required type. 
        /// If not its null and false.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="convert"></param>
        /// <returns></returns>
        public static bool IsSimpleObjectType(Type type, out PDFValueConverter convert)
        {
            return ConvertObjects.IsSimpleObjectType(type, out convert);
        }

        #endregion

        #region internal static bool IsCustomParsableObjectType(Type type, out PDFValueConverter convert)

        /// <summary>
        /// Returns true if the type specified is a custom parsable type - and should have a static (shared) Parse method on the definition on it.
        /// The convert parameter will be set to a delegate that calls this method.
        /// </summary>
        /// <param name="type">The type to check for parsability</param>
        /// <param name="convert">If it is a custom parsable type then it will be set to the converter of this type</param>
        /// <returns>True if the type is decoated with a custom parsable type</returns>
        public static bool IsCustomParsableObjectType(Type type, out PDFValueConverter convert)
        {
            PDFParsableValueAttribute valattr = GetCustomAttribute<PDFParsableValueAttribute>(type, true);
            if (null != valattr)
            {
                convert = ConvertObjects.GetParsableValueConverter(type);
                return null != convert;
            }
            else
            {
                convert = null;
                return false;
            }
        }

        #endregion

        #region internal static ParserControllerDefinition GetControllerDefinition(string controllertype)

        /// <summary>
        /// Returns an new ParserControllerDefinition based on the provided type name of the required controller.
        /// Retuns null if the type cannot be found.
        /// </summary>
        /// <param name="controllertype"></param>
        /// <returns></returns>
        public static ParserControllerDefinition GetControllerDefinition(string controllertype)
        {
            ParserControllerDefinition found;
            lock(_controllerlock)
            {
                if (_controllers.TryGetValue(controllertype, out found) == false)
                {
                    found = LoadControllerDefinition(controllertype);
                    if (null != found)
                        _controllers.Add(controllertype, found);
                }
 
            }
            return found;
        }

        public static ParserControllerDefinition GetControllerDefinition(Type type)
        {
            string full = type.AssemblyQualifiedName;
            return GetControllerDefinition(full);
        }

        #endregion

        //
        // private implementation
        //

        #region private static Dictionary<string, string> InitAllowedUnqualifiedEntries()

        /// <summary>
        /// Builds the special list of elements that are allowed to be referenced without a namespace.
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, string> InitAllowedUnqualifiedEntries()
        {
            Dictionary<string, string> knownAssemblies = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            knownAssemblies.Add(UnqualifiedBold, UnqualifiedAssembly);
            knownAssemblies.Add(UnqualifiedItalic, UnqualifiedAssembly);
            knownAssemblies.Add(UnqualifiedLineBreak, UnqualifiedAssembly);
            return knownAssemblies;

        }

        #endregion

        private static Dictionary<string, string> InitNamespaceMappings()
        {
            var config = ServiceProvider.GetService<IScryberConfigurationService>();
            var entries = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var maps = config.ParsingOptions.Namespaces;
            if(null != maps && maps.Count > 0)
            {
                foreach (var map in maps)
                {
                    entries.Add(map.Source, map.Namespace + ", " + map.Assembly);
                }
            }
            return entries;
        }

        #region private static ParserClassDefinition UnsafeGetClassDefinitionFromType(Type parsabletype)

        /// <summary>
        /// unsafe method to either retrieve or load the class definiton from the provided type.
        /// Must be called in a thread safe way for multithreaded environments
        /// </summary>
        /// <param name="parsabletype"></param>
        /// <returns></returns>
        private static ParserClassDefinition UnsafeGetClassDefinitionFromType(Type parsabletype)
        {
            ParserClassDefinition found;
            if (!_typedefinitions.TryGetValue(parsabletype, out found))
            {
                found = LoadClassDefinition(parsabletype);
                _typedefinitions[parsabletype] = found;
            }
            return found;
        }

        #endregion

        #region private static Type UnsafeGetType(string elementname, string assemblyQualifiedNamespace, out bool isremote)

        /// <summary>
        /// Unsafe method to extract a type based on a declared assembly namespace.
        /// Must be called from within a thread safe block
        /// </summary>
        /// <param name="elementname"></param>
        /// <param name="assemblyQualifiedNamespace"></param>
        /// <param name="isremote"></param>
        /// <returns></returns>
        private static Type UnsafeGetType(string elementname, string assemblyQualifiedNamespace, bool throwOnNotFound, out bool isremote)
        {
            if (null == assemblyQualifiedNamespace)
                assemblyQualifiedNamespace = string.Empty;
            if (string.IsNullOrEmpty(elementname))
                throw new ArgumentNullException("elementname");

            AssemblyDefn assmdefn;
            NamespaceDefn nsdefn;
            Type t;

            string assm;
            string ns;
            ExtractAssemblyAndNamespace(assemblyQualifiedNamespace, out assm, out ns);
            if (string.IsNullOrEmpty(assm))
            {
                if (throwOnNotFound)
                    throw new PDFParserException(String.Format(Errors.ParserDoesNotHaveAssemblyRegisteredForNamespace, assemblyQualifiedNamespace, elementname));
                else
                {
                    isremote = false;
                    return null;
                }

            }


            if (!_application.TryGetValue(assm, out assmdefn))
            {
                assmdefn = new AssemblyDefn();
                Assembly found = GetAssemblyByName(assm);
                if (null == found)
                {
                    if (throwOnNotFound)
                        throw new PDFParserException(String.Format(Errors.ParserCannotFindAssemblyWithName, assm));
                    else
                    {
                        isremote = false;
                        return null;
                    }
                }
                assmdefn.InnerAssembly = found;
                _application[assm] = assmdefn;
            }

            if (!assmdefn.TryGetValue(ns, out nsdefn))
            {
                nsdefn = new NamespaceDefn();
                PopulateNamespaceFromAssembly(ns, assmdefn, nsdefn);
                assmdefn[ns] = nsdefn;
            }

            if (!nsdefn.TryGetValue(elementname, out t))
            {
                string actual;
                //it's not an actual type so check the remote types and if it's still not fount throw an exception
                if (!nsdefn.RemoteTypes.TryGetValue(elementname, out actual) || !nsdefn.TryGetValue(actual, out t))
                {
                    if (throwOnNotFound)
                        throw new PDFParserException(String.Format(Errors.NoPDFComponentDeclaredWithNameInNamespace, elementname, assemblyQualifiedNamespace));
                    else
                    {
                        isremote = false;
                        return null;
                    }
                }
                else
                {
                    isremote = true;
                }
            }
            else
                isremote = false;

            return t;
        }

        #endregion

        #region private static void PopulateNamespaceFromAssembly(string ns, AssemblyDefn assmdefn, NamespaceDefn nsdefn)

        /// <summary>
        /// Reflects all types in the AssembyDefn's addembly and extracts all the types in theasembly in the required 
        /// namespace that have the PDFParsableComponent attibute defined on the class - adding them to the NamespaceDefn
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="assmdefn"></param>
        /// <param name="nsdefn"></param>
        private static void PopulateNamespaceFromAssembly(string ns, AssemblyDefn assmdefn, NamespaceDefn nsdefn)
        {
            if (assmdefn == null)
                throw new ArgumentNullException("assmdefn");
            if (assmdefn.InnerAssembly == null)
                throw new ArgumentNullException("assmdefn.InnerAssembly");

            if (null == nsdefn)
                throw new ArgumentNullException("nsdefn");
            
            Type[] all = assmdefn.InnerAssembly.GetTypes();
            foreach (Type t in all)
            {
                if (string.Equals(t.Namespace, ns))
                {
                    object[] attrs = t.GetCustomAttributes(typeof(PDFParsableComponentAttribute), false);
                    if (null != attrs && attrs.Length > 0)
                    {
                        PDFParsableComponentAttribute compattr = (PDFParsableComponentAttribute)attrs[0];
                        string name = compattr.ElementName;
                        if (string.IsNullOrEmpty(name))
                            name = t.Name;

                        nsdefn.Add(name, t);

                        //check to see if it has a remote name too
                        attrs = t.GetCustomAttributes(typeof(PDFRemoteParsableComponentAttribute), false);
                        if (null != attrs && attrs.Length > 0)
                        {
                            PDFRemoteParsableComponentAttribute remattr = (PDFRemoteParsableComponentAttribute)attrs[0];
                            string remotename = remattr.ElementName;
                            if (string.IsNullOrEmpty(name))
                                remotename = t.Name + "-Ref";
                            nsdefn.RemoteTypes.Add(remotename, name);
                        }
                    }

                }
            }
        }

        #endregion

        #region private static Assembly GetAssemblyByName(string name)

        /// <summary>
        /// Loads and returns an assembly with the specified name. If name is null or empty, then the entry assembly is returned.
        /// </summary>
        /// <param name="assm"></param>
        /// <returns></returns>
        private static Assembly GetAssemblyByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return Assembly.GetEntryAssembly();
            else
                return Assembly.Load(name);
        }

        #endregion

        #region private static void ExtractAssemblyAndNamespace(string assemblyQualifiedNamespace, out string assm, out string ns)

        /// <summary>
        /// Splits the assembly qualified namespace into the assembly name, and the namespace value
        /// </summary>
        /// <param name="assemblyQualifiedNamespace"></param>
        /// <param name="assm"></param>
        /// <param name="ns"></param>
        private static void ExtractAssemblyAndNamespace(string assemblyQualifiedNamespace, out string assm, out string ns)
        {
            int index = assemblyQualifiedNamespace.IndexOf(NamespaceSeparator);
            if (index < 0)
            {
                assm = string.Empty;
                ns = assemblyQualifiedNamespace;
            }
            else
            {
                assm = assemblyQualifiedNamespace.Substring(index + 1).Trim();
                ns = assemblyQualifiedNamespace.Substring(0, index);
            }
        }

        #endregion

        #region private static ParserClassDefinition LoadClassDefinition(Type parsertype)

        /// <summary>
        /// Loads all the properties and methods on the specified type into a full ParserClassDefinition
        /// </summary>
        /// <param name="parsertype"></param>
        /// <returns></returns>
        private static ParserClassDefinition LoadClassDefinition(Type parsertype)
        {
            ParserClassDefinition defn = new ParserClassDefinition(parsertype);
            LoadClassAttributes(parsertype, defn);
            LoadClassProperties(parsertype, defn);
            LoadClassEvents(parsertype, defn);
            return defn;
        }

        #endregion

        #region private static void LoadClassAttributes(Type parsertype, ParserClassDefinition defn)

        /// <summary>
        /// Loads the class attributes for the type
        /// </summary>
        /// <param name="parsertype"></param>
        /// <param name="defn"></param>
        private static void LoadClassAttributes(Type parsertype, ParserClassDefinition defn)
        {
            PDFRequiredFrameworkAttribute req = GetCustomAttribute<PDFRequiredFrameworkAttribute>(parsertype, true);

            if (null != req)
            {
                if (req.Minimum != PDFRequiredFrameworkAttribute.Empty)
                {
                    if (defn.MinRequiredFramework == PDFRequiredFrameworkAttribute.Empty)
                        defn.MinRequiredFramework = req.Minimum;
                    else if (req.Minimum > defn.MinRequiredFramework)
                        defn.MinRequiredFramework = req.Minimum;
                }
                if (req.Maximum != PDFRequiredFrameworkAttribute.Empty)
                {
                    if (defn.MaxSupportedFramework == PDFRequiredFrameworkAttribute.Empty)
                        defn.MaxSupportedFramework = req.Maximum;
                    else if (req.Maximum < defn.MaxSupportedFramework)
                        defn.MaxSupportedFramework = req.Maximum;
                }
            }

            PDFRemoteParsableComponentAttribute remote = GetCustomAttribute<PDFRemoteParsableComponentAttribute>(parsertype, true);
            if (null != remote)
                defn.SetRemoteParsable(true, remote.ElementName, remote.SourceAttribute);
        }

        #endregion

        #region private static void LoadClassEvents(Type parsertype, ParserClassDefinition defn)

        /// <summary>
        /// Reflects over all the public events on the type and adds EventDefinitions to the class defintion
        /// </summary>
        /// <param name="parsertype"></param>
        /// <param name="defn"></param>
        private static void LoadClassEvents(Type parsertype, ParserClassDefinition defn)
        {
            EventInfo[] all = parsertype.GetEvents(BindingFlags.Public | BindingFlags.Instance);

            foreach (EventInfo ei in all)
            {
                PDFParserIgnoreAttribute ignore = GetCustomAttribute<PDFParserIgnoreAttribute>(ei, true);
                if (null != ignore && ignore.Ignore)
                    continue; //Ignore the event

                PDFAttributeAttribute attr = GetCustomAttribute<PDFAttributeAttribute>(ei, true);
                if (null != attr)
                {
                    string name = attr.AttributeName;
                    if (string.IsNullOrEmpty(name))
                        throw new PDFParserException(String.Format(Errors.ParserAttributeNameCannotBeEmpty, ei.Name, parsertype.FullName));


                    ParserEventDefinition evt = new ParserEventDefinition(name, ei);
                    defn.Events.Add(evt);
                    
                }
            }
        }

        #endregion

        #region private static void LoadClassProperties(Type parsertype, ParserClassDefinition defn)

        /// <summary>
        /// Reflects over all the public properties and adds them to the class definition
        /// </summary>
        /// <param name="parsertype"></param>
        /// <param name="defn"></param>
        private static void LoadClassProperties(Type parsertype, ParserClassDefinition defn)
        {
            PropertyInfo[] all = parsertype.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo pi in all)
            {
                PDFXmlConverter convert;
                string name;
                string ns = string.Empty;
                bool bindOnly = false;

                PDFParserIgnoreAttribute ignore = GetCustomAttribute<PDFParserIgnoreAttribute>(pi, true);
                if (null != ignore && ignore.Ignore)
                    continue; //This property should be ignored.

                PDFAttributeAttribute attr = GetCustomAttribute<PDFAttributeAttribute>(pi, true);

                if (null != attr)
                {
                    name = attr.AttributeName;
                    ns = attr.AttributeNamespace;

                    bool iscustom = false;
                    if (string.IsNullOrEmpty(name))
                        throw new PDFParserException(String.Format(Errors.ParserAttributeNameCannotBeEmpty, pi.Name, parsertype.FullName));
                    if (IsReservedName(name))
                        throw new PDFParserException(String.Format(Errors.ReservedAttributeNameCannotBeUsed, name, pi.Name));

                    if (!IsKnownType(pi.PropertyType, out convert) && !IsCustomParsableType(pi.PropertyType, out convert, out iscustom))
                    {
                        if (!attr.BindingOnly)
                            throw new PDFParserException(String.Format(Errors.ParserAttributeMustBeSimpleOrCustomParsableType, pi.Name, parsertype.FullName, pi.PropertyType));
                        else
                            bindOnly = attr.BindingOnly;
                    }
                    ParserAttributeDefinition ad = new ParserAttributeDefinition(name, ns, pi, convert, iscustom, bindOnly);
                    defn.Attributes.Add(ad);
                }

                PDFElementAttribute ele = GetCustomAttribute<PDFElementAttribute>(pi, true);
                if (null != ele)
                {
                    ParserPropertyDefinition propele;
                    bool isdefault = false;
                    name = ele.Name;
                    ns = ele.NameSpace;

                    if (string.IsNullOrEmpty(name))
                        isdefault = true;
                    PDFArrayAttribute array = GetCustomAttribute<PDFArrayAttribute>(pi, true);
                    PDFTemplateAttribute template = GetCustomAttribute<PDFTemplateAttribute>(pi, true);
                    bool iscustom = false;

                    if (null != array)
                    {
                        Type basetype = array.ContentBaseType;
                        //ArrayCollection
                        if (null == basetype)
                            basetype = typeof(IPDFComponent);

                        propele = new ParserArrayDefinition(name, ns, basetype, pi);
                    }
                    else if (null != template)
                    {
                        propele = new ParserTemplateDefintion(name, pi);
                    }
                    else if (IsKnownType(pi.PropertyType, out convert) || IsCustomParsableType(pi.PropertyType, out convert, out iscustom))
                    {
                        //SimpleElement
                        propele = new ParserSimpleElementDefinition(name, ns, pi, convert, iscustom);
                    }
                    else
                    {
                        //Complex Element
                        propele = new ParserComplexElementDefiniton(name, ns, pi);
                    }

                    if (isdefault)
                    {
                        if (null != defn.DefaultElement)
                            throw new PDFParserException(String.Format(Errors.DuplicateDefaultElementOnClass, pi.Name, pi.DeclaringType));
                        else
                            defn.DefaultElement = propele;
                    }
                    else
                        defn.Elements.Add(propele);
                }
            }
        }

        #endregion

        #region private static bool IsReservedName(string attrName)

        /// <summary>
        /// Checks if the attribute name provided is one of the reserved names - inherits, code-file
        /// </summary>
        /// <param name="attrName"></param>
        /// <returns></returns>
        private static bool IsReservedName(string attrName)
        {
            if (attrName == PDFXMLParser.InheritsAttributeName)
                return true;
            else if (attrName == PDFXMLParser.CodeBehindAttributeName)
                return true;
            else
                return false;
        }

        #endregion

        #region private static bool IsKnownType(Type type, out PDFConverter convert)

        /// <summary>
        /// Checks whether the specified type is a known type.
        /// Known types have pre-defined converters to take Xml content and convert to the correct value. 
        /// This method will return a converter for an xml reader
        /// </summary>
        /// <param name="type"></param>
        /// <param name="xmlconvert">If the return value is true then the xmlconvert value will be set to a delegate that should be able to convert the current node to the proper type</param>
        /// <returns></returns>
        private static bool IsKnownType(Type type, out PDFXmlConverter xmlconvert)
        {
            bool result = false;
            if (type.IsEnum)
            {
                xmlconvert = new PDFXmlConverter(ConverterXml.ToEnum);
                return true;
            }
            TypeCode code = Type.GetTypeCode(type);
            
            switch (code)
            {
                case TypeCode.Boolean:
                    xmlconvert = new PDFXmlConverter(ConverterXml.ToBool);
                    result = true;
                    break;
                case TypeCode.Byte:
                    xmlconvert = new PDFXmlConverter(ConverterXml.ToByte);
                    result = true;
                    break;
                case TypeCode.Char:
                    xmlconvert = new PDFXmlConverter(ConverterXml.ToChar);
                    result = true;
                    break;
                case TypeCode.DBNull:
                    xmlconvert = new PDFXmlConverter(ConverterXml.ToDBNull);
                    result = true;
                    break;
                case TypeCode.DateTime:
                    xmlconvert = new PDFXmlConverter(ConverterXml.ToDateTime);
                    result = true;
                    break;
                case TypeCode.Decimal:
                    xmlconvert = new PDFXmlConverter(ConverterXml.ToDecimal);
                    result = true;
                    break;
                case TypeCode.Double:
                    xmlconvert = new PDFXmlConverter(ConverterXml.ToDouble);
                    result = true;
                    break;
                case TypeCode.Int16:
                    xmlconvert = new PDFXmlConverter(ConverterXml.ToInt16);
                    result = true;
                    break;
                case TypeCode.Int32:
                    xmlconvert = new PDFXmlConverter(ConverterXml.ToInt32);
                    result = true;
                    break;
                case TypeCode.Int64:
                    xmlconvert = new PDFXmlConverter(ConverterXml.ToInt64);
                    result = true;
                    break;
                case TypeCode.SByte:
                    xmlconvert = new PDFXmlConverter(ConverterXml.ToSByte);
                    result = true;
                    break;
                case TypeCode.Single:
                    xmlconvert = new PDFXmlConverter(ConverterXml.ToFloat);
                    result = true;
                    break;
                case TypeCode.String:
                    xmlconvert = new PDFXmlConverter(ConverterXml.ToString);
                    result = true;
                    break;
                case TypeCode.UInt16:
                    xmlconvert = new PDFXmlConverter(ConverterXml.ToUInt16);
                    result = true;
                    break;
                case TypeCode.UInt32:
                    xmlconvert = new PDFXmlConverter(ConverterXml.ToUInt32);
                    result = true;
                    break;
                case TypeCode.UInt64:
                    xmlconvert = new PDFXmlConverter(ConverterXml.ToUInt64);
                    result = true;
                    break;
                case TypeCode.Object:
                    if (type == typeof(Guid))
                    {
                        xmlconvert = new PDFXmlConverter(ConverterXml.ToGuid);
                        result = true;
                    }
                    else if(type == typeof(DateTime))
                    {
                        xmlconvert = new PDFXmlConverter(ConverterXml.ToDateTime);
                        result = true;
                    }
                    else if (type == typeof(TimeSpan))
                    {
                        xmlconvert = new PDFXmlConverter(ConverterXml.ToTimeSpan);
                        result = true;
                    }
                    else if (type == typeof(Uri))
                    {
                        xmlconvert = new PDFXmlConverter(ConverterXml.ToUri);
                        result = true;
                    }
                    else if (type == typeof(Type))
                    {
                        xmlconvert = new PDFXmlConverter(ConverterXml.ToType);
                        result = true;
                    }
                    else if (type == typeof(System.Xml.XPath.IXPathNavigable))
                    {
                        xmlconvert = new PDFXmlConverter(ConverterXml.ToXPathNavigable);
                        result = true;
                    }
                    else if (type == typeof(System.Xml.XPath.XPathNavigator))
                    {
                        xmlconvert = new PDFXmlConverter(ConverterXml.ToXPathNavigator);
                        result = true;
                    }
                    else if (type == typeof(System.Xml.XmlNode))
                    {
                        xmlconvert = new PDFXmlConverter(ConverterXml.ToXmlNode);
                        result = true;
                    }
                    else if(type== typeof(IPDFTemplate))
                    {
                        xmlconvert = new PDFXmlConverter(ConverterXml.ToPDFTemplate);
                        result = true;
                    }
                    else if (type == typeof(System.Xml.XmlDocument))
                    {
                        xmlconvert = new PDFXmlConverter(ConverterXml.ToXmlDocument);
                        result = true;
                    }
                    else
                    {
                        xmlconvert = null;
                        result = false;
                    }
                    break;
                case TypeCode.Empty:
                default:
                    xmlconvert = null;
                    result = false;
                    break;
            }
            return result;
        }

        #endregion

        #region private static bool IsCustomParsableType(Type type, out PDFXmlConverter convert, out bool iscustom)

        /// <summary>
        /// Retrurns true and an XML converter to convert a reader value to the required type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="convert"></param>
        /// <param name="iscustom"></param>
        /// <returns></returns>
        private static bool IsCustomParsableType(Type type, out PDFXmlConverter convert, out bool iscustom)
        {
            PDFParsableValueAttribute valattr = GetCustomAttribute<PDFParsableValueAttribute>(type, true);
            if (null != valattr)
            {
                iscustom = true;
                convert = ConverterXml.GetParsableXmlConverter(type);
                return null != convert;
            }
            else
            {
                iscustom = false;
                convert = null;
                return false;
            }
        }

        #endregion

        //
        // controller defn methods
        //

        #region private static ParserControllerDefinition LoadControllerDefinition(string typename)

        /// <summary>
        /// Attempts to reflect and load a new ParserControllerDefinition for a type that matches typename
        /// </summary>
        /// <param name="typename">The full name of the type to load the definition for</param>
        /// <returns>Either Null if the type was not found, or a new populated ParserControllerDefinition</returns>
        private static ParserControllerDefinition LoadControllerDefinition(string typename)
        {
            Type controllerType = Type.GetType(typename, false);
            if (null == controllerType)
                return null;
            else
                return LoadControllerDefinition(typename, controllerType);
        }

        #endregion

        #region private static ParserControllerDefinition LoadControllerDefinition(string name, Type type)

        /// <summary>
        /// Creates a new ParserControllerDefinition instance based on the Type and fills the actions and outlets 
        /// declated on the type
        /// </summary>
        /// <param name="name">The name of the controller type</param>
        /// <param name="type">The type of the controller</param>
        /// <returns>The populated definition</returns>
        private static ParserControllerDefinition LoadControllerDefinition(string name, Type type)
        {
            ParserControllerDefinition defn = new ParserControllerDefinition(name, type);
            FillControllerOutlets(defn);
            FillControllerActions(defn);

            return defn;
        }

        #endregion

        #region private static void FillControllerOutlets(ParserControllerDefinition defn)

        /// <summary>
        /// Fills the definitions outlets (properties and fields) with all the declared outlets on the type
        /// </summary>
        /// <param name="defn"></param>
        private static void FillControllerOutlets(ParserControllerDefinition defn)
        {
            PropertyInfo[] allprops = defn.ControllerType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo aprop in allprops)
            {
                //Don't use the standard methods as the bug in the .Net framework for inherited PropertyInfo.GetCustomAttributes
                //Will not return properties where they are set to be inherited.
                //Use the System.Attribute.GetCustomAttribute method instead.
                //Keeping the same method for main factory parsing as that is tested and in-use.

                Attribute found = System.Attribute.GetCustomAttribute(aprop, typeof(PDFOutletAttribute), true);
                if (found != null)
                {
                    PDFOutletAttribute outletAttr = (PDFOutletAttribute)found;
                    if (outletAttr.IsOutlet)
                    {
                        ParserControllerOutlet outlet = new ParserControllerOutlet(aprop, outletAttr.ComponentID, outletAttr.Required);
                        if (defn.Actions.Contains(outlet.ID))
                            throw new PDFParserException(string.Format(Errors.ControllerAlreadyHasOutletWithID, defn.ControllerTypeName, outlet.ID));
                        defn.Outlets.Add(outlet);
                    }
                }
            }

            FieldInfo[] allfileds = defn.ControllerType.GetFields(BindingFlags.Public | BindingFlags.Instance);
            if (null != allfileds)
            {
                foreach (FieldInfo afield in allfileds)
                {
                    
                    Attribute found = System.Attribute.GetCustomAttribute(afield, typeof(PDFOutletAttribute), true);
                    if (found != null)
                    {
                        PDFOutletAttribute outletAttr = (PDFOutletAttribute)found;
                        if (outletAttr.IsOutlet)
                        {
                            ParserControllerOutlet outlet = new ParserControllerOutlet(afield, outletAttr.ComponentID, outletAttr.Required);
                            if (defn.Actions.Contains(outlet.ID))
                                throw new PDFParserException(string.Format(Errors.ControllerAlreadyHasOutletWithID, defn.ControllerTypeName, outlet.ID));

                            defn.Outlets.Add(outlet);
                        }
                    }
                }
            }
        }

        #endregion

        #region private static void FillControllerActions(ParserControllerDefinition defn)
        
        /// <summary>
        /// Fills the definitions actions (methods) with all the declared actions on its type
        /// </summary>
        /// <param name="defn"></param>
        private static void FillControllerActions(ParserControllerDefinition defn)
        {
            MethodInfo[] allmeths = defn.ControllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance);

            foreach (MethodInfo ameth in allmeths)
            {
                Attribute found = System.Attribute.GetCustomAttribute(ameth, typeof(PDFActionAttribute), true);
                if (found != null)
                {
                    PDFActionAttribute actionAttr = (PDFActionAttribute)found;
                    if (actionAttr.IsAction)
                    {
                        ParserControllerAction action = new ParserControllerAction(ameth, actionAttr.Name);
                        if (defn.Actions.Contains(action.Name))
                            throw new PDFParserException(string.Format(Errors.ControllerAlreadyHasActionName, defn.ControllerTypeName, action.Name));
                        defn.Actions.Add(action);
                    }
                }
            }
        }

        #endregion

        //
        // GetCustomAttribute overloads
        //

        #region internal static T GetCustomAttribute<T>(Type ontype, bool inherit) where T : Attribute

        /// <summary>
        /// Returns one (and only one) custom attribute of type &lt;T&gt; defined on the class ontype specified
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ontype"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        internal static T GetCustomAttribute<T>(Type ontype, bool inherit) where T : Attribute
        {
            return GetCustomAttribute(ontype, typeof(T), inherit) as T;
        }

        #endregion

        #region private static Attribute GetCustomAttribute(Type declaring, Type attrType, bool inherit)

        /// <summary>
        /// Returns one (and only one) custom attribute of type attrType defined on the class declaring specified
        /// </summary>
        /// <param name="declaring"></param>
        /// <param name="attrType"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        private static Attribute GetCustomAttribute(Type declaring, Type attrType, bool inherit)
        {
            object[] found = declaring.GetCustomAttributes(attrType, inherit);
            if (found == null || found.Length == 0)
                return null;
            else
                return found[0] as Attribute;
        }

        #endregion

        #region private static T GetCustomAttribute<T>(MemberInfo pi, bool inherit) where T : Attribute

        /// <summary>
        /// Returns one (and only one) custom attribute of type &lt;T&gt; defined on the member specified
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pi"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        private static T GetCustomAttribute<T>(MemberInfo pi, bool inherit) where T : Attribute
        {
            return GetCustomAttribute(pi, typeof(T), inherit) as T;
        }

        #endregion

        #region private static Attribute GetCustomAttribute(MemberInfo pi, Type attrType, bool inherit)
        
        /// <summary>
        /// Returns one (and only one) custom attribute of type attrType defined on the member specified
        /// </summary>
        /// <param name="pi"></param>
        /// <param name="attrType"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        private static Attribute GetCustomAttribute(MemberInfo pi, Type attrType, bool inherit)
        {
            object[] found = pi.GetCustomAttributes(attrType, inherit);
            if (found == null || found.Length == 0)
                return null;
            else
                return found[0] as Attribute;
        }

        #endregion
    }
}
