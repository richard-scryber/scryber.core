using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.PDF;
using Scryber.PDF.Native;

namespace Scryber.Core.UnitTests.Html
{
    /// <summary>
    /// Tests for Phase 5 - CSS :hover/:active driving a colour-only repaint of a field's
    /// Rollover (/R) / Down (/D) appearance, reusing Normal's (/N) exact box. Structure and
    /// paint content are verified against the real generated PDF.
    /// </summary>
    [TestClass()]
    public class HTMLFormFieldStates_Tests
    {
        private static Document ParseHtml(string bodyHtml)
        {
            var src = "<html><body>" + bodyHtml + "</body></html>";
            return Document.ParseHtmlDocument(new StringReader(src));
        }

        private static Document ParseHtmlWithStyle(string css, string bodyHtml)
        {
            var src = "<html><head><style>" + css + "</style></head><body>" + bodyHtml + "</body></html>";
            return Document.ParseHtmlDocument(new StringReader(src));
        }

        private static PDFDictionary GetDictionary(PDFReader reader, PDFObjectRef oref)
        {
            Assert.IsNotNull(oref);
            var obj = reader.GetObject(oref);
            Assert.IsNotNull(obj, "Could not resolve object " + oref);
            return obj.GetContents() as PDFDictionary;
        }

        private static PDFDictionary GetSoleWidgetAP(PDFReader reader, out PDFObjectRef nRef, out PDFObjectRef rRef, out PDFObjectRef dRef)
        {
            var catalog = GetDictionary(reader, reader.DocumentCatalogRef.Reference);
            catalog.TryGetValue("AcroForm", out var acroFormEntry);
            var acroForm = GetDictionary(reader, acroFormEntry as PDFObjectRef);
            acroForm.TryGetValue("Fields", out var fieldsEntry);
            var fields = fieldsEntry as PDFArray;
            Assert.AreEqual(1, fields.Count);

            var widget = GetDictionary(reader, fields[0] as PDFObjectRef);
            widget.TryGetValue("AP", out var apEntry);
            var ap = apEntry as PDFDictionary;

            ap.TryGetValue("N", out var n);
            ap.TryGetValue("R", out var r);
            ap.TryGetValue("D", out var d);
            nRef = n as PDFObjectRef;
            rRef = r as PDFObjectRef;
            dRef = d as PDFObjectRef;

            return ap;
        }

        [TestMethod()]
        [TestCategory("Html-Forms")]
        public void Input_NoHoverActiveCss_SameAppearanceRefForAllStates()
        {
            var doc = ParseHtml("<input id='i1' type='text' name='name' value='Jane' />");

            using var stream = DocStreams.GetOutputStream("FieldStates_None.pdf");
            doc.SaveAsPDF(stream);
            stream.Flush();

            var reader = PDFReader.Create(stream, new Scryber.Logging.DoNothingTraceLog(TraceRecordLevel.Diagnostic));
            GetSoleWidgetAP(reader, out var n, out var r, out var d);

            Assert.AreEqual(n.Number, r.Number, "No :hover style - /R should reuse the exact /N object");
            Assert.AreEqual(n.Number, d.Number, "No :active style - /D should reuse the exact /N object");
        }

        [TestMethod()]
        [TestCategory("Html-Forms")]
        public void Input_WithHoverCss_DistinctRolloverAppearance()
        {
            var doc = ParseHtmlWithStyle(
                "input:hover { background-color: #ff0000; }",
                "<input id='i1' type='text' name='name' value='Jane' />");

            using var stream = DocStreams.GetOutputStream("FieldStates_Hover.pdf");
            doc.SaveAsPDF(stream);
            stream.Flush();

            var reader = PDFReader.Create(stream, new Scryber.Logging.DoNothingTraceLog(TraceRecordLevel.Diagnostic));
            GetSoleWidgetAP(reader, out var n, out var r, out var d);

            Assert.AreNotEqual(n.Number, r.Number, "A matching :hover style should produce a distinct /R appearance");
            Assert.AreEqual(n.Number, d.Number, "No :active style - /D should still reuse /N");
        }

        [TestMethod()]
        [TestCategory("Html-Forms")]
        public void Input_WithActiveCss_DistinctDownAppearance()
        {
            var doc = ParseHtmlWithStyle(
                "input:active { background-color: #0000ff; }",
                "<input id='i1' type='text' name='name' value='Jane' />");

            using var stream = DocStreams.GetOutputStream("FieldStates_Active.pdf");
            doc.SaveAsPDF(stream);
            stream.Flush();

            var reader = PDFReader.Create(stream, new Scryber.Logging.DoNothingTraceLog(TraceRecordLevel.Diagnostic));
            GetSoleWidgetAP(reader, out var n, out var r, out var d);

            Assert.AreEqual(n.Number, r.Number, "No :hover style - /R should still reuse /N");
            Assert.AreNotEqual(n.Number, d.Number, "A matching :active style should produce a distinct /D appearance");
        }

        [TestMethod()]
        [TestCategory("Html-Forms")]
        public void Input_WithHoverCss_RepaintedStreamContainsStateColour()
        {
            var doc = ParseHtmlWithStyle(
                "input:hover { background-color: #ff0000; }",
                "<input id='i1' type='text' name='name' value='Jane' />");

            using var stream = DocStreams.GetOutputStream("FieldStates_HoverColour.pdf");
            doc.SaveAsPDF(stream);
            stream.Flush();

            stream.Position = 0;
            using var sr = new StreamReader(stream, System.Text.Encoding.GetEncoding("ISO-8859-1"), false, 4096, leaveOpen: true);
            var text = sr.ReadToEnd();

            // #ff0000 -> full-intensity red fill (1.0000 0.0000 0.0000 rg), matching the state colour.
            StringAssert.Contains(text, "1.0000 0.0000 0.0000 rg");
        }
    }
}
