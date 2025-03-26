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
        /// <summary>
        /// Gets or sets the drawing canvas containing the graphic components that will be drawn.
        /// </summary>
        public ICanvas GraphicCanvas { get; set; }
        
        /// <summary>
        /// Gets or sets the layout resource that can render the pattern contents
        /// </summary>
        public PDFResource PatternLayout { get; set; }
        
        /// <summary>
        /// Gets or sets the descriptor
        /// </summary>
        public GraphicTilingPatternDescriptor PatternDescriptor { get; set; }
        
       
        /// <summary>
        /// Gets the rect that will be filled with the pattern
        /// </summary>
        public Rect TilingBounds { get; protected set; }

        /// <summary>
        /// Creates a new graphic tiling pattern for a PDF document.
        /// </summary>
        /// <param name="pattern">The original pattern creating and owning this pattern</param>
        /// <param name="tilingkey">The resource name of this tiling pattern</param>
        /// <param name="canvas">The Canvas used for the the rendering and inner pattern resources</param>
        /// <param name="layoutKey">The resource key name of the XObjectResource layout to actually draw the pattern each time.</param>
        public PDFGraphicTilingPattern(IComponent pattern, string resourceKey, GraphicTilingPatternDescriptor descriptor, PDFResource layout, Rect tilingBounds) 
            : base(pattern, resourceKey)
        {
            this.PatternLayout = layout;
            this.PatternDescriptor = descriptor;
            this.TilingBounds = tilingBounds;
            this.Name = (PDFName)pattern.Document.GetIncrementID(ObjectTypes.GraphicPattern);
            
        }

        /// <summary>
        /// Used to set the bounds of the parent that will have this pattern applied.
        /// </summary>
        /// <param name="bounds">The bounds of the parent to tile.</param>
        public virtual void SetTilingBounds(Rect bounds)
        {
            this.TilingBounds = bounds;
        }

        public override bool Equals(string resourcetype, string key)
        {
            if (String.Equals(this.ResourceType, resourcetype) && String.Equals(key, this.ResourceKey))
                return true;
            else
                return base.Equals(resourcetype, key);
        }

        
        
        private Size _absoluteStep = Size.Empty;
        private Rect _absoluteBounds = Rect.Empty;

        public override Rect CalculatePatternBoundingBox(ContextBase context)
        {
            this._absoluteBounds = this.PatternDescriptor.CalculatePatternBoundsForShape(this.TilingBounds, context);
            return this._absoluteBounds;
            
        }

        public override Size CalculateStepSize(ContextBase context)
        {
            this._absoluteStep = this.PatternDescriptor.CalculatePatternStepForShape(this.TilingBounds, context);
            return this._absoluteStep;
        }

        /// <summary>
        /// overrides the base implementation to actually render the object stream of the layout
        /// </summary>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        protected override PDFObjectRef RenderTileContents(ContextBase context, PDFWriter writer)
        {
            var prev = this.PatternDescriptor.CurrentPattern;
            
            this.PatternDescriptor.CurrentPattern = this;
            this.PatternDescriptor.CurrentBounds = CalculatePatternBoundingBox(context);
            this.PatternDescriptor.CurrentSize = this.CalculateStepSize(context);
            
            
            var oref = this.PatternLayout.EnsureRendered(context, writer);
            
            // if (null != objResource)
            // {
            //     //make sure the resource is registered, and then ensure it is rendered (as we know we are using it).
            //     var prevStep = this.Step;
            //     var prevViewPort = this.ViewPort;
            //     this.Step = _absoluteStep;
            //     this.ViewPort = this._absoluteBounds;
            //     
            //     objResource.RegisterUse(this.GraphicCanvas.Resources, this.GraphicCanvas);
            //     
            //     var oref = objResource.EnsureRendered(context, writer);
            //     
            //     this.Step = prevStep;
            //     this.ViewPort = prevViewPort;
            //     
            //     return oref;
            // }
            // else
            // {
            //     throw new NullReferenceException("Resource not found for the layout with key '" + key + "'");
            // }
            
            this.PatternDescriptor.CurrentPattern = prev;
            return oref;
        }
    }
}