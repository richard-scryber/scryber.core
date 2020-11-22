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
using System.Xml;
using Scryber.Native;
using Scryber.Styles;
using Scryber.Resources;
using Scryber.Drawing;
using Scryber.Data;
using Scryber.Generation;
using Scryber.Layout;
using Scryber.Options;
using System.Runtime.CompilerServices;

namespace Scryber.Components
{
    [PDFParsableComponent("Document")]
    [PDFRemoteParsableComponent("Document-Ref")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_document")]
    public class Document : ContainerComponent, IPDFDocument, IPDFViewPortComponent, IPDFRemoteComponent, IPDFStyledComponent,
                                                      IPDFTemplateParser, IPDFXMLParsedDocument, IPDFControlledComponent
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
        protected virtual void OnComponentRegistered(IPDFComponent comp)
        {
            if (null != ComponentRegistered)
            {
                this.ComponentRegistered(this, comp);
            }
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
            : this(PDFObjectTypes.Document)
        {

        }

        #endregion

        #region protected PDFDocument(PDFObjectType type)

        protected Document(PDFObjectType type)
            : base(type)
        {
            this._incrementids = new Dictionary<PDFObjectType, int>();

            var config = ServiceProvider.GetService<IScryberConfigurationService>();
            this.ImageFactories = config.ImagingOptions.GetFactories();
            this._startTime = DateTime.Now;
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

        private PDFListNumbering _numbering;

        /// <summary>
        /// Gets the list numbering manager for this document. This can be used for starting to enumerate over a list, and remembering last values.
        /// </summary>
        public PDFListNumbering ListNumbering
        {
            get
            {
                if (null == _numbering)
                    _numbering = new PDFListNumbering();
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

        #region public PDFImageFactoryList ImageFactories { get; private set; }

        /// <summary>
        /// Gets the collection of image data factories for this document
        /// </summary>
        public PDFImageFactoryList ImageFactories { get; private set; }

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

        private PDFItemCollection _items = null;

        /// <summary>
        /// Gets a document centered collection of objects that can be accessed by name or index
        /// </summary>
        [PDFElement("Params")]
        [PDFArray(typeof(IKeyValueProvider))]
        public PDFItemCollection Params
        {
            get
            {
                if (null == _items)
                    _items = new PDFItemCollection(this);
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
        protected virtual PDFItemCollection CreateItems()
        {
            return new PDFItemCollection(this);
        }

        #endregion

        #region public OutputCompression Compression {get;set;}

        /// <summary>
        /// Gets or Sets the compression option for the output pdf. 
        /// If Compress then content streams, images, fonts etc will be compressed using the Zlib/Deflate method
        /// </summary>
        /// <remarks>Surfaces the Output Compression type of the RenderOptions at the document level.
        /// For greater control use the RenderOptions.Compression value</remarks>
        [PDFAttribute("compression")]
        [Obsolete("Use the RenderOptions.Compression attributes instead. Retained for backwards compatibility", false)]
        [PDFDesignable("Compression", Ignore = true)]
        public OutputCompression Compression
        {
            get { return this.RenderOptions.Compression == OutputCompressionType.None ? OutputCompression.None : OutputCompression.Compress; }
            set
            {
                if (value == OutputCompression.Compress)
                    this.RenderOptions.Compression = OutputCompressionType.FlateDecode;
                else
                    this.RenderOptions.Compression = OutputCompressionType.None;
            }
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

        #region public PDFDocumentAdditionList Additions {get;set;}

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
        [PDFArray(typeof(IPDFComponent))]
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

        #region public PDFDataComponentList DataSources

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

        #region public PDFPageList Pages {get;}

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

        /// <summary>
        /// Creates and returns a new page list wrapping on the inner content. Inheritors can override.
        /// </summary>
        /// <returns></returns>
        protected virtual PageList CreatePageList()
        {
            return new PageList(this.InnerContent);
        }

        #endregion

        #region public PDFStyleCollection Styles {get;} + protected virtual PDFStyleCollection CreateStyleCollection()

        private PDFStyleCollection _styles;

        /// <summary>
        /// Gets the collection of styles in this document
        /// </summary>
        [PDFElement("Styles")]
        [PDFArray(typeof(PDFStyleBase))]
        public PDFStyleCollection Styles
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
        protected virtual PDFStyleCollection CreateStyleCollection()
        {
            return new PDFStyleCollection(this);
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

        // IPDFStyledComponent explicit interface

        PDFStyle IPDFStyledComponent.Style { get; }

        bool IPDFStyledComponent.HasStyle { get { return null != (this as IPDFStyledComponent).Style; } }

        //
        // document level services and values
        //

        #region public IPDFCacheProvider CacheProvider {get;} + protected virtual IPDFCacheProvider CreateCacheProvider()

        private IPDFCacheProvider _cacheprov;
        /// <summary>
        /// Gets the caching provider for this instance
        /// </summary>
        public IPDFCacheProvider CacheProvider
        {
            get
            {
                if (null == _cacheprov)
                    _cacheprov = this.CreateCacheProvider();
                return _cacheprov;
            }
        }

        /// <summary>
        /// Gets an instance of the ICaching provider from the service provider.
        /// </summary>
        /// <returns></returns>
        protected virtual IPDFCacheProvider CreateCacheProvider()
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



        //
        // IXMLParsedDocument interface (explicit implementation)
        //

        #region public bool AppendTraceLog {get;set;}

        /// <summary>
        /// Gets or sets the append trace log flag, that indicates if the entire log output should be 
        /// appended to the document after is has been generated
        /// </summary>
        public bool AppendTraceLog
        {
            get;
            set;
        }

        #endregion

        #region Scryber.PDFTraceLog TraceLog {get;set;}

        private Scryber.PDFTraceLog _log;
        private Scryber.Logging.PDFCollectorTraceLog _collector;

        /// <summary>
        /// Gets or sets the log  for this document.
        /// </summary>
        public Scryber.PDFTraceLog TraceLog
        {
            get
            {
                if (null == _log)
                    _log = this.CreateTraceLog();
                return _log;
            }
            set
            {
                _log = value;
                if (null == value)
                    _collector = null;
                else
                    _collector = _log.GetLogWithName(Scryber.PDFTraceLog.ScryberAppendTraceLogName) as Scryber.Logging.PDFCollectorTraceLog;
            }
        }

        /// <summary>
        /// Instantiates a new trace log based on the configuration and returns the instance
        /// </summary>
        /// <returns></returns>
        protected virtual PDFTraceLog CreateTraceLog()
        {
            var config = ServiceProvider.GetService<IScryberConfigurationService>();
            PDFTraceLog log = config.TracingOptions.GetTraceLog();

            if (this.AppendTraceLog)
            {
                _collector = new Logging.PDFCollectorTraceLog(log.RecordLevel, Scryber.PDFTraceLog.ScryberAppendTraceLogName, true);
                Scryber.Logging.CompositeTraceLog composite = new Logging.CompositeTraceLog(new PDFTraceLog[] { log, _collector }, "");
                log = composite;

            }
            return log;
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

        #region public Scryber.PDFPerformanceMonitor PerformanceMonitor  {get;set;}

        private Scryber.PDFPerformanceMonitor _perfmon;

        /// <summary>
        /// Gets or sets the preformance monitor for this document
        /// </summary>
        public Scryber.PDFPerformanceMonitor PerformanceMonitor
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
        protected virtual PDFPerformanceMonitor CreatePerformanceMonitor()
        {
            TraceRecordLevel level = this.TraceLog.RecordLevel;
            bool measure = (level >= TraceRecordLevel.Verbose);

            //TODO: Make this look at the configuration
            return new PDFPerformanceMonitor(measure);
        }

        #endregion

        #region public ParserConformanceMode ConformanceMode {get;set;}

        private ParserConformanceMode _confmode = ParserConformanceMode.Strict;
        /// <summary>
        /// Gets or sets the parser conformance mode
        /// </summary>
        public ParserConformanceMode ConformanceMode
        {
            get { return _confmode; }
            set { _confmode = value; }
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
        public override PDFStyle GetAppliedStyle(Component forComponent, PDFStyle baseStyle)
        {
            if (null == baseStyle)
                baseStyle = new PDFStyle();
            this.Styles.MergeInto(baseStyle, forComponent, ComponentState.Normal);
            return baseStyle;
        }

        #endregion

        #region protected virtual PDFStyle CreateDefaultStyle()

        /// <summary>
        /// Creates the standard default style - setting page size, font + size, fill color - and returns the new PDFStyle instance.
        /// Inheritors can override this to adjust the default style for any document
        /// </summary>
        /// <returns></returns>
        protected virtual PDFStyle CreateDefaultStyle()
        {
            PDFStyle style = this.GetBaseStyle();


            //Get the applied style and then merge it into the base style
            PDFStyle applied = this.GetAppliedStyle(this, style);
            //if (null != applied)
            //    applied.MergeInto(style);
            applied.Flatten();
            return applied;
        }

        #endregion

        #region protected override PDFStyle GetBaseStyle()
        /// <summary>
        /// Overrides the base implementation to get a full (yet empty) style for a document.
        /// </summary>
        /// <returns></returns>
        protected override PDFStyle GetBaseStyle()
        {
            PDFStyle style = base.GetBaseStyle();
            //style.Items.Add(new PDFBackgroundStyle());
            //style.Items.Add(new PDFBorderStyle());
            //style.Items.Add(new PDFStrokeStyle());
            PDFFillStyle fill = new PDFFillStyle();
            style.StyleItems.Add(fill);
            fill.Color = new PDFColor(ColorSpace.RGB, System.Drawing.Color.Black);


            PDFPageStyle defpaper = new PDFPageStyle();
            style.StyleItems.Add(defpaper);
            defpaper.PaperSize = PaperSize.A4;
            defpaper.PaperOrientation = PaperOrientation.Portrait;

            PDFFontStyle fs = new PDFFontStyle();
            style.StyleItems.Add(fs);
            fs.FontFamily = ServiceProvider.GetService<IScryberConfigurationService>().FontOptions.DefaultFont;
            fs.FontSize = new PDFUnit(24.0, PageUnits.Points);


            return style;
        }

        #endregion

        //
        // Component registration
        // Designed to impove the look up of components
        // 

        #region public void RegisterComponent(PDFComponent comp)

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

        #region public void UnRegisterComponent(PDFComponent comp)

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

        #region public void ReRegisterComponent(PDFComponent comp, string oldname) + 1 overload

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

        private Dictionary<PDFObjectType, int> _incrementids = null;

        /// <summary>
        /// Gets the next unique id for an Component of a specific type
        /// </summary>
        /// <param name="type">The type of Component to create the new ID for</param>
        /// <returns>A unique id</returns>
        public override string GetIncrementID(PDFObjectType type)
        {
            if (this._incrementids == null)
                this._incrementids = new Dictionary<PDFObjectType, int>();
            int lastindex;

            if (this._incrementids.TryGetValue(type, out lastindex) == false)
                lastindex = 0;

            lastindex += 1;
            this._incrementids[type] = lastindex;
            string id = type.ToString() + lastindex.ToString();

            if (null != _originalPageResources)
            {
                //if we have an existing set of resources, then make sure we are not conflicting
                while (_originalPageResources.ContainsKey(id))
                {
                    lastindex += 1;
                    this._incrementids[type] = lastindex;
                    id = type.ToString() + lastindex.ToString();
                }
            }
            return id;

        }

        #endregion

        #region public override PDFComponentArrangement GetFirstArrangement()

        /// <summary>
        /// Override the GetFirstArrangement to return the arrangement of the first page
        /// </summary>
        /// <returns></returns>
        public override PDFComponentArrangement GetFirstArrangement()
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

        protected virtual PDFResourceCollection CreateResourceCollection()
        {
            PDFResourceCollection resxcol = new PDFResourceCollection(this);
            //PDFFonts.InitStdFonts(resxcol);
            return resxcol;
        }

        #endregion

        #region GetImageResource(string fullpath, bool create)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        public PDFImageXObject GetImageResource(string fullpath, Component owner, bool create)
        {
            fullpath = fullpath.ToLower();
            
            PDFImageXObject img = this.GetResource(PDFResource.XObjectResourceType, owner, fullpath, create) as PDFImageXObject;

            return img;

        }


        #endregion

        #region public PDFFontResource GetFontResource(PDFFont font)

        /// <summary>
        /// Retrieves a PDFFontResource for the specified font, based upon it's full name
        /// </summary>
        /// <param name="font">The font to get the resource for</param>
        /// <param name="create">true if the PDFFontResource should be created if it is not already listed</param>
        /// <returns>A PDFFontResource that will be included in the document (or null if it is not loaded and should not be created)</returns>
        public virtual PDFFontResource GetFontResource(PDFFont font, bool create)
        {
            string fullname = font.FullName;

            string type = PDFResource.FontDefnResourceType;

            PDFFontResource rsrc = this.GetResource(type, fullname, true) as PDFFontResource;
            return rsrc;

        }

        #endregion

        #region public virtual PDFResource GetResource(string resourceType, PDFComponent owner, string resourceKey, bool create)

        /// <summary>
        /// Implements the IPDFDocument.GetResource interface and supports the extraction and creation of document specific resources.
        /// </summary>
        /// <param name="resourceType">The Type of resource to get - XObject, Font</param>
        /// <param name="resourceKey">The resource specific key name (Fonts is full name, images is the full path or resource key)</param>
        /// <param name="create">Specify true to attempt the loading of the resource if it is not currently in the collection</param>
        /// <returns>The loaded resource if any</returns>
        public virtual PDFResource GetResource(string resourceType, Component owner, string resourceKey, bool create)
        {
            PDFResource found = this.SharedResources.GetResource(resourceType, resourceKey);
            if (null == found && create)
            {
                found = this.CreateAndAddResource(resourceType, owner, resourceKey);
            }
            return found;
        }

        public virtual PDFResource GetResource(string resourceType, string resourceKey, bool create)
        {
            return this.GetResource(resourceType, this, resourceKey, create);
        }

        #endregion

        #region protected virtual PDFResource CreateAndAddResource(string resourceType, string resourceKey)

        /// <summary>
        /// Creates a new PDFResource of the required type based on the specified key and adds it to the SharedResource collection
        /// </summary>
        /// <param name="resourceType">The type of resource to create (image XObject or Font)</param>
        /// <param name="resourceKey"></param>
        /// <returns>A new PDFResoure sub class instance</returns>
        protected virtual PDFResource CreateAndAddResource(string resourceType, Component owner, string resourceKey)
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

            if (null != created)
                created = this.SharedResources.Add(created);

            return created;
        }

        #endregion

        #region private PDFImageXObject CreateImageResource(string fullpath)

        private PDFImageXObject CreateImageResource(string fullpath, Component owner)
        {
            using (this.PerformanceMonitor.Record(PerformanceMonitorType.Image_Load, fullpath))
            {
                PDFImageData data = this.LoadImageData(owner, fullpath);
                if (null != data)
                {
                    string id = this.GetIncrementID(PDFObjectTypes.ImageXObject);
                    PDFImageXObject img = PDFImageXObject.Load(data, id);
                    return img;
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
                PDFFontDefinition defn = PDFFontFactory.GetFontDefinition(fullname);
                string id = this.GetIncrementID(PDFObjectTypes.FontResource);
                PDFFontResource rsrc = PDFFontResource.Load(defn, id);
                if (rsrc.ResourceKey.Equals(fullname) == false)
                    rsrc.SetResourceKey(fullname);
                return rsrc;
            }
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
        // document processing
        //

        
        #region public void SaveAsPDF(System.IO.Stream stream, bool bind) + 1 overload


        /// <summary>
        /// Performs a complete initialization, load and then render the document to the path. 
        /// If a document exists at hte specified path, then an IOException will be raised.
        /// Uses the Autobind property to stipulate if data binding should also take place
        /// </summary>
        /// <param name="path">The path to the file to write to</param>
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
                this.SaveAs(stream, bind, OutputFormat.PDF);
            }
        }




        /// <summary>
        /// Performs a complete initialization, load and then renders the document to the output stream.
        /// Uses the Autobind property to stipulate if data binding should also take place
        /// </summary>
        /// <param name="stream"></param>
        public void SaveAsPDF(System.IO.Stream stream)
        {
            this.SaveAs(stream, this.AutoBind, OutputFormat.PDF);
        }

        public void SaveAsPDF(System.IO.Stream stream, bool bind)
        {
            this.SaveAs(stream, bind, OutputFormat.PDF);
        }

        /// <summary>
        /// Performs a complete initialization, load and then renders the document to the output stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="bind"></param>
        public virtual void SaveAs(System.IO.Stream stream, bool bind, OutputFormat format)
        {
            if (null == stream)
                throw new ArgumentNullException("stream");

            this.InitializeAndLoad();

            if (bind)
                this.DataBind();

            this.RenderTo(stream, format);
        }


        #endregion

        #region public void InitializeAndLoad()

        /// <summary>
        /// Performs any initializing and loading, but does not render the document
        /// </summary>
        public void InitializeAndLoad()
        {
            PDFTraceLog log = this.TraceLog;
            PDFPerformanceMonitor perfmon = this.PerformanceMonitor;
            PDFItemCollection items = this.Params;

            PDFInitContext icontext = CreateInitContext(log, perfmon, items);

            log.Begin(TraceLevel.Message, "Document", "Beginning Document Initialize");
            perfmon.Begin(PerformanceMonitorType.Document_Init_Stage);

            this.Init(icontext);

            perfmon.End(PerformanceMonitorType.Document_Init_Stage);
            log.End(TraceLevel.Message, "Document", "Completed Document Initialize");
            this.GenerationStage = DocumentGenerationStage.Initialized;

            PDFLoadContext loadcontext = CreateLoadContext(log, perfmon, items);

            log.Begin(TraceLevel.Message, "Document", "Beginning Document Load");
            perfmon.Begin(PerformanceMonitorType.Document_Load_Stage);

            this.Load(loadcontext);

            perfmon.End(PerformanceMonitorType.Document_Load_Stage);
            log.End(TraceLevel.Message, "Document", "Completed Document Load");
            this.GenerationStage = DocumentGenerationStage.Loaded;
        }


        protected virtual PDFInitContext CreateInitContext(PDFTraceLog log, PDFPerformanceMonitor perfmon, PDFItemCollection items)
        {
            PDFInitContext icontext = new PDFInitContext(items, log, perfmon);
            this.PopulateContextBase(icontext);
            return icontext;
        }

        protected virtual PDFLoadContext CreateLoadContext(PDFTraceLog log, PDFPerformanceMonitor perfmon, PDFItemCollection items)
        {
            PDFLoadContext loadcontext = new PDFLoadContext(items, log, perfmon);
            this.PopulateContextBase(loadcontext);
            return loadcontext;
        }


        #endregion

        #region protected override void DoInit(PDFInitContext context)

        /// <summary>
        /// Overrides the base implementation to check the document initialization stage
        /// </summary>
        /// <param name="context"></param>
        protected override void DoInit(PDFInitContext context)
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

        protected virtual void DoInitStyles(PDFInitContext context)
        {
            if (this.Styles != null && this.Styles.Count > 0)
            {
                foreach (PDFStyleBase style in this.Styles)
                {
                    try
                    {
                        if (context.ShouldLogDebug)
                            context.TraceLog.Add(TraceLevel.Debug, "PDF Document", "Initializing the style '" + style.ToString() + "'");

                        if (style is IPDFComponent)
                            (style as IPDFComponent).Init(context);
                    }
                    catch (PDFException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        string msg = string.Format("Could not init the data to the style '{0}' : {1}", style, ex.Message);
                        throw new PDFException(msg, ex);
                    }
                }
            }
        }

        #endregion

        #region protected override void DoLoad(PDFLoadContext context)

        /// <summary>
        /// Overrides the base implementation to check the document generation stage.
        /// </summary>
        /// <param name="context"></param>
        protected override void DoLoad(PDFLoadContext context)
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

        protected virtual void DoLoadStyles(PDFLoadContext context)
        {
            if (this.Styles != null && this.Styles.Count > 0)
            {
                foreach (PDFStyleBase style in this.Styles)
                {
                    try
                    {
                        if (context.ShouldLogDebug)
                            context.TraceLog.Add(TraceLevel.Debug, "PDF Document", "Loading the style '" + style.ToString() + "'");

                        if (style is IPDFComponent)
                            (style as IPDFComponent).Load(context);
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

        #region public void DataBind()

        /// <summary>
        /// Data binds the entire document
        /// </summary>
        public void DataBind()
        {
            PDFTraceLog log = this.TraceLog;
            PDFPerformanceMonitor perfmon = this.PerformanceMonitor;
            PDFItemCollection items = this.Params;

            PDFDataContext context = this.CreateDataContext(log, perfmon, items);

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
        protected virtual PDFDataContext CreateDataContext(PDFTraceLog log, PDFPerformanceMonitor perfmon, PDFItemCollection items)
        {

            PDFDataContext context = new PDFDataContext(items, log, perfmon);
            this.PopulateContextBase(context);
            return context;
        }

        #endregion

        #region protected override void DoDataBind(PDFDataContext context, bool includeChildren)

        /// <summary>
        /// Overrides the default implementation, calling the base method within it
        /// </summary>
        /// <param name="context"></param>
        /// <param name="includeChildren"></param>
        protected override void DoDataBind(PDFDataContext context, bool includeChildren)
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

        protected virtual void DoBindStyles(PDFDataContext context)
        {
            if (this.Styles != null && this.Styles.Count > 0)
            {
                foreach (PDFStyleBase style in this.Styles)
                {
                    try
                    {
                        if (context.ShouldLogDebug)
                            context.TraceLog.Add(TraceLevel.Debug, "PDF Document", "Binding the style '" + style.ToString() + "'");
                        if (style is IPDFBindableComponent)
                            (style as IPDFBindableComponent).DataBind(context);
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
        /// Retruns a new namespace manager for the namespaces name table
        /// </summary>
        /// <param name="nameTable"></param>
        /// <returns></returns>
        public virtual IXmlNamespaceResolver CreateNamespaceResolver(PDFXmlNamespaceCollection namespaceDeclarations, XmlNameTable nameTable)
        {
            XmlNamespaceManager mgr = new XmlNamespaceManager(nameTable);
            foreach (XmlNamespaceDeclaration dec in namespaceDeclarations)
            {
                if (string.IsNullOrEmpty(dec.Prefix))
                    throw new ArgumentNullException("prefix", string.Format(Errors.CannotDeclareNamespaceWithoutPrefix, dec.NamespaceURI));

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
        public virtual IPDFDataProvider AssertGetProvider(string key)
        {
            IPDFDataProvider provider;
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
        internal override void RegisterPreLayout(PDFLayoutContext context)
        {
            if (this.HasAdditions)
                this.Additions.RegisterPreLayout(context);

            base.RegisterPreLayout(context);
        }

        #endregion

        #region internal override void RegisterLayoutComplete(PDFLayoutContext context)

        /// <summary>
        /// Overrides the base implementation to register layout complete for any additions as well
        /// </summary>
        /// <param name="context"></param>
        internal override void RegisterLayoutComplete(PDFLayoutContext context)
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
        internal override void RegisterPreRender(PDFRenderContext context)
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
        internal override void RegisterPostRender(PDFRenderContext context)
        {
            base.RegisterPostRender(context);

            if (this.HasAdditions)
                this.Additions.RegisterPostRender(context);

        }

        #endregion

        //
        // rendering
        //

        #region RenderTo(string path, System.IO.FileMode mode) + 2 Overloads

        /// <summary>
        /// Renders the complete document to a file at the specified path, using the specified file mode. It is up to callers to
        /// to use temporary files and replacement
        /// </summary>
        /// <param name="path">The complete path at which to create the file.</param>
        /// <param name="mode">The FileMode option for CreateNew, Append etc.</param>
        public void RenderTo(string path, System.IO.FileMode mode, OutputFormat format)
        {
            using (System.IO.Stream stream = this.DoOpenFileStream(path, mode))
            {
                this.RenderTo(stream, format);
            }
        }

        /// <summary>
        /// Renders the complete document to an IO Stream
        /// </summary>
        /// <param name="tostream">The stream to write the document to</param>
        public void RenderTo(System.IO.Stream tostream, OutputFormat format)
        {
            switch (format)
            {
                case OutputFormat.PDF:
                    this.RenderToPDF(tostream);
                    break;
                default:
                    break;
            }
            
        }


        public virtual void RenderToPDF(System.IO.Stream tostream)
        {
            PDFRenderContext context = this.DoCreateRenderContext();

            using (PDFWriter writer = this.DoCreateRenderWriter(tostream, context))
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


            PDFStyle style = this.GetAppliedStyle();


            //Layout components before rendering
            PDFLayoutContext layoutcontext = CreateLayoutContext(style, context.OutputFormat, context.Items, context.TraceLog, context.PerformanceMonitor);
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
        /// Wraps the OutputToPDF in a separate stream then builds an ammended 
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
                    appended = CreateTraceLogAppendDocument(genData, origFile, appended);
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

        protected virtual Document CreateTraceLogAppendDocument(PDFDocumentGenerationData genData, PDFFile origFile, Document appended)
        {
            appended = new PDFTraceLogDocument(this.FileName, origFile, genData);
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
            data.TraceLog = this.TraceLog.GetLogWithName(Scryber.PDFTraceLog.ScryberAppendTraceLogName) as Scryber.Logging.PDFCollectorTraceLog;
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
        protected virtual PDFLayoutContext CreateLayoutContext(PDFStyle style, PDFOutputFormatting format, PDFItemCollection items, PDFTraceLog log, PDFPerformanceMonitor perfmon)
        {
            PDFLayoutContext context = new PDFLayoutContext(style, format, items, log, perfmon);
            PopulateContextBase(context);
            return context;
        }

        #endregion

        #region public IPDFLayoutEngine GetEngine(...)

        IPDFLayoutEngine IPDFViewPortComponent.GetEngine(IPDFLayoutEngine parent, PDFLayoutContext context, PDFStyle style)
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
        protected virtual PDFRenderContext DoCreateRenderContext()
        {
            PDFTraceLog log = this.TraceLog;
            PDFPerformanceMonitor perfmon = this.PerformanceMonitor;


            PDFStyle def = this.CreateDefaultStyle();
            PDFOutputFormatting format = this.GetOutputFormat(this.RenderOptions.OuptputCompliance);
            PDFRenderContext context = new PDFRenderContext(DrawingOrigin.TopLeft, 0, format, def, this.Params, log, perfmon);

            this.PopulateContextBase(context);
            return context;
        }

        #endregion

        #region protected virtual PDFOutputFormatting GetOutputFormat(string formatName)

        /// <summary>
        /// Gets the required document formatter based on it's name
        /// </summary>
        /// <param name="formatName"></param>
        /// <returns></returns>
        protected virtual PDFOutputFormatting GetOutputFormat(string formatName)
        {
            return PDFOutputFormatting.GetFormat(formatName);
        }

        #endregion

        #region private void PopulateContextBase(PDFContextBase context)

        /// <summary>
        /// Fills the context base options for any created context
        /// </summary>
        /// <param name="context"></param>
        protected virtual void PopulateContextBase(PDFContextBase context)
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
            //TODO: Add configuration for document writer version support.

            PDFWriter writer = this.RenderOptions.CreateWriter(this, tostream, 0, context.TraceLog);

            writer.UseHex = (this.RenderOptions.StringOutput == OutputStringType.Hex);
            writer.DefaultStreamFilters = GetStreamFilters();

            return writer;
        }

        #endregion

        #region protected virtual IStreamFilter[] GetStreamFilters()
        /// <summary>
        /// Loads and returns the default stream filters
        /// </summary>
        /// <returns></returns>
        protected virtual IStreamFilter[] GetStreamFilters()
        {
            List<IStreamFilter> all = new List<IStreamFilter>();
            return all.ToArray();
        }

        #endregion

        //
        // Path manipulation and loading
        //

        #region public PDFImageData LoadImageData(IPDFComponent parent, string source)

        /// <summary>
        /// Loads a file name or url from a source and returns the correct image data encapsulating the image stream
        /// </summary>
        /// <param name="source">The full path or absolute location of the image data file</param>
        /// <returns></returns>
        public PDFImageData LoadImageData(IPDFComponent owner, string path)
        {
            PDFImageData data;
            string key = path;
            bool compress = false;
            if (owner is IPDFOptimizeComponent)
                compress = ((IPDFOptimizeComponent)owner).Compress;
            try
            {
                object cached;
                IPDFImageDataFactory factory;

                if (string.IsNullOrEmpty(path))
                    throw new ArgumentNullException("path");

                path = owner.MapPath(path);

                if (this.ImageFactories.TryGetMatch(path, out factory))
                {
                    data = LoadImageDataFromFactory(owner, factory, path);
                }
                else
                {
                    bool isfile;

                    if (System.Uri.IsWellFormedUriString(path, UriKind.Absolute))
                    {
                        isfile = false;
                    }
                    else if (System.IO.Path.IsPathRooted(path))
                    {
                        isfile = true;
                    }
                    else
                        throw RecordAndRaise.Argument(Errors.CannotLoadFileWithRelativePath);

                    if (!this.CacheProvider.TryRetrieveFromCache(PDFObjectTypes.ImageData.ToString(), key, out cached))
                    {
                        IPDFDataProvider prov;
                        if (isfile)
                            data = PDFImageData.LoadImageFromLocalFile(path, owner);

                        else if (this.DataProviders.TryGetDomainProvider("", path, out prov))
                        {
                            object returned = prov.GetResponse(prov.ProviderKey + "Image", path, null);
                            if (returned is PDFImageData)
                                data = (PDFImageData)returned;
                            else if (returned is byte[])
                            {
                                byte[] imgdata = (byte[])returned;
                                data = PDFImageData.InitImageData(path, imgdata, compress);
                            }
                            else
                                data = null;
                        }
                        else
                        {
                            
                            data = PDFImageData.LoadImageFromURI(path, owner);
                        }

                        if (null != data)
                        {
                            DateTime expires = this.GetImageCacheExpires();
                            this.CacheProvider.AddToCache(PDFObjectTypes.ImageData.ToString(), key, data, expires);
                        }
                    }
                    else
                        data = (PDFImageData)cached;
                }
            }
            catch (Exception ex)
            {
                PDFTraceLog log = this.TraceLog;
                log.Add(TraceLevel.Error, "Document", "Could not load the image data for '" + key + "'. Failed with message : " + ex.Message, ex);

                if (this.RenderOptions.AllowMissingImages)
                {
                    data = GetNotFoundLogo(path);
                }
                else
                    throw;
            }


            return data;
        }

        private PDFImageData LoadImageDataFromFactory(IPDFComponent owner, IPDFImageDataFactory factory, string path)
        {
            PDFImageData data;

            if (factory.ShouldCache)
            {
                object cached;
                if (!this.CacheProvider.TryRetrieveFromCache(PDFObjectTypes.ImageData.ToString(), path, out cached))
                {
                    data = factory.LoadImageData(this, owner, path);

                    if (null != data)
                    {
                        DateTime expires = this.GetImageCacheExpires();
                        this.CacheProvider.AddToCache(PDFObjectTypes.ImageData.ToString(), path, data, expires);
                    }
                }
                else
                    data = (PDFImageData)cached;
            }
            else
                data = factory.LoadImageData(this, owner, path);

            return data;
        }

        public PDFImageData GetNotFoundLogo(string path)
        {
            return null;

            //if (null == this.Info)
            //    this.Info = new PDFDocumentInfo();
            //if (null == this.Info.Extras)
            //    this.Info.Extras = new PDFDocumentInfoExtraCollection();

            //string value = this.Info.Extras["Missing Images"];
            //if (string.IsNullOrEmpty(value))
            //    value =  path.Replace("\\","\\\\");
            //else
            //    value += ";" + path.Replace("\\", "\\\\");
                
            //this.Info.Extras["Missing Images"] = value;

            //System.Drawing.Bitmap bmp = Properties.Resources.scryber_NotFoundLogo;
            //return PDFImageData.LoadImageFromBitmap(path, bmp, false);
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
            bool isfile;
            return this.MapPath(path, out isfile);
        }

        

        #endregion        

        //
        // Find a component
        //


        #region  public PDFComponent FindAComponent(string id) + 1 overload

        public Component FindAComponentById(string id)
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

        private bool FindAComponentById(ComponentList list, string id, out Component found)
        {
            if (list != null)
            {
                foreach (Component ele in list)
                {
                    if (ele.ID.Equals(id))
                    {
                        found = ele;
                        return true;
                    }
                    else if (ele is IPDFContainerComponent)
                    {
                        IPDFContainerComponent container = ele as IPDFContainerComponent;
                        if (container.HasContent && this.FindAComponentById(container.Content, id, out found))
                            return true;
                    }
                }
            }
            found = null;
            return false;
        }

        #endregion

        #region  public PDFComponent FindAComponentByName(string name) + 1 overload

        public Component FindAComponentByName(string name)
        {
            Component value;
            if(this._namedictionary.TryGetValue(name, out value))
                return value;

            Component ele = null;
            if (string.Equals(this.Name, name))
                return this;
            else if (this.FindAComponentByName(this.Pages.InnerList, name, out ele))
                return ele;
            else
                return null;
        }

        private bool FindAComponentByName(ComponentList list, string name, out Component found)
        {
            if (list != null)
            {
                foreach (Component ele in list)
                {
                    if (string.Equals(name, ele.Name))
                    {
                        found = ele;
                        return true;
                    }
                    else if (ele is IPDFContainerComponent)
                    {
                        IPDFContainerComponent container = ele as IPDFContainerComponent;
                        if (container.HasContent && this.FindAComponentByName(container.Content, name, out found))
                            return true;
                    }
                }
            }
            found = null;
            return false;
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
        protected override IPDFComponent ParseComponentAtPath(string path)
        {
            IPDFComponent parsed = null;
            try
            {
                parsed = Document.Parse(path);
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
                    PDFTraceLog log = this.TraceLog;
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

        #region public static IPDFComponent Parse(string fullpath) + 11 overloads

        public static IPDFComponent Parse(string fullpath)
        {
            using (System.IO.Stream stream = new System.IO.FileStream(fullpath,System.IO.FileMode.Open,System.IO.FileAccess.Read))
            {
                
                PDFReferenceChecker checker = new PDFReferenceChecker(fullpath);

                IPDFComponent comp = Parse(fullpath, stream, ParseSourceType.LocalFile, checker.Resolver);

                if (comp is IPDFRemoteComponent)
                    ((IPDFRemoteComponent)comp).LoadedSource = fullpath;

                return comp;
            }
        }

        public static IPDFComponent Parse(System.IO.Stream stream, ParseSourceType type)
        {
            PDFReferenceChecker checker = new PDFReferenceChecker(string.Empty);
            IPDFComponent comp = Parse(string.Empty, stream, type, checker.Resolver);

            return comp;
        }

        public static IPDFComponent Parse(System.IO.TextReader reader, ParseSourceType type)
        {
            PDFReferenceChecker checker = new PDFReferenceChecker(string.Empty);
            IPDFComponent comp = Parse(string.Empty, reader, type, checker.Resolver);

            return comp;
        }

        public static IPDFComponent Parse(System.Xml.XmlReader reader, ParseSourceType type)
        {
            PDFReferenceChecker checker = new PDFReferenceChecker(string.Empty);
            IPDFComponent comp = Parse(string.Empty, reader, type, checker.Resolver);

            return comp;
        }

        public static IPDFComponent Parse(string fullpath, PDFReferenceResolver resolver)
        {
            ParserConformanceMode mode = ParserConformanceMode.Lax;

            PDFGeneratorSettings settings = Document.CreateGeneratorSettings(resolver, mode, null);
            return Parse(fullpath, resolver, settings);
        }

        public static IPDFComponent Parse(string fullpath, PDFReferenceResolver resolver, PDFGeneratorSettings settings)
        {
            using (System.IO.Stream stream = new System.IO.FileStream(fullpath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                IPDFComponent comp = Parse(fullpath, stream, ParseSourceType.LocalFile, resolver, settings);
                if (comp is IPDFRemoteComponent)
                    ((IPDFRemoteComponent)comp).LoadedSource = fullpath;
                return comp;
            }
        }

        public static IPDFComponent Parse(string source, System.IO.Stream stream, ParseSourceType type, PDFReferenceResolver resolver)
        {
            ParserConformanceMode mode = ParserConformanceMode.Lax;
            PDFGeneratorSettings settings = Document.CreateGeneratorSettings(resolver, mode, null);
            return Parse(source, stream, type, resolver, settings);
        }

        public static IPDFComponent Parse(string source, System.IO.Stream stream, ParseSourceType type, PDFReferenceResolver resolver, PDFGeneratorSettings settings)
        {
            IPDFParser parser = new Scryber.Generation.PDFXMLParser(settings);

            IPDFComponent comp = parser.Parse(source, stream, type);
            return comp;
        }

        public static IPDFComponent Parse(string source, System.IO.TextReader textreader, ParseSourceType type, PDFReferenceResolver resolver)
        {
            ParserConformanceMode mode = ParserConformanceMode.Lax;
            PDFGeneratorSettings settings = Document.CreateGeneratorSettings(resolver, mode, null);
            return Parse(source, textreader, type, resolver, settings);
        }

        public static IPDFComponent Parse(string source, System.IO.TextReader textreader, ParseSourceType type, PDFReferenceResolver resolver, PDFGeneratorSettings settings)
        {
            IPDFParser parser = new Scryber.Generation.PDFXMLParser(settings);

            IPDFComponent comp = parser.Parse(source, textreader, type);
            return comp;
        }

        public static IPDFComponent Parse(string source, System.Xml.XmlReader xmlreader, ParseSourceType type, PDFReferenceResolver resolver)
        {
            ParserConformanceMode mode = ParserConformanceMode.Lax;
            PDFGeneratorSettings settings = Document.CreateGeneratorSettings(resolver, mode, null);
            
            return Parse(source, xmlreader, type, resolver, settings);
        }

        public static IPDFComponent Parse(string source, System.Xml.XmlReader xmlreader, ParseSourceType type, PDFReferenceResolver resolver, PDFGeneratorSettings settings)
        {
            IPDFParser parser =  new Scryber.Generation.PDFXMLParser(settings);

            IPDFComponent comp = parser.Parse(source, xmlreader, type);
            return comp;
        }

        #endregion

        #region public IPDFComponent ParseTemplate(IPDFRemoteComponent owner, string referencepath, Stream stream) + 2 overloads

        public IPDFComponent ParseTemplate(IPDFRemoteComponent owner, System.IO.TextReader reader)
        {
            return ParseTemplate(owner, owner.LoadedSource, reader);
        }

        public IPDFComponent ParseTemplate(IPDFRemoteComponent owner, string referencepath, System.IO.Stream stream)
        {
            using (System.Xml.XmlReader xml = System.Xml.XmlReader.Create(stream))
            {
                return ParseTemplate(owner, referencepath, xml);
            }
        }



        public IPDFComponent ParseTemplate(IPDFComponent owner, string referencepath, System.IO.TextReader reader)
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
        public virtual IPDFComponent ParseTemplate(IPDFComponent owner, string referencepath, System.Xml.XmlReader reader)
        {
            if (null == owner)
                throw RecordAndRaise.ArgumentNull("owner");
            if (null == reader)
                throw RecordAndRaise.ArgumentNull("reader");
            
            IPDFComponent comp;

            PDFReferenceResolver resolver = this.Resolver;
            if (null == resolver)
            {
                PDFReferenceChecker checker = new PDFReferenceChecker(referencepath);
                resolver = checker.Resolver;
            }

            PDFGeneratorSettings settings = this.DoCreateGeneratorSettings(resolver);
            //settings.Controller = this.Controller;

            IPDFParser parser = new Scryber.Generation.PDFXMLParser(settings);

            parser.RootComponent = owner;

            comp = parser.Parse(referencepath, reader, ParseSourceType.Template);
            
            
            return comp;
        }

        #endregion

        #region protected virtual PDFGeneratorSettings CreateGeneratorSettings(PDFReferenceResolver resolver)
        /// <summary>
        /// Creates the generator settings required to parse the XML files
        /// </summary>
        /// <param name="resolver"></param>
        /// <returns></returns>
        protected virtual PDFGeneratorSettings DoCreateGeneratorSettings(PDFReferenceResolver resolver)
        {
            PDFTraceLog log = this.TraceLog;
            PDFPerformanceMonitor monitor = this.PerformanceMonitor;
            ParserConformanceMode mode = this.ConformanceMode;
            ParserLoadType loadtype = this.LoadType;
            object controller = this.Controller;

            return CreateGeneratorSettings(resolver, mode, 
                loadtype, log, monitor, controller);
        }

        private static PDFGeneratorSettings CreateGeneratorSettings(PDFReferenceResolver resolver, ParserConformanceMode mode, object controller)
        {
            ParserConformanceMode conformance = mode;
            ParserLoadType loadtype = ParserLoadType.ReflectiveParser;
            var config = ServiceProvider.GetService<IScryberConfigurationService>();

            PDFTraceLog log = config.TracingOptions.GetTraceLog();
            PDFPerformanceMonitor perfmon = new PDFPerformanceMonitor(log.RecordLevel <= TraceRecordLevel.Verbose);

            return CreateGeneratorSettings(resolver, conformance, loadtype, log, perfmon, controller);
        }

        protected static PDFGeneratorSettings CreateGeneratorSettings(PDFReferenceResolver resolver, 
            ParserConformanceMode conformance, ParserLoadType loadtype, PDFTraceLog log, PDFPerformanceMonitor perfmon, object controller)
        {
            PDFGeneratorSettings settings = new PDFGeneratorSettings(typeof(TextLiteral)
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



        #region public static PDFDocument ParseDocument(string path)

        public static Document ParseDocument(string path)
        {
            var provider = Scryber.ServiceProvider.GetService<IPDFPathMappingService>();

            bool isFile;
            var newPath = provider.MapPath(ParserLoadType.ReflectiveParser, path, null, out isFile);

            var doc = Parse(path) as Document;
            return doc;

        }

        #endregion

        #region public static PDFDocument ParseDocument(stream stream, ParseSourceType type) + 3 overloads


        public static Document ParseDocument(System.IO.Stream stream, ParseSourceType type)
        {
            PDFReferenceChecker checker = new PDFReferenceChecker(string.Empty);
            IPDFComponent parsed = Parse(string.Empty, stream, type, checker.Resolver);

            if (!(parsed is Document))
                throw new InvalidCastException(String.Format(Errors.CannotConvertObjectToType, parsed.GetType(), typeof(Document)));

            Document doc = parsed as Document;

            return doc;
        }

        public static Document ParseDocument(System.IO.TextReader reader, ParseSourceType type)
        {
            PDFReferenceChecker checker = new PDFReferenceChecker(string.Empty);
            IPDFComponent parsed = Parse(string.Empty, reader, type, checker.Resolver);

            if (!(parsed is Document))
                throw new InvalidCastException(String.Format(Errors.CannotConvertObjectToType, parsed.GetType(), typeof(Document)));

            Document doc = parsed as Document;

            return doc;
        }

        public static Document ParseDocument(System.Xml.XmlReader reader, ParseSourceType type)
        {
            PDFReferenceChecker checker = new PDFReferenceChecker(string.Empty);
            IPDFComponent parsed = Parse(string.Empty, reader, type, checker.Resolver);

            if (!(parsed is Document))
                throw new InvalidCastException(String.Format(Errors.CannotConvertObjectToType, parsed.GetType(), typeof(Document)));

            Document doc = parsed as Document;

            return doc;
        }

        #endregion

        #region IPDFRemoteComponent Members

        void Scryber.IPDFRemoteComponent.RegisterNamespaceDeclaration(string prefix, string ns)
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

        IDictionary<string, string> Scryber.IPDFRemoteComponent.GetDeclaredNamespaces()
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
            }
            base.Dispose(disposing);
        }

        #endregion

        //
        // inner classes
        //


        #region private class PDFReferenceChecker

        /// <summary>
        /// Tracks the references to paths and resolves relative paths in each file to be parsed.
        /// </summary>
        private class PDFReferenceChecker
        {
            private string _rootpath;
            private Stack<string> _route;
            private PDFReferenceResolver _resolver;
            private bool _hasroot;
            
            
            public PDFReferenceChecker(string fullpath)
            {
                _hasroot = !string.IsNullOrEmpty(fullpath);

                _rootpath = fullpath;
                _route = new Stack<string>();

                if (_hasroot)
                {
                    _route.Push(NormalizePath(fullpath));
                }

                _resolver = new PDFReferenceResolver(this.Resolve);
            }

            

            public IPDFComponent Resolve(string filepath, string xpathselect, PDFGeneratorSettings settings)
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
                    fullpath = filepath.ToLower();
                }
                else if (System.IO.Path.IsPathRooted(filepath))
                {
                    fullpath = filepath.ToLower();
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
                IPDFComponent comp = null;
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
            private XmlReader GetSelectedPathReader(string filepath, string xpathselect, PDFGeneratorSettings settings)
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
            private void MatchProcessingInstructions(XmlDocument doc, PDFGeneratorSettings settings)
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


    public static class PDFDocumentExtensions
    {

        public static bool TryFindAComponentByID<T>(this Scryber.Components.Document doc, string id, out T found) where T : Scryber.Components.Component
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
