using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Scryber.Drawing;
using Scryber.PDF.Native;
using Scryber.PDF.Resources;
using Scryber.PDF.Graphics;

namespace Scryber.PDF.Layout
{
    /// <summary>
    /// Implements an Form XObject where the content is rendered into a separate 
    /// </summary>
    public class PDFLayoutXObjectRun : PDFLayoutRun, IResourceContainer
    {
        

        private PDFLayoutRegion _childContainer;
        private PDFResourceList _resources;
        private PDFLayoutPage _page;
        private PDFPositionOptions _position;

        public PDFLayoutPage Page
        {
            get { return _page; }
            set { _page = value; }
        }

        public PDFLayoutXObjectRun(PDFLayoutLine parent, PDFLayoutRegion childContainer, PDFPositionOptions position, IComponent owner) 
            :base(parent, owner as IComponent)
        {
            this._childContainer = childContainer;
            this._resources = (owner is IResourceContainer container) ? container.Resources : new PDFResourceList(this, false);
            this.SubType = "Form";
            this.Matrix = PDFTransformationMatrix.Identity();
            this._position = position;
            this.DrawingOrigin = DrawingOrigin.BottomLeft;
        }

        public PDFLayoutRegion ChildContainer
        {
            get { return _childContainer; }
        }

        public PDFTransformationMatrix Matrix { get; set; }

        public Rect? ClipRect { get; set; }

        public DrawingOrigin DrawingOrigin { get; set; }

        public string SubType { get; set; }

        public Point Location { get; set; }

        public PDFPositionOptions PositionOptions { get { return _position; } }

        public PDFResourceList Resources
        {
            get { return _resources; }
        }

        public override Unit OffsetX
        {
            get { return this._offsetX; }
        }

        private Unit? _explicitH;
        private Unit? _explicitW;

        private Unit _offsetY = Unit.Zero;
        private Unit _offsetX = Unit.Zero;

        /// <summary>
        /// Baseline/vertical-align offset assigned by the containing line's
        /// EnsureAllRunsOnSameLevel/AlignBlocksFromBaseline pass, mirroring
        /// PDFLayoutInlineBlockRun - previously silently discarded via the no-op base
        /// SetOffsetY, which is why every field sat pinned to the top of its line.
        /// </summary>
        public override void SetOffsetY(Unit y)
        {
            this._offsetY = y;
        }

        public void SetOffsetX(Unit x)
        {
            this._offsetX = x;
        }

        public override Unit Height
        {
            get { return _explicitH.HasValue ? _explicitH.Value : this._childContainer.Height; }
            
        }

        public override Unit Width
        {
            get { return _explicitW.HasValue ? _explicitW.Value :  this._childContainer.Width; }
            
        }

        public IDocument Document
        {
            get { return this.Owner.Document; }
        }

        #region public PDFObjectRef RenderReference {get;}

        private PDFObjectRef _renderRef;

        public PDFObjectRef RenderReference
        {
            get { return _renderRef; }
        }

        #endregion

        
        protected override void DoPushComponentLayout(PDFLayoutContext context, int pageIndex, Unit xoffset, Unit yoffset)
        {
            this._page = context.DocumentLayout.CurrentPage;

            //Mirrors PDFLayoutInlineBlockRun.DoPushComponentLayout - the incoming yoffset isn't
            //meaningful for a positioned-region-based run; the real vertical position is the
            //line's own offset plus whatever baseline/vertical-align offset was assigned to us.
            yoffset = this.Line.OffsetY;
            if (this._offsetY != Unit.Zero)
                yoffset += this._offsetY;

            this._childContainer.PushComponentLayout(context, pageIndex, xoffset, yoffset);
        }

        protected override bool DoClose(ref string msg)
        {
            if (this._childContainer.IsClosed == false)
                this._childContainer.Close();

            return base.DoClose(ref msg);
        }

        
        
        /// <summary>
        /// If set then this xObject will be rendered to the current stream with a /name Do operation.
        /// </summary>
        public PDFName OutPutName { get; set; }


        /// <summary>
        /// This will render the transformation matrix and then the XObject name operation.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        protected virtual bool OutputDrawingContent(PDFRenderContext context, PDFWriter writer)
        {
            
            if (null != this.OutPutName)
            {

                context.Graphics.SaveGraphicsState();

                var x = context.Offset.X.RealValue;
                x = context.Graphics.GetXPosition(x);

                var y = (context.Offset.Y + this.Height).RealValue;
                y = context.Graphics.GetYPosition(y);




                //Set the transformation matrix for the current offset independent of the matrix for the view-box

                var origin = new Point(0, context.PageSize.Height);
                origin.Y -= this.Height;
                origin.Y -= this.Location.Y;

                origin.X += this.Location.X;
                
                if(!origin.IsZero)
                {
                    var moveMatrix = new PDFTransformationMatrix();
                    moveMatrix.SetTranslation(origin.X, origin.Y);
                    context.Graphics.SetTransformationMatrix(moveMatrix, true, true);
                }

                context.Graphics.PaintXObject(this.OutPutName);

                context.Graphics.RestoreGraphicsState();

                
                return true;
            }
            else
                return false;
        }

        public virtual bool ResetOutputReference()
        {
            if (null != this._renderRef)
            {
                this._renderRef = null;
                return true;
            }
            return false;
        }

        public void SetOutputSize(Unit? width, Unit? height)
        {
            this._explicitW = width;
            this._explicitH = height;
        }

        protected override PDFObjectRef DoOutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            //Only do this once, but can be referenced from multiple places;
            

            if (null == _renderRef)
            {
                _renderRef = this.OutputContent(context, writer);
            }

            OutputDrawingContent(context, writer);

            return _renderRef;
        }

        
        /// <summary>
        /// This will render the actual content of the XObject graphical content in a new object reference.
        /// This can then be referred to.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        private PDFObjectRef OutputContent(PDFRenderContext context, PDFWriter writer)
        {
            if(this.ChildContainer.ExcludeFromOutput)
                return null;
            
            Point origOffset = context.Offset;
            Size origSpace = context.Space.Clone();
            PDFGraphics prevGraphics = context.Graphics;
            
            
            PDFObjectRef xObject = writer.BeginObject();
            IStreamFilter[] filters = (context.Compression == OutputCompressionType.FlateDecode) ? this._page.PageCompressionFilters : null;

            writer.BeginStream(xObject, filters);
            

            this.Location = context.Offset.Offset(this._offsetX, this.Line.OffsetY + this._offsetY);

            using (PDFGraphics g = this.CreateGraphics(writer, context.StyleStack, context))
            {
                context.Graphics = g;
                g.SaveGraphicsState();
                g.RestoreGraphicsState();

                context.Offset = Drawing.Point.Empty;

                this._childContainer.OutputToPDF(context, writer);

            }
            

            long len = writer.EndStream();
            writer.BeginDictionary();

            this.WriteXObjectDictionaryContent(context, writer, len, filters);

            writer.EndDictionary();
            writer.EndObject();
            
            
            context.Offset = origOffset;
            context.Space = origSpace;
            context.Graphics = prevGraphics;

            return xObject;
        }

        private void WriteXObjectDictionaryContent(PDFRenderContext context, PDFWriter writer, long len, IStreamFilter[] filters)
        {
            writer.WriteDictionaryNameEntry("Type", "XObject");
            if (!string.IsNullOrEmpty(this.SubType))
                writer.WriteDictionaryNameEntry("Subtype", "Form");

            writer.BeginDictionaryEntry("Matrix");
            writer.WriteArrayRealEntries( this.Matrix.Components);
            writer.EndDictionaryEntry();

            writer.BeginDictionaryEntry("BBox");
            writer.BeginArrayS();

            if (this._position.ViewPort.HasValue)
            {
                Rect vp = this._position.ViewPort.Value;
                writer.WriteReal(vp.X.PointsValue);
                writer.WriteRealS(vp.Y.PointsValue);
                writer.WriteRealS(vp.Width.PointsValue);
                writer.WriteRealS(vp.Height.PointsValue);
            }
            else
            {
                writer.WriteReal(0.0F);
                writer.WriteRealS(0.0F);
                writer.WriteRealS(this._childContainer.Width.PointsValue);
                writer.WriteRealS(this._childContainer.Height.PointsValue);
            }
            writer.EndArray();
            writer.EndDictionaryEntry();

            
            PDFObjectRef res = this._resources.WriteResourceList(context, writer);
            if (null != res)
                writer.WriteDictionaryObjectRefEntry("Resources", res);

            if (null != filters && filters.Length > 0)
            {
                writer.BeginDictionaryEntry("Length");
                writer.WriteNumberS(len);
                writer.EndDictionaryEntry();
                writer.BeginDictionaryEntry("Filter");
                writer.BeginArray();

                foreach (IStreamFilter filter in filters)
                {
                    writer.BeginArrayEntry();
                    writer.WriteName(filter.FilterName);
                    writer.EndArrayEntry();
                }
                writer.EndArray();
                writer.EndDictionaryEntry();
            }
            else
            {
                writer.BeginDictionaryEntry("Length");
                writer.WriteNumberS(len);
                writer.EndDictionaryEntry();
            }
        }

        protected virtual PDFGraphics CreateGraphics(PDFWriter writer, Styles.StyleStack styles, PDFRenderContext context)
        {
            var sz = new Size(this._childContainer.Width, this._childContainer.Height);
            if(this._position.ViewPort.HasValue)
            {
                sz = this._position.ViewPort.Value.Size;
            }
            return PDFGraphics.Create(writer, false, this, DrawingOrigin.TopLeft, sz, context);
        }

        string IResourceContainer.Register(ISharedResource rsrc)
        {
            return this.Register((PDFResource)rsrc).Value;
        }

        public PDFName Register(PDFResource rsrc)
        {
            if (null == rsrc.Name || string.IsNullOrEmpty(rsrc.Name.Value))
            {
                string name = this.Document.GetIncrementID(rsrc.Type);
                rsrc.Name = (PDFName)name;
            }
            rsrc.RegisterUse(this._resources, this.Owner);
            return rsrc.Name;
        }

        public string MapPath(string source)
        {
            IResourceContainer parentRegister = this.GetParentResourceRegister();
            if (null == parentRegister)
                return source;
            else
                return parentRegister.MapPath(source);
            
        }

        protected virtual IResourceContainer GetParentResourceRegister()
        {
            PDFLayoutItem parent = this.Parent;
            while (null != parent && !(parent is IResourceContainer))
            {
                parent = parent.Parent;
            }
            return parent as IResourceContainer;
        }

       
    }

    

}
