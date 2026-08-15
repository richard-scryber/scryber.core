using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Html.Components;
using Scryber.PDF;
using Scryber.PDF.Native;

namespace Scryber.Core.UnitTests.Html
{
    /// <summary>
    /// Tests for Phase 3 submission actions - submit/reset buttons attaching a PDFSubmitFormAction
    /// or PDFResetFormAction (written as /A on the field widget) built from the parent Form's
    /// action=/method= attributes. Structure is verified against the real generated PDF.
    /// </summary>
    [TestClass()]
    public class HTMLFormSubmit_Tests
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

        /// <summary>
        /// Returns the sole field widget, descending through a Phase 1 /Kids group node if the
        /// field is inside a &lt;form&gt; (in which case the root /Fields entry is the group, not
        /// the widget itself) - the field is expected to be the only one in the document either way.
        /// </summary>
        private static PDFDictionary GetSoleWidget(PDFReader reader)
        {
            var acroForm = GetAcroForm(reader);
            Assert.IsTrue(acroForm.TryGetValue("Fields", out var fieldsEntry));
            var fields = fieldsEntry as PDFArray;
            Assert.AreEqual(1, fields.Count);

            var dict = GetDictionary(reader, fields[0] as PDFObjectRef);
            if (dict.TryGetValue("Kids", out var kidsEntry))
            {
                var kids = kidsEntry as PDFArray;
                Assert.AreEqual(1, kids.Count);
                dict = GetDictionary(reader, kids[0] as PDFObjectRef);
            }
            return dict;
        }

        [TestMethod()]
        [TestCategory("Html-Forms")]
        public void Button_Submit_BuildsSubmitFormAction()
        {
            var doc = ParseHtml("<form action='https://example.com/submit'><button id='b1' name='go'>Save</button></form>");

            using var stream = DocStreams.GetOutputStream("Submit_Button.pdf");
            doc.SaveAsPDF(stream);
            stream.Flush();

            var reader = PDFReader.Create(stream, new Scryber.Logging.DoNothingTraceLog(TraceRecordLevel.Diagnostic));
            var widget = GetSoleWidget(reader);

            Assert.IsTrue(widget.TryGetValue("A", out var actionEntry));
            var action = actionEntry as PDFDictionary;
            Assert.IsNotNull(action, "/A should be an inline dictionary");

            action.TryGetValue("S", out var s);
            Assert.AreEqual("SubmitForm", (s as PDFName)?.Value);

            Assert.IsTrue(action.TryGetValue("F", out var fEntry));
            var fileSpec = fEntry as PDFDictionary;
            Assert.IsNotNull(fileSpec, "/F should be an inline file specification dictionary");
            fileSpec.TryGetValue("F", out var url);
            Assert.AreEqual("https://example.com/submit", (url as PDFString)?.Value);
            fileSpec.TryGetValue("FS", out var fs);
            Assert.AreEqual("URL", (fs as PDFName)?.Value);

            action.TryGetValue("Flags", out var flags);
            Assert.AreEqual(4L, ((PDFNumber)flags).Value, "Default method=post -> ExportFormat flag only");
        }

        [TestMethod()]
        [TestCategory("Html-Forms")]
        public void Button_Submit_MethodGet_SetsGetFlag()
        {
            var doc = ParseHtml("<form action='https://example.com/submit' method='get'><button id='b1' name='go'>Save</button></form>");

            using var stream = DocStreams.GetOutputStream("Submit_Get.pdf");
            doc.SaveAsPDF(stream);
            stream.Flush();

            var reader = PDFReader.Create(stream, new Scryber.Logging.DoNothingTraceLog(TraceRecordLevel.Diagnostic));
            var widget = GetSoleWidget(reader);

            widget.TryGetValue("A", out var actionEntry);
            var action = actionEntry as PDFDictionary;
            action.TryGetValue("Flags", out var flags);
            Assert.AreEqual(12L, ((PDFNumber)flags).Value, "ExportFormat (4) + GetMethod (8)");
        }

        [TestMethod()]
        [TestCategory("Html-Forms")]
        public void Button_Reset_BuildsResetFormAction()
        {
            var doc = ParseHtml("<form action='https://example.com/submit'><button id='b1' name='clear' type='reset'>Clear</button></form>");

            using var stream = DocStreams.GetOutputStream("Reset_Button.pdf");
            doc.SaveAsPDF(stream);
            stream.Flush();

            var reader = PDFReader.Create(stream, new Scryber.Logging.DoNothingTraceLog(TraceRecordLevel.Diagnostic));
            var widget = GetSoleWidget(reader);

            Assert.IsTrue(widget.TryGetValue("A", out var actionEntry));
            var action = actionEntry as PDFDictionary;
            action.TryGetValue("S", out var s);
            Assert.AreEqual("ResetForm", (s as PDFName)?.Value);
            Assert.IsFalse(action.ContainsKey((PDFName)"F"), "/ResetForm should have no /F entry");
        }

        [TestMethod()]
        [TestCategory("Html-Forms")]
        public void Button_NoFormAction_NoActionAttached()
        {
            // A submit button with no Form ancestor action= has nowhere to submit to - no /A
            // should be written at all, rather than an incomplete/broken action dictionary.
            var doc = ParseHtml("<button id='b1' name='go'>Save</button>");

            using var stream = DocStreams.GetOutputStream("Submit_NoAction.pdf");
            doc.SaveAsPDF(stream);
            stream.Flush();

            var reader = PDFReader.Create(stream, new Scryber.Logging.DoNothingTraceLog(TraceRecordLevel.Diagnostic));
            var widget = GetSoleWidget(reader);

            Assert.IsFalse(widget.ContainsKey((PDFName)"A"));
        }

        [TestMethod()]
        [TestCategory("Html-Forms")]
        public void Button_TypeButton_NoActionAttached()
        {
            // An explicit type="button" has no default click behaviour, unlike a bare <button>.
            var doc = ParseHtml("<form action='https://example.com/submit'><button id='b1' name='go' type='button'>Do</button></form>");

            using var stream = DocStreams.GetOutputStream("Button_Plain.pdf");
            doc.SaveAsPDF(stream);
            stream.Flush();

            var reader = PDFReader.Create(stream, new Scryber.Logging.DoNothingTraceLog(TraceRecordLevel.Diagnostic));
            var widget = GetSoleWidget(reader);

            Assert.IsFalse(widget.ContainsKey((PDFName)"A"));
        }

        [TestMethod()]
        [TestCategory("Html-Forms")]
        public void InputSubmit_BuildsSubmitFormAction()
        {
            var doc = ParseHtml("<form action='https://example.com/submit'><input id='i1' type='submit' name='go' value='Go' /></form>");

            using var stream = DocStreams.GetOutputStream("Submit_Input.pdf");
            doc.SaveAsPDF(stream);
            stream.Flush();

            var reader = PDFReader.Create(stream, new Scryber.Logging.DoNothingTraceLog(TraceRecordLevel.Diagnostic));
            var widget = GetSoleWidget(reader);

            Assert.IsTrue(widget.TryGetValue("A", out var actionEntry));
            var action = actionEntry as PDFDictionary;
            action.TryGetValue("S", out var s);
            Assert.AreEqual("SubmitForm", (s as PDFName)?.Value);
        }
    }
}
