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
using System.IO;
using System.Xml;
using System.Data.Common;

using Scryber.Logging;

namespace Scryber.Generation
{
    public class XMLParser : IComponentParser
    {
        private static readonly string[] TextElements = new string[] { "BR" };

        protected const string ScryberProcessingInstructionsName = "scryber";
        protected string ParserLogCategory = "Xml Parser";
        public const string InheritsAttributeName = "inherits";
        public const string CodeBehindAttributeName = "code-file";
        public const string IDAttributeName = "id";
        public const string FilePathAttributeName = "source";
        public const string SelectAttributeName = "select";
        public const string XmlSchemaNamespace = "http://www.w3.org/XML/1998/namespace";

        //
        // properties
        //

        #region public object RootComponent {get;set;}

        private object _root;

        /// <summary>
        /// Gets or sets the root component for the parser. 
        /// If set before parsing then this instance will be used as any event handler. 
        /// If not set, then this parser will set it with the top level parsed component
        /// </summary>
        public object RootComponent
        {
            get { return _root; }
            set { _root = value; }
        }

        #endregion

        #region public PDFGeneratorSettings Settings {get;}

        private ParserSettings _settings;
        /// <summary>
        /// Gets the settings for this parser / generator
        /// </summary>
        public ParserSettings Settings
        {
            get { return _settings; }
        }

        #endregion

        #region public ConformanceMode Mode {get;set;}

        /// <summary>
        /// Gets or sets the conformance mode (Strict or Lax) for the parser
        /// </summary>
        /// <remarks>If the mode is strict, then unknown attributes and components will raise exceptions.
        /// If lax, then they will be logged, skipped, and parsing will continue</remarks>
        public ParserConformanceMode Mode
        {
            get
            {
                return _settings.ConformanceMode;
            }
            set
            {
                _settings.ConformanceMode = value;
            }
        }

        #endregion

        #region public System.Collections.Specialized.NameValueCollection PrefixedNamespaces {get;}

        private Dictionary<string, string> _nsDeclarations = new Dictionary<string, string>();

        /// <summary>
        /// Gets the dictionary of prefixes to namespaces in the parsed document
        /// </summary>
        public Dictionary<string, string> PrefixedNamespaces
        {
            get
            {
                return _nsDeclarations;
            }
        }

        #endregion

        #region public string LoadedSourcePath { get; set; }

        /// <summary>
        /// Gets or sets the path to local and remote sources.
        /// </summary>
        public string LoadedSourcePath { get; set; }

        #endregion

        #region internal ParserControllerDefinition ControllerDefinition {get; private set;}

        /// <summary>
        /// Gets the Controller Definition that contains the outlets and actions available to be hooked into during parsing.
        /// </summary>
        internal ParserControllerDefinition ControllerDefinition
        {
            get;
            set;
        }

        #endregion

        #region internal object Controller {get; private set;}

        /// <summary>
        /// Gets the actual controller instance that can be hooked into and bindings made with
        /// </summary>
        public object Controller
        {
            get;
            private set;
        }

        #endregion

        #region internal bool HasController {get;}

        /// <summary>
        /// Returns true if this Parser has a controller that Actions and Outlets can be assigned to.
        /// </summary>
        internal bool HasController
        {
            get { return null != this.Controller; }
        }

        #endregion

        #region private List<ParserControllerOutlet> UnassignedOutlets { get; set; }

        /// <summary>
        /// Gets or sets the list of outlets in the current controller that have not yet been assigned a value.
        /// </summary>
        private List<ParserControllerOutlet> UnassignedOutlets { get; set; }

        #endregion

        //
        // ctor
        //

        #region public PDFXMLParser(PDFGeneratorSettings settings)

        /// <summary>
        /// Instantiates a new PDFXMLParser that will use the provided PDFGeneratorSettings as rules for parsing content
        /// </summary>
        /// <param name="settings"></param>
        public XMLParser(ParserSettings settings)
        {
            this._settings = settings;
        }

        #endregion

        //
        // public interface
        //

        #region public IPDFComponent Parse(string source, Stream stream, bool istemplate) + 2 overloads

        /// <summary>
        /// Parses a stream (with the source value set to an appropriate value)
        /// </summary>
        /// <param name="source">An identifier (usually a file path or unique id) that can be recognised further into the execution by any reference resolver</param>
        /// <param name="stream">The stream of data to parse</param>
        /// <param name="istemplate">True if this stream is a template content (rather than a complete standalone file)</param>
        /// <returns>The parsed component</returns>
        public IComponent Parse(string source, Stream stream, ParseSourceType type)
        {
            using (XmlReader reader = new XmlHtmlEntityReader(stream))
            {
                return Parse(source, reader, type);
            }
        }

        /// <summary>
        /// Parses a text reader (with the source value set to an appropriate value)
        /// </summary>
        /// <param name="source">An identifier (usually a file path or unique id) that can be recognised further into the execution by any reference resolver</param>
        /// <param name="reader">The TextReader of information to parse</param>
        /// <param name="istemplate">True if this stream is a template content (rather than a complete standalone file)</param>
        /// <returns>The parsed component</returns>
        public IComponent Parse(string source, TextReader reader, ParseSourceType type)
        {
            using (XmlReader xreader = new XmlHtmlEntityReader(reader))
            {
                return this.Parse(source, xreader, type);
            }
        }

        /// <summary>
        /// Parses an XML reader (with the source value set to an appropriate value)
        /// </summary>
        /// <param name="source">An identifier (usually a file path or unique id) that can be recognised further into the execution by any reference resolver</param>
        /// <param name="stream">The stream of data to parse</param>
        /// <param name="istemplate">True if this stream is a template content (rather than a complete standalone file)</param>
        /// <returns></returns>
        /// <returns></returns>
        public IComponent Parse(string source, XmlReader reader, ParseSourceType type)
        {
            try
            {
                return DoParse(source, reader, type);
            }
            catch(Exception ex)
            {
                throw new PDFParserException(string.Format(Errors.CouldNotParseSource, source, ex.Message), ex);
            }
        }

        #endregion

        #region protected virtual IPDFComponent DoParse(string source, XmlReader reader, ParseSourceType type)


        /// <summary>
        /// Top level Parse method which returns the complete component that was parsed from the source.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="reader"></param>
        /// <param name="istemplate"></param>
        /// <returns></returns>
        protected virtual IComponent DoParse(string source, XmlReader reader, ParseSourceType type)
        {
            
            IDisposable recorder;
            if (type == ParseSourceType.Template)
                recorder = this.Settings.PerformanceMonitor.Record(PerformanceMonitorType.Parse_Templates, source);
            else
                recorder = this.Settings.PerformanceMonitor.Record(PerformanceMonitorType.Parse_Files, source);

            IComponent parsed = null;
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.ProcessingInstruction)
                {
                    string name = reader.Name;
                    if (name == ScryberProcessingInstructionsName)
                        this.ParseProcessingInstructions(reader);
                }
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (this.Settings.AppendLog)
                        this.InjectCollectorLog();

                    if (type == ParseSourceType.Template)
                        LogBegin(TraceLevel.Verbose, "Beginning parsing tempalate");
                    else if (this.Settings.TraceLog.ShouldLog(TraceLevel.Verbose))
                        LogBegin(TraceLevel.Message, "Beginning parse of XML source '{0}'", source);

                    //make sure we have an instance of our controller type to hand
                    this.EnsureControllerInstance();

                    object value = ParseComponent(reader, true);

                    // must return a value from this parse methos
                    if (!(value is IComponent))
                        throw BuildParserXMLException(reader, Errors.CannotConvertObjectToType, value.GetType(), typeof(IComponent));

                    parsed = (IComponent)value;

                    if (type == ParseSourceType.Template)
                        LogEnd(TraceLevel.Verbose, "Completed parsing template and generated component '{0}'", parsed.GetType(), source);
                    else if (this.Settings.TraceLog.ShouldLog(TraceLevel.Verbose))
                        LogEnd(TraceLevel.Message, "Completed parse of XML source and generated component '{0}'", parsed.GetType(), source);
                    else
                        LogAdd(reader, TraceLevel.Message, "Parsed the XML source '{1}' and generated component {0}", parsed.GetType(), source);
                    break;
                }
            }

            ApplyParsedComponentProperties(source, ParserLoadType.ReflectiveParser, parsed);
            EnsureAllOutletsAreAssigned(reader, parsed);

            recorder.Dispose();

            return parsed;
        }

        #endregion

        #region protected virtual void EnsureAllOutletsAreAssigned(XmlReader reader, IPDFComponent parsed)

        /// <summary>
        /// Makes sure that if this has a controller, then all the outlets 
        /// on that controller which are required are actually assigned.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="parsed"></param>
        protected virtual void EnsureAllOutletsAreAssigned(XmlReader reader, IComponent parsed)
        {
            if (parsed is IControlledComponent && this.HasController)
            {
                foreach (ParserControllerOutlet outlet in this.UnassignedOutlets)
                {
                    if (outlet.Required)
                        throw new PDFParserException(String.Format("The required outlet {0} on the controller has not been assigned from the parsed source. " +
                            " Make sure there is a component with id '{1}' in the main content of source, or UNmark the outlet as required", outlet.OutletMember.Name, outlet.ID));
                }

                LogAdd(reader, TraceLevel.Verbose, "All required outlets have been assigned on the controller.");

                ((IControlledComponent)parsed).Controller = this.Controller;
            }
        }

        #endregion

        #region protected virtual void ApplyParsedComponentProperties(string source, ParserLoadType loadtype, IPDFComponent parsed)

        /// <summary>
        /// Makes sure the top level parsed component has any properties set that come from the parsing
        /// </summary>
        /// <param name="source"></param>
        /// <param name="loadtype"></param>
        /// <param name="parsed"></param>
        protected virtual void ApplyParsedComponentProperties(string source, ParserLoadType loadtype, IComponent parsed)
        {
            if (parsed is IRemoteComponent)
            {
                IRemoteComponent comp = parsed as IRemoteComponent;

                if (!string.IsNullOrEmpty(source))
                    comp.LoadedSource = source;
                else if (!string.IsNullOrEmpty(this.LoadedSourcePath))
                    comp.LoadedSource = this.LoadedSourcePath;

                comp.LoadType = loadtype;

                foreach (string key in this.PrefixedNamespaces.Keys)
                {
                    comp.RegisterNamespaceDeclaration(key, this.PrefixedNamespaces[key]);
                }
            }
            if (parsed is IParsedDocument)
            {
                IParsedDocument xpdoc = parsed as IParsedDocument;
                xpdoc.SetTraceLog(this.Settings.TraceLog);
                xpdoc.SetConformanceMode(this.Settings.ConformanceMode);
                xpdoc.AppendTraceLog = this.Settings.AppendLog;
                xpdoc.PerformanceMonitor = this.Settings.PerformanceMonitor;
            }
        }

        #endregion

        //
        // parse implementation
        //

        #region protected XmlReaderSettings GetXmlSettings()

        /// <summary>
        /// Gets the XML Settings for the reader.
        /// </summary>
        /// <returns></returns>
        protected XmlReaderSettings GetXmlSettings()
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = false;
            settings.DtdProcessing = DtdProcessing.Ignore;
            //settings.ValidationType = ValidationType.DTD;
            //settings.ValidationEventHandler += Settings_ValidationEventHandler;
            
            return settings;
        }

        //private void Settings_ValidationEventHandler(object sender, System.Xml.Schema.ValidationEventArgs e)
        //{
        //    this.LogAdd(null, TraceLevel.Error, "DTD Not present for the entity " + e.Message);
        //}

        #endregion

        #region protected virtual object ParseComponent(XmlReader reader, bool isroot)

        /// <summary>
        /// Instantiates and parses the current component at the xml readers current node including any inner components or reference components.
        /// </summary>
        /// <param name="reader">The current xml reader to read the component from</param>
        /// <param name="isroot">If true this is the top level component. If false then this </param>
        /// <returns></returns>
        protected virtual object ParseComponent(XmlReader reader, bool isroot)
        {
            if (reader.NodeType != XmlNodeType.Element)
                throw BuildParserXMLException(reader, Errors.CanOnlyParseComponentAsElement);
            
            bool isremote;
            ParserClassDefinition cdef;
            if (this.Mode == ParserConformanceMode.Strict)
                cdef = AssertGetClassDefinition(reader, out isremote);
            else if (!TryGetClassDefinition(reader, out cdef, out isremote))
            {
                //No Class definition - so skip this element
                SkipOverCurrentElement(reader);
                return null;
            }

            this.CheckFrameworkIsSupported(reader, cdef, this.Mode);

            if (isremote)
                return this.ParseRemoteComponent(cdef, reader);
            else
                return ParseComplexComponent(reader, cdef, isroot);
        }

        #endregion

        #region private object ParseComplexComponent(XmlReader reader, ParserClassDefinition cdef, bool isroot)

        /// <summary>
        /// Top level ParseComplexComponent that will parse a complex component element (and all the inner elements in there) and return an instance
        /// </summary>
        /// <param name="reader">The reader to parse from</param>
        /// <param name="cdef">The class definition of the type of component that to be parsed</param>
        /// <param name="isroot">If true then this is the top level root component</param>
        /// <returns>The parsed component</returns>
        private object ParseComplexComponent(XmlReader reader, ParserClassDefinition cdef, bool isroot)
        {
            string fullname = reader.Name;
            object value = null;
            LogBegin(TraceLevel.Verbose, "Parsing found component '{0}' of type '{1}'", fullname, cdef.ClassType);

            try
            {
                value = this.CreateInstance(cdef);
                
                if (isroot && null == this.RootComponent)
                    this.RootComponent = value;

                ParseComplexComponentXml(value, reader, cdef);

                LogEnd(TraceLevel.Verbose, "Completed parsing of component of type '{0}'", cdef.ClassType);
            }
            catch (PDFParserException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildParserXMLException(ex, reader, Errors.CouldNotParseComponentOfType, cdef.ClassType, ex.Message);
            }
            return value;
        }

        #endregion

        #region private void ParseComplexComponentXml(object component, XmlReader reader, ParserClassDefinition cdef)

        

        /// <summary>
        /// Parses the actual XML content of the reader and pushes the parsed values onto the provided object.
        /// Also parses any inner content or template content of the element
        /// </summary>
        /// <param name="component"></param>
        /// <param name="reader"></param>
        /// <param name="cdef"></param>
        private void ParseComplexComponentXml(object component, XmlReader reader, ParserClassDefinition cdef)
        {
            string name = reader.LocalName;
            string ns = reader.NamespaceURI;
            string prefix = reader.Prefix;


            bool isempty = reader.IsEmptyElement;


            if (!string.IsNullOrEmpty(prefix))
                this.EnsureNamespaceRegistered(prefix, ns);

            if (component is Scryber.IComponent)
                (component as IComponent).ElementName = reader.Name;

            if (reader.HasAttributes)
                ParseAttributes(component, false, reader, cdef);
            if (!isempty)
            {
                if (cdef.DefaultElement != null && cdef.DefaultElement.ParseType == DeclaredParseType.TempateElement)
                {
                    if (reader.HasAttributes)
                        reader.MoveToElement();
                    ParseTemplateContent(component, reader, cdef.DefaultElement);
                }
                else if (cdef.DefaultElement != null && cdef.DefaultElement.PropertyInfo.PropertyType == typeof(System.Xml.XmlNode))
                    ParseSimpleElement(component, reader, cdef.DefaultElement, cdef);
                else
                    ParseContents(component, reader, name, ns, cdef);
            }
            
        }

        #endregion

        #region private object ParseRemoteComponent(ParserClassDefinition cdef, XmlReader reader)

        /// <summary>
        /// Resolves and loads a remote referenced definition
        /// </summary>
        /// <param name="cdef"></param>
        /// <param name="resolver"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        private object ParseRemoteComponent(ParserClassDefinition cdef, XmlReader reader)
        {
            string element = reader.LocalName;
            string ns = reader.NamespaceURI;
            bool empty = reader.IsEmptyElement;

            if (!reader.MoveToAttribute(cdef.RemoteSourceAttribute))
                throw BuildParserXMLException(reader, Errors.RequiredAttributeNoFoundOnElement, FilePathAttributeName, element);

            string path = reader.Value;
            string select = string.Empty;

            if (reader.MoveToAttribute(SelectAttributeName))
                select = reader.Value;

            object complex = ResolveAndParseRemoteComponent(reader, path, select);

            if (null != complex)
            {
                if (reader.HasAttributes && reader.AttributeCount > 1)
                    this.ParseAttributes(complex, true, reader, cdef);

                if (!empty)
                    this.ParseContents(complex, reader, element, ns, cdef);
            }
            //this.ParseComplexComponent(complex, reader, cdef, resolver);
            return complex;
        }

        #endregion

        #region private void ParseAttributes(object container, bool isremotecomponent, XmlReader reader, ParserClassDefinition cdef)

        /// <summary>
        /// Parses all the attributes for the current compment in the XMlReaders current element
        /// </summary>
        /// <param name="container"></param>
        /// <param name="isremotecomponent"></param>
        /// <param name="reader"></param>
        /// <param name="cdef"></param>
        /// <param name="skipUnknown">Optional. If true then any unknown attributes will simply be ignored</param>
        private void ParseAttributes(object container, bool isremotecomponent, XmlReader reader, ParserClassDefinition cdef, bool skipUnknown = false)
        {
            if (!reader.MoveToFirstAttribute())
                return;
            do
            {
                string name = reader.LocalName;
                string ns = reader.NamespaceURI;
                string prefix = reader.Prefix;

                if(!string.IsNullOrEmpty(prefix))
                    ns = ParserDefintionFactory.LookupAssemblyForXmlNamespace(ns);
                
                ParserPropertyDefinition attr;
                ParserEventDefinition evt;
                IPDFBindingExpressionFactory factory;
                if (IsSpecialAttribute(reader, isremotecomponent, cdef))
                {
                    if ((reader.Name.ToLower() == "xmlns" || reader.Name.ToLower().StartsWith("xmlns:")))
                        this.RegisterNamespaceDeclaration(reader);
                }
                else if (cdef.Attributes.TryGetPropertyDefinition(name, ns, out attr))
                {
                    if(!string.IsNullOrEmpty(prefix))
                        this.EnsureNamespaceRegistered(prefix, ns);

                    string value =reader.Value;
                    if (ParserHelper.IsBindingExpression(ref value,out factory, this.Settings))
                    {
                        GenerateBindingExpression(reader, container, cdef, attr, value, factory);
                    }
                    else
                    {
                        object actualValue;
                        actualValue = attr.GetValue(reader, this.Settings);

                        if (actualValue != DBNull.Value) //DBNull is a special value to signify nothing (but not null)
                        {
                            this.SetValue(container, actualValue, attr);

                            if (attr.IsParserSourceValue)
                                this.LoadedSourcePath = actualValue.ToString();
                        }
                    }

                    //Special case for setting a property on the controller if declared.
                    if (this.IsIDAttribute(name, prefix, ns, cdef) && this.HasController)
                    {
                        this.TryAssignToControllerOutlet(reader, container, value);
                    }
                }
                else if (cdef.Events.TryGetPropertyDefinition(name, out evt))
                {
                    string expression = reader.Value;
                    
                    if (ParserHelper.IsBindingExpression(ref expression, out factory, this.Settings))
                    {
                        if (this.Mode == ParserConformanceMode.Strict)
                            throw BuildParserXMLException(reader, Errors.CannotSpecifyBindingExpressionsOnEvents, expression, name);
                        else
                            LogAdd(reader, TraceLevel.Error, Errors.CannotSpecifyBindingExpressionsOnEvents, expression, name);
                    }
                    else if(this.HasController)
                        TryAttachEventHandlerToController(reader, container, reader.Value, evt, cdef);
                    
                }
                else if(skipUnknown)
                {
                    LogAdd(reader, TraceLevel.Verbose, "Unknown attribute " + name + " is being explicitly skipped on " + cdef.ClassType);
                }
                else if (this.Mode == ParserConformanceMode.Strict)
                    throw BuildParserXMLException(reader, Errors.ParsedTypeDoesNotContainDefinitionFor, cdef.ClassType, "attribute", name);
                else
                    LogAdd(reader, TraceLevel.Error, "Skipping unknown attribute '{0}' on type {1}", name, cdef.ClassType);

            } while (reader.MoveToNextAttribute());
        }

        #endregion

        #region private void ParseContents(object container, XmlReader reader, string element, string ns, ParserClassDefinition cdef)

        /// <summary>
        /// Parses the inner contents of a component
        /// </summary>
        /// <param name="container">The current component that should have the contents parsed</param>
        /// <param name="reader">The current xml reader</param>
        /// <param name="element">The element name of the current component</param>
        /// <param name="ns">The declared namespace of the current component</param>
        /// <param name="cdef">The class definition of the current component</param>
        private void ParseContents(object container, XmlReader reader, string element, string ns, ParserClassDefinition cdef)
        {

            LogAdd(reader, TraceLevel.Debug, "Parsing container contents");
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.EndElement)
                {
                    if (reader.LocalName == element && reader.NamespaceURI == ns)
                        break;
                }
                else if (reader.NodeType == XmlNodeType.Element)
                {
                    ParserPropertyDefinition prop;
                    if (cdef.Elements.TryGetPropertyDefinition(reader.LocalName, reader.NamespaceURI, out prop))
                    {

                        if (prop.ParseType == DeclaredParseType.SimpleElement)
                            this.ParseSimpleElement(container, reader, prop, cdef);
                        else if (prop.ParseType == DeclaredParseType.ArrayElement)
                            this.ParseInnerCollection(container, reader, prop);
                        else if (prop.ParseType == DeclaredParseType.ComplexElement)
                            this.ParseComplexType(container, reader, prop);
                        else if (prop.ParseType == DeclaredParseType.TempateElement)
                            this.ParseTemplateContent(container, reader, prop);
                        else if (this.Mode == ParserConformanceMode.Strict)
                            throw BuildParserXMLException(reader, Errors.ParsedTypeDoesNotContainDefinitionFor, cdef.ClassType, "element", reader.LocalName);
                        else
                            reader.Skip();
                    }
                    else if (cdef.DefaultElement != null)
                    {
                        if (cdef.DefaultElement.Name == reader.LocalName && ns == reader.NamespaceURI)
                            this.ParseComplexType(container, reader, cdef.DefaultElement);
                        else if (cdef.DefaultElement.ParseType == DeclaredParseType.ArrayElement)
                        {

                            ParserArrayDefinition arry = (ParserArrayDefinition)cdef.DefaultElement;
                            object collection = InitArrayCollection(container, arry);
                            object component = ParseComponent(reader, false);

                            if (null != component)
                            {
                                LogAdd(reader, TraceLevel.Debug, "Adding component '{0}' to default collection in property {1} of type '{2}'", component, arry.PropertyInfo.Name, cdef.ClassType);
                                arry.AddToCollection(collection, component);
                            }


                        }
                        else if (String.IsNullOrEmpty(cdef.DefaultElement.Name))
                            this.ParseComplexType(container, reader, cdef.DefaultElement);

                        else if (reader.NamespaceURI == XmlSchemaNamespace)
                        {
                            //Ignore xml namespace elements
                        }
                        else if (this.Mode == ParserConformanceMode.Strict)
                            throw BuildParserXMLException(reader, Errors.ParsedTypeDoesNotContainDefinitionFor, cdef.ClassType, "element", reader.LocalName);
                        else
                        {
                            LogAdd(reader, TraceLevel.Error, "Skipping unknown element '{0}'", reader.Name);
                            reader.Skip();
                        }
                    }
                    else if (reader.NamespaceURI == XmlSchemaNamespace)
                    {
                        //Ignore xml namespace elements
                    }
                    else if (this.Mode == ParserConformanceMode.Strict)
                        throw BuildParserXMLException(reader, Errors.ParsedTypeDoesNotContainDefinitionFor, cdef.ClassType, "element", reader.LocalName);
                    else
                    {
                        LogAdd(reader, TraceLevel.Error, "Skipping unknown element '{0}'", reader.Name);
                        reader.Skip();
                    }

                }
                else if (reader.NodeType == XmlNodeType.Text)
                {
                    if (cdef.DefaultElement != null && string.IsNullOrEmpty(cdef.DefaultElement.Name))
                    {
                        if (cdef.DefaultElement.ParseType == DeclaredParseType.SimpleElement)
                        {
                            ParserSimpleElementDefinition se = (ParserSimpleElementDefinition)cdef.DefaultElement;
                            object value = se.GetValue(reader, this.Settings);
                            this.SetValue(container, value, se);
                            break; //we have read past the end of the content of this container
                        }
                        else if (cdef.DefaultElement.ParseType == DeclaredParseType.ArrayElement)
                        {
                            //We are already positioned on the current content so it should be 
                            //included until we end the element
                            bool usecurrenttextnode = true;
                            ParseInnerCollection(container, element, ns, usecurrenttextnode, reader, cdef.DefaultElement);
                            //If we have parsed all the content of the element then we need to break off
                            if (reader.NodeType == XmlNodeType.EndElement)
                            {
                                if (reader.LocalName == element && reader.NamespaceURI == ns)
                                    break;
                            }
                        }
                        else if (this.Mode == ParserConformanceMode.Strict)
                            throw this.BuildParserXMLException(reader, Errors.ParserAttributeMustBeSimpleOrCustomParsableType, cdef.DefaultElement.PropertyInfo.Name, cdef.ClassType);
                        else
                            LogAdd(reader, TraceLevel.Message, "Skipping text content of property '" + cdef.DefaultElement.PropertyInfo.Name + "' on class '" + cdef.ClassType + ", because it is not a simple or parsable type");
                    }
                    else if (reader.NamespaceURI == XmlSchemaNamespace)
                    {
                        //Ignore xml namespace elements
                    }
                    else if (this.Mode == ParserConformanceMode.Strict)
                    {
                        throw this.BuildParserXMLException(reader, Errors.ParsedTypeDoesNotContainDefinitionFor, cdef.ClassType, "element", reader.LocalName);
                    }
                    else
                    {
                        LogAdd(reader, TraceLevel.Message, "Skipping unknown element '{0}'", reader.Name);
                        reader.Skip();
                    }

                }
            }
        }

        #endregion

        #region private void ParseSimpleElement(object container, XmlReader reader, ParserPropertyDefinition prop, ParserClassDefinition cdef)

        /// <summary>
        /// Parses the simple element content of the reader and sets the value onto the containers property
        /// </summary>
        /// <param name="container"></param>
        /// <param name="reader"></param>
        /// <param name="prop"></param>
        /// <param name="cdef"></param>
        private void ParseSimpleElement(object container, XmlReader reader, ParserPropertyDefinition prop, ParserClassDefinition cdef)
        {
            LogAdd(reader, TraceLevel.Debug, "Parsing simple element '{0}' for property '{2}' on class '{3}'", reader.Name, prop.Name, prop.PropertyInfo.Name, prop.PropertyInfo.DeclaringType);
            
            if (reader.IsEmptyElement == false)
            {
                

                reader.Read();
                string value = reader.Value;
                
                IPDFBindingExpressionFactory factory;
                if (ParserHelper.IsBindingExpression(ref value, out factory, this.Settings))
                    GenerateBindingExpression(reader, container, cdef, prop, value, factory);
                else
                {
                    object converted = prop.GetValue(reader, this.Settings);
                    if (converted != DBNull.Value)
                        this.SetValue(container, converted, prop);
                }
            }
        }

        #endregion

        #region private void ParseInnerCollection(object container, XmlReader reader, ParserPropertyDefinition prop) + 1 overload

        /// <summary>
        /// Parses the inner collection of components in the current reader adding
        /// them to the collection of the container object in the specified property
        /// </summary>
        /// <param name="container"></param>
        /// <param name="reader"></param>
        /// <param name="prop"></param>
        private void ParseInnerCollection(object container, XmlReader reader, ParserPropertyDefinition prop)
        {
            if (reader.IsEmptyElement)
                return; //No inner content

            //Extract the end element names from the current node
            string endname = reader.LocalName;
            string endns = reader.NamespaceURI;

            //we want to read the inner content
            bool parseCurrentNode = false;

            ParseInnerCollection(container, endname, endns, parseCurrentNode, reader, prop);
        }

        /// <summary>
        /// Parses the inner collection of components in the current reader adding
        /// them to the collection of the container object in the specified property stopping at the specified end element
        /// </summary>
        /// <param name="container">The container that has the collection property to be populated.</param>
        /// <param name="endname">The name of the element to end parsing at</param>
        /// <param name="endns">The namespace of the element to end parsing at</param>
        /// <param name="parsecurrentNode">If true then the current none the reader is positioned at will be parsed (otherwise it will be sipped and the next node will start the parsing</param>
        /// <param name="reader">The current xml reader</param>
        /// <param name="prop">The property definition that refers to the collection to be added to by the parsed contents</param>
        private void ParseInnerCollection(object container, string endname, string endns, bool parsecurrentNode, XmlReader reader, ParserPropertyDefinition prop)
        {
            LogAdd(reader, TraceLevel.Debug, "Parsing inner collection from element '{0}' for property '{2}' on class '{3}'", reader.Name, prop.Name, prop.PropertyInfo.Name, prop.PropertyInfo.DeclaringType);
            bool lastwastext = false;
            StringBuilder textString = new StringBuilder();

            ParserArrayDefinition arraydefn = (ParserArrayDefinition)prop;


            object collection = InitArrayCollection(container, arraydefn);

            while (parsecurrentNode || reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (lastwastext)
                    {
                        AppendTextToCollection(textString, arraydefn, collection);
                        textString.Length = 0;
                        lastwastext = false;
                    }
                    object inner = this.ParseComponent(reader, false);
                    if (null != inner)
                    {
                        arraydefn.AddToCollection(collection, inner);
                    }
                }
                else if (reader.NodeType == XmlNodeType.EndElement)
                {
                    if (reader.LocalName == endname && reader.NamespaceURI == endns)
                        break;
                }
                else if (reader.NodeType == XmlNodeType.Text)
                {
                    string val = reader.Value;
                    val = System.Security.SecurityElement.Escape(val);

                    textString.Append(val);
                    lastwastext = true;
                }
                parsecurrentNode = false;
            }

            if (lastwastext && textString.Length > 0)
            {
                AppendTextToCollection(textString, arraydefn, collection);
            }
        }

        #endregion

        #region private void ParseComplexType(object container, XmlReader reader, ParserPropertyDefinition prop)

        /// <summary>
        /// Creates as required an instance of the property type, and assigns to the 
        /// containers property then parses the complex content of the new instance
        /// </summary>
        /// <param name="container"></param>
        /// <param name="reader"></param>
        /// <param name="prop"></param>
        private void ParseComplexType(object container, XmlReader reader, ParserPropertyDefinition prop)
        {
            ParserClassDefinition cdef = ParserDefintionFactory.GetClassDefinition(prop.ValueType);

            bool created = false;
            bool isremote;
            object value = this.GetValue(container, prop);
            if (null == value)
            {
                if (string.IsNullOrEmpty(prop.Name))
                {
                    //We are the default property so the inner element can be of a different type as declared
                    //and we create based on that, rather than the actual property type

                    string name = reader.LocalName;
                    string ns = reader.NamespaceURI;
                    if (!this.TryGetClassDefinition(reader, out cdef, out isremote))
                    {
                        if (this.Settings.ConformanceMode == ParserConformanceMode.Strict)
                            throw this.BuildParserXMLException(reader, Errors.NoPDFComponentDeclaredWithNameInNamespace, name, ns);
                        else
                        {
                            this.LogAdd(reader, TraceLevel.Error, Errors.NoPDFComponentDeclaredWithNameInNamespace, name, ns);
                            this.SkipOverCurrentElement(reader);
                            return;
                        }
                    }
                    else
                    {
                        value = this.CreateInstance(cdef);
                        created = true;
                    }
                }
                else
                {
                    value = this.CreateInstance(cdef);
                    created = true;
                }
            }
            this.ParseComplexComponentXml(value, reader, cdef);

            if (created)
                this.SetValue(container, value, prop);
        }

        #endregion

        #region private void ParseTemplateContent(object container, XmlReader reader, ParserPropertyDefinition prop)

        /// <summary>
        /// Creates a new template generator with the contents of the XmlReader inner xml 
        /// and then assigns this to the property of the container (which must be an IPDFComponent instance)
        /// </summary>
        /// <param name="container"></param>
        /// <param name="reader"></param>
        /// <param name="prop"></param>
        private void ParseTemplateContent(object container, XmlReader reader, ParserPropertyDefinition prop)
        {
            var temp = (ParserTemplateDefintion)prop;

            object gen = CreateInstance(this.Settings.TempateGeneratorType);
            var defn = ParserDefintionFactory.GetClassDefinition(this.Settings.TempateGeneratorType);

            if ((gen is IPDFTemplateGenerator) == false)
                throw new InvalidCastException("The returned type for the Settings.TemplateGeneratorType does not implement the IPDFTemplateGenerator interface");

            IPDFTemplateGenerator tempgen = (IPDFTemplateGenerator)gen;
            tempgen.ElementName = reader.Name;
            tempgen.IsBlock = temp.RenderAsBlock;

            if (reader.NodeType == XmlNodeType.Element && reader.HasAttributes)
            {
                bool skipUnknown = true;
                this.ParseAttributes(tempgen, false, reader, defn, skipUnknown);
                reader.MoveToElement();
            }
            
            string all = reader.ReadInnerXml();
            tempgen.InitTemplate(all, new XmlNamespaceManager(reader.NameTable));

            prop.PropertyInfo.SetValue(container, tempgen, null);
        }

        #endregion


        //
        // support methods
        //

        #region private void EnsureControllerInstance()

        /// <summary>
        /// If we have a controller type in the settings, then load the definition and instance.
        /// </summary>
        private void EnsureControllerInstance()
        {
            if (this.Settings.ControllerType != null)
            {
                this.ControllerDefinition = ParserDefintionFactory.GetControllerDefinition(this.Settings.ControllerType);

                if (null == this.Settings.Controller)
                    this.Controller = CreateInstance(this.Settings.ControllerType);
                else
                    this.Controller = this.Settings.Controller;

                this.UnassignedOutlets = new List<ParserControllerOutlet>(this.ControllerDefinition.Outlets);
            }
            else
            {
                this.ControllerDefinition = null;
                this.Controller = null;
                this.UnassignedOutlets = new List<ParserControllerOutlet>();
            }
        }

        #endregion

        #region private bool IsIDAttribute(string attrName, string prefix, string ns, ParserClassDefinition cdef)

        private bool IsIDAttribute(string attrName, string prefix, string ns, ParserClassDefinition cdef)
        {
            if (string.IsNullOrEmpty(prefix) && attrName == IDAttributeName)
                return true;
            else
                return false;
        }

        #endregion

        #region private void TryAssignToControllerOutlet(XmlReader reader, object instance, string outletName)

        private void TryAssignToControllerOutlet(XmlReader reader, object instance, string outletName)
        {
            ParserControllerOutlet outlet;
            if (this.HasController && this.ControllerDefinition.Outlets.TryGetOutlet(outletName, out outlet))
            {
                try
                {
                    outlet.SetValue(this.Controller, instance);
                    LogAdd(reader, TraceLevel.Verbose, "Assigned the component with id '{0}' to the declared outlet '{1}' on controller '{2}'", outletName, outlet.OutletMember.Name, this.Controller.ToString());
                }
                catch (Exception ex)
                {
                    if (outlet.Required == true || this.Mode == ParserConformanceMode.Strict)
                        throw BuildParserXMLException(ex, reader, "Could not assign the parsed component {0} with id '{1}' to the outlet '{2}' on the controller {3}", instance, outletName, outlet.OutletMember.Name, this.Controller);
                    else
                        LogAdd(reader, TraceLevel.Error, "Could not assign the parsed component {0} with id '{1}' to the outlet '{2}' on the controller {3} : {4}", instance, outletName, outlet.OutletMember.Name, this.Controller, ex);
                }

                //remove the assignement from the unassigned list
                this.UnassignedOutlets.Remove(outlet);
            }
            else
                LogAdd(reader, TraceLevel.Debug, "No controller outlet defined for component with id '{0}'", outletName);
        }

        #endregion

        #region  private void CheckFrameworkIsSupported(XmlReader reader, ParserClassDefinition cdef, ConformanceMode conformanceMode)

        /// <summary>
        /// Checks the Minimum and maximum required version. If not then the method either throws an exception, or 
        /// if the conformance mode is lax, then it will log an error to the trace, and skip over the entire current (unsupported) element
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="cdef"></param>
        /// <param name="conformanceMode"></param>
        private void CheckFrameworkIsSupported(XmlReader reader, ParserClassDefinition cdef, ParserConformanceMode conformanceMode)
        {
            if (cdef.IsMinFrameworkSupported == false)
            {
                string message = string.Format(Errors.ComponentCannotBeUsedBasedOnMinVersion, reader.Name, cdef.MinRequiredFramework, Scryber.Utilities.FrameworkHelper.CurrentVersion);
                if (conformanceMode == ParserConformanceMode.Strict)
                    throw BuildParserXMLException(reader, message);
                else
                {
                    LogAdd(reader, TraceLevel.Error, message);
                    SkipOverCurrentElement(reader);
                }
            }
            if (cdef.IsMaxFrameworkSupported == false)
            {
                string message = string.Format(Errors.ComponentCannotBeUsedBasedOnMaxVersion, reader.Name, cdef.MaxSupportedFramework, Scryber.Utilities.FrameworkHelper.CurrentVersion);
                if (conformanceMode == ParserConformanceMode.Strict)
                    throw BuildParserXMLException(reader, message);
                else
                {
                    LogAdd(reader, TraceLevel.Error, message);
                    SkipOverCurrentElement(reader);
                }

            }
        }

        #endregion

        #region private void SkipOverCurrentElement(XmlReader reader)

        /// <summary>
        /// Skips over all the inner content in an element
        /// </summary>
        /// <param name="reader"></param>
        private void SkipOverCurrentElement(XmlReader reader)
        {
            if (reader.NodeType != XmlNodeType.Element)
                throw new PDFParserException("Can only skip over the current element when the XmlReader is positioned on the actual element");

            if (reader.IsEmptyElement)
            {
                reader.ReadInnerXml();
            }
            else
            {
                string endname = reader.Name;
                string endns = reader.Prefix;
                do
                {
                    reader.Read();
                }
                while (!reader.EOF && (string.Equals(reader.Name,endname) == false) && (string.Equals(reader.Prefix,endns) == false) && (reader.NodeType == XmlNodeType.EndElement));
            }
        }

        #endregion

        #region private object ResolveAndParseRemoteComponent(XmlReader reader, string path, string select)

        /// <summary>
        /// Tries to resolve a remote reference based on a path and optional
        /// XPath select expression, and returns the result of that remote parsing
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="path"></param>
        /// <param name="select"></param>
        /// <returns></returns>
        private object ResolveAndParseRemoteComponent(XmlReader reader, string path, string select)
        {
            if(string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            object parsed = null;
            bool mylog = this.Settings.LogParserOutput;

            if (!string.IsNullOrEmpty(this.LoadedSourcePath))
                path = this.CombinePath(this.LoadedSourcePath, path);

            try
            {
                parsed = this.Settings.Resolver(path, select, this.Settings);
            }
            catch (System.IO.FileNotFoundException ex)
            {
                parsed = this.HandleRemoteReferenceException(reader, path, ex);
            }
            catch (System.Security.SecurityException ex)
            {
                parsed = this.HandleRemoteReferenceException(reader, path, ex);
            }
            catch (System.IO.DirectoryNotFoundException ex)
            {
                parsed = this.HandleRemoteReferenceException(reader, path, ex);
            }
            catch (System.IO.DriveNotFoundException ex)
            {
                parsed = this.HandleRemoteReferenceException(reader, path, ex);
            }
            catch (System.Net.WebException ex)
            {
                parsed = this.HandleRemoteReferenceException(reader, path, ex);
            }
            catch (PDFParserException)
            {
                throw;
            }

            this.Settings.LogParserOutput = mylog;

            return parsed;
        }

        private string CombinePath(string basePath, string localPath)
        {
            if (Uri.IsWellFormedUriString(localPath, UriKind.Absolute))
                return localPath;
            else if (Uri.IsWellFormedUriString(basePath, UriKind.Absolute) && Uri.IsWellFormedUriString(localPath, UriKind.Relative))
                return basePath + localPath;
            
            else if (System.IO.Path.IsPathRooted(localPath))
                return localPath;
            else
                return System.IO.Path.Combine(basePath, localPath);

        }

        #endregion

        #region private object HandleRemoteReferenceException(XmlReader reader, string path, Exception ex)

        /// <summary>
        /// Raises an approperties exception or logs an error if the missing reference action is to log (or just does nothing).
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="path"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        private object HandleRemoteReferenceException(XmlReader reader, string path, Exception ex)
        {
            object replacement = null;
            
            switch (this.Settings.MissingReferenceAction)
            {
                case ParserReferenceMissingAction.LogError:
                    this.LogAdd(reader, TraceLevel.Error, "Could not parse the file at path '" + path + "'. " + ex.Message);
                    break;
                case ParserReferenceMissingAction.DoNothing:
                    //Do nothing
                    break;
                case ParserReferenceMissingAction.RaiseException:
                default:
                    throw BuildParserXMLException(ex, reader, "Could not parse the file at path '" + path + "'. " + ex.Message);

            }
            return replacement;
        }

        #endregion

        #region private object CreateInheritedInstance(XmlReader reader, string typename, ParserClassDefinition cdef)

        /// <summary>
        /// Creates a new instance of the required typename Type, rather than the pre-defined type of the class definiton.
        /// The resultant type must inherit from the class definition type
        /// </summary>
        /// <param name="reader">The current xml reader</param>
        /// <param name="typename">The name of the runtime type to create an actual instace of.</param>
        /// <param name="cdef">The base type</param>
        /// <returns>The instantiated compoment</returns>
        private object CreateInheritedInstance(XmlReader reader, string typename, ParserClassDefinition cdef)
        {
            LogAdd(reader, TraceLevel.Debug, "Creating instance of inherited type '{0}'", typename);

            object created = null;

            if (string.IsNullOrEmpty(typename) == false)
            {
                Type found;
                try
                {
                    found = Type.GetType(typename, false);
                }
                catch (Exception ex)
                {
                    if (this.Mode == ParserConformanceMode.Strict)
                        throw BuildParserXMLException(ex, reader, Errors.CannotCreateInstanceOfType, typename, ex.Message);
                    else
                        LogAdd(reader, TraceLevel.Error, Errors.CannotCreateInstanceOfType, typename, ex.Message);
                    //Default to returning an instance of the base type
                    return this.CreateInstance(cdef);
                }

                LogAdd(reader, TraceLevel.Debug, "Loaded type from name");

                if (null == found || !cdef.ClassType.IsAssignableFrom(found))
                {
                    if (this.Mode == ParserConformanceMode.Strict)
                        throw BuildParserXMLException(reader, Errors.CannotCreateInstanceOfType, typename, "Cannot create or cast an object of type '" + typename + "'");
                    else
                        LogAdd(reader, TraceLevel.Error, Errors.CannotCreateInstanceOfType, typename, "Cannot create or cast an object of type '" + typename + "'");

                }
                else
                {
                    LogAdd(reader, TraceLevel.Debug, "Type can be converted to base type '" + cdef.ClassType.FullName);

                    try
                    {
                        created = System.Activator.CreateInstance(found);
                    }
                    catch (Exception ex)
                    {
                        if (this.Mode == ParserConformanceMode.Strict)
                            throw BuildParserXMLException(ex, reader, Errors.CannotCreateInstanceOfType, typename, ex.Message);
                        else
                            LogAdd(reader, TraceLevel.Error, Errors.CannotCreateInstanceOfType, typename, ex.Message);
                        //Default to returning an instance of the base type
                    }

                }
            }

            return created;

        }

        #endregion

        #region  private void TryAttachEventHandlerToController(XmlReader reader, object container, string methodname, ParserEventDefinition evt, ParserClassDefinition cdef)

        private const string EventInvokeMethodName = "Invoke";
        /// <summary>
        /// Dynamically creates a delegate for the event handler method, and attaches this to the event on the container instance
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="container"></param>
        /// <param name="methodname"></param>
        /// <param name="evt"></param>
        /// <param name="cdef"></param>
        private bool TryAttachEventHandlerToController(XmlReader reader, object container, string methodname, ParserEventDefinition evt, ParserClassDefinition cdef)
        {
            ParserControllerAction action;
            if (this.ControllerDefinition.Actions.TryGetAction(methodname, out action))
            {
                System.Reflection.MethodInfo declared = action.ActionMethod;
                Type evtType = evt.Event.EventHandlerType;
                Delegate del = Delegate.CreateDelegate(evtType, this.Controller, declared);
                bool success = false;
                try
                {
                    evt.Event.AddEventHandler(container, del);
                    success = true;
                }
                catch (Exception ex)
                {
                    if (this.Mode == ParserConformanceMode.Strict)
                        throw BuildParserXMLException(ex, reader, Errors.ParsedTypeDoesNotContainDefinitionFor, this.Controller.GetType().FullName, "PDFAction event handler", methodname);
                    else
                        LogAdd(reader, TraceLevel.Error, Errors.ParsedTypeDoesNotContainDefinitionFor + ": {3}", this.Controller.GetType().FullName, "PDFAction event handler method", methodname, ex);
                }
                return success;
            }
            else
            {
                if (this.Mode == ParserConformanceMode.Strict)
                    throw BuildParserXMLException(reader, Errors.ParsedTypeDoesNotContainDefinitionFor, this.Controller.GetType().FullName, "PDFAction event handler", methodname);
                else
                    LogAdd(reader, TraceLevel.Error, Errors.ParsedTypeDoesNotContainDefinitionFor, this.Controller.GetType().FullName, "PDFAction event handler method", methodname);

                return false;
            }
        }

        #endregion

        #region private bool IsSpecialAttribute(XmlReader reader, bool isremotecomponent)

        /// <summary>
        /// Returns true if the current attribute is a known special type (xmlns, file-path, select, or in xml schema namespace)
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="isremotecomponent"></param>
        /// <returns></returns>
        private bool IsSpecialAttribute(XmlReader reader, bool isremotecomponent, ParserClassDefinition cdef)
        {
            if (reader.Depth == 1)
            {
                if (reader.LocalName == InheritsAttributeName)
                    return true;
                else if (reader.LocalName == CodeBehindAttributeName)
                    return true;
                //check code-behind and inherits
            }
            if (reader.Name.ToLower() == "xmlns" || reader.Name.ToLower().StartsWith("xmlns:"))
                return true;
            else if (isremotecomponent && (reader.LocalName.ToLower() == FilePathAttributeName))
                return true;
            else if (isremotecomponent && (reader.LocalName.ToLower() == SelectAttributeName))
                return true;
            else if (isremotecomponent && reader.LocalName.ToLower() == cdef.RemoteSourceAttribute)
                return true;
            else if (reader.NamespaceURI == XmlSchemaNamespace) //is an attribute in the XML namespace
                return true;
            else
                return false;
        }

        #endregion

        #region private void GenerateBindingExpression(XmlReader reader, object container, ParserClassDefinition cdef, ParserPropertyDefinition prop, string expression, BindingType bindingtype)

        private void GenerateBindingExpression(XmlReader reader, object container, ParserClassDefinition cdef, ParserPropertyDefinition prop, string expression, IPDFBindingExpressionFactory factory)
        {
            using (var recorder = this.Settings.PerformanceMonitor.Record(PerformanceMonitorType.Expression_Build, factory.BindingKey))
            {
                if (factory.BindingStage == DocumentGenerationStage.Bound)
                {
                    if (container is IBindableComponent)
                        ((IBindableComponent)container).DataBinding += factory.GetDataBindingExpression(expression, cdef.ClassType, prop.PropertyInfo);
                    else
                        throw BuildParserXMLException(reader, Errors.DatabindingIsNotSupportedOnType, cdef.ClassType);
                }
                else if (factory.BindingStage == DocumentGenerationStage.Initialized)
                {
                    if (container is IComponent)
                        ((IComponent)container).Initialized += factory.GetInitBindingExpression(expression, cdef.ClassType, prop.PropertyInfo);
                    else
                        throw BuildParserXMLException(reader, Errors.BindingIsNotSupportedOnType, cdef.ClassType, DocumentGenerationStage.Initialized.ToString(), "IPDFComponent");
                }
                else if (factory.BindingStage == DocumentGenerationStage.Loaded)
                {
                    if (container is IComponent)
                        ((IComponent)container).Loaded += factory.GetLoadBindingExpression(expression, cdef.ClassType, prop.PropertyInfo);
                    else
                        throw BuildParserXMLException(reader, Errors.BindingIsNotSupportedOnType, cdef.ClassType, DocumentGenerationStage.Initialized.ToString(), "IPDFComponent");
                }
                else
                    throw new NotSupportedException(factory.BindingStage.ToString());
            }
            //if (bindingtype == BindingType.XPath)
            //{
            //    GenerateXPathBindingExpression(reader, container, cdef, prop, expression, bindingtype);
            //}
            //else if (bindingtype == BindingType.Code)
            //{
            //    throw new NotSupportedException("No code type supported");
            //}
            //else if (bindingtype == BindingType.QueryString)
            //{
            //    GenerateQueryStringBindingExpression(reader, container, cdef, prop, expression, bindingtype);
            //}
            //else if (bindingtype == BindingType.ContextItem)
            //{
            //    GenerateContextItemBindingExpression(reader, container, cdef, prop, expression, bindingtype);
            //}
            //else
            //    throw new ArgumentOutOfRangeException("bindingtype");
        }

        //private void GenerateXPathBindingExpression(XmlReader reader, object container, ParserClassDefinition cdef, ParserPropertyDefinition attr, string expression, BindingType bindingtype)
        //{
        //    //throw new NotImplementedException();
        //    if (container is IPDFBindableComponent)
        //    {
        //        try
        //        {
        //            IPDFBindableComponent bindable = (IPDFBindableComponent)container;
        //            PDFValueConverter conv = this.GetValueConverter(reader, attr);
        //            BindingXPathExpression binding = BindingXPathExpression.Create(expression, conv, attr.PropertyInfo);
        //            bindable.DataBinding += new PDFDataBindEventHandler(binding.BindComponent);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw BuildParserXMLException(ex, reader, "A new binding expression could not be created on container " + container.ToString());
        //        }
        //    }
        //    else
        //        throw BuildParserXMLException(reader, Errors.DatabindingIsNotSupportedOnType, cdef.ClassType.FullName);
        //}

        //private void GenerateQueryStringBindingExpression(XmlReader reader, object container, ParserClassDefinition cdef, ParserPropertyDefinition attr, string expression, BindingType bindingtype)
        //{
        //    if (container is IPDFComponent)
        //    {
        //        IPDFComponent bindable = (IPDFComponent)container;
        //        PDFValueConverter conv = this.GetValueConverter(reader, attr);
        //        BindingQueryStringExpression binding = BindingQueryStringExpression.Create(expression, conv, attr.PropertyInfo);
        //        bindable.Initialized += new PDFInitializedEventHandler(binding.InitComponent);
        //    }
        //}

        //private void GenerateContextItemBindingExpression(XmlReader reader, object container, ParserClassDefinition cdef, ParserPropertyDefinition attr, string expression, BindingType bindingtype)
        //{
        //    if (container is IPDFBindableComponent)
        //    {
        //        try
        //        {
        //            IPDFBindableComponent bindable = (IPDFBindableComponent)container;
        //            BindingItemExpression binding = BindingItemExpression.Create(expression, attr.PropertyInfo);
        //            bindable.DataBinding += new PDFDataBindEventHandler(binding.BindComponent);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw BuildParserXMLException(ex, reader, "A new binding expression could not be created on container " + container.ToString());
        //        }
        //    }
        //}

        #endregion

        #region private PDFValueConverter GetValueConverter(XmlReader reader, ParserPropertyDefinition attr)

        /// <summary>
        /// Retuns a PDFValueConverter that can convert a string value to the required type value
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        private ValueConverter GetValueConverter(XmlReader reader, ParserPropertyDefinition attr)
        {
            ValueConverter valueconverter;
            if (ParserDefintionFactory.IsSimpleObjectType(attr.ValueType, out valueconverter))
            {
                return valueconverter;
            }
            else if (ParserDefintionFactory.IsCustomParsableObjectType(attr.ValueType, out valueconverter))
            {
                return valueconverter;
            }
            else
                throw BuildParserXMLException(reader, Errors.ParserAttributeMustBeSimpleOrCustomParsableType, attr.Name, attr.ValueType);

        }

        #endregion

        #region private void AppendTextToCollection(StringBuilder textString, ParserArrayDefinition arraydefn, object collection)

        /// <summary>
        /// Appends a blob of text contained in the StringBuilder to the current collection property specified.
        /// </summary>
        /// <param name="textString">The text that needs to be appended</param>
        /// <param name="arraydefn">The property definition of the collection</param>
        /// <param name="collection">The instance which has the property to add the text component to</param>
        private void AppendTextToCollection(StringBuilder textString, ParserArrayDefinition arraydefn, object collection)
        {
            TextFormat format = TextFormat.XML;
            
            //Add any bindings to the text
            string textSubString = textString.ToString();

            string[] splits;
            IPDFBindingExpressionFactory[] factories;

            if(ParserHelper.ContainsBindingExpressions(textSubString, out splits, out factories))
            {
                for (int i = 0; i < splits.Length; i++)
                {
                    var content = splits[i];
                    var factory = factories[i];
                    if(null == factory)
                    {
                        AddTextString(content, arraydefn, collection, format);
                    }
                    else
                    {
                        AddTextBindingExpression(content, arraydefn, collection, factory, format);
                    }
                }
            }
            else
            {
                AddTextString(textSubString, arraydefn, collection, format);
            }
            
        }

        private void AddTextBindingExpression(string expr, ParserArrayDefinition arraydefn, object collection, IPDFBindingExpressionFactory factory, TextFormat format)
        {
            Type literaltype = this.Settings.TextLiteralType;

            IPDFTextLiteral lit = (IPDFTextLiteral)CreateInstance(literaltype);

            IBindableComponent bindable = (IBindableComponent)lit;

            lit.ReaderFormat = format;
            arraydefn.AddToCollection(collection, lit);

            var cdef = this.AssertGetClassDefinition(this.Settings.TextLiteralType);
            var prop = cdef.Attributes["value"];

            GenerateBindingExpression(null, lit, cdef, prop, expr, factory);

        }

        private void AddTextString(string text, ParserArrayDefinition arraydefn, object collection, TextFormat format)
        {
            Type literaltype = this.Settings.TextLiteralType;
            IPDFTextLiteral literal = (IPDFTextLiteral)CreateInstance(literaltype);
            
            literal.Text = text;
            literal.ReaderFormat = format;

            if (this.Settings.TraceLog.ShouldLog(TraceLevel.Debug))
            {
                if (text.Length > 60)
                    text = text.Substring(0, 30) + "..." + text.Substring(text.Length - 30);

                this.LogAdd(null, TraceLevel.Debug, "Generated text literal component with text '{0}' in format {1}", text, format);
            }

            arraydefn.AddToCollection(collection, literal);
        }

        #endregion


        #region private object InitArrayCollection(object container, ParserArrayDefinition arraydefn)

        /// <summary>
        /// Initializes if required, a new collection instance for the container.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="arraydefn"></param>
        /// <returns></returns>
        /// <remarks> If the container instance property value is not null, then it's actual value is retrund. 
        /// If it is null, then a new instance of the required type is created, and then set to the property before returning</remarks>
        private object InitArrayCollection(object container, ParserArrayDefinition arraydefn)
        {
            object collection = arraydefn.PropertyInfo.GetValue(container, null);
            if (null == collection)
            {
                LogAdd(null, TraceLevel.Debug, "Collection from property is null - creating instance of type '{0}'", arraydefn.ValueType);
                collection = arraydefn.CreateInstance();
                this.SetValue(container, collection, arraydefn);
            }
            return collection;
        }

        #endregion

        #region private bool TryGetClassDefinition(XmlReader reader, out ParserClassDefinition cdef, out bool isremote)

        /// <summary>
        /// Attempats to retrieve the class definition of the element the XmlReader is currently positioned on. 
        /// Setting isremote to true if the reference is to a remote component.
        /// If the definition cannot be found, then the method returns false.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="cdef"></param>
        /// <param name="isremote"></param>
        /// <returns></returns>
        private bool TryGetClassDefinition(XmlReader reader, out ParserClassDefinition cdef, out bool isremote)
        {
            LogAdd(reader, TraceLevel.Debug, "Looking for PDFComponent declared with local name '{0}' and namespace '{1}'", reader.LocalName, reader.NamespaceURI);
            var ns = reader.NamespaceURI;
            if(!string.IsNullOrEmpty(ns))
            {
                ns = ns.Trim();
                if (ns.EndsWith(";"))
                    ns = ns.Substring(0, ns.Length - 1);
            }
            cdef = ParserDefintionFactory.GetClassDefinition(reader.LocalName, ns, false, out isremote);

            if (null == cdef)
            {
                LogAdd(reader, TraceLevel.Error, Errors.NoTypeFoundWithPDFComponentNameInNamespace, reader.LocalName, reader.NamespaceURI);
                return false;
            }
            else
                return true;

        }

        #endregion

        #region private ParserClassDefinition AssertGetClassDefinition(XmlReader reader, out bool isremote)

        /// <summary>
        /// Attempts to retrieve the class definition of the element the XmlReader is currently positioned on. 
        /// Setting isremote to true if the reference is to a remote component.
        /// If the definition cannot be found then an exception is raised.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="isremote"></param>
        /// <returns></returns>
        private ParserClassDefinition AssertGetClassDefinition(XmlReader reader, out bool isremote)
        {
            LogAdd(reader, TraceLevel.Debug, "Looking for PDFComponent declared with local name '{0}' and namespace '{1}'", reader.LocalName, reader.NamespaceURI);
            
            ParserClassDefinition cdef = ParserDefintionFactory.GetClassDefinition(reader.LocalName, reader.NamespaceURI, true, out isremote);

            if (null == cdef)
                throw this.BuildParserXMLException(reader, Errors.NoTypeFoundWithPDFComponentNameInNamespace, reader.LocalName, reader.NamespaceURI);
            return cdef;
        }

        
        /// <summary>
        /// Attempts to retrieve the class definition of the element the XmlReader is currently positioned on. 
        /// Setting isremote to true if the reference is to a remote component.
        /// If the definition cannot be found then an exception is raised.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="isremote"></param>
        /// <returns></returns>
        private ParserClassDefinition AssertGetClassDefinition(Type type)
        {
            
            ParserClassDefinition cdef = ParserDefintionFactory.GetClassDefinition(type);

            if (null == cdef)
                throw new PDFParserException("Parser definition could not be found for type '" + type.FullName + "'");
            return cdef;
        }

        #endregion

        #region private object GetValue(object instance, ParserPropertyDefinition property)

        /// <summary>
        /// Gets the current value for the property of the specified instance
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        private object GetValue(object instance, ParserPropertyDefinition property)
        {
            return property.PropertyInfo.GetValue(instance, null);
        }

        #endregion

        #region private void SetValue(object instance, object value, ParserPropertyDefinition property)

        /// <summary>
        /// Sets the value of the property on the instance to the specified value
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="value"></param>
        /// <param name="property"></param>
        private void SetValue(object instance, object value, ParserPropertyDefinition property)
        {
            if (property.PropertyInfo.CanWrite)
            {
                try
                {
                    property.PropertyInfo.SetValue(instance, value, null);
                    LogAdd(null, TraceLevel.Debug, "Set value of property '{0}' on class {1} to '{2}'", property.Name, property.PropertyInfo.DeclaringType, value);

                }
                catch (Exception ex)
                {
                    if (this.Mode == ParserConformanceMode.Strict)
                        throw new PDFParserException(string.Format("Could not set the property '{0}' on class {1}: {2}" , property.Name, property.PropertyInfo.DeclaringType, ex.Message), ex);
                    else
                        LogAdd(null, TraceLevel.Error, string.Format("Could not set the property '{0}' on class {1}: {2}", property.Name, property.PropertyInfo.DeclaringType, ex.Message), ex);
                }
            }
            else
                throw new PDFParserException(String.Format("The property '{0}' on class '{1}' cannot be written to. It does not have a set method.", property.Name, property.PropertyInfo.DeclaringType.Name));
            
        }

        #endregion

        //
        // Logging
        //

        #region protected void LogAdd(XmlReader reader, TraceLevel level, string message, params object[] args)

        /// <summary>
        /// Adds the (formatted) message to the current trace log - appending line number and position if available
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="level"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        protected void LogAdd(XmlReader reader, TraceLevel level, string message, params object[] args)
        {
            if (this.Settings.LogParserOutput && this.Settings.TraceLog.ShouldLog(level))
            {
                if (reader != null && reader is IXmlLineInfo)
                {
                    IXmlLineInfo li = reader as IXmlLineInfo;
                    message = message + " [line " + li.LineNumber + ", pos " + li.LinePosition + "]";
                }
                this.Settings.TraceLog.Add(level, ParserLogCategory, SafeFormat(message, args));
            }
        }

        #endregion

        #region protected void LogBegin(TraceLevel level, string message, params object[] args)

        /// <summary>
        /// Begins a new logging group to the current log with the safe formatted message.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        protected void LogBegin(TraceLevel level, string message, params object[] args)
        {
            if (this.Settings.LogParserOutput && this.Settings.TraceLog.ShouldLog(level))
            {
                this.Settings.TraceLog.Begin(level, ParserLogCategory, SafeFormat(message, args));
            }
        }

        #endregion

        #region protected void LogEnd(TraceLevel level, string message, params object[] args)

        /// <summary>
        /// Ends the current logging group appending the (save formatted) message in the process
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        protected void LogEnd(TraceLevel level, string message, params object[] args)
        {
            if (this.Settings.LogParserOutput && this.Settings.TraceLog.ShouldLog(level))
            {
                this.Settings.TraceLog.End(level, ParserLogCategory, SafeFormat(message, args));
            }
        }

        #endregion

        #region private string SafeFormat(string message, object[] args)

        /// <summary>
        /// Formats the message with the arguments, but if an excpetion 
        /// is thrown with the format it is sunk and the original message unformatted is returned.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private string SafeFormat(string message, object[] args)
        {
            if (string.IsNullOrEmpty(message))
                return string.Empty;
            else if (null == args || args.Length == 0)
                return message;
            else
            {
                try
                {
                    string formatted = string.Format(message, args);
                    message = formatted;
                }
                catch (Exception)
                {
                    //Intentional sinking of exception
                }
                return message;
            }
        }

        #endregion

        #region private Scryber.Logging.PDFCollectorTraceLog InjectCollectorLog()

        /// <summary>
        /// Injects the collector log into the current log
        /// </summary>
        /// <returns></returns>
        private void InjectCollectorLog()
        {
            if (this.Settings.TraceLog != null && this.Settings.TraceLog.GetLogWithName(TraceLog.ScryberAppendTraceLogName) == null)
            {
                Scryber.Logging.CollectorTraceLogFactory factory = new Logging.CollectorTraceLogFactory();

                Scryber.Logging.CollectorTraceLog coll = (Scryber.Logging.CollectorTraceLog)factory.CreateLog(this.Settings.TraceLog.RecordLevel, TraceLog.ScryberAppendTraceLogName);
                if (null == this.Settings.TraceLog)
                    this.Settings.TraceLog = coll;
                else
                {
                    Scryber.Logging.CompositeTraceLog comp = new Logging.CompositeTraceLog(new TraceLog[] { this.Settings.TraceLog, coll }, string.Empty);
                    this.Settings.TraceLog = comp;
                }
            }
        }

        #endregion

        //
        // processing instructions
        //

        #region private void ParseProcessingInstructions(XmlReader reader)

        /// <summary>
        /// Parses the scryber processing instructions
        /// </summary>
        /// <param name="reader"></param>
        protected void ParseProcessingInstructions(XmlReader reader)
        {
            string value = reader.Value;

            this.Settings.ReadProcessingInstructions(value);

            LogAdd(reader, TraceLevel.Message, "Parsed processing instructions. Controller = {5}, parser mode = {1}, logging = {2}, append-log = {3}, log-level = {4}", 
                value, this.Mode, this.Settings.LogParserOutput, this.Settings.AppendLog, this.Settings.TraceLog.RecordLevel, this.Settings.ControllerType == null? "[NONE]": this.Settings.ControllerType.ToString());
        }

        #endregion


        //
        // object activation
        //

        #region internal object CreateInstance(ParserClassDefinition cdef)

        /// <summary>
        /// Create a new instance of the type based on the class definition
        /// </summary>
        /// <param name="cdef"></param>
        /// <returns></returns>
        internal object CreateInstance(ParserClassDefinition cdef)
        {
            if (null == cdef)
                throw new ArgumentNullException("cdef");
            
            return CreateInstance(cdef.ClassType);
        }

        #endregion

        #region internal object CreateInstance(Type type)

        /// <summary>
        /// Creates a new instace of the specified type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal object CreateInstance(Type type)
        {
            if (null == type)
                throw new ArgumentNullException("type");
            try
            {
                return Activator.CreateInstance(type);
            }
            catch (Exception ex)
            {
                throw new PDFParserException(String.Format(Errors.CannotCreateInstanceOfType, type, ex.Message), ex);
            }
        }

        #endregion

        //
        // namespace registration
        //

        #region private void RegisterNamespaceDeclaration(XmlReader reader)

        /// <summary>
        /// Registed the current xmlns namespace with this parser so they can all be collected together.
        /// </summary>
        /// <param name="reader"></param>
        private void RegisterNamespaceDeclaration(XmlReader reader)
        {
            string prefix;
            string value;

            if (reader.Prefix == "xmlns")
            {
                prefix = reader.LocalName;
                value = reader.Value;
                this.RegisterNamespaceDeclaration(prefix, value);
            }
            else if (reader.Name == "xmlns")
            {
                //default namespace
                prefix = string.Empty;
                value = reader.Value;
                this.RegisterNamespaceDeclaration(prefix, value);
            }
        }

        #endregion

        #region private void EnsureNamespaceRegistered(string prefix, string ns)

        /// <summary>
        /// Makes sure the specified prefix and namespace are registered in this parser
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="ns"></param>
        private void EnsureNamespaceRegistered(string prefix, string ns)
        {
            string found;

            if (!this._nsDeclarations.TryGetValue(prefix, out found))
                _nsDeclarations.Add(prefix, ns);

        }

        #endregion

        #region private void RegisterNamespaceDeclaration(string prefix, string ns)

        /// <summary>
        /// Registers the prefix with the associated namesapce in this parser
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="ns"></param>
        private void RegisterNamespaceDeclaration(string prefix, string ns)
        {
            if (null == prefix)
                prefix = string.Empty;
            this._nsDeclarations[prefix] = ns;
        }

        #endregion

        //
        // exception
        //

        #region private Exception BuildParserXMLException(XmlReader reader, string msg, params object[] args)

        /// <summary>
        /// Creates and returns a new PDFParserException
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="msg"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        protected Exception BuildParserXMLException(XmlReader reader, string msg, params object[] args)
        {
            return BuildParserXMLException(null, reader, msg, args);
        }

        #endregion

        #region protected Exception BuildParserXMLException(Exception inner, XmlReader reader, string msg, params object[] args)

        /// <summary>
        /// Creates and returns a new PDFParser excption that can be thrown
        /// </summary>
        /// <param name="inner"></param>
        /// <param name="reader"></param>
        /// <param name="msg"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        protected Exception BuildParserXMLException(Exception inner, XmlReader reader, string msg, params object[] args)
        {
            if (null != reader && (reader is IXmlLineInfo))
            {
                IXmlLineInfo li = (IXmlLineInfo)reader;
                msg = string.Format(msg, args) + " [line " + li.LineNumber + ", pos " + li.LinePosition + "]";
            }
            else
                msg = string.Format(msg, args);

            return new PDFParserException(msg, inner);
        }

        #endregion
    }
}
