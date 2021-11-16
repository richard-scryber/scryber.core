

using System;
using System.IO;
using Scryber.Drawing;
using Scryber.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Scryber.Imaging
{
    public class ImageReader
    {

        protected ImageReader()
        {
        }

        public ImageData ReadData(string name, byte[] data, bool compress)
        {
            Image image = Image.Load(data);
            if (compress)
                image = CompressImage(image);

            var imgData = GetImageData(name, image);
            
            return imgData;
        }

        public ImageData ReadStream(string name, Stream stream, bool compress)
        {
            Image image = Image.Load(stream);

            if (compress)
                image = CompressImage(image);

            var imgData = GetImageData(name, image);

            return imgData;
        }

        protected virtual ImageData GetImageData(string name, Image img)
        {
            var imgData = ImageFactoryBase.GetImageDataForImage(img, name);
            imgData.SetSourceImageFormat(ImageFormat.Bitmap, imgData.BitsPerColor, imgData.HasAlpha, ColorSpace.RGB);
            return imgData;
        }

        protected virtual Image CompressImage(Image original)
        {
            return original;
        }

        public static ImageReader Create()
        {
            return new ImageReader();
        }
    }
}