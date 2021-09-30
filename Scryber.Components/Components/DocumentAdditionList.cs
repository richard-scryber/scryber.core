using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.PDF;

namespace Scryber.Components
{
    /// <summary>
    /// List for all the document additions which are not visual components.
    /// </summary>
    /// <remarks>
    /// The class includes methods for calling each document stage,
    /// and it will inturn call that stage on
    /// each of the contained components if they support it.
    /// </remarks>
    public class DocumentAdditionList : IList<IComponent>
    {

        private Document _doc;
        private List<IComponent> _inner;

        #region public PDFDocument Owner {get;set;}

        /// <summary>
        /// Gets the document owner of this list
        /// </summary>
        public Document Owner
        {
            get { return _doc; }
            internal set
            {
                _doc = value;

                foreach (IComponent comp in this)
                {
                    comp.Parent = value;
                }
            }
        }

        #endregion

        #region public int Count

        /// <summary>
        /// Gets the number of components in this list 
        /// </summary>
        public int Count
        {
            get { return _inner.Count; }
        }

        #endregion

        #region  bool ICollection<IPDFComponent>.IsReadOnly

        /// <summary>
        /// Returns false
        /// </summary>
        bool ICollection<IComponent>.IsReadOnly
        {
            get { return ((ICollection<IComponent>)_inner).IsReadOnly; }
        }

        #endregion

        #region public IPDFComponent this[int index]

        /// <summary>
        /// Gets or Sets the component at the specified index of this list. Any component added or removed
        /// in the set operation will have it's parent updated to the owning document.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IComponent this[int index]
        {
            get { return this._inner[index]; }
            set
            {
                IComponent orig = this._inner[index];
                this._inner[index] = value;

                if (null != orig)
                    orig.Parent = null;

                if (null != value)
                    value.Parent = _doc;
            }
        }

        #endregion

        //
        // .ctor
        //

        #region public PDFDocumentAdditionList(PDFDocument parent)

        /// <summary>
        /// Creates a new instance of the additions list for holding non visual components
        /// </summary>
        /// <param name="parent"></param>
        public DocumentAdditionList(Document parent)
        {
            _doc = parent;
            _inner = new List<IComponent>();
        }

        #endregion

        //
        // IList methods
        //

        #region public void Add(IPDFComponent comp)

        /// <summary>
        /// Adds a component to the end of the list and sets it's parent to the document
        /// </summary>
        /// <param name="comp"></param>
        public void Add(IComponent comp)
        {
            comp.Parent = _doc;
            _inner.Add(comp);
        }

        #endregion

        #region public bool Remove(IPDFComponent comp)

        /// <summary>
        /// Removes a component from the list and clears it's parent (if it is this list's owner).
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        public bool Remove(IComponent comp)
        {
            if (_inner.Remove(comp))
            {
                if (comp.Parent == this._doc)
                    comp.Parent = null;
                return true;
            }
            else
                return false;
        }

        #endregion

        #region public void RemoveAt(int index)

        /// <summary>
        /// Removes the component at the specified index out of the list,
        /// and clears it's parent (if it is this list's owner)
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            IComponent removed = this._inner[index];
            this._inner.RemoveAt(index);
            if (removed.Parent == this._doc)
                removed.Parent = null;
        }

        #endregion

        #region public int IndexOf(IPDFComponent comp)

        /// <summary>
        /// Returns the index of the component in the list
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        public int IndexOf(IComponent comp)
        {
            return this._inner.IndexOf(comp);
        }

        #endregion

        #region public void Insert(int index, IPDFComponent comp)

        /// <summary>
        /// Inserts the component at the specified index in the list
        /// </summary>
        /// <param name="index"></param>
        /// <param name="comp"></param>
        public void Insert(int index, IComponent comp)
        {
            this._inner.Insert(index, comp);
            comp.Parent = this._doc;
        }

        #endregion

        #region public void Clear()

        /// <summary>
        /// Removes all the components from the list and clears their parent
        /// </summary>
        public void Clear()
        {
            IComponent[] all = this._inner.ToArray();
            this._inner.Clear();
            for (int i = 0; i < all.Length; i++)
            {
                if (all[i].Parent == this._doc)
                    all[i].Parent = null;
            }

        }

        #endregion

        #region public bool Contains(IPDFComponent comp)

        /// <summary>
        /// Returns true if this list contains the component provided
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        public bool Contains(IComponent comp)
        {
            return this._inner.Contains(comp);
        }

        #endregion

        #region public void CopyTo(IPDFComponent[] all)

        /// <summary>
        /// Copies all the components into the array
        /// </summary>
        /// <param name="all"></param>
        public void CopyTo(IComponent[] all)
        {
            this._inner.CopyTo(all);
        }

        #endregion

        #region public void CopyTo(IPDFComponent[] all, int arrayIndex)

        /// <summary>
        /// Copies all the components into the array starting at the specified index
        /// </summary>
        /// <param name="all"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(IComponent[] all, int arrayIndex)
        {
            this._inner.CopyTo(all, arrayIndex);
        }

        #endregion

        #region public IEnumerator<IPDFComponent> GetEnumerator()

        /// <summary>
        /// Returns an enumerator that itterates through the list
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IComponent> GetEnumerator()
        {
            return this._inner.GetEnumerator();
        }

        
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this._inner.GetEnumerator();
        }

        #endregion

        #region public IPDFComponent[] ToArray()
         
        /// <summary>
        /// Copies the elements of the list to a new array and returns it
        /// </summary>
        /// <returns></returns>
        public IComponent[] ToArray()
        {
            return _inner.ToArray();
        }

        #endregion

        //
        // IPDFComponent method invocation on each inner component
        //

        #region public void Init(PDFInitContext context)

        /// <summary>
        /// Initializes each of the components in this list
        /// </summary>
        /// <param name="context"></param>
        public void Init(PDFInitContext context)
        {
            foreach (IComponent comp in this)
            {
                comp.Init(context);
            }
        }

        #endregion

        #region public void Load(PDFLoadContext context)

        /// <summary>
        /// Invokes the load on each of the components in this list
        /// </summary>
        /// <param name="context"></param>
        public void Load(PDFLoadContext context)
        {
            foreach (IComponent comp in this)
            {
                comp.Load(context);
            }
        }

        #endregion

        #region public void DataBind(PDFDataContext context)

        /// <summary>
        /// Invokes the data binding on each of the components in this list
        /// that implement the IPDFBindable interface
        /// </summary>
        /// <param name="context"></param>
        public void DataBind(PDFDataContext context)
        {
            IComponent[] all = this.ToArray();

            for (int i = 0; i < all.Length; i++)
            {
                IComponent comp = all[i];
                if (comp is IBindableComponent)
                    ((IBindableComponent)comp).DataBind(context);
            }
        }

        #endregion

        public void RegisterPreLayout(PDFLayoutContext context)
        {
            foreach(IComponent com in this)
            {
                if (com is Component)
                    ((Component)com).RegisterPreLayout(context);
            }
        }

        
        public void RegisterLayoutComplete(PDFLayoutContext context)
        {
            foreach (IComponent comp in this)
            {
                if (comp is Component)
                    ((Component)comp).RegisterLayoutComplete(context);
            }
        }


        
        public void RegisterPreRender(PDFRenderContext context)
        {
            foreach (IComponent comp in this)
            {
                if (comp is Component)
                    ((Component)comp).RegisterPreRender(context);
            }
        }
        
        #region public void OutputToPDF(PDFRenderContext context, PDFWriter writer)

        /// <summary>
        /// Invokes the output to PDF on each of the components in this list
        /// that implement the IPDFRenderComponent interface
        /// </summary>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        public void OutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            foreach (IComponent comp in this)
            {
                if (comp is IPDFRenderComponent)
                    ((IPDFRenderComponent)comp).OutputToPDF(context, writer);
            }
        }

        #endregion


        public void RegisterPostRender(PDFRenderContext context)
        {
            foreach (IComponent comp in this)
            {
                if (comp is Component)
                    ((Component)comp).RegisterPostRender(context);
            }
        }
    }
}
