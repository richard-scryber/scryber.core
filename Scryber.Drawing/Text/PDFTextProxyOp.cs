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
    /// Marker within a text reader for characters that will be replaced before final rendering by this proxies owner.
    /// </summary>
    public class PDFTextProxyOp : PDFTextOp
    {

        /// <summary>
        /// Gets or sets the key that identifies this operation to the owner.
        /// </summary>
        public string Key
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the text value for this operation
        /// </summary>
        public string Text
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the component that this replacement operation relies on
        /// </summary>
        public IComponent Owner
        {
            get;
            private set;
        }

        /// <summary>
        /// Overrides the abstract implementation to return the OpType of Replacement.
        /// </summary>
        public override PDFTextOpType OpType
        {
            get { return PDFTextOpType.Proxy; }
        }


        /// <summary>
        /// Creates a new PDFTextReplacementOp
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="key"></param>
        /// <param name="text"></param>
        public PDFTextProxyOp(IComponent owner, string key, string text)
            : base()
        {
            this.Owner = owner;
            this.Key = key;
            this.Text = text;
        }

    }
}
