namespace Scryber.PDF.Layout
{
    public partial class LayoutEngineTable : LayoutEngineBase
    {
        /// <summary>
        /// Enum to represent the type of content in a cell reference. This is used to determine how to handle empty cells and spanned cells during layout.
        /// </summary>
        protected enum CellContentType
        {
            /// <summary>
            /// This is a cell that is not defined in the layout, and has no content and is not spanned by another cell.
            /// </summary>
            Empty,
            /// <summary>
            /// This is a cell that has content and is not spanned by another cell (but can span other cells).
            /// </summary>
            Content,
            /// <summary>
            /// This is an empty cell that is spanned by another cell in the same row.
            /// </summary>
            SpannedRow,
            /// <summary>
            /// This is an empty cell that is spanned by another cell in the same column.
            /// </summary>
            SpannedColumn
        }
    }
}