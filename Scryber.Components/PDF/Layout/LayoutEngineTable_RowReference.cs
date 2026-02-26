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
        /// A reference to a single row in a table
        /// </summary>
        protected class RowReference
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
    }
}