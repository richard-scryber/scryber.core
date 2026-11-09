using System;
using Scryber.Drawing;
using Scryber.PDF.Resources;

namespace Scryber.PDF.Graphics
{
    /// <summary>
    /// A fill brush 
    /// </summary>
    public class PDFGraphicPatternBrush : PDFBrush
    {
        public override FillType FillStyle { get { return FillType.Pattern; } }
        
        public string DescriptorKey { get; set; }
        
        public string LayoutKey { get; set; }

        public IComponent Owner { get; set; }
        
        public PDFGraphicPatternBrush(IComponent owner, string descriptorKey, string layoutKey)
        {
            this.Owner = owner;
            this.DescriptorKey = descriptorKey;
            this.LayoutKey = layoutKey;
        }
        
        public override bool SetUpGraphics(PDFGraphics graphics, Rect bounds)
        {
            var descriptor = graphics.Container.Document.GetResource(PDFResource.PatternResourceType, this.DescriptorKey, false) as GraphicTilingPatternDescriptor;
            if (null == descriptor)
            {
                if (graphics.Context.Conformance == ParserConformanceMode.Strict)
                    throw new ArgumentException(
                        "No resource found for the GrpahicTilingPatternDescriptor with the given key :" + this.DescriptorKey,
                        "DescriptorKey");
                else
                {
                    graphics.Context.TraceLog.Add(TraceLevel.Error, "Pattern",
                        "No resource found for the GrpahicTilingPatternDescriptor with the given key :" + this.DescriptorKey);
                    return false;
                }
            }

            var layout = graphics.Container.Document.GetResource(PDFResource.XObjectResourceType, this.LayoutKey, false) as PDFResource;
            
            if(null == layout)
            {
                if (graphics.Context.Conformance == ParserConformanceMode.Strict)
                    throw new ArgumentException(
                        "No resource found for the PatternLayout with the given key :" + this.LayoutKey,
                        "LayoutKey");
                else
                {
                    graphics.Context.TraceLog.Add(TraceLevel.Error, "Pattern", 
                        "No resource found for the PatternLayout with the given key :" + this.LayoutKey);
                    return false;
                }
            }

            var key = descriptor.GetNextPatternResourceKey();
            
            var pattern = new PDFGraphicTilingPattern(this.Owner, key, descriptor, layout, bounds);

            //Make sure we register their use
            pattern.RegisterUse(graphics.Container.Resources, graphics.Container.Document);
            descriptor.RegisterUse(graphics.Container.Resources, graphics.Container.Document);
            layout.RegisterUse(graphics.Container.Resources, graphics.Container.Document);
            
            //Do this last so we don't alter the output until we are OK.
            graphics.SetFillPattern(pattern.Name);
            return true;
        }

        
        public override void ReleaseGraphics(PDFGraphics g, Rect bounds)
        {
            g.ClearFillPattern();
        }
    }
}