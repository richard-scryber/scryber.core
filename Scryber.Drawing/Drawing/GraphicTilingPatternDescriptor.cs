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
        /// Gets or sets the type of positioning and scaling for the viewbox.
        /// </summary>
        public ViewPortAspectRatio PatternAspectRatio { get; set; }


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

        

        public GraphicTilingPatternDescriptor(string descriptorKey, Point patternOffset, Size patternSize, Rect patternViewBox, ViewPortAspectRatio aspectRatio) : base(ObjectTypes.GraphicPattern)
        {
            this._descriptorKey = descriptorKey;
            this.PatternOffset = patternOffset;
            this.PatternSize = patternSize;
            this.PatternViewBox = patternViewBox;
            this.PatternAspectRatio = aspectRatio;
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

            if (this.PatternAspectRatio.Meet == AspectRatioMeet.Slice)
            {
                var resultwidth = EnsureAbsolute(this.PatternSize.Width, tilingBounds.Width); //8pt
                var resultheight = EnsureAbsolute(this.PatternSize.Height, tilingBounds.Height); //18pt
                var scaleX = resultwidth.PointsValue / width.PointsValue;
                var scaleY = resultheight.PointsValue / height.PointsValue; //1.8
                
                double scale;
                
                if (scaleX == scaleY)
                {
                    scale = scaleX;
                }
                else if (scaleX > scaleY)
                {
                    //find the difference in the size and halve it to give the y offsets.
                    var space = resultwidth.PointsValue - resultheight.PointsValue;
                    var half = space / 2;
                    
                    //then convert back to the pattern viewbox based on the alignment
                    switch (this.PatternAspectRatio.Align)
                    {
                        case AspectRatioAlign.xMaxYMin:
                        case AspectRatioAlign.xMidYMin:
                        case AspectRatioAlign.xMinYMin:
                        default:
                            //This is the top section - which is actually the highest
                            y = space / scaleX;
                            break;
                        case AspectRatioAlign.xMaxYMid:
                        case AspectRatioAlign.xMidYMid:
                        case AspectRatioAlign.xMinYMid:
                            y += half / scaleX;
                            break;
                        case AspectRatioAlign.xMaxYMax:
                        case AspectRatioAlign.xMidYMax:
                        case AspectRatioAlign.xMinYMax:
                            //no need to shift as at the bottom (in PDF)
                            
                            break;
                    }
                    
                    height = resultheight.PointsValue / scaleX;
                    
                }
                else
                {
                    //find the difference in the size and halve it to give the x offset.
                    var space = resultheight.PointsValue - resultwidth.PointsValue;
                    var half = space / 2;
                    
                    //then convert back to the pattern viewbox based on the alignment
                    switch (this.PatternAspectRatio.Align)
                    {
                        case AspectRatioAlign.xMinYMax:
                        case AspectRatioAlign.xMinYMin:
                        case AspectRatioAlign.xMinYMid:
                        default:
                            //no need to shift
                            break;
                        case AspectRatioAlign.xMidYMax:
                        case AspectRatioAlign.xMidYMin:
                        case AspectRatioAlign.xMidYMid:
                            x += half / scaleY;
                            break;
                        case AspectRatioAlign.xMaxYMax:
                        case AspectRatioAlign.xMaxYMin:
                        case AspectRatioAlign.xMaxYMid:
                            x += space / scaleY;
                        break;
                    }
                    
                    width = resultwidth.PointsValue / scaleY;
 
                }
                
            }
            
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
            
            if(this.PatternAspectRatio.Align == AspectRatioAlign.None)
                return new Size(viewwidth, viewheight);
            else if (this.PatternAspectRatio.Meet == AspectRatioMeet.Slice)
            {
                if (scaleX == scaleY)
                {
                    //matching scale so we are ok
                }
                else if (scaleX > scaleY)
                {
                    //horizontal scale is bigger so...
                    //alter the height for the width scale so it completely fills
                    viewheight = resultheight / scaleX;
                }
                else
                {
                    //vertical scale is bigger
                    //alter the width for the height scale so it completely fills
                    viewwidth = resultwidth / scaleY;
                }
            }
            else
            {
                if (scaleX == scaleY)
                {
                    //matching scale so we are ok
                }
                else if (scaleX < scaleY)
                {
                    //Alter the height so the ratio is preserved within the bounds
                    viewheight = resultheight / scaleX;
                }
                else
                {
                    //Alter the width so the ratio is preserved within the bounds.
                    viewwidth = resultwidth / scaleY;
                }
            }

            return new Size(viewwidth, viewheight);
        }

        public PDFTransformationMatrix CalculatePatternTransformMatrixForShape(Rect tilingBounds, ContextBase context)
        {

            if (this.PatternAspectRatio.Align == AspectRatioAlign.None || this.PatternAspectRatio.Meet == AspectRatioMeet.None)
            {
                //We are not preserving the aspect ratio so we simply stretch the viewbox to the desired width
                return CalculatePatternTransformMatrixForNoPreservation(tilingBounds, context);
            }
            else if (this.PatternAspectRatio.Meet == AspectRatioMeet.Meet)
            {
                //meet will scale the pattern down so the entire viewport will be visible and located around the requested width and height based on the ratio and alignment
                return CalculatePatternTransformMatrixForMeet(tilingBounds, context);
            }
            else
            {
                //we are slicing - so the pattern will fill the requested width and height, possibly cutting off part of the pattern based on the ratio and alignment.
                return CalculatePatternTransformMatrixForSliced(tilingBounds, context);
            }
        }

        protected PDFTransformationMatrix CalculatePatternTransformMatrixForNoPreservation(Rect tilingBounds, ContextBase context)
        {
            //TODO: implement none
            var resultwidth = EnsureAbsolute(this.PatternSize.Width, tilingBounds.Width);
            var resultheight = EnsureAbsolute(this.PatternSize.Height, tilingBounds.Height);

            
            //get the internal pattern size
            var viewwidth = EnsureAbsolute(this.PatternViewBox.Width, tilingBounds.Width);
            var viewheight = EnsureAbsolute(this.PatternViewBox.Height, tilingBounds.Height);
            
            var step = CalculatePatternStepForShape(tilingBounds, context);
            
            var scaleX = resultwidth.PointsValue / viewwidth.PointsValue;
            var scaleY = resultheight.PointsValue / viewheight.PointsValue;

            var set = new TransformOperationSet();
            set.AppendOperation(new TransformScaleOperation(scaleX, scaleY));
            
            var matrix = set.GetTransformationMatrix(this.CurrentContainerSize, null);
            
            var movex = 0 - matrix.Components[4];
            var movey = 0 - matrix.Components[5];
            
            
            movex += tilingBounds.X.PointsValue;
            
            movey += this.CurrentContainerSize.Height.PointsValue;
            movey -= tilingBounds.Y.PointsValue;
            movey -= (step.Height.PointsValue * matrix.Components[3]); //scale y
            
            if (this.PatternOffset != Point.Empty)
            {
                var offsetX = EnsureAbsolute(this.PatternOffset.X, tilingBounds.Width);
                var offsetY = EnsureAbsolute(this.PatternOffset.Y, tilingBounds.Height);
                movex += offsetX.PointsValue;
                movey += offsetY.PointsValue;
            }
            
            matrix.SetTranslation(movex, movey);

            return matrix;
        }
        
        protected PDFTransformationMatrix CalculatePatternTransformMatrixForSliced(Rect tilingBounds, ContextBase context)
        {
            //get the output expected size
            var resultwidth = EnsureAbsolute(this.PatternSize.Width, tilingBounds.Width);
            var resultheight = EnsureAbsolute(this.PatternSize.Height, tilingBounds.Height);

            
            //get the internal pattern size
            var viewwidth = EnsureAbsolute(this.PatternViewBox.Width, tilingBounds.Width);
            var viewheight = EnsureAbsolute(this.PatternViewBox.Height, tilingBounds.Height);
            
            if(viewwidth == 0.0)
                viewwidth = resultwidth;
            if(viewheight == 0.0)
                viewheight = resultheight;
            
            
            var scaleX = resultwidth.PointsValue / viewwidth.PointsValue;
            var scaleY = resultheight.PointsValue / viewheight.PointsValue;
            
            //Use the maximum scale for slicing so we make sure we cover the entire area.
            var maxscale = Math.Max(scaleX, scaleY);
            
            TransformOperationSet set = new TransformOperationSet();
            set.AppendOperation(new TransformScaleOperation(maxscale, maxscale));
            
            TransformOrigin origin = null; //updated manually below.
            
            var matrix = set.GetTransformationMatrix(this.CurrentContainerSize, origin);
            var movex = 0 - matrix.Components[4]; //we want to clear this so we are back to 0,0
            var movey = 0 - matrix.Components[5];
            
            
            var step = CalculatePatternStepForShape(tilingBounds, context);
            var view = CalculatePatternBoundsForShape(tilingBounds, context);


            movex += tilingBounds.X.PointsValue;
           
            movey += this.CurrentContainerSize.Height.PointsValue;
            movey -= tilingBounds.Y.PointsValue;
            movey -= (step.Height.PointsValue * matrix.Components[3]);

            //movex += view.X.PointsValue;
            movey += view.Y.PointsValue;
            
            
            
            Point aspectOffset = GetAspectRatioForSlice(maxscale, maxscale, step, view, tilingBounds, context);
           
            movex += aspectOffset.X.PointsValue;
            movey += aspectOffset.Y.PointsValue;
            
            if (this.PatternOffset != Point.Empty)
            {
                var offsetX = EnsureAbsolute(this.PatternOffset.X, tilingBounds.Width);
                var offsetY = EnsureAbsolute(this.PatternOffset.Y, tilingBounds.Height);
                movex += offsetX.PointsValue;
                movey += offsetY.PointsValue;
            }
            
            //movex /= maxscale;
            
            matrix.SetTranslation(movex, movey);
            
            return matrix;
        }

        protected PDFTransformationMatrix CalculatePatternTransformMatrixForMeet(Rect tilingBounds, ContextBase context)
        {
            //get the output expected size
            var resultwidth = EnsureAbsolute(this.PatternSize.Width, tilingBounds.Width);
            var resultheight = EnsureAbsolute(this.PatternSize.Height, tilingBounds.Height);

            
            //get the internal pattern size
            var viewwidth = EnsureAbsolute(this.PatternViewBox.Width, tilingBounds.Width);
            var viewheight = EnsureAbsolute(this.PatternViewBox.Height, tilingBounds.Height);
            
            if(viewwidth == 0.0)
                viewwidth = resultwidth;
            if(viewheight == 0.0)
                viewheight = resultheight;
            
            
            var scaleX = resultwidth.PointsValue / viewwidth.PointsValue;
            var scaleY = resultheight.PointsValue / viewheight.PointsValue;
            
            var min = Math.Min(scaleX, scaleY);
            
            
            TransformOperationSet set = new TransformOperationSet();
            set.AppendOperation(new TransformScaleOperation(min, min));
            
            TransformOrigin origin = null; //updated manually below.
            
            var matrix = set.GetTransformationMatrix(this.CurrentContainerSize, origin);
            var movex = 0 - matrix.Components[4];
            var movey = 0 - matrix.Components[5];
            
            
            movex += tilingBounds.X.PointsValue;
            
            var step = CalculatePatternStepForShape(tilingBounds, context);
            var view = CalculatePatternBoundsForShape(tilingBounds, context);
            
            movey += this.CurrentContainerSize.Height.PointsValue;
            movey -= tilingBounds.Y.PointsValue;
            movey -= (step.Height.PointsValue * matrix.Components[3]); //scale y

            Point aspectOffset = GetAspectRatioForMeet(min, min, step, view, tilingBounds, context);
           
            movex += aspectOffset.X.PointsValue;
            movey += aspectOffset.Y.PointsValue;

            if (this.PatternOffset != Point.Empty)
            {
                var offsetX = EnsureAbsolute(this.PatternOffset.X, tilingBounds.Width);
                var offsetY = EnsureAbsolute(this.PatternOffset.Y, tilingBounds.Height);
                movex += offsetX.PointsValue;
                movey += offsetY.PointsValue;
            }
            
            
            matrix.SetTranslation(movex, movey);

            return matrix;
        }

        

        /// <summary>
        /// Calculates the offsets for the view to match the requested preserved aspect ratio when the 
        /// </summary>
        /// <param name="scaleX"></param>
        /// <param name="scaleY"></param>
        /// <param name="step"></param>
        /// <param name="view"></param>
        /// <param name="tilingBounds"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private Point GetAspectRatioForMeet(double scaleX, double scaleY, Size step, Rect view, Rect tilingBounds, ContextBase context)
        {
            Point result = Point.Empty;

            if (step.Width > view.Width)
            {
                switch (this.PatternAspectRatio.Align)
                {
                    case AspectRatioAlign.None:
                        //Do nothing
                        break;
                    case AspectRatioAlign.xMinYMin:
                    case AspectRatioAlign.xMinYMid:
                    case AspectRatioAlign.xMinYMax:
                        result.X = Unit.Empty;
                        break;
                    
                    case AspectRatioAlign.xMidYMin:
                    case AspectRatioAlign.xMidYMid:
                    case AspectRatioAlign.xMidYMax:
                        var half = (step.Width - view.Width) / 2.0;
                        half *= scaleX;
                        result.X = half;
                        break;
                    case AspectRatioAlign.xMaxYMin:
                    case AspectRatioAlign.xMaxYMid:
                    case AspectRatioAlign.xMaxYMax:
                        result.X = (step.Width - view.Width);
                        break;

                    default:

                        break;

                }
            }
            else if (step.Height > view.Height)
            {
                switch (this.PatternAspectRatio.Align)
                {
                    case AspectRatioAlign.None:
                        //Do nothing
                        break;
                    case AspectRatioAlign.xMinYMin:
                    case AspectRatioAlign.xMidYMin:
                    case AspectRatioAlign.xMaxYMin:
                        var offset = (step.Height - view.Height);
                        offset *= scaleY;
                        result.Y = offset;
                        break;
                    
                    case AspectRatioAlign.xMinYMid:
                    case AspectRatioAlign.xMidYMid:
                    case AspectRatioAlign.xMaxYMid:
                        var half = (step.Height - view.Height) / 2.0;
                        half *= scaleY;
                        result.Y = half;
                        break;
                    case AspectRatioAlign.xMinYMax:
                    case AspectRatioAlign.xMidYMax:
                    case AspectRatioAlign.xMaxYMax:
                        result.Y = Unit.Empty;
                        break;

                    default:

                        break;

                }
            }


            return result;
        }
        
        private Point GetAspectRatioForSlice(double scaleX, double scaleY, Size step, Rect view, Rect tilingBounds, ContextBase context)
        {
            Point result = Point.Empty;

            

            if (step.Width < this.PatternViewBox.Width) // we are scaling up the pattern (scaleX > scaleY)
            {
                var count = (tilingBounds.Width.PointsValue / scaleX) / step.Width.PointsValue;
                count = Math.Floor(count);
                var full = count * (step.Width.PointsValue * scaleX);
                var space = tilingBounds.Width.PointsValue - full;
                
                //Wide stretched patterns
                switch (this.PatternAspectRatio.Align)
                {
                    case AspectRatioAlign.None:
                        //Do nothing
                        break;
                    case AspectRatioAlign.xMinYMin:
                    case AspectRatioAlign.xMinYMid:
                    case AspectRatioAlign.xMinYMax:
                        //x is min so we are OK
                        result.X = Unit.Empty;
                        break;
                    
                    case AspectRatioAlign.xMidYMin:
                    case AspectRatioAlign.xMidYMid:
                    case AspectRatioAlign.xMidYMax:
                        var half = space / 2.0;
                        //the pattern should be in the middle
                        //var half = (step.Width - view.Width) / 2.0;
                        //half *= scaleX;
                        result.X = half / scaleX;
                        
                        break;
                    case AspectRatioAlign.xMaxYMin:
                    case AspectRatioAlign.xMaxYMid:
                    case AspectRatioAlign.xMaxYMax:
                        //pattern should be at the right
                        result.X = tilingBounds.Width.PointsValue - step.Width.PointsValue;
                        
                        break;

                    default:

                        break;

                }
            }
            
            //return result;
            
            if (true) // (scaleY > scaleX)
            {
                //High stretched result
                switch (this.PatternAspectRatio.Align)
                {
                    case AspectRatioAlign.None:
                        //Do nothing
                        break;
                    case AspectRatioAlign.xMinYMin:
                    case AspectRatioAlign.xMidYMin:
                    case AspectRatioAlign.xMaxYMin:
                        var offset = (step.Height - view.Height);
                        offset *= scaleY;
                        result.Y = offset;
                        break;
                    
                    case AspectRatioAlign.xMinYMid:
                    case AspectRatioAlign.xMidYMid:
                    case AspectRatioAlign.xMaxYMid:
                        var half = (step.Height - view.Height) / 2.0;
                        half *= scaleY;
                        result.Y = half;
                        break;
                    case AspectRatioAlign.xMinYMax:
                    case AspectRatioAlign.xMidYMax:
                    case AspectRatioAlign.xMaxYMax:
                        result.Y = Unit.Empty;
                        break;

                    default:

                        break;

                }
            }


            return result;
        }
    }
}