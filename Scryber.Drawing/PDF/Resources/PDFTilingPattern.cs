using System.Runtime.InteropServices;
using Scryber.Drawing;
using Scryber.PDF.Native;
using System;

namespace Scryber.PDF.Resources
{
    /// <summary>
    /// Base abstract tiling pattern class
    /// </summary>
    public abstract class PDFTilingPattern : PDFPattern, IResourceContainer
    {
        
        #region public IPDFDocument Document

        public IDocument Document
        {
            get { return this.OwningComponent.Document; }
        }
        

        #endregion
        
        #region public PDFResourceList Resources {get; private set;}

        /// <summary>
        /// Gets the list of resources used by this tiling pattern
        /// </summary>
        public PDFResourceList Resources
        {
            get;
            private set;
        }

        #endregion
        
        #region public PatternPaintType PaintType {get;set;}

        /// <summary>
        /// Gets or sets the paint type for this pattern
        /// </summary>
        public PatternPaintType PaintType
        {
            get;
            set;
        }

        #endregion

        #region public PatternTilingType TilingType {get;set;}

        /// <summary>
        /// Gets or sets the Tiling type for this pattern
        /// </summary>
        public PatternTilingType TilingType
        {
            get;
            set;
        }

        #endregion

        #region public PDFPoint Start {get;set;}

        /// <summary>
        /// Gets or sets the offset from the bottom left corner of the
        /// page (0,0) that the pattern repeats (top left of the pattern).
        /// </summary>
        public Point Start
        {
            get;
            set;
        }

        #endregion

        #region public PDFSize Step {get;set;}

        /// <summary>
        /// Gets or sets the distance between the start of each tile
        /// </summary>
        public Size Step
        {
            get;
            set;
        }

        #endregion

        

        /// <summary>
        /// Protected 
        /// </summary>
        /// <param name="container"></param>
        /// <param name="fullkey"></param>
        protected PDFTilingPattern(IComponent container, string fullkey)
            : base(container, PatternType.TilingPattern, fullkey)
        {
            this.TilingType = PatternTilingType.NoDistortion;
            this.PaintType = PatternPaintType.ColoredTile;
            this.Resources = new PDFResourceList(this);
        }


        public virtual Rect CalculatePatternBoundingBox(ContextBase context)
        {
            return new Rect(this.Start.X, this.Start.Y, this.Step.Width, this.Step.Height);
        }

        public virtual Size CalculateStepSize(ContextBase context)
        {
            return new Size(this.Step.Width, this.Step.Height);
        }

        protected virtual Graphics.PDFTransformationMatrix CalculateTransformMatrix(ContextBase context)
        {
            return Graphics.PDFTransformationMatrix.Identity();
        }
        
        protected override PDFObjectRef DoRenderToPDF(ContextBase context, PDFWriter writer)
        {
            Rect bbox = this.CalculatePatternBoundingBox(context);
            Size step = this.CalculateStepSize(context);
            Scryber.PDF.Graphics.PDFTransformationMatrix matrix = this.CalculateTransformMatrix(context);
            
            var oref = writer.BeginObject(this.Name.Value);
            writer.BeginDictionary();
            writer.WriteDictionaryNameEntry("Type", "Pattern");
            writer.WriteDictionaryNumberEntry("PatternType", (int)this.PatternType);
            writer.WriteDictionaryNumberEntry("PaintType", (int)this.PaintType);
            writer.WriteDictionaryNumberEntry("TilingType", (int)this.TilingType);
            
            
            
            writer.BeginDictionaryEntry("BBox");
            var offset = bbox.Location;
            var size = bbox.Size;
            
            writer.WriteArrayRealEntries(true,
                offset.X.PointsValue,
                offset.Y.PointsValue,
                offset.X.PointsValue + size.Width.PointsValue,
                offset.Y.PointsValue + size.Height.PointsValue);
            
            
            writer.EndDictionaryEntry();
            writer.WriteDictionaryRealEntry("XStep", step.Width.PointsValue);
            writer.WriteDictionaryRealEntry("YStep", step.Height.PointsValue);
            
            if (null != matrix && matrix.IsIdentity == false)
            {
                writer.BeginDictionaryEntry("Matrix");
                var comp = this.TransformationMatrix.Components;
                writer.WriteArrayRealEntries(true, comp);
                writer.EndDictionaryEntry();
            }
            
            writer.BeginStream(oref);
            
            try
            {
                this.RenderTileContents(context, writer);
            }
            catch (Exception e)
            {
                context.TraceLog.Add(TraceLevel.Error, "Pattern", "Could not output the Pattern Tile Contents as " + e.Message, e);
                
                //if we are strict then re-throw.
                if (context.Conformance == ParserConformanceMode.Strict)
                    throw;
            }
            finally
            {
                var len = writer.EndStream();
                
                if(null != this.Resources)
                {
                    var rsrcOref = this.Resources.WriteResourceList(context, writer);
                
                    if (null != rsrcOref)
                        writer.WriteDictionaryObjectRefEntry("Resources", rsrcOref);
                }
                
                
                writer.WriteDictionaryNumberEntry("Length", len);
                writer.EndDictionary();
                writer.EndObject();
            }
            
            return oref;
            
        }

        protected virtual PDFObjectRef RenderTileContents(ContextBase context, PDFWriter writer)
        {
            return null;
        }
        
        
        #region IPDFResourceContainer Members



        public string MapPath(string source)
        {
            return this.Container.MapPath(source);
        }

        string IResourceContainer.Register(ISharedResource rsrc)
        {
            if (rsrc is PDFResource pdfrsrc)
                return this.Register(pdfrsrc).Value;
            else
                throw new InvalidCastException("PDFImageTilingPatterns can only use PDFResources");
        }

        protected PDFName Register(PDFResource rsrc)
        {
            if (null == rsrc.Name || string.IsNullOrEmpty(rsrc.Name.Value))
            {
                string name = this.Document.GetIncrementID(rsrc.Type);
                rsrc.Name = (PDFName)name;
            }
            if (this.Container is IComponent)
                rsrc.RegisterUse(this.Resources, (IComponent)this.Container);
            else
            {
                rsrc.RegisterUse(this.Resources, this.Document);
            }
            //    throw new InvalidCastException(string.Format(Errors.CouldNotCastObjectToType, "IPDFComponent", this.Container.GetType()));

            return rsrc.Name;
        }

        
        #endregion

    }
}