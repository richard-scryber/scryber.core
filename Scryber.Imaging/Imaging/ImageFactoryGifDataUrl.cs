using System;
using System.IO;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Scryber.Drawing;
using SixLabors.ImageSharp;

namespace Scryber.Imaging
{
    /// <summary>
    /// Handles Gif images from a data url e.g. data:image/gif;base64,.....
    /// </summary>
    public class ImageFactoryGifDataUrl : ImageFactoryGif
    {
        private static readonly Regex GifDataMatch = new Regex("^\\s*data:image/gif;", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
        private const string GifDataName = "Gif inline data image factory";
        private const bool GifDataShouldCache = false;

        private const string DataImagePrefix = "data:image/gif;base64,";
        
        public ImageFactoryGifDataUrl() : this(GifDataMatch, GifDataName, GifDataShouldCache)
        {}
        
        
        public ImageFactoryGifDataUrl(Regex match, string name, bool shouldCache)
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
            var bin = Utilities.DataUrlHelper.ExtractBase64Data(path, "image/gif");
            
            using (var ms = new MemoryStream(bin))
            {
                var data = DoDecodeImageData(ms, document, owner, path);
                return Task.FromResult(data);
            }
        }
    }
}