
using Scryber.Components;
using Scryber.Styles;

namespace Scryber.PDF.Layout
{
    public partial class LayoutEngineTable : LayoutEngineBase
    {
        
        /// <summary>
        /// A struct to hold references to the content of a cell, including the full and applied styles, and the type of content (empty, content, spanned row, spanned column)
        /// </summary>
        protected struct CellContentReference
        {
            /// <summary>
            /// A reference to the full style of a content cell. Null if the cell is empty or spanned.
            /// </summary>
            public Style Full;

            /// <summary>
            /// A reference to the applied style of a content cell. Null if the cell is empty or spanned.
            /// </summary>
            public Style Applied;

            ///<summary>
            /// The type of content this cell contains (empty, content, spanned row, spanned column)
            /// </summary>
            public CellContentType Type;

            public CellContentReference(Style full, Style applied, CellContentType type)
            {
                this.Full = full;
                this.Applied = applied;
                this.Type = type;
            }
        }
    }
}   