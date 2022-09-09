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
using Scryber.PDF.Native;
using Scryber.Styles;
using Scryber.PDF.Resources;
using Scryber.Drawing;
using Scryber.PDF;
using Scryber.PDF.Graphics;
using Scryber.PDF.Layout;

namespace Scryber.Components
{
    public abstract class PageBase : VisualComponent, IStyledComponent, IResourceContainer, IPDFViewPortComponent,
                                                  IRemoteComponent, IControlledComponent, INamingContainer, ITopAndTailedComponent
    {

        //inner classes

        #region protected class PageAdornmentHash : Dictionary<int,PDFPageAdornment>

        /// <summary>
        /// Defines a collection of page adornments (Headers and Footers) based upon the page index
        /// </summary>
        protected class PageAdornmentHash : Dictionary<int,PageAdornment>
        {
        }

        #endregion


        //
        // inner elements
        //

        #region public PDFItemCollection Items {get;}

        private ItemCollection _items = null;

        /// <summary>
        /// Gets a page centered collection of objects that can be accessed by name or index. 
        /// The values will only be applied to this page and it's contents, rather than across the whole document
        /// </summary>
        [PDFElement("Params")]
        [PDFArray(typeof(IKeyValueProvider))]
        public ItemCollection Params
        {
            get
            {
                if (null == _items)
                    _items = new ItemCollection(this);
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

        #endregion

        #region  public IPDFTemplate Header {get;set;}

        private ITemplate _header;

        /// <summary>
        /// Gets or sets the header of this Page
        /// </summary>
        [PDFTemplate()]
        [PDFElement("Header")]
        public virtual ITemplate Header
        {
            get { return _header; }
            set { _header = value; }
        }

        #endregion

        #region public IPDFTemplate Footer {get;set;}

        private ITemplate _footer;

        /// <summary>
        /// Gets or sets the template for the footer of this Page
        /// </summary>
        [PDFTemplate()]
        [PDFElement("Footer")]
        public virtual ITemplate Footer
        {
            get { return _footer; }
            set { _footer = value; }
        }

        #endregion

        // INamingContianer interface

        IComponent INamingContainer.Owner
        {
            get { return this.Parent; }
            set { this.Parent = (Component)value; }
        }

        //
        // style properties
        //

        #region public PaperOrientation PageOrientation {get;set;}
        /// <summary>
        /// Gets or sets the orientation of the page
        /// </summary>
        [PDFAttribute("paper-orientation", Const.PDFStylesNamespace)]
        [PDFJSConvertor("scryber.studio.design.convertors.paperorientation_css")]
        [PDFDesignable("Orientation", Category = "Paper", Priority = 1, Type = "Select")]
        public PaperOrientation PaperOrientation
        {
            get { return this.Style.PageStyle.PaperOrientation; }
            set { this.Style.PageStyle.PaperOrientation = value; }
        }

        #endregion

        #region public PaperSize PaperSize  {get;set;}

        /// <summary>
        /// Gets or sets the size of the paper
        /// 
        /// </summary>
        [PDFAttribute("paper-size", Const.PDFStylesNamespace)]
        [PDFJSConvertor("scryber.studio.design.convertors.papersize_css")]
        [PDFDesignable("Size", Category = "Paper", Priority = 1, Type = "Select")]
        public PaperSize PaperSize
        {
            get { return this.Style.PageStyle.PaperSize; }
            set { this.Style.PageStyle.PaperSize = value; }
        }

        #endregion

        #region public int PageNumberStartIndex {get;set;}

        [PDFAttribute("page-number-start-index", Const.PDFStylesNamespace)]
        public int PageNumberStartIndex
        {
            get { return this.Style.PageStyle.NumberStartIndex; }
            set { this.Style.PageStyle.NumberStartIndex = value; }
        }

        #endregion

        #region public PageNumberStyle PageNumberStyle {get;set;}

        [PDFAttribute("page-number-style", Const.PDFStylesNamespace)]
        public PageNumberStyle PageNumberStyle
        {
            get { return this.Style.PageStyle.NumberStyle; }
            set { this.Style.PageStyle.NumberStyle = value; }
        }

        #endregion

        #region public int PageNumberStartIndex {get;set;}

        [PDFAttribute("page-number-display-format", Const.PDFStylesNamespace)]
        public string PageNumberDisplayFormat
        {
            get { return this.Style.PageStyle.PageNumberFormat; }
            set { this.Style.PageStyle.PageNumberFormat = value; }
        }

        #endregion

        #region public OverflowAction OverflowAction {get;set;}

        /// <summary>
        /// Gets or sets the overflow action of this page
        /// </summary>
        [PDFAttribute("overflow-action",Style.PDFStylesNamespace)]
        public OverflowAction OverflowAction
        {
            get { return this.Style.Overflow.Action; }
            set { this.Style.Overflow.Action = value; }
        }

        #endregion

        [PDFAttribute("page-angle", Style.PDFStylesNamespace)]
        [PDFDesignable("Page Rotation", Ignore = true, Category = "Paper", Priority = 1, Type = "Select")]
        public PageRotationAngles PageAngle
        {
            get { return this.Style.PageStyle.PageAngle; }
            set { this.Style.PageStyle.PageAngle = value; }
        }

        //
        // resources and artefacts
        //

        #region public PDFResourceList Resources {get;set;} + DoInitResources()

        /// <summary>
        /// private instance variable to hold the list of resources
        /// </summary>
        private PDFResourceList _resources;
        
        /// <summary>
        /// Gets the list of resources this page and its contents use 
        /// </summary>
        /// <remarks>Also implements the IPDFResourceContainer interface</remarks>
        [System.ComponentModel.Browsable(false)]
        public PDFResourceList Resources
        {
            get 
            {
                if (_resources == null)
                    _resources = this.DoInitResources();
                return _resources;
            }
            protected set { _resources = value; }
        }

        /// <summary>
        /// Virtual method that creates a new PDFResourceList for holding a pages resources.
        /// </summary>
        /// <returns>A new PDFResourceList</returns>
        protected virtual PDFResourceList DoInitResources()
        {
            PDFResourceList list = new PDFResourceList(this);
            return list;
        }

        #endregion

        #region protected PDFArtefactCollectionSet Artefacts

        private PDFArtefactCollectionSet _artefacts = null;

        /// <summary>
        /// Gets the set of Artefact collections for this Page
        /// </summary>
        protected PDFArtefactCollectionSet Artefacts
        {
            get
            {
                if (null == _artefacts)
                    _artefacts = new PDFArtefactCollectionSet();
                return _artefacts;
            }
        }

        #endregion

        #region IPDFDocument IPDFResourceContainer.Document
        
        IDocument IResourceContainer.Document
        {
            get { return this.Document; }
        }

        #endregion

        //
        // general processing properties
        //

        #region protected PDFItemCollection OriginalItems {get;set;}

        private ItemCollection _origitems;

        /// <summary>
        /// Property that stores the original items used by outer and sibling components whilst this and inner components will get the updated items collection
        /// </summary>
        protected ItemCollection OriginalItems
        {
            get { return _origitems; }
            set { _origitems = value; }
        }

        #endregion

        #region private int PageIndex { get; protected set; }

        private int _pageindex;

        /// <summary>
        /// Gets the first arranged (zero based) page index of this page in the document
        /// </summary>
        public int PageIndex
        {
            get { return _pageindex; }
            protected set { _pageindex = value; }
        }

        #endregion

        #region public int LastPageIndex {get; protected set;}

        private int _lastpageindex;

        public int LastPageIndex
        {
            get { return _lastpageindex; }
            protected set { _lastpageindex = value; }
        }

        #endregion

        #region internal bool ShowBadge {get;}

        private bool _howbadge = false;

        /// <summary>
        /// Returns true if this page should show the badge (scryber logo) when rendering. 
        /// This is usually as a result of insufficient licensing.
        /// </summary>
        internal bool ShowBadge
        {
            get
            {
                return _howbadge;
            }
        }

        #endregion

        #region protected PageAdornmentHash PageHeaders {get;set;}

        private PageAdornmentHash _headers;

        /// <summary>
        /// Gets or sets the collection of instantiated Page headers (can be null)
        /// </summary>
        protected PageAdornmentHash PageHeaders
        {
            get { return _headers; }
            set { _headers = value; }
        }

        #endregion

        #region protected PageAdornmentHash PageFooters {get;set;}

        private PageAdornmentHash _footers;

        /// <summary>
        /// Gets or sets the collection of instantiated PDFPageFooters (can be null)
        /// </summary>
        protected PageAdornmentHash PageFooters
        {
            get { return _footers; }
            set { _footers = value; }
        }

        #endregion

        #region public Scryber.Data.PDFXmlNamespaceCollection NamespaceDeclarations {get;}

        private Scryber.Data.PDFXmlNamespaceCollection _namespaces;

        /// <summary>
        /// Gets the namespaces that were declared on this page component if it was loaded remotely
        /// </summary>
        public Scryber.Data.PDFXmlNamespaceCollection NamespaceDeclarations
        {
            get
            {
                return _namespaces;
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

        //ctor

       

        #region protected PDFPage(PDFObjectType type)

        /// <summary>
        /// Protected constructor that can be called from subclasses to create a new PDF Page
        /// with a custom type
        /// </summary>
        /// <param name="type">The type name for the Page - usually 'Page'</param>
        protected PageBase(ObjectType type)
            : base(type)
        {
            this._resources = null;
        }


        #endregion

        // methods

        

        #region AddGeneratedHeader/Footer + GetGeneratedHeader/Footer

        /// <summary>
        /// Sets the PDFPageHeader Instance for the identified page
        /// </summary>
        /// <param name="header"></param>
        /// <param name="pageindex"></param>
        internal void AddGeneratedHeader(PDFPageHeader header, int pageindex)
        {
            if (null == PageHeaders)
                PageHeaders = new PageAdornmentHash();

            PageHeaders[pageindex] = header;
            header.Parent = this;
        }

        /// <summary>
        /// Gets the PDFPageHeader for the identified page and returns true if there is one.
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        internal bool GetHeaderForPage(int pageindex, out PDFPageHeader header)
        {
            PageAdornment adorn;
            if (null != PageHeaders && PageHeaders.TryGetValue(pageindex, out adorn))
            {
                header = (PDFPageHeader)adorn;
                return true;
            }
            else
            {
                header = null;
                return false;

            }
        }

        /// <summary>
        /// Adds the generated page footer to this page
        /// </summary>
        /// <param name="footer"></param>
        /// <param name="pageindex"></param>
        internal void AddGeneratedFooter(PDFPageFooter footer, int pageindex)
        {
            if (null == PageFooters)
                PageFooters = new PageAdornmentHash();

            PageFooters[pageindex] = footer;
            footer.Parent = this;
        }

        /// <summary>
        /// Attempts to retrieve a page footer for the specified page index. returning true if found
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="footer"></param>
        /// <returns></returns>
        internal bool GetFooterForPage(int pageindex, out PDFPageFooter footer)
        {
            PageAdornment adorn;
            if (null != PageFooters && PageFooters.TryGetValue(pageindex, out adorn))
            {
                footer = (PDFPageFooter)adorn;
                return true;
            }
            else
            {
                footer = null;
                return false;
            }
        }

        #endregion

        

        #region public virtual int GetPageIndex(PDFVisualComponent ele)

        /// <summary>
        /// Gets the zero based index of the page in the document where this Component appears 
        /// </summary>
        /// <param name="ele"></param>
        /// <returns></returns>
        public virtual int GetPageIndex(VisualComponent ele)
        {
            return this.PageIndex;
        }

        #endregion

        #region public override PDFGraphics CreateGraphics(PDFWriter writer, PDFStyleStack styles)

        public PDFGraphics CreateGraphics(PDFWriter writer, StyleStack styles, ContextBase context)
        {
            Style full = styles.GetFullStyle(this);
            PageSize size = full.CreatePageSize();
            return PDFGraphics.Create(writer, false, this, DrawingOrigin.TopLeft, size.Size, context);
        }

        #endregion

        #region internal override void RegisterPreLayout() + RegisterLayoutComplete() + RegisterPostRender() + RegisterPreRender()

        /// <summary>
        /// Overrides base implementation to extend to the headers and footers on the page
        /// </summary>
        internal override void RegisterPreLayout(LayoutContext context)
        {
            if (null != this.PageHeaders)
            {
                foreach (PageAdornment adorn in this.PageHeaders.Values)
                {
                    adorn.RegisterPreLayout(context);
                }
            }
            base.RegisterPreLayout(context);

            if (null != this.PageFooters)
            {
                foreach (PageAdornment adorn in this.PageFooters.Values)
                {
                    adorn.RegisterPreLayout(context);
                }
            }
        }

        /// <summary>
        /// Overrides base implementation to extend to the headers and footers on the page
        /// </summary>
        internal override void RegisterLayoutComplete(LayoutContext context)
        {
            
            if (null != this.PageHeaders)
            {
                foreach (PageAdornment adorn in this.PageHeaders.Values)
                {
                    adorn.RegisterLayoutComplete(context);
                }
            }

            base.RegisterLayoutComplete(context);
            if (null != this.PageFooters)
            {
                foreach (PageAdornment adorn in this.PageFooters.Values)
                {
                    adorn.RegisterLayoutComplete(context);
                }
            }
        }

        /// <summary>
        /// Overrides base implementation to extend to the headers and footers on the page
        /// </summary>
        internal override void RegisterPostRender(RenderContext context)
        {
            if (null != this.PageHeaders)
            {
                foreach (PageAdornment adorn in this.PageHeaders.Values)
                {
                    adorn.RegisterPostRender(context);
                }
            }

            base.RegisterPostRender(context);

            if (null != this.PageFooters)
            {
                foreach (PageAdornment adorn in this.PageFooters.Values)
                {
                    adorn.RegisterPostRender(context);
                }
            }
        }

        /// <summary>
        /// Overrides base implementation to extend to the headers and footers on the page
        /// </summary>
        internal override void RegisterPreRender(RenderContext context)
        {
           

            if (null != this.PageHeaders)
            {
                foreach (PageAdornment adorn in this.PageHeaders.Values)
                {
                    adorn.RegisterPreRender(context);
                }
            }

            base.RegisterPreRender(context);

            if (null != this.PageFooters)
            {
                foreach (PageAdornment adorn in this.PageFooters.Values)
                {
                    adorn.RegisterPreRender(context);
                }
            }
        }

        #endregion

        #region protected override PDFStyle GetBaseStyle()

        /// <summary>
        /// Overrides the GetBaseStyle() to include an overflow action of none - default for a page
        /// </summary>
        /// <returns></returns>
        protected override Style GetBaseStyle()
        {
            Scryber.Styles.Style flat = base.GetBaseStyle();
            flat.Overflow.Action = OverflowAction.None;

            return flat;
        }

        #endregion

        #region public void SetShowBadge()

        /// <summary>
        /// Call this method to ensure that the badge (scryber logo) is rendered on the page output.
        /// </summary>
        public void SetShowBadge()
        {
            this._howbadge = true;
        }

        #endregion

        #region IResourceContainer Members

        string IResourceContainer.Register(ISharedResource rsrc)
        {
            return this.Register((PDFResource)rsrc).Value;
        }

        public PDFName Register(PDFResource reference)
        {
            if (null == reference.Name || string.IsNullOrEmpty(reference.Name.Value))
            {
                string name = this.Document.GetIncrementID(reference.Type);
                reference.Name = (PDFName)name;
            }
            reference.RegisterUse(this.Resources,this);
            return reference.Name;
        }

        #endregion

        /// <summary>
        /// Abstract declaration of the IPDFViewPort.GetEngine method
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="context"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        public abstract IPDFLayoutEngine GetEngine(IPDFLayoutEngine parent, PDFLayoutContext context, Style style);
        

        #region IPDFRemoteComponent Members

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
            if (null != this.NamespaceDeclarations)
            {
                foreach (Scryber.Data.XmlNamespaceDeclaration dec in this.NamespaceDeclarations)
                {
                    all.Add(dec.Prefix, dec.NamespaceURI);
                }
            }
            return all;
        }

        #endregion

        //
        // data bind and artefact overrides for updating the items collection with any registered on this page so that 
        // they are used by this and inner components
        //

        #region protected override void OnDataBinding(PDFDataContext context)

        /// <summary>
        /// Overrides the base implementation to update the items collection (if and only if this page has some of it's own items)
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDataBinding(DataContext context)
        {
            this.OriginalItems = context.Items;

            if (this.HasParams)
            {
                ItemCollection updated = this.OriginalItems.Clone();
                updated.Merge(this.Params);
                context.Items = updated;
            }

            base.OnDataBinding(context);
        }

        #endregion

        #region protected override void OnDataBound(PDFDataBindEventArgs e)

        /// <summary>
        /// Overrides the base implementation to restore the items collection to it's original value
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDataBound(DataContext context)
        {
            base.OnDataBound(context);

            if (null != this.OriginalItems)
                context.Items = this.OriginalItems;
        }

        #endregion

        #region protected override void DoRegisterArtefacts(PDFLayoutContext context, PDFArtefactRegistrationSet set, PDFStyle fullstyle)

        /// <summary>
        /// Overrides the base implementation to update the items collection (if and only if this page has some of it's own declared items)
        /// </summary>
        /// <param name="context"></param>
        /// <param name="set"></param>
        /// <param name="fullstyle"></param>
        protected override void DoRegisterArtefacts(PDFLayoutContext context, PDFArtefactRegistrationSet set, Style fullstyle)
        {
            this.OriginalItems = context.Items;

            if(this.HasParams)
            {
                ItemCollection updated = this.OriginalItems.Clone();
                updated.Merge(this.Params);
                context.Items = updated;
            }

            base.DoRegisterArtefacts(context, set, fullstyle);
        }

        #endregion

        #region protected override void DoCloseLayoutArtefacts(PDFLayoutContext context, PDFArtefactRegistrationSet artefacts, PDFStyle fullstyle)

        /// <summary>
        /// Overrides the base implementation to restore the items collection to it's original value
        /// </summary>
        /// <param name="context"></param>
        /// <param name="artefacts"></param>
        /// <param name="fullstyle"></param>
        protected override void DoCloseLayoutArtefacts(PDFLayoutContext context, PDFArtefactRegistrationSet artefacts, Style fullstyle)
        {
            base.DoCloseLayoutArtefacts(context, artefacts, fullstyle);

            context.Items = this.OriginalItems;
        }

        #endregion
    }


    /// <summary>
    /// A list of pages, sections or page groups (all inheriting from PDFPageBase)
    /// </summary>
    public class PageList : ComponentWrappingList<PageBase>
    {
        public PageList(ComponentList innerList)
            : base(innerList)
        {
        }
    }


    
}
