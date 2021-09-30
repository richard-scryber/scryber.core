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
using Scryber.PDF.Native;

namespace Scryber.PDF
{
    /// <summary>
    /// A collection of object references to all the pages written in the PDFDocument
    /// </summary>
    public class PDFPageReferenceCollection
    {
        private Dictionary<int, PDFObjectRef> _pages = new Dictionary<int, PDFObjectRef>();
        private int _max = 0;

        internal void Add(int pgIndex, PDFObjectRef reference)
        {
            this._pages.Add(pgIndex, reference);
            _max = Math.Max(_max,pgIndex);
        }

        internal void Clear()
        {
            this._pages.Clear();
        }

        public int Count
        {
            get { return _max; }
        }

        public PDFObjectRef this[int pageIndex]
        {
            get
            {
                PDFObjectRef oref;
                if (!this._pages.TryGetValue(pageIndex, out oref))
                    oref = null;
                return oref;
            }
        }
    }
}
