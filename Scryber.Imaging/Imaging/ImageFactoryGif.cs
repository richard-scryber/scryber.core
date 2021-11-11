using System;
using System.IO;
using System.Threading.Tasks;
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
        public ImageFactoryGif()
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
