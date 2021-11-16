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
        : base(match, name, shouldCache)
        {
            this.CustomFactory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        protected override async Task<ImageData> DoLoadImageDataAsync(IDocument document, IComponent owner, string path)
        {
            return this.CustomFactory.LoadImageData(document, owner, path);
        }

        

        protected override ImageData DoDecodeImageData(Stream stream, IDocument document, IComponent owner, string path)
        {
            throw new NotSupportedException(
                "The custom factory only supports the primary interface method for document, owner and path");
        }
    }
}