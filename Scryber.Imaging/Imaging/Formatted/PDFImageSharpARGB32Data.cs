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
            : base(img, source)
        {
        }


        protected override long DoRenderImageData(IStreamFilter[] filters, ContextBase context, PDFWriter writer)
        {
            var config = SixLabors.ImageSharp.Configuration.Default;
            var ops = new Argb32().CreatePixelOperations();
            long total = 0;

            //New v2 ImageSharp accessor pattern
            this.PixelImage.ProcessPixelRows(accessor =>
            {
                int width = this.PixelWidth;
                
                byte[] buffer = new byte[width * 3];

                Span<byte> bufferSpan = new Span<byte>(buffer);
                for (int r = 0; r < this.PixelHeight; r++)
                {
                    var span = accessor.GetRowSpan(r);
                    ops.ToRgb24Bytes(config, span, bufferSpan, width);
                    writer.WriteRaw(buffer, 0, buffer.Length);
                    total += buffer.Length;
                }

            });
            

            //for (int r = 0; r < this.PixelHeight; r++)
            //{
            //    ReadOnlySpan<Argb32> span = this.PixelImage.GetPixelRowSpan(r);
            //    ops.ToRgb24Bytes(config, span, buffer, width);
            //    writer.WriteRaw(buffer, 0, buffer.Length);
            //    total += buffer.Length;
            //}
            return total;
        }

        protected override long DoRenderAlphaData(IStreamFilter[] filters, ContextBase context, PDFWriter writer)
        {
            long total = 0;

            //New v2 ImageSharp accessor pattern
            this.PixelImage.ProcessPixelRows(accessor =>
            {

                int width = this.PixelWidth;
                byte[] buffer = new byte[width];

                Span<byte> bufferSpan = new Span<byte>(buffer);

                for (int r = 0; r < this.PixelHeight; r++)
                {
                    ReadOnlySpan<Argb32> span = accessor.GetRowSpan(r);

                    for (int p = 0; p < buffer.Length; p++)
                    {
                        buffer[p] = span[p].A;
                    }

                    writer.WriteRaw(buffer, 0, buffer.Length);
                    total += buffer.Length;
                }
            });


            //for (int r = 0; r < this.PixelHeight; r++)
            //{
                
            //    for (int p = 0; p < buffer.Length; p++)
            //    {
            //        buffer[p] = span[p].A;
            //    }
            //    writer.WriteRaw(buffer, 0, buffer.Length);

            //    total += buffer.Length;
            //}

            return total;
        }

        public override void ResetFilterCache()
        {

        }
    }
}
