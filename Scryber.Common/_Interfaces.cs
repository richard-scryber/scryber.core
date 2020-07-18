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
using System.Linq;
using System.Text;
using Scryber.Native;
using Scryber.Resources;

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
        PDFObjectType Type { get; }

    }

    #endregion

    #region public interface IFileObject : ITypedObject

    /// <summary>
    /// Base abstract class of all native file objects (PDFBoolean, PDFNumber etc...)
    /// </summary>
    public interface IFileObject : ITypedObject
    {
        /// <summary>
        /// Writes the underlying data of the file object to the passed text writer
        /// </summary>
        /// <param name="tw">The text writer object to write data to</param>
        void WriteData(PDFWriter writer);

    }

    #endregion

    #region public interface IIndirectObject : IDisposable

    /// <summary>
    /// Defines the interface that all indirect objects must adhere to.
    /// </summary>
    public interface IIndirectObject : IDisposable
    {
        /// <summary>
        /// Gets the object number of this indirect object
        /// </summary>
        int Number { get; set; }

        /// <summary>
        /// Gets the generation number of this indirect object
        /// </summary>
        int Generation { get; set; }

        /// <summary>
        /// Gets the byte offset of this indirect object in the base stream
        /// </summary>
        long Offset { get; set; }

        /// <summary>
        /// Gets the associated object data for this indirect object
        /// </summary>
        PDFStream ObjectData { get; }

        /// <summary>
        /// Returns true if this indirect object is deleted.
        /// </summary>
        bool Deleted { get; }

        /// <summary>
        /// Returns true if this indirect object has already been written to the base stream
        /// </summary>
        bool Written { get; set; }

        /// <summary>
        /// Returns true if this indirect object has an inner stream data
        /// </summary>
        bool HasStream { get; }

        /// <summary>
        /// Returns the inner stream data for this indirect object
        /// </summary>
        PDFStream Stream { get; }

        /// <summary>
        /// Gets the associated object data as a byte array
        /// </summary>
        /// <returns></returns>
        byte[] GetObjectData();

        /// <summary>
        /// Gets the associated stream data as a byte array
        /// </summary>
        /// <returns></returns>
        byte[] GetStreamData();
    }

    #endregion

    #region public interface IParsedIndirectObject : IIndirectObject

    /// <summary>
    /// Interface for indirect objects that have been parsed from an existing file
    /// </summary>
    public interface IParsedIndirectObject : IIndirectObject
    {
        /// <summary>
        /// Returns the parsed object data 
        /// </summary>
        /// <returns></returns>
        IFileObject GetContents();
    }

    #endregion

    #region public interface IStreamFactory

    /// <summary>
    /// Interface for instance that creates PDFStreams indirect objects can use
    /// </summary>
    public interface IStreamFactory
    {
        PDFStream CreateStream(IStreamFilter[] filters, IIndirectObject forObject);
    }

    #endregion

    #region public interface IStreamFilter

    /// <summary>
    /// Defines the interface that all Stream Filters must adhere to 
    /// </summary>
    public interface IStreamFilter
    {
        /// <summary>
        /// Gets or Sets the name of the filter
        /// </summary>
        string FilterName
        {
            get;
            set;
        }

        /// <summary>
        /// Filters the stream reading from the TextReader, applying the filter and writing to the TextWriter
        /// </summary>
        /// <param name="read"></param>
        /// <param name="write"></param>
        void FilterStream(System.IO.Stream read, System.IO.Stream write);

        /// <summary>
        /// Performs a filter on the original data array, and returns the filtered data as a new byte[]
        /// </summary>
        /// <param name="orig"></param>
        /// <returns></returns>
        byte[] FilterStream(byte[] orig);
    }

    #endregion

    #region public interface IObjectContainer

    /// <summary>
    /// Interface that defines a container of IFileObjects
    /// </summary>
    public interface IObjectContainer
    {
        void Add(IFileObject obj);
    }

    #endregion

    #region public interface IPDFObject : ITypedObject

    /// <summary>
    /// Base interface for all pdf objects
    /// </summary>
    public interface IPDFObject : ITypedObject //, IDisposable
    {
    }

    #endregion

    #region public interface IPDFResourceContainer

    /// <summary>
    /// Interface for any items that hold a collection of resources
    /// </summary>
    public interface IPDFResourceContainer
    {

        IPDFDocument Document { get; }

        Scryber.Native.PDFName Register(PDFResource rsrc);

        string MapPath(string source);

    }

    #endregion

    #region public interface IPDFResource

    /// <summary>
    /// Defines a top level resource that is contained in the PDF Document, and used for rendering the pages - e.g. Font or Image
    /// </summary>
    public interface IPDFResource
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
        /// If this resource has not been previously rendered, then this resource will render its content within the document.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        PDFObjectRef EnsureRendered(PDFContextBase context, PDFWriter writer);
    }

    #endregion

    #region public interface IPDFTemplate

    /// <summary>
    /// Interface for a class that supports the instantiation of one or more copies its own content into the container
    /// </summary>
    public interface IPDFTemplate
    {
        /// <summary>
        /// Creates a copy of any content of this template in the specified container
        /// </summary>
        /// <param name="index">The current index of the instantiation</param>
        /// <param name="owner">The owner of this template</param>
        IEnumerable<IPDFComponent> Instantiate(int index, IPDFComponent owner);
    }

    #endregion

    #region public interface IPDFComponent : IPDFObject

    /// <summary>
    /// Interface that complex pdf objects should support, including initialization and disposal
    /// </summary>
    public interface IPDFComponent : IPDFObject, IDisposable
    {
        /// <summary>
        /// Event that is raised when the object is initialized
        /// </summary>
        event PDFInitializedEventHandler Initialized;

        /// <summary>
        /// Event that is raised when the object is loaded
        /// </summary>
        event PDFLoadedEventHandler Loaded;

        /// <summary>
        /// Initializes a PDFComponent
        /// </summary>
        void Init(PDFInitContext context);

        /// <summary>
        /// Loads the component.
        /// </summary>
        /// <param name="context"></param>
        void Load(PDFLoadContext context);

        /// <summary>
        /// Gets or sets the ID of the component
        /// </summary>
        string ID { get; set; }

        /// <summary>
        /// Gets the document that contains this PDFComponent, and forms the root of the PDF Component hierarchy
        /// </summary>
        IPDFDocument Document { get; }

        /// <summary>
        /// Gets or sets the containing parent of this PDFComponent
        /// </summary>
        IPDFComponent Parent { get; set; }

        /// <summary>
        /// Returns the full path to a file relative to the component or its contianer(s).
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        string MapPath(string source);

    }

    #endregion

    #region public interface IPDFComponentWrappingList

    /// <summary>
    /// Interface for lists that wrap IPDFComponentLists for strongly typing
    /// </summary>
    public interface IPDFComponentWrappingList
    {
        IPDFComponentList InnerList { get; }
    }

    #endregion

    #region public interface IPDFComponentList : ICollection<IPDFComponent>

    /// <summary>
    /// A List of IPDFComponents - ICollection interface and Insert
    /// </summary>
    public interface IPDFComponentList : ICollection<IPDFComponent>
    {
        void Insert(int index, IPDFComponent component);
    }

    #endregion

    #region public interface IPDFLoadableComponent : IPDFComponent

    /// <summary>
    /// Defines a component interface that can be remotely loaded and the FileSource set as 
    /// the path the file was loaded from
    /// </summary>
    public interface IPDFLoadableComponent : IPDFComponent
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

    #region public interface IPDFRemoteComponent : IPDFComponent
    /// <summary>
    /// Defines a component that can be parsed from a remote XML file, extending the Loadable 
    /// component with the namespace declarations in the XML and any 
    /// parsed items that need to be passed into it.
    /// </summary>
    public interface IPDFRemoteComponent : IPDFLoadableComponent
    {
        /// <summary>
        /// Gets the Items defined and associated with this remote component
        /// </summary>
        PDFItemCollection Params { get; }

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

    #region public interface IPDFBindableComponent

    /// <summary>
    /// Interface that identifies the Databinding features of an Component
    /// </summary>
    public interface IPDFBindableComponent
    {
        /// <summary>
        /// Event that is raised before an Component is databound
        /// </summary>
        event PDFDataBindEventHandler DataBinding;
        /// <summary>
        /// Event that is raised after an object has been databound
        /// </summary>
        event PDFDataBindEventHandler DataBound;

        /// <summary>
        /// Databinds a PFComponent
        /// </summary>
        void DataBind(PDFDataContext context);
    }

    #endregion

    #region public interface IPDFOptimizeComponent 

    /// <summary>
    /// Interface for the compression flag
    /// </summary>
    public interface IPDFOptimizeComponent : IPDFComponent
    {
        bool Compress { get; set; }
    }

    #endregion

    #region public interface IPDFNamingContainer

    /// <summary>
    /// Placeholder interface to identify instances that are included in creating a unique ID
    /// </summary>
    public interface IPDFNamingContainer
    {
    }

    #endregion

    #region public interface IPDFPathMappingService

    public interface IPDFPathMappingService
    {
        string MapPath(ParserLoadType loadtype, string reference, string parent, out bool isFile);
    }

    #endregion

    /// <summary>
    /// Interface for the 
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
        IPDFCacheProvider GetProvider();
    }

    #region public interface IPDFCacheProvider

    /// <summary>
    /// Defines the contract all CacheProviders must conform to 
    /// in order to support access to the data cache
    /// </summary>
    public interface IPDFCacheProvider
    {
        bool TryRetrieveFromCache(string type, string key, out object data);

        void AddToCache(string type, string key, object data);

        void AddToCache(string type, string key, object data, TimeSpan duration);

        void AddToCache(string type, string key, object data, DateTime expires);

    }

    #endregion

    #region public interface IPDFDocument : IPDFLoadableComponent

    /// <summary>
    /// Top Level document interface - supports resourses and componentID's
    /// </summary>
    public interface IPDFDocument : IPDFLoadableComponent
    {

        /// <summary>
        /// Loads a specific PDFResource based on the requested resource type and key
        /// </summary>
        /// <param name="type">The resource type</param>
        /// <param name="key">The resource key </param>
        /// <returns></returns>
        PDFResource GetResource(string type, string key, bool create);

        /// <summary>
        /// Returns a document unique identifier for a particular object type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        string GetIncrementID(PDFObjectType type);

    }

    #endregion

    #region public interface IPDFXMLParsedDocument : IPDFDocument

    /// <summary>
    /// Defines the contract for a component so that the PDFXmlParser can interact with it.
    /// </summary>
    /// <remarks>
    /// During the parsing the XMLParser will encounter pre</remarks>
    public interface IPDFXMLParsedDocument : IPDFDocument
    {
        /// <summary>
        /// Set to true if the document should append a trace log table after the document has been generated.
        /// </summary>
        bool AppendTraceLog { get; set; }

        /// <summary>
        /// When the PDFXmlParser reads a document, then if it encounters a switch to tell it to append the log to the document it must add a collector log
        /// and set it on the document, so that it can be output. This interface defines a property that can be set by the parser and then have the component use.
        /// </summary>
        Scryber.PDFTraceLog TraceLog { get; set; }

        /// <summary>
        /// Gets or sets the performance monitor instance that the document will use to capture performance statistics
        /// </summary>
        Scryber.PDFPerformanceMonitor PerformanceMonitor { get; set; }

        /// <summary>
        /// Gets or sets the conformance mode of this parsed document
        /// </summary>
        Scryber.ParserConformanceMode ConformanceMode { get; set; }

    }

    #endregion

    #region public interface IPDFControlledComponent : IPDFRemoteComponent

    /// <summary>
    /// Defines an interface where the remote component can have a controller assigned, and the XML parser can hook up outlets and actions.
    /// </summary>
    public interface IPDFControlledComponent : IPDFRemoteComponent
    {
        /// <summary>
        /// Gets or sets the controller for this remote component.
        /// </summary>
        object Controller { get; set; }
    }

    #endregion

    #region public interface IPDFSimpleExpressionValue

    /// <summary>
    /// Interface that simple types can implement to return custom construction code expression
    /// </summary>
    public interface IPDFSimpleExpressionValue
    {
        System.Linq.Expressions.Expression GetConstructorExpression();
    }

    #endregion

    public interface IPDFDataComponent : IPDFComponent
    {

    }

    #region public interface IPDFDataSource

    /// <summary>
    /// Defines the interface that all controls must implement if 
    /// they want to be used as a source of data within the page.
    /// </summary>
    public interface IPDFDataSource : IPDFDataComponent
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
        PDFDataSchema GetDataSchema(string path, PDFDataContext context);

        /// <summary>
        /// Performs the selection of data configured on this source
        /// </summary>
        /// <param name="path">The optional restriction.</param>
        /// <returns>The top level data associated with this DataSource control.</returns>
        object Select(string path, PDFDataContext context);

        /// <summary>
        /// Performs a select of the provided data based on the path in the current data context
        /// </summary>
        /// <param name="path">The path to select the data from</param>
        /// <param name="current">The current data to select the data from</param>
        /// <param name="context">The current data context</param>
        /// <returns>The </returns>
        object Select(string path, object current, PDFDataContext context);

        /// <summary>
        /// Performs a test of the provided expression against the provided data in the current context
        /// </summary>
        /// <param name="expr"></param>
        /// <param name="withData"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        bool EvaluateTestExpression(string expr, object withData, PDFDataContext context);


        /// <summary>
        /// Performs an evaluate of the provided data based on the path and the current data context
        /// </summary>
        /// <param name="expr"></param>
        /// <param name="withData"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        object Evaluate(string expr, object withData, PDFDataContext context);

    }

    #endregion

    #region public interface IKeyValueProvider

    /// <summary>
    /// Defines an open interface for returning an object associated with a key
    /// </summary>
    public interface IKeyValueProvider
    {
        string ID { get; }

        object GetNativeValue(string key, IPDFComponent comp);

        void SetValue(string key, string value, IPDFComponent owner);

        void SetNativeValue(string key, object value, IPDFComponent owner);

        void Init();
    }

    #endregion

    #region public interface IPDFTraceLogFactory

    /// <summary>
    /// Contract that the trace log factories, defined in the configuration file must support to create new loggers
    /// </summary>
    public interface IPDFTraceLogFactory
    {
        PDFTraceLog CreateLog(TraceRecordLevel level, string name);
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
        PDFInitializedEventHandler GetInitBindingExpression(string expressionvalue, Type classType, System.Reflection.PropertyInfo forProperty);

        /// <summary>
        /// Gets a binding expression delegate for the provided expression and related property 
        /// that will be invoked at the appropriate point in the lifecycle
        /// </summary>
        /// <param name="expressionvalue">The defined expression to be used when binding</param>
        /// <param name="forProperty">The property the expression should be bound to</param>
        /// <returns></returns>
        PDFLoadedEventHandler GetLoadBindingExpression(string expressionvalue, Type classType, System.Reflection.PropertyInfo forProperty);


        /// <summary>
        /// Gets a binding expression delegate for the provided expression and related property 
        /// that will be invoked at theDataBinding point in the lifecycle - so it will be passed
        /// a PDFDataContextArgs to the event handler
        /// </summary>
        /// <param name="expressionvalue">The defined expression to be used when binding</param>
        /// <param name="forProperty">The property the expression should be bound to</param>
        /// <param name="classType">The type that contains the property</param>
        /// <returns></returns>
        PDFDataBindEventHandler GetDataBindingExpression(string expressionvalue, Type classType, System.Reflection.PropertyInfo forProperty);

    }

    #endregion
}
