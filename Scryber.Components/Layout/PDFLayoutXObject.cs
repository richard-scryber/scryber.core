using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Drawing;
using Scryber.Native;
using Scryber.Resources;

namespace Scryber.Layout
{
    /// <summary>
    /// Implements an Form XObject where the content is rendered into a separate 
    /// </summary>
    public class PDFLayoutXObject : PDFLayoutRun, IPDFResourceContainer
    {
        private static readonly double[] IdentityMatrix = new double[] { 1, 0, 0, 1, 0, 0 };

        private PDFLayoutRegion _childContainer;
        private PDFResourceList _resources;
        private PDFLayoutPage _page;

        public PDFLayoutXObject(PDFLayoutLine parent, PDFLayoutRegion childContainer, IPDFComponent owner) 
            :base(parent, owner as IPDFComponent)
        {
            this._childContainer = childContainer;
            this._resources = new PDFResourceList(this, false);
            this.SubType = "Form";
            this.Matrix = IdentityMatrix;
        }

        public double[] Matrix { get; set; }

        public string SubType { get; set; }

        public PDFPoint Location { get; private set; }

        public override PDFUnit Height
        {
            get { return this._childContainer.Height; }
        }

        public override PDFUnit Width
        {
            get { return this._childContainer.Width; }
        }

        public IPDFDocument Document
        {
            get { return this.Owner.Document; }
        }

        protected override void DoPushComponentLayout(PDFLayoutContext context, int pageIndex, PDFUnit xoffset, PDFUnit yoffset)
        {
            this._page = context.DocumentLayout.CurrentPage;
            this._childContainer.PushComponentLayout(context, pageIndex, xoffset, yoffset);
        }

        protected override bool DoClose(ref string msg)
        {
            if (this._childContainer.IsClosed == false)
                this._childContainer.Close();

            return base.DoClose(ref msg);
        }

        private PDFObjectRef _renderRef;

        protected override PDFObjectRef DoOutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            //Only do this once, but can be referenced from multiple places;
            if (null == _renderRef)
            {
                _renderRef = this.OutputContent(context, writer);
            }
            return _renderRef;
        }

        protected string GetMarkedContentType()
        {
            string type;
            switch (this.SubType)
            {
                default:
                    type = "Tx"; //TExt form input type
                    break;
            }

            return type;
        }

        private PDFObjectRef OutputContent(PDFRenderContext context, PDFWriter writer)
        {
            PDFObjectRef xObject = writer.BeginObject();
            IStreamFilter[] filters = (context.Compression == OutputCompressionType.FlateDecode) ? this._page.PageCompressionFilters : null;

            writer.BeginStream(xObject, filters);

            this.Location = context.Offset.Offset(0, this.Line.OffsetY);
            PDFSize origSpace = context.Space.Clone();
            PDFGraphics prevGraphics = context.Graphics;

            using (PDFGraphics g = this.CreateGraphics(writer, context.StyleStack, context))
            {
                context.Graphics = g;
                g.SaveGraphicsState();
                g.RestoreGraphicsState();

                context.Offset = Drawing.PDFPoint.Empty;

                string contentType = this.GetMarkedContentType();
                
                this._childContainer.OutputToPDF(context, writer);

            }
            context.Offset = this.Location;
            context.Space = origSpace;
            context.Graphics = prevGraphics;

            long len = writer.EndStream();
            writer.BeginDictionary();

            this.WriteXObjectDictionaryContent(context, writer, len, filters);

            writer.EndDictionary();
            writer.EndObject();

            return xObject;
        }

        private void WriteXObjectDictionaryContent(PDFRenderContext context, PDFWriter writer, long len, IStreamFilter[] filters)
        {
            writer.WriteDictionaryNameEntry("Type", "XObject");
            if (!string.IsNullOrEmpty(this.SubType))
                writer.WriteDictionaryNameEntry("Subtype", "Form");

            writer.BeginDictionaryEntry("Matrix");
            writer.WriteArrayRealEntries(this.Matrix);
            writer.EndDictionaryEntry();

            writer.BeginDictionaryEntry("BBox");
            writer.BeginArrayS();
            writer.WriteReal(0.0F);
            writer.WriteRealS(0.0F);
            writer.WriteRealS(this.Width.PointsValue);
            writer.WriteRealS(this.Height.PointsValue);
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

        protected virtual PDFGraphics CreateGraphics(PDFWriter writer, Styles.PDFStyleStack styles, PDFRenderContext context)
        {
            return PDFGraphics.Create(writer, false, this, DrawingOrigin.TopLeft, new PDFSize(this.Width, this.Height), context);
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
            IPDFResourceContainer parentRegister = this.GetParentResourceRegister();
            if (null == parentRegister)
                return source;
            else
                return parentRegister.MapPath(source);
            
        }

        protected virtual IPDFResourceContainer GetParentResourceRegister()
        {
            PDFLayoutItem parent = this.Parent;
            while (null != parent && !(parent is IPDFResourceContainer))
            {
                parent = parent.Parent;
            }
            return parent as IPDFResourceContainer;
        }

       
    }

    

}
