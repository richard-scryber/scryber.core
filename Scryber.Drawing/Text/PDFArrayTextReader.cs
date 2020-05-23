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

namespace Scryber.Text
{
    /// <summary>
    /// A PDFTextReader that simply reads from an array passed into the constructor
    /// </summary>
    public class PDFArrayTextReader : PDFTextReader
    {
        private int _index;
        private PDFTextOp[] _ops;

        public PDFArrayTextReader(PDFTextOp[] ops)
            : base()
        {
            if (null == ops)
                _ops = new PDFTextOp[] { };
            else
                _ops = ops;

            _index = -1;

        }

        public override bool Read()
        {
            _index++;
            return _index < _ops.Length;
        }

        public override PDFTextOp Value
        {
            get { return _ops[_index]; }
        }

        public override int Length
        {
            get { return _ops.Length; }
        }

        public override bool EOF
        {
            get { return _index >= _ops.Length; }
        }

        protected override void ResetTextMarkers()
        {
            _index = -1;
        }
    }
}
