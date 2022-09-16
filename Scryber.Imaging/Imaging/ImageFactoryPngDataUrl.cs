using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Scryber.Drawing;


namespace Scryber.Imaging
{
    /// <summary>
    /// Handles Gif images from a data url e.g. data:image/gif;base64,.....
    /// </summary>
    public class ImageFactoryPngDataUrl : ImageFactoryPng
    {
        private static readonly Regex GifDataMatch = new Regex("^\\s*data:image/png;", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
        private const string GifDataName = "Png inline data image factory";
        private const bool GifDataShouldCache = false;

        //prefix is "data:image/png;base64,"
        
        public ImageFactoryPngDataUrl() : this(GifDataMatch, GifDataName, GifDataShouldCache)
        {}
        
        
        protected ImageFactoryPngDataUrl(Regex match, string name, bool shouldCache)
            : base(match, name, shouldCache)
        {}

        //As we run syncronously from the data in the url - we do not need the proxy or resouce requestors
        
        public override ImageData LoadImageData(IDocument document, IComponent owner, string path)
        {
            return this.DoLoadImageDataAsync(document, owner, path).Result;
        }

        public override Task<ImageData> LoadImageDataAsync(IDocument document, IComponent owner, string path)
        {
            return this.DoLoadImageDataAsync(document, owner, path);
        }


        protected override Task<ImageData> DoLoadImageDataAsync(IDocument document, IComponent owner, string path)
        {
            var bin = Utilities.DataUrlHelper.ExtractBase64Data(path, "image/png");

            using (var ms = new MemoryStream(bin))
            {
                var data = DoDecodeImageData(ms, document, owner, path);
                return Task.FromResult(data);
            }
        }

        
    }
}