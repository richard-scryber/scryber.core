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
using Scryber.Styles;
using Scryber.Components;

namespace Scryber.Layout
{
    /// <summary>
    /// A layout item that can contain other layout items. LayoutPage and LayoutDocument are sub classes
    /// </summary>
    public abstract class PDFLayoutContainerItem : PDFLayoutItem
    {
        //
        // properties
        //


        #region public PDFStyle FullStyle { get; set; }

        /// <summary>
        /// The full style of this item
        /// </summary>
        public Style FullStyle { get; set; }

        #endregion

        #region public IPDFLayoutEngine Engine { get; private set; }

        /// <summary>
        /// Gets the layout engine that created this item
        /// </summary>
        public IPDFLayoutEngine Engine { get; private set; }

        #endregion

        //
        // .ctor(s)
        //

        #region protected PDFLayoutContainerItem(PDFLayoutItem parent, IPDFComponent owner, IPDFLayoutEngine engine, PDFStyle full)

        /// <summary>
        /// Creates a new PDFLayoutContainerItem with the specified parent, owner and style
        /// </summary>
        /// <param name="parent">The parent container of this item</param>
        /// <param name="owner">The component owner of this item</param>
        /// <param name="full">The style applied to this item based on it's container owner</param>
        protected PDFLayoutContainerItem(PDFLayoutItem parent, IPDFComponent owner, IPDFLayoutEngine engine, Style full)
            : base(parent, owner)
        {
            this.FullStyle = full;
            this.Engine = engine;
        }

        #endregion

        
        

    }
}
