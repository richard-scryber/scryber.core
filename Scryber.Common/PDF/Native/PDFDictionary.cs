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
    /// A dictionary of PDFName Keys with PDFFileobject values. The dictionary is also a PDFFileObject
    /// </summary>
    public class PDFDictionary : IFileObject, IDictionary<PDFName,IFileObject>, ICloneable
    {
        public ObjectType Type { get { return PDFObjectTypes.Dictionary; } }
 
        /// <summary>
        /// Initializes a new Empty PDFDictionary
        /// </summary>
        public PDFDictionary()
        {
        }

        private Dictionary<PDFName, IFileObject> _dict = new Dictionary<PDFName, IFileObject>();
        
        /// <summary>
        /// Gets the inner dictionary of PDFName, PFFileObject pairs
        /// </summary>
        protected Dictionary<PDFName, IFileObject> InnerDictionary
        {
            get { return this._dict; }
        }

        /// <summary>
        /// Gets the inner collection of KeyValuePairs
        /// </summary>
        protected ICollection<KeyValuePair<PDFName, IFileObject>> InnerCollection
        {
            get { return (ICollection<KeyValuePair<PDFName, IFileObject>>)this.InnerDictionary; }
        }

        #region IDictionary<PDFName,PDFFileObject> Members

        /// <summary>
        /// Adds a new entry to the dictionary with the name and associated file object
        /// </summary>
        /// <param name="key">The PDFName to use a the key</param>
        /// <param name="value">The PDFFileObject to associate</param>
        public void Add(PDFName key, IFileObject value)
        {
            this.InnerDictionary.Add(key, value);
        }

        /// <summary>
        /// Returns true if this dictionary contains the specified name
        /// </summary>
        /// <param name="key">The name to find</param>
        /// <returns>True if found otherwise false</returns>
        public bool ContainsKey(PDFName key)
        {
            return this.InnerDictionary.ContainsKey(key);
        }

        /// <summary>
        /// A collection of all the keys (PDFName(s)) in the dictionary
        /// </summary>
        public ICollection<PDFName> Keys
        {
            get { return this.InnerDictionary.Keys; }
        }

        /// <summary>
        /// Removes the specified key and any associated FileObject from the dictionary
        /// </summary>
        /// <param name="key">The PDFName key to remove</param>
        /// <returns></returns>
        public bool Remove(PDFName key)
        {
            return this.InnerDictionary.Remove(key);
        }

        /// <summary>
        /// Tries to retrieve the associated data for a particular name, and if found returns true, otherwise false.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(string name, out IFileObject value)
        {
            return this.TryGetValue((PDFName)name, out value);
        }

        /// <summary>
        /// Tries to retrieve the associated data for a particular key, and if found returns true otherwise false
        /// </summary>
        /// <param name="key">The key to find</param>
        /// <param name="value">The found data associated with the key if any</param>
        /// <returns>true if found</returns>
        public bool TryGetValue(PDFName key, out IFileObject value)
        {
            return this.InnerDictionary.TryGetValue(key, out value);
        }

        /// <summary>
        /// A collection of all the values in the dictionary
        /// </summary>
        public ICollection<IFileObject> Values
        {
            get { return this.InnerDictionary.Values; }
        }

        /// <summary>
        /// Gets or sets the associated data specified by the key
        /// </summary>
        /// <param name="key">The PDFName key</param>
        /// <returns>The found PDFFileObject associated with the key</returns>
        public IFileObject this[PDFName key]
        {
            get
            {
                return this.InnerDictionary[key];
            }
            set
            {
                this.InnerDictionary[key] = value;
            }
        }

        #endregion

        public IFileObject this[string name]
        {
            get { return this[(PDFName)name]; }
            set { this[(PDFName)name] = value; }
        }

        #region ICollection<KeyValuePair<PDFName,PDFFileObject>> Members

        void ICollection<KeyValuePair<PDFName,IFileObject>>.Add(KeyValuePair<PDFName, IFileObject> item)
        {
            this.InnerCollection.Add(item);
        }

        /// <summary>
        /// Clears the current dictionary
        /// </summary>
        public void Clear()
        {
            this.InnerDictionary.Clear();
        }

        bool ICollection<KeyValuePair<PDFName,IFileObject>>.Contains(KeyValuePair<PDFName, IFileObject> item)
        {
            return this.InnerCollection.Contains(item);
        }

        void ICollection<KeyValuePair<PDFName,IFileObject>>.CopyTo(KeyValuePair<PDFName, IFileObject>[] array, int arrayIndex)
        {
            this.InnerCollection.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the total count of items in the dictionary
        /// </summary>
        public int Count
        {
            get { return this.InnerDictionary.Count; }
        }

        /// <summary>
        /// Identifies if this dictionary is read only
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        bool ICollection<KeyValuePair<PDFName,IFileObject>>.Remove(KeyValuePair<PDFName, IFileObject> item)
        {
            return this.InnerCollection.Remove(item);
        }

        #endregion

        #region IEnumerable<KeyValuePair<PDFName,PDFFileObject>> Members

        /// <summary>
        /// Returns the KeyValuePair enumerator to enumerate over each entry in the dictionary
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<PDFName, IFileObject>> GetEnumerator()
        {
            return this.InnerCollection.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region public void WriteData(PDFWriter writer)
        /// <summary>
        /// Writes the dictionary entries usign the PDFWriter
        /// </summary>
        /// <param name="writer">The PDFWriter to use</param>
        public void WriteData(PDFWriter writer)
        {
            writer.BeginDictionaryS();
            foreach (KeyValuePair<PDFName,IFileObject> kvp in this)
            {
                writer.BeginDictionaryEntry(kvp.Key);
                kvp.Value.WriteData(writer);
                writer.EndDictionaryEntry();
            }
            writer.EndDictionary();
        }

        #endregion

        #region public virtual PDFDictionary Clone()

        public virtual PDFDictionary Clone()
        {
            PDFDictionary theClone = (PDFDictionary)this.MemberwiseClone();
            theClone.Clear();
            foreach (KeyValuePair<PDFName,IFileObject> item in this)
            {
                theClone.Add(item.Key, item.Value);
            }
            return theClone;
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion

        #region public static PDFDictionary Parse(string value) + 1 overload

        public static PDFDictionary Parse(string value)
        {
            int end;
            return PDFParserHelper.ParseDictionary(value, 0, out end);
        }

        public static PDFDictionary Parse(string value, int offset, out int end)
        {
            return PDFParserHelper.ParseDictionary(value, offset, out end);
        }

        #endregion

    }


}
