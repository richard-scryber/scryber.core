using System;
using System.IO;
using Scryber.Drawing;
using Scryber.PDF;
using Scryber.PDF.Native;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata;

namespace Scryber.Imaging.Formatted
{
    public class PDFImageJpegData : PDFImageDataBase
    {
        private const ColorSpace JpegColorSpace = ColorSpace.RGB;
        private const int JpegBitsPerColor = 8;
        private const int JpegColorsPerSample = 3;
        
        private byte[] CompressedData { get; }


        internal PDFImageJpegData(string source, PDFImageJpegMetadata metadata, byte[] compressedData)
           : base(source, metadata.PixelWidth, metadata.PixelHeight)
        {
            if (null == metadata)
                throw new ArgumentNullException(nameof(metadata));

            this.CompressedData = compressedData;
            this.Filters = new IStreamFilter[] { new PDFImageJpegStreamFilter() };
            this.InitPixelData(
                metadata.HasAlpha,
                metadata.ColorSpace,
                metadata.BitsPerColor,
                metadata.ColorsPerSample,
                metadata.HorizontalResolution,
                metadata.VerticalResolution,
                metadata.ResolutionUnits);
        }


        public PDFImageJpegData(Image image, string source, byte[] compressedData)
            : base(image, source)
        {
            this.CompressedData = compressedData ?? throw new ArgumentNullException(nameof(compressedData),"The compressed data cannot be null");
            this.HasAlpha = false;
            
            this.InitPixelData(false, JpegColorSpace, JpegBitsPerColor, JpegColorsPerSample,
                image.Metadata.HorizontalResolution,
                image.Metadata.VerticalResolution,
                image.Metadata.ResolutionUnits);
            
            this.Filters = new IStreamFilter[] {new PDFImageJpegStreamFilter()};

        }

        public override bool IsPrecompressedData
        {
            get { return true; }
        }

        public override PDFObjectRef Render(PDFName name, IStreamFilter[] filters, ContextBase context, PDFWriter writer)
        {
            //We override any defalte filters on the request to render
            return base.Render(name, this.Filters, context, writer);
        }

        protected override long DoRenderImageData(IStreamFilter[] filters, ContextBase context, PDFWriter writer)
        {
            var filtered = this.CompressedData;
            foreach (var filter in filters)
            {
                filtered = filter.FilterStream(filtered);
            }
            
            writer.WriteRaw(filtered, 0, filtered.Length);

            return filtered.Length;
        }

        protected override long DoRenderAlphaData(IStreamFilter[] filters, ContextBase context, PDFWriter writer)
        {
            throw new NotImplementedException("The jpeg image data does not have an alpha stream");
        }


        /// <summary>
        /// A Jpeg stream filter - actually just copies the original data to the output.
        /// As the source should already be compressed
        /// </summary>
        internal class PDFImageJpegStreamFilter : IStreamFilter
        {
            public string FilterName
            {
                get { return "DCTDecode"; }
                set { throw new NotSupportedException("Name of the filter cannot be changed"); }
            }
            

            public PDFImageJpegStreamFilter()
            {
            }
            
            public void FilterStream(Stream read, Stream write)
            {
                read.CopyTo(write);
            }

            public byte[] FilterStream(byte[] orig)
            {
                return orig;
            }
            
            
        }

        internal class PDFImageJpegMetadata
        {

            public int PixelWidth { get; set; }

            public int PixelHeight { get; set; }

            public bool HasAlpha { get; set; }

            public ColorSpace ColorSpace { get; set; }

            public int BitsPerColor { get; set; }

            public int ColorsPerSample { get; set; }

            public double HorizontalResolution { get; set; }

            public double VerticalResolution { get; set; }

            public PixelResolutionUnit ResolutionUnits { get; set; }
        }


    }
}