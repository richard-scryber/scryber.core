using System;
using Scryber.Drawing;


namespace Scryber.PDF.Graphics
{
    public interface IPDFGraphicsAdapter
    {
        bool SetUpGraphics(PDFGraphics graphics, PDFRect bounds);

        void ReleaseGraphics(PDFGraphics g, PDFRect bounds);
    }
}
