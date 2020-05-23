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
using Scryber.Drawing;

namespace Scryber.Text
{
    public class PDFTextDrawOp : PDFTextOp
    {

        public override PDFTextOpType OpType
        {
            get { return PDFTextOpType.TextContent; }
        }

        private string _chars;

        public string Characters
        {
            get { return _chars; }
            set { _chars = value; }
        }

        public PDFTextDrawOp(string chars)
            : base()
        {
            this._chars = chars;
        }

        public override string ToString()
        {
            return base.ToString() + ": '" + Characters + "'";
        }

        public override bool Equals(PDFTextOp other)
        {
            if (base.Equals(other))
            {
                return string.Equals(this.Characters, ((PDFTextDrawOp)other).Characters);
            }
            return false;
        }

    }

}
