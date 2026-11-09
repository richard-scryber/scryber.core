using System;
using Scryber.PDF;
using Scryber.PDF.Native;
using Scryber.PDF.Resources;
using Scryber.Drawing;

namespace Scryber.PDF.Graphics
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


        protected Rect ConvertToPageRect(PDFGraphics graphics, Rect bounds)
        {
            Rect pgRect = Rect.Empty;

            pgRect.X = new Unit(graphics.GetXPosition(bounds.X).Value);
            pgRect.Y = new Unit(graphics.GetYPosition(bounds.Y).Value);
            pgRect.Width = new Unit(graphics.GetXOffset(bounds.Width).Value);
            pgRect.Height = new Unit(graphics.GetYOffset(bounds.Height).Value);

            return pgRect;
        }

        public PDFResource GetLinearShadingPattern(PDFGraphics g, string key, GradientLinearDescriptor descriptor, Rect bounds)
        {
            PDFLinearShadingPattern pattern = new PDFLinearShadingPattern(g.Container.Document, key, descriptor, bounds);
            
            //Implement and add to the 
            // if(g.CurrentTransformationMatrix != null)
            //     pattern.TransformationMatrix = g.CurrentTransformationMatrix;
            
            return pattern;
        }

        public PDFResource GetRadialShadingPattern(PDFGraphics g, string key, GradientRadialDescriptor descriptor, Rect bounds)
        {
            PDFRadialShadingPattern pattern = new PDFRadialShadingPattern(g.Container.Document, key, descriptor, bounds);
            
            // if(g.CurrentTransformationMatrix != null)
            //     pattern.TransformationMatrix = g.CurrentTransformationMatrix;

            return pattern;
        }
    }

    
}
