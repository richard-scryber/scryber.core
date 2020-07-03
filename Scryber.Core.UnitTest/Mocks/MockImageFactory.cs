using System;
using Scryber.Drawing;

namespace Scryber.UnitTests.Mocks
{
    public class MockImageFactory : IPDFImageDataFactory
    {
        public MockImageFactory()
        {
        }

        public bool ShouldCache { get { return false; } }

        public PDFImageData LoadImageData(IPDFDocument document, IPDFComponent owner, string path)
        {
            throw new NotImplementedException();
        }
    }
}
