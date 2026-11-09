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
        /// A reference to a single continuous table grid in one region.
        /// </summary>
        protected class GridReference
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
    }
}