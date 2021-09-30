using System;
using Scryber.OpenType.SubTables;
using Scryber.PDF.Native;

namespace Scryber.Drawing
{
    public class PDFGradientRadialBrush : PDFGradientBrush
    {
        private PDFGradientRadialDescriptor _descriptor;

        public RadialShape Shape
        {
            get { return _descriptor.Shape; }
        }

        public PDFGradientRadialBrush(PDFGradientRadialDescriptor descriptor)
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
            var id = doc.GetIncrementID(PDFObjectTypes.Pattern);

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
