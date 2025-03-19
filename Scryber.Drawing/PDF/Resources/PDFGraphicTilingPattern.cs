using Scryber.Drawing;
using Scryber.PDF.Native;

namespace Scryber.PDF.Resources
{
    /// <summary>
    /// A document shared resource for rendering out a repeating pattern described by the GraphicPatternDescriptor to a PDFWriters stream
    /// </summary>
    /// <remarks>This class is used by the PDFGraphicPatternBrush to register the pattern resource with the document.
    /// A single pattern should be output only once per document rendition</remarks>
    public class PDFGraphicTilingPattern: PDFTilingPattern
    {
        public GraphicPatternDescriptor Descriptor { get; private set; }  

        public PDFGraphicTilingPattern(IComponent container, string fullkey, GraphicPatternDescriptor descriptor) : base(container, fullkey)
        {
            
        }

        protected override PDFObjectRef DoRenderToPDF(ContextBase context, PDFWriter writer)
        {
            throw new System.NotImplementedException();
        }
    }
}