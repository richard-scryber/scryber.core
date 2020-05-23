using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scryber.Native;
using Scryber.Components;
using Scryber.Layout;

namespace Scryber
{
    internal class PDFTraceLogLayoutEngineDocument : LayoutEngineDocument
    {

        private PDFFile _orig;

        protected PDFFile OriginalFile
        {
            get { return _orig; }
        }

        public PDFTraceLogLayoutEngineDocument(PDFDocument doc, PDFFile originalFile, IPDFLayoutEngine parent, PDFLayoutContext context)
            : base(doc, parent, context)
        {
            this._orig = originalFile;
        }

        protected override void LayoutAllPages()
        {
            base.LayoutAllPages();
        }

        

        protected override PDFLayoutDocument CreateDocumentLayout()
        {
            return new PDFTraceLogLayoutDocument(this.Document, this, this.OriginalFile);
        }
    }

    
}
