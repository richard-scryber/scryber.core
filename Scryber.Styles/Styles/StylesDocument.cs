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

namespace Scryber.Styles
{
    [PDFParsableComponent("Styles")]
    [PDFRemoteParsableComponent("Styles-Ref")]
    public class StylesDocument : StyleBase, IComponent, IBindableComponent, IRemoteComponent
    {

        public event PDFInitializedEventHandler Initialized;

        protected virtual void OnInit(PDFInitContext context)
        {
            if (null != this.Initialized)
                this.Initialized(this, new PDFInitEventArgs(context));
        }

        public event PDFLoadedEventHandler Loaded;

        protected virtual void OnLoaded(PDFLoadContext context)
        {
            if (null != this.Loaded)
                this.Loaded(this, new PDFLoadEventArgs(context));
        }

        public event PDFDataBindEventHandler DataBinding;

        protected virtual void OnDataBinding(PDFDataBindEventArgs args)
        {
            if (null != this.DataBinding)
                this.DataBinding(this, args);
        }

        public event PDFDataBindEventHandler DataBound;

        protected virtual void OnDataBound(PDFDataBindEventArgs args)
        {
            if (null != this.DataBound)
                this.DataBound(this, args);
        }

        //
        // properties
        //

        #region public PDFStyleCollection Styles

        private StyleCollection _styles;

        /// <summary>
        /// Gets or sets the inner style collection of this document
        /// </summary>
        [PDFElement("")]
        [PDFArray(typeof(Style))]
        public StyleCollection Styles
        {
            get
            {
                if (null == _styles)
                    _styles = new StyleCollection(this);
                return _styles;
            }
            set
            {
                if (null != _styles)
                    _styles.Owner = null;

                _styles = value;

                if (null != _styles)
                    _styles.Owner = this;
            }
        }

        #endregion

        #region public IPDFDocument Document {get;}

        /// <summary>
        /// Gets the PDF Document that holds this Styles document (if any)
        /// </summary>
        public IDocument Document
        {
            get { return (null == _parent) ? null : _parent.Document; }
        }

        #endregion

        #region public IPDFComponent Parent {get;set;}

        private IComponent _parent;

        /// <summary>
        /// Gets or sets the parent owner of this styles document
        /// </summary>
        public IComponent Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                _parent = value;
            }
        }

        #endregion

        #region public string ID {get;set;}

        private string _id;

        /// <summary>
        /// Gets or sets the ID of the style document
        /// </summary>
        [PDFAttribute("id")]
        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        #endregion

        #region public string ElementName {get;set;}

        /// <summary>
        /// Gets or sets the name of the element that this component was parsed from.
        /// </summary>
        public string ElementName
        {
            get;
            set;
        }

        #endregion

        #region public string LoadedSource {get;set;}

        private string _source;
        /// <summary>
        /// Gets or sets the full path to the source this document was loaded from
        /// </summary>
        public string LoadedSource
        {
            get { return _source; }
            set
            {
                this._source = value;
            }
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
                if (this._loadtype == ParserLoadType.None && this.Parent != null && this.Parent is IRemoteComponent)
                    return ((IRemoteComponent)this.Parent).LoadType;
                else
                    return _loadtype;
            }
            set { _loadtype = value; }
        }

        #endregion

        #region protected Dictionary<string, string> XmlNamespaceDeclarations {get;}

        private System.Collections.Specialized.NameValueCollection _namespaces;

        /// <summary>
        /// Dictionary of the namespaces declared in this styles document
        /// </summary>
        protected System.Collections.Specialized.NameValueCollection XmlNamespaceDeclarations
        {
            get { return _namespaces; }
        }

        #endregion

        #region public PDFItemCollection Items {get;}

        private PDFItemCollection _items = null;

        /// <summary>
        /// Gets a document centered collection of objects that can be accessed by name or index
        /// </summary>
        [PDFElement("Items")]
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

        #endregion

        //
        // ctor(s)
        //


        public StylesDocument()
            : this(PDFObjectTypes.StyleDocument)
        {
        }

        public StylesDocument(ObjectType type)
            : base(type)
        {
        }

        //
        // methods
        //

        /// <summary>
        /// Returns a full path to a resource based upon the 
        /// provided path and the root path of the document. If the 
        /// path cannot be determined returns the original path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public virtual string MapPath(string path)
        {
            bool isFile;
            if (!string.IsNullOrEmpty(this.LoadedSource))
                return this.MapPath(path, out isFile);
            else if (null == this.Parent)
                return path;
            else
                return Parent.MapPath(path);

        }


        public virtual string MapPath(string source, out bool isfile)
        {
            var service = ServiceProvider.GetService<IPathMappingService>();

            if (!string.IsNullOrEmpty(this.LoadedSource))
            {
                return service.MapPath(this.LoadType, source, this.LoadedSource, out isfile);
            }
            else
            {
                return service.MapPath(this.LoadType, source, string.Empty, out isfile);
            }
        }



        /// <summary>
        /// Merges all the appropriate style item values in this document into the provided style based on the component and it's class.
        /// </summary>
        /// <param name="style">The style to populate with items</param>
        /// <param name="Component">The component the styles should be associated with.</param>
        /// <param name="state">The state of the component - not used</param>
        public override void MergeInto(Scryber.Styles.Style style, IComponent Component, ComponentState state)
        {
            this.Styles.MergeInto(style, Component, state);
        }

        ///// <summary>
        ///// Creates a new style and returns all the merges styles
        ///// </summary>
        ///// <param name="classname"></param>
        ///// <returns></returns>
        //public override Scryber.Styles.PDFStyle MatchClass(string classname)
        //{
        //    return this.Styles.MatchClass(classname);
        //}

        public void Init(PDFInitContext context)
        {
            this.DoInit(context, true);
            this.OnInit(context);
        }

        protected virtual void DoInit(PDFInitContext context, bool includeChildren)
        {
            if (includeChildren)
                this.Styles.Init(context);
        }

        public void Load(PDFLoadContext context)
        {
            this.DoLoad(context, true);
            this.OnLoaded(context);
        }

        protected virtual void DoLoad(PDFLoadContext context, bool includeChildren)
        {
            if (includeChildren)
                this.Styles.Load(context);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                this.Styles.Dispose();
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        ~StylesDocument()
        {
            this.Dispose(false);
        }


        void Scryber.IRemoteComponent.RegisterNamespaceDeclaration(string prefix, string ns)
        {
            if (null == this._namespaces)
                this._namespaces = new System.Collections.Specialized.NameValueCollection();
            _namespaces.Add(prefix, ns);
        }


        IDictionary<string, string> Scryber.IRemoteComponent.GetDeclaredNamespaces()
        {
            Dictionary<string, string> all = new Dictionary<string, string>();
            if (null != this._namespaces)
            {
                foreach (string dec in this._namespaces.AllKeys)
                {
                    all.Add(dec, _namespaces[dec]);
                }
            }
            return all;
        }

        public void DataBind(PDFDataContext context)
        {
            PDFDataBindEventArgs args = new PDFDataBindEventArgs(context);
            this.OnDataBinding(args);
            this.DoDataBind(context, true);
            this.OnDataBound(args);

        }

        protected override void DoDataBind(PDFDataContext context, bool includechildren)
        {
            base.DoDataBind(context, includechildren);
            if (includechildren)
                this.Styles.DataBind(context);
        }
    }
}
