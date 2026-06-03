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
            : base(match, MimeType.PngImage, name, shouldCache)
        {
        }

        protected override ImageData DoLoadRawImageData(IDocument document, IComponent owner, byte[] rawData, MimeType type)
        {
            var span = new ReadOnlySpan<byte>(rawData);
            var img = Image.Load(span);
            var meta =  img.Metadata.GetFormatMetadata(PngFormat.Instance);
            
            ImageData data = null;
            
            if (null != meta)
            {
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
                        throw new IndexOutOfRangeException("The bit depth for the raw Png image was out of range : " + meta.BitDepth.ToString());
                }
                
                var alpha = (meta.ColorType.HasValue && meta.ColorType.Value == PngColorType.RgbWithAlpha) 
                            || (meta.ColorType.HasValue && meta.ColorType == PngColorType.GrayscaleWithAlpha);
                
                var name = document.GetIncrementID(ObjectTypes.ImageData) + ".png";
                
                data = CreatePngImageDataWithFallback(img, name, depth, alpha, colorSpace);
            }
            else
            {
                var name = document.GetIncrementID(ObjectTypes.ImageData) + ".png";
                data = CreatePngImageDataWithFallback(img, name, 8, true, ColorSpace.RGB);
            }

            return data;
        }


        protected override ImageData DoDecodeImageData(Stream stream, IDocument document, IComponent owner, string path)
        {
            var img = Image.Load(stream);
            var  meta = img.Metadata.GetFormatMetadata(PngFormat.Instance);
            ImageData data = null;

            if (null != meta)
            {
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
                
                var alpha = (meta.ColorType.HasValue && meta.ColorType.Value == PngColorType.GrayscaleWithAlpha) 
                            || (meta.ColorType.HasValue && meta.ColorType == PngColorType.RgbWithAlpha);
                
                
                data = CreatePngImageDataWithFallback(img, path, depth, alpha, colorSpace);
                
            }
            else
            {
                data = CreatePngImageDataWithFallback(img, path, 8, true, ColorSpace.RGB);
            }



            return data;
        }

        private ImageData CreatePngImageDataWithFallback(Image image, string source, int depth, bool hasAlpha, ColorSpace colorSpace)
        {
            try
            {
                return GetImageDataForImage(Scryber.Drawing.ImageFormat.Png, image, source, depth, hasAlpha, colorSpace);
            }
            catch (NotSupportedException)
            {
                var converted = image.CloneAs<Rgba32>();
                return GetImageDataForImage(Scryber.Drawing.ImageFormat.Png, converted, source, 8, true, ColorSpace.RGB);
            }
        }




    }
}
