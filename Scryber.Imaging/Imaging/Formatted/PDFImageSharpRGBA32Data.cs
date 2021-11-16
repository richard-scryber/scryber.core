using System;
using System.IO;
using System.Web;
using Scryber.Drawing;
using Scryber.PDF;
using Scryber.PDF.Native;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Scryber.Imaging.Formatted
{
    public class PDFImageSharpRGBA32Data : PDFImageSharpData<Rgba32>
    {

        public PDFImageSharpRGBA32Data(Image img, string source)
            : base(img, source)
        {
        }


        
        
        protected override long DoRenderImageData(IStreamFilter[] filters, ContextBase context, PDFWriter writer)
        {
            var config = SixLabors.ImageSharp.Configuration.Default;
            var ops = new Rgba32().CreatePixelOperations();

            int width = this.PixelWidth;
            long total = 0;
            byte[] buffer = new byte[width * 3];

            Span<byte> bufferSpan = new Span<byte>(buffer);

            for (int r = 0; r < this.PixelHeight; r++)
            {
                ReadOnlySpan<Rgba32> span = this.PixelImage.GetPixelRowSpan(r);
                ops.ToRgb24Bytes(config, span, buffer, width);
                writer.WriteRaw(buffer, 0, buffer.Length);

                total += buffer.Length;
            }

            return total;
        }


        protected override long DoRenderAlphaData(IStreamFilter[] filters, ContextBase context, PDFWriter writer)
        {
            int width = this.PixelWidth;
            long total = 0;
            byte[] buffer = new byte[width];

            Span<byte> bufferSpan = new Span<byte>(buffer);

            for (int r = 0; r < this.PixelHeight; r++)
            {
                var span = this.PixelImage.GetPixelRowSpan(r);

                for (var p = 0; p < buffer.Length; p++)
                    buffer[p] = span[p].A;
                
                writer.WriteRaw(buffer, 0, buffer.Length);

                total += buffer.Length;
            }

            return total;
        }

        


        public override void ResetFilterCache()
        {

        }
    }
}
