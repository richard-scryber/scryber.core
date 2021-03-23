using System;
using Scryber.Drawing;

namespace Scryber.Layout
{


    /// <summary>
    /// A linked list of the left or right floats
    /// </summary>
    public abstract class PDFFloatAddition
    {
        public PDFUnit Inset, Height, Offset;
        public PDFFloatAddition Prev;

        public PDFFloatAddition(PDFUnit inset, PDFUnit height, PDFUnit offset, PDFFloatAddition prev)
        {
            this.Inset = inset;
            this.Height = height;
            this.Offset = offset;
            this.Prev = prev;
        }

        public virtual PDFUnit ApplyWidths(PDFUnit available, PDFUnit yoffset)
        {
            if (yoffset > this.Offset && yoffset < (this.Offset + this.Height))
                available -= this.Inset;

            if (null != this.Prev)
                available = this.Prev.ApplyWidths(available, yoffset);

            return available;
        }

        public virtual PDFUnit ApplyXInset(PDFUnit x, PDFUnit yoffset, PDFUnit height)
        {
            if (null != this.Prev)
                x = this.Prev.ApplyXInset(x, yoffset, height);

            return x;
        }
    }

    public class PDFFloatLeftAddition : PDFFloatAddition
    {
        public PDFFloatLeftAddition(PDFUnit inset, PDFUnit height, PDFUnit offset, PDFFloatAddition prev)
            : base(inset, height, offset, prev)
        {
        }

        public override PDFUnit ApplyXInset(PDFUnit x, PDFUnit yoffset, PDFUnit height)
        {
            if ((yoffset + height) >= this.Offset && yoffset < (this.Offset + this.Height))
                x += this.Inset;
            return base.ApplyXInset(x, yoffset, height);
        }
    }

    public class PDFFloatRightAddition : PDFFloatAddition
    {
        public PDFFloatRightAddition(PDFUnit inset, PDFUnit height, PDFUnit offset, PDFFloatAddition prev)
            : base(inset, height, offset, prev)
        {
        }
    }

}
