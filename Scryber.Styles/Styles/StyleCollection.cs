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
using Scryber.Styles.Selectors;

namespace Scryber.Styles
{
    /// <summary>
    /// A collection of style items
    /// </summary>
    public class StyleCollection : System.Collections.ObjectModel.Collection<StyleBase>
    {
        /// <summary>
        /// The number of items in the collection after which an index tree should be built and used, rather than just looping through the items.
        /// </summary>
        private const int StyleIndexLimit = 30;

        private IComponent _owner;
        private bool _hasowner;
        private StyleRootIndexTree _index;

        /// <summary>
        /// Gets or sets the owner of this collection
        /// </summary>
        public IComponent Owner
        {
            get { return _owner; }
            set
            {
                _owner = value;
                _hasowner = null != value;
                this.UpdateAllParents(value);
            }
        }

        protected bool HasOwner
        {
            get { return _hasowner; }
        }

        //
        // ctor
        //

        public StyleCollection()
            : this(null, false)
        { }

        public StyleCollection(IComponent owner)
            : this(owner, true)
        {
        }

        protected StyleCollection(IComponent owner, bool hasowner)
        {
            this.Owner = owner;
            this._hasowner = hasowner;
            this._index = null;
        }


        public void MergeInto(Style style, IComponent forComponent)
        {
            if (this.ShouldUseIndex())
            {
                var index = this.EnsureIndex();
                foreach (var item in index.GetTopMatched(forComponent))
                {
                    item.MergeInto(style, forComponent);
                }
            }
            else
            {
                for (int i = 0; i < this.Count; i++)
                {
                    StyleBase inner = this[i];
                    inner.MergeInto(style, forComponent);
                }
            }
            
        }

        private void UpdateAllParents(IComponent parent)
        {
            foreach (StyleBase item in this)
            {
                if (item is IComponent comp)
                {
                    comp.Parent = parent;
                }
                else if(item is INamingContainer naming)
                {
                    naming.Owner = parent;
                }
            }
        }

        //
        // TreeIndex methods
        //

        public virtual bool ShouldUseIndex()
        {
            return this.Count > StyleIndexLimit;
        }

        

        protected virtual StyleRootIndexTree EnsureIndex()
        {
            if(null == this._index)
            {
                this._index = new StyleRootIndexTree();
                this.PopulateIndex(this._index, this);
            }

            return this._index;
        }

        private void PopulateIndex(StyleRootIndexTree index, StyleCollection withCollection)
        {
            foreach (var style in withCollection)
            {
                index.AddStyle(style);
            }
        }

        protected virtual void ResetIndex()
        {
            this._index = null;
        }

        //
        // lifecycle methods
        //

        public void Init(InitContext context)
        {
            foreach (TypedObject item in this)
            {
                if (item is IComponent)
                    (item as IComponent).Init(context);
            }
        }

        public void Load(LoadContext context)
        {
            foreach (TypedObject item in this)
            {
                if (item is IComponent)
                    (item as IComponent).Load(context);
            }
        }

        public void DataBind(DataContext context)
        {
            foreach (TypedObject item in this)
            {
                if (item is IBindableComponent)
                    (item as IBindableComponent).DataBind(context);
            }

            //After databinding we make sure the index gets populated
            this.ResetIndex();
            
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if(disposing)
            {
                foreach (TypedObject item in this)
                {
                    if (item is IDisposable)
                        (item as IDisposable).Dispose();
                }
            }
        }

        //
        // overrides
        //

        protected override void SetItem(int index, StyleBase item)
        {
            base.SetItem(index, item);

            if (HasOwner)
            {
                if (null != item)
                {
                    if (item is IComponent comp)
                        comp.Parent = this.Owner;
                    else if (item is INamingContainer naming)
                        naming.Owner = this.Owner;
                }
            }
            this.ResetIndex();
        }

        protected override void InsertItem(int index, StyleBase item)
        {
            base.InsertItem(index, item);

            if (HasOwner)
            {
                if (null != item)
                {
                    if (item is IComponent comp)
                        comp.Parent = this.Owner;
                    else if (item is INamingContainer naming)
                        naming.Owner = this.Owner;
                }
            }
            this.ResetIndex();
        }

        protected override void ClearItems()
        {
            this.ResetIndex();

            if (HasOwner)
            {
                this.UpdateAllParents(null);
            }
            base.ClearItems();
        }


        protected override void RemoveItem(int index)
        {
            StyleBase removed = null;
            if (HasOwner)
            {
                if (index >= 0 && index < this.Count)
                {
                    removed = this[index];
                   
                }
            }

            base.RemoveItem(index);

            if (null != removed)
            {
                if (removed is IComponent comp)
                    comp.Parent = null;
                else if (removed is INamingContainer naming)
                    naming.Owner = null;
            }

            this.ResetIndex();
        }

        
    }
}
