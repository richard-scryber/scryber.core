using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scryber.PDF.Parsing
{
    [Serializable]
    public class PDFDocumentStructureException : ApplicationException
    {
        public PDFDocumentStructureException() { }
        public PDFDocumentStructureException(string message) : base(message) { }
        public PDFDocumentStructureException(string message, Exception inner) : base(message, inner) { }
        protected PDFDocumentStructureException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
