using System;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using Scryber.PDF;
using Scryber.PDF.Native;

namespace Scryber.Imaging.Formatted
{
    public class PDFImageSharpARGB32Data : PDFImageSharpData<Argb32>
    {


        public PDFImageSharpARGB32Data(Image img, string source)
            : base(img, source, GetAlphaFlagForInfo(img.PixelType))
        {
        }


        protected override long DoRenderImageData(IStreamFilter[] filters, ContextBase context, PDFWriter writer)
        {
            var config = SixLabors.ImageSharp.Configuration.Default;
            var ops = new Argb32().CreatePixelOperations();

            int width = this.PixelWidth;
            long total = 0;
            byte[] buffer = new byte[width * 3];

            Span<byte> bufferSpan = new Span<byte>(buffer);

            for (int r = 0; r < this.PixelHeight; r++)
            {
                ReadOnlySpan<Argb32> span = this.PixelImage.GetPixelRowSpan(r);
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
                ReadOnlySpan<Argb32> span = this.PixelImage.GetPixelRowSpan(r);

                for (int p = 0; p < buffer.Length; p++)
                {
                    buffer[p] = span[p].A;
                }
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
