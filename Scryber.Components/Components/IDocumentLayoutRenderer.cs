using System.IO;
using Scryber.PDF;
using Scryber.PDF.Layout;

namespace Scryber.Components
{
    /// <summary>
    /// Contract for custom renderers that consume a fully laid out Scryber document.
    /// This enables output backends beyond PDF (for example ESC/POS, ZPL, LINX).
    /// </summary>
    public interface IDocumentLayoutRenderer
    {
        /// <summary>
        /// Called after Scryber has completed layout and before standard PDF output is written.
        /// Implementations should write their output to the provided stream.
        /// </summary>
        /// <param name="document">The source document.</param>
        /// <param name="layout">The fully computed PDF layout tree.</param>
        /// <param name="layoutContext">The layout context that produced the layout.</param>
        /// <param name="output">Destination stream for backend output.</param>
        void Render(Document document, PDFLayoutDocument layout, PDFLayoutContext layoutContext, Stream output);
    }
}
