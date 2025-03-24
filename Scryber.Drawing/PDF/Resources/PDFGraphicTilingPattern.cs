using System;
using Scryber.Drawing;
using Scryber.PDF.Graphics;
using Scryber.PDF.Native;

namespace Scryber.PDF.Resources
{
    /// <summary>
    /// A document shared resource for rendering out a repeating pattern described by the GraphicPatternDescriptor
    /// and a referenced layout, specified by the PatternLayoutKey to a PDFWriters stream
    /// </summary>
    /// <remarks>This class is used by the PDFGraphicPatternBrush to register the pattern resource with the document.
    /// A single pattern should be output only once per document rendition.
    /// The descriptor and layout detail the way this pattern is rendered with the EnsureRendered base method.</remarks>
    public class PDFGraphicTilingPattern: PDFTilingPattern
    {
        public ICanvas GraphicCanvas { get; set; }
        public string PatternLayoutKey { get; set; }

        /// <summary>
        /// Creates a new graphic tiling pattern for a PDF document.
        /// </summary>
        /// <param name="pattern">The original pattern creating and owning this pattern</param>
        /// <param name="tilingkey">The resource name of this tiling pattern</param>
        /// <param name="canvas">The Canvas used for the the rendering and inner pattern resources</param>
        /// <param name="layoutKey">The resource key name of the XObjectResource layout to actually draw the pattern each time.</param>
        public PDFGraphicTilingPattern(IComponent pattern, string tilingkey, string layoutKey, ICanvas canvas) : base(pattern, tilingkey)
        {
            this.PatternLayoutKey = layoutKey;
            this.GraphicCanvas = canvas;
        }


        public override bool Equals(string resourcetype, string key)
        {
            if (String.Equals(this.ResourceType, resourcetype) && String.Equals(key, this.ResourceKey))
                return true;
            else
                return base.Equals(resourcetype, key);
        }

        /// <summary>
        /// overrides the base implementation to actually render the object stream of the layout
        /// </summary>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        protected override PDFObjectRef RenderTileContents(ContextBase context, PDFWriter writer)
        {
            var key = this.PatternLayoutKey;
            
            var objResource = this.Container.Document.GetResource(PDFResource.XObjectResourceType, key, false) as PDFResource;
            if (null != objResource)
            {
                //make sure the resource is registered, and then ensure it is rendered (as we know we are using it).
                
                objResource.RegisterUse(this.GraphicCanvas.Resources, this.GraphicCanvas);
                
                var oref = objResource.EnsureRendered(context, writer);
                
                return oref;
            }
            else
            {
                throw new NullReferenceException("Resource not found for the layout with key '" + key + "'");
            }
            
            return null;
        }
    }
}