using System;
using Scryber.Drawing;
using Scryber.PDF;
using Scryber.PDF.Native;
using Scryber.Svg.Components;

namespace Scryber.Svg.Imaging
{

    public class SVGPDFImageData : ImageData
    {
        private PDFObjectRef _renderRef = null;
        private SVGCanvas _svgCanvas = null;
        
        public SVGPDFImageData(string source, SVGCanvas canvas, int w, int h) 
            : base(ObjectTypes.ImageData, source, w, h)
        {
            _svgCanvas = canvas ?? throw new ArgumentNullException(nameof(canvas));
        }

       

        public override PDFObjectRef Render(PDFName name, IStreamFilter[] filters, ContextBase context, PDFWriter writer)
        {
            if(_renderRef == null)
                _renderRef = this.DoRenderToPDF(name, filters, context, writer);
            return _renderRef;
        }

        protected virtual PDFObjectRef DoRenderToPDF(PDFName name, IStreamFilter[] filters, ContextBase context, PDFWriter writer)
        {
            return null;
        }

        public override void ResetFilterCache()
        {
            
        }
    }

}