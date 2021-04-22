using System;
using Scryber.OpenType.SubTables;

namespace Scryber.Drawing
{
    public class PDFGradientRadialBrush : PDFGradientBrush
    {
        private PDFRadialGradientDescriptor _descriptor;

        public RadialShape Shape
        {
            get { return _descriptor.Shape; }
        }

        public PDFGradientRadialBrush(PDFRadialGradientDescriptor descriptor)
            : base(descriptor)
        {
            _descriptor = descriptor;
        }

        public override void ReleaseGraphics(PDFGraphics g, PDFRect bounds)
        {
            
        }

        public override bool SetUpGraphics(PDFGraphics graphics, PDFRect bounds)
        {
            return false;
        }
    }
}
