using System;
using Scryber.Drawing;
using Scryber.PDF.Graphics;

namespace Scryber.PDF
{
    public class PDFPenBorders
    {
        /// <summary>
        /// The general pen that should be used to draw the borders for each side specified in AllSides
        /// </summary>
        public PDFPen AllPen { get; set; }

        /// <summary>
        /// The flags for each of the sides that should be drawn using the AllSides pen
        /// </summary>
        public Sides AllSides { get; set; }

        /// <summary>
        /// The explicit left pen
        /// </summary>
        public PDFPen LeftPen { get; set; }

        /// <summary>
        /// The explicit top side pen
        /// </summary>
        public PDFPen TopPen { get; set; }

        /// <summary>
        /// The explicit right side pen
        /// </summary>
        public PDFPen RightPen { get; set; }

        /// <summary>
        /// The explicit bottom side pen
        /// </summary>
        public PDFPen BottomPen { get; set; }

        public PDFUnit? CornerRadius { get; set; }

        public bool HasBorders
        {
            get
            {
                if (null != AllPen && AllSides > 0)
                    return true;
                else if (null != LeftPen || null != RightPen || null != TopPen || null != BottomPen)
                    return true;
                else
                    return false;
            }
        }

        public Sides BorderSides
        {
            get
            {
                Sides sides = this.AllSides;
                if (null != LeftPen)
                    sides |= Sides.Left;
                if (null != TopPen)
                    sides |= Sides.Top;
                if (null != BottomPen)
                    sides |= Sides.Bottom;
                if (null != RightPen)
                    sides |= Sides.Right;

                return sides;
            }
        }

        public PDFPenBorders()
        {

        }

        public PDFPenBorders(PDFPen all, Sides allSides)
        {
            this.AllPen = all;
            this.AllSides = allSides;
        }
    }
}
