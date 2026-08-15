using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Html.Components;

namespace Scryber.Core.UnitTests.Html
{
    /// <summary>
    /// Tests for real HTML &lt;input&gt; parsing - previously "input" was in
    /// HTMLParserSettings.DefaultSkipOverTags (entirely dropped, contents included)
    /// and had no registered factory, so HTML-sourced &lt;input&gt; produced nothing.
    /// </summary>
    [TestClass()]
    public class HTMLFormFields_Tests
    {
        private HTMLInput ParseSingleInput(string inputTag)
        {
            // Document.ParseDocument always uses the strict XML/reflective parser (Scryber.Generation.XMLParser),
            // regardless of xmlns - it never reaches HTMLParserComponentFactory/HTMLParserSettings at all.
            // Document.ParseHtmlDocument is the one that actually exercises the loose HTML parser this test targets.
            var src = "<html><body>" + inputTag + "</body></html>";
            var doc = Document.ParseHtmlDocument(new StringReader(src));
            var matches = doc.FindMatches("input");
            Assert.AreEqual(1, matches.Count, "Expected exactly one input to be parsed");
            return matches[0] as HTMLInput;
        }

        [TestMethod()]
        [TestCategory("Html-Forms")]
        public void Input_BasicTextType_ParsesNameValueType()
        {
            var input = ParseSingleInput("<input id='f1' type='text' name='first-name' value='Jane' />");

            Assert.IsNotNull(input, "Input should have been parsed as an HTMLInput");
            Assert.AreEqual("f1", input.ID);
            Assert.AreEqual("first-name", input.Name);
            Assert.AreEqual("Jane", input.Value);
            Assert.AreEqual(FormInputFieldType.Text, input.FieldType);
        }

        [TestMethod()]
        [TestCategory("Html-Forms")]
        public void Input_NoTypeAttribute_DefaultsToText()
        {
            var input = ParseSingleInput("<input name='x' value='y' />");
            Assert.AreEqual(FormInputFieldType.Text, input.FieldType);
        }

        [TestMethod()]
        [TestCategory("Html-Forms")]
        public void Input_PasswordType_SetsPasswordOption()
        {
            var input = ParseSingleInput("<input type='password' name='pwd' />");
            Assert.AreEqual(FormInputFieldType.Text, input.FieldType);
            Assert.IsTrue((input.Options & FormFieldOptions.Password) == FormFieldOptions.Password, "Password option flag should be set");
        }

        [TestMethod()]
        [TestCategory("Html-Forms")]
        public void Input_FileType_SetsFileOption()
        {
            var input = ParseSingleInput("<input type='file' name='upload' />");
            Assert.AreEqual(FormInputFieldType.Text, input.FieldType);
            Assert.IsTrue((input.Options & FormFieldOptions.File) == FormFieldOptions.File, "File option flag should be set");
        }

        [TestMethod()]
        [TestCategory("Html-Forms")]
        public void Input_SubmitResetButtonTypes_MapToButtonFieldType()
        {
            Assert.AreEqual(FormInputFieldType.Button, ParseSingleInput("<input type='submit' name='go' />").FieldType);
            Assert.AreEqual(FormInputFieldType.Button, ParseSingleInput("<input type='reset' name='clear' />").FieldType);
            Assert.AreEqual(FormInputFieldType.Button, ParseSingleInput("<input type='button' name='click' />").FieldType);
        }

        [TestMethod()]
        [TestCategory("Html-Forms")]
        public void Input_SignatureType_MapsToSignatureFieldType()
        {
            var input = ParseSingleInput("<input type='signature' name='sig' />");
            Assert.AreEqual(FormInputFieldType.Signature, input.FieldType);
        }

        [TestMethod()]
        [TestCategory("Html-Forms")]
        public void Input_CheckboxRadioTypes_FallBackToTextWithoutError()
        {
            // Checkbox/radio widget support is not yet implemented (later phase of work) -
            // this asserts the safe fallback behaviour rather than a crash or silent mismatch.
            Assert.AreEqual(FormInputFieldType.Text, ParseSingleInput("<input type='checkbox' name='agree' />").FieldType);
            Assert.AreEqual(FormInputFieldType.Text, ParseSingleInput("<input type='radio' name='choice' />").FieldType);
        }

        [TestMethod()]
        [TestCategory("Html-Forms")]
        public void Input_GeneratesRealPDFOutput_EndToEnd()
        {
            var src = @"<html>
<body>
    <p>Name:</p>
    <input id='name-field' type='text' name='name' value='Default Name' style='width: 200pt;' />
</body>
</html>";

            var doc = Document.ParseHtmlDocument(new StringReader(src));
            using (var ms = DocStreams.GetOutputStream("HTMLFormFields_EndToEnd.pdf"))
            {
                doc.SaveAsPDF(ms);
            }

            var input = doc.FindAComponentById("name-field") as HTMLInput;
            Assert.IsNotNull(input);
            Assert.IsNotNull(input.Widget, "The field widget should have been registered during layout");
            Assert.AreEqual("name", input.Widget.Name);
        }
    }
}
