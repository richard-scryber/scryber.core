using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scryber.PDF.Native;
using Scryber.Components;
using Scryber.PDF.Layout;

namespace Scryber.PDF
{
    internal class PDFTraceLogLayoutEngineDocument : LayoutEngineDocument
    {

        private PDFFile _orig;

        protected PDFFile OriginalFile
        {
            get { return _orig; }
        }

        public PDFTraceLogLayoutEngineDocument(Document doc, PDFFile originalFile, IPDFLayoutEngine parent, PDF.PDFLayoutContext context)
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
            var layoutDoc = new PDFTraceLogLayoutDocument(this.Document, this, this.OriginalFile);
            var page = this.Document.GetPageStyle(string.Empty);
        

            if(null == page)
            {
                throw new InvalidOperationException("Cannot get the current page style from the document layout, this should always be set when we are laying out a page");
            }

            var size = page.CreatePageSize();
            layoutDoc.PushPageSize(string.Empty, size);

            return layoutDoc;
        }
    }

    
}
