using System;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Scryber.Drawing;
using Scryber.Imaging.Formatted;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Webp;

namespace Scryber.Imaging
{
    public class ImageFactoryWebP : ImageFactoryBase, IPDFImageDataFactory
    {
        private static readonly Regex PngMatch = new Regex("\\.(webp)?\\s*$", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
        private static readonly string PngName = "WebP Image factory";
        private static readonly bool PngShouldCache = true;
        
        public ImageFactoryWebP()
            :this(PngMatch, PngName, PngShouldCache)
        {

        }

        protected ImageFactoryWebP(Regex match, string name, bool shouldCache)
            : base(match, MimeType.PngImage, name, shouldCache)
        {
        }

        protected override ImageData DoLoadRawImageData(IDocument document, IComponent owner, byte[] rawData, MimeType type)
        {
            var span = new ReadOnlySpan<byte>(rawData);
            var img = Image.Load(span);
            var meta =  img.Metadata.GetFormatMetadata(WebpFormat.Instance);
            
            ImageData data = null;
            
            if (null != meta)
            {
                ColorSpace colorSpace = ColorSpace.RGB;
                int depth = 8;
                var alpha = true;
                
                var name = document.GetIncrementID(ObjectTypes.ImageData) + ".webq";
                
                data = GetImageDataForImage(Scryber.Drawing.ImageFormat.WebP, img, name, depth, alpha, colorSpace);
            }
            else
            {
                if (document.ConformanceMode == ParserConformanceMode.Strict)
                    throw new PDFDataException(
                        "The format of the raw image data was expected to be WebP");
                
                document.TraceLog.Add(TraceLevel.Error,"Image", "The format of the raw image data was expected to be WebP");
            }

            return data;
        }


        protected override ImageData DoDecodeImageData(Stream stream, IDocument document, IComponent owner, string path)
        {
            SixLabors.ImageSharp.Configuration config = SixLabors.ImageSharp.Configuration.Default;
            var img = Image.Load(stream);
            var  meta = img.Metadata.GetFormatMetadata(WebpFormat.Instance);
            ImageData data = null;

            if (null != meta)
            {
                ColorSpace colorSpace = ColorSpace.RGB;
                int depth = 8;
                var alpha = true;
                
                data = GetImageDataForImage(Scryber.Drawing.ImageFormat.WebP, img, path, depth, alpha, colorSpace);
                
            }

            return data;
        }




    }
}
