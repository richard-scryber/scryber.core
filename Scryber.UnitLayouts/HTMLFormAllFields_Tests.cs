using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.PDF;
using Scryber.PDF.Native;

namespace Scryber.UnitLayouts
{
    /// <summary>
    /// A single, comprehensive layout test covering every AcroForm field type built across
    /// Phases 0-5 (text, password, textarea, select/option, checkbox, radio group, submit
    /// button, reset button) all within one &lt;form&gt;, verifying the real generated PDF
    /// structure - grouping under one /Kids array, per-field /FT and /Ff, the select's /Opt,
    /// the checkbox/radio nested /AP, and the submit/reset /A actions.
    /// </summary>
    [TestClass()]
    public class HTMLFormAllFields_Tests
    {
        private const string TestCategory = "Html-Forms";

        private const string FormTemplatePath = "Content/HTML/Forms/AllFieldTypes.html";

        private static PDFDictionary GetDictionary(PDFReader reader, PDFObjectRef oref)
        {
            Assert.IsNotNull(oref);
            var obj = reader.GetObject(oref);
            Assert.IsNotNull(obj, "Could not resolve object " + oref);
            return obj.GetContents() as PDFDictionary;
        }

        private static string NameOf(PDFDictionary dict)
        {
            dict.TryGetValue("T", out var t);
            return (t as PDFString)?.Value;
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void AllFieldTypes_InOneForm_GenerateCorrectStructure()
        {
            var template = DocStreams.AssertGetTemplatePath(FormTemplatePath);
            var doc = Document.ParseHtmlDocument(template);

            using var stream = DocStreams.GetOutputStream("HTMLFormAllFields_2.pdf");
            //doc.RenderOptions.Compression = OutputCompressionType.None;
            doc.SaveAsPDF(stream);
            stream.Flush();

            var reader = PDFReader.Create(stream, new Scryber.Logging.DoNothingTraceLog(TraceRecordLevel.Diagnostic));

            var catalog = GetDictionary(reader, reader.DocumentCatalogRef.Reference);
            Assert.IsTrue(catalog.TryGetValue("AcroForm", out var acroFormEntry));
            var acroForm = GetDictionary(reader, acroFormEntry as PDFObjectRef);

            // Every field is inside the one <form>, so the root /Fields should hold just its
            // single group node - not each field flattened directly into it.
            Assert.IsTrue(acroForm.TryGetValue("Fields", out var fieldsEntry));
            var rootFields = fieldsEntry as PDFArray;
            Assert.AreEqual(1, rootFields.Count, "All fields share one <form> - root /Fields should hold just its group node");

            var group = GetDictionary(reader, rootFields[0] as PDFObjectRef);
            group.TryGetValue("T", out var groupName);
            Assert.AreEqual("registration", (groupName as PDFString)?.Value);

            Assert.IsTrue(group.TryGetValue("Kids", out var kidsEntry));
            var kids = kidsEntry as PDFArray;
            Assert.AreEqual(9, kids.Count, "text, password, textarea, select, checkbox, 2 radios, submit, reset");

            var fields = new List<PDFDictionary>();
            foreach (var kid in kids)
                fields.Add(GetDictionary(reader, kid as PDFObjectRef));

            // ---- text ----
            var text = fields.Single(f => NameOf(f) == "fullname");
            text.TryGetValue("FT", out var textFt);
            Assert.AreEqual("Tx", (textFt as PDFName)?.Value);
            text.TryGetValue("V", out var textV);
            Assert.AreEqual("Jane Doe", (textV as PDFString)?.Value);

            // ---- password ----
            var pwd = fields.Single(f => NameOf(f) == "pwd");
            pwd.TryGetValue("FT", out var pwdFt);
            Assert.AreEqual("Tx", (pwdFt as PDFName)?.Value);
            pwd.TryGetValue("Ff", out var pwdFf);
            Assert.AreEqual(8192L, ((PDFNumber)pwdFf).Value, "Password flag");

            // ---- textarea ----
            var notes = fields.Single(f => NameOf(f) == "notes");
            notes.TryGetValue("FT", out var notesFt);
            Assert.AreEqual("Tx", (notesFt as PDFName)?.Value);
            notes.TryGetValue("Ff", out var notesFf);
            Assert.AreEqual(4096L, ((PDFNumber)notesFf).Value, "MultiLine flag");
            notes.TryGetValue("V", out var notesV);
            Assert.AreEqual("Some notes", (notesV as PDFString)?.Value);

            // ---- select ----
            var country = fields.Single(f => NameOf(f) == "country");
            country.TryGetValue("FT", out var countryFt);
            Assert.AreEqual("Ch", (countryFt as PDFName)?.Value);
            country.TryGetValue("Ff", out var countryFf);
            Assert.AreEqual(131072L, ((PDFNumber)countryFf).Value, "Combo flag - a plain <select> is a dropdown");
            country.TryGetValue("V", out var countryV);
            Assert.AreEqual("US", (countryV as PDFString)?.Value);
            Assert.IsTrue(country.TryGetValue("Opt", out var optEntry));
            Assert.AreEqual(3, (optEntry as PDFArray).Count);

            // ---- checkbox ----
            var agree = fields.Single(f => NameOf(f) == "agree");
            agree.TryGetValue("FT", out var agreeFt);
            Assert.AreEqual("Btn", (agreeFt as PDFName)?.Value);
            agree.TryGetValue("AS", out var agreeAs);
            Assert.AreEqual("yes", (agreeAs as PDFName)?.Value);
            Assert.IsTrue(agree.TryGetValue("AP", out var agreeApEntry));
            var agreeAp = agreeApEntry as PDFDictionary;
            Assert.IsTrue(agreeAp.TryGetValue("N", out var agreeNEntry));
            var agreeN = agreeNEntry as PDFDictionary;
            Assert.IsTrue(agreeN.ContainsKey((PDFName)"Off"));
            Assert.IsTrue(agreeN.ContainsKey((PDFName)"yes"));

            // ---- radio group (both share the name "choice") ----
            var radios = fields.Where(f => NameOf(f) == "choice").ToList();
            Assert.AreEqual(2, radios.Count);
            foreach (var radio in radios)
            {
                radio.TryGetValue("FT", out var radioFt);
                Assert.AreEqual("Btn", (radioFt as PDFName)?.Value);
                radio.TryGetValue("Ff", out var radioFf);
                Assert.AreEqual(32768L, ((PDFNumber)radioFf).Value, "Radio flag");
            }
            var radioStates = radios.Select(r =>
            {
                r.TryGetValue("AS", out var asEntry);
                return (asEntry as PDFName)?.Value;
            }).ToList();
            CollectionAssert.AreEquivalent(new[] { "Off", "B" }, radioStates, "Only the checked radio (B) should be in its on-state");

            // ---- submit button ----
            var submit = fields.Single(f => NameOf(f) == "go");
            submit.TryGetValue("FT", out var submitFt);
            Assert.AreEqual("Btn", (submitFt as PDFName)?.Value);
            submit.TryGetValue("Ff", out var submitFf);
            Assert.AreEqual(65536L, ((PDFNumber)submitFf).Value, "Pushbutton flag");
            Assert.IsTrue(submit.TryGetValue("MK", out var submitMkEntry));
            var submitMk = submitMkEntry as PDFDictionary;
            submitMk.TryGetValue("CA", out var submitCa);
            Assert.AreEqual("Save", (submitCa as PDFString)?.Value, "/MK /CA is what readers show as a pushbutton's caption, not /V or /AP content");
            Assert.IsTrue(submit.TryGetValue("A", out var submitAEntry));
            var submitA = submitAEntry as PDFDictionary;
            submitA.TryGetValue("S", out var submitS);
            Assert.AreEqual("SubmitForm", (submitS as PDFName)?.Value);
            Assert.IsTrue(submitA.TryGetValue("F", out var submitFEntry));
            var submitFileSpec = submitFEntry as PDFDictionary;
            submitFileSpec.TryGetValue("F", out var submitUrl);
            Assert.AreEqual("https://example.com/submit", (submitUrl as PDFString)?.Value);

            // ---- reset button ----
            var reset = fields.Single(f => NameOf(f) == "clear");
            reset.TryGetValue("FT", out var resetFt);
            Assert.AreEqual("Btn", (resetFt as PDFName)?.Value);
            Assert.IsTrue(reset.TryGetValue("MK", out var resetMkEntry));
            var resetMk = resetMkEntry as PDFDictionary;
            resetMk.TryGetValue("CA", out var resetCa);
            Assert.AreEqual("Clear", (resetCa as PDFString)?.Value);
            Assert.IsTrue(reset.TryGetValue("A", out var resetAEntry));
            var resetA = resetAEntry as PDFDictionary;
            resetA.TryGetValue("S", out var resetS);
            Assert.AreEqual("ResetForm", (resetS as PDFName)?.Value);
            Assert.IsFalse(resetA.ContainsKey((PDFName)"F"));

            // No signature field anywhere in this form.
            Assert.IsFalse(acroForm.ContainsKey((PDFName)"SigFlags"));
        }
    }
}
