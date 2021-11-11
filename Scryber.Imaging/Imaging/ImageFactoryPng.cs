using System;
using System.IO;
using System.Threading.Tasks;
using Scryber.Drawing;
using Scryber.Imaging.Formatted;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;

namespace Scryber.Imaging
{
    public class ImageFactoryPng : ImageFactoryBase, IPDFImageDataFactory
    {
        public ImageFactoryPng()
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
            
            PDFImageSharpData data = null;

            if (format.Name == "PNG")
            {
                var meta = img.Metadata.GetFormatMetadata(PngFormat.Instance);

                
                
                ColorSpace colorSpace = meta.ColorType == PngColorType.Grayscale ? ColorSpace.G : ColorSpace.RGB;
                int depth;

                switch (meta.BitDepth)
                {
                    case (PngBitDepth.Bit1):
                        depth = 1;
                        break;
                    case (PngBitDepth.Bit2):
                        depth = 2;
                        break;
                    case (PngBitDepth.Bit4):
                        depth = 4;
                        break;
                    case (PngBitDepth.Bit8):
                        depth = 8;
                        break;
                    case (PngBitDepth.Bit16):
                        depth = 16;
                        break;
                    default:
                        throw new IndexOutOfRangeException("The bit depth for the Png image " + path +
                                                           " was out of range : " + meta.BitDepth.ToString());
                }
                
                var alpha = meta.HasTransparency;
                
                data = GetImageDataForImage(Scryber.Drawing.ImageFormat.Png, img, path, depth, alpha, colorSpace);
                
            }



            return data;
        }




    }
}
