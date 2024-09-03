/*  Copyright 2012 PerceiveIT Limited
 *  This file is part of the Scryber library.
 *
 *  You can redistribute Scryber and/or modify 
 *  it under the terms of the GNU Lesser General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 * 
 *  Scryber is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 * 
 *  You should have received a copy of the GNU Lesser General Public License
 *  along with Scryber source code in the COPYING.txt file.  If not, see <http://www.gnu.org/licenses/>.
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.PDF.Graphics;
using Scryber.PDF.Resources;
using Scryber.PDF.Native;

namespace Scryber.PDF.Layout
{
    public class PDFLayoutPositionedRegionRun : PDFLayoutRun, IResourceContainer
    {
        /// <summary>
        /// Gets the RelativeRegion associated with this
        /// </summary>
        public PDFLayoutRegion Region
        {
            get;
            private set;
        }

        public bool IsFloating
        {
            get; set;
        }

        public bool RenderAsXObject
        {
            get;
            set;
        }
        
        

        protected PDFPositionOptions PositionOptions
        {
            get; 
            set;
        }

        protected Point Location
        {
            get;
            set;
        }
        
        public override Unit Height
        {
            get { return Unit.Zero; }
        }

        private Unit _width = Unit.Zero;

        public override Unit Width
        {
            get { return _width; }
        }

        protected PDFLayoutPage Page
        {
            get;
            set;
        }

        public PDFTransformationMatrix Matrix
        {
            get; 
            set;
        }

        public PDFResourceList Resources
        {
            get;
            protected set;
        }

        public Rect? ClipRect
        {
            get;
            set;
        }

        public PDFName OutputName
        {
            get;
            set;
        }
        
        /// <summary>
        /// Gets the Object Reference for this run if rendered as an xobject
        /// </summary>
        public PDFObjectRef RenderReference { get; protected set; }
        
        /// <summary>
        /// Gets the Bounding Box (BBox) for this run if rendered as an XObject
        /// </summary>
        public Rect RenderBoundingBox { get; protected set; }

        public IDocument Document
        {
            get { return this.Owner.Document; }
        }

        public PDFLayoutPositionedRegionRun(PDFLayoutRegion region, PDFLayoutLine line, IComponent owner, PDFPositionOptions pos)
            : base(line, owner)
        {
            this.Region = region;
            this.PositionOptions = pos;
        }

        public override void SetOffsetY(Unit y)
        {
            //Do Nothing
        }

        

        protected override void DoPushComponentLayout(PDFLayoutContext context, int pageIndex, Unit xoffset, Unit yoffset)
        {
            if (this.RenderAsXObject)
            {
                xoffset = 0;
                yoffset = 0;
                
            }

            this.Region.PushComponentLayout(context, pageIndex, xoffset, yoffset);
            this.Page = context.DocumentLayout.CurrentPage;
        }

        protected override bool DoClose(ref string msg)
        {
            return this.Region.Close();
        }

        protected override Native.PDFObjectRef DoOutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            Scryber.Drawing.Point oldOffset = context.Offset;

            if (this.Region.PositionMode == PositionMode.Fixed)
            {
                context.Offset = Point.Empty;
            }
            else if(this.Region.PositionMode == PositionMode.Absolute)
            {
                context.Offset = Point.Empty;
            }

            Native.PDFObjectRef oref;
            if (this.RenderAsXObject)
            {
                oref = this.OutputAsXObject(context, writer);
                this.RenderReference = oref;
            }
            else
            {
                oref = this.Region.OutputToPDF(context, writer);
            }

            context.Offset = oldOffset;
            return oref;
        }

        protected virtual Native.PDFObjectRef OutputAsXObject(PDFRenderContext context, PDFWriter writer)
        {
            // set up the new xObject
            
            var xObj = writer.BeginObject();
            IStreamFilter[] filters = (context.Compression == OutputCompressionType.FlateDecode)
                ? this.Page.PageCompressionFilters
                : null;
            writer.BeginStream(xObj, filters);

            this.Location = context.Offset.Offset(0, this.Line.OffsetY);
            var prevSpace = context.Space;
            var prevGraphics = context.Graphics;

            using (var g = this.CreateXObjectGraphics(writer, context.StyleStack, context))
            {
                context.Graphics = g;
                context.Offset = Point.Empty;
                var prev = context.RenderMatrix;
                
                //store the offsets so that they can be used when calculating the render bounds.
                if(null == context.RenderMatrix)
                    context.RenderMatrix = PDFTransformationMatrix.Identity();
                else
                {
                    context.RenderMatrix = context.RenderMatrix.Clone();
                }
                context.RenderMatrix.SetTranslation(this.Location.X, this.Location.Y);
                
                this.Region.OutputToPDF(context, writer);

                context.RenderMatrix = prev;
            }

            //write the xObjectContent
            var len = writer.EndStream();
            writer.BeginDictionary();
            this.WriteXObjectDictionaryContent(context, writer, len, filters);
            writer.EndDictionary();
            writer.EndObject();
            

            

            //restore graphics
            context.Offset = this.Location;
            context.Graphics = prevGraphics;
            context.Space = prevSpace;
            
            this.WriteXObjectDo(context, writer);
            
            return xObj;

            
        }
        
        protected virtual bool WriteXObjectDo(PDFRenderContext context, PDFWriter writer)
        {
            
            if (null != this.OutputName)
            {

                
                context.Graphics.SaveGraphicsState();

                var x = context.Offset.X.RealValue;
                x = context.Graphics.GetXPosition(x);

                var y = (context.Offset.Y + this.Height).RealValue;
                y = context.Graphics.GetYPosition(y);




                //Set the transformation matrix for the current offset independent of the matrix for the view-box

                var origin = new Point(0, context.PageSize.Height);
                origin.Y -= this.Region.Height;
                //Y origin takes account of the margins as from the bottom up (so need to be included)
                origin.Y -= this.Location.Y + (this.PositionOptions.Margins.Top + this.PositionOptions.Margins.Bottom);
                //X origin does not need margins as from the left.
                origin.X += this.Location.X;
                
                if(!origin.IsZero)
                {
                    var moveMatrix = new PDFTransformationMatrix();
                    moveMatrix.SetTranslation(origin.X, origin.Y);
                    context.Graphics.SetTransformationMatrix(moveMatrix, true, true);
                    
                }

                context.Graphics.PaintXObject(this.OutputName);

                context.Graphics.RestoreGraphicsState();

                
                return true;
            }
            else
                return false;
        }

        protected virtual Rect GetBoundingBox(PDFRenderContext context)
        {
            Rect vp = Rect.Empty;
            if (this.PositionOptions.ViewPort.HasValue)
            {
                vp = this.PositionOptions.ViewPort.Value;
            }
            else
            {
                //Bounding box includes any margins.
                vp = new Rect(
                    Unit.Empty, Unit.Empty, 
                    this.Region.Width + this.PositionOptions.Margins.Left + this.PositionOptions.Margins.Right, 
                    this.Region.Height + this.PositionOptions.Margins.Top + this.PositionOptions.Margins.Bottom);
            }
            
            return vp;
        }
        
        
        private void WriteXObjectDictionaryContent(PDFRenderContext context, PDFWriter writer, long len, IStreamFilter[] filters)
        {
            writer.WriteDictionaryNameEntry("Type", "XObject");
            writer.WriteDictionaryNameEntry("Subtype", "Form");

            if (null != this.Matrix)
            {
                writer.BeginDictionaryEntry("Matrix");
                writer.WriteArrayRealEntries(this.Matrix.Components);
                writer.EndDictionaryEntry();
            }

            var bbox = this.GetBoundingBox(context);
            
            writer.BeginDictionaryEntry("BBox");
            writer.BeginArrayS();
            writer.WriteReal(bbox.X.PointsValue);
            writer.WriteRealS(bbox.Y.PointsValue);
            writer.WriteRealS(bbox.Width.PointsValue);
            writer.WriteRealS(bbox.Height.PointsValue);

            writer.EndArray();
            writer.EndDictionaryEntry();

            this.RenderBoundingBox = bbox;
            
            if (null != this.Resources)
            {
                var res = this.Resources.WriteResourceList(context, writer);
                writer.WriteDictionaryObjectRefEntry("Resources", res);
            }

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

        protected virtual PDFGraphics CreateXObjectGraphics(PDFWriter writer, Styles.StyleStack styles, PDFRenderContext context)
        {
            Size sz;
            
            if(this.PositionOptions.ViewPort.HasValue)
            {
                sz = this.PositionOptions.ViewPort.Value.Size;
            }
            else
            {
                sz = new Size(this.Region.Width + this.PositionOptions.Margins.Left + this.PositionOptions.Margins.Right, 
                    this.Region.Height + this.PositionOptions.Margins.Top + this.PositionOptions.Margins.Bottom);
            }
            return PDFGraphics.Create(writer, false, this, DrawingOrigin.TopLeft, sz, context);
        }
        
        //
        // IResourceContainer
        //
        
        string IResourceContainer.Register(ISharedResource rsrc)
        {
            return this.Register((PDFResource)rsrc).Value;
        }

        public PDFName Register(PDFResource rsrc)
        {
            if (this.RenderAsXObject)
            {
                if (null == rsrc.Name || string.IsNullOrEmpty(rsrc.Name.Value))
                {
                    string name = this.Page.Document.DocumentComponent.GetIncrementID(rsrc.Type);
                    rsrc.Name = (PDFName)name;
                }

                if (null == this.Resources)
                    this.Resources = new PDFResourceList(this, false);

                rsrc.RegisterUse(this.Resources, this.Owner);
                return rsrc.Name;
            }
            else
            {
                var parent = this.GetParentResourceRegister();
                if (null == parent)
                    throw new InvalidOperationException(
                        "This positioned run is not an xObject, but does not have a parent resource container to register resources with");
                return (PDFName)parent.Register(rsrc);
            }
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
