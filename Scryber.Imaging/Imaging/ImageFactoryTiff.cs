using System;
using System.IO;
using System.Threading.Tasks;
using Scryber.Drawing;
using Scryber.Imaging.Formatted;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;

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


        protected override PDFImageData DoDecodeImageData(Stream stream, IDocument document, IComponent owner, string path)
        {
            IImageFormat format;
            SixLabors.ImageSharp.Configuration config = SixLabors.ImageSharp.Configuration.Default;
            var img = Image.Load(config, stream, out format);

            PDFImageData data = null;

            if (format.Name == "TIFF")
            {
                data = GetImageDataForImage(img, path);
            }



            return data;
        }




    }
}
