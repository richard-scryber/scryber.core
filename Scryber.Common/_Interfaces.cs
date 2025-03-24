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
using System.Threading.Tasks;
using Scryber.Logging;
using Scryber.PDF.Resources;

namespace Scryber
{
    #region public interface ITypedObject

    /// <summary>
    /// Implementors must have an explicit PDFObjectType
    /// </summary>
    public interface ITypedObject
    {
        /// <summary>
        /// Gets the name for the object type
        /// </summary>
        ObjectType Type { get; }

    }

    #endregion


    #region public interface IPDFTemplate

    /// <summary>
    /// Interface for a class that supports the instantiation of one or more copies its own content into the container
    /// </summary>
    public interface ITemplate
    {
        /// <summary>
        /// Creates a copy of any content of this template in the specified container
        /// </summary>
        /// <param name="index">The current index of the instantiation</param>
        /// <param name="owner">The owner of this template</param>
        IEnumerable<IComponent> Instantiate(int index, IComponent owner);
    }

    #endregion

    

    #region public interface IComponent : ITypedObject

    /// <summary>
    /// Interface that complex objects should support, including initialization and disposal
    /// </summary>
    public interface IComponent : ITypedObject, IDisposable
    {
        /// <summary>
        /// Event that is raised when the object is initialized
        /// </summary>
        event InitializedEventHandler Initialized;

        /// <summary>
        /// Event that is raised when the object is loaded
        /// </summary>
        event LoadedEventHandler Loaded;

        /// <summary>
        /// Initializes a PDFComponent
        /// </summary>
        void Init(InitContext context);

        /// <summary>
        /// Loads the component.
        /// </summary>
        /// <param name="context"></param>
        void Load(LoadContext context);

        /// <summary>
        /// Gets or sets the ID of the component
        /// </summary>
        string ID { get; set; }

        /// <summary>
        /// Gets or sets the name of the element that this component was parsed from
        /// </summary>
        string ElementName { get; set; }

        /// <summary>
        /// Gets the document that contains this PDFComponent, and forms the root of the PDF Component hierarchy
        /// </summary>
        IDocument Document { get; }

        /// <summary>
        /// Gets or sets the containing parent of this PDFComponent
        /// </summary>
        IComponent Parent { get; set; }

        /// <summary>
        /// Returns the full path to a file relative to the component or its contianer(s).
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        string MapPath(string source);

    }

    #endregion

    #region public interface IComponentWrappingList

    /// <summary>
    /// Interface for lists that wrap IComponentLists for strongly typing
    /// </summary>
    public interface IComponentWrappingList
    {
        IComponentList InnerList { get; }
    }

    #endregion

    #region public interface IComponentList : ICollection<IComponent>

    /// <summary>
    /// A List of IPDFComponents - ICollection interface and Insert
    /// </summary>
    public interface IComponentList : ICollection<IComponent>
    {
        void Insert(int index, IComponent component);
    }

    #endregion

    #region public interface ILoadableComponent : IComponent

    /// <summary>
    /// Defines a component interface that can be remotely loaded and the FileSource set as 
    /// the path the file was loaded from
    /// </summary>
    public interface ILoadableComponent : IComponent
    {
        /// <summary>
        /// Gets or sets the full path to the file the component was loaded from
        /// </summary>
        string LoadedSource { get; set; }

        /// <summary>
        /// Gets or sets the way the component was loaded
        /// </summary>
        ParserLoadType LoadType { get; set; }
    }

    #endregion

    #region public interface IRemoteComponent : ILoadableComponent
    /// <summary>
    /// Defines a component that can be parsed from a remote XML file, extending the Loadable 
    /// component with the namespace declarations in the XML and any 
    /// parsed items that need to be passed into it.
    /// </summary>
    public interface IRemoteComponent : ILoadableComponent
    {
        /// <summary>
        /// Gets the Items defined and associated with this remote component
        /// </summary>
        ItemCollection Params { get; }

        /// <summary>
        /// Should return true if this remote component has one or more items declared on it.
        /// </summary>
        bool HasParams { get; }

        /// <summary>
        /// Registers a specific xml namespace that was declared on the parsed source.
        /// </summary>
        /// <param name="prefix">The prefix associated with the namespace</param>
        /// <param name="namepace">The actual namespace</param>
        void RegisterNamespaceDeclaration(string prefix, string namepace);


        /// <summary>
        /// Gets a hash table of all the namespaces declared for this remote component as prefix to namespace.
        /// </summary>
        /// <returns></returns>
        IDictionary<string, string> GetDeclaredNamespaces();

    }

    #endregion

    #region public interface IBindableComponent

    /// <summary>
    /// Interface that identifies the Databinding features of an Component
    /// </summary>
    public interface IBindableComponent
    {
        /// <summary>
        /// Event that is raised before an Component is databound
        /// </summary>
        event DataBindEventHandler DataBinding;
        /// <summary>
        /// Event that is raised after an object has been databound
        /// </summary>
        event DataBindEventHandler DataBound;

        /// <summary>
        /// Databinds a PFComponent
        /// </summary>
        void DataBind(DataContext context);
    }

    #endregion

    #region public interface ICountableComponent

    public interface ICountableComponent : IComponent
    {
        bool HasCounters { get; }

        ComponentCounterSet Counters { get; }
    }

    #endregion

    #region public interface IOptimizeComponent 

    /// <summary>
    /// Interface for the compression flag
    /// </summary>
    public interface IOptimizeComponent : IComponent
    {
        bool Compress { get; set; }
    }

    #endregion

    #region public interface INamingContainer

    /// <summary>
    /// Placeholder interface to identify instances that are included in creating a unique ID
    /// </summary>
    public interface INamingContainer
    {
        IComponent Owner { get; set; }
    }

    #endregion

    #region public interface IPathMappingService

    public interface IPathMappingService
    {
        string MapPath(ParserLoadType loadtype, string reference, string parent, out bool isFile);
    }

    #endregion

    /// <summary>
    /// Interface for the configuration service
    /// </summary>
    public interface IScryberConfigurationService
    {
        Options.FontOptions FontOptions { get; }

        Options.ParsingOptions ParsingOptions { get; }

        Options.ImagingOptions ImagingOptions { get; }

        Options.OutputOptions OutputOptions { get; }

        Options.TracingOptions TracingOptions { get; }

        object GetScryberSection(Type ofType, string name);

        void Reset();
    }

    /// <summary>
    /// Service for getting a caching provider
    /// </summary>
    public interface IScryberCachingServiceFactory
    {
        ICacheProvider GetProvider();
    }

    #region public interface ICacheProvider

    /// <summary>
    /// Defines the contract all CacheProviders must conform to 
    /// in order to support access to the data cache
    /// </summary>
    public interface ICacheProvider
    {
        bool TryRetrieveFromCache(string type, string key, out object data);

        void AddToCache(string type, string key, object data);

        void AddToCache(string type, string key, object data, TimeSpan duration);

        void AddToCache(string type, string key, object data, DateTime expires);

    }

    #endregion

    #region public interface IDocumentPageContainer

    /// <summary>
    /// Interface that all containers of document pages should support so the pages they contain can be extracted
    /// </summary>
    public interface IDocumentPageContainer
    {
        IDocumentPage[] AllPages { get; }
    }

    #endregion

    #region public interface IDocument : ILoadableComponent

    /// <summary>
    /// Top Level document interface - supports resourses and componentID's
    /// </summary>
    public interface IDocument : ILoadableComponent, IDocumentPageContainer
    {

        /// <summary>
        /// Loads a specific PDFResource based on the requested resource type and key
        /// </summary>
        /// <param name="type">The resource type</param>
        /// <param name="key">The resource key </param>
        /// <returns></returns>
        ISharedResource GetResource(string type, string key, bool create);

        /// <summary>
        /// Ensures that the provided resource is registered in the documents
        /// shared resources, and if not, then it is added and a resource reference returned.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="key">The indentifying key for the resource that can be used to retrieve again</param>
        /// <param name="resource">This is the actual resource itself</param>
        /// <returns></returns>
        ISharedResource EnsureResource(string type, string key, object resource);

        /// <summary>
        /// Returns a document unique identifier for a particular object type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        string GetIncrementID(ObjectType type);

        /// <summary>
        /// Returns any component registered with the document with a specific ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IComponent FindComponentById(string id);

        /// <summary>
        /// Gets the current conformance mode (strict or lax)
        /// </summary>
        ParserConformanceMode ConformanceMode { get; }

        /// <summary>
        /// Gets the trace log associated with the current execution
        /// </summary>
        TraceLog TraceLog { get; }


    }

    #endregion

    #region public interface IResourceContainer

    /// <summary>
    /// Interface for any items that hold a collection of resources
    /// </summary>
    public interface IResourceContainer
    {

        IDocument Document { get; }
        
        PDFResourceList Resources { get; }

        string Register(ISharedResource rsrc);

        string MapPath(string source);

    }

    #endregion

    #region public interface ISharedResource
    
    public interface ISharedResource
    {
        /// <summary>
        /// Gets the type of this resource
        /// </summary>
        string ResourceType { get; }

        /// <summary>
        /// Gets the unique key of this resource within the document
        /// </summary>
        string ResourceKey { get; }

        /// <summary>
        /// Gets the container for this resource
        /// </summary>
        IResourceContainer Container { get; }

        bool Registered { get; }
    }
    
    #endregion

    #region public interface IParsedDocument : IDocument

    /// <summary>
    /// Defines the contract for a component so that the XmlParser can interact with it.
    /// </summary>
    /// <remarks>
    /// During the parsing the XMLParser will encounter pre</remarks>
    public interface IParsedDocument : IDocument
    {
        /// <summary>
        /// Set to true if the document should append a trace log table after the document has been generated.
        /// </summary>
        bool AppendTraceLog { get; set; }

        

        /// <summary>
        /// Gets or sets the performance monitor instance that the document will use to capture performance statistics
        /// </summary>
        Scryber.Logging.PerformanceMonitor PerformanceMonitor { get; set; }

        /// <summary>
        /// Sets the current conformance mode (strict or lax)
        /// </summary>
        void SetConformanceMode(ParserConformanceMode mode);

        /// <summary>
        /// Gets the trace log associated with the current execution
        /// </summary>
        void SetTraceLog(TraceLog log);

    }

    #endregion
    
    #region public interface IControlledComponent : IRemoteComponent

    /// <summary>
    /// Defines an interface where the remote component can have a controller assigned, and the XML parser can hook up outlets and actions.
    /// </summary>
    public interface IControlledComponent : IRemoteComponent
    {
        /// <summary>
        /// Gets or sets the controller for this remote component.
        /// </summary>
        object Controller { get; set; }
    }

    #endregion

    public interface IRemoteRequest
    {
        event RequestCompletedEventHandler Completed;

        string ResourceType { get; }
        
        string FilePath { get; }  
        
        IComponent Owner { get; }
        
        object Arguments { get; }
        
        RemoteRequestCallback Callback { get; }
        
        Exception Error { get; }
        
        object Result { get; }
        
        bool IsCompleted { get; }
        
        bool IsSuccessful { get; }

        void CompleteRequest(object cachableResult, bool success, Exception error);
        
    }
    
    /// <summary>
    /// Defines the interface that the supports making requests for external content, returned as a stream to the callback method.
    /// </summary>
    public interface IResourceRequester
    {
        IRemoteRequest RequestResource(string type, string path, RemoteRequestCallback callback, IComponent owner, object arguments);
    }


    #region public interface IDocumentPage

    /// <summary>
    /// Interface that all document pages should support so they can be contained in the IDocument interface
    /// </summary>
    public interface IDocumentPage : IResourceContainer, IComponent
    {
        
    }

    #endregion

    

    #region public interface IDataSource

    /// <summary>
    /// Defines the interface that all controls must implement if 
    /// they want to be used as a source of data within the page.
    /// </summary>
    public interface IDataSource : IComponent
    {
        /// <summary>
        /// If true then the data source can be interrogated to get the data schema for runtime analysis
        /// of results.
        /// </summary>
        bool SupportsDataSchema { get; }

        /// <summary>
        /// Returns a PDFDataSchema that describes the underlying returned data.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="context"></param>
        /// <returns>A PDFDataSchema that describes the underlying data structure at runtime.</returns>
        /// <exception cref="Scryber.Data.PDFException" >Thrown if this source does not support data schema analysis</exception>
        /// <exception cref="System.ArgumentException" >Thrown if the provided path is not a valid xpath</exception>
        DataSchema GetDataSchema(string path, DataContext context);

        /// <summary>
        /// Performs the selection of data configured on this source
        /// </summary>
        /// <param name="path">The optional restriction.</param>
        /// <param name="context"></param>
        /// <returns>The top level data associated with this DataSource control.</returns>
        object Select(string path, DataContext context);

        /// <summary>
        /// Performs a select of the provided data based on the path in the current data context
        /// </summary>
        /// <param name="path">The path to select the data from</param>
        /// <param name="current">The current data to select the data from</param>
        /// <param name="context">The current data context</param>
        /// <returns>The </returns>
        object Select(string path, object current, DataContext context);

        /// <summary>
        /// Performs a test of the provided expression against the provided data in the current context
        /// </summary>
        /// <param name="expr"></param>
        /// <param name="withData"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        bool EvaluateTestExpression(string expr, object withData, DataContext context);


        /// <summary>
        /// Performs an evaluate of the provided data based on the path and the current data context
        /// </summary>
        /// <param name="expr"></param>
        /// <param name="withData"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        object Evaluate(string expr, object withData, DataContext context);

    }

    #endregion

    #region public interface IKeyValueProvider

    /// <summary>
    /// Defines an open interface for returning an object associated with a key
    /// </summary>
    public interface IKeyValueProvider
    {
        string ID { get; }

        object GetNativeValue(string key, IComponent comp);

        void SetValue(string key, string value, IComponent owner);

        void SetNativeValue(string key, object value, IComponent owner);

        void Init();
    }

    #endregion

    #region public interface IPDFTraceLogFactory

    /// <summary>
    /// Contract that the trace log factories, defined in the configuration file must support to create new loggers
    /// </summary>
    public interface IPDFTraceLogFactory
    {
        TraceLog CreateLog(TraceRecordLevel level, string name);
    }

    #endregion

    #region public interface IPDFBindingExpressionFactory

    /// <summary>
    /// Interface that all binding expression builders must conform to so they can be used by the parser to 
    /// be attached to components for binding values to properties at the point of DataBinding.
    /// </summary>
    public interface IPDFBindingExpressionFactory
    {
        /// <summary>
        /// Gets the stage in the document lifcycle delegate from this expression factory should be invoked.
        /// Supported values are Init, Load and DataBind
        /// </summary>
        DocumentGenerationStage BindingStage { get; }

        string BindingKey { get; }

        /// <summary>
        /// Gets a binding expression delegate for the provided expression and related property 
        /// that will be invoked at the appropriate point in the lifecycle
        /// </summary>
        /// <param name="expressionvalue">The defined expression to be used when binding</param>
        /// <param name="forProperty">The property the expression should be bound to</param>
        /// <returns></returns>
        InitializedEventHandler GetInitBindingExpression(string expressionvalue, Type classType, System.Reflection.PropertyInfo forProperty);

        /// <summary>
        /// Gets a binding expression delegate for the provided expression and related property 
        /// that will be invoked at the appropriate point in the lifecycle
        /// </summary>
        /// <param name="expressionvalue">The defined expression to be used when binding</param>
        /// <param name="forProperty">The property the expression should be bound to</param>
        /// <returns></returns>
        LoadedEventHandler GetLoadBindingExpression(string expressionvalue, Type classType, System.Reflection.PropertyInfo forProperty);


        /// <summary>
        /// Gets a binding expression delegate for the provided expression and related property 
        /// that will be invoked at theDataBinding point in the lifecycle - so it will be passed
        /// a PDFDataContextArgs to the event handler
        /// </summary>
        /// <param name="expressionvalue">The defined expression to be used when binding</param>
        /// <param name="forProperty">The property the expression should be bound to</param>
        /// <param name="classType">The type that contains the property</param>
        /// <returns></returns>
        DataBindEventHandler GetDataBindingExpression(string expressionvalue, Type classType, System.Reflection.PropertyInfo forProperty);

    }

    #endregion


    #region public interface IPDFParser

    /// <summary>
    /// A parser that can read a stream to generate a PDFComponent
    /// </summary>
    public interface IComponentParser
    {

        /// <summary>
        /// Gets or sets the root component for the parser. 
        /// If set before parsing then this instance will be used as any event handler. 
        /// If not set, then the parser will set it with the top level parsed component
        /// </summary>
        object RootComponent { get; set; }

        /// <summary>
        /// Parses the specified stream using the resolver to load any referenced files and returns the PDFComponent representation
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="source"></param>
        /// <param name="istemplate">If the source to be parsed is a template (not a physical file) set this to true</param>
        /// <returns></returns>
        IComponent Parse(string source, System.IO.Stream stream, ParseSourceType type);

        /// <summary>
        /// Parses the specified stream using the resolver to load any referenced files and returns the PDFComponent representation
        /// </summary>
        /// <param name="source"></param>
        /// <param name="reader"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IComponent Parse(string source, System.IO.TextReader reader, ParseSourceType type);

        /// <summary>
        /// Parses the specified stream using the resolver to load any referenced files and returns the PDFComponent representation
        /// </summary>
        /// <param name="source"></param>
        /// <param name="reaser"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IComponent Parse(string source, System.Xml.XmlReader reader, ParseSourceType type);
    }

    #endregion

    /// <summary>
    /// Interface for the parser factory
    /// </summary>
    public interface IParserFactory
    {
        MimeType[] SupportedTypes { get; }

        IComponentParser CreateParser(MimeType forType, Generation.ParserSettings settings);
    }

    /// <summary>
    /// Interface all template generators should implement
    /// </summary>
    public interface IPDFTemplateGenerator
    {
        /// <summary>
        /// Gets the full name of the elememnt that the 
        /// </summary>
        string ElementName { get; set; }

        /// <summary>
        /// Gets or sets the flag to indicate if the template should be a block or inline invisible
        /// </summary>
        bool IsBlock { get; set; }

        void InitTemplate(string xmlContent, System.Xml.XmlNamespaceManager namespaces);
    }


    #region public interface IPDFDataTemplate

    /// <summary>
    /// Interface for a class that is a template generator with a prefix
    /// data style identifier that will be applied to each item in the template.
    /// </summary>
    public interface IPDFDataTemplateGenerator : IPDFTemplateGenerator
    {
        string DataStyleStem { get; set; }

        bool UseDataStyleIdentifier { get; set; }
    }

    #endregion

    /// <summary>
    /// Interface for a text literal component that will be used by the parse when it encounters general textual content
    /// </summary>
    public interface IPDFTextLiteral : IComponent
    {
        string Text { get; set; }
        TextFormat ReaderFormat { get; set; }
    }

    /// <summary>
    /// Placeholder for the Canvas - has its own styles and resources.
    /// </summary>
    public interface ICanvas : IResourceContainer, IComponent
    {
        
    }

}
