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
using Scryber.PDF;
using Scryber.PDF.Native;

namespace Scryber.PDF.Parsing
{
    public abstract class PDFParsedObject
    {
        private PDFObjectRef _oref;
        private IPDFFileObject _content;
        private PDFFile _owner;

        /// <summary>
        /// Gets the contents of the object
        /// </summary>
        protected IPDFFileObject Contents
        {
            get { return _content; }
        }

        protected PDFFile Owner
        {
            get { return _owner; }
        }

        public PDFObjectRef ObjectReference
        {
            get { return _oref; }
        }

        protected PDFParsedObject(PDFObjectRef oref, IPDFFileObject content, PDFFile owner)
        {
            this._oref = oref;
            this._content = content;
            this._owner = owner;
        }
    }
}
