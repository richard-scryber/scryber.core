using System;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.PDF.Graphics;
using Scryber.Styles;

namespace Scryber.PDF.Layout
{
    public partial class LayoutEngineTable : LayoutEngineBase
    {

        /// <summary>
        /// Applies the border-collapse and border-spacing of the table to the cell styles. 
        /// Must be called after the cell styles are built, and before the references are built from them.
        /// </summary>
        private void ApplyTableBorderModel(int rowcount, int columncount)
        {
            if (rowcount == 0 || columncount == 0)
                return;

            var collapse = this.FullStyle.GetValue(StyleKeys.TableBorderCollapseKey, BorderCollapse.Separate);

            if (collapse == BorderCollapse.Collapse)
            {
                this.CollapseCellBorders(rowcount, columncount);
            }
            else
            {
                var h = this.FullStyle.GetValue(StyleKeys.TableBorderSpacingHorizontalKey, Unit.Zero);
                var v = this.FullStyle.GetValue(StyleKeys.TableBorderSpacingVerticalKey, Unit.Zero);

                if (h > Unit.Zero || v > Unit.Zero)
                    this.ApplyCellSpacing(rowcount, columncount, h, v);
            }
        }

        #region private void CollapseCellBorders(int rowcount, int columncount)

        [Flags]
        private enum RemovedEdges
        {
            None = 0,
            Left = 1,
            Top = 2,
            Right = 4,
            Bottom = 8
        }

        /// <summary>
        /// For collapsed borders, a border shared between 2 cells (or a cell and the table) is only drawn once,
        /// by removing the narrower of the two. If they are the same width, the border of the following cell (or the table) is the one removed.
        /// </summary>
        private void CollapseCellBorders(int rowcount, int columncount)
        {
            var removals = new RemovedEdges[rowcount, columncount];
            var any = false;
            var tableBorders = this.FullStyle.CreateBorderPen();

            for (int r = 0; r < rowcount; r++)
            {
                for (int c = 0; c < columncount; c++)
                {
                    var cref = _cellContents[r, c];
                    if (cref.Type != CellContentType.Content)
                        continue;

                    var borders = cref.Full.CreateBorderPen();
                    var colspan = GetSpan(cref.Full, StyleKeys.TableCellColumnSpanKey);
                    var rowspan = GetSpan(cref.Full, StyleKeys.TableCellRowSpanKey);

                    var left = GetSideWidth(borders, Sides.Left);
                    var right = GetSideWidth(borders, Sides.Right);
                    var top = GetSideWidth(borders, Sides.Top);
                    var bottom = GetSideWidth(borders, Sides.Bottom);

                    //the right edge shared with the next cell
                    if (right > Unit.Zero && c + colspan < columncount && TryFindOwner(r, c + colspan, out var or, out var oc))
                    {
                        var neighbourLeft = GetSideWidth(_cellContents[or, oc].Full.CreateBorderPen(), Sides.Left);
                        if (neighbourLeft > Unit.Zero)
                        {
                            if (right >= neighbourLeft)
                                removals[or, oc] |= RemovedEdges.Left;
                            else
                                removals[r, c] |= RemovedEdges.Right;
                            any = true;
                        }
                    }

                    //the bottom edge shared with the cell below
                    if (bottom > Unit.Zero && r + rowspan < rowcount && TryFindOwner(r + rowspan, c, out var br, out var bc))
                    {
                        var neighbourTop = GetSideWidth(_cellContents[br, bc].Full.CreateBorderPen(), Sides.Top);
                        if (neighbourTop > Unit.Zero)
                        {
                            if (bottom >= neighbourTop)
                                removals[br, bc] |= RemovedEdges.Top;
                            else
                                removals[r, c] |= RemovedEdges.Bottom;
                            any = true;
                        }
                    }

                    //the edges shared with the table. The cell border is only removed if the table border is as wide.
                    if (null != tableBorders)
                    {
                        if (c == 0 && left > Unit.Zero && GetSideWidth(tableBorders, Sides.Left) >= left)
                        {
                            removals[r, c] |= RemovedEdges.Left;
                            any = true;
                        }
                        if (c + colspan >= columncount && right > Unit.Zero && GetSideWidth(tableBorders, Sides.Right) >= right)
                        {
                            removals[r, c] |= RemovedEdges.Right;
                            any = true;
                        }
                        if (r == 0 && top > Unit.Zero && GetSideWidth(tableBorders, Sides.Top) >= top)
                        {
                            removals[r, c] |= RemovedEdges.Top;
                            any = true;
                        }
                        if (r + rowspan >= rowcount && bottom > Unit.Zero && GetSideWidth(tableBorders, Sides.Bottom) >= bottom)
                        {
                            removals[r, c] |= RemovedEdges.Bottom;
                            any = true;
                        }
                    }
                }
            }

            if (!any)
                return;

            for (int r = 0; r < rowcount; r++)
            {
                for (int c = 0; c < columncount; c++)
                {
                    var removed = removals[r, c];
                    if (removed == RemovedEdges.None)
                        continue;

                    var cref = _cellContents[r, c];

                    //Use a copy, as the cell style can be shared with other cells (e.g. from a repeating template).
                    var copy = CopyStyle(cref.Full);

                    if ((removed & RemovedEdges.Left) != 0)
                        copy.SetPriorityValue(StyleKeys.BorderLeftStyleKey, LineType.None, int.MaxValue);
                    if ((removed & RemovedEdges.Right) != 0)
                        copy.SetPriorityValue(StyleKeys.BorderRightStyleKey, LineType.None, int.MaxValue);
                    if ((removed & RemovedEdges.Top) != 0)
                        copy.SetPriorityValue(StyleKeys.BorderTopStyleKey, LineType.None, int.MaxValue);
                    if ((removed & RemovedEdges.Bottom) != 0)
                        copy.SetPriorityValue(StyleKeys.BorderBottomStyleKey, LineType.None, int.MaxValue);

                    _cellContents[r, c] = new CellContentReference(copy, cref.Applied, cref.Type);
                }
            }
        }

        #endregion

        #region private void ApplyCellSpacing(int rowcount, int columncount, Unit horizontal, Unit vertical)

        /// <summary>
        /// For separate borders with border-spacing, the space between the cells is made by margins on each cell 
        /// (half the spacing between cells, and the full spacing at the edges of the table).
        /// </summary>
        private void ApplyCellSpacing(int rowcount, int columncount, Unit horizontal, Unit vertical)
        {
            var halfH = horizontal / 2;
            var halfV = vertical / 2;

            for (int r = 0; r < rowcount; r++)
            {
                for (int c = 0; c < columncount; c++)
                {
                    var cref = _cellContents[r, c];
                    if (cref.Type != CellContentType.Content)
                        continue;

                    var colspan = GetSpan(cref.Full, StyleKeys.TableCellColumnSpanKey);
                    var rowspan = GetSpan(cref.Full, StyleKeys.TableCellRowSpanKey);

                    var copy = CopyStyle(cref.Full);

                    copy.SetPriorityValue(StyleKeys.MarginsLeftKey, c == 0 ? horizontal : halfH, int.MaxValue);
                    copy.SetPriorityValue(StyleKeys.MarginsRightKey, c + colspan >= columncount ? horizontal : halfH, int.MaxValue);
                    copy.SetPriorityValue(StyleKeys.MarginsTopKey, r == 0 ? vertical : halfV, int.MaxValue);
                    copy.SetPriorityValue(StyleKeys.MarginsBottomKey, r + rowspan >= rowcount ? vertical : halfV, int.MaxValue);

                    _cellContents[r, c] = new CellContentReference(copy, cref.Applied, cref.Type);
                }
            }
        }

        #endregion

        private static Style CopyStyle(Style style)
        {
            var copy = new StyleFull();
            style.MergeInto(copy);
            return copy;
        }

        private static int GetSpan(Style style, StyleKey<int> key)
        {
            var span = style.GetValue(key, 1);
            return span < 1 ? 1 : span;
        }

        private static Unit GetSideWidth(PDFPenBorders borders, Sides side)
        {
            if (null == borders)
                return Unit.Zero;

            PDFPen pen;
            switch (side)
            {
                case Sides.Left: pen = borders.LeftPen; break;
                case Sides.Right: pen = borders.RightPen; break;
                case Sides.Top: pen = borders.TopPen; break;
                default: pen = borders.BottomPen; break;
            }

            if (null == pen && (borders.AllSides & side) == side)
                pen = borders.AllPen;

            return (null == pen || pen.LineStyle == LineType.None) ? Unit.Zero : pen.Width;
        }

        /// <summary>
        /// Finds the cell that occupies the grid position, which may be the origin (starting) row and column of a cell that spans the position.
        /// </summary>
        private bool TryFindOwner(int row, int column, out int ownerRow, out int ownerColumn)
        {
            ownerRow = row;
            ownerColumn = column;

            while (ownerRow >= 0 && ownerColumn >= 0)
            {
                switch (_cellContents[ownerRow, ownerColumn].Type)
                {
                    case CellContentType.Content:
                        return true;
                    case CellContentType.SpannedColumn:
                        ownerColumn--;
                        break;
                    case CellContentType.SpannedRow:
                        ownerRow--;
                        break;
                    default:
                        return false;
                }
            }
            return false;
        }
    }
}
