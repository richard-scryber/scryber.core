using Scryber.Drawing;

namespace Scryber.PDF.Resources
{
    /// <summary>
    /// Base abstract tiling pattern class
    /// </summary>
    public abstract class PDFTilingPattern : PDFPattern
    {
        #region public PatternPaintType PaintType {get;set;}

        /// <summary>
        /// Gets or sets the paint type for this pattern
        /// </summary>
        public PatternPaintType PaintType
        {
            get;
            set;
        }

        #endregion

        #region public PatternTilingType TilingType {get;set;}

        /// <summary>
        /// Gets or sets the Tiling type for this pattern
        /// </summary>
        public PatternTilingType TilingType
        {
            get;
            set;
        }

        #endregion

        #region public PDFPoint Start {get;set;}

        /// <summary>
        /// Gets or sets the offset from the bottom left corner of the
        /// page (0,0) that the pattern repeats (top left of the pattern).
        /// </summary>
        public Point Start
        {
            get;
            set;
        }

        #endregion

        #region public PDFSize Step {get;set;}

        /// <summary>
        /// Gets or sets the distance between the start of each tile
        /// </summary>
        public Size Step
        {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Protected 
        /// </summary>
        /// <param name="container"></param>
        /// <param name="fullkey"></param>
        protected PDFTilingPattern(IComponent container, string fullkey)
            : base(container, PatternType.TilingPattern, fullkey)
        {
            this.TilingType = PatternTilingType.NoDistortion;
            this.PaintType = PatternPaintType.ColoredTile;
        }

    }
}