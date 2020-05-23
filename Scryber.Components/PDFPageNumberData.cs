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

namespace Scryber
{
    /// <summary>
    /// Encapsulates the number and label values of a single page 
    /// including number groups and total pages
    /// </summary>
    public class PDFPageNumberData
    {
        private PDFPageNumberGroup _group;

        public PDFPageNumberGroup GroupOptions
        {
            get { return _group; }
        }


        /// <summary>
        /// Gets the string label for this page with prefix and number type 
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets the last label in the current page group
        /// </summary>
        public string LastLabel { get; set; }

        /// <summary>
        /// Gets the ONE BASED nuber of this page in the WHOLE of the document
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Gets the ONE BASED number of the LAST page in the WHOLE document
        /// </summary>
        public int LastPageNumber { get; set; }

        /// <summary>
        /// Gets the ONE BASED number of this page in the current numbering group
        /// </summary>
        public int GroupNumber { get; set; }

        /// <summary>
        /// Gets the ONE BASED number of the LAST page in the current numbering group
        /// </summary>
        public int GroupLastNumber { get; set; }

        //
        // ctor
        //

        /// <summary>
        /// Creates and returns a new PDFPageNumber instance
        /// </summary>
        public PDFPageNumberData(PDFPageNumberGroup group)
        {
            this._group = group;
        }


        public override string ToString()
        {
            return string.IsNullOrEmpty(this.Label) ? "" : this.Label;
        }

        /// <summary>
        /// the format is used with the following fields...
        ///                     {0} = page label, {1} = total label for this pages numbering group, 
        ///                     {2} global page index, {3} global page count,
        ///                     {4} index in group, {5} group count
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public string ToString(string format)
        {
            if (null == format)
                throw new ArgumentNullException("format");

            return string.Format(format, this.Label, this.LastLabel, this.PageNumber, this.LastPageNumber, this.GroupNumber, this.GroupLastNumber);
        }
    }
}
