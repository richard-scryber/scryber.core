using System;
using System.IO;
using System.Text.RegularExpressions;
using Scryber.Drawing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;

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
            : base(match, MimeType.JpegImage,  name, shouldCache)
        {
        }

        protected override ImageData DoLoadRawImageData(IDocument document, IComponent owner, byte[] rawData, MimeType type)
        {
            Configuration config = Configuration.Default;
            using (var binary = new MemoryStream(rawData))
            {
                ImageData data = null;

                var name = document.GetIncrementID(ObjectTypes.ImageData) + ".jpg";

                var headerInfo = ReadJpegHeader(binary);
                if (null == headerInfo)
                {
                    var img = Image.Load(binary);
                    var meta = img.Metadata.GetFormatMetadata(JpegFormat.Instance);

                    if (meta != null)
                    {
                        data = GetImageDataForImage(img, name, rawData);
                    }

                }
                else
                {
                    data = new Formatted.PDFImageJpegData(name, headerInfo, rawData);
                    data.ExifMetadata = TryExtractExif(binary);
                }

                return data;
            }
        }


        protected override ImageData DoDecodeImageData(Stream stream, IDocument document, IComponent owner, string path)
        {
            Configuration config = Configuration.Default;
            
            //For JPEG we want the original compressed image data
            bool disposable;
            
            var binary = ExtractImageDataFromStream(stream, out disposable);
            binary.Position = 0;
            ImageData data = null;

            try
            {
                var headerInfo = ReadJpegHeader(binary);
                if (null == headerInfo)
                {
                    //var bmp = System.Drawing.Bitmap.FromStream(binary) as System.Drawing.Bitmap;
                    //return GetImageDataForBitmap(bmp, path, binary.ToArray());

                    var img = Image.Load(binary);
                    var meta = img.Metadata.GetFormatMetadata(JpegFormat.Instance);

                    if (null != meta)
                    {
                        data = GetImageDataForImage(img, path, binary.ToArray());
                    }
                    else
                    {
                        throw new NotSupportedException(
                            "The source image was not interpreted as a Jpeg image at path " + path);
                    }

                }
                else
                {
                    data = new Formatted.PDFImageJpegData(path, headerInfo, binary.ToArray());
                    data.ExifMetadata = TryExtractExif(binary);
                }
            }
            finally
            {
                if(disposable)
                    binary.Dispose();
            }

            return data;
        }

        

        protected ImageData GetImageDataForImage(Image baseImage, string source, byte[] jpegdata)
        {
            return new Formatted.PDFImageJpegData(baseImage, source, jpegdata);

        }

        private MemoryStream ExtractImageDataFromStream(Stream stream, out bool disposable)
        {
            disposable = false;
            if (stream is MemoryStream)
                return (MemoryStream) stream;
            
            var ms = new MemoryStream();
            stream.CopyTo(ms);
            disposable = true;
            
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

        /// <summary>
        /// Deliberately still a stub - when the very first APP marker identifies as "Exif" (as
        /// opposed to "JFIF"), ReadJpegHeader returning null here forces the caller to fall
        /// through to Image.Load, which already picks up EXIF via GetImageDataForImage's own
        /// hook. TryExtractExif below covers the other case: a JFIF-first file (fast dimension
        /// path taken, no full decode) that also carries a *later* APP1/Exif segment, which this
        /// stub never sees since ReadJpegHeader only inspects the first marker.
        /// </summary>
        private Formatted.PDFImageJpegData.PDFImageJpegMetadata ReadExifHeader(Stream stream, ushort blocklen, long offset)
        {
            return null;
        }

        private static readonly byte[] ExifIdentifier = { (byte)'E', (byte)'x', (byte)'i', (byte)'f', 0, 0 };

        /// <summary>
        /// Independently scans the leading marker segments for an APP1/Exif segment, regardless
        /// of whether the fast JFIF dimension path or the ident check above handled this file -
        /// so a JFIF-branded JPEG that also carries EXIF (common - many cameras/editors write
        /// both) doesn't silently lose it just because the fast path took over for dimensions.
        /// Hands the raw segment bytes straight to SixLabors' ExifProfile(byte[]) constructor,
        /// which parses the TIFF-structured EXIF data without needing a full image decode.
        /// </summary>
        private Scryber.Drawing.ImageEXIFMap TryExtractExif(Stream stream)
        {
            if (!stream.CanSeek || !stream.CanRead)
                return null;

            var pos = stream.Position;
            try
            {
                stream.Position = 0;

                var (soi1, soi2) = stream.ReadDoubleByte();
                if (soi1 != 0xFF || soi2 != 0xD8)
                    return null;

                while (stream.Position < stream.Length - 4)
                {
                    var (one, two) = stream.ReadDoubleByte();
                    if (one != 0xFF)
                        break; //not a marker - stop, we've drifted out of the header region

                    if (two == 0xDA || two == 0xD9)
                        break; //Start of Scan / End of Image - no more APP markers follow

                    var blocklen = stream.ReadUShort(); //includes the 2 length bytes themselves
                    var segmentStart = stream.Position;

                    if (two == 0xE1 && blocklen > 2 + ExifIdentifier.Length)
                    {
                        var ident = new byte[ExifIdentifier.Length];
                        if (stream.Read(ident, 0, ident.Length) == ident.Length && IdentifierMatches(ident))
                        {
                            var exifLen = blocklen - 2 - ExifIdentifier.Length;
                            var exifBytes = new byte[exifLen];
                            var read = stream.Read(exifBytes, 0, exifLen);

                            if (read == exifLen)
                            {
                                try
                                {
                                    var profile = new SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifProfile(exifBytes);
                                    return ImageEXIFExtractor.Extract(profile);
                                }
                                catch
                                {
                                    //Malformed/truncated EXIF segment - not worth failing the
                                    //whole image load over, just proceed without metadata.
                                    return null;
                                }
                            }
                        }
                    }

                    stream.Position = segmentStart + (blocklen - 2);
                }
            }
            catch
            {
                //Any stream/parsing issue here should never prevent the image itself loading.
                return null;
            }
            finally
            {
                stream.Position = pos;
            }

            return null;
        }

        private static bool IdentifierMatches(byte[] ident)
        {
            for (int i = 0; i < ExifIdentifier.Length; i++)
            {
                if (ident[i] != ExifIdentifier[i])
                    return false;
            }
            return true;
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
                else if(density == 0)
                {
                    
                    if (densityX == 1 )
                    {
                        densityX = 72;
                    }
                    if(densityY == 1)
                    {
                        densityY = 72;
                    }
                }

                return new Formatted.PDFImageJpegData.PDFImageJpegMetadata()
                {
                    BitsPerColor = precision,
                    ColorsPerSample = componentCount * precision,
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