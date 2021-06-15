using System;
using Scryber.Resources;

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
            get { return this.Descriptor.Colors.ToArray(); }
        }

        public bool Repeating
        {
            get { return this.Descriptor.Repeating; }
        }

        public PDFGradientBrush(PDFGradientDescriptor descriptor)
        {
            this.Descriptor = descriptor;
        }


        protected PDFRect ConvertToPageRect(PDFGraphics graphics, PDFRect bounds)
        {
            PDFRect pgRect = PDFRect.Empty;

            pgRect.X = new PDFUnit(graphics.GetXPosition(bounds.X).Value);
            pgRect.Y = new PDFUnit(graphics.GetYPosition(bounds.Y).Value);
            pgRect.Width = new PDFUnit(graphics.GetXOffset(bounds.Width).Value);
            pgRect.Height = new PDFUnit(graphics.GetYOffset(bounds.Height).Value);

            return pgRect;
        }

        public PDFResource GetLinearShadingPattern(PDFGraphics g, string key, PDFGradientLinearDescriptor descriptor, PDFRect bounds)
        {
            PDFLinearShadingPattern pattern = new PDFLinearShadingPattern(g.Container.Document, key, descriptor, bounds);
            return pattern;
        }

        public PDFResource GetRadialShadingPattern(PDFGraphics g, string key, PDFGradientRadialDescriptor descriptor, PDFRect bounds)
        {
            PDFRadialShadingPattern pattern = new PDFRadialShadingPattern(g.Container.Document, key, descriptor, bounds);
            return pattern;
        }
    }

    
}
