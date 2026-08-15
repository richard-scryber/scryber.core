using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Html.Components;
using Scryber.PDF;
using Scryber.PDF.Native;

namespace Scryber.Core.UnitTests.Html
{
    /// <summary>
    /// Tests for Phase 4 - the unsigned signature field placeholder. No cryptographic signing,
    /// /ByteRange, or incremental-update writer - just a correctly-flagged /FT /Sig field with
    /// no forced placeholder text, so a reader draws its own "click to sign" UI.
    /// </summary>
    [TestClass()]
    public class HTMLSignatureField_Tests
    {
        private static Document ParseHtml(string bodyHtml)
        {
            var src = "<html><body>" + bodyHtml + "</body></html>";
            return Document.ParseHtmlDocument(new StringReader(src));
        }

        private static PDFDictionary GetDictionary(PDFReader reader, PDFObjectRef oref)
        {
            Assert.IsNotNull(oref);
            var obj = reader.GetObject(oref);
            Assert.IsNotNull(obj, "Could not resolve object " + oref);
            return obj.GetContents() as PDFDictionary;
        }

        private static PDFDictionary GetAcroForm(PDFReader reader)
        {
            var catalog = GetDictionary(reader, reader.DocumentCatalogRef.Reference);
            Assert.IsTrue(catalog.TryGetValue("AcroForm", out var acroFormEntry));
            return GetDictionary(reader, acroFormEntry as PDFObjectRef);
        }

        [TestMethod()]
        [TestCategory("Html-Forms")]
        public void SignatureField_NoValue_HasNoProxyText()
        {
            var doc = ParseHtml("<input id='s1' type='signature' name='sig' />");
            var input = doc.FindAComponentById("s1") as HTMLInput;

            using (var ms = DocStreams.GetOutputStream("Signature_NoProxyText.pdf"))
            {
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(FormInputFieldType.Signature, input.FieldType);
            Assert.AreEqual(string.Empty, input.Value, "A signature field should never get placeholder text forced into it");
        }

        [TestMethod()]
        [TestCategory("Html-Forms")]
        public void SignatureField_SetsSigFlags_EndToEnd()
        {
            var doc = ParseHtml("<input id='s1' type='signature' name='sig' />");

            using var stream = DocStreams.GetOutputStream("Signature_SigFlags.pdf");
            doc.SaveAsPDF(stream);
            stream.Flush();

            var reader = PDFReader.Create(stream, new Scryber.Logging.DoNothingTraceLog(TraceRecordLevel.Diagnostic));
            var acroForm = GetAcroForm(reader);

            Assert.IsTrue(acroForm.TryGetValue("Fields", out var fieldsEntry));
            var fields = fieldsEntry as PDFArray;
            var widget = GetDictionary(reader, fields[0] as PDFObjectRef);
            widget.TryGetValue("FT", out var ft);
            Assert.AreEqual("Sig", (ft as PDFName)?.Value);

            Assert.IsTrue(acroForm.TryGetValue("SigFlags", out var sigFlags));
            Assert.AreEqual(1L, ((PDFNumber)sigFlags).Value);

            Assert.IsFalse(widget.ContainsKey((PDFName)"V"), "An unsigned signature field should have no /V at all");
        }

        [TestMethod()]
        [TestCategory("Html-Forms")]
        public void NoSignatureField_NoSigFlags()
        {
            var doc = ParseHtml("<input id='t1' type='text' name='name' value='Jane' />");

            using var stream = DocStreams.GetOutputStream("NoSignature_NoSigFlags.pdf");
            doc.SaveAsPDF(stream);
            stream.Flush();

            var reader = PDFReader.Create(stream, new Scryber.Logging.DoNothingTraceLog(TraceRecordLevel.Diagnostic));
            var acroForm = GetAcroForm(reader);

            Assert.IsFalse(acroForm.ContainsKey((PDFName)"SigFlags"));
        }

        [TestMethod()]
        [TestCategory("Html-Forms")]
        public void SignatureField_InForm_StillSetsSigFlags()
        {
            // The signature field is grouped under the form's /Kids (Phase 1) - SigFlags detection
            // must recurse into the group, not just check the root /Fields entries directly.
            var doc = ParseHtml("<form name='agreement'><input id='s1' type='signature' name='sig' /></form>");

            using var stream = DocStreams.GetOutputStream("Signature_InForm.pdf");
            doc.SaveAsPDF(stream);
            stream.Flush();

            var reader = PDFReader.Create(stream, new Scryber.Logging.DoNothingTraceLog(TraceRecordLevel.Diagnostic));
            var acroForm = GetAcroForm(reader);

            Assert.IsTrue(acroForm.TryGetValue("SigFlags", out var sigFlags));
            Assert.AreEqual(1L, ((PDFNumber)sigFlags).Value);
        }
    }
}
