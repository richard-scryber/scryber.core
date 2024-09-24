using System;
using Newtonsoft.Json.Linq;
using Scryber.PDF.Layout;
using Scryber.PDF.Native;
using Scryber.Drawing;

namespace Scryber.PDF.Resources
{
    public class PDFLayoutXObjectResource : PDFResource
    {

        private string _type;
        private string _key;
        private PDFLayoutPositionedRegionRun _layout;
        private PDFXObjectRenderer _renderer;
        
        public override string ResourceType => _type;
        public override string ResourceKey => _key;

        public PDFLayoutPositionedRegionRun Layout => _layout;

        public PDFXObjectRenderer Renderer => _renderer;
        
        public Rect BoundingBox { get; set; }
            
        public PDFTransformationMatrix ViewMatrix { get; set; }
        
        public PDFLayoutXObjectResource(string type, string key, PDFLayoutPositionedRegionRun layout) : base(ObjectTypes.CanvasXObject)
        {
            this._type = type;
            this._key = key;
            this._layout = layout ?? throw new ArgumentNullException(nameof(layout));
            this.Name = this._layout.OutputName;
        }

        public PDFLayoutXObjectResource(string type, string key, PDFXObjectRenderer renderer) : base(ObjectTypes.CanvasXObject)
        {
            this._type = type;
            this._key = key;
            this._renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
            this.Name = this._renderer.OutputName;
        }
        
        protected override PDFObjectRef DoRenderToPDF(ContextBase context, PDFWriter writer)
        {
            if (null == this._layout)
                return this._renderer.RenderReference;
            else
                return this._layout.RenderReference;
        }
    }
}