using System;
using Scryber.Drawing;

namespace Scryber.PDF.Layout
{


    /// <summary>
    /// A linked list of the left or right floats
    /// </summary>
    public abstract class PDFFloatAddition
    {

        public FloatMode Mode { get; private set; }

        public PDFUnit FloatWidth, FloatHeight, FloatInset, YOffset;
        public PDFFloatAddition Prev;

        public PDFFloatAddition(FloatMode mode, PDFUnit floatWidth, PDFUnit floatheight, PDFUnit floatInset, PDFUnit yoffset, PDFFloatAddition prev)
        {
            this.Mode = mode;
            this.FloatWidth = floatWidth;
            this.FloatHeight = floatheight;
            this.FloatInset = floatInset;
            this.YOffset = yoffset;
            this.Prev = prev;
        }

        /// <summary>
        /// Based on the full available width, if this float affects the width of a component at the specified offest (with height).
        /// then it will be reduced by the width of this float. It will also check any Previous floats.
        /// </summary>
        /// <param name="available"></param>
        /// <param name="yoffset"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public PDFUnit ApplyWidthInset(PDFUnit available, PDFUnit yoffset, PDFUnit height)
        {
            PDFUnit inset = PDFUnit.Zero;

            var leftOffset = this.GetLeftOffset(0, yoffset, height);
            var rightInset = this.GetRightInset(0, yoffset, height);

            available -= (leftOffset + rightInset);

            return available;

            PDFUnit newAvail = available;
            if (this.IsAffectingWidth(yoffset, height))
            {
                inset = this.FloatWidth + this.FloatInset;
                newAvail = available - inset;
            }

            if (null != this.Prev)
            {
                var prevAvail = this.Prev.ApplyWidthInset(available, yoffset, height);
                newAvail = PDFUnit.Min(newAvail, prevAvail);
            }

            return newAvail;
        }

        /// <summary>
        /// If this float will affect the left offset of any component at yoffset (with height), then the
        /// returned offset will be the offset from the start of the container.
        /// </summary>
        /// <param name="xoffset">Any initial x offset to use (can be zero)</param>
        /// <param name="yoffset">The vertical offset of the component in the container this float is in</param>
        /// <param name="height">The height of the component, for ensuring exact matches are not missed use 0.1 if zero</param>
        /// <returns></returns>
        public virtual PDFUnit GetLeftOffset(PDFUnit xoffset, PDFUnit yoffset, PDFUnit height)
        {
            if (null != this.Prev)
            {
                xoffset = this.Prev.GetLeftOffset(xoffset, yoffset, height);
            }

            return xoffset;
        }

        public virtual PDFUnit GetRightInset(PDFUnit xoffset, PDFUnit yoffset, PDFUnit height)
        {
            if (null != this.Prev)
            {
                xoffset = this.Prev.GetRightInset(xoffset, yoffset, height);
            }

            return xoffset;
        }

        public virtual bool IsAffectingWidth(PDFUnit yoffset, PDFUnit height)
        {
            if ((yoffset + height) > this.YOffset && yoffset < (this.YOffset + this.FloatHeight))
                return true;
            else
                return false;
        }
    }

    public class PDFFloatLeftAddition : PDFFloatAddition
    {
        public PDFFloatLeftAddition(PDFUnit floatWidth, PDFUnit floatHeight, PDFUnit floatInset, PDFUnit yoffset, PDFFloatAddition prev)
            : base(FloatMode.Left, floatWidth, floatHeight, floatInset, yoffset, prev)
        {
        }

        public override PDFUnit GetLeftOffset(PDFUnit x, PDFUnit yoffset, PDFUnit height)
        {
            if (this.IsAffectingWidth(yoffset, height))
                x = PDFUnit.Max(this.FloatWidth + this.FloatInset, x);

            x = base.GetLeftOffset(x, yoffset, height);

            return x;
        }
    }

    public class PDFFloatRightAddition : PDFFloatAddition
    {
        public PDFFloatRightAddition(PDFUnit floatWidth, PDFUnit floatHeight, PDFUnit floatInset, PDFUnit yoffset, PDFFloatAddition prev)
            : base(FloatMode.Right, floatWidth, floatHeight, floatInset, yoffset, prev)
        {
        }

        public override PDFUnit GetRightInset(PDFUnit x, PDFUnit yoffset, PDFUnit height)
        {
            if (this.IsAffectingWidth(yoffset, height))
                x = PDFUnit.Max(this.FloatWidth + this.FloatInset, x);

            x = base.GetRightInset(x, yoffset, height);

            return x;
        }

    }

}
