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

        /// <summary>
        /// Gets or sets the matrix from the pattern space to the default space of the content it is used in
        /// - the transformation of the shape it fills, as the pattern does not follow the transformation of the drawing.
        /// </summary>
        public Scryber.PDF.Graphics.PDFTransformationMatrix Matrix { get; set; }

        /// <summary>
        /// Writes the /Matrix entry of the pattern dictionary, if there is a matrix that is not the identity
        /// </summary>
        protected void WriteMatrix(PDFWriter writer)
        {
            if (null != this.Matrix && !this.Matrix.IsIdentity)
            {
                writer.BeginDictionaryEntry("Matrix");
                writer.WriteArrayRealEntries(true, this.Matrix.Components, "F4");
                writer.EndDictionaryEntry();
            }
        }

        public PDFShadingPattern(IComponent owner, string key, Rect bounds, PatternType type = PatternType.ShadingPattern)
            : base(owner, type, key)
        {
            this.Start = bounds.Location;
            this.Size = bounds.Size;
        }
    }
}
