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

//define this compiler switch to support the value of this components Document to be held in a local variable.
//If not defined it will be looked up dyanically every time.
#define DOCUMENT_CACHING

#define DOCUMENT_COMPONENT_REGISTRY_CACHE

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Scryber.Styles;
using Scryber.Drawing;
using Scryber.PDF;

namespace Scryber.Components
{
    /// <summary>
    /// Base class for all complex pdf Components
    /// </summary>
    public abstract class Component : PDFObject, IDisposable, IComponent, IBindableComponent, ILoadableComponent
    {
        // static event keys for the PDFEventList

        private static readonly object InitEventKey = new object();
        private static readonly object LoadedEventKey = new object();
        private static readonly object DataBindingEventKey = new object();
        private static readonly object DataBoundEventKey = new object();
        private static readonly object PreLayoutEventKey = new object();
        private static readonly object PostLayoutEventKey = new object();
        private static readonly object PreRenderEventKey = new object();
        private static readonly object PostRenderEventKey = new object();
        private static readonly object DisposedEventKey = new object();


        // ivars

        private Style _appliedStyle = null; //local caching for a the style information on this Component
        private ComponentArrangement _arrange = null; //the arrangement from the layout
        private ComponentEventList _events; //holds any registered events without needing a field for each.

        //
        // Events property and HasRegisteredEvents
        //

        #region protected PDFEventList Events {get;} + protected bool HasRegisteredEvents {get;}

        /// <summary>
        /// Gets the PDFEventList that holds references to all events that 
        /// have been registered on this component for the standard events.
        /// </summary>
        protected ComponentEventList Events
        {
            get
            {
                if (null == _events)
                    _events = new ComponentEventList(this);
                return _events;
            }
        }

        /// <summary>
        /// Returns true if any events have been registered for raising 
        /// aginst this component
        /// </summary>
        protected bool HasRegisteredEvents
        {
            get { return null != _events && _events.IsEmpty == false; }
        }

        #endregion

        //
        // Event Declarations and raising methods
        //

        #region PDFInitializedEventHandler Initialized Event + OnInitialized(PDFInitContext)

        /// <summary>
        /// Event that notifies receivers that this instance has now been fully initialized
        /// </summary>
        [PDFAttribute("on-init")]
        public event PDFInitializedEventHandler Initialized
        {
            add { this.Events.AddHandler(InitEventKey, value); }
            remove { this.Events.RemoveHandler(InitEventKey, value); }
        }

        /// <summary>
        /// Raises the initialized event 
        /// </summary>
        /// <param name="context"></param>
        protected virtual void OnInitialized(PDFInitContext context)
        {
            if (this.HasRegisteredEvents)
            {
                PDFInitializedEventHandler handler = (PDFInitializedEventHandler)this.Events[InitEventKey];
                if (null != handler)
                    handler(this, new PDFInitEventArgs(context));
            }
        }

        #endregion

        #region PDFLoadedEventHandler Loaded Event + OnLoaded(PDFLoadContext)

        /// <summary>
        /// Event that notifies receivers that this instance has now been loaded
        /// </summary>
        [PDFAttribute("on-loaded")]
        public event PDFLoadedEventHandler Loaded
        {
            add { this.Events.AddHandler(LoadedEventKey, value); }
            remove { this.Events.RemoveHandler(LoadedEventKey, value); }
        }

        /// <summary>
        /// Raises the loaded event. 
        /// </summary>
        /// <param name="context"></param>
        protected virtual void OnLoaded(PDFLoadContext context)
        {
            if (this.HasRegisteredEvents)
            {
                PDFLoadedEventHandler handler = (PDFLoadedEventHandler)this.Events[LoadedEventKey];
                if (null != handler)
                    handler(this, new PDFLoadEventArgs(context));
            }
        }

        #endregion

        #region PDFDataBindEventHandler DataBinding Event + OnDataBinding(PDFDataContext)

        /// <summary>
        /// Notifies receivers that this instance is in the process of being data bound
        /// </summary>
        [PDFAttribute("on-databinding")]
        public event PDFDataBindEventHandler DataBinding
        {
            add { this.Events.AddHandler(DataBindingEventKey, value); }
            remove { this.Events.RemoveHandler(DataBindingEventKey, value); }
        }


        /// <summary>
        /// Raises the DataBinding event. Inheritors can override this method to perfom their own actions
        /// </summary>
        /// <param name="e">The arguments</param>
        protected virtual void OnDataBinding(PDFDataContext context)
        {
            if (this.HasRegisteredEvents)
            {
                PDFDataBindEventHandler handler = (PDFDataBindEventHandler)this.Events[DataBindingEventKey];
                if (null != handler)
                    handler(this, new PDFDataBindEventArgs(context));

            }
        }

        #endregion

        #region PDFDataBindEventHandler DataBound Event + OnDataBound(PDFDataContext)

        /// <summary>
        /// Notifies receivers that this instance has been databound
        /// </summary>
        [PDFAttribute("on-databound")]
        public event PDFDataBindEventHandler DataBound
        {
            add { this.Events.AddHandler(DataBoundEventKey, value); }
            remove { this.Events.RemoveHandler(DataBoundEventKey, value); }
        }

        /// <summary>
        /// Raises the DataBound event. Inheritors can override this method to perfom their own actions
        /// </summary>
        /// <param name="e">The arguments</param>
        protected virtual void OnDataBound(PDFDataContext context)
        {
            if (this.HasRegisteredEvents)
            {
                PDFDataBindEventHandler handler = (PDFDataBindEventHandler)this.Events[DataBoundEventKey];
                if (null != handler)
                    handler(this, new PDFDataBindEventArgs(context));

            }
        }

        #endregion

        #region PDFLayoutEventHandler PreLayout event + OnPreLayout(PDFLayoutContext) + RegisterPreLayout(PDFLayoutContext)

        /// <summary>
        /// Event that is raised before any of the layout of the document has been started.
        /// </summary>
        [PDFAttribute("on-prelayout")]
        public event PDFLayoutEventHandler PreLayout
        {
            add { this.Events.AddHandler(PreLayoutEventKey, value);  }
            remove { this.Events.RemoveHandler(PreLayoutEventKey, value); }
        }

        /// <summary>
        /// Raises the PreLayout event
        /// </summary>
        /// <param name="context"></param>
        protected virtual void OnPreLayout(PDFLayoutContext context)
        {
            if (this.HasRegisteredEvents)
            {
                PDFLayoutEventHandler handler = (PDFLayoutEventHandler)this.Events[PreLayoutEventKey];
                if (null != handler)
                    handler(this, new PDFLayoutEventArgs(context));
            }
        }

        /// <summary>
        /// Registers the PreLayout event raising with the layout context
        /// Friendly inheritors can override.
        /// </summary>
        /// <param name="context"></param>
        internal virtual void RegisterPreLayout(PDFLayoutContext context)
        {
            this.OnPreLayout(context);
        }

        #endregion

        #region PDFLayoutEventHandler LayoutComplete event OnLayoutComplete(PDFLayoutContext) + RegisterLayoutComplete(PDFLayoutContext)

        /// <summary>
        /// Event that is raised once all the layout of the document has been completed.
        /// </summary>
        [PDFAttribute("on-postlayout")]
        public event PDFLayoutEventHandler LayoutComplete
        {
            add { this.Events.AddHandler(PostLayoutEventKey, value); }
            remove { this.Events.RemoveHandler(PostLayoutEventKey, value); }
        }

        /// <summary>
        /// Raises the LayoutComplete event
        /// </summary>
        /// <param name="context"></param>
        protected virtual void OnLayoutComplete(PDFLayoutContext context)
        {
            if (this.HasRegisteredEvents)
            {
                PDFLayoutEventHandler handler = (PDFLayoutEventHandler)this.Events[PostLayoutEventKey];
                if (null != handler)
                    handler(this, new PDFLayoutEventArgs(context));
            }

        }

        /// <summary>
        /// Registers the LayoutComplete event raising with the layout context. 
        /// Friendly inheritors can override
        /// </summary>
        /// <param name="context"></param>
        internal virtual void RegisterLayoutComplete(PDFLayoutContext context)
        {
            this.OnLayoutComplete(context);
        }

        #endregion

        #region PDFRenderEventHandler PreRender Event + OnPreRender(PDFRenderContext) + RegisterPreRender(PDFRenderContext)

        /// <summary>
        /// Notifies receivers that this instance is about to be Rendered. This is the last chance to change properties
        /// </summary>
        [PDFAttribute("on-prerender")]
        public event PDFRenderEventHandler PreRender
        {
            add { this.Events.AddHandler(PreRenderEventKey, value); }
            remove { this.Events.RemoveHandler(PreRenderEventKey, value); }
        }

        /// <summary>
        /// Raises the PreRender event. Inheritors can override this method to perfom their own actions
        /// </summary>
        /// <param name="e">The arguments</param>
        protected virtual void OnPreRender(PDFRenderContext context)
        {
            if (this.HasRegisteredEvents)
            {
                PDFRenderEventHandler handler = (PDFRenderEventHandler)this.Events[PreRenderEventKey];
                if (handler != null)
                    handler(this, new PDFRenderEventArgs(context));
            }
        }

        /// <summary>
        /// Registers the PreRender event raising with the render context. 
        /// Friendly inheritors can override
        /// </summary>
        /// <param name="context"></param>
        internal virtual void RegisterPreRender(PDFRenderContext context)
        {
            this.OnPreRender(context);
        }

        #endregion

        #region PDFRenderEventHandler PostRender Event + OnPostRender(PDFRenderContext) + RegisterPostRender(PDFRenderContext)

        /// <summary>
        /// Notifies receivers that this instance has been rendered. Clean up can now be performed
        /// </summary>
        [PDFAttribute("on-postrender")]
        public event PDFRenderEventHandler PostRender
        {
            add { this.Events.AddHandler(PostRenderEventKey, value); }
            remove { this.Events.RemoveHandler(PostRenderEventKey, value); }
        }

        /// <summary>
        /// Raises the PostRender event. Inheritors can override this method to perfom their own actions
        /// </summary>
        /// <param name="context">The current render context</param>
        protected virtual void OnPostRender(PDFRenderContext context)
        {
            if (this.HasRegisteredEvents)
            {
                PDFRenderEventHandler handler = (PDFRenderEventHandler)this.Events[PostRenderEventKey];
                if (null != handler)
                    handler(this, new PDFRenderEventArgs(context));
            }
        }

        /// <summary>
        /// Registers the PostRender event raising with the render context. 
        /// Friendly inheritors can override
        /// </summary>
        /// <param name="context"></param>
        internal virtual void RegisterPostRender(PDFRenderContext context)
        {
            this.OnPostRender(context);
        }

        #endregion

        //
        // public properties
        //

        #region public PDFComponent Parent {get;}

        private Component _par;
        private bool _registered = false;

        /// <summary>
        /// Gets or Sets the Parent of this Component in the PDF Hierarchy
        /// </summary>
        public Component Parent
        {
            get { return _par; }
            set 
            {
                if (object.Equals(_par, value))
                    return;

                if(_registered)
                    this.UnregisterParent(_par);

                this._par = value;

                if (null != _par)
                {
                    this.RegisterParent(_par);
                    _registered = true;
                }
                
            }
        }


        /// <summary>
        /// Explicit interface implementation
        /// </summary>
        IComponent IComponent.Parent
        {
            get { return this.Parent; }
            set 
            {
                if (value is Component)
                    this.Parent = (Component)value;
                else if (null == value)
                    this.Parent = null;
                else
                    throw RecordAndRaise.InvalidCast(Errors.CannotConvertObjectToType, value.GetType().Name, "PDFComponent");
            }
        }

        /// <summary>
        /// Called when the parent of this component has changed
        /// </summary>
        /// <param name="parent"></param>
        internal protected virtual void UnregisterParent(Component parent)
        {

#if DOCUMENT_COMPONENT_REGISTRY_CACHE

            if (null != parent)
            {
                Document doc = parent.Document;
                if (null != doc)
                    doc.UnRegisterComponent(this);
            }
#endif

#if DOCUMENT_CACHING
            _cacheddoc = null;
            _cachedpage = null;
#endif

        }

        internal protected virtual void RegisterParent(Component parent)
        {

            if (null != parent)
            {

#if DOCUMENT_COMPONENT_REGISTRY_CACHE
                Document doc = parent.Document;
                if (null != doc)
                    doc.RegisterComponent(this);
#endif
                this.ResetChildIDs();
            }
        }


        

        /// <summary>
        /// Sets the Parent of prevValue to null if it is owned by this instance, 
        /// and sets the Parent of newValue to this if it is not owned by this instance
        /// </summary>
        /// <param name="prevvalue">The previously owned value if any</param>
        /// <param name="newvalue">The new owned value if any</param>
        /// <returns>The new owned value</returns>
        protected Component AssignSelfAsParent(Component prevchild, Component newchild)
        {
            if (null != prevchild && object.ReferenceEquals(this, prevchild.Parent))
                prevchild.Parent = null;

            if (null != newchild && object.ReferenceEquals(this, newchild.Parent) == false)
                newchild.Parent = this;

            return newchild;
        }

        #endregion

        #region public PDFDocument Document {get;}

#if DOCUMENT_CACHING

        private Document _cacheddoc;
        private Page _cachedpage;

#endif


        /// <summary>
        /// Gets the document that this Component belongs to.
        /// </summary>
        public Document Document
        {
            get
            {
                //Dual strategy - One holds a local reference to this components document
                //The other checks this value everytime

#if DOCUMENT_CACHING

                if (this._cacheddoc == null)
                {
                    if (this.Parent != null)
                        this._cacheddoc = this.Parent.Document;
                    else if (this is Document)
                        this._cacheddoc = this as Document;
                    else
                        this._cacheddoc = null;
                }

                return _cacheddoc;
#else
                if (this is PDFDocument)
                    return this as PDFDocument;
                else if (this.Parent != null)
                    return this.Parent.Document;
                else
                    return null;

#endif
            }
        }

        /// <summary>
        /// Explicit interface implementation
        /// </summary>
        IDocument IComponent.Document
        {
            get { return this.Document; }
        }


        #endregion

        #region public PDFPage Page {get;}

        /// <summary>
        /// Gets the page that this Component belongs to.
        /// </summary>
        public virtual Page Page
        {
            get
            {
                //Dual strategy to store a chached reference to the page

#if DOCUMENT_CACHING

                if (_cachedpage == null)
                {
                    if (this is Page)
                        _cachedpage = this as Page;
                    else if (this.Parent != null)
                        _cachedpage = this.Parent.Page;
                    else
                        _cachedpage = null;
                }
                return _cachedpage;

#else
                if (this is PDFPage)
                    return this as PDFPage;
                else if (this.Parent != null)
                    return this.Parent.Page;
                else
                    return null;
                    
#endif
            }
        }

        #endregion

        #region ID {get;set;} + UniqueID{get;}

        private string _id;

        /// <summary>
        /// Gets or sets the ID for this instance
        /// </summary>
        [PDFAttribute("id")]
        [PDFDesignable("Id",Category = "General", Priority = 1, Type = "ID")]
        public string ID
        {
            get 
            {
                if (String.IsNullOrEmpty(_id))
                {
                    if (this.Document != null)
                        _id = this.Document.GetIncrementID(this.Type);
                    else
                        _id = string.Empty;
                }
                return this._id;
            }
            set 
            {
                _id = value;
            }
        }

        /// <summary>
        /// Gets the complete string representation of the ID of this instance separated using '$'
        /// </summary>
        public string UniqueID
        {
            get
            {
                return this.BuildUniqueID(Const.UniqueIDSeparator);
            }
        }

        /// <summary>
        /// Builds and returns the full id for this Component based upon its naming containers.
        /// </summary>
        /// <param name="separator">The string that is used to separate individual ids when building the ID</param>
        /// <returns>The full unique id</returns>
        protected string BuildUniqueID(string separator)
        {
            StringBuilder sb = new StringBuilder();
            this.BuildUniqueID(sb, separator);
            return sb.ToString();

        }

        private void BuildUniqueID(System.Text.StringBuilder sb, string separator)
        {
            Component ppe = this.Parent;
            Stack<string> names = new Stack<string>();

            while (ppe != null)
            {
                if (ppe is INamingContainer)
                {
                    names.Push(ppe.ID);
                }
                ppe = ppe.Parent;
            }
            while (names.Count > 0)
            {
                sb.Append(names.Pop());
                sb.Append(separator);
            }
            
            sb.Append(this.ID);

        }

        #endregion

        #region public string Name {get;set;}

        private string _name;
        /// <summary>
        /// Gets or sets the name of this component - used in the name dictionary and for linking.
        /// Names (if set) must be unique across the whole document.
        /// </summary>
        [PDFAttribute("name")]
        [PDFDesignable("Name", Category = "General", Priority = 1, Type = "ID")]
        public string Name
        {
            get { return _name; }
            set 
            {
                string oldname = this._name;
                _name = value;

#if DOCUMENT_COMPONENT_REGISTRY_CACHE

                Document doc = this.Document;
                if (doc != null)
                    doc.ReRegisterComponent(this, oldname, value);

#endif
            }
        }

        #endregion

        #region public string ElementName

        /// <summary>
        /// Gets or sets the name of the element that this component was parsed from.
        /// </summary>
        public string ElementName
        {
            get;
            set;
        }

        #endregion

        #region string StyleClass {get;set;}

        private string _class;
        
        /// <summary>
        /// Gets or sets the associated style class name for this Component
        /// </summary>
        [PDFAttribute("class",Const.PDFStylesNamespace)]
        [PDFJSConvertor("scryber.studio.design.convertors.styleClass_attr")]
        [PDFDesignable("Class Name(s)", Category = "Style Classes", Priority = 1, Type = "ClassName")]
        public virtual string StyleClass
        {
            get { return _class; }
            set 
            {
                _class = value; 
            }
        }

        #endregion

        #region public int PageLayoutIndex {get;} + SetPageLayoutIndex(int) and ClearPageLayoutIndex()

        private int _pagelayoutIndex = -1;

        /// <summary>
        /// Gets the first page index this component appears on in the final layout. (-1 if not set).
        /// </summary>
        public int PageLayoutIndex
        {
            get { return _pagelayoutIndex; }
        }

        internal void SetPageLayoutIndex(int index)
        {
            if (this._pagelayoutIndex < 0)
                _pagelayoutIndex = index;
        }

        internal void ClearPageLayoutIndex()
        {
            _pagelayoutIndex = -1;
        }

        #endregion

        #region public bool Visible {get;set;}

        private bool _vis = true;

        /// <summary>
        /// Gets or sets the visibility of the Page Component
        /// </summary>
        [PDFAttribute("visible")]
        [PDFJSConvertor("scryber.studio.design.convertors.visible_attr")]
        [PDFDesignable("Visible", Category = "General", Priority = 1, Type = "Boolean")]
        public virtual bool Visible
        {
            get
            {
                return _vis;
            }
            set
            {
                _vis = value;
            }
        }



        #endregion

        #region public object Tag

        private string _tag;
        /// <summary>
        /// Gets or sets any tag for this component. This is not used by the core framework.
        /// </summary>
        [PDFAttribute("tag")]
        [PDFDesignable("Tag", Ignore = true)]
        public string Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        #endregion

        #region public string LoadedSource {get;set;}

        private string _src;

        /// <summary>
        /// Gets or sets the full source path this component was loaded from (if any)
        /// </summary>
        public virtual string LoadedSource
        {
            get { return _src; }
            set { _src = value; }
        }

        #endregion

        #region public ComponentLoadType LoadType

        private ParserLoadType _loadtype = ParserLoadType.None;

        /// <summary>
        /// Gets or sets a value that indicates the load type for this component. Inheritors can set this value.
        /// </summary>
        /// <remarks>Based upon this value we can identify if the component was loaded via the reflective parser, 
        /// or CodeDomGenerator, or a web build provider. If this components value is none, then the parent value is 
        /// checked and returned (and so on up the hierarchy)</remarks>
        public ParserLoadType LoadType
        {
            get
            {
                if (this._loadtype == ParserLoadType.None && this.Parent != null)
                    return this.Parent.LoadType;
                else
                    return _loadtype;
            }
            set { _loadtype = value; }
        }

        #endregion

        #region public PDFOutline Outline {get;set;} + bool HasOutline {get;}

        private Outline _outline;

        /// <summary>
        /// Gets or sets the outline title for this component
        /// </summary>
        [PDFElement("Outline")]
        public virtual Outline Outline
        {
            get 
            {
                if (null == this._outline)
                {
                    this._outline = new Outline();
                    this._outline.BelongsTo = this;
                }
                return this._outline;
            }
            set
            {
                if (null != this._outline)
                    this._outline.BelongsTo = null;

                this._outline = value;
                
                if (null != value)
                    this._outline.BelongsTo = this;
            }
        }

        public bool HasOutline
        {
            get { return null != this._outline && !string.IsNullOrEmpty(this._outline.Title); }
        }

        #endregion


        //
        //constructors
        //

        #region protected .ctor(PDFObjectType)

        protected Component(ObjectType type): base(type)
        {
        }

        #endregion

        //
        //public methods
        //

        #region public void Init() + protected virtual void DoInit()

        /// <summary>
        /// Initializes the instance
        /// </summary>
        public void Init(PDFInitContext context)
        {
            if (context.ShouldLogDebug)
                context.TraceLog.Begin(TraceLevel.Debug, "Component", "Init Component '" + this.UniqueID + "'");

            this.DoInit(context);
            this.OnInitialized(context);

            if (context.ShouldLogDebug)
                context.TraceLog.End(TraceLevel.Debug, "Component", "Init Component '" + this.UniqueID + "'");

        }

        /// <summary>
        /// Inheritors should override this method to perform their own initialization
        /// </summary>
        protected virtual void DoInit(PDFInitContext context)
        {
        }

        #endregion

        #region public void Load(PDFLoadContext context) + protected virtual void DoLoad()

        /// <summary>
        /// Load operation
        /// </summary>
        public void Load(PDFLoadContext context)
        {
            if (context.ShouldLogDebug)
                context.TraceLog.Begin(TraceLevel.Debug,"Component", "Load Component '" + this.UniqueID + "'");

            this.DoLoad(context);
            this.OnLoaded(context);

            if (context.ShouldLogDebug)
                context.TraceLog.End(TraceLevel.Debug,"Component", "Load Component '" + this.UniqueID + "'");

        }

        /// <summary>
        /// Inheritors should override this method to perform their own loading operations
        /// </summary>
        protected virtual void DoLoad(PDFLoadContext context)
        {
        }

        #endregion

        #region public void DataBind() + public void DataBind(bool includeChildren) + protected virtual void DoDataBind(bool includeChildren)

        /// <summary>
        /// Databinds this page Component and any children
        /// </summary>
        public void DataBind(PDFDataContext context)
        {
            if (context.ShouldLogDebug)
                context.TraceLog.Begin(TraceLevel.Debug, "Component", "Databind Component '" + this.UniqueID + "'");

            try
            {

                this.OnDataBinding(context);

                this.DoDataBind(context, true);

                this.OnDataBound(context);
            }
            catch (PDFDataException)
            {
                throw;
            }
            catch(Exception ex)
            {
                if (context.Conformance == ParserConformanceMode.Lax)
                    context.TraceLog.Add(TraceLevel.Error, "DataBind", "Databinding failed for component " + this.UniqueID + ", " + ex.Message, ex);
                else
                    throw new PDFDataException("Databinding failed for component " + this.UniqueID + ", " + ex.Message, ex);
            }

            if (context.ShouldLogDebug)
                context.TraceLog.End(TraceLevel.Debug, "Component", "Databind Component '" + this.UniqueID + "'");
        }

        /// <summary>
        /// Inheritors should override this method to provide their own data binding implementations
        /// </summary>
        /// <param name="includeChildren"></param>
        protected virtual void DoDataBind(PDFDataContext context, bool includeChildren)
        {
            if (this._outline != null)
                this.Outline.DataBind(context);
        }

        #endregion

        #region  public PDFComponent FindAComponent(string id) + 1 overload

        public virtual Component FindAComponentById(string id)
        {
            if (this.ID.Equals(id))
                return this;
            else
                return null;
        }

        protected bool FindAComponentById(ComponentList list, string id, out Component found)
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

        public virtual Component FindAComponentByName(string name)
        {
            
            if (string.Equals(this.Name, name))
                return this;
            else
                return null;
        }

        protected bool FindAComponentByName(ComponentList list, string name, out Component found)
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

        #region public void RegisterLayoutArtefacts(PDFRegistrationContext context) + CloseLayoutArtefacts()

        /// <summary>
        /// Registers all the artefacts (name, outline) for this component (and only this component). Artefacts can represent
        /// </summary>
        /// <param name="context"></param>
        /// <param name="fullstyle"></param>
        /// <returns></returns>
        public virtual PDFArtefactRegistrationSet RegisterLayoutArtefacts(PDFLayoutContext context, Style fullstyle)
        {
            PDFArtefactRegistrationSet set = new PDFArtefactRegistrationSet(context.DocumentLayout, this);

            bool registername = this.ShouldRegisterName(context);
            bool registerOutline = this.ShouldRegisterOutline(context);

            if (registername || registerOutline)
            {
                //register the name
                object name = context.DocumentLayout.RegisterCatalogEntry(context, PDFArtefactTypes.Names, new PDFDestination(this, OutlineFit.FullPage));
                set.SetArtefact(PDFArtefactTypes.Names, name);
            }

            object outline = null;
            //if we have a title then register the outline component
            if (registerOutline)
            {
                outline = context.DocumentLayout.RegisterCatalogEntry(context, PDFArtefactTypes.Outlines, new PDF.PDFOutlineRef(this.Outline, fullstyle.Outline));
                set.SetArtefact(PDFArtefactTypes.Outlines, outline);
            }

            this.DoRegisterArtefacts(context, set, fullstyle);
            return set;
        }

        protected virtual bool ShouldRegisterName(PDFLayoutContext context)
        {
            return context.DocumentLayout.ShouldRenderAllNames() || !string.IsNullOrEmpty(this.Name);
        }

        protected virtual bool ShouldRegisterOutline(PDFLayoutContext context)
        {
            return this.HasOutline && !(this is IPDFInvisibleContainer);
        }

        protected virtual void DoRegisterArtefacts(PDFLayoutContext context, PDFArtefactRegistrationSet set, Style fullstyle)
        {

        }


        public void CloseLayoutArtefacts(PDFLayoutContext context, PDFArtefactRegistrationSet artefacts, Style fullstyle)
        {
            object outline, name;
            if (artefacts.TryGetArtefact(PDFArtefactTypes.Outlines, out outline))
                context.DocumentLayout.CloseArtefactEntry(PDFArtefactTypes.Outlines, outline);

            if (artefacts.TryGetArtefact(PDFArtefactTypes.Names, out name))
                context.DocumentLayout.CloseArtefactEntry(PDFArtefactTypes.Names, name);

            this.DoCloseLayoutArtefacts(context, artefacts, fullstyle);
        }


        protected virtual void DoCloseLayoutArtefacts(PDFLayoutContext context, PDFArtefactRegistrationSet artefacts, Style fullstyle)
        {
            //Does nothing in base class
        }

        #endregion



        //
        // private and protected methods
        //

        #region protected PDFGraphics CreateGraphics(PDFStyleStack styles) + 1 overload

        /// <summary>
        /// Creates a new PDFGraphics context within which drawing to the PDF Surface can take place
        /// </summary>
        /// <param name="styles">The styles of the new graphics context</param>
        /// <returns>A newly instantiated graphics context</returns>
        public PDFGraphics CreateGraphics(StyleStack styles, PDFContextBase context)
        {
            return this.CreateGraphics(null, styles, context);
        }


        /// <summary>
        /// Creates a new PDFGraphics context within which drawing to the PDF Surface can take place
        /// </summary>
        /// <param name="writer">The writer used to write graphical instructions to</param>
        /// <param name="styles">The styles of the new graphics context</param>
        /// <returns>A newly instantiated graphics context</returns>
        public virtual PDFGraphics CreateGraphics(PDFWriter writer, StyleStack styles, PDFContextBase context)
        {
            if (this.Parent == null)
                throw RecordAndRaise.NullReference(Errors.InvalidCallToGetGraphicsForStructure);
            else
                return this.Parent.CreateGraphics(writer, styles, context);
        }

        #endregion

        #region protected virtual PDFStyle GetAppliedStyle() + public virtual PDFStyle GetAppliedStyle(PDFComponent forComponent)

        
        /// <summary>
        /// Gets the Defined Style for this Component (Style Items that are to be applied directly)
        /// </summary>
        /// <returns></returns>
        public Style GetAppliedStyle()
        {
            if (null == _appliedStyle)
            {
                Style s = this.GetAppliedStyle(this, GetBaseStyle());

                MergeDeclaredStyles(s);
                _appliedStyle = s;
            }
            return _appliedStyle;
        }

        /// <summary>
        /// Merges any explictly declared styles on this component, onto the cacluated applied style
        /// </summary>
        /// <param name="applied"></param>
        protected virtual void MergeDeclaredStyles(Style applied)
        {
            if (this is IPDFStyledComponent styledComponent)
            {
                if (styledComponent.HasStyle)
                    styledComponent.Style.MergeInto(applied, Style.DirectStylePriority);
            }
        }

        /// <summary>
        /// Gets the base style for this component. Any styles that would be applied if no explict user styles override. Default is empty
        /// </summary>
        /// <returns></returns>
        protected virtual Style GetBaseStyle()
        {
            return new Style();
        }

        /// <summary>
        /// Traverses the hierarchy of Components to retrieve the defined style for the Component provided
        /// </summary>
        /// <param name="forComponent">The Component to get the style for</param>
        /// <param name="baseStyle"></param>
        /// <returns>The merged style</returns>
        public virtual Style GetAppliedStyle(Component forComponent, Style baseStyle)
        {
            if (this.Parent != null)
                return this.Parent.GetAppliedStyle(forComponent, baseStyle);
            else
                return baseStyle;
        }

        #endregion


        #region protected virtual bool ClipGraphicsToSize(PDFContextStyleBase context)

        /// <summary>
        /// Returns true of false if the current rendering should be clipped to the bounds, or allowed to overflow
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual bool ClipGraphicsToSize(PDFContextStyleBase context)
        {
            return false;
        }

        #endregion

        #region protected virtual PDFComponent FindComponent(string id)

        /// <summary>
        /// Finds an Component in the entire document hierarchy with the specified ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected virtual Component FindDocumentComponentById(string id)
        {
            if (this.Document != null)
                return this.Document.FindAComponentById(id);
            else
                throw RecordAndRaise.Operation(Errors.CannotUseFindForComponentNotInDocumentHeirarchy);
        }

        #endregion

        #region protected virtual IPDFResourceContainer GetResourceContainer()

        /// <summary>
        /// Searches this components parent hirerachy for the IPDFResourceContainer (usually the page).
        /// </summary>
        /// <returns>The found container or null</returns>
        protected virtual IResourceContainer GetResourceContainer()
        {
            Component comp = this.Parent;
            while(null != comp)
            {
                if (comp is IResourceContainer)
                    return (IResourceContainer)comp;
                else
                    comp = comp.Parent;
            }

            return null;
        }

        #endregion

        #region  public string GetFullPath()

        /// <summary>
        /// Gets the full path to the loaded source for this component if set or
        /// refers the request up the hierarchy until a LoadedSource value is found
        /// or the top of the tree is reached
        /// </summary>
        /// <returns></returns>
        public string GetFullPath()
        {
            if (!string.IsNullOrEmpty(this.LoadedSource))
                return this.LoadedSource;
            else if (null != this.Parent)
                return this.Parent.GetFullPath();
            else
                return string.Empty;
        }

        #endregion

        #region public virtual string MapPath(string path)

        /// <summary>
        /// Returns a full path to a resource based upon the 
        /// provided path and the root path of the document. If the 
        /// path cannot be determined returns the original path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public virtual string MapPath(string path)
        {
            if (System.Uri.IsWellFormedUriString(path, UriKind.Absolute))
                return path;
            

            if (path.IndexOf('\\') > -1 && System.IO.Path.IsPathRooted(path))
                return path;

            if (!string.IsNullOrEmpty(this.LoadedSource))
            {
                bool isfile;
                path = this.MapPath(path, out isfile);
                return path;
            }
            else if (null == this.Parent)
                return path;
            else
                return this.Parent.MapPath(path);
        }

        /// <summary>
        /// Checks the source string and convers its to a full reference if possible. Returns true for isfile if the 
        /// resultant string is a local file reference.
        /// </summary>
        /// <param name="source">The orignal reference</param>
        /// <param name="isfile">Set to true if the result is now a local file</param>
        /// <returns>The converted full reference</returns>
        public virtual string MapPath(string source, out bool isfile)
        {
            var service = ServiceProvider.GetService<IPathMappingService>();

            if (!string.IsNullOrEmpty(this.LoadedSource))
            {
                return service.MapPath(this.LoadType, source, this.LoadedSource, out isfile);
            }
            else if (null != this.Parent)
                return this.Parent.MapPath(source, out isfile);
            else
            {
                return service.MapPath(this.LoadType, source, string.Empty, out isfile);
            }
        }

        
        #endregion

        #region internal virtual string GetIncrementID(PDFObjectType type)
        
        /// <summary>
        /// Returns a new ID for this object type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual string GetIncrementID(ObjectType type)
        {
            if (this.Parent != null)
                return this.Parent.GetIncrementID(type);
            else
                return TempIDFactory.GetTempID(type);
        }

        #endregion

        #region protected virtual IPDFComponent ParseComponentAtPath(string path)

        /// <summary>
        /// Parses a pdfx file located at the full path 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected virtual IComponent ParseComponentAtPath(string path)
        {
            if (null == this.Parent)
                throw RecordAndRaise.NullReference(Errors.ParentDocumentCannotBeNull);
            else
                return this.Parent.ParseComponentAtPath(path);
        }

        #endregion

        #region protected virtual IPDFRemoteComponent GetParsedParent()

        /// <summary>
        /// Gets the closest parent component that was parsed from a file
        /// </summary>
        /// <returns></returns>
        protected virtual IRemoteComponent GetParsedParent()
        {
            if (this is IRemoteComponent)
            {
                IRemoteComponent remote = (IRemoteComponent)this;
                if (remote.LoadType != ParserLoadType.None && !string.IsNullOrEmpty(remote.LoadedSource))
                    return remote;
            }

            if (null != this.Parent)
                return this.Parent.GetParsedParent();
            else
                return null;
        }

        #endregion

        #region internal virtual void ResetChildIDs()

        internal virtual void ResetChildIDs()
        {
            //Do Nothing
        }

        #endregion

        #region public void SetArrangement(PDFComponentArrangement arrange) + GetArrangement() + ClearArrangement()


        public void SetArrangement(PDFRenderContext context, Style style, PDFRect contentBounds)
        {
            ComponentMultiArrangement arrange = new ComponentMultiArrangement();
            arrange.PageIndex = context.PageIndex;
            arrange.RenderBounds = contentBounds;
            arrange.FullStyle = style;
            this.SetArrangement(arrange);
        }
        
        
        /// <summary>
        /// Sets an arrangement for this component
        /// </summary>
        /// <param name="arrange"></param>
        protected virtual void SetArrangement(ComponentArrangement arrange)
        {
            if (arrange is ComponentMultiArrangement)
            {
                if (_arrange == null)
                {
                    _arrange = arrange;
                    //this.EnsureAllChildrenAreOnThisPage(arrange);
                }

                else if (_arrange != null && _arrange is ComponentMultiArrangement)
                {
                    ((ComponentMultiArrangement)_arrange).AppendArrangement((ComponentMultiArrangement)arrange);
                }
                else
                    throw RecordAndRaise.InvalidCast(Errors.CannotConvertObjectToType, typeof(ComponentArrangement), typeof(ComponentMultiArrangement));
            }
            else
                _arrange = arrange;
        }

        #endregion

        #region public virtual PDFComponentArrangement GetFirstArrangement()

        /// <summary>
        /// Gets the root arrangement for this component. Use GetArrangement(pageindex) to get the arrangement for a particular page
        /// </summary>
        /// <returns></returns>
        public virtual ComponentArrangement GetFirstArrangement()
        {
            return _arrange;
        }

        #endregion

        #region public virtual void ClearArrangement()

        /// <summary>
        /// Clears all arrangements (rendering information) for this component
        /// </summary>
        public virtual void ClearArrangement()
        {
            _arrange = null;
        }

        #endregion

        #region public override string ToString()

        public override string ToString()
        {
            return String.Format("{0}:{1}",this.Type,string.IsNullOrEmpty(this.ID)?"[NO_ID]":this.ID);
        }

        #endregion

        #region IDisposable implementation

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (null != this._events)
                    _events.Dispose();
            }
        }

        ~Component()
        {
            this.Dispose(false);
        }

        #endregion
    }
}
