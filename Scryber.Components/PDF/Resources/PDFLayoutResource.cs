using System;
using Scryber.PDF.Layout;
using Scryber.PDF.Native;

namespace Scryber.PDF.Resources
{
    public class PDFLayoutXObjectResource : PDFResource
    {

        private string _type;
        private string _key;
        private PDFLayoutXObject _layout;
        
        public override string ResourceType => _type;
        public override string ResourceKey => _key;

        public PDFLayoutXObject Layout => _layout;
        
        public PDFLayoutXObjectResource(string type, string key, PDFLayoutXObject layout) : base(ObjectTypes.CanvasXObject)
        {
            this._type = type;
            this._key = key;
            this._layout = layout ?? throw new ArgumentNullException(nameof(layout));
            this.Name = this._layout.OutPutName;
        }

        
        protected override PDFObjectRef DoRenderToPDF(ContextBase context, PDFWriter writer)
        {
            return Layout.RenderReference;
        }
    }
}