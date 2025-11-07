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
using Scryber.PDF;
using Scryber.PDF.Native;

namespace Scryber.PDF.Parsing
{
    /// <summary>
    /// Defines an existing entry in an object in a PDFFile
    /// </summary>
    public class PDFParsedObjectEntry
    {
        private IPDFFileObject _origData;
        private bool _modified;
        private string _name;

        /// <summary>
        /// The file data associated with the entry
        /// </summary>
        public IPDFFileObject OriginalData
        {
            get { return _origData; }
        }

        /// <summary>
        /// Returns true if the existing entry has been modified in the modified file (rendering the object invalid).
        /// Use the MarkAsModified() method to change this value.
        /// </summary>
        public bool HasBeenModified
        {
            get { return _modified; }
        }

        /// <summary>
        /// Gets the name of the object - cannot be null or empty
        /// </summary>
        public string EntryName
        {
            get { return _name; }
        }

        /// <summary>
        /// Creates a new existing entry value.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="orig"></param>
        public PDFParsedObjectEntry(string name, IPDFFileObject orig)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            this._name = name;
            this._origData = orig;
            this._modified = false;
        }

        public void MarkAsModified()
        {
            this._modified = true;
        }
    }


    /// <summary>
    /// A collection of existing entries that can be accessed by index or name
    /// </summary>
    public class PDFParsedObjectEntryCollection : System.Collections.ObjectModel.KeyedCollection<string, PDFParsedObjectEntry>
    {

        public PDFParsedObjectEntryCollection()
            : this(null, null)
        {
        }

        private static string[] TypeEntry = new string[] { "Type" };
        private static string[] EmptyEntries = new string[] {};

        /// <summary>
        /// Creates a new existing entry collection and populates with the contents of the dictionary.
        /// If ignoreTpye is true, then the Type dictionary entry is not included in this populated collection
        /// </summary>
        /// <param name="entries"></param>
        /// <param name="ignoreType"></param>
        public PDFParsedObjectEntryCollection(PDFDictionary entries, bool ignoreType)
            : this(entries, ignoreType? TypeEntry : null)
        {

        }

        /// <summary>
        /// Creates a new existing entry collection and populates with the contents of the dictionary, except any entries that are maked by key in the excludedEntries array
        /// </summary>
        /// <param name="entries"></param>
        public PDFParsedObjectEntryCollection(PDFDictionary entries, string[] excludeEntries)
        {
            if (null != entries)
            {
                if (null == excludeEntries)
                    excludeEntries = EmptyEntries;

                this.FillEntries(entries, excludeEntries);
            }
        }

        protected override string GetKeyForItem(PDFParsedObjectEntry item)
        {
            return item.EntryName;
        }

        public void FillEntries(PDFDictionary existing, string[] excluded)
        {
            foreach (PDFName name in existing.Keys)
            {
                if (!excluded.Contains(name.Value))
                {
                    PDFParsedObjectEntry entry = new PDFParsedObjectEntry(name.Value, existing[name]);
                    this.Add(entry);
                }
            }
        }

        public void MarkAsModified(string name)
        {
            PDFParsedObjectEntry entry;
            if (this.TryGetEntry(name, out entry) && !entry.HasBeenModified)
                entry.MarkAsModified();
        }

        public bool TryGetEntry(PDFName name, out PDFParsedObjectEntry entry)
        {
            if (null == name)
                throw new ArgumentNullException("name");
            return this.TryGetEntry(name.Value, out entry);
        }

        public bool TryGetEntry(string name, out PDFParsedObjectEntry entry)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");
            if (this.Count == 0)
            {
                entry = null;
                return false;
            }
            else
                return this.Dictionary.TryGetValue(name, out entry);
        }
    }
}
