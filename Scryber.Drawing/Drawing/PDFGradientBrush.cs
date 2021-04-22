using System;

namespace Scryber.Drawing
{
    public abstract class PDFGradientBrush : PDFBrush
    {

        public override FillType FillStyle { get { return FillType.Pattern; } }

        public PDFGradientDescriptor Descriptor
        {
            get;
            protected set;
        }

        public GradientType GradientType
        {
            get { return this.Descriptor.GradientType; }
        }

        public PDFGradientColor[] Colors
        {
            get { return this.Descriptor.Colors; }
        }

        public bool Repeating
        {
            get { return this.Descriptor.Repeating; }
        }

        public PDFGradientBrush(PDFGradientDescriptor descriptor)
        {
            this.Descriptor = descriptor;
        }
    }

    
}
