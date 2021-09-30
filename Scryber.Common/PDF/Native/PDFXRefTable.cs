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
    /// Implements the data storage for writing the XRef table in a PDF file
    /// </summary>
    public class PDFXRefTable
    {
        private int _gen;
        private long _offset;
        private List<PDFXRefTableSection> _sections;
        private PDFXRefTable _prev;
        private bool _readOnly;

        /// <summary>
        /// Gets the current generation of the XRefTable
        /// </summary>
        public int Generation
        {
            get { return _gen; }
        }


        //Gets or Sets the postion of the XRefTable in the file
        public long Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }

        public PDFXRefTable Previous
        {
            get { return _prev; }
            set { _prev = value; }
        }

        public bool ReadOnly
        {
            get { return _readOnly; }
            set { _readOnly = value; }
        }

        public int ReferenceCount
        {
            get
            {
                int sum = 0;

                foreach (PDFXRefTableSection section in this._sections)
                {
                    sum += section.Count;
                }

                return sum;
            }
        }

        public int MaxReference
        {
            get
            {
                int max = 0;

                for (int i = this._sections.Count - 1; i >= 0; i--)
                {
                    PDFXRefTableSection section = this._sections[i];
                    max = Math.Max(section.Start + (section.Count - 1), max);
                }
                if (null != this.Previous)
                    max = Math.Max(this.Previous.MaxReference, max);

                return max;
            }
        }

        public PDFXRefTableEntry this[int number]
        {
            get
            {
                for (int i = this._sections.Count - 1; i >= 0; i--)
                {
                    PDFXRefTableSection section = this._sections[i];
                    if (section.Start <= number && section.Start + section.Count > number)
                    {
                        return section[number - section.Start];
                    }
                }
                if (null != this.Previous)
                    return this.Previous[number];
                else
                    return null;
            }
        }

        public PDFXRefTableEntry this[PDFObjectRef oref]
        {
            get
            {
                for (int i = this._sections.Count - 1; i >= 0; i--)
                {
                    PDFXRefTableSection section = this._sections[i];
                    if (section.Start <= oref.Number && section.Start + section.Count > oref.Number)
                    {
                        PDFXRefTableEntry entry = section[oref];
                        if (null != entry)
                            return entry;
                    }
                }
                if (null != this.Previous)
                    return this.Previous[oref];
                else
                    return null;
            }
        }

        public IEnumerable<PDFXRefTableSection> Sections
        {
            get { return this._sections.AsReadOnly(); }
        }

        public int SectionCount
        {
            get { return this._sections.Count; }
        }

        public PDFXRefTable(int generation)
            : this(generation, 0, null)
        {
        }

        public PDFXRefTable(int generation, int startindex, PDFXRefTable previous)
        {
            this._gen = generation;
            this._prev = previous;
            this._sections = new List<PDFXRefTableSection>();
            this._sections.Add(new PDFXRefTableSection(startindex, generation));
            if (startindex == 0)
                this._sections[0].Add(new EmptyRef(0, 65535, 0));
        }

        /// <summary>
        /// Adds an indirect object, and in turn setting its number and generation
        /// </summary>
        /// <param name="obj">The object to add</param>
        /// <returns>The added objects number</returns>
        public int Append(IIndirectObject obj)
        {
            if (this.ReadOnly)
                throw new InvalidOperationException("Read Only XRef Table");

            int index = this._sections.Count - 1;
            PDFXRefTableSection section = this._sections[index];

            return section.Add(obj);
        }

        public bool Contains(IIndirectObject obj)
        {
            for (int i = this._sections.Count - 1; i >= 0; i--)
            {
                PDFXRefTableSection section = this._sections[i];
                if (section.Start < obj.Number)
                {
                    return section.Contains(obj);
                }
            }
            if (null != this.Previous)
                return this.Previous.Contains(obj);
            else
                return false;
        }

        /// <summary>
        /// Removes the current IIndirectObject and fills the space with an empty item
        /// </summary>
        /// <param name="obj"></param>
        public void Delete(IIndirectObject obj)
        {
            if (this.ReadOnly)
                throw new InvalidOperationException("Read Only XRef Table");

            for (int i = this._sections.Count - 1; i >= 0; i--)
            {
                PDFXRefTableSection section = this._sections[i];
                if (section.Start < obj.Number)
                {
                    section.Delete(obj);
                    break;
                }
            }
        }

        public void StartNewSection(int index)
        {
            if (this.ReadOnly)
                throw new InvalidOperationException("Read Only XRef Table");

            PDFXRefTableSection section = new PDFXRefTableSection(index, this.Generation);
            this._sections.Add(section);
        }

        #region private class EmptyRef : IIndirectObject

        /// <summary>
        /// An empty ref is a placeholder for a cell in the table that used to contain a reference, but it was removed
        /// </summary>
        private class EmptyRef : IIndirectObject
        {

            public EmptyRef(int num, int gen, long offset)
            {
                this._num = num;
                this._gen = gen;
                this._off = offset;
            }

            #region IIndirectObject Members

            private int _num;
            public int Number
            {
                get
                {
                    return this._num;
                }
                set
                {

                }
            }

            private int _gen;
            public int Generation
            {
                get
                {
                    return this._gen;
                }
                set
                {

                }
            }

            private long _off;
            public long Offset
            {
                get
                {
                    return _off;
                }
                set
                {

                }
            }

            public PDFStream ObjectData
            {
                get { return null; }
            }

            public void WriteData(PDFWriter writer)
            {
                writer.WriteCommentLine("This cell is empty : {0} {1} R", this.Number, this.Generation);
            }

            public bool HasStream { get { return false; } }

            public PDFStream Stream { get { return null; } }

            public bool Deleted
            {
                get { return true; }
            }

            private bool _written = false;

            public bool Written
            {
                get { return _written; }
                set { _written = value; }
            }

            public byte[] GetObjectData() { return new byte[] { }; }

            public byte[] GetStreamData() { return new byte[] { }; }

            #endregion

            public void Dispose() { }

            public override string ToString()
            {
                return "[NULL]";
            }
        }

        #endregion


        public static PDFXRefTable Parse(string value)
        {
            int end;
            return Parse(value, 0, out end);
        }

        public static PDFXRefTable Parse(string value, int offset, out int end)
        {
            if (value.Substring(offset, 4) != "xref")
                throw new PDFNativeParserException(CommonErrors.XRefTableDoesNotStartWithXRef);
            offset += 4;

            PDFXRefTable table = new PDFXRefTable(0);

            while (char.IsWhiteSpace(value, offset))
                offset++;

            //Force at least one subsection with at least one entry.
            if (char.IsDigit(value, offset) == false)
                throw new PDFNativeParserException(CommonErrors.XRefTableSectionMustBe2Integers);

            PDFXRefTableSection section = PDFXRefTableSection.Parse(value, offset, out end);
            if (section.Count == 0)
                throw new PDFNativeParserException(CommonErrors.XRefTableSectionMustBe2Integers);

            offset = end;

            table._sections.Clear();
            table._sections.Add(section);

            while (offset < value.Length)
            {
                if (char.IsDigit(value, offset))
                {
                    section = PDFXRefTableSection.Parse(value, offset, out end);
                    table._sections.Add(section);
                    offset = end;
                }
                else
                    break;
            }
            return table;


        }
    }
}
