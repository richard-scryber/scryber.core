using System;
using Scryber.Native;
using Scryber.Resources;

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
            var doc = graphics.Container.Document;
            var id = doc.GetIncrementID(PDFObjectTypes.Pattern);

            bounds = ConvertToPageRect(graphics, bounds);
            
            var linear = this.GetLinearShadingPattern(graphics, id, this._descriptor, bounds);
            if (null != linear)
            {
                var name = graphics.Container.Register(linear);
                graphics.SetFillPattern(name);
                return true;
            }
            else
            {
                return false;
            }
        }

        private PDFRect ConvertToPageRect(PDFGraphics graphics, PDFRect bounds)
        {
            PDFRect pgRect = PDFRect.Empty;

            pgRect.X = new PDFUnit(graphics.GetXPosition(bounds.X).Value);
            pgRect.Y = new PDFUnit(graphics.GetYPosition(bounds.Y).Value);
            pgRect.Width = new PDFUnit(graphics.GetXOffset(bounds.Width).Value);
            pgRect.Height = new PDFUnit(graphics.GetYOffset(bounds.Height).Value);

            return pgRect;
        }

        public PDFResource GetLinearShadingPattern(PDFGraphics g, string key, PDFLinearGradientDescriptor descriptor, PDFRect bounds)
        {
            PDFLinearShadingPattern pattern = new PDFLinearShadingPattern(g.Container.Document, key, descriptor, bounds);
            return pattern;
        }
    }
}
