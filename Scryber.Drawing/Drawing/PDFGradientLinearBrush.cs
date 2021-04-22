using System;
namespace Scryber.Drawing
{
    public class PDFGradientLinearBrush : PDFGradientBrush
    {
        private PDFLinearGradientDescriptor _descriptor;

        public double Angle
        {
            get { return this._descriptor.Angle; }
        }

        
        public PDFGradientLinearBrush(PDFLinearGradientDescriptor descriptor) : base(descriptor)
        {
            this._descriptor = descriptor;
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
