using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scryber.Resources
{
    public class PDFCombinedFontWidths : PDFFontWidths
    {

        public override bool IsEmpty
        {
            get { throw new NotImplementedException(); }
        }

        public override void RenderWidthsArrayToPDF(PDFContextBase context, PDFWriter writer)
        {
            throw new NotImplementedException();
        }

        public override char RegisterGlyph(char c)
        {
            throw new NotImplementedException();
        }
    }
}
