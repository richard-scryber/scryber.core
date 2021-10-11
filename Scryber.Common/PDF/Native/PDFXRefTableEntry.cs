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

namespace Scryber.PDF.Native
{

    /// <summary>
    /// Encapsulates a single entry in an XRefTable(Section)
    /// </summary>
    public class PDFXRefTableEntry
    {
        private bool _free;
        private int _index;
        private int _generation;
        private IPDFIndirectObject _ref;
        private long _offset;
        private PDFXRefTableEntry _nextfree;

        public bool Free
        {
            get { return (null != this._ref) ? this._ref.Deleted : _free; }
        }

        public int Index
        {
            get { return (null != this._ref) ? this._ref.Number : _index; }
        }

        public int Generation
        {
            get { return (null != this._ref) ? this._ref.Generation : _generation; }
        }

        public long Offset
        {
            get { return (null != this._ref) ? this._ref.Offset : _offset; }
            set
            {
                _offset = value;
                if (null != this.Reference)
                    this.Reference.Offset = value;
            }
        }

        public PDFXRefTableEntry NextFree
        {
            get { return _nextfree; }
            set { _nextfree = value; }
        }

        public IPDFIndirectObject Reference
        {
            get { return _ref; }
            internal set { _ref = value; }
        }

        public PDFXRefTableEntry(int index, int generation, bool free, IPDFIndirectObject reference)
        {
            this._free = free;
            this._generation = generation;
            this._index = index;
            this._ref = reference;
        }

        public void Delete()
        {
            this._free = true;
            if (null != _ref && _ref is PDFIndirectObject)
            {
                PDFIndirectObject obj = (PDFIndirectObject)_ref;
                obj.Deleted = true;
            }
        }

        const int XRefEntryLength = 20;
        const int XRefOffsetLength = 10;
        const int XRefGenLength = 5;
        const int XRefStateLength = 1;

        const string IsUsed = "n";
        const string IsFree = "f";

        internal static PDFXRefTableEntry Parse(int index, string value, int offset, out int end)
        {
            string byteOffset = value.Substring(offset, XRefOffsetLength);
            string genValue = value.Substring(offset + XRefOffsetLength + 1, XRefGenLength);
            string stateValue = value.Substring(offset + XRefOffsetLength + 1 + XRefGenLength + 1, XRefStateLength);

            int ibyteOffset;
            int igenValue;
            bool free;

            if (!int.TryParse(byteOffset, out ibyteOffset))
                throw new PDFNativeParserException(CommonErrors.XRefTableEntryMustBeInCorrectFormat);
            if (!int.TryParse(genValue, out igenValue))
                throw new PDFNativeParserException(CommonErrors.XRefTableEntryMustBeInCorrectFormat);

            if (stateValue == IsUsed)
                free = false;
            else if (stateValue == IsFree)
                free = true;
            else
                throw new PDFNativeParserException(CommonErrors.XRefTableEntryMustBeInCorrectFormat);
            PDFXRefTableEntry entry = new PDFXRefTableEntry(index, igenValue, free, null);
            entry.Offset = ibyteOffset;

            end = offset + XRefEntryLength;
            return entry;
        }
    }


}
