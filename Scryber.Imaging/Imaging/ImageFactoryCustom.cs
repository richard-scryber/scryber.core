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

        /// <summary>
        /// Loads image data from raw bytes by passing them to the wrapped handler.
        /// The base class only accepts its own image type, but a custom handler decides for itself
        /// which types and formats it supports, so the call is always forwarded.
        /// </summary>
        public override ImageData LoadImageData(IDocument document, IComponent owner, byte[] rawData, MimeType type)
        {
            return this.CustomFactory.LoadImageData(document, owner, rawData, type);
        }

        protected override ImageData DoLoadRawImageData(IDocument document, IComponent owner, byte[] rawData, MimeType type)
        {
            return this.CustomFactory.LoadImageData(document, owner, rawData, type);
        }

        /// <summary>
        /// Called with the content of a request for an image on a path this handler matches (e.g. from a RemoteFileRegistered requestor
        /// calling request.Callback with the stream). The content is read and passed to the wrapped handler to decode.
        /// As the type is not known for a custom path, the mime-type is empty and the handler should decide from the data.
        /// </summary>
        protected override ImageData DoDecodeImageData(Stream stream, IDocument document, IComponent owner, string path)
        {
            if (null == stream)
                throw new ArgumentNullException(nameof(stream));

            byte[] data;
            using (var copy = new MemoryStream())
            {
                stream.CopyTo(copy);
                data = copy.ToArray();
            }

            return this.CustomFactory.LoadImageData(document, owner, data, this.ImageType);
        }
    }
}