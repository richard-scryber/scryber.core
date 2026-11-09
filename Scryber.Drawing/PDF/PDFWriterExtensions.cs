using Scryber.Drawing;

namespace Scryber.PDF
{
    public static class PDFWriterExtensions
    {
        public const string ColorNumberFormat = "F4";
        public static void WriteColorArray(this PDFWriter writer, Color color)
        {
            writer.BeginArray();
            writer.WriteRealS(color.Red, color.Green, color.Blue, ColorNumberFormat);
            writer.EndArray();
        }
    }
}