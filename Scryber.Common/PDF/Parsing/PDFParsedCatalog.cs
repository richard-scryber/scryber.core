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
using Scryber.PDF.Native;

namespace Scryber.PDF.Parsing
{
    /// <summary>
    /// Represents the required catalog entry in a parsed file
    /// </summary>
    public class PDFParsedCatalog : PDFParsedObject
    {


        #region public PDFParsedObjectEntryCollection Entries

        private PDFParsedObjectEntryCollection _existingEntries;

        /// <summary>
        /// Gets a list of all the existing catalog entries in the original document
        /// </summary>
        public PDFParsedObjectEntryCollection Entries
        {
            get { return _existingEntries; }
        }

        #endregion


        internal PDFParsedCatalog(PDFObjectRef catalogId, PDFDictionary catalogDict, PDFFile file)
            : base(catalogId, catalogDict, file)
        {
            bool ignoreType = true;
            this._existingEntries = new PDFParsedObjectEntryCollection(catalogDict, ignoreType);
        }

        public PDFObjectRef GetPageTree()
        {
            PDFParsedObjectEntry entry;

            if (!this.Entries.TryGetEntry(Const.PagesName, out entry))
                throw new NullReferenceException("The referenced file does not contain a Pages dictionary entry in the catalog");
            else if (!(entry.OriginalData is PDFObjectRef))
                throw new PDFDocumentStructureException("The Pages entry in the catalog must be an indirect object reference.");
            else
                return entry.OriginalData as PDFObjectRef;
        }
    }
}
