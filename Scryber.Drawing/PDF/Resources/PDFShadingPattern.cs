using System;
using Scryber.Drawing;

namespace Scryber.PDF.Resources
{
    public abstract class PDFShadingPattern : PDFPattern
    {
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

        #region public PDFSize Size {get;set;}

        /// <summary>
        /// Gets or sets the distance between the start of each tile
        /// </summary>
        public Size Size
        {
            get;
            set;
        }

        #endregion

        public PDFShadingPattern(IComponent owner, string key, Rect bounds, PatternType type = PatternType.ShadingPattern)
            : base(owner, type, key)
        {
            this.Start = bounds.Location;
            this.Size = bounds.Size;
        }
    }
}
