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
using Scryber.Styles;
using Scryber.Drawing;
using Scryber.PDF;
using Scryber.PDF.Layout;

namespace Scryber.Components
{
    [PDFParsableComponent("Cell")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_cell")]
    public class TableCell : VisualComponent, IPDFViewPortComponent
    {
        //
        // properties
        //

        #region public int ColumnSpan {get;set;}

        

        /// <summary>
        /// Gets or sets the column count for this cell.
        /// </summary>
        [PDFAttribute("column-span")]
        [PDFJSConvertor("scryber.studio.design.convertors.integer_attr", JSParams = "\"colspan\"")]
        [PDFDesignable("Column Span", Category = "General", Priority = 2,Type ="Number")]
        public virtual int CellColumnSpan
        {
            get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.TableCellColumnSpanKey, 1);
                else
                    return 1;
            }
            set
            {
                if (value <= 0)
                    this.Style.Table.RemoveCellColumnSpan();
                else
                    this.Style.Table.CellColumnSpan = value;
            }
        }

        #endregion

        #region public PDFTableRow ContainingRow

        /// <summary>
        /// Gets the row that contains this cell, or null.
        /// </summary>
        /// <remarks>We cannot directly return the parent as this cell 
        /// could be within a binding invisible container. So we must walk up the hierarchy 
        /// until we find a row (or not a row - in which case it's null).</remarks>
        public TableRow ContainingRow
        {
            get
            {
                Component Component = this.Parent;
                while (Component != null)
                {
                    if (Component is TableRow)
                        return Component as TableRow;
                    else if (Component is IInvisibleContainer)
                        Component = Component.Parent;
                    else
                        Component = null;
                }
                return null; //not found.
            }
        }

        #endregion 

        #region public PDFComponentList Contents {get;}

        /// <summary>
        /// Gets the contents of the cell
        /// </summary>
        [PDFArray(typeof(Component))]
        [PDFElement("")]
        public virtual ComponentList Contents
        {
            get { return this.InnerContent; }
        }

        #endregion

        //
        // internal build properties
        //

        internal Style appliedStyle { get; set; }
        internal Style fullStyle { get; set; }
        internal PDFPositionOptions positionOptions { get; set; }
        internal int columnIndex { get; set; }
        internal int columnSpan { get; set; }

        //
        // ctor(s)
        //

        #region .ctor() + .ctor(PDFObjectType)

        /// <summary>
        /// Creates a new instance of the PDFTableCell
        /// </summary>
        public TableCell()
            : this(ObjectTypes.TableCell)
        {
        }
        
        /// <summary>
        /// Protected constructor that sub classes 
        /// can use to create an instance of their class using a different ObjectType
        /// </summary>
        /// <param name="type">The type identifier</param>
        protected TableCell(ObjectType type)
            : base(type)
        {
        }

        #endregion
    
        //
        // overrides
        //

        #region protected override PDFStyle GetBaseStyle()

        /// <summary>
        /// Overrides the base implementation to add specific styles for the TableCell class.
        /// Specifically: FullWidth = true, so that each cell fills the row region it is assigned to.
        /// </summary>
        /// <returns>The populated style</returns>
        protected override Style GetBaseStyle()
        {
            Style style = base.GetBaseStyle();
            style.Columns.ColumnCount = 1;
            style.Overflow.Action = OverflowAction.Clip; //we don't split on a row or go over the page
            style.Padding.All = (Unit)4;
            style.Border.Color = new Color(153);
            style.Border.LineStyle = LineType.Solid;
            style.Border.Width = (Unit)1;
            //style.Position.FullWidth = true; //Cells are always the full width of their container row
            return style;
        }

        #endregion

        //
        // interface implementation
        //

        #region IPDFViewPortComponent Members
        /// <summary>
        /// Implements the layout engine so that it can return the default panel layout engine
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="context"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        public IPDFLayoutEngine GetEngine(IPDFLayoutEngine parent, PDFLayoutContext context, Style style)
        {
            return new PDF.Layout.LayoutEnginePanel(this, parent);
        }

        #endregion


    }

    [PDFParsableComponent("Header-Cell")]
    public class TableHeaderCell : TableCell
    {
        public TableHeaderCell()
            : this(ObjectTypes.TableHeaderCell)
        { }


        protected TableHeaderCell(ObjectType type) : base(type)
        { }
        
    }

    [PDFParsableComponent("Footer-Cell")]
    public class TableFooterCell : TableCell
    {
        public TableFooterCell()
            : this(ObjectTypes.TableFooterCell)
        { }

        protected TableFooterCell(ObjectType type) : base(type)
        {
        }

    }

    public class TableCellList : ComponentWrappingList<TableCell>
    {
        public TableCellList(ComponentList inner)
            : base(inner)
        {
        }

    }
}
