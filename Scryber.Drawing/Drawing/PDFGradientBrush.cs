using System;
using Scryber.PDF;
using Scryber.PDF.Native;
using Scryber.PDF.Resources;
using Scryber.PDF.Graphics;

namespace Scryber.Drawing
{
    public abstract class PDFGradientBrush : PDFBrush
    {

        public override FillType FillStyle { get { return FillType.Pattern; } }

        public GradientDescriptor Descriptor
        {
            get;
            protected set;
        }

        public GradientType GradientType
        {
            get { return this.Descriptor.GradientType; }
        }

        public GradientColor[] Colors
        {
            get { return this.Descriptor.Colors.ToArray(); }
        }

        public bool Repeating
        {
            get { return this.Descriptor.Repeating; }
        }

        public PDFGradientBrush(GradientDescriptor descriptor)
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

        public PDFResource GetLinearShadingPattern(PDFGraphics g, string key, GradientLinearDescriptor descriptor, PDFRect bounds)
        {
            PDFLinearShadingPattern pattern = new PDFLinearShadingPattern(g.Container.Document, key, descriptor, bounds);
            return pattern;
        }

        public PDFResource GetRadialShadingPattern(PDFGraphics g, string key, GradientRadialDescriptor descriptor, PDFRect bounds)
        {
            PDFRadialShadingPattern pattern = new PDFRadialShadingPattern(g.Container.Document, key, descriptor, bounds);
            return pattern;
        }
    }

    
}
