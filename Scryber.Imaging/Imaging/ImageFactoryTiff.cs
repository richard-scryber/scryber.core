using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Scryber.Drawing;
using Scryber.Imaging.Formatted;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Tga;
using SixLabors.ImageSharp.Formats.Tiff;

namespace Scryber.Imaging
{
    public class ImageFactoryTiff : ImageFactoryBase, IPDFImageDataFactory
    {
        private static readonly Regex TiffMatch = new Regex("\\.(tif|tiff)?\\s*$", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
        private const string TiffName = "Tiff Image factory";
        private const bool TiffShouldCache = true;
        
        public ImageFactoryTiff()
            :this(TiffMatch, TiffName, TiffShouldCache)
        {

        }

        protected ImageFactoryTiff(Regex match, string name, bool shouldCache)
            : base(match, MimeType.TiffImage,  name, shouldCache)
        {
        }


        protected override ImageData DoLoadRawImageData(IDocument document, IComponent owner, byte[] rawData, MimeType type)
        {
            IImageFormat format;
            SixLabors.ImageSharp.Configuration config = SixLabors.ImageSharp.Configuration.Default;
            var span = new ReadOnlySpan<byte>(rawData);
            var img = Image.Load(config, span, out format);

            ImageData data = null;

            if (format.Name == "TIFF")
            {
                var meta = img.Metadata.GetFormatMetadata(TiffFormat.Instance);
                var name = document.GetIncrementID(ObjectTypes.ImageData) + ".tiff";
                const ColorSpace colorSpace = ColorSpace.RGB;
                const int bitDepth = 8; 
                bool hasAlpha = img.PixelType.AlphaRepresentation.HasValue 
                                && img.PixelType.AlphaRepresentation != PixelAlphaRepresentation.None;
                
                data = GetImageDataForImage(ImageFormat.Tiff, img, name, bitDepth, hasAlpha, colorSpace);
            }



            return data;
        }


        protected override ImageData DoDecodeImageData(Stream stream, IDocument document, IComponent owner, string path)
        {
            IImageFormat format;
            SixLabors.ImageSharp.Configuration config = SixLabors.ImageSharp.Configuration.Default;
            var img = Image.Load(config, stream, out format);

            ImageData data = null;

            if (format.Name == "TIFF")
            {
                var meta = img.Metadata.GetFormatMetadata(TiffFormat.Instance);
                
                const ColorSpace colorSpace = ColorSpace.RGB;
                const int bitDepth = 8; 
                bool hasAlpha = img.PixelType.AlphaRepresentation.HasValue 
                                && img.PixelType.AlphaRepresentation != PixelAlphaRepresentation.None;
                
                data = GetImageDataForImage(ImageFormat.Tiff, img, path, bitDepth, hasAlpha, colorSpace);
            }



            return data;
        }




    }
}
