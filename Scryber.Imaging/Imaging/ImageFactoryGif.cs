using System;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Scryber.Drawing;
using Scryber.Imaging.Formatted;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Gif;

namespace Scryber.Imaging
{
    public class ImageFactoryGif : ImageFactoryBase, IPDFImageDataFactory
    {
        private static readonly Regex GifMatch = new Regex("\\.(gif)?\\s*$", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
        private static readonly string GifName = "Gif Image factory";
        private static readonly bool GifShouldCache = true;
        
        public ImageFactoryGif()
            :this(GifMatch, GifName, GifShouldCache)
        {

        }

        protected ImageFactoryGif(Regex match, string name, bool shouldCache)
            : base(match, name, shouldCache)
        {
        }


        protected override ImageData DoDecodeImageData(Stream stream, IDocument document, IComponent owner, string path)
        {
            IImageFormat format;
            SixLabors.ImageSharp.Configuration config = SixLabors.ImageSharp.Configuration.Default;
            var img = Image.Load(config, stream, out format);

            ImageData data = null;

            if (format.Name == "GIF")
            {
                var meta = img.Metadata.GetFormatMetadata(GifFormat.Instance);
                
                const bool hasAlpha = true;
                const ColorSpace colorSpace = ColorSpace.RGB;
                const int bitDepth = 8;
                
                data = GetImageDataForImage(ImageFormat.Gif, img, path, bitDepth, hasAlpha, colorSpace);
            }



            return data;
        }
    }
    
}
