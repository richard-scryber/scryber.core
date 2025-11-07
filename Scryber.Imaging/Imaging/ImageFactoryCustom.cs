using System;
using System.IO;
using Scryber.Drawing;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Scryber.Imaging
{
    public class ImageFactoryCustom : ImageFactoryBase
    {
        private IPDFImageDataFactory CustomFactory { get; }
        
        
        
        public ImageFactoryCustom(Regex match, string name, bool shouldCache, IPDFImageDataFactory factory)
        : base(match, MimeType.Empty, name, shouldCache)
        {
            this.CustomFactory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        protected override Task<ImageData> DoLoadImageDataAsync(IDocument document, IComponent owner, string path)
        {
            var data = this.CustomFactory.LoadImageData(document, owner, path);
            return Task.FromResult(data);
        }

        protected override ImageData DoLoadRawImageData(IDocument document, IComponent owner, byte[] rawData, MimeType type)
        {
            throw new NotSupportedException(
                "The custom factory only supports the primary interface method for document, owner and path");
        }


        protected override ImageData DoDecodeImageData(Stream stream, IDocument document, IComponent owner, string path)
        {
            throw new NotSupportedException(
                "The custom factory only supports the primary interface method for document, owner and path");
        }
    }
}