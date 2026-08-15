using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Html.Components;
using Scryber.PDF.Native;

namespace Scryber.Core.UnitTests.Html
{
    /// <summary>
    /// Tests for the /Kids grouping ("Form" revival) of AcroForm fields: fields inside a
    /// &lt;form&gt; should register as terminal widgets under that form's own group node
    /// (with /T and /Kids), rather than directly in the document's root /Fields array.
    /// Structure is asserted by walking the real generated PDF with PDFReader, per the
    /// established pattern for AcroForm dictionary-level verification.
    /// </summary>
    [TestClass()]
    public class HTMLFormGrouping_Tests
    {
        private static Stream GenerateAndReopen(string bodyHtml, string outputName)
        {
            var src = "<html><body>" + bodyHtml + "</body></html>";
            var doc = Document.ParseHtmlDocument(new StringReader(src));

            var ms = DocStreams.GetOutputStream(outputName);
            doc.SaveAsPDF(ms);
            ms.Flush();
            return ms;
        }

        private static PDFDictionary GetDictionary(PDFReader reader, PDFObjectRef oref)
        {
            Assert.IsNotNull(oref, "Expected a valid object reference");
            var obj = reader.GetObject(oref);
            Assert.IsNotNull(obj, "Could not resolve object " + oref);
            return obj.GetContents() as PDFDictionary;
        }

        private static PDFDictionary GetAcroForm(PDFReader reader)
        {
            var catalog = GetDictionary(reader, reader.DocumentCatalogRef.Reference);
            Assert.IsTrue(catalog.TryGetValue("AcroForm", out var acroFormEntry), "Catalog should have an /AcroForm entry");
            return GetDictionary(reader, acroFormEntry as PDFObjectRef);
        }

        private static PDFArray GetArray(PDFDictionary dict, string key)
        {
            Assert.IsTrue(dict.TryGetValue(key, out var entry), "Expected a /" + key + " entry");
            var arr = entry as PDFArray;
            Assert.IsNotNull(arr, "/" + key + " should be an array");
            return arr;
        }

        [TestMethod()]
        [TestCategory("Html-Forms")]
        public void Input_WithFormAncestor_GroupsUnderKids()
        {
            using var stream = GenerateAndReopen(
                "<form name='myform'><input id='f1' name='field1' value='A' /></form>",
                "HTMLFormGrouping_SingleField.pdf");

            var reader = PDFReader.Create(stream, new Scryber.Logging.DoNothingTraceLog(TraceRecordLevel.Diagnostic));
            var acroForm = GetAcroForm(reader);

            var rootFields = GetArray(acroForm, "Fields");
            Assert.AreEqual(1, rootFields.Count, "Root /Fields should contain exactly the form's own group node");

            var group = GetDictionary(reader, rootFields[0] as PDFObjectRef);
            Assert.IsTrue(group.TryGetValue("T", out var groupName), "Group node should have a /T name");
            Assert.AreEqual("myform", (groupName as PDFString)?.Value);
            Assert.IsFalse(group.ContainsKey((PDFName)"Subtype"), "The group node itself should not be a Widget");

            var kids = GetArray(group, "Kids");
            Assert.AreEqual(1, kids.Count, "The form's group should have exactly one kid widget");

            var widget = GetDictionary(reader, kids[0] as PDFObjectRef);
            Assert.IsTrue(widget.TryGetValue("Subtype", out var subtype));
            Assert.AreEqual("Widget", (subtype as PDFName)?.Value);
            Assert.IsTrue(widget.TryGetValue("T", out var fieldName));
            Assert.AreEqual("field1", (fieldName as PDFString)?.Value);
        }

        [TestMethod()]
        [TestCategory("Html-Forms")]
        public void Input_MultipleFieldsInForm_ShareOneKidsGroup()
        {
            using var stream = GenerateAndReopen(
                "<form name='grp'><input name='a' value='1' /><input name='b' value='2' /></form>",
                "HTMLFormGrouping_MultiField.pdf");

            var reader = PDFReader.Create(stream, new Scryber.Logging.DoNothingTraceLog(TraceRecordLevel.Diagnostic));
            var acroForm = GetAcroForm(reader);

            var rootFields = GetArray(acroForm, "Fields");
            Assert.AreEqual(1, rootFields.Count, "Both fields should share a single group node in the root /Fields");

            var group = GetDictionary(reader, rootFields[0] as PDFObjectRef);
            var kids = GetArray(group, "Kids");
            Assert.AreEqual(2, kids.Count, "Both fields should appear as kids of the one group");

            var names = new System.Collections.Generic.List<string>();
            foreach (var kidObj in kids)
            {
                var widget = GetDictionary(reader, kidObj as PDFObjectRef);
                widget.TryGetValue("T", out var fieldName);
                names.Add((fieldName as PDFString)?.Value);
            }
            CollectionAssert.AreEquivalent(new[] { "a", "b" }, names);
        }

        [TestMethod()]
        [TestCategory("Html-Forms")]
        public void Input_WithoutFormAncestor_RegistersDirectlyAsWidget()
        {
            using var stream = GenerateAndReopen(
                "<input id='f1' name='standalone' value='A' />",
                "HTMLFormGrouping_NoForm.pdf");

            var reader = PDFReader.Create(stream, new Scryber.Logging.DoNothingTraceLog(TraceRecordLevel.Diagnostic));
            var acroForm = GetAcroForm(reader);

            var rootFields = GetArray(acroForm, "Fields");
            Assert.AreEqual(1, rootFields.Count);

            var widget = GetDictionary(reader, rootFields[0] as PDFObjectRef);
            Assert.IsTrue(widget.TryGetValue("Subtype", out var subtype), "A field with no Form ancestor should register directly as a Widget");
            Assert.AreEqual("Widget", (subtype as PDFName)?.Value);
            Assert.IsTrue(widget.TryGetValue("T", out var fieldName));
            Assert.AreEqual("standalone", (fieldName as PDFString)?.Value);
        }
    }
}
