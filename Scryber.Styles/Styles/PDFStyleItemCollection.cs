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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scryber.Styles
{
    public class PDFStyleItemCollection : ICollection<PDFStyleItemBase>
    {
        private PDFStyleBase _owner;
        private Dictionary<PDFStyleKey, PDFStyleItemBase> _itemsbykey;

        public PDFStyleBase Owner
        {
            get { return _owner; }
        }

        public PDFStyleItemBase this[PDFStyleKey key]
        {
            get { return this._itemsbykey[key]; }
        }

        public PDFStyleItemCollection(PDFStyleBase owner)
        {
            this._owner = owner;
            this._itemsbykey = new Dictionary<PDFStyleKey, PDFStyleItemBase>();
        }

        public void Add(PDFStyleItemBase item)
        {
            if (null == item)
                throw new ArgumentNullException("item");
            
            // automatically copy the items into this style.
            item.Owner = this.Owner;

            //We only add if there is not an existing matching item
            if (!this._itemsbykey.ContainsKey(item.ItemKey))
                this._itemsbykey.Add(item.ItemKey, item);
        }

        public void AddRange(IEnumerable<PDFStyleItemBase> all)
        {
            foreach (PDFStyleItemBase item in all)
            {
                this.Add(item);
            }
        }

        public void Clear()
        {
            foreach (PDFStyleItemBase item in this._itemsbykey.Values)
            {
                item.Owner = null;
            }
            this._itemsbykey.Clear();
        }

        public bool Contains(PDFStyleKey key)
        {
            return this._itemsbykey.ContainsKey(key);
        }

        public bool Contains(PDFStyleItemBase item)
        {
            return this._itemsbykey.ContainsKey(item.ItemKey);
        }

        public void CopyTo(PDFStyleItemBase[] array, int arrayIndex)
        {
            this._itemsbykey.Values.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return this._itemsbykey.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(PDFStyleItemBase item)
        {
            if (this._itemsbykey.Remove(item.ItemKey))
            {
                if (item.Owner == this.Owner)
                    this.Owner.RemoveItemStyleValues(item.ItemKey);
                item.Owner = null;
                return true;
            }
            else
                return false;
        }

        public bool TryGetItem(PDFStyleKey key, out PDFStyleItemBase found)
        {
            return this._itemsbykey.TryGetValue(key, out found);
        }

        public IEnumerator<PDFStyleItemBase> GetEnumerator()
        {
            return this._itemsbykey.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
