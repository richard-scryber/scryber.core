using System;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
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
        private static readonly Regex PngMatch = new Regex("\\.(png)?\\s*$", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
        private static readonly string PngName = "Png Image factory";
        private static readonly bool PngShouldCache = true;
        
        public ImageFactoryPng()
            :this(PngMatch, PngName, PngShouldCache)
        {

        }

        protected ImageFactoryPng(Regex match, string name, bool shouldCache)
            : base(match, name, shouldCache)
        {
        }


        protected override ImageData DoDecodeImageData(Stream stream, IDocument document, IComponent owner, string path)
        {
            IImageFormat format;
            SixLabors.ImageSharp.Configuration config = SixLabors.ImageSharp.Configuration.Default;
            var img = Image.Load(config, stream, out format);
            
            PDFImageDataBase data = null;

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
                
                var alpha = meta.HasTransparency || (meta.ColorType.HasValue && meta.ColorType == PngColorType.RgbWithAlpha);
                
                
                data = GetImageDataForImage(Scryber.Drawing.ImageFormat.Png, img, path, depth, alpha, colorSpace);
                
            }



            return data;
        }




    }
}
