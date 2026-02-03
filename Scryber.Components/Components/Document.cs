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
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Xml;
using System.Net.Http;

using Scryber.Styles;
using Scryber.PDF.Resources;

using Scryber.Drawing;
using Scryber.Imaging;
using Scryber.Data;
using Scryber.Generation;

using Scryber.PDF;
using Scryber.PDF.Layout;
using Scryber.PDF.Native;
using Scryber.Options;
using Scryber.Logging;
using System.IO;

namespace Scryber.Components
{
    [PDFParsableComponent("Document")]
    [PDFRemoteParsableComponent("Document-Ref")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_document")]
    public partial class Document : ContainerComponent, IDocument, IPDFViewPortComponent, IRemoteComponent, IStyledComponent,
                                                      ITemplateParser, IParsedDocument, IControlledComponent, IResourceRequester
    {
        //
        // events
        //

        #region public event PDFComponentRegisteredHandler ComponentRegistered

        /// <summary>
        /// Event that is raised when a component is registered with the document
        /// </summary>
        public event ComponentRegisteredHandler ComponentRegistered;

        /// <summary>
        /// Raises the ComponentRegistered event if there are receivers.
        /// </summary>
        /// <param name="comp"></param>
        protected virtual void OnComponentRegistered(IComponent comp)
        {
            if (null != ComponentRegistered)
            {
                this.ComponentRegistered(this, comp);
            }
        }

        #endregion

        #region public event RemoteFileRequestEventHandler RemoteFileRegistered

        /// <summary>
        /// Event that is raised when a request is being made for a file or url for inclusion within the document
        /// </summary>
        public event RemoteFileRequestEventHandler RemoteFileRegistered;

        /// <summary>
        /// Rasises the RemoteFileRegisted event to any added handlers
        /// </summary>
        /// <param name="request"></param>
        protected virtual void OnRemoteFileRequestRegistered(RemoteFileRequest request)
        {
            if (null != RemoteFileRegistered)
                this.RemoteFileRegistered(this, new RemoteFileRequestEventArgs(request));
        }

        #endregion 

        //
        // ctors
        // 

        #region public PDFDocument()

        /// <summary>
        /// Default constructor - creates a new instance of the document
        /// </summary>
        public Document()
            : this(ObjectTypes.Document)
        {

        }

        #endregion

        #region protected PDFDocument(PDFObjectType type)

        protected Document(ObjectType type)
            : base(type)
        {
            this._incrementids = new Dictionary<ObjectType, int>();
            
            this.ImageFactories = this.DoCreateImageFactories();
            this._startTime = DateTime.Now;
            this._requests = new RemoteFileRequestSet(this);
        }

        #endregion

        //
        // properties
        //

        #region public DocumentGenerationStage CurrentStage {get;}

        private DocumentGenerationStage _stage = DocumentGenerationStage.None;

        /// <summary>
        /// Marks the state this document is at in the process of generation
        /// </summary>
        public DocumentGenerationStage GenerationStage
        {
            get { return _stage; }
            protected set { _stage = value; }
        }

        #endregion
        

        #region public PDFDocumentListNumbering ListNumbering {get;}

        private ListNumbering _numbering;

        /// <summary>
        /// Gets the list numbering manager for this document. This can be used for starting to enumerate over a list, and remembering last values.
        /// </summary>
        public ListNumbering ListNumbering
        {
            get
            {
                if (null == _numbering)
                    _numbering = new ListNumbering();
                return _numbering;
            }
        }

        #endregion

        #region public Scryber.Data.PDFXmlNamespaceCollection NamespaceDeclarations {get;}

        private Scryber.Data.PDFXmlNamespaceCollection _namespaces;

        /// <summary>
        /// Gets the namespaces that were declared on this document component if it was loaded remotely
        /// </summary>
        public Scryber.Data.PDFXmlNamespaceCollection NamespaceDeclarations
        {
            get
            {
                return _namespaces;
            }
        }

        #endregion

        #region public string CurrentDirectory {get;}

        private string _currdirectory;

        /// <summary>
        /// Gets the full path to this document's directory. If the document is not loaded from a 
        /// specific file this will be either the root of the current web application, 
        /// or the executable location of the binary. 
        /// </summary>
        public string CurrentDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(_currdirectory))
                {
                    if (!string.IsNullOrEmpty(this.LoadedSource))
                        _currdirectory = System.IO.Path.GetDirectoryName(this.LoadedSource);
                    else
                        throw new NotSupportedException("Document Loaded Source has not been set");
                    //_currdirectory = Scryber.Utilities.PathHelper.GetRootDirectory(this.LoadType);
                }
                return _currdirectory;
            }
            private set
            {
                _currdirectory = value;
            }
        }

        #endregion

        #region public Object Controller {get;set;}

        /// <summary>
        /// Gets or sets the controller object for this document. 
        /// Normally set by the XMLParser when it encounters the controller option on the scryber processing instructions.
        /// </summary>
        public Object Controller
        {
            get;
            set;
        }

        #endregion

        #region private NameDictionary ComponentNames {get;}

        /// <summary>
        /// Local cache of components by name
        /// </summary>
        private NameDictionary _namedictionary = new NameDictionary();

        private NameDictionary ComponentNames
        {
            get { return _namedictionary; }
        }

        #endregion




        //
        // Document level attributes
        //

        #region public string FileName {get;set;}

        private string _filename;

        /// <summary>
        /// Gets or sets the name of the output file
        /// </summary>
        [PDFAttribute("file-name")]
        [PDFDesignable("File Name", Category = "General", Priority = 3, Type = "String")]
        public string FileName
        {
            get { return _filename; }
            set { this._filename = value; }
        }

        #endregion

        #region public bool AutoBind {get;set;}

        private bool _autobind = true;

        /// <summary>
        /// Flag to identify if the document should automatically call databaind when processing. Default is true
        /// </summary>
        [PDFAttribute("auto-bind")]
        [PDFDesignable("Auto Bind", Ignore = true)]
        public bool AutoBind
        {
            get { return _autobind; }
            set { _autobind = true; }
        }

        #endregion

        #region public PDFDocumentID DocumentID

        private PDFDocumentID _ids;

        /// <summary>
        /// Gets or sets the document id. This uniquely identifies the document as a particular PDF File
        /// </summary>
        [PDFAttribute("document-id")]
        [PDFDesignable("Document ID", Ignore = true)]
        public PDFDocumentID DocumentID
        {
            get { return _ids; }
            set { _ids = value; }
        }

        #endregion

        #region public string DocumentIdentifierPrefix
        
        
        private string _idPrefix = "";

        /// <summary>
        /// Gets the prefix for unique resources, fonts, and ID's that the document uses to identify individual components.
        /// </summary>
        public string DocumentIdentifierPrefix
        {
            get { return _idPrefix; }
        }
        
        #endregion

        #region public string DocumentFormatting {get;set;}


        /// <summary>
        /// Gets or sets the document formatting options - None, PDF/A, PDF/X etc.
        /// for the output of the document. Not currently supported and the attribute is legacy.
        /// </summary>
        [PDFAttribute("output-format")]
        [Obsolete("Use the RenderOptions.OutputCompliance instead", false)]
        [PDFDesignable("Format", Ignore = true)]
        public string DocumentOutputFormat
        {
            get { return this.RenderOptions.OuptputCompliance; }
            set { this.RenderOptions.OuptputCompliance = value; }
        }

        #endregion

        #region public PDFFile PrependedFile
        
        private PDFFile _prependFile;
        
        /// <summary>
        /// Gets or sets the PDFFile that this document should be a new version for.
        /// This will then appear in the final output and be included as part of the document
        /// </summary>
        public PDFFile PrependedFile
        {
            get { return this._prependFile; }
            set { this._prependFile = value; }
        }
        
        #endregion

        //
        // Document inner elements
        //

        #region public PDFDocumentInfo Info {get;set;}

        private DocumentInfo _info = new DocumentInfo();

        /// <summary>
        /// Gets or sets the information for this document
        /// </summary>
        [PDFElement("Info")]
        public DocumentInfo Info
        {
            get
            {
                if (null == _info)
                    _info = CreateDocumentInfo();
                return _info;
            }
            set { _info = value; }
        }

        /// <summary>
        /// Returns true if this document has a specific instance of the PDFDocumentInfo.
        /// </summary>
        public bool HasInfo
        {
            get { return null != _info; }
        }

        /// <summary>
        /// Creates and returns a new PDFDocumentInfo for this document.
        /// </summary>
        /// <returns></returns>
        protected virtual DocumentInfo CreateDocumentInfo()
        {
            return new DocumentInfo();
        }

        #endregion

        #region public PDFItemCollection Items {get;}

        private ItemCollection _items = null;

        /// <summary>
        /// Gets a document centered collection of objects that can be accessed by name or index
        /// </summary>
        [PDFElement("Params")]
        [PDFArray(typeof(IKeyValueProvider))]
        public ItemCollection Params
        {
            get
            {
                if (null == _items)
                    _items = this.CreateItems();
                return _items;
            }
        }

        /// <summary>
        /// Returns true if this page has one or more specific stored items. Otherwise false
        /// </summary>
        public bool HasParams
        {
            get { return null != this._items && _items.Count > 0; }
        }

        /// <summary>
        /// Creates and returns a new PDFItemCollection for this document. Inheritors can override
        /// </summary>
        /// <returns></returns>
        protected virtual ItemCollection CreateItems()
        {
            return new ItemCollection(this);
        }

        #endregion

        #region public PDFDocumentViewPreferences ViewPreferences {get;set;}

        private DocumentViewPreferences _viewPrefs;

        /// <summary>
        /// Gets or sets the viewer preferences for this document
        /// </summary>
        [PDFElement("Viewer")]
        public DocumentViewPreferences ViewPreferences
        {
            get
            {
                if (null == _viewPrefs)
                    _viewPrefs = CreateViewPreferences();
                return _viewPrefs;
            }
            set { _viewPrefs = value; }
        }

        /// <summary>
        /// Creates and returns a new PDFDocumentViewPreferences for this document. Inheritors can override
        /// </summary>
        /// <returns></returns>
        protected virtual DocumentViewPreferences CreateViewPreferences()
        {
            return new DocumentViewPreferences();
        }

        #endregion

        #region public DocumentAdditionList Additions {get;set;}

        private DocumentAdditionList _additions;

        /// <summary>
        /// Returns true if this document has one or more components in it's additions collection.
        /// </summary>
        public bool HasAdditions
        {
            get { return null != _additions && _additions.Count > 0; }
        }

        /// <summary>
        /// Gets or sets the list of Additions (non-visual components) in this document.
        /// </summary>
        [PDFElement("Additions")]
        [PDFArray(typeof(IComponent))]
        public DocumentAdditionList Additions
        {
            get
            {
                if (null == _additions)
                    _additions = CreateAdditionsList();
                return _additions;
            }
            set
            {
                if (this.HasAdditions)
                    this._additions.Owner = null;

                _additions = value;

                if (this.HasAdditions)
                    _additions.Owner = this;
            }
        }

        /// <summary>
        /// Creates and returns a new PDFDocumentAdditionsList for this document. Inheritors can override
        /// </summary>
        /// <returns></returns>
        protected virtual DocumentAdditionList CreateAdditionsList()
        {
            return new DocumentAdditionList(this);
        }

        #endregion

        #region public DataComponentList DataSources

        private DataComponentList _sources;

        [PDFElement("Data")]
        [PDFArray(typeof(DataComponentBase))]
        public DataComponentList DataSources
        {
            get
            {
                if (null == this._sources)
                    this._sources = CreateDataList();
                return this._sources;
            }
        }

        protected virtual DataComponentList CreateDataList()
        {
            return new DataComponentList(this);
        }

        public bool HasDataSources
        {
            get
            {
                if (null == _sources || _sources.Count == 0) return false;
                else return true;

            }
        }
        #endregion

        #region public PageList Pages {get;}

        private PageList _pages = null;

        /// <summary>
        /// Gets the list of pages in this document
        /// </summary>
        [PDFElement("Pages")]
        [PDFArray(typeof(PageBase))]
        public virtual PageList Pages
        {
            get
            {
                if (this._pages == null)
                    this._pages = CreatePageList();
                return _pages;
            }
        }


        internal static readonly IDocumentPage[] NoPagesArray = new IDocumentPage[] { };

        /// <summary>
        /// Gets all of the components that implement the IDocummentPage interface within the document. (Including any nested document pages)
        /// </summary>
        IDocumentPage[] IDocumentPageContainer.AllPages
        {
            get
            {
                if (this.InnerContent.Count == 0)
                    return NoPagesArray;
                else
                {
                    List<IDocumentPage> all = new List<IDocumentPage>(this.InnerContent.Count);
                    this.DoExtractDocumentPages(all, this.InnerContent);
                    return all.ToArray();
                }
            }
        }

        protected virtual void DoExtractDocumentPages(List<IDocumentPage> all, ComponentList contents)
        {
            for (var i = 0; i < contents.Count; i++)
            {
                var pb = contents[i];
                if (pb is IDocumentPageContainer container)
                    all.AddRange(container.AllPages);
                else if (pb is IDocumentPage docPg)
                    all.Add(docPg);
                else if (pb is IInvisibleContainer invisible)
                {
                    var inner = invisible.Content;
                    this.DoExtractDocumentPages(all, inner);
                }
            }
        }

        /// <summary>
        /// Creates and returns a new page list wrapping on the inner content. Inheritors can override.
        /// </summary>
        /// <returns></returns>
        protected virtual PageList CreatePageList()
        {
            return new PageList(this.InnerContent);
        }

        #endregion

        #region public StyleCollection Styles {get;} + protected virtual StyleCollection CreateStyleCollection()

        private StyleCollection _styles;

        /// <summary>
        /// Gets the collection of styles in this document
        /// </summary>
        [PDFElement("Styles")]
        [PDFArray(typeof(StyleBase))]
        public StyleCollection Styles
        {
            get
            {
                if (_styles == null)
                    _styles = CreateStyleCollection();
                return _styles;
            }
        }

        /// <summary>
        /// Creates a new empty style collection
        /// </summary>
        /// <returns></returns>
        protected virtual StyleCollection CreateStyleCollection()
        {
            return new StyleCollection(this);
        }

        #endregion

        #region public PDFDocumentRenderOptions RenderOptions {get; set;}

        private PDFDocumentRenderOptions _renderopts;

        /// <summary>
        /// Gets or sets the rendering options for this document
        /// </summary>
        /// <remarks>The RenderOptions controls how this document will be output - including compression, name dictionary and conformance mode.</remarks>
        [PDFElement("Render-Options")]
        public PDFDocumentRenderOptions RenderOptions
        {
            get
            {
                if (null == _renderopts)
                    _renderopts = CreateRenderOptions();
                return _renderopts;
            }
            set
            {
                _renderopts = value;
            }
        }

        /// <summary>
        /// Creates and returns a new PDFDocumentRenderOptions instance for this document. Inheritors can override
        /// </summary>
        /// <returns></returns>
        protected virtual PDFDocumentRenderOptions CreateRenderOptions()
        {
            return new PDFDocumentRenderOptions();
        }

        #endregion

        #region IPDFStyledComponent.Style {get;} + IPDFStyledComponent.HasStyle {get;}

        // IPDFStyledComponent explicit interface - so we dont show the properties by default

        Style IStyledComponent.Style { get; }

        bool IStyledComponent.HasStyle { get { return null != (this as IStyledComponent).Style; } }

        #endregion

        //
        // document level services and values
        //

        #region public IPDFCacheProvider CacheProvider {get;} + protected virtual IPDFCacheProvider CreateCacheProvider()

        private ICacheProvider _cacheprov;
        /// <summary>
        /// Gets the caching provider for this instance
        /// </summary>
        public ICacheProvider CacheProvider
        {
            get
            {
                if (null == _cacheprov)
                    _cacheprov = this.CreateCacheProvider();
                return _cacheprov;
            }
            set
            {
                _cacheprov = value;
            }
        }

        /// <summary>
        /// Gets an instance of the ICaching provider from the service provider.
        /// </summary>
        /// <returns></returns>
        protected virtual ICacheProvider CreateCacheProvider()
        {

            var factory = ServiceProvider.GetService<IScryberCachingServiceFactory>();
            return factory.GetProvider();
        }

        #endregion

        #region public Scryber.PDFReferenceResolver Resolver {get;set;}

        /// <summary>
        /// Gets or sets the reference resolver for this document
        /// </summary>
        /// <remarks>This resolver will be used to generate any referenced files</remarks>
        public Scryber.PDFReferenceResolver Resolver
        {
            get;
            set;
        }

        #endregion

        #region public Scryber.PDFDataProviderList DataProviders { get }

        private Scryber.PDFDataProviderList _providers = new PDFDataProviderList();

        /// <summary>
        /// Contains the list of all the providers that are either registered or required in the document.
        /// </summary>
        public Scryber.PDFDataProviderList DataProviders { get { return this._providers; } }

        #endregion

        #region protected DocumentFontMatcher FontMatcher {get; set;}

        private DocumentFontMatcher _fontMatcher;

        /// <summary>
        /// Gets the font matching services
        /// </summary>
        protected DocumentFontMatcher FontMatcher
        {
            get {
                if (null == _fontMatcher)
                    _fontMatcher = new DocumentFontMatcher(this);
                return _fontMatcher;
            }
            set
            {
                this._fontMatcher = value;
            }
        }


        #endregion

        #region public PDFImageFactoryList ImageFactories { get; private set; }

        /// <summary>
        /// Gets the collection of image data factories for this document
        /// </summary>
        public ImageFactoryList ImageFactories
        {
            get; 
            private set;
        }

        protected virtual ImageFactoryList DoCreateImageFactories()
        {
            var config = ServiceProvider.GetService<IScryberConfigurationService>();
            var list = config.ImagingOptions.GetConfiguredFactories();
             
            var svgFactory = new Scryber.Svg.Imaging.SVGImagingFactory();
            list.Insert(list.Count, svgFactory);
            var svgDataFactory = new Scryber.Svg.Imaging.SVGDataUrlImagingFactory();
            list.Insert(list.Count, svgDataFactory);
            
            return list;
        }

        #endregion

        //
        // file requests
        //


        #region public RemoteFileRequestSet RemoteRequests { get;set; } + HasRemoteRequests {get;}

        private RemoteFileRequestSet _requests;

        /// <summary>
        /// Gets the set of all the remote file requests for this document that are still pending.
        /// </summary>
        public RemoteFileRequestSet RemoteRequests
        {
            get
            {
                if (null == _requests)
                    _requests = new RemoteFileRequestSet(this);
                return _requests;
            }
            set
            {
                _requests = value;
            }
        }

        /// <summary>
        /// Returns true if this document has some pending remote file requests.
        /// </summary>
        public bool HasRemoteRequests
        {
            get { return null != _requests && _requests.Count > 0; }
        }

        #endregion

        #region public virtual RemoteFileRequest RegisterRemoteFileRequest(string filePath, RemoteRequestCallback callback, IComponent owner = null, object arguments = null)

        /// <summary>
        /// Registers a file to be loaded from an external source or web url. With a callback to be used once the request has completed.
        /// </summary>
        /// <param name="type">The type of resource this request this is for</param>
        /// <param name="filePath">The full path to the file to be loaded</param>
        /// <param name="callback">The delegate method to execute when the request has succeeded</param>
        /// <param name="owner">Optional owner for the request</param>
        /// <param name="arguments">Optional arguments that will be give back to the callback method</param>
        /// <returns>A new disposable RemoteFileRequest object</returns>
        public virtual RemoteFileRequest RegisterRemoteFileRequest(string type, string filePath, TimeSpan cacheDuration, RemoteRequestCallback callback, IComponent owner = null, object arguments = null)
        {
            var request = new RemoteFileRequest(type, filePath, cacheDuration, callback, owner, arguments);
            this.RegisterRemoteFileRequest(request);

            return request;
        }

        IRemoteRequest IResourceRequester.RequestResource(string type, string path, TimeSpan cacheDuration, RemoteRequestCallback callback,
             IComponent owner, object arguments)
        {
            return this.RegisterRemoteFileRequest(type, path, cacheDuration, callback, owner, arguments);
        }

        /// <summary>
        /// Registers a new RemoteFileRequest. If the document exec mode is Immediate, then this will be syncronouly executed, otherwise it will be waited.
        /// </summary>
        /// <param name="request"></param>
        public virtual void RegisterRemoteFileRequest(RemoteFileRequest request)
        {
            if (null == request)
                throw new ArgumentNullException(nameof(request));
            
            object found;

            if (null != this.CacheProvider &&
                this.CacheProvider.TryRetrieveFromCache(request.ResourceType, request.FilePath, out found))
            {
                if(this.TraceLog.ShouldLog(TraceLevel.Verbose))
                    this.TraceLog.Add(TraceLevel.Verbose, "Document", "Cache hit for remote request '" + request.ResourceType + ", " + request.FilePath + "'. Completing and calling back with the result");
                
                request.CompleteRequest(found, true);
                request.Callback(request.Owner, request, null);
            }

            this.RemoteRequests.AddRequest(request);
            this.OnRemoteFileRequestRegistered(request);

            if (this.RemoteRequests.ExecMode == DocumentExecMode.Immediate && request.IsExecuting == false && request.IsCompleted == false)
            {
                if(this.TraceLog.ShouldLog(TraceLevel.Verbose))
                    this.TraceLog.Add(TraceLevel.Verbose,"Document", "Fulfilling the result for '" + request.FilePath + "' immediately as we are not asyncronous.");

                this.RemoteRequests.FullfillRequest(request, this.ConformanceMode == ParserConformanceMode.Strict);
            }

            if (null != this.CacheProvider && request.CacheDuration > TimeSpan.Zero)
            {
                if (request.Result != null)
                {
                    if (this.TraceLog.ShouldLog(TraceLevel.Verbose))
                        this.TraceLog.Add(TraceLevel.Verbose, "Document",
                            "Adding the result for '" + request.FilePath +
                            "' to the document cache, so it can be reused");

                    this.CacheProvider.AddToCache(request.ResourceType, request.FilePath, request.Result,
                        request.CacheDuration);
                }
                else if(this.TraceLog.ShouldLog(TraceLevel.Verbose))
                    this.TraceLog.Add(TraceLevel.Verbose, "Document", "The result of the remote execution was null, so not adding it to the cache for file "+ request.FilePath +"'");
            }
        }

        #endregion

        //
        // logging and performance intance management
        //
        
        #region public bool AppendTraceLog {get;set;}

        private bool _appendTraceLog = false;
        
        /// <summary>
        /// Gets or sets the append trace log flag, that indicates if the entire log output should be 
        /// appended to the document after is has been generated
        /// </summary>
        public bool AppendTraceLog
        {
            get => _appendTraceLog;
            set
            {
                _appendTraceLog = value;
                if(value)
                    this.EnsureCollectorLog();
            }
        }

        #endregion

        #region Scryber.PDFTraceLog TraceLog {get; protected set;} + support methods

        private TraceLog _log;
        private Scryber.Logging.CollectorTraceLog _collector;

        /// <summary>
        /// Gets the log  for this document.
        /// </summary>
        public TraceLog TraceLog
        {
            get
            {
                if (null == _log)
                    _log = this.CreateTraceLog();
                return _log;
            }
            protected set
            {
                _log = value;
                if (null == value)
                    _collector = null;
                else
                    _collector = _log.GetLogWithName(TraceLog.ScryberAppendTraceLogName) as Scryber.Logging.CollectorTraceLog;
            }
        }

        /// <summary>
        /// IParsed Document implementation to set the TraceLog. This will overwrite any existing trace log instance in this document.
        /// </summary>
        /// <param name="log">The log to set. No checks are made on instance validity.</param>
        void IParsedDocument.SetTraceLog(TraceLog log)
        {
            this.TraceLog = log;
        }

        /// <summary>
        /// Add a(nother) trace logging implementation to the document.
        /// If there is an existing log on the document, then it will be maintained and the new log appended.
        /// </summary>
        /// <param name="log">The log to add</param>
        /// <exception cref="ArgumentNullException">If the log to be added is null.</exception>
        public void AddTraceLog(TraceLog log)
        {
            if(null == log)
                throw new ArgumentNullException(nameof(log));
            
            var orig = this.TraceLog;

            if (null == orig)
                this._log = log;
            else
                this._log = new Logging.CompositeTraceLog(orig, log);
        }

        /// <summary>
        /// Instantiates a new trace log based on the configuration and returns the instance
        /// </summary>
        /// <returns></returns>
        protected virtual TraceLog CreateTraceLog()
        {
            var config = ServiceProvider.GetService<IScryberConfigurationService>();
            TraceLog log = config.TracingOptions.GetTraceLog();

            if (this.AppendTraceLog)
            {
                _collector = new Logging.CollectorTraceLog(log.RecordLevel, TraceLog.ScryberAppendTraceLogName, true);
                
                if (log is DoNothingTraceLog)
                    log = _collector;
                else
                {
                    Scryber.Logging.CompositeTraceLog composite =
                        new Logging.CompositeTraceLog(new TraceLog[] {log, _collector}, "");
                    log = composite;
                }
            }
            return log;
        }

        /// <summary>
        /// Makes sure the collector log (for appending the trace log output to the document) is present in the current TraceLog.
        /// </summary>
        protected void EnsureCollectorLog()
        {
            var currentLog = this.TraceLog;
            if (null == currentLog)
            {
                this.TraceLog = new Logging.CollectorTraceLog(TraceRecordLevel.Messages,
                    TraceLog.ScryberAppendTraceLogName, true);
            }
            else if (currentLog is DoNothingTraceLog)
            {
                this.TraceLog = new Logging.CollectorTraceLog(currentLog.RecordLevel,
                    TraceLog.ScryberAppendTraceLogName, true);
            }
            else
            {
                var collector = currentLog.GetLogWithName(TraceLog.ScryberAppendTraceLogName);
                if (null == collector)
                {
                    collector = new Logging.CollectorTraceLog(currentLog.RecordLevel,
                        TraceLog.ScryberAppendTraceLogName, true);
                    this.TraceLog = new Logging.CompositeTraceLog(new TraceLog[] {currentLog, collector}, "");
                }
            }
        }

        #endregion

        #region protected DateTime StartTime {get;}

        private DateTime _startTime;

        /// <summary>
        /// Gets the timestamp when this document was stating to be created and generated.
        /// </summary>
        public DateTime StartTime
        {
            get { return _startTime; }
        }

        #endregion

        #region public PDFPerformanceMonitor PerformanceMonitor  {get;set;}

        private PerformanceMonitor _perfmon;

        /// <summary>
        /// Gets or sets the preformance monitor for this document
        /// </summary>
        public PerformanceMonitor PerformanceMonitor
        {
            get
            {
                if (null == _perfmon)
                    _perfmon = this.CreatePerformanceMonitor();
                return _perfmon;
            }
            set
            {
                _perfmon = value;
            }
        }

        /// <summary>
        /// Instantiates a new trace log and returns it - inheritors can override this method
        /// </summary>
        /// <returns></returns>
        protected virtual PerformanceMonitor CreatePerformanceMonitor()
        {
            TraceRecordLevel level = this.TraceLog.RecordLevel;
            bool measure = (level >= TraceRecordLevel.Verbose);

            //TODO: Make this look at the configuration
            return new PerformanceMonitor(measure);
        }

        #endregion

        #region public ParserConformanceMode ConformanceMode {get;set;}

        private ParserConformanceMode _confmode = ParserConformanceMode.Strict;
        /// <summary>
        /// Gets or sets the parser conformance mode
        /// </summary>
        public ParserConformanceMode ConformanceMode
        {
            get
            {
                return this.RenderOptions.ConformanceMode; }
            set { this.RenderOptions.ConformanceMode = value; }
        }

        /// <summary>
        /// Explicit IParsedDocument implementation to set the conformance mode.
        /// </summary>
        /// <param name="mode"></param>
        void IParsedDocument.SetConformanceMode(ParserConformanceMode mode)
        {
            this._confmode = mode;
        }

        #endregion

        //
        // Style methods
        //

        #region public override PDFStyle GetAppliedStyle(PDFComponent forComponent)

        /// <summary>
        /// Overrides the default behaviour to get all the appropriate styles for the Components based on the inline and externally 
        /// refferences style definitions, starting with the Default style.
        /// </summary>
        /// <param name="forComponent">The Component to get the styles for</param>
        /// <param name="baseStyle">The base set of styles for the component</param>
        /// <returns>A newly constructed style appropriate for the Component</returns>
        public override Style GetAppliedStyle(Component forComponent, Style baseStyle)
        {
            if (null == baseStyle)
                baseStyle = new Style();

            this.Styles.MergeInto(baseStyle, forComponent);

            

            return baseStyle;
        }

        #endregion

        #region protected virtual PDFStyle CreateDefaultStyle()


        /// <summary>
        /// Creates the standard default style - setting page size, font + size, fill color - and returns the new PDFStyle instance.
        /// Inheritors can override this to adjust the default style for any document
        /// </summary>
        /// <returns></returns>
        protected virtual Style CreateDefaultStyle()
        {
            Style style = this.GetBaseStyle();
            
            //Get the applied style and then merge it into the base style
            Style applied = this.GetAppliedStyle(this, style);
            

            return applied;
        }

        #endregion

        #region protected override PDFStyle GetBaseStyle()

        /// <summary>
        /// Overrides the base implementation to get a full (yet empty) style for a document.
        /// </summary>
        /// <returns></returns>
        protected override Style GetBaseStyle()
        {
            Style style = base.GetBaseStyle();
            Styles.FillStyle fill = new Styles.FillStyle();
            style.StyleItems.Add(fill);
            fill.Color = StandardColors.Black;


            PageStyle defpaper = new PageStyle();
            style.StyleItems.Add(defpaper);
            defpaper.PaperSize = PaperSize.A4;
            defpaper.PaperOrientation = PaperOrientation.Portrait;

            Styles.FontStyle fs = new Styles.FontStyle();
            style.StyleItems.Add(fs);
            fs.FontFamily = (FontSelector)ServiceProvider.GetService<IScryberConfigurationService>().FontOptions.DefaultFont;
            fs.FontSize = new Unit(24.0, PageUnits.Points);


            return style;
        }

        #endregion

        //
        // Component registration
        // Designed to improve the lookup of components
        // 

        #region public void RegisterComponent(Component comp)

        /// <summary>
        /// Registers a component in the document. It is an error to register 2 components with the same name.
        /// </summary>
        /// <param name="comp"></param>
        /// <exception cref="ArgumentException" >Thrown if there is already a component with the same name registered in this document</exception>
        public virtual void RegisterComponent(Component comp)
        {
            if (!string.IsNullOrEmpty(comp.Name))
            {
                try
                {
                    this.ComponentNames[comp.Name] = comp;
                }
                catch (ArgumentException ex)
                {
                    throw new ArgumentException(string.Format(Errors.ComponentNamerAlreadyRegistered, comp.Name), ex);
                }
            }
            this.OnComponentRegistered(comp);
        }

        #endregion

        #region public void UnRegisterComponent(Component comp)

        /// <summary>
        /// Unregisters a component in the document.
        /// </summary>
        /// <param name="comp"></param>
        public void UnRegisterComponent(Component comp)
        {
            if (!string.IsNullOrEmpty(comp.Name))
            {
                this.ComponentNames.Remove(comp.Name);
            }
        }

        #endregion

        #region public void ReRegisterComponent(Component comp, string oldname) + 1 overload

        /// <summary>
        /// Re-registers the component, removing any registration with the old name
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="oldname"></param>
        public void ReRegisterComponent(Component comp, string oldname)
        {
            this.ReRegisterComponent(comp, oldname, comp.Name);
        }

        /// <summary>
        /// Re-registers a component with the new name, removing any registration with the old name
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="oldname"></param>
        /// <param name="newname"></param>
        public void ReRegisterComponent(Component comp, string oldname, string newname)
        {
            if (string.IsNullOrEmpty(oldname) == false)
                this.ComponentNames.Remove(oldname);
            if (string.IsNullOrEmpty(newname) == false)
            {
                try
                {
                    this.ComponentNames.Add(newname, comp);
                }
                catch (ArgumentException ex)
                {
                    throw new ArgumentException(string.Format(Errors.ComponentNamerAlreadyRegistered, comp.Name), ex);
                }
            }
        }

        #endregion

        #region GetIncrementID

        private Dictionary<ObjectType, int> _incrementids = null;
        

        /// <summary>
        /// Gets the next unique id for an Component of a specific type
        /// </summary>
        /// <param name="type">The type of Component to create the new ID for</param>
        /// <returns>A unique id</returns>
        public override string GetIncrementID(ObjectType type)
        {
            if (this._incrementids == null)
            {
                this._incrementids = new Dictionary<ObjectType, int>();
            }
            if(string.IsNullOrEmpty(this._idPrefix))
                this._idPrefix = GetRandomPrefix() + "_";

            int lastindex;

            if (this._incrementids.TryGetValue(type, out lastindex) == false)
                lastindex = 0;

            lastindex += 1;
            this._incrementids[type] = lastindex;
            
            string id = this._idPrefix + type.ToString() + lastindex.ToString();

            if (null != _originalPageResources)
            {
                //if we have an existing set of resources, then make sure we are not conflicting
                while (_originalPageResources.ContainsKey(id))
                {
                    lastindex += 1;
                    this._incrementids[type] = lastindex;
                    id = this._idPrefix + type.ToString()  + lastindex.ToString();
                }
            }
            return id;

        }

        private static string GetRandomPrefix(int length = 3)
        {
            var rnd = new Random();
            char[] all = new char[length];
            for (var i = 0; i < length; i++)
            {
                all[i] = (char)((int)'a' + rnd.Next(25));
            }

            return new string(all);
        }

        #endregion

        #region public override ComponentArrangement GetFirstArrangement()

        /// <summary>
        /// Override the GetFirstArrangement to return the arrangement of the first page
        /// </summary>
        /// <returns></returns>
        public override ComponentArrangement GetFirstArrangement()
        {
            if (this.Pages.Count > 0)
                return this.Pages[0].GetFirstArrangement();
            else
                return null;
        }

        #endregion

        //
        // resources
        //

        #region public PDFResourceCollection SharedResources {get;} + protected virtual PDFResourceCollection CreateResourceCollection()

        /// <summary>
        /// Internal reference to the resources in this document
        /// </summary>
        private PDFResourceCollection _resx;

        /// <summary>
        /// Gets the shared resources in this document - eg Images, Fonts, ProcSets
        /// </summary>
        public PDFResourceCollection SharedResources
        {
            get
            {
                if (_resx == null)
                    _resx = CreateResourceCollection();
                return _resx;
            }
        }

        /// <summary>
        /// Called from the SharedResources property if the collection is not currently initialized
        /// </summary>
        /// <returns>A new instance of a PDFResourceCollection</returns>
        protected virtual PDFResourceCollection CreateResourceCollection()
        {
            PDFResourceCollection resxcol = new PDFResourceCollection(this);
            return resxcol;
        }

        #endregion

        #region GetImageResource(string fullpath, Component owner bool create)

        /// <summary>
        /// Retrieves an image resource from this document, if it exists in the documents shared resources.
        /// If it does not and create is true, then the image will attempt to be loaded from any cache or ImageFactory
        /// </summary>
        /// <param name="fullpath">The fully resolvable path to the image, that cache providers can use to uniquely identify it, and factories use to load it.</param>
        /// <param name="owner" >The component that will own the resource</param>
        /// <param name="create">Flag for identifying if the resource should be loaded if it does not currently exist in the document.</param>
        /// <returns></returns>
        public PDFImageXObject GetImageResource(string fullpath, Component owner, bool create)
        {
            PDFImageXObject img = this.GetResource(PDFResource.XObjectResourceType, owner, fullpath, create) as PDFImageXObject;
            return img;

        }


        #endregion

        #region public PDFFontResource GetFontResource(PDFFont font)

        /// <summary>
        /// Retrieves a PDFFontResource for the specified font, using this documents FontMatcher
        /// </summary>
        /// <param name="font">The font to get the resource for</param>
        /// <param name="throwOnNotFound">If true (default) and the font is not found, then a null reference exception will be thrown, otherwise null will be returned</param>
        /// <param name="create">true if the PDFFontResource should be created if it is not already listed</param>
        /// <returns>A PDFFontResource that can be included in the document (or null if it is not loaded and should not be created)</returns>
        public virtual PDFFontResource GetFontResource(Font font, bool create, bool throwOnNotFound = true)
        {
            var found = this.FontMatcher.GetFont(font, create);
            if(null == found)
            {
                if(throwOnNotFound)
                    throw new NullReferenceException("No fonts could be found that matched the selector " + font.Selector.ToString() + " with name " + (font.FamilyName ?? "UNNAMED") + ", style " + font.FontStyle.ToString() + " and weight " + font.FontWeight);

                font.ClearResourceFont();
            }
            return found;

        }

        

        #endregion

        #region public virtual PDFResource GetResource(string resourceType, PDFComponent owner, string resourceKey, bool create)

        /// <summary>
        /// Explicit interface implementation that calls the GetResource Document method
        /// </summary>
        /// <param name="type">The type of resource to retrieve or optionally create <see cref="Scryber.PDF.Resources.PDFResource.FontDefnResourceType"/>
        /// and <see cref="Scryber.PDF.Resources.PDFResource.XObjectResourceType"/>
        /// for well known resource key types for fonts and ImageXObjects respectively</param>
        /// <param name="key">The resource key, Font.FullName or Image.SourcePath for example, as the fully identifiable resource key for these types.</param>
        /// <param name="create">If the create flag is true then the resource will attempt to be loaded automatically</param>
        /// <returns>A shared document resource. If created, then it will automatically be added to the documents shared resource collection</returns>
        ISharedResource IDocument.GetResource(string type, string key, bool create)
        {
            return this.GetResource(type, key, create);
        }

        /// <summary>
        /// Implements the return and optional creation of document specific resources such as images and fonts, that can be used in documents.
        /// </summary>
        /// <param name="resourceType">The Type of resource to get - retrieve or optionally create <see cref="Scryber.PDF.Resources.PDFResource.FontDefnResourceType"/>
        /// and <see cref="Scryber.PDF.Resources.PDFResource.XObjectResourceType"/>
        /// for well known resource key types for fonts and ImageXObjects respectively</param>
        /// <param name="resourceKey">The resource specific key name , Font.FullName or Image.SourcePath for example, as the fully identifiable resource key for these types.</param>
        /// <param name="owner">The required owner of the resource to be loaded</param>
        /// <param name="create">Specify true to attempt the loading of the resource if it is not currently in the collection</param>
        /// <returns>The loaded resource if any. If created, then it will automatically be added to the documents shared resource collection</returns>
        public virtual PDFResource GetResource(string resourceType, Component owner, string resourceKey, bool create)
        {
            PDFResource found = this.SharedResources.GetResource(resourceType, resourceKey);
            if (null == found && create)
            {
                found = this.CreateAndAddResource(resourceType, resourceKey, owner);
            }
            return found;
        }

        /// <summary>
        /// Implements the return and optional creation of document specific resources such as images and fonts, that can be used in documents.
        /// </summary>
        /// <param name="resourceType">The Type of resource to get - retrieve or optionally create <see cref="Scryber.PDF.Resources.PDFResource.FontDefnResourceType"/>
        /// and <see cref="Scryber.PDF.Resources.PDFResource.XObjectResourceType"/>
        /// for well known resource key types for fonts and ImageXObjects respectively</param>
        /// <param name="resourceKey">The resource specific key name , Font.FullName or Image.SourcePath for example, as the fully identifiable resource key for these types.</param>
        /// <param name="create">Specify true to attempt the loading of the resource if it is not currently in the collection</param>
        /// <returns>The loaded resource if any. If created, then it will automatically be added to the documents shared resource collection</returns>
        public virtual PDFResource GetResource(string resourceType, string resourceKey, bool create)
        {
            return this.GetResource(resourceType, this, resourceKey, create);
        }

        #endregion
        
        #region protected virtual PDFResource CreateAndAddResource(string resourceType, string resourceKey)

        /// <summary>
        /// Creates a new PDFResource of the required type based on the specified key and adds it to the SharedResource collection
        /// </summary>
        /// <param name="resourceType">The type of resource to create <see cref="Scryber.PDF.Resources.PDFResource.FontDefnResourceType"/>
        /// and <see cref="Scryber.PDF.Resources.PDFResource.XObjectResourceType"/>
        /// for well known resource key types for fonts and ImageXObjects respectively</param>
        /// <param name="resourceKey">The resource specific key name , Font.FullName or Image.SourcePath for example, as the fully identifiable resource key for these types.</param>
        /// <param name="owner">The required owner of the resource to be loaded</param>
        /// <returns>A new PDFResoure sub class instance</returns>
        protected virtual PDFResource CreateAndAddResource(string resourceType, string resourceKey, Component owner)
        {
            PDFResource created;
            if (resourceType == PDFResource.FontDefnResourceType)
            {
                created = CreateFontResource(resourceKey, owner);
            }
            else if (resourceType == PDFResource.XObjectResourceType)
            {
                created = CreateImageResource(resourceKey, owner);
            }
            else
                throw new ArgumentOutOfRangeException("resourceType");

            return created;
        }

        #endregion

        #region private PDFImageXObject CreateImageResource(string src)

        /// <summary>
        /// Creates a new image resource by loading from the factories base on the source and owner, and if not null then adds it to the documents shared resources
        /// </summary>
        /// <param name="src">The source to load the image for</param>
        /// <param name="owner">The owner of the image resource</param>
        /// <returns>The loaded image if successful or null</returns>
        private PDFImageXObject CreateImageResource(string src, Component owner)
        {
            using (this.PerformanceMonitor.Record(PerformanceMonitorType.Image_Load, src))
            {
                PDFImageXObject data = this.LoadImageData(owner, src);
                if (null != data)
                {
                    return RegisterXObjectResource(src, owner, data) as PDFImageXObject;
                }
                else
                    return null;
            }
        }

        #endregion

        #region private PDFFontResource CreateFontResource(string fullname)

        private PDFFontResource CreateFontResource(string fullname, Component owner)
        {
            using (this.PerformanceMonitor.Record(PerformanceMonitorType.Font_Load, fullname))
            {
                //var match = this.FontMatcher.GetFont(fullname, true);
                var defn = FontFactory.GetFontDefinition(fullname);
                if (null != defn)
                {
                    return RegisterFontResource(fullname, owner, defn);
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion

        #region public PDFResource EnsureResource(string type, string fullname, object resource) + 1 overload

        ISharedResource IDocument.EnsureResource(string type, string key, object resource)
        {
            return this.EnsureResource(type, key, resource);
        }

        public PDFResource EnsureResource(string type, string fullname, object resource)
        {
            return EnsureResource(type, fullname, this, resource);
        }

        public PDFResource EnsureResource(string type, string fullname, Component owner, object resource)
        {
            PDFResource found = this.SharedResources.GetResource(type, fullname);
            if (null != found)
                return found;

            if (type == PDFResource.XObjectResourceType)
                return this.RegisterXObjectResource(fullname, owner, resource);

            else if (type == PDFResource.FontDefnResourceType)
                return this.RegisterFontResource(fullname, owner, resource);

            else if (resource is PDFResource)
            {
                PDFResource exist = resource as PDFResource;
                this.SharedResources.Add(exist);
                return exist;
            }
            else
                throw new InvalidCastException("The resource type could not be determined, or is is not a PDFResourceType");

        }

        #endregion

        #region protected PDFImageXObject RegisterImageResource(string fullname, Component owner, object resource)

        protected PDFResource RegisterXObjectResource(string fullname, Component owner, object resource)
        {
            if (resource is ImageData)
            {
                ImageData data = resource as ImageData;
                string id = this.GetIncrementID(ObjectTypes.ImageXObject);
                
                PDFImageXObject img = PDFImageXObject.Load(data, this.RenderOptions.Compression, id);
                resource = img;
            }

            if (resource is PDFImageXObject)
            {
                PDFImageXObject xobj = resource as PDFImageXObject;
                this.SharedResources.Add(xobj);
                return xobj;
            }
            else if (resource is PDFResource)
            {
                this.SharedResources.Add(resource as PDFResource);
                return (resource as PDFResource);
            }
            else throw new InvalidCastException("The resource is not image data or image xobj, or standard XObjectResouce");
        }

        #endregion

        #region protected PDFFontResource RegisterFontResource(string fullname, Component owner, object resource)

        protected PDFFontResource RegisterFontResource(string fullname, Component owner, object resource)
        {

            if (resource is FontDefinition)
            {
                FontDefinition defn = (FontDefinition)resource;
                string id = this.GetIncrementID(ObjectTypes.FontResource);
                resource = PDFFontResource.Load(defn, id);
            }
            if (resource is PDFFontResource)
            {
                PDFFontResource rsrc = resource as PDFFontResource;
                if (rsrc.ResourceKey.Equals(fullname) == false)
                    rsrc.SetResourceKey(fullname);

                this.SharedResources.Add(rsrc);

                return rsrc;
            }
            else throw new InvalidCastException("The font resource is not a font definition or font resource");
        }

        #endregion

        #region public void RegisterExistingResource(PDFResource rsrc)

        private Dictionary<string, PDFResource> _originalPageResources;

        /// <summary>
        /// Records an existing resource in an original file, 
        /// so we don't get any name conflicts when adding an update to a new resource
        /// </summary>
        /// <param name="rsrc"></param>
        public void RegisterExistingResource(PDFResource rsrc)
        {
            if (null == _originalPageResources)
                _originalPageResources = new Dictionary<string, PDFResource>();
            _originalPageResources[rsrc.Name.Value] = rsrc;
        }

        #endregion

        //
        // Binding Content
        //

        private Dictionary<MimeType, IParserFactory> _parsers = null;
        private Generation.ParserSettings _settings = null;

        /// <summary>
        /// Returns the default binding content type for any complex content in this document
        /// </summary>
        /// <returns></returns>
        public virtual MimeType GetDefaultContentMimeType()
        {
            return MimeType.Xml;
        }

        /// <summary>
        /// Returns the parser factory that an create content based on the provided mime-type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual IComponentParser EnsureParser(MimeType type)
        {
            //Check and initialize the parser dictionary

            if (null == _parsers)
            {
                _parsers = this.InitKnownParsers();
            }

            if (null == _settings)
            {
                PDFReferenceResolver resolver = this.Resolver;
                if (null == resolver)
                {
                    ReferenceChecker checker = new ReferenceChecker(this.LoadedSource);
                    resolver = checker.Resolver;
                }
                _settings = DoCreateGeneratorSettings(resolver);
            }

            //try and get a parser for the mime-type

            IParserFactory factory;

            if (!_parsers.TryGetValue(type, out factory))
                throw new PDFParserException("The mime-type '" + type.ToString() + "' is not a known or supported mime type.");

            return factory.CreateParser(type, _settings);

            
        }

        protected virtual Dictionary<MimeType, IParserFactory> InitKnownParsers()
        {
            var config = ServiceProvider.GetService<IScryberConfigurationService>();
            ParserFactoryDictionary factories = config.ParsingOptions.GetParserFactories();
            return factories;
        }




        //
        // Save as ...
        //


        #region public void SaveAsPDF(System.IO.Stream stream, bool bind) + 1 overload


        /// <summary>
        /// Performs a complete initialization, load and then render the document to the path. 
        /// If a document exists at the specified path, then an IOException will be raised.
        /// </summary>
        /// <param name="path">The path to the file to write to</param>
        /// <remarks>Uses the Autobind property of the document to stipulate if data binding should also take place</remarks>
        public void SaveAsPDF(string path)
        {
            this.SaveAsPDF(path, System.IO.FileMode.CreateNew);
        }

        /// <summary>
        /// Performs a complete initialization, load and then render the document to the path.
        /// Uses the Autobind property to stipulate if data binding should also take place
        /// </summary>
        /// <param name="path"></param>
        /// <param name="mode"></param>
        public void SaveAsPDF(string path, System.IO.FileMode mode)
        {
            this.SaveAsPDF(path, mode, this.AutoBind);
        }


        /// <summary>
        /// Performs a complete initialization, load and then render the document to the path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="mode"></param>
        public void SaveAsPDF(string path, System.IO.FileMode mode, bool bind)
        {
            using (System.IO.Stream stream = this.DoOpenFileStream(path, mode))
            {
                this.DoSaveAs(stream, bind, OutputFormat.PDF);
            }
        }




        /// <summary>
        /// Performs a complete initialization, load and then render of the document as a PDF to the output stream
        /// </summary>
        /// <param name="stream">The stream to write the document to</param>
        /// <remarks>Uses the Autobind property to stipulate if data binding should also take place</remarks>
        public void SaveAsPDF(System.IO.Stream stream)
        {
            this.DoSaveAs(stream, this.AutoBind, OutputFormat.PDF);
        }

        public void SaveAsPDF(System.IO.Stream stream, bool bind)
        {
            this.DoSaveAs(stream, bind, OutputFormat.PDF);
        }

        /// <summary>
        /// Performs a complete initialization, load and then renders the document to the output stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="bind"></param>
        /// <param name="format">The output format of the document. The base implementation only supports the OutputFormat.PDF format, inheritors can provide their own implementation</param>
        protected virtual void DoSaveAs(System.IO.Stream stream, bool bind, OutputFormat format)
        {
            if (null == stream)
                throw new ArgumentNullException("stream");

            this.InitializeAndLoad(format);

            if (bind)
                this.DataBind(format);

            if (format == OutputFormat.PDF)
                this.RenderToPDF(stream);
            else
                throw new NotSupportedException("The output format '" + format + "' is not supported");
        }


        #endregion

        //
        // initialization and loading methods
        //

        #region public void InitializeAndLoad()

        /// <summary>
        /// Performs any initializing and loading, but does not bind or render the document
        /// </summary>
        public void InitializeAndLoad(OutputFormat format)
        {
            TraceLog log = this.TraceLog;
            PerformanceMonitor perfmon = this.PerformanceMonitor;
            ItemCollection items = this.Params;

            var initContext = CreateInitContext(log, perfmon, items, format);

            log.Begin(TraceLevel.Message, "Document", "Beginning Document Initialize");
            perfmon.Begin(PerformanceMonitorType.Document_Init_Stage);

            this.Init(initContext);

            perfmon.End(PerformanceMonitorType.Document_Init_Stage);
            
            log.End(TraceLevel.Message, "Document", "Completed Document Initialize");
            this.GenerationStage = DocumentGenerationStage.Initialized;

            LoadContext loadContext = CreateLoadContext(log, perfmon, items, format);

            log.Begin(TraceLevel.Message, "Document", "Beginning Document Load");
            perfmon.Begin(PerformanceMonitorType.Document_Load_Stage);

            this.Load(loadContext);

            perfmon.End(PerformanceMonitorType.Document_Load_Stage);
            log.End(TraceLevel.Message, "Document", "Completed Document Load");
            
            this.GenerationStage = DocumentGenerationStage.Loaded;
        }


        /// <summary>
        /// Create a new Initialization context with the required items collection, performance monitor, parameter items collection and output format
        /// </summary>
        /// <param name="log">The current execution trace log</param>
        /// <param name="perfmon">The current execution performance monitor</param>
        /// <param name="items">The current execution items collection</param>
        /// <param name="format">The current output format</param>
        /// <returns>A new instance of the InitContext</returns>
        protected virtual InitContext CreateInitContext(TraceLog log, PerformanceMonitor perfmon, ItemCollection items, OutputFormat format)
        {
            var context = new InitContext(
                items ?? throw  new ArgumentNullException(nameof(items))
                , log ?? throw  new ArgumentNullException(nameof(log))
                , perfmon ?? throw new ArgumentNullException(nameof(perfmon))
                , this
                , format ?? throw new ArgumentNullException(nameof(format)));
            
            this.PopulateContextBase(context);
            return context;
        }

        /// <summary>
        /// Create a new Load context with the required items collection, performance monitor, parameter items collection and output format
        /// </summary>
        /// <param name="log">The current execution trace log</param>
        /// <param name="perfmon">The current execution performance monitor</param>
        /// <param name="items">The current execution items collection</param>
        /// <param name="format">The current output format</param>
        /// <returns>A new instance of the LoadContext</returns>
        protected virtual LoadContext CreateLoadContext(TraceLog log, PerformanceMonitor perfmon, ItemCollection items, OutputFormat format)
        {
            var context = new LoadContext(
                items ?? throw  new ArgumentNullException(nameof(items))
                , log ?? throw  new ArgumentNullException(nameof(log))
                , perfmon ?? throw  new ArgumentNullException(nameof(perfmon))
                , this
                , format ?? throw  new ArgumentNullException(nameof(format)));
            this.PopulateContextBase(context);
            return context;
        }


        #endregion

        #region protected override void DoInit(PDFInitContext context)

        /// <summary>
        /// Overrides the base implementation to check the document initialization stage and initialize any inner contents such as Styles, Parameters, additions and data sources
        /// along with calling the base implementation to initialize the inner contents of the document.
        /// </summary>
        /// <param name="context">The current init context</param>
        /// <exception cref="PDFException">Thrown if this document has previously been initialized (GenerationState is not None)</exception>
        protected override void DoInit(InitContext context)
        {
            if (this.GenerationStage != DocumentGenerationStage.None)
                throw new PDFException(Errors.DocumentHasAlreadyBeenInitialized);

            if (this.HasParams)
                this.Params.Init(context);

            if (this.HasAdditions)
                this.Additions.Init(context);

            if (this.HasDataSources)
                this.DataSources.Init(context);

            this.DoInitStyles(context);

            base.DoInit(context);

            this.GenerationStage = DocumentGenerationStage.Initialized;
        }

        #endregion

        #region protected virtual void DoInitStyles(PDFInitContext context)

        /// <summary>
        /// Invokes the initialization of all styles in this document that support it (implement the IComponent interface).
        /// </summary>
        /// <param name="context">The current init context</param>
        /// <exception cref="PDFException">Raised if there is an error during the initialization process</exception>
        protected virtual void DoInitStyles(InitContext context)
        {
            if (this.Styles != null && this.Styles.Count > 0)
            {
                foreach (StyleBase style in this.Styles)
                {
                    try
                    {
                        if (style is IComponent)
                        {
                            if (context.ShouldLogDebug)
                                context.TraceLog.Add(TraceLevel.Debug, "PDF Document", "Initializing the style '" + style.ToString() + "'");
                            
                            (style as IComponent).Init(context);
                        }
                        else if (context.ShouldLogDebug)
                            context.TraceLog.Add(TraceLevel.Debug, "PDF Document", "The style '" + style.ToString() + "' does not implement IComponent, so cannot be initialized");
                    }
                    catch (PDFException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        string msg = $"Could not init the data to the style '{style}' : {ex.Message}";
                        throw new PDFException(msg, ex);
                    }
                }
            }
        }

        #endregion

        #region protected override void DoLoad(PDFLoadContext context)

        /// <summary>
        /// Overrides the base implementation to check the document generation stage and invoke the loading any children.
        /// </summary>
        /// <param name="context">The current load context</param>
        /// <exception cref="PDFException">If this document has not been initialized, or already loaded an error will be raised</exception>
        /// <remarks>If executing this outside of the standard document lifecycle then the M<see cref="InitializeAndLoad"/> method offers an easier route.
        /// It is also the ideal place where inheritors can override this method if they add any contents or collections that also need loading.</remarks>
        protected override void DoLoad(LoadContext context)
        {
            if (this.GenerationStage < DocumentGenerationStage.Initialized)
                throw new PDFException(Errors.DocumentHasNotBeenInitialized);

            else if (this.GenerationStage > DocumentGenerationStage.Initialized)
                throw new PDFException(Errors.DocumentHasAlreadyBeenLoaded);

            if (this.HasAdditions)
                this.Additions.Load(context);

            if (this.HasDataSources)
                this.DataSources.Load(context);

            this.DoLoadStyles(context);

            base.DoLoad(context);

            this.GenerationStage = DocumentGenerationStage.Loaded;
        }

        #endregion

        #region protected virtual void DoLoadStyles(PDFDataContext context)

        /// <summary>
        /// Invokes the loading of all styles in this document that support it (implement the IComponent interface).
        /// </summary>
        /// <param name="context">The current load context</param>
        /// <exception cref="PDFException">Raised if there is an error during the loading process</exception>
        protected virtual void DoLoadStyles(LoadContext context)
        {
            if (this.Styles != null && this.Styles.Count > 0)
            {
                foreach (StyleBase style in this.Styles)
                {
                    try
                    {
                        if (context.ShouldLogDebug)
                            context.TraceLog.Add(TraceLevel.Debug, "PDF Document", "Loading the style '" + style.ToString() + "'");

                        if (style is IComponent)
                            (style as IComponent).Load(context);
                    }
                    catch (PDFException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        string msg = string.Format("Could not load the data to the style '{0}' : {1}", style, ex.Message);
                        throw new PDFException(msg, ex);
                    }
                }
            }
        }

        #endregion
        
        //
        // Data binding methods
        //
        
        #region public void DataBind(OutputFormat format)

        /// <summary>
        /// Main entry method for an entire document to be data bound.
        /// This will make sure any expressions bound on values and attributes are bound too.
        /// </summary>
        /// <param name="format">The ultimate output format of the document. This will be passed to the binding context, and if null an exception wil be thrown</param>
        /// <remarks>The data-binding stage is a key stage within the document creation process. It should occur once a document has been initialized and loaded</remarks>
        public void DataBind(OutputFormat format)
        {
            TraceLog log = this.TraceLog;
            PerformanceMonitor perfmon = this.PerformanceMonitor;
            ItemCollection items = this.Params;

            if (null == format)
                throw new ArgumentNullException(nameof(format),
                    @"The format was null for binding, use one of the static OutputFormat types, or one of your own.");
            
            DataContext context = this.CreateDataContext(log, perfmon, items, format);

            context.TraceLog.Begin(TraceLevel.Message, "Document", "Beginning Document Databind");
            context.PerformanceMonitor.Begin(PerformanceMonitorType.Document_Bind_Stage);

            this.DataBind(context);

            context.PerformanceMonitor.End(PerformanceMonitorType.Document_Bind_Stage);
            context.TraceLog.End(TraceLevel.Message, "Document", "Completed Document Databind");
            this.GenerationStage = DocumentGenerationStage.Bound;
        }

        /// <summary>
        /// Creates a new data context that is passed to the main data binding method
        /// </summary>
        /// <returns></returns>
        protected virtual DataContext CreateDataContext(TraceLog log, PerformanceMonitor perfmon, ItemCollection items, OutputFormat format)
        {

            DataContext context = new DataContext(items, log, perfmon, this, format);
            this.PopulateContextBase(context);
            return context;
        }

        #endregion

        #region protected override void DoDataBind(PDFDataContext context, bool includeChildren)

        /// <summary>
        /// Overrides the default implementation, calling the base method within it
        /// </summary>
        /// <param name="context">The current data context</param>
        /// <param name="includeChildren">Flag for marking the children to be bound (including any styles, additions, info and data sources</param>
        /// <remarks>If include children, then this method will also bind any inner content and styles.</remarks>
        protected override void DoDataBind(DataContext context, bool includeChildren)
        {
            if (this.GenerationStage < DocumentGenerationStage.Loaded)
                throw new PDFException(Errors.DocumentHasNotBeenLoaded);

            if (this.GenerationStage >= DocumentGenerationStage.Disposed)
                throw new PDFException(Errors.DocumentHasBeenDisposed);

            if (this.GenerationStage > DocumentGenerationStage.Loaded)
                throw new PDFException(Errors.DocumentCannotBeBoundAtThisStage);

            if (includeChildren)
            {
                if (this.HasAdditions)
                    this.Additions.DataBind(context);

                if (null != this.Info)
                    this.Info.DataBind(context);

                if (this.HasDataSources)
                    this.DataSources.DataBind(context);

                this.DoBindStyles(context);

            }

            base.DoDataBind(context, includeChildren);


            this.GenerationStage = DocumentGenerationStage.Bound;

        }

        #endregion

        #region protected virtual void DoBindStyles(PDFDataContext context)

        /// <summary>
        /// Databinds all the styles in this document, inheritors can override the default functionality to add their own bindings
        /// </summary>
        /// <param name="context">The current databind context</param>
        /// <exception cref="PDFDataException">Thrown if there is an error raised during the binding of the styles</exception>
        protected virtual void DoBindStyles(DataContext context)
        {
            if (this.Styles != null && this.Styles.Count > 0)
            {
                foreach (StyleBase style in this.Styles)
                {
                    try
                    {
                        if (context.ShouldLogDebug)
                            context.TraceLog.Add(TraceLevel.Debug, "PDF Document", "Binding the style '" + style.ToString() + "'");
                        if (style is IBindableComponent)
                            (style as IBindableComponent).DataBind(context);
                    }
                    catch (PDFDataException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        string msg = string.Format("Could not bind the data to the style '{0}' : {1}", style, ex.Message);
                        throw new PDFDataException(msg, ex);
                    }
                }
            }
        }

        #endregion

        #region public virtual IXmlNamespaceResolver CreateNamespaceResolver(PDFXmlNamespaceCollection namespaceDeclarations, XmlNameTable nameTable)

        /// <summary>
        /// Returns a new namespace manager for the namespaces name table
        /// </summary>
        /// <param name="namespaceDeclarations">The explicit namespaces with prefixes to add to the resolver</param>
        /// <param name="nameTable">The original xml document nametable</param>
        /// <returns>A new IXmlNamespaceResolver ( an instance of the XmlNamespaceManager)</returns>
        public virtual IXmlNamespaceResolver CreateNamespaceResolver(PDFXmlNamespaceCollection namespaceDeclarations, XmlNameTable nameTable)
        {
            XmlNamespaceManager mgr = new XmlNamespaceManager(nameTable);
            
            foreach (XmlNamespaceDeclaration dec in namespaceDeclarations)
            {
                if (string.IsNullOrEmpty(dec.Prefix))
                    throw new NullReferenceException(string.Format(Errors.CannotDeclareNamespaceWithoutPrefix, dec.NamespaceURI));
                
                mgr.AddNamespace(dec.Prefix, dec.NamespaceURI);
            }
            return mgr;
        }

        #endregion

        #region public virtual IPDFDataProvider AssertGetProvider(string key)

        /// <summary>
        /// Attempts to retrieve a valid data provider from this documents registered providers.
        /// If it fails an exception will be thrown.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual IDataProvider AssertGetProvider(string key)
        {
            IDataProvider provider;
            if (this.DataProviders.TryGetProvider(key, out provider))
            {
                string error;
                if (!provider.IsValid(out error))
                    throw new InvalidOperationException("The " + key + " provider is not valid: " + error);
                else
                    return provider;
            }
            else
                throw new InvalidOperationException("The " + key + " has not been registred. Data providers must be registered by any components requiring them and fullfilled by the requestor");
        }

        #endregion

        //
        // Component overrides - invoke for Additions
        //

        #region internal override void RegisterPreLayout(PDFLayoutContext context)

        /// <summary>
        /// Overrides the base implementation to register pre layout for any additions as well
        /// </summary>
        /// <param name="context"></param>
        internal override void RegisterPreLayout(LayoutContext context)
        {
            if (this.HasAdditions)
                this.Additions.RegisterPreLayout(context);

            if(null != this.Styles)
            {
                if (this.Styles.ShouldUseIndex())
                    context.TraceLog.Add(TraceLevel.Message, "Document", "Document style collection is indexed as count is " + this.Styles.Count);
            }

            base.RegisterPreLayout(context);
        }

        #endregion

        #region internal override void RegisterLayoutComplete(PDFLayoutContext context)

        /// <summary>
        /// Overrides the base implementation to register layout complete for any additions as well
        /// </summary>
        /// <param name="context"></param>
        internal override void RegisterLayoutComplete(LayoutContext context)
        {
            if (this.HasAdditions)
                this.Additions.RegisterLayoutComplete(context);

            base.RegisterLayoutComplete(context);
        }

        #endregion

        #region internal override void RegisterPreRender(PDFRenderContext context)

        /// <summary>
        /// Overrides the base implementation to register pre render for any additions as well
        /// </summary>
        /// <param name="context"></param>
        internal override void RegisterPreRender(RenderContext context)
        {
            base.RegisterPreRender(context);

            if (this.HasAdditions)
                this.Additions.RegisterPreRender(context);
        }

        #endregion

        #region internal override void RegisterPostRender(PDFRenderContext context)

        /// <summary>
        /// Overrides the base implementation to register post render for any additions as well
        /// </summary>
        /// <param name="context"></param>
        internal override void RegisterPostRender(RenderContext context)
        {
            base.RegisterPostRender(context);

            if (this.HasAdditions)
                this.Additions.RegisterPostRender(context);

        }

        #endregion

        //
        // rendering
        //

        #region RenderToPDF(string path, System.IO.FileMode mode) + 2 Overloads


        public virtual void RenderToPDF(System.IO.Stream toStream)
        {
            PDFRenderContext context = this.CreateRenderContext();

            using (PDFWriter writer = this.DoCreateRenderWriter(toStream, context))
            {
                this.RenderToPDF(context, writer);
            }
        }

        

        /// <summary>
        /// Preforms the actual rendering of the document to the writer with the context
        /// </summary>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        /// <remarks>This method performs the required actions to render a PDF document.
        /// 
        /// Layout - measures and explicitly lays out each visual component 
        /// 
        /// Writing - performs the actual output onto the writer</remarks>
        public virtual void RenderToPDF(PDFRenderContext context, PDFWriter writer)
        {


            Style style = this.GetAppliedStyle();


            //Layout components before rendering
            PDFLayoutContext layoutcontext = CreateLayoutContext(style, context.Items, context.TraceLog, context.PerformanceMonitor);
            this.RegisterPreLayout(layoutcontext);

            context.TraceLog.Begin(TraceLevel.Message, "Document", "Beginning Document layout");
            IPDFLayoutEngine engine = null;

            context.PerformanceMonitor.Begin(PerformanceMonitorType.Document_Layout_Stage);
            using (engine = this.GetEngine(null, layoutcontext))
            {
                engine.Layout(layoutcontext, style);
            }
            context.PerformanceMonitor.End(PerformanceMonitorType.Document_Layout_Stage);

            this.GenerationStage = DocumentGenerationStage.Laidout;
            context.TraceLog.End(TraceLevel.Message, "Document", "Completed Document layout");
            PDFLayoutDocument doc = layoutcontext.DocumentLayout;

            this.RegisterLayoutComplete(layoutcontext);

            if (this.AppendTraceLog)
                this.DoOutputAndAppendToPDF(doc, context, writer);
            else
                this.DoOutputToPDF(doc, context, writer);


        }

        #endregion

        #region protected virtual void DoOutputAndAppendToPDF(PDFLayoutDocument doc, PDFRenderContext context, PDFWriter writer)

        /// <summary>
        /// Wraps the OutputToPDF in a separate stream then builds an amended 
        /// document with the trace log after the original content
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        protected virtual void DoOutputAndAppendToPDF(PDFLayoutDocument doc, PDFRenderContext context, PDFWriter writer)
        {
            // create internal memory stream and PDFWriter to generate the document to, 
            // and then apply as a file to the PDFTraceLogDocument

            System.IO.MemoryStream interim = null;
            PDFWriter baseWriter = null;
            PDFWriter final = writer;

            try
            {
                interim = new System.IO.MemoryStream();
                baseWriter = this.DoCreateRenderWriter(interim, context);
                this.DoOutputToPDF(doc, context, baseWriter);

                baseWriter.Dispose();
                baseWriter = null;
                DateTime end = DateTime.Now;
                TimeSpan total = end - this.StartTime;
                // we know no exceptions were rasied so we can now generate the appended document.
                PDFDocumentGenerationData genData = this.DoGetGenerationData(interim.Length, total, context);
                PDFFile origFile = null;
                interim.Flush();
                interim.Position = 0;

                if (null != this.TraceLog)
                    this.TraceLog.SetRecordLevel(TraceRecordLevel.Off);

                context.TraceLog.Begin(TraceLevel.Message, "Document", "Starting to generate the appended trace log");
                Document appended = null;
                try
                {
                    origFile = PDFFile.Load(interim, new Scryber.Logging.DoNothingTraceLog(TraceRecordLevel.Off));
                    appended = CreateTraceLogAppendDocument(genData, origFile, doc.DocumentComponent.SharedResources);
                    appended.SaveAsPDF(writer.InnerStream, true);
                }
                finally
                {
                    if (null != origFile)
                        origFile.Dispose();
                    if (null != appended)
                        appended.Dispose();

                    context.TraceLog.End(TraceLevel.Message, "Document", "Completed the generation of the appended trace log");
                }
            }
            finally
            {
                if (null != baseWriter)
                    baseWriter.Dispose();
                if (null != interim)
                    interim.Dispose();
            }
        }

        private Document CreateTraceLogAppendDocument(PDFDocumentGenerationData genData, PDFFile origFile, PDFResourceCollection resources)
        {
            var appended = new TraceLogDocument(this.FileName, origFile, genData, resources);
            appended.RenderOptions = this.RenderOptions;
            appended.DocumentID = this.DocumentID;
            return appended;
        }

        /// <summary>
        /// Creates and populates the Document Generation Data
        /// </summary>
        /// <param name="docSize"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private PDFDocumentGenerationData DoGetGenerationData(long docSize, TimeSpan totalTime, PDFRenderContext context)
        {
            PDFDocumentGenerationData data = new PDFDocumentGenerationData();
            data.DocumentSize = docSize;
            data.DocumentGenerationTime = totalTime;
            data.DocumentID = this.DocumentID;
            System.Reflection.Assembly assm = typeof(Document).Assembly;
            data.ScryberVersion = assm.GetName().Version;
            data.ScryberFileVersion = Scryber.Utilities.FrameworkHelper.CurrentVersion;
            data.TemplatePath = this.LoadedSource;
            data.TraceLevel = context.TraceLog.RecordLevel;
            data.DocumentInfo = this.Info;
            data.DocumentViewerPrefs = this.ViewPreferences;
            data.TraceLog = this.TraceLog.GetLogWithName(TraceLog.ScryberAppendTraceLogName) as Scryber.Logging.CollectorTraceLog;
            data.Namespaces = this.NamespaceDeclarations;
            data.PerformanceMetrics = this.PerformanceMonitor;
            return data;
        }

        #endregion

        #region protected virtual PDFObjectRef DoOutputToPDF(PDFLayoutDocument doc, PDFRenderContext context, PDFWriter writer)

        /// <summary>
        /// Outputs all the PDF instructions to the PDFWriter for the document layout.
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        protected virtual PDFObjectRef DoOutputToPDF(PDFLayoutDocument layout, PDFRenderContext context, PDFWriter writer)
        {

            if (layout.DocumentComponent != this)
                throw new PDFException(Errors.TryingToOutputADifferentDocumentLayout);

            PDFObjectRef root = null;
            context.TraceLog.Begin(TraceLevel.Message, "Document", "Beginning output of the document with render options : " + layout.RenderOptions.ToString());
            context.PerformanceMonitor.Begin(PerformanceMonitorType.Document_Render_Stage);

            this.RegisterPreRender(context);

            //Reset the page index
            context.PageIndex = 0;
            context.PageCount = layout.TotalPageCount;

            if (null != _additions)
            {
                _additions.OutputToPDF(context, writer);
            }

            root = layout.OutputToPDF(context, writer);



            this.RegisterPostRender(context);

            context.PerformanceMonitor.End(PerformanceMonitorType.Document_Render_Stage);


            context.TraceLog.End(TraceLevel.Message, "Document", "Completed output of the document");

            this.GenerationStage = DocumentGenerationStage.Written;

            //context.PerformanceMonitor.OutputToTraceLog(context.TraceLog);

            return root;
        }

        #endregion

        #region protected virtual PDFLayoutContext CreateLayoutContext(PDFStyle style, PDFItemCollection items, PDFTraceLog log)

        /// <summary>
        /// Creates the layout context used when performing the layout of a complete document
        /// </summary>
        /// <param name="style">The base style for the document</param>
        /// <param name="items">A PDFItemCollection</param>
        /// <param name="log">The log to use</param>
        /// <returns>A new layout context</returns>
        protected virtual PDFLayoutContext CreateLayoutContext(Style style, ItemCollection items, TraceLog log, PerformanceMonitor perfmon)
        {
            PDFLayoutContext context = new PDFLayoutContext(style, items, log, perfmon, this);
            PopulateContextBase(context);
            return context;
        }

        #endregion

        #region public IPDFLayoutEngine GetEngine(...)

        IPDFLayoutEngine IPDFViewPortComponent.GetEngine(IPDFLayoutEngine parent, PDFLayoutContext context, Style style)
        {
            return this.GetEngine(parent, context);
        }

        /// <summary>
        /// Gets the layout engine for this document.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="context"></param>
        /// <returns>Either a ModifyDocumentLayoutEngine or a standard DocumentLayoutEngine depending on if this document has an original source</returns>
        public virtual IPDFLayoutEngine GetEngine(IPDFLayoutEngine parent, PDFLayoutContext context)
        {
            return new LayoutEngineDocument(this, parent, context);
        }

        #endregion

        #region protected virtual System.IO.Stream DoOpenFileStream(string path, System.IO.FileMode mode)

        /// <summary>
        /// Opens a new stream onto a file at the specified path. Inheritors can override this method to provide custom implementation
        /// </summary>
        /// <param name="path">The complete path</param>
        /// <param name="mode">The open file mode option</param>
        /// <returns>A new FileStream onto the path</returns>
        protected virtual System.IO.Stream DoOpenFileStream(string path, System.IO.FileMode mode)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            System.IO.FileStream fs = new System.IO.FileStream(path, mode);

            return fs;
        }

        #endregion

        #region protected virtual PDFRenderContext DoCreateRenderContext()

        /// <summary>
        /// Creates the render context for this document. Inheritors can override this method to provide custom implementation
        /// </summary>
        /// <returns>A newly constructed render context for the operation</returns>
        protected virtual PDFRenderContext CreateRenderContext()
        {
            TraceLog log = this.TraceLog;
            PerformanceMonitor perfmon = this.PerformanceMonitor;


            Style def = this.CreateDefaultStyle();
            PDFRenderContext context = new PDFRenderContext(DrawingOrigin.TopLeft, 0, def, this.Params, log, perfmon, this);

            this.PopulateContextBase(context);
            return context;
        }

        #endregion
        
        #region private void PopulateContextBase(PDFContextBase context)

        /// <summary>
        /// Fills the context base options for any created context
        /// </summary>
        /// <param name="context"></param>
        protected virtual void PopulateContextBase(ContextBase context)
        {
            context.Compression = this.RenderOptions.Compression;
            context.Conformance = this.ConformanceMode;

        }

        #endregion

        #region protected virtual PDFWriter DoCreateRenderWriter(System.IO.Stream tostream, PDFRenderContext context)

        /// <summary>
        /// Creates a new PDFWriter instance for this document. Inheritors can override this method to provide custom implementation
        /// </summary>
        /// <param name="tostream">The output stream the writer should use</param>
        /// <returns>A new PDFWriter</returns>
        protected virtual PDFWriter DoCreateRenderWriter(System.IO.Stream tostream, PDFRenderContext context)
        {
            PDFWriter writer = this.RenderOptions.CreateWriter(this, tostream, 0, context.TraceLog);
            
            writer.UseHex = (this.RenderOptions.StringOutput == OutputStringType.Hex);
            writer.DefaultStreamFilters = GetStreamFilters();

            return writer;
        }

        #endregion

        #region protected virtual IStreamFilter[] GetStreamFilters()

        private static IStreamFilter[] _empty = new IStreamFilter[] { };
        /// <summary>
        /// Loads and returns the default stream filters
        /// </summary>
        /// <returns></returns>
        protected virtual IStreamFilter[] GetStreamFilters()
        {
            return _empty;
        }

        #endregion

        //
        // Path manipulation and loading
        //

        #region public PDFImageData LoadImageData(IPDFComponent parent, string source)

        /// <summary>
        /// Loads a file name or url from a source and returns the correct ImageXObject encapsulating the image data
        /// </summary>
        /// <param name="owner">The component that is requesting the resource (as source value may be releative to where the component was loaded from</param>
        /// <param name="src">The full path or absolute location of the image data file, or inline image data</param>
        /// <returns></returns>
        public PDFImageXObject LoadImageData(IComponent owner, string src)
        {
            ImageData data = null;
            string key = src;

            try
            {
                if (string.IsNullOrEmpty(src))
                    throw new ArgumentNullException(nameof(src));

                src = RemoveReturns(src);
                src = owner.MapPath(src);

                
                if (this.SharedResources.GetResource(PDFResource.XObjectResourceType, src) is PDFImageXObject exists)
                    return exists;
                
                if (this.ImageFactories.TryGetMatch(src, out var factory))
                {
                    if (factory.ShouldCache)
                    {
                        if (this.CacheProvider.TryRetrieveFromCache(ObjectTypes.ImageData.ToString(), src,
                                out var cached))
                        {
                            if(this.TraceLog.ShouldLog(TraceLevel.Verbose))
                                this.TraceLog.Add(TraceLevel.Verbose, "Document","Cache matched for the image source " + (src.Length > 100 ? (src.Substring(50)+ "..." + src.Substring(src.Length-10)): src) + " adding to the resources and returning.");

                            data = (ImageData) cached;
                            key = GetIncrementID(ObjectTypes.ImageXObject);
                        }
                    }

                    if (null == data)
                    {
                        if (this.TraceLog.ShouldLog(TraceLevel.Verbose))
                            this.TraceLog.Add(TraceLevel.Verbose, "Document",
                                "Factory '" + factory.Name + "' found for image with source " +
                                (src.Length > 100
                                    ? (src.Substring(50) + "..." + src.Substring(src.Length - 10))
                                    : src) + " adding to the resources and returning.");

                        data = LoadImageDataFromFactory(owner, factory, src);
                        key = GetIncrementID(ObjectTypes.ImageXObject);
                    }
                }
                else if (this.RenderOptions.AllowMissingImages)
                {
                    this.TraceLog.Add(TraceLevel.Error, "Document", "Could not load the image data for '" + key + "'. As missing images are allowed, returning nothing.");
                    data = null;
                }
                else
                    throw new Scryber.Imaging.PDFImageFormatException("Could not load the image from source " + src);

                if (null != data)
                {
                    IStreamFilter[] filters = null;
                    if (this.RenderOptions.Compression == OutputCompressionType.FlateDecode)
                        filters = new IStreamFilter[] { new PDF.PDFDeflateStreamFilter() };
                    
                    PDFImageXObject xobj = PDFImageXObject.Load(data, filters, key);
                    return xobj;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                TraceLog log = this.TraceLog;
                log.Add(TraceLevel.Error, "Document", "Could not load the image data for '" + key + "'. Failed with message : " + ex.Message, ex);

                if (this.RenderOptions.AllowMissingImages)
                {
                    return null;
                }
                else
                    throw;
            }
        }


        //private static readonly Regex whiteSpace = new Regex(@"\s+");
        private StringBuilder _buffer = new StringBuilder(0);
        private static char[] _replace = new char[] { '\r','\n' };

        private string RemoveReturns(string data)
        {
            _buffer.Clear();

            var index = data.IndexOf('\n');
            if (index > 0)
            {
                var lines = data.Split('\n');
                foreach (var line in lines)
                {
                    var trimmed = line.Trim();
                    _buffer.Append(trimmed);
                }

                return _buffer.ToString();
            }
            else
                return data;
        }

        private ImageData LoadImageDataFromFactory(IComponent owner, IPDFImageDataFactory factory, string path)
        {
            ImageData data = factory.LoadImageData(this, owner, path);

            if (null != data && factory.ShouldCache)
            {
                var expires = this.GetImageCacheExpires();
                this.CacheProvider.AddToCache(ObjectTypes.ImageData.ToString(), path, data, expires);
            }

            return data;
        }


        private DateTime GetImageCacheExpires()
        {
            int mins = this.RenderOptions.ImageCacheDurationMinutes;
            if (mins < 0)
                return Scryber.Caching.PDFCacheProvider.NoAbsoluteExpiration;
            else if (mins == 0)
                return DateTime.MinValue;
            else
                return DateTime.Now.AddMinutes(mins);
        }

        #endregion

        #region protected override string MapPath(string path)

        /// <summary>
        /// Overrides the default behavior to map any file reference to either an absolute uri or filepath.
        /// </summary>
        /// <param name="path">The path to map</param>
        /// <returns></returns>
        public override string MapPath(string path)
        {
            return this.MapPath(path, out var isfile);
        }

        

        #endregion        

        //
        // Find a component
        //

        #region  public PDFComponent FindAComponent(string id) + 1 overload

        public override Component FindAComponentById(string id)
        {
            Component ele = null;
            if (this.ID.Equals(id))
                return this;
            else if (this.FindAComponentById(this.Pages.InnerList, id, out ele))
                return ele;
            else if (this.FindAComponentById(this.DataSources.InnerList, id, out ele))
                return ele;
            else
                return null;
        }

        IComponent IDocument.FindComponentById(string id)
        {
            return this.FindAComponentById(id);
        }
        #endregion

        #region  public PDFComponent FindAComponentByName(string name) + 1 overload

        public override Component FindAComponentByName(string name)
        {
            Component value;
            if(this._namedictionary.TryGetValue(name, out value))
                return value;

            Component ele = null;
            if (string.Equals(this.Name, name))
                return this;
            else if (this.FindAComponentByName(this.Pages.InnerList, name, out ele))
                return ele;
            else if (this.FindAComponentByName(this.DataSources.InnerList, name, out ele))
                return ele;
            else
                return null;
        }


        #endregion

        //
        // parse methods
        //

        #region protected override IPDFComponent ParseComponentAtPath(string path)

        /// <summary>
        /// Overrides  the default behaviour to parse the component using the static Document.Parse method
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected override IComponent ParseComponentAtPath(string path)
        {
            IComponent parsed = null;
            try
            {
                parsed = Document.Parse(path, this.Resolver ?? new ReferenceChecker(string.Empty).Resolver);
            }
            catch (System.IO.FileNotFoundException ex)
            {
                this.HandleRemoteReferenceException(path, ex);
            }
            catch (System.Security.SecurityException ex)
            {
                this.HandleRemoteReferenceException(path, ex);
            }
            catch (System.IO.DirectoryNotFoundException ex)
            {
                this.HandleRemoteReferenceException(path, ex);
            }
            catch (System.IO.DriveNotFoundException ex)
            {
                this.HandleRemoteReferenceException(path, ex);
            }
            catch (System.Net.WebException ex)
            {
                this.HandleRemoteReferenceException(path, ex);
            }
            return parsed;
        }

        private void HandleRemoteReferenceException(string path, System.Exception ex)
        {
            var config = ServiceProvider.GetService<IScryberConfigurationService>();

            ParserReferenceMissingAction action =config.ParsingOptions.MissingReferenceAction;
            switch (action)
            {
                case ParserReferenceMissingAction.LogError:
                    TraceLog log = this.TraceLog;
                    if (null != log)
                        log.Add(TraceLevel.Error, "PDFDocument", "File at path '" + path + " does not exist or could not be opened", ex);
                    else
                        throw ex;
                    break;
                case ParserReferenceMissingAction.DoNothing:
                    //Ignore the missing path
                    break;
                case ParserReferenceMissingAction.RaiseException:
                default:
                    throw new PDFParserException("Could not parse the file at path '" + path + "'. " + ex.Message, ex);
            }
        }

        #endregion

        #region public static IComponent Parse(string fullpath) + 11 overloads

        public static IComponent Parse(string fullpath)
        {
            using (System.IO.Stream stream = new System.IO.FileStream(fullpath,System.IO.FileMode.Open,System.IO.FileAccess.Read))
            {
                
                ReferenceChecker checker = new ReferenceChecker(fullpath);

                IComponent comp = Parse(fullpath, stream, ParseSourceType.LocalFile, checker.Resolver);

                if (comp is IRemoteComponent)
                    ((IRemoteComponent)comp).LoadedSource = fullpath;

                return comp;
            }
        }

        public static IComponent Parse(System.IO.Stream stream, ParseSourceType type)
        {
            ReferenceChecker checker = new ReferenceChecker(string.Empty);
            IComponent comp = Parse(string.Empty, stream, type, checker.Resolver);

            return comp;
        }

        public static IComponent Parse(string path, System.IO.Stream stream)
        {
            return Parse(path, stream, ParseSourceType.Other);
        }

        public static IComponent Parse(string path, System.IO.Stream stream, ParseSourceType type)
        {
            ReferenceChecker checker = new ReferenceChecker(path);
            var comp = Parse(path, stream, type, checker.Resolver);

            return comp;
        }

        public static IComponent Parse(System.IO.TextReader reader, ParseSourceType type)
        {
            ReferenceChecker checker = new ReferenceChecker(string.Empty);
            IComponent comp = Parse(string.Empty, reader, type, checker.Resolver);

            return comp;
        }

        public static IComponent Parse(System.Xml.XmlReader reader, ParseSourceType type)
        {
            ReferenceChecker checker = new ReferenceChecker(string.Empty);
            IComponent comp = Parse(string.Empty, reader, type, checker.Resolver);

            return comp;
        }

        public static IComponent Parse(string fullpath, PDFReferenceResolver resolver)
        {
            ParserConformanceMode mode = ParserConformanceMode.Lax;

            ParserSettings settings = Document.CreateGeneratorSettings(resolver, mode, null);
            return Parse(fullpath, resolver, settings);
        }

        public static IComponent Parse(string fullpath, PDFReferenceResolver resolver, ParserSettings settings)
        {
            if (Uri.IsWellFormedUriString(fullpath, UriKind.Absolute))
            {
                var client = Scryber.ServiceProvider.GetService<HttpClient>();
                bool disposeclient = false;

                if (null == client)
                {
                    client = new HttpClient();
                    disposeclient = true;
                }
                try
                {
                    using (var stream = client.GetStreamAsync(fullpath).Result)
                    {
                        IComponent comp = Parse(fullpath, stream, ParseSourceType.LocalFile, resolver, settings);
                        if (comp is IRemoteComponent)
                            ((IRemoteComponent)comp).LoadedSource = fullpath;
                        return comp;
                    }
                }
                finally
                {
                    if (disposeclient)
                        client.Dispose();
                }
                
            }
            else
            {
                using (System.IO.Stream stream = new System.IO.FileStream(fullpath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    IComponent comp = Parse(fullpath, stream, ParseSourceType.LocalFile, resolver, settings);
                    if (comp is IRemoteComponent)
                        ((IRemoteComponent)comp).LoadedSource = fullpath;
                    return comp;
                }
            }
        }

        public static IComponent Parse(string source, System.IO.Stream stream, ParseSourceType type, PDFReferenceResolver resolver)
        {
            ParserConformanceMode mode = ParserConformanceMode.Lax;
            ParserSettings settings = Document.CreateGeneratorSettings(resolver, mode, null);
            return Parse(source, stream, type, resolver, settings);
        }

        public static IComponent Parse(string source, System.IO.Stream stream, ParseSourceType type, PDFReferenceResolver resolver, ParserSettings settings)
        {
            IComponentParser parser = new Scryber.Generation.XMLParser(settings);

            IComponent comp = parser.Parse(source, stream, type);
            return comp;
        }

        public static IComponent Parse(string source, System.IO.TextReader textreader, ParseSourceType type, PDFReferenceResolver resolver)
        {
            ParserConformanceMode mode = ParserConformanceMode.Lax;
            ParserSettings settings = Document.CreateGeneratorSettings(resolver, mode, null);
            return Parse(source, textreader, type, resolver, settings);
        }

        public static IComponent Parse(string source, System.IO.TextReader textreader, ParseSourceType type, PDFReferenceResolver resolver, ParserSettings settings)
        {
            IComponentParser parser = new Scryber.Generation.XMLParser(settings);

            IComponent comp = parser.Parse(source, textreader, type);
            return comp;
        }

        public static IComponent Parse(string source, System.Xml.XmlReader xmlreader, ParseSourceType type, PDFReferenceResolver resolver)
        {
            ParserConformanceMode mode = ParserConformanceMode.Lax;
            ParserSettings settings = Document.CreateGeneratorSettings(resolver, mode, null);
            
            return Parse(source, xmlreader, type, resolver, settings);
        }

        public static IComponent Parse(string source, System.Xml.XmlReader xmlreader, ParseSourceType type, PDFReferenceResolver resolver, ParserSettings settings)
        {
            IComponentParser parser =  new Scryber.Generation.XMLParser(settings);

            IComponent comp = parser.Parse(source, xmlreader, type);
            return comp;
        }

        #endregion

        #region public static IComponent ParseHtml(string fullpath) + 6 overloads
        
        public static IComponent ParseHtml(string fullpath)
        {
            using (System.IO.Stream stream = new System.IO.FileStream(fullpath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {

                ReferenceChecker checker = new ReferenceChecker(fullpath);

                IComponent comp = ParseHtml(fullpath, stream, ParseSourceType.LocalFile, checker.Resolver);

                if (comp is IRemoteComponent)
                    ((IRemoteComponent)comp).LoadedSource = fullpath;

                return comp;
            }
        }

        public static IComponent ParseHtml(string source, System.IO.Stream stream, ParseSourceType type)
        {
            ReferenceChecker checker = new ReferenceChecker(source);

            IComponent comp = ParseHtml(source, stream, type, checker.Resolver);

            if (comp is IRemoteComponent)
                ((IRemoteComponent)comp).LoadedSource = source;

            return comp;
        }

        public static IComponent ParseHtml(string source, System.IO.Stream stream, ParseSourceType type, PDFReferenceResolver resolver)
        {
            ParserConformanceMode mode = ParserConformanceMode.Lax;
            ParserSettings settings = Document.CreateGeneratorSettings(resolver, mode, null);
            return ParseHtml(source, stream, type, resolver, settings);
        }

        public static IComponent ParseHtml(string source, System.IO.Stream stream, ParseSourceType type, PDFReferenceResolver resolver, ParserSettings settings)
        {
            IComponentParser parser = new Scryber.Html.Parsing.HTMLParser(settings);

            IComponent comp = parser.Parse(source, stream, type);
            return comp;
        }

        public static IComponent ParseHtml(string source, System.IO.TextReader reader, ParseSourceType type)
        {
            ReferenceChecker checker = new ReferenceChecker(source);

            IComponent comp = ParseHtml(source, reader, type, checker.Resolver);

            if (comp is IRemoteComponent)
                ((IRemoteComponent)comp).LoadedSource = source;

            return comp;
        }

        public static IComponent ParseHtml(string source, System.IO.TextReader reader, ParseSourceType type, PDFReferenceResolver resolver)
        {
            ParserConformanceMode mode = ParserConformanceMode.Lax;
            ParserSettings settings = Document.CreateGeneratorSettings(resolver, mode, null);
            return ParseHtml(source, reader, type, resolver, settings);
        }

        public static IComponent ParseHtml(string source, System.IO.TextReader reader, ParseSourceType type, PDFReferenceResolver resolver, ParserSettings settings)
        {
            IComponentParser parser = new Scryber.Html.Parsing.HTMLParser(settings);

            IComponent comp = parser.Parse(source, reader, type);
            return comp;
        }

        #endregion
        

        #region public IPDFComponent ParseTemplate(IPDFRemoteComponent owner, string referencepath, Stream stream) + 2 overloads

        public IComponent ParseTemplate(IRemoteComponent owner, System.IO.TextReader reader)
        {
            return ParseTemplate(owner, owner.LoadedSource, reader);
        }

        public IComponent ParseTemplate(IRemoteComponent owner, string referencepath, System.IO.Stream stream)
        {
            using (System.Xml.XmlReader xml = System.Xml.XmlReader.Create(stream))
            {
                return ParseTemplate(owner, referencepath, xml);
            }
        }



        public IComponent ParseTemplate(IComponent owner, string referencepath, System.IO.TextReader reader)
        {
            using (System.Xml.XmlReader xml = System.Xml.XmlReader.Create(reader))
            {
                return ParseTemplate(owner, referencepath, xml);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="referencepath"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        public virtual IComponent ParseTemplate(IComponent owner, string referencepath, System.Xml.XmlReader reader)
        {
            if (null == owner)
                throw RecordAndRaise.ArgumentNull("owner");
            if (null == reader)
                throw RecordAndRaise.ArgumentNull("reader");
            
            IComponent comp;

            PDFReferenceResolver resolver = this.Resolver;
            if (null == resolver)
            {
                ReferenceChecker checker = new ReferenceChecker(referencepath);
                resolver = checker.Resolver;
            }

            ParserSettings settings = this.DoCreateGeneratorSettings(resolver);
            //settings.Controller = this.Controller;

            IComponentParser parser = new Scryber.Generation.XMLParser(settings);

            parser.RootComponent = owner;

            comp = parser.Parse(referencepath, reader, ParseSourceType.Template);
            
            
            return comp;
        }

        #endregion

        #region protected virtual ParserSettings CreateGeneratorSettings(PDFReferenceResolver resolver)

        /// <summary>
        /// Creates the generator settings required to parse the XML files
        /// </summary>
        /// <param name="resolver"></param>
        /// <returns></returns>
        protected virtual ParserSettings DoCreateGeneratorSettings(PDFReferenceResolver resolver)
        {
            TraceLog log = this.TraceLog;
            PerformanceMonitor monitor = this.PerformanceMonitor;
            ParserConformanceMode mode = this.ConformanceMode;
            ParserLoadType loadtype = this.LoadType;
            object controller = this.Controller;

            return CreateGeneratorSettings(resolver, mode, 
                loadtype, log, monitor, controller);
        }

        private static ParserSettings CreateGeneratorSettings(PDFReferenceResolver resolver, ParserConformanceMode mode, object controller)
        {
            ParserConformanceMode conformance = mode;
            ParserLoadType loadtype = ParserLoadType.ReflectiveParser;
            var config = ServiceProvider.GetService<IScryberConfigurationService>();

            TraceLog log = config.TracingOptions.GetTraceLog();
            PerformanceMonitor perfmon = new PerformanceMonitor(log.RecordLevel <= TraceRecordLevel.Verbose);

            return CreateGeneratorSettings(resolver, conformance, loadtype, log, perfmon, controller);
        }

        protected static ParserSettings CreateGeneratorSettings(PDFReferenceResolver resolver, 
            ParserConformanceMode conformance, ParserLoadType loadtype, TraceLog log, PerformanceMonitor perfmon, object controller)
        {
            ParserSettings settings = new ParserSettings(typeof(TextLiteral), typeof(Whitespace)
                                                                    , typeof(ParsableTemplateGenerator)
                                                                    , typeof(TemplateInstance),
                                                                    resolver,
                                                                    conformance,
                                                                    loadtype,
                                                                    log,
                                                                    perfmon,
                                                                    controller);
            return settings;
        }

        #endregion


        //
        // parse document
        //

        #region public static Document ParseDocument(string path)

        /// <summary>
        /// Parses xml or xhtml content from a local file into a new <see cref="Document"/> instance.
        /// All relative paths to images, fonts, etc. should be relative to this path, unless a base path is set within the document.
        /// </summary>
        /// <param name="path">The full <paramref name="path"/> to the file, or a relative path from the current working directory</param>
        /// <returns>A complete parsed document, ready for any changes to be made and saving with <see cref="SaveAsPDF(Stream)" />or one of its overloads</returns>
        /// <exception cref="DirectoryNotFoundException" >Thrown if the specified path could not be found in the local file system.</exception>
        /// <exception cref="FileNotFoundException" >Thrown if the specified path could not be found in the local file system.</exception>
        /// <exception cref="PDFParserException">Thrown if the content of the file could not be parsed (invalid content)</exception>
        /// <exception cref="InvalidCastException" >Thrown if the parsed content was not a document type</exception>
        public static Document ParseDocument(string path)
        {
            var provider = Scryber.ServiceProvider.GetService<IPathMappingService>();

            bool isFile;
            var newPath = provider.MapPath(ParserLoadType.ReflectiveParser, path, null, out isFile);

            var parsed = Parse(path);

            if (!(parsed is Document))
                throw new InvalidCastException(String.Format(Errors.CannotConvertObjectToType, parsed.GetType(), typeof(Document)));

            Document doc = parsed as Document;

            return doc;

        }

        #endregion

        #region public static Document ParseDocument(Stream stream)

        /// <summary>
        /// Parses an xml or xhtml <see cref="Stream"/> into a new <see cref="Document"/> instance.
        /// As no path is provided, all relative paths to images, fonts, etc. will be relative to the current working directory,
        /// or a base path if set within the document itself
        /// </summary>
        /// <param name="stream">The stream to read the content from, positioned at the start of the content to be parsed.</param>
        /// <returns>A complete parsed document, ready for any changes to be made and saving with <see cref="SaveAsPDF(Stream)" />or one of its overloads </returns>
        /// <exception cref="PDFParserException">Thrown if the content of the file could not be parsed (invalid content)</exception>
        /// <exception cref="InvalidCastException">Thrown if the parsed content was not a document type</exception>
        public static Document ParseDocument(Stream stream)
        {
            return ParseDocument(stream, ParseSourceType.DynamicContent);
        }

        #endregion

        #region public static Document ParseDocument(stream stream, ParseSourceType type)

        /// <summary>
        /// Parses an xml or xhtml <see cref="Stream"/> into a new <see cref="Document"/> instance.
        /// As no path is provided, all relative paths to images, fonts, etc. will be relative to the current working directory,
        /// or a base path if set within the document itself
        /// </summary>
        /// <param name="stream">The stream to read the content from, positioned at the start of the content to be parsed.</param>
        /// <param name="type">The <see cref="ParseSourceType"/> as an indicator of where the content is sourced from to assist with loading any external resources.</param>
        /// <returns>A complete parsed document, ready for any changes to be made and saving with <see cref="SaveAsPDF(Stream)" />or one of its overloads</returns>
        /// <exception cref="PDFParserException">Thrown if the content of the file could not be parsed (invalid content)</exception>
        /// <exception cref="InvalidCastException">Thrown if the parsed content was not a document type</exception>
        public static Document ParseDocument(System.IO.Stream stream, ParseSourceType type)
        {
            ReferenceChecker checker = new ReferenceChecker(string.Empty);
            IComponent parsed = Parse(string.Empty, stream, type, checker.Resolver);

            if (!(parsed is Document))
                throw new InvalidCastException(String.Format(Errors.CannotConvertObjectToType, parsed.GetType(), typeof(Document)));

            Document doc = parsed as Document;

            return doc;
        }

        #endregion

        #region public static Document ParseDocument(System.IO.TextReader reader)

        /// <summary>
        /// Parses a <see cref="TextReader"/> with the inner xml or xhtml content read into a new <see cref="Document"/> instance.
        /// As no path is provided, all relative paths to images, fonts, etc. will be relative to the current working directory,
        /// or a base path if set within the document itself
        /// </summary>
        /// <param name="reader">The text reader to read the content from, positioned at the start of the content to be parsed.</param>
        /// <returns>A complete parsed document, ready for any changes and saving with <see cref="SaveAsPDF(Stream)" /> or one of its overloads</returns>
        /// <exception cref="PDFParserException">Thrown if the content of the file could not be parsed (invalid content)</exception>
        /// <exception cref="InvalidCastException">Thrown if the parsed content was not a document type</exception>
        public static Document ParseDocument(System.IO.TextReader reader)
        {
            return ParseDocument(reader, ParseSourceType.DynamicContent);
        }

        #endregion

        #region public static Document ParseDocument(System.IO.TextReader reader, ParseSourceType type)

        /// <summary>
        /// Parses a <see cref="TextReader"/> with the inner xml or xhtml content read into a new <see cref="Document"/> instance.
        /// As no path is provided, all relative paths to images, fonts, etc. will be relative to the current working directory,
        /// or a base path if set within the document itself
        /// </summary>
        /// <param name="reader">The text reader to read the content from, positioned at the start of the content to be parsed.</param>
        /// <param name="type">The <see cref="ParseSourceType"/> as an indicator of where the content is sourced from to assist with loading any external resources.</param>
        /// <returns>A complete parsed document, ready for any changes and saving with <see cref="SaveAsPDF(Stream)" /> or one of its overloads</returns>
        /// <exception cref="PDFParserException">Thrown if the content of the file could not be parsed (invalid content)</exception>
        /// <exception cref="InvalidCastException">Thrown if the parsed content was not a document type</exception>
        public static Document ParseDocument(System.IO.TextReader reader, ParseSourceType type)
        {
            ReferenceChecker checker = new ReferenceChecker(string.Empty);
            IComponent parsed = Parse(string.Empty, reader, type, checker.Resolver);

            if (!(parsed is Document))
                throw new InvalidCastException(String.Format(Errors.CannotConvertObjectToType, parsed.GetType(), typeof(Document)));

            Document doc = parsed as Document;

            return doc;
        }

        #endregion

        #region public static Document ParseDocument(System.Xml.XmlReader reader)

        /// <summary>
        /// Parses an <see cref="XmlReader"/> with the inner xml content read into a new <see cref="Document"/> instance.
        /// As no path is provided, all relative paths to images, fonts, etc. will be relative to the current working directory, or a base path if set within the document itself
        /// </summary>
        /// <param name="reader">The xml reader to read the content from.</param>
        /// <returns>A complete parsed document, ready for any changes to be made and saving with <see cref="SaveAsPDF(Stream)" /> or one of its overloads </returns>
        /// <exception cref="PDFParserException">Thrown if the content of the file could not be parsed (invalid content)</exception>
        /// <exception cref="InvalidCastException">Thrown if the parsed content was not a document type</exception>
        public static Document ParseDocument(System.Xml.XmlReader reader)
        {
            return ParseDocument(reader, ParseSourceType.DynamicContent);
        }

        #endregion

        #region public static Document ParseDocument(System.Xml.XmlReader reader, ParseSourceType type)

        /// <summary>
        /// Parses an <see cref="XmlReader"/> with the inner xml content read into a new <see cref="Document"/> instance.
        /// As no path is provided, all relative paths to images, fonts, etc. will be relative to the current working directory, or a base path if set within the document itself
        /// </summary>
        /// <param name="reader">The xml reader to read the content from.</param>
        /// <param name="type">The <see cref="ParseSourceType"/> as an indicator of where the content is sourced from to assist with loading any external resources.</param>
        /// <returns>A complete parsed document, ready for any changes to be made and saving with <see cref="SaveAsPDF(Stream)" /> or one of its overloads </returns>
        /// <exception cref="PDFParserException">Thrown if the content of the file could not be parsed (invalid content)</exception>
        /// <exception cref="InvalidCastException">Thrown if the parsed content was not a document type</exception>
        public static Document ParseDocument(System.Xml.XmlReader reader, ParseSourceType type)
        {
            ReferenceChecker checker = new ReferenceChecker(string.Empty);
            IComponent parsed = Parse(string.Empty, reader, type, checker.Resolver);

            if (!(parsed is Document))
                throw new InvalidCastException(String.Format(Errors.CannotConvertObjectToType, parsed.GetType(), typeof(Document)));

            Document doc = parsed as Document;

            return doc;
        }

        #endregion

        #region public static Document ParseDocument(System.IO.Stream stream, string path, ParseSourceType type)

        /// <summary>
        /// Parses an xml or xhtml <see cref="Stream"/> into a new <see cref="Document"/> instance.
        /// The path is provided, to map to all relative paths to contained images, fonts, etc.,
        /// unless a base path if set within the document itself
        /// </summary>
        /// <param name="stream">The stream to read the content from, positioned at the start of the content to be parsed.</param>
        /// <param name="path">The file path, url, or resource path originally used to read the content from, that can then be used for relative content</param>
        /// <param name="type">The <see cref="ParseSourceType"/> as an indicator of where the content is sourced from to assist with loading any external resources.</param>
        /// <returns>A complete parsed document, ready for any changes to be made and saving with <see cref="SaveAsPDF(Stream)" />or one of its overloads</returns>
        /// <exception cref="PDFParserException">Thrown if the content of the file could not be parsed (invalid content)</exception>
        /// <exception cref="InvalidCastException">Thrown if the parsed content was not a document type</exception>
        public static Document ParseDocument(System.IO.Stream stream, string path, ParseSourceType type)
        {
            ReferenceChecker checker = new ReferenceChecker(path);
            IComponent parsed = Parse(path, stream, type, checker.Resolver);

            if (!(parsed is Document))
                throw new InvalidCastException(String.Format(Errors.CannotConvertObjectToType, parsed.GetType(), typeof(Document)));

            Document doc = parsed as Document;

            return doc;
        }

        #endregion

        #region public static Document ParseDocument(System.IO.TextReader reader, string path, ParseSourceType type)

        /// <summary>
        /// Parses a <see cref="TextReader"/> with the inner xml or xhtml content read into a new <see cref="Document"/> instance.
        /// The path is provided, to map to all relative paths to contained images, fonts, etc.,
        /// unless a base path if set within the document itself
        /// </summary>
        /// <param name="reader">The text reader to read the content from, positioned at the start of the content to be parsed.</param>
        /// <param name="path">The file path, url, or resource path originally used to read the content from, that can then be used for relative content</param>
        /// <param name="type">The <see cref="ParseSourceType"/> as an indicator of where the content is sourced from to assist with loading any external resources.</param>
        /// <returns>A complete parsed document, ready for any changes and saving with <see cref="SaveAsPDF(Stream)" /> or one of its overloads</returns>
        /// <exception cref="PDFParserException">Thrown if the content of the file could not be parsed (invalid content)</exception>
        /// <exception cref="InvalidCastException">Thrown if the parsed content was not a document type</exception>
        public static Document ParseDocument(System.IO.TextReader reader, string path, ParseSourceType type)
        {
            ReferenceChecker checker = new ReferenceChecker(path);
            IComponent parsed = Parse(path, reader, type, checker.Resolver);

            if (!(parsed is Document))
                throw new InvalidCastException(String.Format(Errors.CannotConvertObjectToType, parsed.GetType(), typeof(Document)));

            Document doc = parsed as Document;

            return doc;
        }

        #endregion

        #region public static Document ParseDocument(System.Xml.XmlReader reader, string path, ParseSourceType type)

        /// <summary>
        /// Parses a <see cref="XmlReader"/> with the inner xml or xhtml content read into a new <see cref="Document"/> instance.
        /// The path is provided, to map to all relative paths to contained images, fonts, etc.,
        /// unless a base path if set within the document itself
        /// </summary>
        /// <param name="reader">The text reader to read the content from, positioned at the start of the content to be parsed.</param>
        /// <param name="path">The file path, url, or resource path originally used to read the content from, that can then be used for relative content</param>
        /// <param name="type">The <see cref="ParseSourceType"/> as an indicator of where the content is sourced from to assist with loading any external resources.</param>
        /// <returns>A complete parsed document, ready for any changes to be made and saving with <see cref="SaveAsPDF(Stream)" />or one of its overloads </returns>
        /// <exception cref="PDFParserException">Thrown if the content of the file could not be parsed (invalid content)</exception>
        /// <exception cref="InvalidCastException">Thrown if the parsed content was not a document type</exception>
        public static Document ParseDocument(System.Xml.XmlReader reader, string path, ParseSourceType type)
        {
            ReferenceChecker checker = new ReferenceChecker(path);
            IComponent parsed = Parse(path, reader, type, checker.Resolver);

            if (!(parsed is Document))
                throw new InvalidCastException(String.Format(Errors.CannotConvertObjectToType, parsed.GetType(), typeof(Document)));

            Document doc = parsed as Document;

            return doc;
        }



        #endregion

        //
        // parse html document
        //


        #region public static Document ParseHtmlDocument(string path)

        /// <summary>
        /// Parses html content (rather than the more formal xhtml or xml) from a local file into a new <see cref="Document"/> instance.
        /// All relative paths to images, fonts, etc. should be relative to this path, unless a base path is set within the document.
        /// </summary>
        /// <param name="path">The full <paramref name="path"/> to the file, or a relative path from the current working directory</param>
        /// <returns>A complete parsed document, ready for any changes to be made and saving with <see cref="SaveAsPDF(Stream)" />or one of its overloads</returns>
        /// <exception cref="DirectoryNotFoundException" >Thrown if the specified path could not be found in the local file system.</exception>
        /// <exception cref="FileNotFoundException" >Thrown if the specified path could not be found in the local file system.</exception>
        /// <exception cref="PDFParserException">Thrown if the content of the file could not be parsed (invalid content)</exception>
        /// <exception cref="InvalidCastException" >Thown if the parsed content was not a document type</exception>
        public static Document ParseHtmlDocument(string path)
        {
            var provider = Scryber.ServiceProvider.GetService<IPathMappingService>();

            bool isFile;
            var newPath = provider.MapPath(ParserLoadType.ReflectiveParser, path, null, out isFile);

            var parsed = ParseHtml(path);

            if (!(parsed is Document))
                throw new InvalidCastException(String.Format(Errors.CannotConvertObjectToType, parsed.GetType(), typeof(Document)));

            Document doc = parsed as Document;

            return doc;

        }

        #endregion

        #region public static Document ParseHtmlDocument(Stream stream)

        /// <summary>
        /// Parses an html <see cref="Stream"/> (rather than the more formal xhtml or xml) into a new <see cref="Document"/> instance.
        /// As no path is provided, all relative paths to images, fonts, etc. will be relative to the current working directory,
        /// or a base path if set within the document itself
        /// </summary>
        /// <param name="stream">The stream to read the content from, positioned at the start of the content to be parsed.</param>
        /// <returns>A complete parsed document, ready for any changes to be made and saving with <see cref="SaveAsPDF(Stream)" />or one of its overloads </returns>
        /// <exception cref="PDFParserException">Thrown if the content of the file could not be parsed (invalid content)</exception>
        /// <exception cref="InvalidCastException">Thrown if the parsed content was not a document type</exception>
        public static Document ParseHtmlDocument(Stream stream)
        {
            return ParseHtmlDocument(stream, ParseSourceType.DynamicContent);
        }

        #endregion

        #region public static Document ParseHtmlDocument(stream stream, ParseSourceType type)

        /// <summary>
        /// Parses an html <see cref="Stream"/> (rather than the more formal xhtml or xml) into a new <see cref="Document"/> instance.
        /// As no path is provided, all relative paths to images, fonts, etc. will be relative to the current working directory,
        /// or a base path if set within the document itself
        /// </summary>
        /// <param name="stream">The stream to read the content from, positioned at the start of the content to be parsed.</param>
        /// <param name="type">The <see cref="ParseSourceType"/> as an indicator of where the content is sourced from to assist with loading any external resources.</param>
        /// <returns>A complete parsed document, ready for any changes to be made and saving with <see cref="SaveAsPDF(Stream)" />or one of its overloads</returns>
        /// <exception cref="PDFParserException">Thrown if the content of the file could not be parsed (invalid content)</exception>
        /// <exception cref="InvalidCastException">Thrown if the parsed content was not a document type</exception>
        public static Document ParseHtmlDocument(System.IO.Stream stream, ParseSourceType type)
        {
            ReferenceChecker checker = new ReferenceChecker(string.Empty);

            IComponent parsed = ParseHtml(string.Empty, stream, type, checker.Resolver);

            if (!(parsed is Document))
                throw new InvalidCastException(String.Format(Errors.CannotConvertObjectToType, parsed.GetType(), typeof(Document)));

            Document doc = parsed as Document;

            return doc;
        }

        #endregion

        #region public static Document ParseHtmlDocument(System.IO.TextReader reader)

        /// <summary>
        /// Parses a <see cref="TextReader"/> with the inner html content (rather than the more formal xhtml or xml), read into a new <see cref="Document"/> instance.
        /// As no path is provided, all relative paths to images, fonts, etc. will be relative to the current working directory,
        /// or a base path if set within the document itself
        /// </summary>
        /// <param name="reader">The text reader to read the content from, positioned at the start of the content to be parsed.</param>
        /// <returns>A complete parsed document, ready for any changes and saving with <see cref="SaveAsPDF(Stream)" /> or one of its overloads</returns>
        /// <exception cref="PDFParserException">Thrown if the content of the file could not be parsed (invalid content)</exception>
        /// <exception cref="InvalidCastException">Thrown if the parsed content was not a document type</exception>
        public static Document ParseHtmlDocument(System.IO.TextReader reader)
        {
            return ParseHtmlDocument(reader, ParseSourceType.DynamicContent);
        }

        #endregion

        #region public static Document ParseHtmlDocument(System.IO.TextReader reader, ParseSourceType type)

        /// <summary>
        /// Parses a <see cref="TextReader"/> with the inner html content (rather than the more formal xhtml or xml) read into a new <see cref="Document"/> instance.
        /// As no path is provided, all relative paths to images, fonts, etc. will be relative to the current working directory,
        /// or a base path if set within the document itself
        /// </summary>
        /// <param name="reader">The text reader to read the content from, positioned at the start of the content to be parsed.</param>
        /// <param name="type">The <see cref="ParseSourceType"/> as an indicator of where the content is sourced from to assist with loading any external resources.</param>
        /// <returns>A complete parsed document, ready for any changes and saving with <see cref="SaveAsPDF(Stream)" /> or one of its overloads</returns>
        /// <exception cref="PDFParserException">Thrown if the content of the file could not be parsed (invalid content)</exception>
        /// <exception cref="InvalidCastException">Thrown if the parsed content was not a document type</exception>
        public static Document ParseHtmlDocument(System.IO.TextReader reader, ParseSourceType type)
        {
            ReferenceChecker checker = new ReferenceChecker(string.Empty);
            IComponent parsed = ParseHtml(string.Empty, reader, type, checker.Resolver);

            if (!(parsed is Document))
                throw new InvalidCastException(String.Format(Errors.CannotConvertObjectToType, parsed.GetType(), typeof(Document)));

            Document doc = parsed as Document;

            return doc;
        }

        #endregion

        #region public static Document ParseHtmlDocument(System.IO.Stream stream, string path, ParseSourceType type)

        /// <summary>
        /// Parses an html <see cref="Stream"/> (rather than the more formal xhtml or xml) into a new <see cref="Document"/> instance.
        /// The path is provided, to map to all relative paths to contained images, fonts, etc.,
        /// unless a base path if set within the document itself
        /// </summary>
        /// <param name="stream">The stream to read the content from, positioned at the start of the content to be parsed.</param>
        /// <param name="path">The file path, url, or resource path originally used to read the content from, that can then be used for relative content</param>
        /// <param name="type">The <see cref="ParseSourceType"/> as an indicator of where the content is sourced from to assist with loading any external resources.</param>
        /// <returns>A complete parsed document, ready for any changes to be made and saving with <see cref="SaveAsPDF(Stream)" />or one of its overloads</returns>
        /// <exception cref="PDFParserException">Thrown if the content of the file could not be parsed (invalid content)</exception>
        /// <exception cref="InvalidCastException">Thrown if the parsed content was not a document type</exception>
        public static Document ParseHtmlDocument(System.IO.Stream stream, string path, ParseSourceType type)
        {
            ReferenceChecker checker = new ReferenceChecker(path);
            IComponent parsed = ParseHtml(path, stream, type, checker.Resolver);

            if (!(parsed is Document))
                throw new InvalidCastException(String.Format(Errors.CannotConvertObjectToType, parsed.GetType(), typeof(Document)));

            Document doc = parsed as Document;

            return doc;
        }

        #endregion

        #region public static Document ParseHtmlDocument(System.IO.TextReader reader, string path, ParseSourceType type)

        /// <summary>
        /// Parses a <see cref="TextReader"/> with the inner html content (rather than the more formal xhtml or xml), read into a new <see cref="Document"/> instance.
        /// The path is provided, to map to all relative paths to contained images, fonts, etc.,
        /// unless a base path if set within the document itself
        /// </summary>
        /// <param name="reader">The text reader to read the content from, positioned at the start of the content to be parsed.</param>
        /// <param name="path">The file path, url, or resource path originally used to read the content from, that can then be used for relative content</param>
        /// <param name="type">The <see cref="ParseSourceType"/> as an indicator of where the content is sourced from to assist with loading any external resources.</param>
        /// <returns>A complete parsed document, ready for any changes and saving with <see cref="SaveAsPDF(Stream)" /> or one of its overloads</returns>
        /// <exception cref="PDFParserException">Thrown if the content of the file could not be parsed (invalid content)</exception>
        /// <exception cref="InvalidCastException">Thrown if the parsed content was not a document type</exception>
        public static Document ParseHtmlDocument(System.IO.TextReader reader, string path, ParseSourceType type)
        {
            ReferenceChecker checker = new ReferenceChecker(path);
            IComponent parsed = ParseHtml(path, reader, type, checker.Resolver);

            if (!(parsed is Document))
                throw new InvalidCastException(String.Format(Errors.CannotConvertObjectToType, parsed.GetType(), typeof(Document)));

            Document doc = parsed as Document;

            return doc;
        }

        #endregion

        //
        // IRemoteComponent
        //

        #region IRemoteComponent Members

        void Scryber.IRemoteComponent.RegisterNamespaceDeclaration(string prefix, string ns)
        {
            Scryber.Data.XmlNamespaceDeclaration dec = new Data.XmlNamespaceDeclaration()
            {
                NamespaceURI = ns,
                Prefix = prefix
            };
            if (null == this._namespaces)
                this._namespaces = new Data.PDFXmlNamespaceCollection();
            this._namespaces.Add(dec);
        }

        IDictionary<string, string> Scryber.IRemoteComponent.GetDeclaredNamespaces()
        {
            Dictionary<string, string> all = new Dictionary<string, string>();
            foreach (Scryber.Data.XmlNamespaceDeclaration dec in this.NamespaceDeclarations)
            {
                all.Add(dec.Prefix, dec.NamespaceURI);
            }
            return all;
        }

        #endregion


        //
        // disposal
        //

        #region protected override void Dispose(bool disposing)

        /// <summary>
        /// Overrides the base dispose method to dispose of any original source file
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (null != this.RenderOptions)
                    this.RenderOptions.Dispose();
                if(null != this.PrependedFile)
                    this.PrependedFile.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        //
        // inner classes
        //

        #region private class ReferenceChecker

        /// <summary>
        /// Tracks the references to paths and resolves relative paths in each file to be parsed.
        /// </summary>
        private class ReferenceChecker
        {
            private string _rootPath;
            private Stack<string> _route;
            private PDFReferenceResolver _resolver;
            private bool _hasroot;
            
            
            public ReferenceChecker(string fullPath)
            {
                _hasroot = !string.IsNullOrEmpty(fullPath);

                _rootPath = fullPath;
                _route = new Stack<string>();

                if (_hasroot)
                {
                    _route.Push(NormalizePath(fullPath));
                }

                _resolver = this.Resolve;
            }

            

            public IComponent Resolve(string filepath, string xpathselect, ParserSettings settings)
            {
                // always clone the settings and reset the controller type for a remote reference.
                //and keep a copy of the trace record level as it can be adjusted in the processing instructions
                //but not part of the clone
                TraceRecordLevel currLevel = settings.TraceLog.RecordLevel;
                settings = settings.Clone();
                settings.ControllerType = null;
                settings.SpecificCulture = null;
                string fullpath = null;
                ParseSourceType type = ParseSourceType.Other;

                if (null == filepath)
                    throw new ArgumentNullException("filepath");
                

                else if (filepath.StartsWith("~"))
                {
                    //if (null != System.Web.HttpContext.Current)
                    //{
                    //    fullpath = System.Web.HttpContext.Current.Server.MapPath(filepath);
                    //    fullpath = NormalizePath(fullpath);
                    //}
                    //else
                    //{
                        string directory = System.Environment.CurrentDirectory;
                        filepath = filepath.Substring(1);
                        fullpath = System.IO.Path.Combine(directory, filepath);
                        fullpath = NormalizePath(fullpath);
                    //}
                    type = ParseSourceType.LocalFile;
                }
                else if (System.Uri.IsWellFormedUriString(filepath, UriKind.Absolute))
                {
                    fullpath = filepath;
                }
                else if (System.IO.Path.IsPathRooted(filepath))
                {
                    fullpath = filepath;
                }
                else if (_route.Count > 0)
                {
                    string current = _route.Peek();
                    current = System.IO.Path.GetDirectoryName(current);
                    fullpath = System.IO.Path.Combine(current, filepath);
                    fullpath = NormalizePath(fullpath);
                }
                else if (!_hasroot)
                    throw new NotSupportedException(Errors.CannotReferenceExternalFilesFromSourcesWithoutARoot);
                else
                    throw new ArgumentOutOfRangeException("Unbalanced stack");

                if (_route.Contains(fullpath))
                    throw RecordAndRaise.ParserException(Errors.CircularReferenceToPath, filepath);

                _route.Push(fullpath);
                IComponent comp = null;
                try
                {
                    if (string.IsNullOrEmpty(xpathselect))
                    {
                        comp = Document.Parse(fullpath, this.Resolver, settings);
                    }
                    else
                    {
                        using (XmlReader reader = GetSelectedPathReader(fullpath, xpathselect, settings))
                        {
                            if (null != reader)
                                comp = Document.Parse(fullpath, reader, type, this.Resolver, settings);
                        }
                    }

                    if (_route.Pop() != fullpath)
                        throw RecordAndRaise.ParserException(Errors.PathStackIsUnbalanced);

                    //HACK: Restore the recording level and measurement recording.
                    settings.TraceLog.SetRecordLevel(currLevel);
                    settings.PerformanceMonitor.RecordMeasurements = currLevel <= TraceRecordLevel.Verbose;
                }
                catch (PDFParserException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    comp = null;
                    if (settings.ConformanceMode == ParserConformanceMode.Lax)
                        settings.TraceLog.Add(TraceLevel.Error, "Reference Resolver", "Resolver could not parse the content at path '" + filepath + "'. See the inner exception for more details", ex);
                    else
                        throw new PDFParserException("Resolver could not Parse the content a path '" + filepath + "'. See the inner exception for more details.", ex);
                }

                return comp;
            }

            /// <summary>
            /// We need a reader for a subpath of an xml document.
            /// Used by this reference checker to load a sub set of nodes in an xml document
            /// </summary>
            /// <param name="filepath"></param>
            /// <param name="xpathselect"></param>
            /// <returns></returns>
            private XmlReader GetSelectedPathReader(string filepath, string xpathselect, ParserSettings settings)
            {
                XmlDocument doc;
                doc = new XmlDocument();
                doc.PreserveWhitespace = true;
                
                try
                {
                    doc.Load(filepath);
                }
                catch (Exception ex)
                {
                    if (settings.ConformanceMode == ParserConformanceMode.Lax)
                    {
                        settings.TraceLog.Add(TraceLevel.Error, "PDF Reference Resolver", Errors.FileNotFound, ex);
                        return null;
                    }
                    else
                        throw new System.IO.IOException("Could not load file at path '" + filepath + "'", ex);
                }

                XmlNode root = doc.DocumentElement;

                try
                {
                    MatchProcessingInstructions(doc, settings);
                }
                catch (PDFParserException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    if (settings.ConformanceMode == ParserConformanceMode.Lax)
                    {
                        settings.TraceLog.Add(TraceLevel.Error, "PDF Reference Resolver", "Could not parse the processing instructions in file '" + filepath + "'", ex);
                        return null;
                    }
                    else
                        throw new PDFParserException("Could not parse the processing instructions in file '" + filepath + "'", ex);
                }

                XmlNode all;

                try
                {
                    XmlNamespaceManager mgr = new XmlNamespaceManager(doc.NameTable);
                    string prefix = root.GetPrefixOfNamespace(Const.PDFComponentNamespace);

                    //Need the namespaces and their prefixs populated.
                    EnsureStandardNamespacesInManager(mgr, root);

                    all = doc.SelectSingleNode(xpathselect, mgr);
                }
                catch (PDFParserException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    if (settings.ConformanceMode == ParserConformanceMode.Lax)
                    {
                        settings.TraceLog.Add(TraceLevel.Error, "PDF Reference Resolver", "Could not resolve the namespaces in file '" + filepath + "'", ex);
                        return null;
                    }
                    else
                        throw new PDFParserException("Could not resolve the namespaces in in file '" + filepath + "'", ex);
                }

                if (null == all)
                {
                    string format = string.Format("XPath node '{0}' not found in file '{1}' for referenced component", xpathselect, filepath);
                    if (settings.ConformanceMode == ParserConformanceMode.Lax)
                        settings.TraceLog.Add(TraceLevel.Error, "PDF Reference Resolver", format);
                    else
                        throw RecordAndRaise.ParserException(format);
                    return null;
                }
                else
                    return new XmlNodeReader(all);
            }

            /// <summary>
            /// Checks the top level nodes in the document for the scryber processing instuctions. If found then the generator settings will be updated with the values in the processing instructions.
            /// </summary>
            /// <param name="doc"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            private void MatchProcessingInstructions(XmlDocument doc, ParserSettings settings)
            {
                foreach (XmlNode node in doc.ChildNodes)
                {
                    if (node.NodeType == XmlNodeType.ProcessingInstruction && node.Name == Const.ScryberProcessingInstructions)
                    {
                        settings.ReadProcessingInstructions(node.Value);
                        break;
                    }
                }
            }

            private static void EnsureStandardNamespacesInManager(XmlNamespaceManager mgr, XmlNode root)
            {
                //We are only going to do the base ones if they don't exist.
                Dictionary<string, string> namespacestoPrefixes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                var config = ServiceProvider.GetService<IScryberConfigurationService>();
                string ns = config.ParsingOptions.GetXmlNamespaceForAssemblyNamespace(Const.PDFComponentNamespace);
                if (string.IsNullOrEmpty(ns))
                    ns = Const.PDFComponentNamespace;

                string prefix = root.GetPrefixOfNamespace(ns);
                if (string.IsNullOrEmpty(prefix))
                    prefix = "pdf";
                
                namespacestoPrefixes.Add(ns, prefix);

                ns = config.ParsingOptions.GetXmlNamespaceForAssemblyNamespace(Const.PDFStylesNamespace);
                if (string.IsNullOrEmpty(ns))
                    ns = Const.PDFStylesNamespace;

                prefix = root.GetPrefixOfNamespace(ns);
                if (string.IsNullOrEmpty(prefix))
                    prefix = "style";

                namespacestoPrefixes.Add(ns, prefix);

                ns = config.ParsingOptions.GetXmlNamespaceForAssemblyNamespace(Const.PDFDataNamespace);
                if (string.IsNullOrEmpty(ns))
                    ns = Const.PDFDataNamespace;

                prefix = root.GetPrefixOfNamespace(ns);
                if (string.IsNullOrEmpty(prefix))
                    prefix = "data";

                namespacestoPrefixes.Add(ns, prefix);

                //These are keyed by prefix in the manager (which could be anything) so lookup as KVP.Value
                IDictionary<string, string> delcarednamespaces = mgr.GetNamespacesInScope(XmlNamespaceScope.All);
                foreach (KeyValuePair<string, string> kvp in delcarednamespaces)
                {
                    if (namespacestoPrefixes.ContainsKey(kvp.Value))
                    {
                        //Found an existing namespace remove it from our dictionary
                        namespacestoPrefixes.Remove(kvp.Value);
                    }
                }

                if (namespacestoPrefixes.Count > 0) //we have an undeclared namespaces
                {
                    foreach (KeyValuePair<string, string> kvp in namespacestoPrefixes)
                    {
                        if (!delcarednamespaces.ContainsKey(kvp.Value)) //the prefix is not in use so we are safe to declare it.
                            mgr.AddNamespace(kvp.Value, kvp.Key);       //in the xml manager
                    }
                }
            }

            private string NormalizePath(string fullpath)
            {
                return System.IO.Path.GetFullPath(fullpath).ToLower();
            }

            public PDFReferenceResolver Resolver
            {
                get { return _resolver; }
            }
        }

        #endregion

        #region private class NameDictionary

        /// <summary>
        /// A collection of components accessible by their name
        /// </summary>
        private class NameDictionary : Dictionary<string, Component>
        {
        }

        
        #endregion

    }


    public static class DocumentExtensions
    {

        public static bool TryFindAComponentById<T>(this Scryber.Components.Document doc, string id, out T found) where T : Scryber.Components.Component
        {
            found = doc.FindAComponentById(id) as T;
            return null != found;
        }

        public static bool TryFindAComponentByName<T>(this Scryber.Components.Document doc, string name, out T found) where T : Scryber.Components.Component
        {
            found = doc.FindAComponentByName(name) as T;
            return null != found;
        }
    }
}
