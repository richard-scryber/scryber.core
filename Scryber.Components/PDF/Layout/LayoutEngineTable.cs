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
using System.Diagnostics;
using System.Linq;
using System.Text;
using Scryber.Drawing;
using Scryber.Styles;
using Scryber.Components;

namespace Scryber.PDF.Layout
{
    public class LayoutEngineTable : LayoutEngineBase
    {
        private const string TableEngineLogCategory = "Table Layout Engine 3";

        private TableGrid _tbl;
        private CellDimension[] _widths;
        private CellDimension[] _heights;
        private Style[] _rowfullstyles, _rowappliedstyles;
        private Style[,] _cellfullstyles, _cellappliedstyles;
        private TableReference _tblRef;
        private PDFLayoutBlock _rowblock;
        private int _rowIndex = -1;

        Unit _rowOffset = Unit.Zero;

        protected TableGrid Table
        {
            get { return _tbl; }
        }


        private TableReference AllCells
        {
            get { return _tblRef; }
        }
        
        protected bool IsInNotSplittingBlock { get; set; }

        public LayoutEngineTable(TableGrid table, IPDFLayoutEngine parent)
            : base(table, parent)
        {
            this._tbl = table;
        }

        protected override void DoLayoutComponent()
        {
            
            try
            {
                if (this.Context.ShouldLogDebug)
                    this.Context.TraceLog.Begin(TraceLevel.Verbose, TableEngineLogCategory, "Beginning layout of '" + this.Table.UniqueID + "' as a table component");

                this.ContinueLayout = true;
                this.CurrentBlock = this.Context.DocumentLayout.CurrentPage.LastOpenBlock();
                if (this.CurrentBlock.CurrentRegion != null && this.CurrentBlock.CurrentRegion.HasOpenItem)
                    this.CurrentBlock.CurrentRegion.CloseCurrentItem();

                this.IsInNotSplittingBlock = false;
                
                var parents = this.CurrentBlock;
                while (parents != null)
                {
                    if (parents.Position.OverflowSplit == OverflowSplit.Never)
                    {
                        this.IsInNotSplittingBlock = true;
                        break;
                    }

                    parents = parents.GetParentBlock();
                }

                PDFPositionOptions tablepos = this.FullStyle.CreatePostionOptions(this.Context.PositionDepth > 0);

                //fix for width - if we have an explicit width, then we should fill it.
                if (tablepos.Width.HasValue)
                    tablepos.FillWidth = true;

                int rowcount, columncount;
                this.BuildStyles(out rowcount, out columncount);

                if (this.Context.ShouldLogDebug)
                    this.Context.TraceLog.Add(TraceLevel.Debug, TableEngineLogCategory, "Styles Built with " + rowcount + " rows, and " + columncount + " columns");

                this.Context.PerformanceMonitor.Begin(PerformanceMonitorType.Table_Build_Process);

                PDFLayoutBlock tableBlock = this.CurrentBlock.BeginNewContainerBlock(this.Table, this, this.FullStyle, tablepos.DisplayMode);

                Rect available = this.CalculateTableSpace(tableBlock, tablepos);

                if (this.Context.ShouldLogDebug)
                    this.Context.TraceLog.Add(TraceLevel.Debug, TableEngineLogCategory, "Calculated Table Space to " + available);

                available.X = 0;

                this.BuildReferenceGrid(rowcount, columncount, tablepos, available, tableBlock);

                if (this.Context.ShouldLogDebug)
                    this.Context.TraceLog.Add(TraceLevel.Debug, TableEngineLogCategory, "Built reference grid");

                this.CalcExplicitSizes(rowcount,columncount);
                if (this.Context.ShouldLogDebug)
                    this.Context.TraceLog.Add(TraceLevel.Debug, TableEngineLogCategory, "Calculated explicit sizes");

                this.CalculateUnassignedWidths();
                if (this.Context.ShouldLogDebug)
                    this.Context.TraceLog.Add(TraceLevel.Debug, TableEngineLogCategory, "Calculated Unassigned Widths");

                this.Context.PerformanceMonitor.End(PerformanceMonitorType.Table_Build_Process);

                this.DoLayoutTableRows();
                if (this.Context.ShouldLogDebug)
                    this.Context.TraceLog.Add(TraceLevel.Debug, TableEngineLogCategory, "Laid out the rows");

                // Adjust rowspan cells to their full height
                this.AdjustRowspanCellHeights();
                if (this.Context.ShouldLogDebug)
                    this.Context.TraceLog.Add(TraceLevel.Debug, TableEngineLogCategory, "Adjusted rowspan cell heights");

                //TODO: Reassess the required widths of columns in the entire table.

                this.PushConsistentCellWidths();
                this.PushRepeatingRowHeaderHeight();

                if (this.Context.ShouldLogDebug)
                    this.Context.TraceLog.Add(TraceLevel.Debug, TableEngineLogCategory, "Pushed Widths and repeating row heights");


                if (this.Context.ShouldLogDebug)
                    this.Context.TraceLog.End(TraceLevel.Verbose, TableEngineLogCategory, "Completed the layout of '" + this.Table.UniqueID + "' as a table component");
                else if (this.Context.ShouldLogVerbose)
                    this.Context.TraceLog.Add(TraceLevel.Verbose, TableEngineLogCategory, "Completed the layout of '" + this.Table.UniqueID + "' as a table component");

                this.Context.PerformanceMonitor.End(PerformanceMonitorType.Table_Build_Process);
            }
            catch (PDFLayoutException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new PDFLayoutException(string.Format(Errors.CouldNotLayoutGridCells, this.Table.ID, ex.Message), ex);
            }
        }

        //
        // actual table layout
        //

        #region private void DoLayoutTableRows()

        /// <summary>
        /// For each of the rows in this engines table - starts it's layout
        /// </summary>
        private void DoLayoutTableRows()
        {
            int index = 0;
            foreach (TableRow row in this.Table.Rows)
            {
                if (row.Visible == false)
                    continue;
                

                _rowIndex = index;
                this.DoLayoutTableRow(row, index, false);

                //check that we should keep going, or we have just run out of space
                if (!this.ContinueLayout)
                    return;

                index++;
            }
        }

        #endregion



        #region private PDFUnit DoLayoutTableRow(PDFTableRow row, int index)

        /// <summary>
        /// Lays out an individual table row
        /// </summary>
        /// <param name="row">The row to lay out</param>
        /// <param name="index">The index of the row in the table</param>
        /// <returns></returns>
        private Unit DoLayoutTableRow(TableRow row, int index, bool repeating)
        {
            if (this.Context.ShouldLogDebug)
                this.Context.TraceLog.Begin(TraceLevel.Debug, TableEngineLogCategory, "Laying out the table row with index " + index);

            PDFLayoutBlock tableblock = this.AllCells.CurrentGrid.TableBlock;
            
            //Check we can continue
            if (tableblock.IsClosed)
            {
                tableblock.ReOpen();
                //throw new InvalidOperationException("Table block already closed, has the cell overflowed");
            }

            RowReference rowRef = _tblRef.AllRows[index];

            Style rowStyle = rowRef.FullStyle;
            this.Context.StyleStack.Push(rowRef.AppliedStyle);


            if (row.Visible == false || rowStyle.GetValue(StyleKeys.PositionDisplayKey, DisplayMode.Block) == DisplayMode.Invisible)
                return Unit.Zero;

            if (repeating) //for repeating rows we hold it in the grid
            {
                rowRef = rowRef.Clone();
                this.AllCells.CurrentGrid.HeaderRows.Add(rowRef);
            }
                           

            


            PDFArtefactRegistrationSet artefacts = null;
            if (!repeating)
                artefacts = row.RegisterLayoutArtefacts(this.Context, rowStyle);


            //create the row block and region and hold a reference in the ivar _rowblock

            PDFPositionOptions rowpos = rowStyle.CreatePostionOptions(this.Context.PositionDepth > 0);

            this._rowblock = tableblock.BeginNewContainerBlock(row, this, rowStyle, rowpos.DisplayMode);

            rowRef.Block = this._rowblock;

            //Set the height to the page height and then we can move it after if it's too big for the height, unless it's explicit.

            Unit pageHeight = this.Context.DocumentLayout.CurrentPage.Height;
            Unit h = pageHeight;// block.AvailableBounds.Height;

            if (_heights[index].Explicit)
                h = _heights[index].Size;
            Unit w = tableblock.AvailableBounds.Width;
            Unit y = _rowOffset;

            Rect totalbounds = new Rect(Unit.Zero, y, w, h);

            PDFColumnOptions rowOpts = new PDFColumnOptions() { AlleyWidth = Unit.Zero, AutoFlow = false, ColumnCount = this.AllCells.TotalColumnCount };
            this._rowblock.InitRegions(totalbounds, rowpos, rowOpts, this.Context);



            //Layout the inner cells
            Unit rowHeight = DoLayoutRowCells(row, index, repeating);

            //Refresh the reference to the current block (just in case)
            tableblock = this.AllCells.CurrentGrid.TableBlock;
            tableblock.CurrentRegion.CloseCurrentItem();

            //Have we extended beyond the boundaries available
            if (rowHeight > this.AllCells.CurrentGrid.AvailableSpace.Height - _rowOffset 
                && this.AllCells.CurrentGrid.Position.OverflowAction != OverflowAction.Clip)
            {
                if (repeating)
                    throw new ArgumentOutOfRangeException("repeating", "Cannot fit all repeating rows within the available height");

                PDFLayoutBlock origparent = this.CurrentBlock;
                PDFLayoutRegion origregion = origparent.CurrentRegion;
                PDFLayoutBlock origtable = tableblock;
                PDFLayoutBlock origRow = this._rowblock;

                //If we are the first row, or as a table we should never be split
                if (!this.CanSplitTable())
                {
                    if (this.MoveFullTableToNextRegion())
                    {
                        _rowOffset += rowHeight;
                        Rect avail = this.AllCells.CurrentGrid.TableBlock.AvailableBounds;
                        //avail.Height = avail.Height - _rowOffset;
                        this.AllCells.CurrentGrid.AvailableSpace = avail;

                    }
                    else
                    {
                        this.AllCells.SetRowCount(index, true);
                        (origRow.Parent as PDFLayoutBlock).CurrentRegion.RemoveItem(origRow);
                        this.Context.TraceLog.Add(TraceLevel.Warning, LOG_CATEGORY, "Table '" + this.Table.UniqueID + "' has filled the available space, and cannot overflow onto a new region. Layout has stopped at row index " + index);
                        this.ContinueLayout = false;
                    }
                }
                else if (this.StartNewTableInAnotherRegion(origRow, index))
                {
                    //origregion.AddToSize(origtable);
                    Style origRowStlye = this.StyleStack.Pop();
                    _rowOffset = 0;
                    Unit repeath = this.DoLayoutRepeatingRows(index);

                    if(IsRowSpannedFromPreviousRow(index))
                    {
                        // Handle rowspan groups when rows overflow
                        // If a row with rowspan > 1 overflows, ensure all affected rows move together
                        var oldRegion = origtable.CurrentRegion;
                        var newRegion = AllCells.CurrentGrid.TableBlock.CurrentRegion;
                        int startRowspanGroup = GetRowspanGroupStart(index);
                        Unit movedHeight = this.MovePreviousRows(startRowspanGroup, index, oldRegion, newRegion, repeath);
                        _rowOffset += movedHeight; // Adjust the offset for the moved rows
                        repeath += movedHeight; // Add the moved height to the repeating rows offset, so the last row that was moved to the new region is in the correct place.
                    }

                    _rowOffset += origRow.Height;
                    origRow.Offset(0, repeath);
                    
                    this.StyleStack.Push(origRowStlye);
                }
                else
                {
                    this.AllCells.SetRowCount(index, true);
                    (origRow.Parent as PDFLayoutBlock).CurrentRegion.RemoveItem(origRow);
                    this.Context.TraceLog.Add(TraceLevel.Warning, LOG_CATEGORY, "Table '" + this.Table.UniqueID + "' has filled the available space, and cannot overflow onto a new region. Layout has stopped at row index " + index);
                    this.ContinueLayout = false;
                }
            }
            else
            {
                //block.CurrentRegion.AddToSize(this._rowblock);
                _rowOffset += rowHeight;
            }


            this._rowblock = null;

            //complete the row layout with registration, closing the artefacts and poping the style stack

            if (this.ContinueLayout)
                RegisterChildLayout(row);

            if (null != artefacts)
                row.CloseLayoutArtefacts(this.Context, artefacts, rowStyle);


            this.Context.StyleStack.Pop();

            if (this.Context.ShouldLogDebug)
                this.Context.TraceLog.End(TraceLevel.Debug, TableEngineLogCategory, "Completed Layout of the table row with index " + index);

            return rowHeight;
        }

        #endregion


        /// <summary>
        /// Determines if the specified row is part of a rowspan that was initiated
        /// in a previous row. For example, if row 0 has a cell with rowspan=3,
        /// then rows 1 and 2 return true for this method.
        /// </summary>
        /// <param name="rowindex">The row index to check</param>
        /// <returns>True if this row is spanned by cells from previous rows</returns>
        private bool IsRowSpannedFromPreviousRow(int rowindex)
        {
            if (rowindex <= 0)
                return false;

            // Check if any column in this row is occupied by a rowspan from a previous row
            for (int colIndex = 0; colIndex < this.AllCells.TotalColumnCount; colIndex++)
            {
                CellReference currentCell = this.AllCells.AllCells[rowindex, colIndex];
                if (currentCell != null && currentCell.IsEmpty)
                {
                    // This cell position is empty, which means it's spanned by a cell from a previous row
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets the starting row index of the rowspan group that includes the specified row.
        /// If the row is part of a rowspan initiated in a previous row, returns that previous row's index.
        /// Otherwise returns the current row index.
        /// </summary>
        /// <param name="rowindex">The row index to check</param>
        /// <returns>The starting row index of the rowspan group</returns>
        private int GetRowspanGroupStart(int rowindex)
        {
            int groupStart = rowindex;
            
            // Look backwards to find if this row is part of a rowspan from a previous row
            for (int prevRowIndex = rowindex - 1; prevRowIndex >= 0; prevRowIndex--)
            {
                bool hasRowspanFromThisRow = false;
                RowReference prevRow = this.AllCells.AllRows[prevRowIndex];
                
                for (int colIndex = 0; colIndex < this.AllCells.TotalColumnCount; colIndex++)
                {
                    CellReference cell = prevRow[colIndex];
                    if (cell != null && !cell.IsEmpty && cell.RowSpan > 1)
                    {
                        int spanEnd = cell.RowIndex + cell.RowSpan - 1;
                        if (spanEnd >= rowindex)
                        {
                            // This row is spanned by a cell from prevRowIndex
                            hasRowspanFromThisRow = true;
                            groupStart = prevRowIndex;
                            break;
                        }
                    }
                }
                
                if (!hasRowspanFromThisRow)
                    break; // No rowspan from this previous row affects our target row
            }
            
            return groupStart;
        }

        /// <summary>
        /// Gets the ending row index of the rowspan group that includes the specified row.
        /// If the row contains cells with rowspan > 1, returns the end of that span.
        /// </summary>
        /// <param name="rowindex">The row index to check</param>
        /// <returns>The ending row index of the rowspan group (inclusive)</returns>
        private int GetRowspanGroupEnd(int rowindex)
        {
            int groupEnd = rowindex;
            RowReference currentRow = this.AllCells.AllRows[rowindex];
            
            // Check all cells in this row to find how far their rowspans extend
            for (int colIndex = 0; colIndex < this.AllCells.TotalColumnCount; colIndex++)
            {
                CellReference cell = currentRow[colIndex];
                if (cell != null && !cell.IsEmpty && cell.RowSpan > 1)
                {
                    int spanEnd = cell.RowIndex + cell.RowSpan - 1;
                    if (spanEnd > groupEnd)
                        groupEnd = spanEnd;
                }
            }
            
            return groupEnd;
        }

        private Unit MovePreviousRows(int startRowIndex, int endRowIndex, PDFLayoutRegion oldRegion, PDFLayoutRegion newParent, Unit headOffset)
        {
            Unit offset = 0;
            for (int i = startRowIndex; i < endRowIndex; i++)
            {
                RowReference rowRef = this.AllCells.AllRows[i];
                if (rowRef.Block != null)
                {
                    // Move this row to the new region
                    oldRegion.RemoveItem(rowRef.Block, updateSize: true);
                    newParent.AddExistingItem(rowRef.Block);

                    rowRef.Block.Offset(0, offset + headOffset);
                    offset += rowRef.Block.Height;
                    //rowRef.OwnerGrid = this.AllCells.CurrentGrid; // Update the owner grid reference for the row
                }
            }
            
            return offset;
        }

        /// <summary>
        /// When a row with rowspan cells overflows to a new region,
        /// this method ensures all rows affected by that rowspan move together.
        /// For example, if row 0 has a cell with rowspan=3, and row 0 overflows,
        /// then rows 1 and 2 are also moved to maintain the rowspan group.
        /// </summary>
        /// <param name="rowindex">The index of the row that overflowed</param>
        /// <param name="pageHeaderHeight">Height of repeating header rows on the new page</param>
        private void LayoutAdditionalSpannedRows(int rowindex, Unit pageHeaderHeight)
        {
            // Find the maximum rowspan extent from the cells in this row
            RowReference currentRow = this._tblRef.AllRows[rowindex];
            int maxSpanEnd = rowindex; // At minimum, the current row
            
            // Check all cells in this row to find how far their rowspans extend
            for (int colIndex = 0; colIndex < this.AllCells.TotalColumnCount; colIndex++)
            {
                CellReference cref = currentRow[colIndex];
                if (cref != null && !cref.IsEmpty && cref.RowSpan > 1)
                {
                    // This cell spans down, so check where it ends
                    int spanEnd = cref.RowIndex + cref.RowSpan - 1;
                    if (spanEnd > maxSpanEnd)
                        maxSpanEnd = spanEnd;
                }
            }
            
            // If there are additional rows affected by the rowspan, layout them too
            if (maxSpanEnd > rowindex)
            {
                if (this.Context.ShouldLogDebug)
                    this.Context.TraceLog.Add(TraceLevel.Debug, TableEngineLogCategory, 
                        $"Row {rowindex} has rowspan cells extending to row {maxSpanEnd}. Laying out additional spanned rows.");
                
                // Layout each additional row affected by the rowspan
                Unit additionalOffset = Unit.Zero;
                for (int spanRowIndex = rowindex + 1; spanRowIndex <= maxSpanEnd && spanRowIndex < this.Table.Rows.Count; spanRowIndex++)
                {
                    if (this.ContinueLayout)
                    {
                        TableRow spanRow = this.Table.Rows[spanRowIndex];
                        if (spanRow != null && spanRow.Visible)
                        {
                            Unit spanRowHeight = this.DoLayoutTableRow(spanRow, spanRowIndex, false);
                            additionalOffset += spanRowHeight;
                        }
                    }
                }
                
                if (this.Context.ShouldLogDebug)
                    this.Context.TraceLog.Add(TraceLevel.Debug, TableEngineLogCategory,
                        $"Completed layout of {maxSpanEnd - rowindex} additional spanned rows with total height {additionalOffset}");
            }
        }

        #region private PDFUnit DoLayoutRepeatingRows(ref int rowindex)

        private Unit DoLayoutRepeatingRows(int rowindex)
        {
            Unit height = 0;
            if (this.AllCells.HasRepeats)
            {
                if(this.Context.ShouldLogDebug)
                    this.Context.TraceLog.Add(TraceLevel.Debug, TableEngineLogCategory, "Repeating rows at the top of a new table grid");

                foreach (RowReference rref in this.AllCells.RepeatRows)
                {
                    if (rref.RowIndex < rowindex)
                    {
                        Unit rowHeight = this.DoLayoutTableRow(rref.Row, rref.RowIndex, true);
                        height += rowHeight;
                    }
                }
            }
            return height;
        }

        #endregion

        #region private void DoLayoutRowCells(PDFTableRow row, int rowindex)

        /// <summary>
        /// Lays out the cells in a single row
        /// </summary>
        /// <param name="row">The cells to layout</param>
        /// <param name="rowindex">The index of the row we are laying out</param>
        /// <returns>The maximum height of the row</returns>
        private Unit DoLayoutRowCells(TableRow row, int rowindex, bool repeating)
        {
            Unit offsetX = Unit.Zero;
            int cellcount = _tblRef.AllCells.GetLength(1);
            int cellsprocessed = 0;
            int cellindex = 0;

            while (cellsprocessed < cellcount && cellindex < cellcount)
            {
                _lastRowWasMovedToANewTable = false;
                CellReference cref = _tblRef.AllCells[rowindex, cellindex];
                PDFLayoutRegion cellRegion = this._rowblock.CurrentRegion;

                Unit w = Unit.Zero;
                for (int i = 0; i < cref.ColumnSpan; i++)
                {
                    w += _widths[cellindex + i].Size;
                }
                //set the total bounds for the region, and make the height the 
                Rect total = new Rect(offsetX, cellRegion.TotalBounds.Y, w, cellRegion.TotalBounds.Height);
                cellRegion.TotalBounds = total;
                this._rowblock.CurrentRegion.SetMaxWidth(_widths[cellindex].Size);

                if (cref.IsEmpty == false && cref.Cell.Visible && cref.FullStyle.GetValue(StyleKeys.PositionDisplayKey, DisplayMode.Block) != DisplayMode.Invisible)
                {
                    this.DoLayoutARowCell(cref, cref.Cell, cellindex, rowindex, repeating);
                }
                if (_lastRowWasMovedToANewTable) //to remove as not using.
                {
                    GridReference gref = AllCells.CurrentGrid;
                    RowReference currrow = gref[0];
                    PDFLayoutBlock rowBlock = gref.TableBlock.LastOpenBlock();
                    currrow.Block = rowBlock;
                    this._rowblock = rowBlock;
                }

                offsetX += cellRegion.TotalBounds.Width;

                bool forced = true;

                //move on to the next region beyond the column span
                for (int i = 0; i < cref.ColumnSpan; i++)
                {
                    this._rowblock.MoveToNextRegion(forced, Unit.Zero, this.Context);
                }

                cellindex += cref.ColumnSpan;
                cellsprocessed++;
            }

            Unit maxh = this._tblRef.GetMaxCellHeightForRow(rowindex);
            this._tblRef.SetCellHeightForRow(rowindex, maxh);

            return maxh;
        }

        #endregion

        #region private void DoLayoutARowCell(PDFTableCell cell, int cellindex, int rowindex)

        /// <summary>
        /// Sets up the cell for laying out, with the styles and artefacts before calling 
        /// DoLayoutAcell - that lays out the content of the cell.
        /// </summary>
        /// <param name="cell">The cell to layout the content for</param>
        /// <param name="colindex">The column index of the cell in the row</param>
        /// <param name="rowindex">the index of the row the cell is on.</param>
        private void DoLayoutARowCell(CellReference cref, TableCell cell, int colindex, int rowindex, bool repeating)
        {
            

            Scryber.Styles.Style applied = cref.AppliedStyle;
            if (null != applied)
                this.StyleStack.Push(applied);

            Scryber.Styles.Style full = cref.FullStyle;

            PDFArtefactRegistrationSet artefacts = cell.RegisterLayoutArtefacts(this.Context, full);

            //Call the base implementation to actually layout the cell.
            this.DoLayoutACell(cref, cell, full, colindex, rowindex, repeating);

            if (null != applied)
                this.StyleStack.Pop();

            //We want to add the child only if it should be rendered and we are a full block
            if (this.ContinueLayout)
                RegisterChildLayout(cell);

            if (null != artefacts)
                cell.CloseLayoutArtefacts(this.Context, artefacts, full);


        }

        #endregion

        #region private void DoLayoutACell(PDFTableCell cell, Styles.PDFStyle full, int colindex, int rowindex)

        /// <summary>
        /// Performs the actual content layout of the cell
        /// </summary>
        /// <param name="cell">The cell to layout</param>
        /// <param name="full">The full style of the cell</param>
        /// <param name="colindex">The index of the column the cell is in</param>
        /// <param name="rowindex">The index of the row the cell is in</param>
        private void DoLayoutACell(CellReference cref, TableCell cell, Styles.Style full, int colindex, int rowindex, bool repeating)
        {
            try
            {
                //Apply the any actual or calculated widths to the cell - the height or width can
                //be applied to other cells in the same row or columm
                if (full.Immutable == false)
                {
                    if (cref.HasExplicitWidth)
                        full.Size.Width = cref.TotalWidth;
                    if (cref.HasExplicitHeight)
                        full.Size.Height = cref.TotalHeight;
                }
                using (IPDFLayoutEngine engine = cell.GetEngine(this, this.Context, full))
                {
                    engine.Layout(this.Context, full);
                }

                PDFLayoutRegion rowregion = this._rowblock.CurrentRegion;


                if (rowregion.Contents.Count > 0)
                {
                    PDFLayoutItem last = rowregion.Contents[rowregion.Contents.Count - 1];
                    if (last is PDFLayoutBlock)
                    {
                        if (repeating)
                        {
                            //TODO: Set a reference to the repeating cell on the current grid
                        }
                        else
                            cref.Block = (PDFLayoutBlock)last;

                    }
                    else if (this.Context.Conformance == ParserConformanceMode.Strict)
                        throw new PDFLayoutException("The last item in the region was not a block. A block should be the last item as we have just laid out a cell");
                    else
                        this.Context.TraceLog.Add(TraceLevel.Failure, TableEngineLogCategory, "The last item in the region was not a block. A block should be the last item as we have just laid out a cell");
                }
            }
            catch (PDFLayoutException)
            {
                throw;
            }
            catch (Exception ex)
            {
                string msg = String.Format(Errors.LayoutFailedForComponent, cell.ID, ex.Message);
                throw new PDFLayoutException(msg, ex);
            }
        }


        #endregion

        #region private void BuildStyles(out int rowcount, out int columncount)

        /// <summary>
        /// Builds complete arrays of all the row and cell full styles in the grid, taking account of any spanned cells.
        /// Setsthe instance variables for the style arrays and returns the row and coulmn count as out (reference) parameters
        /// </summary>
        /// <param name="rowcount"></param>
        /// <param name="columncount"></param>
        private void BuildStyles(out int rowcount, out int columncount)
        {
            List<Style> rowfulls = new List<Style>();
            List<List<Style>> cellfulls = new List<List<Style>>();

            List<Style> rowapplieds = new List<Style>();
            List<List<Style>> cellapplieds = new List<List<Style>>();
            int maxcolcount = 0;
            List<int> rowspanRemaining = new List<int>();

            var tablePos = this.FullStyle.CreatePostionOptions(this.Context.PositionDepth > 0);
            var tableFont = this.FullStyle.CreateTextOptions();

            foreach (TableRow row in this.Table.Rows)
            {
                if (row.Visible == false)
                    continue;

                Style rowapplied = null;
                Style rowfull = null;

                if (!string.IsNullOrEmpty(row.DataStyleIdentifier) && this.Context.DocumentLayout.TryGetStyleWithIdentifier(row.DataStyleIdentifier, out rowapplied, out rowfull))
                {
                    this.StyleStack.Push(rowapplied);
                }
                else
                {
                    this.Context.PerformanceMonitor.Begin(PerformanceMonitorType.Style_Build);
                    //Populate the style stack with the applied row style and build it's full style.
                    rowapplied = row.GetAppliedStyle();

                    this.StyleStack.Push(rowapplied);

                    rowfull = this.BuildRowFullStyle(row, tablePos, tableFont);

                    if (!string.IsNullOrEmpty(row.DataStyleIdentifier))
                        this.Context.DocumentLayout.SetStyleWithIdentifier(row.DataStyleIdentifier, rowapplied, rowfull);

                    this.Context.PerformanceMonitor.End(PerformanceMonitorType.Style_Build);

                }

                //If we are set to invisible then ingnore eveything

                StyleValue<DisplayMode> found;
                if(rowfull.GetValue(StyleKeys.PositionDisplayKey, DisplayMode.Block) == DisplayMode.Invisible)
                {
                    this.StyleStack.Pop();
                    row.Visible = false;
                    continue;
                }

                rowapplieds.Add(rowapplied);
                rowfulls.Add(rowfull);

                var rowPos = rowfull.CreatePostionOptions(this.Context.PositionDepth > 0);
                var rowFont = rowfull.CreateTextOptions();

                //For each of the cells in the row
                List<Style> rowcellstyles = new List<Style>();
                cellfulls.Add(rowcellstyles);

                List<Style> rowcellapplieds = new List<Style>();
                cellapplieds.Add(rowcellapplieds);

                int columnIndex = 0;
                foreach (TableCell cell in row.Cells)
                {
                    if (cell.Visible == false)
                        continue;

                    // Skip columns occupied by a rowspan from previous rows
                    while (columnIndex < rowspanRemaining.Count && rowspanRemaining[columnIndex] > 0)
                    {
                        rowcellstyles.Add(null);
                        rowcellapplieds.Add(null);
                        rowspanRemaining[columnIndex]--;
                        columnIndex++;
                    }

                    Style cellfull = null;
                    Style cellapplied = null;

                    if (!string.IsNullOrEmpty(cell.DataStyleIdentifier) && Context.DocumentLayout.TryGetStyleWithIdentifier(cell.DataStyleIdentifier, out cellapplied, out cellfull))
                    {
                        this.StyleStack.Push(cellapplied);
                    }
                    else
                    {
                        this.Context.PerformanceMonitor.Begin(PerformanceMonitorType.Style_Build);

                        cellapplied = cell.GetAppliedStyle();
                        this.StyleStack.Push(cellapplied);

                        cellfull = this.BuildCellFullStyle(cell, row, rowfull, tablePos, rowPos, rowFont);

                        if (!string.IsNullOrEmpty(cell.DataStyleIdentifier))
                            Context.DocumentLayout.SetStyleWithIdentifier(cell.DataStyleIdentifier, cellapplied, cellfull);

                        this.Context.PerformanceMonitor.End(PerformanceMonitorType.Style_Build);
                    }

                    //If we are set to invisible then ingnore eveything

                    if (cellfull.TryGetValue(StyleKeys.PositionDisplayKey, out found)
                        && found.Value(cellfull) == DisplayMode.Invisible)
                    {
                        this.StyleStack.Pop();
                        cell.Visible = false;
                        continue;
                    }

                    rowcellapplieds.Add(cellapplied);
                    rowcellstyles.Add(cellfull);

                    //If this cell spans more than 1 column then add null to the row cell styles for each spanned column

                    StyleValue<int> spanVal;
                    int span;
                    if (cellfull.TryGetValue(StyleKeys.TableCellColumnSpanKey, out spanVal))
                        span = spanVal.Value(cellfull);
                    else
                        //Deleted the allow changes to set the cellColumnSpan value on the style
                        span = 1;

                    if (span == 0)
                        throw new ArgumentOutOfRangeException("span","Cell column count cannot be less than 1. ");

                    // Get the rowspan for this cell (default 1)
                    StyleValue<int> rowspanVal;
                    int rowspan = 1;
                    if (cellfull.TryGetValue(StyleKeys.TableCellRowSpanKey, out rowspanVal))
                        rowspan = rowspanVal.Value(cellfull);

                    if (rowspan < 1)
                        rowspan = 1;

                    // Ensure rowspan tracking list has enough columns
                    for (int i = rowspanRemaining.Count; i < columnIndex + span; i++)
                        rowspanRemaining.Add(0);

                    // Track rowspan occupancy for subsequent rows
                    if (rowspan > 1)
                    {
                        for (int i = 0; i < span; i++)
                        {
                            int idx = columnIndex + i;
                            rowspanRemaining[idx] = Math.Max(rowspanRemaining[idx], rowspan - 1);
                        }
                    }

                    int remainingSpan = span;
                    while (remainingSpan > 1)
                    {
                        rowcellstyles.Add(null);
                        rowcellapplieds.Add(null);
                        remainingSpan--;
                    }

                    columnIndex += span;

                    this.StyleStack.Pop();
                }

                // Fill remaining rowspan-occupied columns at end of row
                while (columnIndex < rowspanRemaining.Count)
                {
                    if (rowspanRemaining[columnIndex] > 0)
                    {
                        rowcellstyles.Add(null);
                        rowcellapplieds.Add(null);
                        rowspanRemaining[columnIndex]--;
                    }
                    columnIndex++;
                }

                if (rowcellstyles.Count > maxcolcount)
                    maxcolcount = rowcellstyles.Count;

                this.StyleStack.Pop();
            }

            rowcount = rowfulls.Count;
            columncount = maxcolcount;

            this._rowfullstyles = rowfulls.ToArray();
            this._rowappliedstyles = rowapplieds.ToArray();

            this._cellfullstyles = new Style[rowfulls.Count, maxcolcount];
            this._cellappliedstyles = new Style[rowfulls.Count, maxcolcount];

            for (int r = 0; r < rowcount; r++)
            {
                List<Style> rsf = cellfulls[r];
                List<Style> rsa = cellapplieds[r];

                for (int c = 0; c < maxcolcount; c++)
                {
                    if (c < rsf.Count)
                    {
                        this._cellfullstyles[r, c] = rsf[c];
                        this._cellappliedstyles[r, c] = rsa[c];
                    }
                }
            }

            if (this.Context.ShouldLogDebug)
                this.Context.TraceLog.Add(TraceLevel.Debug, TableEngineLogCategory, string.Format("Table grid calculated at {0} rows by {1} columns", rowcount, maxcolcount));
        }

        #endregion


        #region private void AdjustRowspanCellHeights()

        /// <summary>
        /// After all rows have been laid out, adjusts the height of cells with rowspan > 1
        /// to span the full height of all rows they cover.
        /// </summary>
        private void AdjustRowspanCellHeights()
        {
            int rowcount = this._tblRef.AllRows.Length;
            int colcount = this._tblRef.TotalColumnCount;

            for (int r = 0; r < rowcount; r++)
            {
                for (int c = 0; c < colcount; c++)
                {
                    CellReference cref = this._tblRef.AllCells[r, c];

                    // Only process actual cells, not null placeholders
                    if (cref != null && cref.Block != null && !cref.IsEmpty)
                    {
                        // Check if this cell has rowspan > 1
                        int rowspan = cref.RowSpan;
                        if (rowspan > 1)
                        {
                            // Calculate the cumulative height of all spanned rows
                            Unit totalHeight = Unit.Zero;
                            for (int spanRow = r; spanRow < r + rowspan && spanRow < rowcount; spanRow++)
                            {
                                // Get the height of this row
                                Unit rowHeight = this._tblRef.GetMaxCellHeightForRow(spanRow);
                                totalHeight += rowHeight;
                            }

                            // Set the cell's block height to span all rows
                            if (totalHeight > Unit.Zero && cref.Block != null)
                            {
                                Rect bounds = cref.Block.TotalBounds;
                                bounds.Height = totalHeight;
                                cref.Block.TotalBounds = bounds;
                            }
                        }
                    }
                }
            }
        }

        #endregion


        protected Size GetSize(Unit? width, Unit containerWidth, Unit? height, Unit containerHeight)
        {
            var w = width.HasValue ? width.Value : containerWidth;
            var h = height.HasValue ? height.Value : containerHeight;
            return new Size(w, h);
        }

        protected virtual Style BuildRowFullStyle(TableRow row, PDFPositionOptions tablePosition, PDFTextRenderOptions tablefont)
        {
            var page = this.DocumentLayout.CurrentPage;
            
            Size pageSize = page.Size;

            Size fontSize = new Size(tablefont.GetZeroCharWidth(), tablefont.GetSize());

            Unit root = Font.DefaultFontSize;

            return this.Context.StyleStack.GetFullStyle(row, pageSize, new ParentComponentSizer(this.GetTableContainerSize), fontSize, root);
        }

        protected virtual Style BuildCellFullStyle(TableCell cell, TableRow inrow, Style rowStyle, PDFPositionOptions tablePosition, PDFPositionOptions rowPosition, PDFTextRenderOptions rowfont)
        {
            var page = this.DocumentLayout.CurrentPage;

            Size pageSize = page.Size;

            Size fontSize = new Size(rowfont.GetZeroCharWidth(), rowfont.GetSize());

            Unit root = Font.DefaultFontSize;

            return this.Context.StyleStack.GetFullStyle(cell, pageSize, new ParentComponentSizer(this.GetTableContainerSize), fontSize, root);
        }

        protected virtual Size GetTableContainerSize(IComponent forComponent, Style withStyle, PositionMode inMode)
        {
            var block = this.CurrentBlock;
            var tablePos = this.FullStyle.CreatePostionOptions(this.Context.PositionDepth > 0);

            var width = tablePos.Width;
            var height = tablePos.Height;

            
            var w = width.HasValue ? width.Value : block.AvailableBounds.Width;
            var h = height.HasValue ? height.Value : block.AvailableBounds.Height;

            return new Size(w, h);
        }

        #region private void CalculateTableSpace()

        /// <summary>
        /// Calculates the available space in the current block to 
        /// fit the table, and initializes it's region to this.
        /// </summary>
        private Rect CalculateTableSpace(PDFLayoutBlock tableblock, PDFPositionOptions tablepos)
        {
            Rect space = this.CurrentBlock.CurrentRegion.TotalBounds;
            space.Y = this.CurrentBlock.CurrentRegion.Height;
            space.X = 0;
            space.Height -= space.Y;


            if (tablepos.Width.HasValue)
                space.Width = tablepos.Width.Value;
            else if (tablepos.MaximumWidth.HasValue)
                space.Width = tablepos.MaximumWidth.Value + tablepos.Margins.Left + tablepos.Margins.Right;

            if (tablepos.Height.HasValue)
                space.Height = tablepos.Height.Value;
            else if (tablepos.MaximumHeight.HasValue)
                space.Height = tablepos.MaximumHeight.Value + tablepos.Margins.Top + tablepos.Margins.Bottom;

            if (tablepos.Margins.IsEmpty == false)
            {
                //We only account for the margins if we don't have an explicit height
                if (tablepos.Width.HasValue == false)
                    space.Width -= tablepos.Margins.Left + tablepos.Margins.Right;

                if (tablepos.Height.HasValue == false)
                    space.Height -= tablepos.Margins.Top + tablepos.Margins.Bottom;
            }

            PDFColumnOptions columnOptions = new PDFColumnOptions() { AlleyWidth = Unit.Zero, AutoFlow = false, ColumnCount = 1 };
            tableblock.InitRegions(space, tablepos, columnOptions, this.Context);

            if (tablepos.Padding.IsEmpty == false)
            {
                //Don't alter the position for the padding
                space.Width -= tablepos.Padding.Left + tablepos.Padding.Right;
                space.Height -= tablepos.Padding.Top + tablepos.Padding.Bottom;
            }

            if (this.Context.ShouldLogDebug)
                this.Context.TraceLog.Add(TraceLevel.Debug, TableEngineLogCategory, string.Format("Table grid available space is {0}", space.ToString()));


            return space;
        }

        #endregion

        #region protected virtual void PushConsistentSizes()

        /// <summary>
        /// Makes sure all the cells in the same column have the same (maximum) width across all rows.
        /// </summary>
        protected virtual void PushConsistentCellWidths()
        {
            if (this.Context.ShouldLogDebug)
                this.Context.TraceLog.Add(TraceLevel.Debug, TableEngineLogCategory, "Starting to push the component sizes");

            Unit[] finalwidths = new Unit[_widths.Length];

            //Set the width of the cells in each column
            Unit tableWidth = Unit.Zero;
            for (int colIndex = 0; colIndex < this.AllCells.TotalColumnCount; colIndex++)
            {
                Unit maxColWidth = _tblRef.GetMaxCellWidthForColumn(colIndex);
                if (_widths[colIndex].Explicit)
                    maxColWidth = _widths[colIndex].Size;
                else if (this.AllCells.CurrentGrid.Position.FillWidth)
                    maxColWidth = Unit.Max(maxColWidth, _widths[colIndex].Size);

                if (this.Context.ShouldLogDebug)
                    this.Context.TraceLog.Add(TraceLevel.Debug, TableEngineLogCategory, "Max column width for #" + colIndex + " is " + maxColWidth);
                finalwidths[colIndex] = maxColWidth;
            }

            for (int colIndex = 0; colIndex < this.AllCells.TotalColumnCount; colIndex++)
            {
                _tblRef.EnsureSufficientWidthForSpannedCells(colIndex, finalwidths);
            }

            tableWidth = Unit.Zero;
            for (int colIndex = 0; colIndex < finalwidths.Length; colIndex++)
            {
                tableWidth += finalwidths[colIndex];
            }

            
            for(int colIndex = 0; colIndex < this.AllCells.TotalColumnCount; colIndex++)
            {
                _tblRef.SetCellWidthForColumn(colIndex, finalwidths);
                _tblRef.SetRowOffsetAndWidthForColumn(colIndex, finalwidths);

            }

            if (this.Context.ShouldLogDebug)
                this.Context.TraceLog.Add(TraceLevel.Debug, TableEngineLogCategory, "All cells set to the maximum width of their column.");


            int index = 0;
            int last = this.AllCells.AllGrids.Count - 1;

            foreach (GridReference grid in this.AllCells.AllGrids)
            {
                if (grid.StartRowIndex < 0)
                    //this is a grid that has no rows - automatically flowed onto a new nape
                    continue;

                PDFLayoutBlock tableblock = grid.TableBlock;
                //Assumption: The table block contains only one region that contains all the row blocks
                PDFLayoutRegion tableregion = tableblock.Columns[0];


                if (grid.HasHeaderRows)
                {
                    for (int i = 0; i < grid.HeaderRows.Count; i++)
                    {
                        RowReference header = grid.HeaderRows[i];
                        Rect rowbounds = header.Block.TotalBounds;
                        rowbounds.Width = tableWidth;
                        header.Block.TotalBounds = rowbounds;
                        tableregion.AddToSize(header.Block);
                    }
                }
                for (int rowindex = grid.StartRowIndex; rowindex < grid.EndRowIndex + 1; rowindex++)
                {
                    RowReference rowref = this.AllCells.AllRows[rowindex];
                    PDFLayoutBlock row = rowref.Block;
                    if (null != row)
                    {
                        Rect rowbounds = row.TotalBounds;
                        rowbounds.Width = tableWidth; // +row.Position.Padding.Left + row.Position.Padding.Right + row.Position.Margins.Left + row.Position.Margins.Right;
                        row.TotalBounds = rowbounds;
                        tableregion.AddToSize(row);

                        if (this.Context.ShouldLogDebug)
                            this.Context.TraceLog.Add(TraceLevel.Debug, TableEngineLogCategory, "Added row index " + rowindex + " to the table grid region " + tableregion.ToString());

                        //for this row set the region size in each of the cells to the matching size of the block.
                        this.SetCellRegionSizesOnRow(rowindex, row);
                    }
                    else
                        this.Context.TraceLog.Add(TraceLevel.Warning, TableEngineLogCategory, "Table row at index " + rowindex + " in table " + this.Table.ID + " does not have a block associated with it, so cannot push consistent widths");

                }

                if (!tableblock.IsClosed)
                    tableblock.Close();
                tableblock.SetContentSize(tableWidth, tableregion.Height);

                //Should only add the size of the last grid to the current region.
                if (index == last)
                {
                    PDFLayoutBlock parent = tableblock.Parent as PDFLayoutBlock;
                    parent.CurrentRegion.AddToSize(tableblock);
                }

                index++;

            }
            if (this.Context.ShouldLogDebug)
                this.Context.TraceLog.Add(TraceLevel.Debug, TableEngineLogCategory, "All row sizes added to the final table height");
        }

        private void SetCellRegionSizesOnRow(int rowindex, PDFLayoutBlock rowblock)
        {
            for (int i = 0; i < this.AllCells.TotalColumnCount; i++)
            {
                CellReference cref = this.AllCells.AllCells[rowindex, i];
                if (null != cref && null != cref.Block)
                {
                    PDFLayoutBlock cellblock = cref.Block;
                    PDFLayoutRegion region = cellblock.Columns[0];
                    Rect bounds = region.TotalBounds;
                    PDFPositionOptions cellpos = cellblock.Position;

                    bounds.Width = cellblock.Width - (cellpos.Padding.Left + cellpos.Padding.Right + cellpos.Margins.Left + cellpos.Margins.Right);
                    bounds.Height = cellblock.Height - (cellpos.Padding.Top + cellpos.Padding.Bottom + cellpos.Margins.Top + cellpos.Margins.Bottom);

                    if (cellblock.ColumnOptions.ColumnCount == 1)
                    {
                        region.TotalBounds = bounds;
                    }
                    else if (cellblock.ColumnOptions.ColumnCount > 1)
                    {
                        //remove total alley width
                        bounds.Width -= (cellblock.ColumnOptions.AlleyWidth * (cellblock.ColumnOptions.ColumnCount - 1));
                        //divide by the column count
                        bounds.Width /= cellblock.ColumnOptions.ColumnCount;
                        for (int col = 0; col < cellblock.ColumnOptions.ColumnCount; col++)
                        {
                            region = cellblock.Columns[col];
                            region.TotalBounds = bounds;
                            //move next region bounds on by a column width and an alley
                            bounds.X += bounds.Width + cellblock.ColumnOptions.AlleyWidth;
                        }
                    }
                }
            }
        }

        #endregion

        #region public void PushRepeatingRowHeaderHeight()

        /// <summary>
        /// Makes sure all the overflow header row cells have the same height.
        /// </summary>
        public void PushRepeatingRowHeaderHeight()
        {
            if (this._tblRef.AllGrids.Count > 1)
            {
                foreach (GridReference gref in this._tblRef.AllGrids)
                {
                    if (gref.HasHeaderRows)
                    {
                        foreach (RowReference rref in gref.HeaderRows)
                        {
                            Unit h = rref.ExplicitHeight;
                            PDFLayoutBlock rowblock = rref.Block;

                            for (int col = 0; col < this._tblRef.TotalColumnCount; col++)
                            {
                                PDFLayoutRegion cellregion = rowblock.Columns[col];

                                //check there is content in the column - Fix for luis 06/01/2015
                                if (null != cellregion.Contents && cellregion.Contents.Count > 0)
                                {
                                    PDFLayoutBlock cell = cellregion.Contents[0] as PDFLayoutBlock;
                                    Rect total = cell.TotalBounds;
                                    total.Height = h;
                                    cell.TotalBounds = total;
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region private void BuildReferenceGrid()

        /// <summary>
        /// Builds a complete grid of references to all rows and cells within the table inc stlyes
        /// </summary>
        private void BuildReferenceGrid(int rowcount, int columncount, PDFPositionOptions tablepos, Rect availableSpace, PDFLayoutBlock tableblock)
        {
            TableReference tbl = new TableReference(rowcount, columncount, this.FullStyle, tablepos);
            GridReference grid = tbl.CurrentGrid;

            grid.AvailableSpace = availableSpace;
            grid.TableBlock = tableblock;
            int rowIndex = 0;

            foreach(TableRow row in this.Table.Rows)
            {

                if (row.Visible == false) //skip hidden rows
                    continue;

                Style applied = _rowappliedstyles[rowIndex];
                Style full = _rowfullstyles[rowIndex];


                RowReference rref = tbl.AddRowReference(row, grid, applied, full, rowIndex);

                this.BuildReferenceCells(tbl, rref, rowIndex, columncount);
                rowIndex++;
            }

            if (this.Context.ShouldLogDebug)
                this.Context.TraceLog.Add(TraceLevel.Debug, TableEngineLogCategory, "Table grid references generated, inc. all styles");

            this._tblRef = tbl;
        }

        private void BuildReferenceCells(TableReference tbl, RowReference rref, int rowIndex, int columncount)
        {
            int cellindex = 0; //the index of the cell in the table row (to take account of spans it is only increments on actual cells).

            for (int column = 0; column < columncount; column++)
            {
                Style applied = _cellappliedstyles[rowIndex, column];
                Style full = _cellfullstyles[rowIndex, column];

                //Check that we are not an empty or spanned cell
                if (null != full)
                {
                    if (cellindex >= rref.Row.Cells.Count)
                        throw new ArgumentNullException("No cell exists at column " + column.ToString() + " in row " + rowIndex);

                    TableCell cell = GetNextVisibleCell(rref.Row, ref cellindex);
                    if (null == cell)
                        return;

                    //If we have a table that is full width we need the cells to be full width too.
                    //watching the immutable options
                    if (tbl.CurrentGrid.Position.FillWidth)
                    {
                        bool appliedImmutable = applied.Immutable;
                        bool fullImmutable = full.Immutable;

                        applied.Immutable = false;
                        full.Immutable = false;

                        applied.Size.FullWidth = true;
                        full.Size.FullWidth = true;

                        applied.Immutable = appliedImmutable;
                        full.Immutable = fullImmutable;
                    }

                    tbl.AddCellReference(cell, rref, applied, full, column);

                    cellindex++;
                }
                else
                {
                    tbl.AddEmptyCellReference(rref, column);
                }
            }
        }

        private TableCell GetNextVisibleCell(TableRow row, ref int cellindex)
        {
            while (cellindex < row.Cells.Count)
            {
                TableCell cell = row.Cells[cellindex];
                if (cell.Visible)
                    return cell;

                cellindex++;
            }

            return null;
        }

        #endregion

        #region private void CalcExplicitSizes(int rowcount, int columncount)

        /// <summary>
        /// Calculates the explicit widths and heights of each of the rows and columns.
        /// Stored in the instance variables _columnWidths, _rowHeights
        /// </summary>
        private void CalcExplicitSizes(int rowcount, int columncount)
        {
            CellDimension[] widths = new CellDimension[columncount];
            CellDimension[] heights = new CellDimension[rowcount];
            List<CellReference> spanned = new List<CellReference>();

           

            for (int v = 0; v < rowcount; v++)
            { 
                for (int h = 0; h < columncount; h++)
                {
                    CellReference cref = _tblRef.AllCells[v, h];
                    if (null != cref && cref.IsEmpty == false)
                    {
                        if (cref.ColumnSpan > 1)
                        {
                            if(cref.HasExplicitWidth)
                                spanned.Add(cref);
                        }
                        else
                        {
                            if (cref.HasExplicitWidth)
                            {
                                widths[h].Size = Unit.Max(widths[h].Size, cref.TotalWidth);
                                widths[h].Explicit = true;
                            }
                        }

                        if (cref.HasExplicitHeight)
                        {
                            heights[v].Size = Unit.Max(heights[v].Size, cref.TotalHeight);
                            heights[v].Explicit = true;
                        }
                    }
                }

                RowReference rowref = _tblRef.AllRows[v];

                if (rowref.HasExplicitHeight)
                {
                    heights[v].Size = Unit.Max(heights[v].Size, rowref.ExplicitHeight);
                    heights[v].Explicit = true;
                }

                //We have an explicit height - so lets push this onto all the cells in the row.
                if (heights[v].Explicit)
                {
                    Unit maxh = heights[v].Size;
                    for (int h = 0; h < columncount; h++)
                    {
                        CellReference cref = _tblRef.AllCells[v, h];
                        cref.TotalHeight = maxh;
                    }
                }
            }

            // now take account of the spanned cells to see if we calculate the explicit widths across multiple columns.
            ApplySpannedWidths(widths, spanned);

            _widths = widths;
            _heights = heights;

            if (this.Context.ShouldLogDebug)
                this.Context.TraceLog.Add(TraceLevel.Debug, TableEngineLogCategory, "All explicit cell sizes accounted for");
        }

        /// <summary>
        /// Takes all the spanned cells and trys to infer an actual width
        /// </summary>
        /// <param name="widths"></param>
        /// <param name="spanned"></param>
        private void ApplySpannedWidths(CellDimension[] widths, List<CellReference> spanned)
        {
            int resolved = 0; //Number of cell reference widths that were resolved in each run. 
                              //When we hit zero after as run we can infer no more, so need to just split across.

            int currColSpanCount = 2; //The  col-span being actioned in the loop below.
            int spanIndex = 0; //The spanned cell being processed.

            do
            {
                resolved = 0;
                List<CellReference> remaining = new List<CellReference>();

                while (spanned.Count > 0)
                {
                    CellReference cref = spanned[spanIndex];
                    if (cref.ColumnSpan == currColSpanCount)
                    {
                        if (cref.HasExplicitWidth)
                        {
                            if (!AppyJustOneMissingWidth(cref, widths))
                            {
                                //Could not resolve the missing width
                                //Need to loop again before can resolve
                                remaining.Add(cref);
                            }
                            else
                                resolved++;
                        }
                        spanned.RemoveAt(spanIndex);
                    }
                    else
                    {
                        spanIndex++;

                        if (spanIndex >= spanned.Count)
                        {
                            //We have processed all with the current col-span count. Start again with the next col-span
                            spanIndex = 0;
                            currColSpanCount++;
                        }
                    }
                }
                spanned = remaining; //switch 
                currColSpanCount = 2; //reset the col span back to 2

            } while (resolved > 0); // we weren't able to infer any more explicit column widths

            if (spanned.Count > 0)
            {
                //We have some that span more than one indeterminate column.
                this.ApplyMoreThanOneMissingWidth(spanned, widths);
                
            }
        }

        private void ApplyMoreThanOneMissingWidth(List<CellReference> spanned, CellDimension[] widths)
        {
            CellDimension[] newwidths = new CellDimension[widths.Length];
            widths.CopyTo(newwidths, 0);

            foreach (CellReference cref in spanned)
            {
                Unit explicitTotal = Unit.Zero;
                int loosecount = 0;
                List<int> looseindexes = new List<int>();

                for (int i = 0; i < cref.ColumnSpan; i++)
                {
                    int col = cref.ColumnIndex + i;
                    if (widths[col].Explicit)
                    {
                        explicitTotal += widths[col].Size;
                    }
                    else
                    {
                        loosecount++;
                        looseindexes.Add(col);
                    }
                }

                if (loosecount > 0)
                {
                    Unit remainder = cref.TotalWidth - explicitTotal;
                    remainder = remainder / loosecount;
                    foreach (int col in looseindexes)
                    {
                        if (newwidths[col].Size < remainder || newwidths[col].Explicit == false)
                        {
                            newwidths[col].Size = remainder;
                            newwidths[col].Explicit = true;
                        }
                        else
                        {
                            loosecount--; //this column has already been asigned a higher vaule by another cell
                                          //so reduce the count, and recalculate the remainder.
                            if (loosecount > 0)
                            {
                                remainder = cref.TotalWidth - explicitTotal - newwidths[col].Size;
                                remainder = remainder / loosecount;
                            }
                            else //no more loose columns
                                break;
                        }
                    }
                }

                //TODO: There is a posibility that this expands the table too wide for more than one multispanning cell.
                //      Should really calculate the width again.

                
            }
            //copy the values back across
            newwidths.CopyTo(widths, 0);
        }

        private bool AppyJustOneMissingWidth(CellReference cref, CellDimension[] widths)
        {
            Unit explicitTotal = Unit.Zero;
            int loosecount = 0;
            int looseindex = -1;

            for (int i = 0; i < cref.ColumnSpan; i++)
            {
                int col = cref.ColumnIndex + i;
                if (widths[col].Explicit)
                {
                    explicitTotal += widths[col].Size;
                }
                else
                {
                    loosecount++;
                    looseindex = col;
                }
            }

            if (loosecount == 1)
            {
                Unit remainder = cref.TotalWidth - explicitTotal;
                widths[looseindex].Size = remainder;
                widths[looseindex].Explicit = true;
                return true;
            }
            else
                return false;
        }

        #endregion

        #region  private void CalculateUnassignedWidths()

        /// <summary>
        /// Based on the available space left after each explicit width. 
        /// This calculates the remaining available for each column and splits it up between them.  
        /// </summary>
        private void CalculateUnassignedWidths()
        {

            Unit tablewidth;
            Unit totalexplicit = Unit.Zero;

            tablewidth = this.AllCells.CurrentGrid.AvailableSpace.Width;

            int nonExplicitCount = 0;
            for (int i = 0; i < this.AllCells.TotalColumnCount; i++)
            {
                CellDimension w = _widths[i];
                if (w.Explicit == false)
                {
                    nonExplicitCount++;
                }
                else
                {
                    tablewidth -= w.Size;
                    totalexplicit += w.Size;
                }
            }

            if (this.Context.ShouldLogDebug)
                this.Context.TraceLog.Add(TraceLevel.Debug, TableEngineLogCategory, "Available width for non explicit sizes is " + tablewidth.ToString());

            //make sure we actually have some widths
            if (nonExplicitCount > 0)
            {
                Unit unassignedwidth = tablewidth / nonExplicitCount;
                for (int i = 0; i < _widths.Length; i++)
                {
                    if (_widths[i].Explicit == false)
                        _widths[i].Size = unassignedwidth;
                }

                if (this.Context.ShouldLogDebug)
                    this.Context.TraceLog.Add(TraceLevel.Debug, TableEngineLogCategory, "Avialable width split between " + nonExplicitCount + " columns, with each given " + unassignedwidth);
            }


        }

        #endregion

        //
        // overflow methods
        //

        protected virtual bool CanSplitTable()
        {
            if (this.AllCells.CurrentGrid.Position.OverflowSplit == OverflowSplit.Never)
                return false;
            else if (this.IsInNotSplittingBlock)
                return false;
            else
                return true;
        }

        #region protected virtual void CloseTableAndReleaseRowsFrom(int rowindex)

        /// <summary>
        /// Closes the tabe reference and removes all rows after and including the specified index
        /// </summary>
        /// <param name="rowindex"></param>
        protected virtual void CloseTableAndReleaseRowsFrom(int rowindex)
        {
            if (this.Context.ShouldLogVerbose)
                this.Context.TraceLog.Add(TraceLevel.Verbose, TableEngineLogCategory, "Closing table and releasing all rows after '" + rowindex + "'");

            this._tblRef.SetRowCount(rowindex, true);
        }

        #endregion

        #region protected virtual bool MoveFullTableToNextRegion()

        //Check to see if we have moved the full table to a new region previously
        //if we have, then it means we cannot fit the entire table in one block - so we should truncate.
        private bool _didmovefulltable = false;

        protected virtual bool MoveFullTableToNextRegion()
        {
            if (_didmovefulltable)
            {
                if (this.Context.ShouldLogVerbose)
                    this.Context.TraceLog.Add(TraceLevel.Verbose, TableEngineLogCategory, "Already moved table to the next region so cannot move table '" + this.Table.ID + "' to the next region");

                return false;
            }

            GridReference origGrid = this.AllCells.CurrentGrid;
            PDFLayoutBlock block = origGrid.TableBlock;
            PDFLayoutRegion region = origGrid.TableBlock.CurrentRegion;
            bool newPage;
            if (this.MoveToNextRegion(block.Height, ref region, ref block, out newPage))
            {
                if (this.Context.ShouldLogVerbose)
                    this.Context.TraceLog.Add(TraceLevel.Verbose, TableEngineLogCategory, "Moved entire table '" + this.Table.ID + "' to the next " + (newPage?"Page":"Region") + " and table block is now '" + block.ToString() + "'");

                //If we have a new block then we set the old one as invisible
                if (block != origGrid.TableBlock)
                {
                    origGrid.TableBlock.ExcludeFromOutput = true;
                }

                origGrid.TableBlock = block;
                _didmovefulltable = true;
                return true;
            }
            else
                return false;
        }

        #endregion

        #region protected virtual bool StartNewTableInAnotherRegion(PDFLayoutBlock row, int rowindex)

        /// <summary>
        /// Called when we have run out of available space in the current region to contine 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="rowindex"></param>
        /// <returns></returns>
        protected virtual bool StartNewTableInAnotherRegion(PDFLayoutBlock row, int rowindex)
        {
            GridReference origGrid = this.AllCells.CurrentGrid;

            origGrid.EndRowIndex = rowindex - 1;
            Size origGridSize = origGrid.CalculateContentSize();

            PDFLayoutBlock origTblblock = origGrid.TableBlock;
            if (origTblblock.IsClosed == false)
                origTblblock.Close();
            origTblblock.SetContentSize(origGridSize.Width, origGridSize.Height);


            PDFLayoutRegion origTblRegion = origTblblock.CurrentRegion;
            PDFLayoutRegion tblparent = (origTblblock.Parent as PDFLayoutBlock).CurrentRegion;
            tblparent.AddToSize(origTblblock);
            this._rowIndex = rowindex;
            PDFLayoutRegion region = row.CurrentRegion;
            bool newpage;

            bool started = this.MoveToNextRegion(row.Height, ref region, ref row, out newpage);
            if (started)
            {
                if (this.Context.ShouldLogVerbose)
                    this.Context.TraceLog.Add(TraceLevel.Verbose, TableEngineLogCategory, "Started table '" + this.Table.ID + "' in a new " + (newpage ? "Page" : "Region") + " and table row block is now '" + row.ToString() + "'");

                PDFLayoutBlock newTableBlock = this.AllCells.CurrentGrid.TableBlock;
                PDFLayoutRegion newTableRegion = newTableBlock.CurrentRegion;
                newTableRegion.UsedSize = Size.Empty;

                if (newpage) //need to go back to the top
                    row.Offset(0, _rowOffset);// _rowOffset - row.Height);

                if (origGrid.EndRowIndex < 0)
                {
                    origGrid.TableBlock.ExcludeFromOutput = true;
                }
            }
            return started;

        }

        #endregion

        #region public override PDFLayoutBlock CloseCurrentBlockAndStartNewInRegion(PDFLayoutBlock blockToClose, PDFLayoutRegion joinToRegion)


        bool _lastRowWasMovedToANewTable = false;

        /// <summary>
        /// Overrides the default behaviour to beging a new GridReference to hold the next rows.
        /// </summary>
        /// <param name="blockToClose"></param>
        /// <param name="joinToRegion"></param>
        /// <returns></returns>
        public override PDFLayoutBlock CloseCurrentBlockAndStartNewInRegion(PDFLayoutBlock blockToClose, PDFLayoutRegion joinToRegion)
        {

            PDFLayoutBlock orig = this.CurrentBlock;
            GridReference curr = this.AllCells.CurrentGrid;
            Rect avail = curr.AvailableSpace;
            avail.Height = joinToRegion.AvailableHeight;

            //TODO: Use this to caclulate the space
            PDFLayoutBlock newTable = base.CloseCurrentBlockAndStartNewInRegion(blockToClose, joinToRegion);

            this.CurrentBlock = (PDFLayoutBlock)newTable.Parent;

            GridReference nextgrid = this.AllCells.BeginNewGrid(curr.TableStyle, curr.Position, this._rowIndex);

            nextgrid.TableBlock = newTable;
            avail.Y = 0;

            

            if (nextgrid.Position.Margins.IsEmpty == false)
            {
                Thickness margins = nextgrid.Position.Margins;
                avail.Width -= margins.Left + margins.Right;
                avail.Height -= margins.Top + margins.Right;
                avail.X += margins.Left;
                avail.Y += margins.Top;

            }
            if (nextgrid.Position.Padding.IsEmpty == false)
            {
                Thickness padding = nextgrid.Position.Padding;
                avail.Width -= padding.Left + padding.Right;
                avail.Height -= padding.Top + padding.Bottom;
                avail.X += padding.Left;
                avail.Y += padding.Top;
            }
            nextgrid.AvailableSpace = avail;

            
            this._rowOffset = 0;

            //this._lastRowWasMovedToANewTable = true;

            return newTable;
        }

        #endregion

        //
        // inner classes
        //

        #region private class CellReference

        /// <summary>
        /// A reference to a single table cell and it's associated data in this table
        /// </summary>
        private class CellReference
        {
            #region ivars

            private TableCell _cell;
            private Style _fullStyle;
            private Style _appliedStyle;
            private int _rowindex;
            private int _colindex;
            private PDFLayoutBlock _block;
            private Unit _totalWidth;
            private Unit _totalHeight;
            private Thickness _margins;
            private RowReference _row;
            private bool _empty = false;

            #endregion

            #region public PDFTableCell Cell {get;}

            /// <summary>
            /// Gets the table cell associated with this reference
            /// </summary>
            public TableCell Cell
            {
                get { return _cell; }
            }

            #endregion

            #region public PDFStyle FullStyle {get;}

            /// <summary>
            /// Gets the full style of the cell
            /// </summary>
            public Style FullStyle
            {
                get { return _fullStyle; }
            }

            #endregion

            #region public PDFStyle AppliedStyle {get;}

            /// <summary>
            /// Gets the style that is Applied to the table cell
            /// </summary>
            public Style AppliedStyle
            {
                get { return _appliedStyle; }
            }

            #endregion

            #region public int RowIndex {get;}

            /// <summary>
            /// Gets the (global) row index of this cell
            /// </summary>
            public int RowIndex
            {
                get { return _rowindex; }
            }

            #endregion

            #region public int ColumnIndex {get;}

            /// <summary>
            /// Gets the column index of this cell
            /// </summary>
            public int ColumnIndex
            {
                get { return _colindex; }
            }

            #endregion

            #region public int ColumnSpan {get;set;}

            private int _colspan;

            /// <summary>
            /// Gets or sets the column span associated with this cell
            /// </summary>
            public int ColumnSpan
            {
                get { return _colspan; }
                set { _colspan = value; }
            }

            #endregion

            #region public int RowSpan {get;set;}

            private int _rowspan;

            /// <summary>
            /// Gets or sets the row span associated with this cell
            /// </summary>
            public int RowSpan
            {
                get { return _rowspan; }
                set { _rowspan = value; }
            }

            #endregion

            #region public PDFThickness Margins {get;}

            /// <summary>
            /// Gets any margins associated with this cell's style
            /// </summary>
            public Thickness Margins
            {
                get { return _margins; }
            }

            #endregion

            #region public bool IsEmpty {get;}

            /// <summary>
            /// Returns true if this is an empty or spanned cell
            /// </summary>
            public bool IsEmpty
            {
                get { return _empty; }
                set { _empty = value; }
            }

            #endregion

            #region public PDFUnit TotalWidth {get} + public bool HasExplicitWidth {get;}

            /// <summary>
            /// Gets the total explicit width specified for the cell
            /// </summary>
            public Unit TotalWidth
            {
                get { return _totalWidth; }
            }

            /// <summary>
            /// Returns true if there is an explicit width to this cell
            /// </summary>
            public bool HasExplicitWidth
            {
                get
                {
                    return _totalWidth != Unit.Zero;
                }
            }

            #endregion

            #region public PDFUnit TotalHeight {get;} + public bool HasExplicitHeight

            /// <summary>
            /// Gets the total explicit height specified for the cell
            /// </summary>
            public Unit TotalHeight
            {
                get { return _totalHeight; }
                set { _totalHeight = value; }
            }

            public bool HasExplicitHeight
            {
                get { return _totalHeight != Unit.Zero; }
            }

            #endregion

            #region public PDFLayoutBlock Block {get;set;}

            /// <summary>
            /// Gets or sets the associated block for this reference
            /// </summary>
            public PDFLayoutBlock Block
            {
                get { return _block; }
                set { _block = value; }
            }

            #endregion

            // .ctor

            #region public CellReference(PDFTableCell cell, RowReference row,  PDFStyle applied, PDFStyle fullstyle, int rowindex, int colindex)

            /// <summary>
            /// Creates a new cell reference
            /// </summary>
            /// <param name="cell"></param>
            /// <param name="row"></param>
            /// <param name="applied"></param>
            /// <param name="fullstyle"></param>
            /// <param name="rowindex"></param>
            /// <param name="colindex"></param>
            public CellReference(TableCell cell, RowReference row, Style applied, Style fullstyle, int rowindex, int colindex)
            {
                this._cell = cell;
                this._appliedStyle = applied;
                this._fullStyle = fullstyle;
                this._rowindex = rowindex;
                this._colindex = colindex;
                this._block = null;
                this._row = row;

                if(null != fullstyle)
                    PopulateStyleValues(fullstyle);
            }

            #endregion

            #region private void PopulateStyleValues(PDFStyle fullstyle)

            /// <summary>
            /// Populates all the style related
            /// ivars of this instance with the values from the full style
            /// </summary>
            /// <param name="fullstyle"></param>
            private void PopulateStyleValues(Style fullstyle)
            {
                PDFPositionOptions opts = fullstyle.CreatePostionOptions(false); //Full Styles cache this so should be quick anyway
                _margins = opts.Margins;
                if (opts.Width.HasValue)
                    _totalWidth = opts.Width.Value + _margins.Left + _margins.Right;
                else
                    _totalWidth = Unit.Zero;

                if (opts.Height.HasValue)
                    _totalHeight = opts.Height.Value + _margins.Top + _margins.Right;
                else
                    _totalHeight = Unit.Zero;

                StyleValue<int> cellspan;
                if (fullstyle.TryGetValue(StyleKeys.TableCellColumnSpanKey, out cellspan))
                    _colspan = cellspan.Value(fullstyle);
                else
                    _colspan = 1;

                StyleValue<int> rowspan;
                if (fullstyle.TryGetValue(StyleKeys.TableCellRowSpanKey, out rowspan))
                    _rowspan = rowspan.Value(fullstyle);
                else
                    _rowspan = 1;
            }

            #endregion
        }

        #endregion

        #region private class RowspanCellMarker

        /// <summary>
        /// Tracks a cell that spans multiple rows, storing metadata about the rowspan
        /// </summary>
        private class RowspanCellMarker
        {
            #region ivars

            private CellReference _cell;
            private int _startRowIndex;
            private int _endRowIndex; // inclusive
            private int _columnIndex;
            private int _columnSpan;

            #endregion

            #region public properties

            /// <summary>
            /// Gets the cell that spans multiple rows
            /// </summary>
            public CellReference Cell
            {
                get { return _cell; }
            }

            /// <summary>
            /// Gets the starting row index of this rowspan (first row it occupies)
            /// </summary>
            public int StartRowIndex
            {
                get { return _startRowIndex; }
            }

            /// <summary>
            /// Gets the ending row index of this rowspan (last row it occupies, inclusive)
            /// </summary>
            public int EndRowIndex
            {
                get { return _endRowIndex; }
                set { _endRowIndex = value; }
            }

            /// <summary>
            /// Gets the column index where this cell starts
            /// </summary>
            public int ColumnIndex
            {
                get { return _columnIndex; }
            }

            /// <summary>
            /// Gets the number of columns this cell spans
            /// </summary>
            public int ColumnSpan
            {
                get { return _columnSpan; }
            }

            #endregion

            #region public constructor

            /// <summary>
            /// Creates a new rowspan cell marker for tracking a cell that spans rows
            /// </summary>
            public RowspanCellMarker(CellReference cell, int startRowIndex, int columnIndex, int columnSpan)
            {
                this._cell = cell;
                this._startRowIndex = startRowIndex;
                this._endRowIndex = startRowIndex + cell.RowSpan - 1;
                this._columnIndex = columnIndex;
                this._columnSpan = columnSpan;
            }

            #endregion

            #region public methods

            /// <summary>
            /// Determines if the specified row and column position is occupied by this rowspan cell
            /// </summary>
            public bool OccupiesPosition(int rowIndex, int colIndex)
            {
                return rowIndex >= _startRowIndex && rowIndex <= _endRowIndex &&
                       colIndex >= _columnIndex && colIndex < _columnIndex + _columnSpan;
            }

            /// <summary>
            /// Determines if this rowspan marker's range overlaps with the given row/column range
            /// </summary>
            public bool OverlapsWith(int rowIndex, int columnIndex, int columnSpan)
            {
                return rowIndex <= _endRowIndex &&
                       columnIndex < _columnIndex + _columnSpan &&
                       columnIndex + columnSpan > _columnIndex;
            }

            #endregion
        }

        #endregion

        #region private class RowReference

        /// <summary>
        /// A reference to a single row in a table
        /// </summary>
        private class RowReference
        {
            #region ivars

            private TableRow _row;
            private Style _fullStyle;
            private Style _appliedStyle;
            private int _rowindex;
            private PDFLayoutBlock _block;
            private Unit _explicitHeight;
            private Thickness _margins;
            private GridReference _grid;

            #endregion

            // properties

            #region public PDFTableRow Row {get;}

            /// <summary>
            /// Gets the table row associated with this reference
            /// </summary>
            public TableRow Row
            {
                get { return _row; }
            }

            #endregion

            #region public PDFStyle FullStyle {get;}

            /// <summary>
            /// Gets the full style associated with the row
            /// </summary>
            public Style FullStyle
            {
                get { return _fullStyle; }
            }

            #endregion

            #region public int RowIndex {get;}

            /// <summary>
            /// Gets the absolute row index of this row in the whole table
            /// </summary>
            public int RowIndex
            {
                get { return _rowindex; }
            }

            #endregion

            #region public TableRowRepeat Repeat { get; set; }

            /// <summary>
            /// Gets or sets the type of repeat to indicate how this row should be repeated in each new region.
            /// </summary>
            public TableRowRepeat Repeat { get; set; }

            #endregion

            #region public PDFStyle AppliedStyle {get;}

            /// <summary>
            /// Gets the applied style for the row (rather than the full style)
            /// </summary>
            public Style AppliedStyle
            {
                get { return _appliedStyle; }
            }

            #endregion

            #region public PDFThickness Margins {get;}

            /// <summary>
            /// Gets the margins for this Row
            /// </summary>
            public Thickness Margins
            {
                get { return this._margins; }
            }

            #endregion

            #region public PDFUnit ExplicitHeight {get} + public bool HasExplicitHeight {get;}

            /// <summary>
            /// Gets the total explicit height specified for the cell
            /// </summary>
            public Unit ExplicitHeight
            {
                get { return _explicitHeight; }
                set { _explicitHeight = value; }
            }

            /// <summary>
            /// Returns true if this row has an explicit height
            /// </summary>
            public bool HasExplicitHeight
            {
                get { return _explicitHeight != Unit.Zero; }
            }

            #endregion

            #region public PDFLayoutBlock Block {get; set;}

            /// <summary>
            /// Gets or sets the associated block for this reference
            /// </summary>
            public PDFLayoutBlock Block
            {
                get { return _block; }
                set { _block = value; }
            }

            #endregion

            #region public GridReference OwnerGrid {get;}

            /// <summary>
            /// Gets the grid that contains this row reference
            /// </summary>
            public GridReference OwnerGrid
            {
                get { return _grid; }
            }

            #endregion


            #region public CellReference this[int columnIndex] {get;}

            /// <summary>
            /// Gets this rows cell reference at the specified column index
            /// </summary>
            /// <param name="columnIndex"></param>
            /// <returns></returns>
            public CellReference this[int columnIndex]
            {
                get { return this.OwnerGrid.OwnerTable.AllCells[this.RowIndex, columnIndex]; }
            }

            #endregion

            // .ctor

            #region public RowReference(PDFTableRow row, PDFStyle applied, PDFStyle fullstyle,  int rowindex)

            /// <summary>
            /// Creates a new instance of a reference to a table row
            /// </summary>
            /// <param name="row"></param>
            /// <param name="applied"></param>
            /// <param name="fullstyle"></param>
            /// <param name="posOpts"></param>
            /// <param name="rowindex"></param>
            public RowReference(TableRow row, GridReference grid, Style applied, Style fullstyle, int rowindex)
            {
                this._row = row;
                this._rowindex = rowindex;
                this._fullStyle = fullstyle;
                this._appliedStyle = applied;
                this._grid = grid;
                this.PopulateStyleValues(fullstyle);
            }

            #endregion

            // methods

            #region private void PopulateStyleValues(PDFStyle fullstyle)

            /// <summary>
            /// Fills the values of this row reference based on the required style values
            /// </summary>
            /// <param name="fullstyle"></param>
            private void PopulateStyleValues(Style fullstyle)
            {
                PDFPositionOptions opts = fullstyle.CreatePostionOptions(false);
                _margins = opts.Margins;

                if (opts.Height.HasValue)
                    _explicitHeight = opts.Height.Value + _margins.Top + _margins.Bottom;

                StyleValue<TableRowRepeat> repeat;
                if (fullstyle.TryGetValue(StyleKeys.TableRowRepeatKey,out repeat))
                    this.Repeat = repeat.Value(fullstyle);
                else
                    this.Repeat = TableRowRepeat.None;
            }

            #endregion

            #region public RowReference Clone()

            /// <summary>
            /// Creates a shallow clone of this row reference
            /// </summary>
            /// <returns></returns>
            public RowReference Clone()
            {
                return this.MemberwiseClone() as RowReference;
            }

            #endregion
        }

        #endregion

        #region private class GridReference

        /// <summary>
        /// A reference to a single continuous table grid in one region.
        /// </summary>
        private class GridReference
        {

            #region ivars

            private int _startRowIndex;
            private int _endRowIndex;
            private PDFLayoutBlock _tableBlock;
            private Style _tableStyle;
            private PDFPositionOptions _posOpts;
            private TableReference _ownerTable;
            private Rect _availspace;

            #endregion

            // properties

            #region public int StartRowIndex {get;set;}

            /// <summary>
            /// Gets or sets the starting index of rows in this table
            /// </summary>
            public int StartRowIndex
            {
                get { return _startRowIndex; }
                set { this._startRowIndex = value; }
            }

            #endregion

            #region  public int EndRowIndex { get; set;}

            /// <summary>
            /// Gets or sets the last index of rows in this table
            /// </summary>
            public int EndRowIndex
            {
                get { return _endRowIndex; }
                set { this._endRowIndex = value; }
            }

            #endregion

            #region public PDFLayoutBlock TableBlock {get;}

            /// <summary>
            /// Gets or sets the layout block for this table
            /// </summary>
            public PDFLayoutBlock TableBlock
            {
                get { return _tableBlock; }
                set { _tableBlock = value; }
            }

            #endregion

            #region public PDFStyle TableStyle {get;}

            /// <summary>
            /// Gets the style associated with the table
            /// </summary>
            public Style TableStyle
            {
                get { return _tableStyle; }
            }

            #endregion

            #region public PDFPositionOptions Position {get;}

            /// <summary>
            /// Gets the position options for the table
            /// </summary>
            public PDFPositionOptions Position
            {
                get { return _posOpts; }
            }

            #endregion

            #region public TableReference OwnerTable {get;}

            /// <summary>
            /// Gets the owner of this grid
            /// </summary>
            public TableReference OwnerTable
            {
                get { return _ownerTable; }
            }

            #endregion

            #region public RowReference this[int index] {get;}

            /// <summary>
            /// Gets the Row reference for this grid
            /// (offset from the whole table by this grids starting row index)
            /// </summary>
            /// <param name="index"></param>
            /// <returns></returns>
            public RowReference this[int index]
            {
                get
                {
                    return this.OwnerTable.AllRows[index];
                }
            }

            #endregion

            #region public PDFRect AvailableSpace {get;set;}

            /// <summary>
            /// Gets or sets the available space with in the outer table block for this grid
            /// </summary>
            public Rect AvailableSpace
            {
                get { return _availspace; }
                set { _availspace = value; }
            }

            #endregion

            #region public List<RowReference> HeaderRows {get;} + bool HasHeaderRows {get;}

            private List<RowReference> _headerrows = null;

            /// <summary>
            /// Gets a list of all the repeated header rows (as layout blocks) in this grid
            /// </summary>
            public List<RowReference> HeaderRows
            {
                get
                {
                    if (null == _headerrows)
                        _headerrows = new List<RowReference>();
                    return _headerrows;
                }
            }

            /// <summary>
            /// Returns true if this grid has repeader header rows
            /// </summary>
            public bool HasHeaderRows
            {
                get { return null != _headerrows && _headerrows.Count > 0; }
            }

            #endregion

            // ctor

            #region public GridReference(TableReference owner, PDFStyle style, PDFPositionOptions opts, int startRow, int endRow)

            /// <summary>
            /// Creates a new instance of a GridReference
            /// </summary>
            /// <param name="owner"></param>
            /// <param name="style"></param>
            /// <param name="opts"></param>
            /// <param name="startRow"></param>
            /// <param name="endRow"></param>
            public GridReference(TableReference owner, Style style, PDFPositionOptions opts, int startRow, int endRow)
            {
                this._ownerTable = owner;
                this._tableStyle = style;
                this._posOpts = opts;
                this._startRowIndex = startRow;
                this._endRowIndex = endRow;
            }

            #endregion

            #region internal PDFSize CalculateContentSize()

            internal Size CalculateContentSize()
            {
                Unit h = Unit.Zero;
                Unit w = Unit.Zero;
                for (int i = this.StartRowIndex; i <= this.EndRowIndex; i++)
                {
                    RowReference row = this[i];
                    if (null != row.Block)
                    {
                        h += row.Block.Height;
                        w = Unit.Max(w, row.Block.Width);
                    }
                }

                return new Size(w, h);
            }

            #endregion
        }

        #endregion

        #region private class TableReference

        /// <summary>
        /// A table of references to the grids, rows and cells in this table
        /// </summary>
        private class TableReference
        {

            #region ivars

            private RowReference[] _rows;
            private CellReference[,] _cells;
            private List<GridReference> _grids;
            private int _columncount;
            private int _rowcount;

            #endregion

            // properties

            #region public RowReference[] AllRows

            /// <summary>
            /// Gets all the row references in all the grids
            /// </summary>
            public RowReference[] AllRows
            {
                get { return _rows; }
            }

            #endregion

            #region public List<RowReference> RepeatRows { get;} + bool HasRepeats {get;}

            private List<RowReference> _repeats;

            /// <summary>
            /// Gets the list of Repeat Rows (can be null)
            /// </summary>
            public List<RowReference> RepeatRows
            {
                get { return _repeats; }
            }

            /// <summary>
            /// Returns true if any of the rows are marked as repeating across regions / pages
            /// </summary>
            public bool HasRepeats
            {
                get { return null != _repeats && _repeats.Count > 0; }
            }

            #endregion

            #region public CellReference[,] AllCells

            /// <summary>
            /// Gets the 2 dimensional array of all the cells in all grids (row, column)
            /// </summary>
            public CellReference[,] AllCells
            {
                get { return _cells; }
            }

            #endregion

            #region public ICollection<GridReference> AllGrids

            /// <summary>
            /// Gets the collection of contiguous grids in this table
            /// </summary>
            public ICollection<GridReference> AllGrids
            {
                get { return _grids; }
            }

            #endregion

            #region public GridReference CurrentGrid {get;}

            /// <summary>
            /// Gets the current grid in this table
            /// </summary>
            public GridReference CurrentGrid
            {
                get
                {
                    int last = _grids.Count - 1;
                    if (last < 0)
                        return null;
                    else
                        return _grids[last];
                }
            }

            #endregion

            #region public int TotalRowCount {get;}

            /// <summary>
            /// Gets the total count of all rows in this referenced table
            /// </summary>
            public int TotalRowCount
            {
                get { return _rowcount; }
            }

            #endregion

            #region public int TotalColumnCount {get;}

            /// <summary>
            /// Gets the total count of all columns in this referenced table
            /// </summary>
            public int TotalColumnCount
            {
                get { return _columncount; }
            }

            #endregion

            // .ctors

            #region public TableReference(int rowcount, int columncount, PDFStyle tableStyle, PDFPositionOptions posOpts)

            /// <summary>
            /// Create a new Table reference, initializing the Rows, Cells and one Grid based on the row and column counts
            /// </summary>
            /// <param name="rowcount"></param>
            /// <param name="columncount"></param>
            /// <param name="tableStyle"></param>
            /// <param name="posOpts"></param>
            public TableReference(int rowcount, int columncount, Style tableStyle, PDFPositionOptions posOpts)
            {
                _rows = new RowReference[rowcount];
                _cells = new CellReference[rowcount, columncount];
                _columncount = columncount;
                _rowcount = rowcount;
                _grids = new List<GridReference>();


                this.BeginNewGrid(tableStyle, posOpts, 0);
            }

            #endregion

            // methods

            #region public GridReference BeginNewGrid(PDFStyle fulltableStyle, PDFPositionOptions posOpts, int startRow)

            /// <summary>
            /// Begins a new contiguous grid for the table closing off any previous grid at theprevious row
            /// </summary>
            /// <param name="fulltableStyle"></param>
            /// <param name="posOpts"></param>
            /// <param name="startRow"></param>
            /// <returns></returns>
            public GridReference BeginNewGrid(Style fulltableStyle, PDFPositionOptions posOpts, int startRow)
            {
                if (null != this.CurrentGrid)
                {
                    //Truncate the pre
                    this.CurrentGrid.EndRowIndex = startRow - 1;
                }

                GridReference grid = new GridReference(this, fulltableStyle, posOpts, startRow, this._rowcount - 1); //one less for the end row index
                _grids.Add(grid);
                return grid;

            }

            #endregion


            #region public RowReference AddRowReference(PDFTableRow row, GridReference grid, PDFStyle applied, PDFStyle fullstyle, int rowIndex) + 1 overload

            /// <summary>
            /// Adds a new row reference at the specified index and returns the instance
            /// </summary>
            /// <param name="row">The associated table row for the reference</param>
            /// <param name="grid">The grid this row is a part of</param>
            /// <param name="applied">The applied style of the row</param>
            /// <param name="fullstyle">The full style of the row</param>
            /// <param name="rowIndex">The index of the row to insert. There cannot be an existing row reference at this index</param>
            public RowReference AddRowReference(TableRow row, GridReference grid, Style applied, Style fullstyle, int rowIndex)
            {
                RowReference rref = new RowReference(row, grid, applied, fullstyle, rowIndex);
                this.AddRowReference(rref);
                return rref;
            }


            /// <summary>
            /// Adds the row reference to the grid at the index specified in the reference's RowIndex. 
            /// NOTE: There cannot be an existing row reference at this index
            /// </summary>
            /// <param name="rref"></param>
            public void AddRowReference(RowReference rref)
            {
                if (null != _rows[rref.RowIndex])
                    throw new ArgumentException("Row reference has already been set");
                this._rows[rref.RowIndex] = rref;
                if (rref.Repeat == TableRowRepeat.RepeatAtTop)
                {
                    if (null == this._repeats)
                        _repeats = new List<RowReference>();
                    _repeats.Add(rref);
                }
            }

            #endregion

            #region public void AddCellReference(PDFTableCell cell, RowReference rowRef, PDFStyle applied, PDFStyle fullstyle, int column) + 1 overload

            /// <summary>
            /// Adds a new cell reference at the specified index and returns the instance
            /// </summary>
            /// <param name="cell">The associated table cell</param>
            /// <param name="rowRef">The row reference for this cell</param>
            /// <param name="applied">The applied style of the cell</param>
            /// <param name="fullstyle">The full style of the cell</param>
            /// <param name="column">The column index of the cell. NOTE: There cannot be an existing cell reference in the rows column</param>
            public CellReference AddCellReference(TableCell cell, RowReference rowRef, Style applied, Style fullstyle, int column)
            {
                CellReference cref = new CellReference(cell, rowRef, applied, fullstyle, rowRef.RowIndex, column);
                this.AddCellReference(cref);
                return cref;
            }



            /// <summary>
            /// Adds a cell reference to the grid at the row and column index in the reference
            /// NOTE: There cannot be an existing cell reference in the rows column
            /// </summary>
            /// <param name="reference"></param>
            public void AddCellReference(CellReference reference)
            {
                if (null != this._cells[reference.RowIndex, reference.ColumnIndex])
                    throw new ArgumentException("Cell reference has already been set");

                this._cells[reference.RowIndex, reference.ColumnIndex] = reference;
            }


            
            /// <summary>
            /// Adds a new empty cell reference to this table 
            /// </summary>
            /// <param name="rowRef"></param>
            /// <param name="colindex"></param>
            /// <returns></returns>
            public CellReference AddEmptyCellReference(RowReference rowRef, int colindex)
            {
                CellReference cref = new CellReference(null, rowRef, null, null, rowRef.RowIndex, colindex);
                cref.IsEmpty = true;
                // Empty cells (occupied by rowspan or colspan) need default span values to ensure proper loop iteration
                cref.ColumnSpan = 1;
                cref.RowSpan = 1;
                this._cells[rowRef.RowIndex, cref.ColumnIndex] = cref;

                return cref;
            }
            
            
            #endregion

            #region public void SetRowCount(int count, bool clear)

            /// <summary>
            /// Alters the total row count in this table, 
            /// and optionally removes / clears any references to rows beyond this.
            /// </summary>
            /// <param name="count">The new row count</param>
            /// <param name="clear">Option to clear rows that have been created but are no longer required.</param>
            public void SetRowCount(int count, bool clear)
            {
                int orig = _rowcount;
                this._rowcount = count;

                if (orig < count)
                {
                    RowReference[] newRows = _rows;
                    Array.Resize<RowReference>(ref newRows, count);
                    _rows = newRows;
                    CellReference[,] newcells = _cells;
                    Resize2DArray<CellReference>(ref newcells, count, _columncount);
                    _cells = newcells;
                }
                else if (clear)
                    this.ClearAllRowReferencesFrom(count);

            }

            #endregion

            #region public void ClearAllRowReferencesFrom(int rowindex)

            /// <summary>
            /// Removes all the cell and row references for all rows after and including the row at the specified index.
            /// And also sets any associated layout blocks for the rows and cells to invisible
            /// </summary>
            /// <param name="rowindex"></param>
            public void ClearAllRowReferencesFrom(int rowindex)
            {
                for (int index = _rowcount - 1; index >= rowindex; index--)
                {
                    this.ClearRowReference(index);
                }
            }

            #endregion

            #region public void ClearRowReference(int rowindex)

            /// <summary>
            /// Clears the row and cell references for the specified index, and also sets any associated layout block to invisible.
            /// </summary>
            /// <param name="rowindex"></param>
            public void ClearRowReference(int rowindex)
            {
                PDFLayoutBlock block = _rows[rowindex].Block;
                if (null != block)
                    block.Position.Visibility = Visibility.None;
                for (int col = 0; col < _columncount; col++)
                {
                    block = this._cells[rowindex, col].Block;
                    if (null != block)
                        block.Position.Visibility = Visibility.None;
                    this._cells[rowindex, col] = null;
                }
                this._rows[rowindex] = null;

            }

            #endregion

            #region public PDFUnit GetMaxCellHeightForRow(int rowindex)

            /// <summary>
            /// Gets the maximum required height of all cells in a particular row
            /// </summary>
            /// <param name="rowindex"></param>
            /// <returns></returns>
            public Unit GetMaxCellHeightForRow(int rowindex)
            {
                Unit maxH = Unit.Zero;
                for (int col = 0; col < _columncount; col++)
                {
                    CellReference cref = this._cells[rowindex, col];
                    if (null != cref && null != cref.Block)
                    {
                        Unit h = cref.Block.TotalBounds.Height;
                        maxH = Unit.Max(h, maxH);
                    }
                }
                return maxH;
            }

            #endregion

            #region public PDFUnit GetMaxCellWidthForColumn(int colIndex)

            /// <summary>
            /// Gets the maximum required width of all cells in a particular column
            /// </summary>
            /// <param name="colIndex"></param>
            /// <returns></returns>
            public Unit GetMaxCellWidthForColumn(int colIndex)
            {
                Unit maxW = Unit.Zero;
                for (int row = 0; row < _rowcount; row++)
                {
                    CellReference cref = this._cells[row, colIndex];
                    if (null != cref && null != cref.Block && cref.IsEmpty == false && cref.ColumnSpan == 1)
                    {
                        Unit w = cref.Block.TotalBounds.Width;
                        maxW = Unit.Max(w, maxW);
                    }
                }
                return maxW;
            }

            #endregion


            public void EnsureSufficientWidthForSpannedCells(int colIndex, Unit[] widths)
            {
                for (int row = 0; row < _rowcount; row++)
                {
                    CellReference cref = this._cells[row, colIndex];
                    if (cref.IsEmpty)
                    {
                        //We have a spanned cell, so we need to look back for the actual cell
                        //based on this - if the last column it spanns is this colIndex
                        // we can calculate the current width in previous columns and ensure that 
                        //this last column can contain the remainder fro the required block size 

                        int cellCol = colIndex;
                        CellReference act = null;
                        while (cellCol >= 0)
                        {
                            act = this._cells[row, cellCol];
                            if (!act.IsEmpty)
                                break; //this is the actual cell that spans the column
                            cellCol--;
                        }

                        if (null != act && null != act.Block && ((act.ColumnSpan-1) + cellCol == colIndex)) //we have a cell and this is the last
                        {
                            Unit widthUpToLast = Unit.Zero;
                            for (int i = 0; i < act.ColumnSpan-1; i++)
                            {
                                widthUpToLast += widths[cellCol + i];
                            }
                            Unit required = act.Block.TotalBounds.Width;
                            required -= widthUpToLast;
                            if (required > widths[colIndex])
                                widths[colIndex] = required;
                        }
                    }
                }
            }

            #region public void SetCellHeightForRow(int rowIndex, PDFUnit h)

            /// <summary>
            /// Sets the height of all cells in a row to the specified value. 
            /// Also sets the height of any block associcated with the cell
            /// </summary>
            /// <param name="rowIndex"></param>
            /// <param name="h"></param>
            public void SetCellHeightForRow(int rowIndex, Unit h)
            {
                RowReference rref = this._rows[rowIndex];
                for (int col = 0; col < _columncount; col++)
                {
                    CellReference cref = rref[col];
                    if (null != cref && null != cref.Block)
                    {
                        Rect total = cref.Block.TotalBounds;
                        total.Height = h; //- cref.Margins.Top - cref.Margins.Bottom;
                        cref.Block.TotalBounds = total;
                    }
                }
                rref.ExplicitHeight = h;
                
            }

            #endregion

            

            #region public void SetCellWidthForColumn(int colIndex, PDFUnit w)

            /// <summary>
            /// Sets the width of all the cell in a column to the specified value. 
            /// Also sets the width of any block associated with the cell
            /// </summary>
            /// <param name="colIndex"></param>
            /// <param name="w"></param>
            public void SetCellWidthForColumn(int colIndex, Unit[] widths)
            {
                try
                {
                    for (int row = 0; row < _rowcount; row++)
                    {
                        CellReference cref = this._cells[row, colIndex];
                        if (null != cref && null != cref.Block && cref.IsEmpty == false)
                        {
                            Unit w = Unit.Zero;
                            for (int i = 0; i < cref.ColumnSpan; i++)
                            {
                                w += widths[colIndex + i];
                            }
                            Rect total = cref.Block.TotalBounds;
                            total.Width = w; //- cref.Margins.Left - cref.Margins.Right;
                            cref.Block.TotalBounds = total;
                        }
                    }

                    foreach (GridReference grid in this._grids)
                    {
                        if (grid.HasHeaderRows)
                        {
                            foreach (RowReference row in grid.HeaderRows)
                            {
                                CellReference cref = this._cells[row.RowIndex, colIndex];
                                int colspan = cref.ColumnSpan;

                                Unit w = Unit.Zero;
                                for (int i = 0; i < cref.ColumnSpan; i++)
                                {
                                    w += widths[colIndex + i];
                                }

                                PDFLayoutBlock rowblock = row.Block;
                                PDFLayoutRegion cellregion = rowblock.Columns[colIndex];

                                //check there is content in the column - Fix for luis 06/01/2015
                                if (cellregion.Contents != null && cellregion.Contents.Count > 0)
                                {
                                    PDFLayoutBlock cell = cellregion.Contents[0] as PDFLayoutBlock;
                                    Rect total = cell.TotalBounds;
                                    total.Width = w;
                                    cell.TotalBounds = total;
                                }

                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    throw new PDFLayoutException("Could not push the cell widths for column " + colIndex + " as : " + ex.Message);
                }

            }

            #endregion

            #region internal void SetRowOffsetAndWidthForColumn(int colIndex, PDFUnit xOffset, PDFUnit maxColWidth)

            /// <summary>
            /// Sets the offset and width of a column region in each of the rows
            /// </summary>
            /// <param name="colIndex">The column region</param>
            /// <param name="xOffset">The horizontal offset from the start of the row</param>
            /// <param name="maxColWidth">The width of the column</param>
            internal void SetRowOffsetAndWidthForColumn(int colIndex, Unit[] widths)
            {
                //TODO: This can be done with an outer loop

                Unit xOffset = Unit.Zero;
                Unit maxColWidth = widths[colIndex];
                for (int i = 0; i < colIndex; i++)
                {
                    xOffset += widths[i];
                }

                for (int rowindex = 0; rowindex < _rowcount; rowindex++)
                {
                    RowReference rowref = _rows[rowindex];
                    if (null != rowref && null != rowref.Block)
                    {
                        PDFLayoutBlock rowblock = rowref.Block;
                        PDFLayoutRegion column = rowblock.Columns[colIndex];
                        Rect colbounds = column.TotalBounds;
                        colbounds.X = xOffset;
                        colbounds.Width = maxColWidth;
                        column.TotalBounds = colbounds;
                    }
                }

                //Update the row region offsets for all the grids with header rows
                foreach (GridReference grid in this._grids)
                {
                    if (grid.HasHeaderRows)
                    {
                        foreach (RowReference rowref in grid.HeaderRows)
                        {
                            if (null != rowref.Block)
                            {
                                PDFLayoutBlock rowblock = rowref.Block;
                                PDFLayoutRegion column = rowblock.Columns[colIndex];
                                Rect colbounds = column.TotalBounds;
                                colbounds.X = xOffset;
                                colbounds.Width = maxColWidth;
                                column.TotalBounds = colbounds;
                            }
                        }
                    }
                }
            }

            #endregion

            #region private static void Resize2DArray<T>(ref T[,] original, int rows, int cols)

            /// <summary>
            /// Helper method that will resize a 2 dimensional array
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="original"></param>
            /// <param name="rows"></param>
            /// <param name="cols"></param>
            private static void Resize2DArray<T>(ref T[,] original, int rows, int cols)
            {
                var newArray = new T[rows, cols];
                int minRows = Math.Min(rows, original.GetLength(0));
                int minCols = Math.Min(cols, original.GetLength(1));
                for (int row = 0; row < minRows; row++)
                    for (int col = 0; col < minCols; col++)
                        newArray[row, col] = original[row, col];
                original = newArray;
            }

            #endregion
        }

        #endregion


        #region private struct CellDimension

        /// <summary>
        /// Holds a single dimension for a cell
        /// </summary>
        private struct CellDimension
        {
            private Unit _sz;
            private bool _explicit;

            public Unit Size { get { return _sz; } set { _sz = value; } }
            public bool Explicit { get { return _explicit; } set { _explicit = value; } }

            public CellDimension(Unit size)
                : this(size, true)
            {
            }

            public CellDimension(Unit size, bool isExplicit)
            {
                _sz = size;
                _explicit = isExplicit;
            }

            public override string ToString()
            {
                return this.Explicit ? _sz.ToString() + " Explicit" : _sz.ToString();
            }
        }

        #endregion
    }
}
