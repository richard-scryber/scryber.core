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

namespace Scryber.Components
{
    /// <summary>
    /// Supports the use of headers and footers - add the header contents to this, measure, and then add to the main content.
    /// </summary>
    [PDFParsableComponent("TopAndTail")]
    internal class TopAndTailPanel : Panel, ITopAndTailedComponent
    {
        
        #region  public IPDFTemplate Header {get;set;}

        private ITemplate _header;

        /// <summary>
        /// Gets or sets the template for the header of this Page
        /// </summary>
        [PDFTemplate()]
        [PDFElement("Header")]
        public ITemplate Header
        {
            get { return _header; }
            set { _header = value; }
        }

        #endregion

        #region public IPDFTemplate Footer {get;set;}

        private ITemplate _footer;

        /// <summary>
        /// Gets or sets the template for the footer of this Page
        /// </summary>
        [PDFTemplate()]
        [PDFElement("Footer")]
        public ITemplate Footer
        {
            get { return _footer; }
            set { _footer = value; }
        }

        #endregion

        public TopAndTailPanel()
            : base(ObjectTypes.TopAndTail)
        {
        }
    }
}
