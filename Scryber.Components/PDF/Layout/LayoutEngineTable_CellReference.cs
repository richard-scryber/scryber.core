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
        /// A reference to a single table cell and it's associated data in this table
        /// </summary>
        protected class CellReference
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
            private CellContentType _type;

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
            /// Returns the type of content this cell contains (empty, content, spanned row, spanned column)
            /// </summary>
            public CellContentType Type
            {
                get { return _type; }
                set { _type = value; }
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
    }
}