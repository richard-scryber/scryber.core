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
    public class ImageFactoryPngDataUrl : ImageFactoryPng
    {
        private static readonly Regex GifDataMatch = new Regex("^\\s*data:image/png;", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
        private const string GifDataName = "Png inline data image factory";
        private const bool GifDataShouldCache = false;

        private const string DataImagePrefix = "data:image/png;base64,";
        
        public ImageFactoryPngDataUrl() : this(GifDataMatch, GifDataName, GifDataShouldCache)
        {}
        
        
        public ImageFactoryPngDataUrl(Regex match, string name, bool shouldCache)
            : base(match, name, shouldCache)
        {}


        protected override async Task<ImageData> DoLoadImageDataAsync(IDocument document, IComponent owner, string path)
        {
            var bin = GetBinaryDataFromDataUrl(path, "image/png");

            using (var ms = new MemoryStream(bin))
            {
                return DoDecodeImageData(ms, document, owner, path);
            }
        }

        private static readonly Regex DataUrlGroups = new Regex(
            @"data:(?<mime>[\w/\-\.]+);(?<encoding>\w+),(?<data>.*)");

        private static byte[] GetBinaryDataFromDataUrl(string path, string expectedMimeType)
        {
            var parts = DataUrlGroups.Match(path);
            if (parts.Success == false)
                throw new FormatException("The data image url could not be split");
            
            if(parts.Groups["mime"].Value != expectedMimeType)
                throw new FormatException("The mime type for the image was not the expected '"  + expectedMimeType +"' format");

            if (parts.Groups["encoding"].Success == false || parts.Groups["encoding"].Value != "base64")
                throw new FormatException("Only base64 encoding is supported in data urls");
            
            var data = parts.Groups["data"].Value;
            if (string.IsNullOrEmpty(data))
                throw new FormatException("The binary data could not be extracted from the data url");
            
            var bin = Convert.FromBase64String(data);
            return bin;
        }
    }
}