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

namespace Scryber.PDF.Native
{
    /// <summary>
    /// An array containing any number of PDFFileObjects of any type. The PDFArray is itself a PDFFileObject
    /// </summary>
    public class PDFArray : IPDFFileObject, ICollection<IPDFFileObject>, IPDFObjectContainer
    {
        public ObjectType Type { get { return ObjectTypes.Array; } }

        /// <summary>
        /// Initializes a new empty PDFArray
        /// </summary>
        public PDFArray()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new PDFArray adding the range of PDFFileObject(s) to itself
        /// </summary>
        /// <param name="items">The items to add</param>
        public PDFArray(IEnumerable<IPDFFileObject> items)
            : this()
        {
            if(null != items)
                this.InnerList.AddRange(items);
        }

        public PDFArray(params IPDFFileObject[] items)
        {
            if(null != items)
                this.InnerList.AddRange(items);
        }

        private List<IPDFFileObject> _items = new List<IPDFFileObject>();

        /// <summary>
        /// The inner list to hold the contents
        /// </summary>
        protected List<IPDFFileObject> InnerList
        {
            get { return this._items; }
        }


        /// <summary>
        /// Gets or sets the IFileObject at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IPDFFileObject this[int index]
        {
            get { return this.InnerList[index]; }
            set { this.InnerList[index] = value; }
        }


        #region ICollection<PDFFileObject> Members

        /// <summary>
        /// Adds a new item to the array
        /// </summary>
        /// <param name="item">The item to add</param>
        public void Add(IPDFFileObject item)
        {
            this.InnerList.Add(item);
        }

        /// <summary>
        /// Clears the array of all items
        /// </summary>
        public void Clear()
        {
            this.InnerList.Clear();
        }

        /// <summary>
        /// Returns true if the Array contains the item
        /// </summary>
        /// <param name="item">The item to find</param>
        /// <returns>True if found</returns>
        public bool Contains(IPDFFileObject item)
        {
            return this.InnerList.Contains(item);
        }

        /// <summary>
        /// Copies the entire contents of this PDFArray into the array parameter starting at arrayIndex of the target array
        /// </summary>
        /// <param name="array">The target array to copy to</param>
        /// <param name="arrayIndex">The index of the target array to start copying items into</param>
        public void CopyTo(IPDFFileObject[] array, int arrayIndex)
        {
            this.InnerList.CopyTo(array, arrayIndex);
        }


        /// <summary>
        /// Gets the count of items in the array
        /// </summary>
        public int Count
        {
            get { return this.InnerList.Count; }
        }

        /// <summary>
        /// Identifies whether this arrray is read only
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the specified item from the array if it exists
        /// </summary>
        /// <param name="item">The item to remove</param>
        /// <returns>True if the item was removed</returns>
        public bool Remove(IPDFFileObject item)
        {
            return this.InnerList.Remove(item);
        }

        /// <summary>
        /// Removes the item from the collection at the specified index
        /// </summary>
        /// <param name="index">The zero based index from which to remove the item</param>
        public void RemoveAt(int index)
        {
            this.InnerList.RemoveAt(index);
        }
        #endregion

        #region IEnumerable<PDFFileObject> Members

        /// <summary>
        /// Returns an enumerator that can itterate though each of the items in the list
        /// </summary>
        /// <returns>The enumerator</returns>
        public IEnumerator<IPDFFileObject> GetEnumerator()
        {
            return this.InnerList.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        /// <summary>
        /// Writes the underlying data of the file object to the passed text writer
        /// </summary>
        /// <param name="tw">The text writer object to write data to</param>
        public void WriteData(PDFWriter writer)
        {
            writer.BeginArrayS();
            foreach (IPDFFileObject pfo in this)
            {
                writer.BeginArrayEntry();
                pfo.WriteData(writer);
                writer.EndArrayEntry();
            }
            writer.EndArray();
            
        }

        public static PDFArray Parse(string value)
        {
            int end;
            return PDFParserHelper.ParseArray(value, 0, out end);
        }

        public static PDFArray Parse(string value, int offset, out int end)
        {
            return PDFParserHelper.ParseArray(value, offset, out end);
        }

        #region IObjectContainer Members

        void IPDFObjectContainer.Add(IPDFFileObject obj)
        {
            this.Add(obj);
        }

        #endregion
    }
}
