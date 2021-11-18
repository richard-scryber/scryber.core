using System;
using System.IO;
using System.Text.RegularExpressions;
using Scryber.Drawing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;

namespace Scryber.Imaging
{
    public class ImageFactoryJpeg : ImageFactoryBase, IPDFImageDataFactory
    {

        private static readonly Regex JpegMatch = new Regex("\\.(jpg|jpeg)?\\s*$", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
        private static readonly string JpegName = "Jpeg Image factory";
        private static readonly bool JpegShouldCache = true;
        
        public ImageFactoryJpeg()
        :this(JpegMatch, JpegName, JpegShouldCache)
        {

        }

        protected ImageFactoryJpeg(Regex match, string name, bool shouldCache)
            : base(match, name, shouldCache)
        {
        }

        protected override ImageData DoDecodeImageData(Stream stream, IDocument document, IComponent owner, string path)
        {
            Configuration config = Configuration.Default;
            
            //For JPEG we want the original compressed image data
            var binary = ExtractImageDataFromStream(stream);
            binary.Position = 0;
            
            var img = Image.Load(config, binary, out var format);

            ImageData data = null;

            if (null != format && format.Name == "JPEG")
            {
                data = GetImageDataForImage(img, path, binary.ToArray());
            }
            else
            {
                throw new NotSupportedException("The source image was not interpreted as a Jpeg image at path " + path);
            }



            return data;
        }

        protected ImageData GetImageDataForImage(Image baseImage, string source, byte[] jpegdata)
        {
            return new Formatted.PDFImageJpegData(baseImage, source, jpegdata);

        }

        private MemoryStream ExtractImageDataFromStream(Stream stream)
        {
            if (stream is MemoryStream)
                return (MemoryStream) stream;
            
            var ms = new MemoryStream();
            stream.CopyTo(ms);
            return ms;
        }
    }
}