using System;
using Scryber.PDF;
using Scryber.PDF.Graphics;
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
        public Size CurrentPatternSize { get; set; }
        
        /// <summary>
        /// Gets or sets the current pattern bounds being rendered.
        /// </summary>
        public Rect CurrentBounds { get; set; }
        
        /// <summary>
        /// Gets or sets the graphic container size where the current pattern is being rendered.
        /// </summary>
        public Size CurrentContainerSize { get; set; }

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
            var x = EnsureAbsolute(this.PatternViewBox.X, tilingBounds.Width);
            var y = EnsureAbsolute(this.PatternViewBox.Y, tilingBounds.Height);
            var width = EnsureAbsolute(this.PatternViewBox.Width, tilingBounds.Width);
            var height = EnsureAbsolute(this.PatternViewBox.Height, tilingBounds.Height);
            
            return new Rect(x, y, width, height);
        }

        public Size CalculatePatternStepForShape(Rect tilingBounds, ContextBase context)
        {
            //get the output expected size
            var resultwidth = EnsureAbsolute(this.PatternSize.Width, tilingBounds.Width);
            var resultheight = EnsureAbsolute(this.PatternSize.Height, tilingBounds.Height);

            
            //get the internal pattern size
            var viewwidth = EnsureAbsolute(this.PatternViewBox.Width, tilingBounds.Width);
            var viewheight = EnsureAbsolute(this.PatternViewBox.Height, tilingBounds.Height);
            
            var scaleX = resultwidth.PointsValue / viewwidth.PointsValue;
            var scaleY = resultheight.PointsValue / viewheight.PointsValue;

            if (scaleX == scaleY)
            {
                //matching scale so we are ok
            }
            else if (scaleX < scaleY)
            {
                //The horizontal scale will be used so the ystep should be increased
                viewheight = resultheight / scaleX;
            }
            else
            {
                //The vertical scale is smallest and will be used. So the xstep should be increased
                viewwidth = resultwidth / scaleY;
            }
            
            
            return new Size(viewwidth, viewheight);
        }

        public PDFTransformationMatrix CalculatePatternTransformMatrixForShape(Rect tilingBounds, ContextBase context)
        {
            //get the output expected size
            var resultwidth = EnsureAbsolute(this.PatternSize.Width, tilingBounds.Width);
            var resultheight = EnsureAbsolute(this.PatternSize.Height, tilingBounds.Height);

            
            //get the internal pattern size
            var viewwidth = EnsureAbsolute(this.PatternViewBox.Width, tilingBounds.Width);
            var viewheight = EnsureAbsolute(this.PatternViewBox.Height, tilingBounds.Height);
            
            var scaleX = resultwidth.PointsValue / viewwidth.PointsValue;
            var scaleY = resultheight.PointsValue / viewheight.PointsValue;
            
            var min = Math.Min(scaleX, scaleY);
            
            var patternView = CalculatePatternBoundsForShape(tilingBounds, context);
            
            TransformOperationSet set = new TransformOperationSet();
            set.AppendOperation(new TransformScaleOperation(min, min));
            
            TransformOrigin origin = null; //updated manually below.
            
            var matrix = set.GetTransformationMatrix(this.CurrentContainerSize, origin);
            var movex = 0 - matrix.Components[4];
            var movey = 0 - matrix.Components[5];

            //update the translations so the pattern is centred horizontally
            //and offset to the bottom of the top repeat within the shape.
            
            movex += tilingBounds.X.PointsValue;
            
            var step = CalculatePatternStepForShape(tilingBounds, context);
            var view = CalculatePatternBoundsForShape(tilingBounds, context);
            if (step.Width > view.Width)
            {
                var half = (step.Width - view.Width) / 2;
                var scaled = half * matrix.Components[0]; //scalex
                movex += scaled.PointsValue;
            }
            
            movey += this.CurrentContainerSize.Height.PointsValue;
            movey -= tilingBounds.Y.PointsValue;
            movey -= (step.Height.PointsValue * matrix.Components[4]); //scale y
            
            
            matrix.SetTranslation(movex, movey);
            
            //matrix.SetTranslation(tilingBounds.X, tilingBounds.Y - this.CurrentContainerSize.Height);
            //if(this.PatternSize.Width.IsRelative)

            return matrix;
        }
    }
}