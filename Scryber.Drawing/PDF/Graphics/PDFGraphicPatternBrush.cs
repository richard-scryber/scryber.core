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
        
        public string PatternKey { get; set; }

        
        public PDFGraphicPatternBrush(string key)
        {
            this.PatternKey = key;
        }
        
        public override bool SetUpGraphics(PDFGraphics graphics, Rect bounds)
        {
            var pattern = graphics.Container.Document.GetResource(PDFResource.PatternResourceType, this.PatternKey, false) as PDFGraphicTilingPattern;
            if (null == pattern)
            {
                throw new ArgumentException("No resource found for the GrpahicTilingPattern with the given key :" + this.PatternKey,
                    "PatternKey");
            }
            
            pattern.RegisterUse(graphics.Container.Resources, graphics.Container.Document);
            
            var xObj = graphics.Container.Document.GetResource(PDFResource.XObjectResourceType, pattern.PatternLayoutKey, false) as PDFResource;
            if(null == xObj)
            {
                throw new ArgumentException("No resource found for the PatternLayout with the given key :" + this.PatternKey,
                    "pattern.PatternLayoutKey");
            }
            xObj.RegisterUse(graphics.Container.Resources, graphics.Container.Document);
            
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