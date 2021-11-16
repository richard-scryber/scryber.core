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


        protected override async Task<ImageData> DoLoadImageDataAsync(IDocument document, IComponent owner, string path)
        {
            if (path.StartsWith("data:", StringComparison.Ordinal))
                path = path.Substring(5);
            else
                throw new FormatException("The data image url did not start with 'data:'");

            var mimeIndex = path.IndexOf(";", StringComparison.Ordinal);
            if (mimeIndex < 0)
                throw new FormatException("The mime type for the data image could not be determined");
            var mimeType = path.Substring(0, mimeIndex);
            path = path.Substring(mimeIndex);

            var dataFormatIndex = path.IndexOf(",", StringComparison.Ordinal);
            if (dataFormatIndex < 0)
                throw new FormatException("The data format for the image could not be determined");

            var dataFormat = path.Substring(0, dataFormatIndex);
            path = path.Substring(dataFormatIndex);

            if (dataFormat != "base64")
                throw new FormatException(
                    "The data format is not base 64, the only supported image encoding format for data images");
            
            var bin = Convert.FromBase64String(path);
            
            using (var ms = new MemoryStream(bin))
            {
                return DoDecodeImageData(ms, document, owner, path);
            }
        }
    }
}