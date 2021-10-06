using System;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using Scryber.PDF;
using Scryber.PDF.Native;

namespace Scryber.Imaging.Formatted
{
    public class PDFImageSharpBgr24Data : PDFImageSharpData<Bgr24>
    {


        public PDFImageSharpBgr24Data(Image img, string source)
            : base(img, source, hasalpha: false)
        {
        }


        protected override long DoRenderImageData(IStreamFilter[] filters, PDFContextBase context, PDFWriter writer)
        {
            var config = SixLabors.ImageSharp.Configuration.Default;
            var ops = new Bgr24().CreatePixelOperations();

            int width = this.PixelWidth;
            long total = 0;
            byte[] buffer = new byte[width * 3];

            Span<byte> bufferSpan = new Span<byte>(buffer);

            for (int r = 0; r < this.PixelHeight; r++)
            {
                ReadOnlySpan<Bgr24> span = this.PixelImage.GetPixelRowSpan(r);
                ops.ToRgb24Bytes(config, span, buffer, width);
                writer.WriteRaw(buffer, 0, buffer.Length);
                total += buffer.Length;
            }

            return total;
        }

        protected override long DoRenderAlphaData(IStreamFilter[] filters, PDFContextBase context, PDFWriter writer)
        {
            throw new InvalidOperationException("The Bgr24 format does not have an alpha component");
        }

        public override void ResetFilterCache()
        {

        }
    }
}
