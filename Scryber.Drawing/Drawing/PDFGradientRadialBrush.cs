using System;
using Scryber.OpenType.SubTables;
using Scryber.PDF.Native;
using Scryber.PDF.Graphics;

namespace Scryber.Drawing
{
    public class PDFGradientRadialBrush : PDFGradientBrush
    {
        private GradientRadialDescriptor _descriptor;

        public RadialShape Shape
        {
            get { return _descriptor.Shape; }
        }

        public PDFGradientRadialBrush(GradientRadialDescriptor descriptor)
            : base(descriptor)
        {
            _descriptor = descriptor;
        }

        public override void ReleaseGraphics(PDFGraphics g, PDFRect bounds)
        {
            
        }

        public override bool SetUpGraphics(PDFGraphics graphics, PDFRect bounds)
        {
            var doc = graphics.Container.Document;
            var id = doc.GetIncrementID(ObjectTypes.Pattern);

            bounds = ConvertToPageRect(graphics, bounds);

            var linear = this.GetRadialShadingPattern(graphics, id, this._descriptor, bounds);
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
