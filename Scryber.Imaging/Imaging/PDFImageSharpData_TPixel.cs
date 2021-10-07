using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using Scryber.Drawing;

namespace Scryber.Imaging
{
    /// <summary>
    /// Generic base class for all of the ImageSharp image types
    /// </summary>
    /// <typeparam name="T">The </typeparam>
    public abstract class PDFImageSharpData<T> : PDFImageSharpData where T : unmanaged, IPixel<T>
    {

        public Image<T> PixelImage { get; protected set; }

        public PDFImageSharpData(Image generic, string source, bool hasalpha = true, ColorSpace color = ColorSpace.RGB, int bitsPerColor = 8, int colorsPerSample = 24)
            : this(AssertImageType<T>(generic), source, hasalpha, color, bitsPerColor, colorsPerSample)
        {
        }

        public PDFImageSharpData(Image<T> img, string source, bool hasalpha = true, ColorSpace color = ColorSpace.RGB, int bitsPerColor = 8, int colorsPerSample = 24)
            : base(img, source)
        {
            this.PixelImage = img ?? throw new ArgumentNullException(nameof(img));
            this.InitPixelData(hasalpha, color, bitsPerColor, colorsPerSample, (int)img.Metadata.HorizontalResolution, (int)img.Metadata.VerticalResolution);
        }

        protected virtual void InitPixelData(bool hasalpha, ColorSpace color, int bitsPerColor, int colorsPerSample, int horizResolution, int vertResolution)
        {
            this.ColorSpace = color;
            this.HasAlpha = hasalpha;
            this.BitsPerColor = bitsPerColor;
            this.ColorsPerSample = colorsPerSample;
            this.HorizontalResolution = (int)this.PixelImage.Metadata.HorizontalResolution;
            this.VerticalResolution = (int)this.PixelImage.Metadata.VerticalResolution;

        }




        public static Image<TPixel> AssertImageType<TPixel>(Image image) where TPixel : unmanaged, IPixel<TPixel>
        {
            if (null == image)
                throw new ArgumentNullException("The image is null or has been invalidly case to an image with pixel data of type " + typeof(TPixel).Name);

            else if (image is Image<TPixel> pixelimg)
                return pixelimg;

            else
                throw new InvalidCastException("Cannot convert the image to pixel data " + typeof(TPixel).Name);
        }
    }
}
