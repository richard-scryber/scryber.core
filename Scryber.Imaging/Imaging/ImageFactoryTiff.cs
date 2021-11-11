using System;
using System.IO;
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
        public ImageFactoryTiff()
        {
        }

        public override bool ShouldCache
        {
            get { return true; }
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
                const bool hasAlpha = true;
                
                data = GetImageDataForImage(ImageFormat.Tiff, img, path, bitDepth, hasAlpha, colorSpace);
            }



            return data;
        }




    }
}
