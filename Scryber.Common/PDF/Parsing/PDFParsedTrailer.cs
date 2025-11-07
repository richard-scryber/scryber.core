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
    public class PDFParsedTrailer : PDFParsedObject
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


        internal PDFParsedTrailer(PDFDictionary trailerDict, PDFFile file)
            : base(null, trailerDict, file)
        {
            this._existingEntries = new PDFParsedObjectEntryCollection(trailerDict, null);
        }
    }
}
