using System;

using Microsoft.AspNetCore.Mvc;

namespace Scryber.Components
{
    public static class PDFDocumentExtensions
    {

        public static IActionResult ProcessDocument(this PDFDocument doc)
        {
            var ms = new System.IO.MemoryStream();

            doc.ProcessDocument(ms, true);
            ms.Flush();
            ms.Position = 0;

            return new FileStreamResult(ms, "application/pdf");
        }
    }
}
