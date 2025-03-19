using Scryber.Drawing;

namespace Scryber.PDF.Graphics
{
    public class PDFGraphicPatternBrush : PDFBrush
    {
        public override FillType FillStyle { get { return FillType.Pattern; } }
        
        public GraphicPatternDescriptor Descriptor { get; set; }

        
        public PDFGraphicPatternBrush(Scryber.Drawing.GraphicPatternDescriptor descriptor)
        {
            this.Descriptor = descriptor;
        }
        
        public override bool SetUpGraphics(PDFGraphics graphics, Rect bounds)
        {
            return base.SetUpGraphics(graphics, bounds);
        }

        public override void ReleaseGraphics(PDFGraphics g, Rect bounds)
        {
            base.ReleaseGraphics(g, bounds);
        }
    }
}