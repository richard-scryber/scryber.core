using Scryber.PDF;
using Scryber.PDF.Native;
using Scryber.PDF.Resources;

namespace Scryber.Drawing
{
    /// <summary>
    /// Contains the drawing information about a tiled graphic pattern to use when outputting a new tiling pattern
    /// </summary>
    /// <remarks>There sould be one instance ot the descriptor per defined pattern, and can be
    /// stored in a documents shared resources for access from other classes such as the PDFGraphicTilingPattern.
    /// The pattern can use the information here to actually calculate and output a PDF pattern.</remarks>
    public class GraphicTilingPatternDescriptor : PDFResource
    {

        private string _descriptorKey;
        public override string ResourceKey
        {
            get { return _descriptorKey; }
        }

        public override string ResourceType
        {
            get { return PDFResource.PatternResourceType; }
        }

        protected int PatternCount { get; set; }

        public Point PatternOffset { get; set; }

        public Size PatternSize { get; set; }
        
        
        public Rect PatternViewBox { get; set; }


        /// <summary>
        /// Gets or sets the current tiling pattern being rendered
        /// </summary>
        public PDFGraphicTilingPattern CurrentPattern { get; set; }

        /// <summary>
        /// Gets or sets the current size of the tiling pattern being rendered
        /// </summary>
        public Size CurrentSize { get; set; }
        
        /// <summary>
        /// Gets or sets the current pattern bounds being rendered.
        /// </summary>
        public Rect CurrentBounds { get; set; }

        public GraphicTilingPatternDescriptor(string descriptorKey, Point patternOffset, Size patternSize, Rect patternViewBox) : base(ObjectTypes.GraphicPattern)
        {
            this._descriptorKey = descriptorKey;
            this.PatternOffset = patternOffset;
            this.PatternSize = patternSize;
            this.PatternViewBox = patternViewBox;
            PatternCount = 0;
        }

        public override bool Equals(string resourcetype, string key)
        {
            return (this.ResourceType == resourcetype && this.ResourceKey == key);
        }

        public string GetNextPatternResourceKey()
        {
            this.PatternCount++;
            return this.ResourceKey + this.PatternCount.ToString();
        }

        public override void RegisterUse(PDFResourceList resourcelist, IComponent Component)
        {
            //Do nothing as we don't want to render any content
        }

        protected override PDFObjectRef DoRenderToPDF(ContextBase context, PDFWriter writer)
        {
            //This does not perform any rendering.
            return null;
        }

        public static string GetResourceKey(string patternbaseid)
        {
            return patternbaseid + "_descriptor";
        }
        
        private Unit EnsureAbsolute(Unit defined, Unit reference)
        {
            if (defined.IsRelative)
            {
                if(defined.Units == PageUnits.Percent)
                    return reference.PointsValue * (defined.Value / 100);
                else
                {
                    defined = reference;
                }
            }
            return defined;
        }

        public Rect CalculatePatternBoundsForShape(Rect tilingBounds, ContextBase context)
        {
            var x = EnsureAbsolute(this.PatternOffset.X, tilingBounds.Width);
            var y = EnsureAbsolute(this.PatternOffset.Y, tilingBounds.Height);
            var width = EnsureAbsolute(this.PatternSize.Width, tilingBounds.Width);
            var height = EnsureAbsolute(this.PatternSize.Height, tilingBounds.Height);
            
            return new Rect(x, y, width, height);
        }

        public Size CalculatePatternStepForShape(Rect tilingBounds, ContextBase context)
        {
            var width = EnsureAbsolute(this.PatternSize.Width, tilingBounds.Width);
            var height = EnsureAbsolute(this.PatternSize.Height, tilingBounds.Height);
            
            return new Size(width, height);
        }
    }
}