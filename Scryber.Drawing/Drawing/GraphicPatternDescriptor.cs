using Scryber.PDF;
using Scryber.PDF.Graphics;
using Scryber.PDF.Native;

namespace Scryber.Drawing
{
    /// <summary>
    /// 
    /// </summary>
    public class GraphicPatternDescriptor
    {
        public ICanvas GraphicCanvas { get; set; }
        
        public string XObjectResourceKey { get; set; }

        public GraphicPatternDescriptor(ICanvas graphicCanvas, string xObjectResourceKey)
        {
            this.GraphicCanvas = graphicCanvas;
            this.XObjectResourceKey = xObjectResourceKey;
        }

        
    }
}