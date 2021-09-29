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
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Text;
using Scryber.Styles.Selectors;

namespace Scryber.Styles
{
    /// <summary>
    /// A collection of style items
    /// </summary>
    public class StyleCollection : System.Collections.ObjectModel.Collection<StyleBase>
    {
        private IPDFComponent _owner;
        private bool _hasowner;

        /// <summary>
        /// Gets or sets the owner of this collection
        /// </summary>
        public IPDFComponent Owner
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

        public StyleCollection(IPDFComponent owner)
            : this(owner, true)
        {
            
        }

        protected StyleCollection(IPDFComponent owner, bool hasowner)
        {
            this.Owner = owner;
            this._hasowner = hasowner;
        }


        public void MergeInto(Style style, IPDFComponent forComponent, ComponentState state)
        {
            for (int i = 0; i < this.Count; i++)
            {
                StyleBase inner = this[i];
                inner.MergeInto(style, forComponent, state);
            }
            
        }

        private void UpdateAllParents(IPDFComponent parent)
        {
            foreach (StyleBase item in this)
            {
                if (item is IPDFComponent)
                {
                    ((IPDFComponent)item).Parent = parent;
                }
            }
        }

        //
        // lifecycle methods
        //

        public void Init(PDFInitContext context)
        {
            foreach (PDFObject item in this)
            {
                if (item is IPDFComponent)
                    (item as IPDFComponent).Init(context);
            }
        }

        public void Load(PDFLoadContext context)
        {
            foreach (PDFObject item in this)
            {
                if (item is IPDFComponent)
                    (item as IPDFComponent).Load(context);
            }
        }

        public void DataBind(PDFDataContext context)
        {
            foreach (PDFObject item in this)
            {
                if (item is IPDFBindableComponent)
                    (item as IPDFBindableComponent).DataBind(context);
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if(disposing)
            {
                foreach (PDFObject item in this)
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
                if (null != item && item is IPDFComponent)
                    ((IPDFComponent)item).Parent = this.Owner;
            }
        }

        protected override void InsertItem(int index, StyleBase item)
        {
            base.InsertItem(index, item);

            if (HasOwner)
            {
                if (null != item && item is IPDFComponent)
                    ((IPDFComponent)item).Parent = this.Owner;
            }
        }

        protected override void ClearItems()
        {
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

            if (null != removed && removed is IPDFComponent)
            {
                IPDFComponent comp = removed as IPDFComponent;
                comp.Parent = null;
            }
        }

        
    }
}
