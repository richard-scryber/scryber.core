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
using System.Collections.ObjectModel;
using Scryber.Native;

namespace Scryber.Components
{
    public class ComponentList : PDFObject, ICollection<Component>, IPDFComponentList, IDisposable
    {
        /// <summary>
        /// Event that is raised when the contents of the collection are changed
        /// </summary>
        public event EventHandler CollectionChanged;

        /// <summary>
        /// Raises the collection changed event
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnCollectionChanged(EventArgs args)
        {
            if (null != CollectionChanged)
                this.CollectionChanged(this, args);
        }

        /// <summary>
        /// Raises the collection changed event with empty arguments
        /// </summary>
        protected void OnCollectionChanged()
        {
            this.OnCollectionChanged(EventArgs.Empty);
        }

        #region protected class InnerComponentList

        protected class InnerComponentList : List<Component>
        {
            internal ComponentList owner;

            public InnerComponentList(ComponentList owner)
            {
                this.owner = owner;
            }

            
        }

        #endregion

        private int _noopcount;

        public int NoOpCount
        {
            get { return this._noopcount; }
        }

        protected void IncrementNoOps()
        {
            this._noopcount++;
        }

        protected void DecrementNoOps()
        {
            if ((--this._noopcount) < 0)
                throw RecordAndRaise.ArgumentOutOfRange(Errors.NoOpCountOutOfRange);
        }

        private Component _owner;
        public Component Owner
        {
            get { return _owner; }
        }
        
        private string GetNextID(Component forComponent)
        {
            return this.Owner.GetIncrementID(forComponent.Type);
        }

        private InnerComponentList _items;

        protected InnerComponentList Items
        {
            get { return this._items; }
        }

        public ComponentList(Component owner, PDFObjectType Componenttype)
            : base(Componenttype)
        {
            this._owner = owner;
            this._items = new InnerComponentList(this);
            this._noopcount = 0;
        }

        public Component this[int index]
        {
            get
            {
                Component Component;
                if (index > -1 && index < Count)
                    Component = this.Items[index];
                else
                    Component = null;

                return Component;
            }
        }

        

        public void AddRange(IEnumerable<Component> items)
        {
            
            foreach (Component item in items)
            {
                this.Add(item);
            }
            this.OnCollectionChanged();
        }

        public int IndexOf(Component Component)
        {
            return this.Items.IndexOf(Component);
        }

        public void Insert(int index, Component Component)
        {
            this.Items.Insert(index, Component);
            Component.Parent = this.Owner;
            if (Component.Type == PDFObjectTypes.NoOp)
                this.IncrementNoOps();
            this.OnCollectionChanged();
        }

        #region ICollection<PDFComponent> Members

        public void Add(Component item)
        {
            if (null == item)
                throw new ArgumentNullException("item");
            item.Parent = this.Owner;
            this.Items.Add(item);
            if (item.Type == PDFObjectTypes.NoOp)
                this.IncrementNoOps();
            this.OnCollectionChanged();
        }

        public void Clear()
        {
            foreach (Component Component in this.Items)
            {
                if (Component.Parent == this.Owner)
                    Component.Parent = null;
            }
            this.Items.Clear();
            this._noopcount = 0;
            this.OnCollectionChanged();
        }

        public bool Contains(Component item)
        {
            return this.Items.Contains(item);
        }

        public void CopyTo(Component[] array, int arrayIndex)
        {
            this.Items.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return this.Items.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(Component item)
        {
            bool b = this.Items.Remove(item);
            if (b)
            {
                if (this.Owner.Equals(item.Parent))
                {
                    item.Parent = null;
                }
                if (item.Type == PDFObjectTypes.NoOp)
                    this.DecrementNoOps();

                this.OnCollectionChanged();
            }
            return b;
        }

        public void RemoveAt(int index)
        {
            Component item = this.Items[index];
            this.Items.RemoveAt(index);

            if (null != item)
            {
                if (this.Owner.Equals(item.Parent))
                    item.Parent = null;
                if (item.Type == PDFObjectTypes.NoOp)
                    this.DecrementNoOps();
            }

            this.OnCollectionChanged();
        }

        #endregion

        public Component[] ToArray()
        {
            return this.Items.ToArray();
        }

        #region IEnumerable<PDFComponent> Members

        public IEnumerator<Component> GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (Component item in this.Items)
                {
                    IDisposable id = item as IDisposable;

                    if(id != null)
                        id.Dispose();
                }
            }
        }

        ~ComponentList()
        {
            this.Dispose(false);
        }

        #endregion

        #region public void DataBind(PDFDataContext context)

        /// <summary>
        /// Databinds each of the components in the list.
        /// </summary>
        /// <param name="context"></param>
        public void DataBind(PDFDataContext context)
        {
            Component[] all = this.Items.ToArray();
            foreach (PDFObject obj in all)
            {
                if (obj is IPDFBindableComponent)
                    ((IPDFBindableComponent)obj).DataBind(context);
            }
        }

        #endregion

        #region IPDFComponentList Members

        void IPDFComponentList.Insert(int index, IPDFComponent component)
        {
            if (component is Component)
            {
                this.Insert(index, (Component)component);
                this.OnCollectionChanged();
            }
            else
                RecordAndRaise.InvalidCast(Errors.CannotConvertObjectToType, component.GetType(), "PDFComponent");
        }

        #endregion

        #region IEnumerable<IPDFComponent> Members

        IEnumerator<IPDFComponent> IEnumerable<IPDFComponent>.GetEnumerator()
        {
            List<IPDFComponent> all = new List<IPDFComponent>();
            foreach (Component comp in this)
            {
                all.Add(comp);
            }
            return all.GetEnumerator();
        }

        #endregion

        /// <summary>
        /// Checks the provided IPDFComponent is a valid full PDFComponent instance, and if so returns the cast instance.
        /// Otherwise throws an exception.
        /// </summary>
        /// <param name="impl"></param>
        /// <returns></returns>
        private Component ValidateFullComponent(IPDFComponent impl)
        {
            if (null == impl)
                throw new ArgumentNullException("impl");
            else if (impl is Component)
                return impl as Component;
            else
                throw new InvalidCastException(string.Format(Errors.CannotConvertObjectToType, impl.GetType(), typeof(Component)));
        }


        #region ICollection<IPDFComponent> Members

        void ICollection<IPDFComponent>.Add(IPDFComponent item)
        {
            Component comp = this.ValidateFullComponent(item);
            this.Add(comp);
        }

        void ICollection<IPDFComponent>.Clear()
        {
            this.Clear();
        }

        bool ICollection<IPDFComponent>.Contains(IPDFComponent item)
        {
            Component comp = this.ValidateFullComponent(item);
            return this.Contains(comp);
        }

        void ICollection<IPDFComponent>.CopyTo(IPDFComponent[] array, int arrayIndex)
        {
            for (int i = 0; i < this.Count; i++)
            {
                array[i + arrayIndex] = this[i];
            }
        }

        int ICollection<IPDFComponent>.Count
        {
            get { return this.Count; }
        }

        bool ICollection<IPDFComponent>.IsReadOnly
        {
            get { return this.IsReadOnly; }
        }

        bool ICollection<IPDFComponent>.Remove(IPDFComponent item)
        {
            Component comp = this.ValidateFullComponent(item);
            return this.Remove(comp);
        }

        #endregion
    }
    
    
}
