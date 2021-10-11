using System;
using Scryber.Drawing;


namespace Scryber.PDF.Graphics
{
    public interface IPDFGraphicsAdapter
    {
        bool SetUpGraphics(PDFGraphics graphics, Rect bounds);

        void ReleaseGraphics(PDFGraphics g, Rect bounds);
    }
}
