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
    public class PDFXRefTableSection
    {
        private int _start;
        private int _generation;

        private List<PDFXRefTableEntry> _entries;

        public int Start
        {
            get { return _start; }
        }

        public int Generation
        {
            get { return this._generation; }
        }

        public int Count
        {
            get { return _entries.Count; }
        }

        public PDFXRefTableEntry this[int index]
        {
            get
            {
                return this._entries[index];
            }
        }

        public PDFXRefTableEntry this[PDFObjectRef oref]
        {
            get
            {
                PDFXRefTableEntry entry = this[oref.Number - this.Start];

                //check the generations match
                if (null != entry && entry.Generation != oref.Generation)
                    entry = null;

                return entry;
            }
        }


        public IEnumerable<PDFXRefTableEntry> Entries
        {
            get { return _entries.AsReadOnly(); }
        }

        public PDFXRefTableSection(int startIndex, int generation)
        {
            this._start = startIndex;
            this._generation = generation;
            this._entries = new List<PDFXRefTableEntry>();
        }

        public int Add(IPDFIndirectObject obj)
        {
            int index = this._start + _entries.Count;

            PDFXRefTableEntry entry = new PDFXRefTableEntry(index, obj.Generation, obj.Deleted, obj);
            this._entries.Add(entry);
            obj.Number = index;
            obj.Generation = this.Generation;
            obj.Offset = -1;

            return index;
        }

        public bool Contains(IPDFIndirectObject obj)
        {
            foreach (PDFXRefTableEntry entry in this._entries)
            {
                if (entry.Reference == obj)
                    return true;
            }
            return false;
        }

        public void Delete(IPDFIndirectObject obj)
        {
            int index = obj.Number - this.Start;
            this._entries[index].Delete();
        }

        internal static PDFXRefTableSection Parse(string value, int offset, out int end)
        {
            if (!char.IsDigit(value, offset))
                throw new PDFNativeParserException(CommonErrors.XRefTableSectionMustBe2Integers);

            StringBuilder buffer = new StringBuilder();
            int index;
            int count;

            do
            {
                buffer.Append(value[offset++]);
            }
            while (char.IsDigit(value, offset));

            index = int.Parse(buffer.ToString());
            buffer.Clear();

            offset++; // past the space

            do
            {
                buffer.Append(value[offset++]);
            }
            while (char.IsDigit(value, offset));

            count = int.Parse(buffer.ToString());
            buffer.Clear();

            while (char.IsDigit(value, offset) == false)
                offset++;

            PDFXRefTableSection section = new PDFXRefTableSection(index, 0);

            for (int i = 0; i < count; i++)
            {
                PDFXRefTableEntry entry = PDFXRefTableEntry.Parse(index + i, value, offset, out end);
                section._entries.Add(entry);
                offset = end;
            }

            end = offset;
            return section;
        }
    }
}
