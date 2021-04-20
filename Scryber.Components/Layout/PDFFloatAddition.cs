using System;
using Scryber.Drawing;

namespace Scryber.Layout
{


    /// <summary>
    /// A linked list of the left or right floats
    /// </summary>
    public abstract class PDFFloatAddition
    {
        public PDFUnit XInset, Height, YOffset;
        public PDFFloatAddition Prev;

        public PDFFloatAddition(PDFUnit xinset, PDFUnit height, PDFUnit yoffset, PDFFloatAddition prev)
        {
            this.XInset = xinset;
            this.Height = height;
            this.YOffset = yoffset;
            this.Prev = prev;
        }

        public virtual PDFUnit ApplyWidths(PDFUnit available, PDFUnit yoffset, PDFUnit height)
        {
            if ((yoffset + height) > this.YOffset && yoffset < (this.YOffset + this.Height))
                available -= this.XInset;

            if (null != this.Prev)
                available = this.Prev.ApplyWidths(available, yoffset, height);

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
            if ((yoffset + height) > this.YOffset && yoffset < (this.YOffset + this.Height))
                x += this.XInset;
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
