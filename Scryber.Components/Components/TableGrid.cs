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
using Scryber.Native;
using Scryber.Styles;

namespace Scryber.Components
{
    [PDFParsableComponent("Table")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_table")]
    public class TableGrid : VisualComponent, IPDFViewPortComponent
    {
        //
        // public properties
        //

        #region public PDFTableRowList Rows {get;set;}

        private TableRowList _rows;

        /// <summary>
        /// Gets the collection of PDFTableRow(s). This will return a wrapped collection of TableRows 
        /// </summary>
        [PDFArray(typeof(TableRow))]
        [PDFElement("")]
        public TableRowList Rows
        {
            get
            {
                if (this._rows == null)
                    this._rows = new TableRowList(this.InnerContent);
                return this._rows;
            }
        }

        #endregion

        //
        // internal build properties
        //

        internal PDFStyle appliedStyle { get; set; }
        internal PDFStyle fullStyle { get; set; }
        internal PDFPositionOptions positionOptions { get; set; }
        internal int tblColumnCount { get; set; }
        internal int tblRowCount { get; set; }
        internal PDFUnit?[] tblColumnWidths { get; set; }

        //
        // ctor(s)
        //

        #region .ctor()

        /// <summary>
        /// Creates a new instance of the PDFTable
        /// </summary>
        public TableGrid()
            : this(PDFObjectTypes.Table)
        {
        }

        #endregion

        #region .ctor(PDFObjectType)

        protected TableGrid(PDFObjectType type)
            : base(type)
        {
        }

        #endregion

        //
        // methods
        //

        #region IPDFViewPortComponent Members

        /// <summary>
        /// Implements the IPDFViewPort to return the PDFTableLayoutEngine
        /// </summary>
        /// <param name="parent">The invoking layout engine</param>
        /// <param name="context">The current layout context</param>
        /// <param name="fullstyle">The full style of the table</param>
        /// <returns>The required engine</returns>
        IPDFLayoutEngine IPDFViewPortComponent.GetEngine(IPDFLayoutEngine parent, PDFLayoutContext context, PDFStyle fullstyle)
        {
            return new Layout.LayoutEngineTable(this, parent);
        }

        #endregion
    }
}
