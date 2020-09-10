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

namespace Scryber.Components
{
    /// <summary>
    /// Base abstract wrapping list, that is used to encapsulate a list of components 
    /// whilst ignoring 'invisible' container and including their children
    /// </summary>
    /// <remarks>This non-stongly typed class allows instance variables to hold </remarks>
    public abstract class ComponentWrappingList : IPDFComponentWrappingList,  System.Collections.IEnumerable
    {

        #region public PDFComponentList InnerList {get;}

        /// <summary>
        /// The instance variable holding a reference to the inner list provided at the constructor
        /// </summary>
        private ComponentList _inner;

        /// <summary>
        /// Gets the source inner list 
        /// </summary>
        public ComponentList InnerList
        {
            get { return _inner; }
        }

        /// <summary>
        /// IPDFComponentWrappingList implementation of the InnerList get accessor
        /// </summary>
        IPDFComponentList IPDFComponentWrappingList.InnerList
        {
            get { return _inner; }
        }

        #endregion


        #region public abstract int Count { get; }

        /// <summary>
        /// Gets the count of items in this collection.
        /// </summary>
        public abstract int Count { get; }

        #endregion


        public ComponentWrappingList(ComponentList inner)
        {
            if (null == inner)
                throw RecordAndRaise.ArgumentNull("innerList");

            this._inner = inner;

            //hook into the changed event so we are notified if any new items are added to the collection
            this._inner.CollectionChanged += new EventHandler(inner_CollectionChanged);
        }



        //
        // IEnumerable implementation
        //


        #region System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()

        /// <summary>
        /// Interface implementation for the IEnumerable of the wrapping list
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.DoGetEnumerator();
        }

        #endregion

        #region protected abstract System.Collections.IEnumerator DoGetEnumerator();

        /// <summary>
        /// Abstract method that must return the loosely typed 
        /// IEnumerator required for the IEnumberable.GetEnumerator implementation
        /// </summary>
        /// <returns></returns>
        protected abstract System.Collections.IEnumerator DoGetEnumerator();

        #endregion

        //
        // inner collection changed notifications
        //

        #region  void inner_CollectionChanged(object sender, EventArgs e)

        /// <summary>
        /// This method is called when the inner wrapped collection is changed.
        /// This could be either through an addition, replace or removal.
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The arguments associated with this event</param>
        void inner_CollectionChanged(object sender, EventArgs e)
        {
            this.DoReleaseCache();
        }

        #endregion

        #region protected virtual void DoReleaseCache()

        /// <summary>
        /// Method that is called when the inner collection has 
        /// changed and any cached data about this list should be cleared.
        /// </summary>
        /// <remarks>Inheritors should override this method to 
        /// release any stored cache data in this instance</remarks>
        protected virtual void DoReleaseCache()
        {
        }

        #endregion

    }

    public abstract class ComponentWrappingList<T> : ComponentWrappingList, ICollection<T> where T : Component
    {

        #region ivars

        /// <summary>
        /// A cached collection of all the items of the required type
        /// </summary>
        private List<T> _allitems = null;

        /// <summary>
        /// A flag that is set when the inner list has changed for any reason
        /// </summary>
        private bool _rebuild = false;

        #endregion

        #region protected List<T> AllItems

        /// <summary>
        /// Gets the collection of all items in the wrapped list. 
        /// This is built dyamically at request time
        /// </summary>
        protected List<T> AllItems
        {
            get
            {
                if (null == _allitems)
                    _allitems = BuildAllItems(this.InnerList);

                else if (_rebuild)
                {
                    _allitems.Clear();
                    BuildAllItems(this.InnerList, _allitems);
                }
                return _allitems;
            }
        }

        #endregion

        #region public T this[int index] {get;}

        /// <summary>
        /// Gets the item at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index]
        {
            get { return (T)this.AllItems[index]; }
        }

        #endregion

        #region public override int Count {get;}

        /// <summary>
        /// Gets the total number of items in this list (including any in invisible containers)
        /// </summary>
        public override int Count
        {
            get { return this.AllItems.Count; }
        }

        #endregion

        #region public bool IsReadOnly {get;}

        /// <summary>
        /// Returns true if this collection is readonly
        /// </summary>
        public bool IsReadOnly
        {
            get { return this.InnerList.IsReadOnly; }
        }

        #endregion

        //
        // .ctor
        //

        #region public PDFComponentWrappingList(PDFComponentList innerList)

        /// <summary>
        /// Creates a new instance of the Strongly typed PDFComponentWrappingList 
        /// using the provided component list as the source of the items in the list
        /// </summary>
        /// <param name="innerList"></param>
        public ComponentWrappingList(ComponentList innerList)
            : base(innerList)
        {
            
        }

        #endregion


        

        

        #region ICollection<T> Members

        public void Add(T item)
        {
            this.InnerList.Add(item);
        }

        public void Clear()
        {
            this.InnerList.Clear();
        }

        public bool Contains(T item)
        {
            return this.InnerList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.InnerList.CopyTo(array, arrayIndex);
        }

        public void RemoveAt(int index)
        {
            this.InnerList.RemoveAt(index);
        }

        public bool Remove(T item)
        {
            return this.InnerList.Remove(item);
        }

        #endregion

        /// <summary>
        /// Inserts the component as the specified position in the list
        /// </summary>
        /// <param name="index"></param>
        /// <param name="component"></param>
        public void Insert(int index, T component)
        {
            this.InnerList.Insert(index, component);
        }

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return this.AllItems.GetEnumerator();
        }

        #endregion


        protected override System.Collections.IEnumerator DoGetEnumerator()
        {
            return this.AllItems.GetEnumerator();
        }
        

        protected override void DoReleaseCache()
        {
            if (null != _allitems)
            {
                this._allitems.Clear();
                this.BuildAllItems(this.InnerList, _allitems);
            }
            base.DoReleaseCache();
        }

        private List<T> BuildAllItems(ComponentList content)
        {
            List<T> all = new List<T>();
            this.BuildAllItems(content, all);
            return all;
        }

        protected virtual void BuildAllItems(ComponentList content, List<T> found)
        {
            foreach (Component comp in content)
            {
                if (comp is IPDFInvisibleContainer)
                {
                    IPDFInvisibleContainer container = comp as IPDFInvisibleContainer;
                    if (container.HasContent)
                        this.BuildAllItems(container.Content, found);
                }
                else if (comp is IPDFDataSource)
                {
                    continue;
                }
                else if (comp.Type != PDFObjectTypes.NoOp)
                {
                    if (comp is T)
                        found.Add(comp as T);
                }
            }
        }

    }
}
