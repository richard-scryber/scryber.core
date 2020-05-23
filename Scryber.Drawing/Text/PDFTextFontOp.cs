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
    public class PDFTextFontOp : PDFTextOp
    {

        public const string BoldMarker = "B";
        public const string ItalicMarker = "I";

        public override PDFTextOpType OpType
        {
            get 
            {
                if (_apply)
                    return PDFTextOpType.StyleStart;
                else
                    return PDFTextOpType.StyleEnd;
            }
        }

        private string _f;

        public string FontStyle
        {
            get { return _f; }
            set { _f = value; }
        }

        private bool _apply;

        public bool ApplyStyle
        {
            get { return this._apply; }
        }

        public PDFTextFontOp(string style, bool isapply)
            : base()
        {
            this._f = style;
            this._apply = isapply;
        }

        public override string ToString()
        {
            return base.ToString() + ":" + FontStyle;
        }

        public override bool Equals(PDFTextOp other)
        {
            if (base.Equals(other))
            {
                PDFTextFontOp op = other as PDFTextFontOp;
                if (String.Equals(op.FontStyle, this.FontStyle) && op.ApplyStyle == this.ApplyStyle)
                    return true;
            }
            return false;
        }
    }
}
