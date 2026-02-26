using System;
using System.Collections.Generic;
using Scryber.Drawing;
using Scryber.Styles;
using Scryber.Components;

namespace Scryber.PDF.Layout
{
    public partial class LayoutEngineTable : LayoutEngineBase
    {
        /// <summary>
        /// A table of references to the grids, rows and cells in this table
        /// </summary>
        protected class TableReference
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
            public CellReference AddCellContentReference(TableCell cell, RowReference rowRef, Style applied, Style fullstyle, int column)
            {
                CellReference cref = new CellReference(cell, rowRef, applied, fullstyle, rowRef.RowIndex, column);
                cref.Type = CellContentType.Content;
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
            public CellReference AddEmptyCellReference(RowReference rowRef, int colindex, CellContentType type)
            {
                CellReference cref = new CellReference(null, rowRef, null, null, rowRef.RowIndex, colindex);
                cref.Type = type;
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
                    if (null != cref && null != cref.Block && cref.Type == CellContentType.Content && cref.ColumnSpan == 1)
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
                    if (cref.Type== CellContentType.SpannedColumn || cref.Type == CellContentType.SpannedRow)
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
                            if (act.Type == CellContentType.Content)
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
                        if (null != cref && null != cref.Block && cref.Type == CellContentType.Content)
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

    }
}