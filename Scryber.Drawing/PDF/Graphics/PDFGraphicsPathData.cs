using System;
using Scryber.PDF;
using Scryber.PDF.Native;
using Scryber.PDF.Resources;
using Scryber.OpenType.SubTables;
using Scryber.Drawing;

namespace Scryber.PDF.Graphics
{
    public abstract class PDFGraphicsPathData : PDFObject
    {

        private PDFSize _size;

        public PDFSize Size
        {
            get { return this._size; }
            set { this._size = value; }
        }

        public PDFGraphicsPathData(PDFSize size) : base(ObjectTypes.GraphicsPathData)
        {
            this._size = size;
        }


        internal PDFObjectRef Render(PDFName name, PDFContextBase context, PDFWriter writer)
        {
            if (context.ShouldLogDebug)
                context.TraceLog.Begin(TraceLevel.Message, "Path Data", "Rendering path data for '" + name.ToString() + "'");

            PDFObjectRef renderref = writer.BeginObject(name.Value);

            writer.BeginDictionaryS();
            writer.WriteDictionaryNameEntry("Name", name.Value);
            writer.WriteDictionaryNameEntry("Type", "XObject");
            writer.WriteDictionaryNameEntry("Subtype", "Image");

            RenderPathInformation(context, writer, renderref);

            int length = this.RenderPathData(context, writer, renderref);

            writer.WriteDictionaryNumberEntry("Length", length);
            writer.EndDictionary();
            //End of add

            writer.EndObject();

            if (context.ShouldLogDebug)
                context.TraceLog.End(TraceLevel.Message, "Path Data", "Completed render of the path data for '" + name.ToString());
            else
                context.TraceLog.Add(TraceLevel.Message, "Path Data", "Rendered the path data for '" + name.ToString());


            return renderref;
        }

        protected virtual void RenderPathInformation(PDFContextBase context, PDFWriter writer, PDFObjectRef xobj)
        {

            writer.BeginDictionaryEntry("BBox");
            writer.BeginArray();
            writer.WriteArrayRealEntries(0.0, 0.0, _size.Width.PointsValue, _size.Height.PointsValue);
            writer.EndArray();
            writer.EndDictionaryEntry();

        }

        protected abstract int RenderPathData(PDFContextBase context, PDFWriter writer, PDFObjectRef xobj);
    }
}
