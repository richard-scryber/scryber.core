using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Scryber.Drawing;
using Scryber.OpenType;
using SixLabors.ImageSharp;

namespace Scryber.Imaging
{
    /// <summary>
    /// Handles Gif images from a data url e.g. data:image/gif;base64,.....
    /// </summary>
    public class ImageFactoryJpegDataUrl : ImageFactoryJpeg
    {
        private static readonly Regex JpegDataMatch = new Regex("^\\s*data:image/jpeg;", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
        private const string JpegDataName = "Jpeg inline data image factory";
        private const bool JpegDataShouldCache = false;

        
        public ImageFactoryJpegDataUrl() : this(JpegDataMatch, JpegDataName, JpegDataShouldCache)
        {}
        
        
        public ImageFactoryJpegDataUrl(Regex match, string name, bool shouldCache)
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
            var bin = Utilities.DataUrlHelper.ExtractBase64Data(path, "image/jpeg");
            
            using (var ms = new MemoryStream(bin))
            {
                var data = DoDecodeImageData(ms, document, owner, path);
                return Task.FromResult(data);
            }
        }
    }
}