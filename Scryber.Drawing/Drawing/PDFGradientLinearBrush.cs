using System;
using Scryber.PDF;
using Scryber.PDF.Native;
using Scryber.PDF.Resources;
using Scryber.PDF.Graphics;

namespace Scryber.Drawing
{
    public class PDFGradientLinearBrush : PDFGradientBrush
    {
        private GradientLinearDescriptor _descriptor;

        public double Angle
        {
            get { return this._descriptor.Angle; }
        }

        
        public PDFGradientLinearBrush(GradientLinearDescriptor descriptor) : base(descriptor)
        {
            this._descriptor = descriptor;
        }

        

        public override void ReleaseGraphics(PDFGraphics g, PDFRect bounds)
        {
            
        }

        public override bool SetUpGraphics(PDFGraphics graphics, PDFRect bounds)
        {
            var doc = graphics.Container.Document;
            var id = doc.GetIncrementID(ObjectTypes.Pattern);

            bounds = ConvertToPageRect(graphics, bounds);
            
            var linear = this.GetLinearShadingPattern(graphics, id, this._descriptor, bounds);
            if (null != linear)
            {
                var name = graphics.Container.Register(linear);
                graphics.SetFillPattern((PDFName)name);
                return true;
            }
            else
            {
                return false;
            }
        }

        
    }
}
