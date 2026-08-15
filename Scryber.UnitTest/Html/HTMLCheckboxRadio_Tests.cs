using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Html.Components;
using Scryber.PDF;
using Scryber.PDF.Native;

namespace Scryber.Core.UnitTests.Html
{
    /// <summary>
    /// Tests for checkbox/radio widgets - the one field type needing a nested /AP /N
    /// appearance sub-dictionary (Off/on-state) plus /AS, rather than the flat /AP other
    /// field types use. Structure is verified against the real generated PDF via PDFReader.
    /// </summary>
    [TestClass()]
    public class HTMLCheckboxRadio_Tests
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
        public void Checkbox_Unchecked_BuildsCheckWidget()
        {
            var doc = ParseHtml("<input id='c1' type='checkbox' name='agree' />");
            var input = doc.FindAComponentById("c1") as HTMLInput;

            using (var ms = DocStreams.GetOutputStream("Checkbox_Unchecked.pdf"))
            {
                doc.SaveAsPDF(ms);
            }

            Assert.IsInstanceOfType(input.Widget, typeof(PDFAcrobatFormCheckWidget));
            var check = (PDFAcrobatFormCheckWidget)input.Widget;
            Assert.IsFalse(check.IsChecked);
            Assert.AreEqual(FormButtonFieldType.CheckBox, check.ButtonType);
        }

        [TestMethod()]
        [TestCategory("Html-Forms")]
        public void Checkbox_Checked_SetsIsCheckedAndOnStateName()
        {
            var doc = ParseHtml("<input id='c1' type='checkbox' name='agree' value='yes' checked='checked' />");
            var input = doc.FindAComponentById("c1") as HTMLInput;

            using (var ms = DocStreams.GetOutputStream("Checkbox_Checked.pdf"))
            {
                doc.SaveAsPDF(ms);
            }

            var check = (PDFAcrobatFormCheckWidget)input.Widget;
            Assert.IsTrue(check.IsChecked);
            Assert.AreEqual("yes", check.OnStateName);
        }

        [TestMethod()]
        [TestCategory("Html-Forms")]
        public void Checkbox_GeneratesNestedAPDictionary_EndToEnd()
        {
            var doc = ParseHtml("<input id='c1' type='checkbox' name='agree' value='yes' checked='checked' />");

            using var stream = DocStreams.GetOutputStream("Checkbox_NestedAP.pdf");
            doc.SaveAsPDF(stream);
            stream.Flush();

            var reader = PDFReader.Create(stream, new Scryber.Logging.DoNothingTraceLog(TraceRecordLevel.Diagnostic));
            var acroForm = GetAcroForm(reader);

            Assert.IsTrue(acroForm.TryGetValue("Fields", out var fieldsEntry));
            var fields = fieldsEntry as PDFArray;
            Assert.AreEqual(1, fields.Count);

            var widget = GetDictionary(reader, fields[0] as PDFObjectRef);
            Assert.IsTrue(widget.TryGetValue("FT", out var ft));
            Assert.AreEqual("Btn", (ft as PDFName)?.Value);

            Assert.IsTrue(widget.TryGetValue("AS", out var asEntry));
            Assert.AreEqual("yes", (asEntry as PDFName)?.Value);

            Assert.IsTrue(widget.TryGetValue("V", out var vEntry));
            Assert.AreEqual("yes", (vEntry as PDFName)?.Value, "/V on a checkbox should be a Name, not a string");

            Assert.IsTrue(widget.TryGetValue("AP", out var apEntry));
            var ap = apEntry as PDFDictionary;
            Assert.IsTrue(ap.TryGetValue("N", out var nEntry));
            var nDict = nEntry as PDFDictionary;
            Assert.IsNotNull(nDict, "/AP /N should be a nested dictionary, not a single stream ref");
            Assert.IsTrue(nDict.ContainsKey((PDFName)"Off"));
            Assert.IsTrue(nDict.ContainsKey((PDFName)"yes"));
        }

        [TestMethod()]
        [TestCategory("Html-Forms")]
        public void Radio_GroupSharesKidsGroup_WithPerWidgetCheckedState()
        {
            var doc = ParseHtml(
                "<form name='choice'>" +
                "<input id='r1' type='radio' name='opt' value='A' />" +
                "<input id='r2' type='radio' name='opt' value='B' checked='checked' />" +
                "</form>");

            using var stream = DocStreams.GetOutputStream("Radio_Group.pdf");
            doc.SaveAsPDF(stream);
            stream.Flush();

            var reader = PDFReader.Create(stream, new Scryber.Logging.DoNothingTraceLog(TraceRecordLevel.Diagnostic));
            var acroForm = GetAcroForm(reader);

            Assert.IsTrue(acroForm.TryGetValue("Fields", out var fieldsEntry));
            var fields = fieldsEntry as PDFArray;
            Assert.AreEqual(1, fields.Count, "Both radios should share one group node");

            var group = GetDictionary(reader, fields[0] as PDFObjectRef);
            Assert.IsTrue(group.TryGetValue("Kids", out var kidsEntry));
            var kids = kidsEntry as PDFArray;
            Assert.AreEqual(2, kids.Count);

            var states = new System.Collections.Generic.List<string>();
            foreach (var kid in kids)
            {
                var widget = GetDictionary(reader, kid as PDFObjectRef);
                widget.TryGetValue("AS", out var asEntry);
                states.Add((asEntry as PDFName)?.Value);
            }

            CollectionAssert.AreEquivalent(new[] { "Off", "B" }, states);
        }
    }
}
