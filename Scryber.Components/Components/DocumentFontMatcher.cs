using System;
using Scryber.Drawing;
using Scryber.PDF.Resources;

namespace Scryber.Components
{
    public class DocumentFontMatcher
    {
        public Document OwnerDocument{
            get;
            private set;
        }

        public DocumentFontMatcher(Document document)
        {
            this.OwnerDocument = document ?? throw new ArgumentNullException(nameof(document));

        }

        public PDFFontResource GetFont(PDFFont font)
        {
            throw new NotImplementedException();
        }
    }
}
