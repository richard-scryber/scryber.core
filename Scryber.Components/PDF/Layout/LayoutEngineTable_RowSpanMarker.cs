
using System;

namespace Scryber.PDF.Layout
{
    public partial class LayoutEngineTable : LayoutEngineBase
    {
        
        /// <summary>
        /// Tracks a cell that spans multiple rows, storing metadata about the rowspan
        /// </summary>
        /// 
        [Obsolete("This class is obsolete.", true)]        
        protected class RowspanCellMarkers
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
            public RowspanCellMarkers(CellReference cell, int startRowIndex, int columnIndex, int columnSpan)
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
    }
}
