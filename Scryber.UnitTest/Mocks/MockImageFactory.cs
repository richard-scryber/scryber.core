using System;
using Scryber.Drawing;


namespace Scryber.UnitTests.Mocks
{
    public class MockImageFactory : IPDFImageDataFactory
    {
            
        public bool ShouldCache { get { return false; } }

        public ImageData LoadImageData(IDocument document, IComponent owner, byte[] data, MimeType type)
        {
            throw new NotSupportedException("Implement the image loading from SixLabors Image library");
        }

        public ImageData LoadImageData(IDocument document, IComponent owner, string path)
        {
            throw new NotSupportedException("Implement the image loading from SixLabors Image library");
            
        }
    }
}
