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

namespace Scryber.PDF.Resources
{
    public class PDFArrayFontWidths : PDFFontWidths
    {
        private List<int> _widths;

        #region protected List<int> Values

        /// <summary>
        /// protected accessor for the width values
        /// </summary>
        protected List<int> Values
        {
            get { return _widths; }
        }

        #endregion

        #region public int Count {get;}

        /// <summary>
        /// Gets the count of widths in this instance.
        /// </summary>
        public int Count
        {
            get { return this._widths.Count; }
        }

        #endregion

        #region public int this[int index] {get;set;}

        /// <summary>
        /// Gets or sets the width of a specific character (based on integer value)
        /// </summary>
        /// <param name="index">The character index to set</param>
        /// <returns></returns>
        /// <remarks>It is still an error to try and est a width beyond the extent of the current widths count.</remarks>
        public int this[int index]
        {
            get { return this._widths[index]; }
            set
            {
                this._widths[index] = value;
            }
        }

        #endregion

        #region public override bool IsEmpty {get;}

        /// <summary>
        /// Overrides base implmentation to return true is there are no widths in this array
        /// </summary>
        public override bool IsEmpty
        {
            get { return this.Count > 0; }
        }

        #endregion

        public PDFArrayFontWidths(int first, int last, IEnumerable<int> widths, Scryber.OpenType.SubTables.CMapEncoding encoding)
            : base(first, last, encoding)
        {
            this._widths = new List<int>();
            this.AddRange(widths);
        }


        //
        // implementation
        //

        public void RemoveAt(int index)
        {
            this._widths.RemoveAt(index);
        }

        public void Clear()
        {
            this._widths.Clear();
        }

        public void AddRange(IEnumerable<int> ws)
        {
            this._widths.AddRange(ws);
        }

        public void Add(int w)
        {
            this._widths.Add(w);
        }

        

        public override void RenderWidthsArrayToPDF(ContextBase context, PDFWriter writer)
        {
            writer.WriteArrayNumberEntries(this._widths.ToArray());

            if (context.ShouldLogDebug)
                context.TraceLog.Add(TraceLevel.Debug, "PDFFontWidths", "Rendered " + this._widths.Count.ToString() + " widths written as a simple array");
        }

        public override char RegisterGlyph(char c)
        {
            int offset = (int)c;
            if (offset < this.FirstChar || offset > this.LastChar)
            {
                return '?';
                throw new ArgumentException("Character is not in allowed character range");
            }
            
            return c;
        }
    }
}
