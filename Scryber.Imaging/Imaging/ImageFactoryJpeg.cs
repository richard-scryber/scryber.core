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
            ImageData data;

            var headerInfo = ReadJpegHeader(binary);
            if (null == headerInfo)
            {
                //var bmp = System.Drawing.Bitmap.FromStream(binary) as System.Drawing.Bitmap;
                //return GetImageDataForBitmap(bmp, path, binary.ToArray());

                var img = Image.Load(config, binary, out var format);

                if (null != format && format.Name == "JPEG")
                {
                    data = GetImageDataForImage(img, path, binary.ToArray());
                }
                else
                {
                    throw new NotSupportedException("The source image was not interpreted as a Jpeg image at path " + path);
                }

            }
            else
            {
                data = new Formatted.PDFImageJpegData(path, headerInfo, binary.ToArray()); 
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

        private Formatted.PDFImageJpegData.PDFImageJpegMetadata ReadJpegHeader(Stream stream, bool setPosition = true)
        {
            if (!stream.CanSeek)
                return null;
            if (!stream.CanRead)
                return null;

            var pos = stream.Position;

            try
            { 
                if (setPosition)
                    stream.Position = 0;

                var offset = stream.Position;

                //check the starting marker - 0xFF, 0xD8 - Start of Image
                var (one, two) = stream.ReadDoubleByte();
                if (one != 0xFF && two != 0xD8)
                    return null;

                //next two bytes 0xFF 0xE0 - JFIF-APP0
                (one, two) = stream.ReadDoubleByte();

                if (one != 0xFF && !(two == 0xE0 || two == 0xE1))
                    return null;

                var blocklen = stream.ReadUShort();

                var ident = stream.ReadJpegIdentity();

                var vers = stream.ReadVersion();
                if (null == vers || vers.Major != 1 || vers.Minor > 2)
                    return null;

                if (ident == "JFIF")
                    return ReadJFIFHeader(stream, blocklen, offset);
                else if (ident == "Exif")
                    return ReadExifHeader(stream, blocklen, offset);

                

            }
            finally
            {
                //reset the position to where we came in.
                stream.Position = pos;
            }
            return null;

        }

        private Formatted.PDFImageJpegData.PDFImageJpegMetadata ReadExifHeader(Stream stream, ushort blocklen, long offset)
        {
            return null;
        }

        private Formatted.PDFImageJpegData.PDFImageJpegMetadata ReadJFIFHeader(Stream stream, ushort blocklen, long offset)
        {

            var density = stream.ReadByte();
            var densityX = stream.ReadUShort();
            var densityY = stream.ReadUShort();

            offset += blocklen + 4; //FF D8 FF E0 Len

            while (offset < stream.Length)
            {
                stream.Position = offset;
                var (one, two) = stream.ReadDoubleByte();
                blocklen = stream.ReadUShort();

                if (one != 0xFF)
                {
                    //The block marker should always begin with 0xFF - we are not in the right place to go any further
                    return null;
                }

                if (two != 0xC0)
                {
                    //This is not the block we are looking for
                    offset += blocklen + 2; // so move on to the next block
                    continue;
                }

                //We should have our start of frame

                var precision = stream.ReadByte();
                var lineCount = stream.ReadUShort();
                var samplePerLine = stream.ReadUShort();
                var componentCount = stream.ReadByte();

                if (componentCount != 1 && componentCount != 3)
                {
                    //only 3 or 1 is supported.
                    return null;
                }

                var unit = SixLabors.ImageSharp.Metadata.PixelResolutionUnit.AspectRatio;

                if (density == 1)
                    unit = SixLabors.ImageSharp.Metadata.PixelResolutionUnit.PixelsPerInch;
                else if (density == 2)
                    unit = SixLabors.ImageSharp.Metadata.PixelResolutionUnit.PixelsPerCentimeter;

                return new Formatted.PDFImageJpegData.PDFImageJpegMetadata()
                {
                    BitsPerColor = precision,
                    ColorsPerSample = componentCount,
                    HasAlpha = false,
                    ColorSpace = (componentCount == 1) ? ColorSpace.G : ColorSpace.RGB,
                    HorizontalResolution = densityX,
                    VerticalResolution = densityY,
                    ResolutionUnits = unit,
                    PixelHeight = lineCount,
                    PixelWidth = samplePerLine
                };

            }

            return null;
        }



        
    }



    internal static class JPEGStreamExtension
    {

        public static (int, int) ReadDoubleByte(this Stream stream)
        {
            var one = stream.ReadByte();
            var two = stream.ReadByte();
            return (one, two);
        }


        public static ushort ReadUShort(this Stream stream)
        {
            var one = stream.ReadByte();
            var two = stream.ReadByte();
            ushort both = (ushort)(one << 8 | two);
            return both;
        }

        public static string ReadJpegIdentity(this Stream stream)
        {
            char[] all = new char[4];
            all[0] = (char)stream.ReadByte();
            all[1] = (char)stream.ReadByte();
            all[2] = (char)stream.ReadByte();
            all[3] = (char)stream.ReadByte();
            var terminator = stream.ReadByte();

            if (terminator != 0x00)
                return string.Empty;
            else
            {
                string full = new string(all);
                return full;
            }
        }

        public static Version ReadVersion(this Stream stream)
        {
            int major = stream.ReadByte();
            int minor = stream.ReadByte();
            return new Version(major, minor);
        }

    }
}