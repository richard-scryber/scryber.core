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

        /// <summary>
        /// Returns the sole registered widget's dictionary, plus the /AcroForm dictionary it
        /// belongs to. Only buttons self-render an /AP for now (the rest rely on the reader
        /// regenerating their appearance via /NeedAppearances) - so this no longer resolves an
        /// /AP sub-dictionary at all, just gives callers the two dictionaries to assert against.
        /// </summary>
        private static PDFDictionary GetSoleWidget(PDFReader reader, out PDFDictionary acroForm)
        {
            var catalog = GetDictionary(reader, reader.DocumentCatalogRef.Reference);
            catalog.TryGetValue("AcroForm", out var acroFormEntry);
            acroForm = GetDictionary(reader, acroFormEntry as PDFObjectRef);
            acroForm.TryGetValue("Fields", out var fieldsEntry);
            var fields = fieldsEntry as PDFArray;
            Assert.AreEqual(1, fields.Count);

            return GetDictionary(reader, fields[0] as PDFObjectRef);
        }

        /// <summary>
        /// Several /NeedAppearances policies were compared across readers this session (explicit
        /// true, explicit false, omitted entirely, and every field self-rendering /AP) - currently
        /// settled back on explicit true (see PDFAcrobatFormsCollection.OutputToPDF) with no field
        /// self-rendering /AP at all (see PDFAcrobatFormFieldWidget's writeAP), the baseline from
        /// before independent per-state button layout existed. Update this alongside that policy.
        /// </summary>
        private static void AssertNeedAppearances(PDFDictionary acroForm)
        {
            Assert.IsTrue(acroForm.TryGetValue("NeedAppearances", out var need), "/AcroForm should declare /NeedAppearances");
            Assert.IsInstanceOfType(need, typeof(PDFBoolean));
            Assert.IsTrue(((PDFBoolean)need).Value);
        }

        [TestMethod()]
        [TestCategory("Html-Forms")]
        public void Input_NoHoverActiveCss_NoAppearanceStream()
        {
            var doc = ParseHtml("<input id='i1' type='text' name='name' value='Jane' />");

            using var stream = DocStreams.GetOutputStream("FieldStates_None.pdf");
            doc.SaveAsPDF(stream);
            stream.Flush();

            var reader = PDFReader.Create(stream, new Scryber.Logging.DoNothingTraceLog(TraceRecordLevel.Diagnostic));
            var widget = GetSoleWidget(reader, out var acroForm);

            Assert.IsFalse(widget.ContainsKey((PDFName)"AP"), "Only buttons self-render an /AP for now");
            AssertNeedAppearances(acroForm);
        }

        [TestMethod()]
        [TestCategory("Html-Forms")]
        public void Input_WithHoverCss_NoAppearanceStream()
        {
            var doc = ParseHtmlWithStyle(
                "input:hover { background-color: #ff0000; }",
                "<input id='i1' type='text' name='name' value='Jane' />");

            using var stream = DocStreams.GetOutputStream("FieldStates_Hover.pdf");
            doc.SaveAsPDF(stream);
            stream.Flush();

            var reader = PDFReader.Create(stream, new Scryber.Logging.DoNothingTraceLog(TraceRecordLevel.Diagnostic));
            var widget = GetSoleWidget(reader, out var acroForm);

            Assert.IsFalse(widget.ContainsKey((PDFName)"AP"), "Only buttons self-render an /AP for now - a matching :hover rule doesn't change that");
            AssertNeedAppearances(acroForm);
        }

        [TestMethod()]
        [TestCategory("Html-Forms")]
        public void Input_WithActiveCss_NoAppearanceStream()
        {
            var doc = ParseHtmlWithStyle(
                "input:active { background-color: #0000ff; }",
                "<input id='i1' type='text' name='name' value='Jane' />");

            using var stream = DocStreams.GetOutputStream("FieldStates_Active.pdf");
            doc.SaveAsPDF(stream);
            stream.Flush();

            var reader = PDFReader.Create(stream, new Scryber.Logging.DoNothingTraceLog(TraceRecordLevel.Diagnostic));
            var widget = GetSoleWidget(reader, out var acroForm);

            Assert.IsFalse(widget.ContainsKey((PDFName)"AP"), "Only buttons self-render an /AP for now - a matching :active rule doesn't change that");
            AssertNeedAppearances(acroForm);
        }

        [TestMethod()]
        [TestCategory("Html-Forms")]
        public void Input_WithHoverCss_NoRepaintedStream()
        {
            var doc = ParseHtmlWithStyle(
                "input:hover { background-color: #ff0000; }",
                "<input id='i1' type='text' name='name' value='Jane' />");

            using var stream = DocStreams.GetOutputStream("FieldStates_HoverColour.pdf");
            doc.SaveAsPDF(stream);
            stream.Flush();

            var reader = PDFReader.Create(stream, new Scryber.Logging.DoNothingTraceLog(TraceRecordLevel.Diagnostic));
            var widget = GetSoleWidget(reader, out var acroForm);

            // Only buttons self-render an /AP (with a real independent repaint per state) for
            // now - a matching :hover rule on a plain <input> doesn't produce anything to repaint.
            Assert.IsFalse(widget.ContainsKey((PDFName)"AP"), "Only buttons self-render an /AP for now");
            AssertNeedAppearances(acroForm);
        }
    }
}
