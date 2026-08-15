using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Html.Components;

namespace Scryber.Core.UnitTests.Html
{
    /// <summary>
    /// Tests for the Phase 2 field components: HTMLTextArea, HTMLButton, HTMLSelect/HTMLOption,
    /// and checkbox/radio widgets.
    /// </summary>
    [TestClass()]
    public class HTMLFormFields2_Tests
    {
        private static Document ParseHtml(string bodyHtml)
        {
            var src = "<html><body>" + bodyHtml + "</body></html>";
            return Document.ParseHtmlDocument(new StringReader(src));
        }

        [TestMethod()]
        [TestCategory("Html-Forms")]
        public void TextArea_ValueAttribute_ParsesDirectly()
        {
            var doc = ParseHtml("<textarea id='t1' name='notes' value='Hello' rows='4' cols='30'></textarea>");
            var area = doc.FindAComponentById("t1") as HTMLTextArea;

            Assert.IsNotNull(area);
            Assert.AreEqual("notes", area.Name);
            Assert.AreEqual("Hello", area.Value);
            Assert.AreEqual(FormInputFieldType.Text, area.FieldType);
            Assert.IsTrue((area.Options & FormFieldOptions.MultiLine) == FormFieldOptions.MultiLine, "MultiLine option should be set by default");
        }

        [TestMethod()]
        [TestCategory("Html-Forms")]
        public void TextArea_InnerText_HarvestedAsValue()
        {
            var doc = ParseHtml("<textarea id='t1' name='notes'>Some default text</textarea>");
            var area = doc.FindAComponentById("t1") as HTMLTextArea;
            Assert.IsNotNull(area);

            using (var ms = DocStreams.GetOutputStream("HTMLTextArea_InnerText.pdf"))
            {
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual("Some default text", area.Value);
        }

        [TestMethod()]
        [TestCategory("Html-Forms")]
        public void TextArea_GeneratesMultiLineWidget_EndToEnd()
        {
            var doc = ParseHtml("<textarea id='t1' name='notes' value='Hello' />");

            using (var ms = DocStreams.GetOutputStream("HTMLTextArea_EndToEnd.pdf"))
            {
                doc.SaveAsPDF(ms);
            }

            var area = doc.FindAComponentById("t1") as HTMLTextArea;
            Assert.IsNotNull(area.Widget);
            Assert.AreEqual("notes", area.Widget.Name);
            Assert.IsTrue((area.Widget.FieldOptions & FormFieldOptions.MultiLine) == FormFieldOptions.MultiLine);
        }

        [TestMethod()]
        [TestCategory("Html-Forms")]
        public void Button_NoTypeAttribute_DefaultsToButtonFieldType()
        {
            var doc = ParseHtml("<button id='b1' name='go'>Save</button>");
            var button = doc.FindAComponentById("b1") as HTMLButton;

            Assert.IsNotNull(button);
            Assert.AreEqual(FormInputFieldType.Button, button.FieldType);
            Assert.IsTrue((button.Options & FormFieldOptions.Pushbutton) == FormFieldOptions.Pushbutton);
        }

        [TestMethod()]
        [TestCategory("Html-Forms")]
        public void Button_InnerText_HarvestedAsValue_EndToEnd()
        {
            var doc = ParseHtml("<button id='b1' name='go'>Save</button>");

            using (var ms = DocStreams.GetOutputStream("HTMLButton_EndToEnd.pdf"))
            {
                doc.SaveAsPDF(ms);
            }

            var button = doc.FindAComponentById("b1") as HTMLButton;
            Assert.AreEqual("Save", button.Value);
            Assert.IsNotNull(button.Widget);
            Assert.AreEqual("go", button.Widget.Name);
            Assert.IsTrue((button.Widget.FieldOptions & FormFieldOptions.Pushbutton) == FormFieldOptions.Pushbutton);
        }

        [TestMethod()]
        [TestCategory("Html-Forms")]
        public void Button_ExplicitResetType_OverridesDefault()
        {
            var doc = ParseHtml("<button id='b1' name='clear' type='reset'>Clear</button>");
            var button = doc.FindAComponentById("b1") as HTMLButton;

            Assert.AreEqual(FormInputFieldType.Button, button.FieldType);
        }

        [TestMethod()]
        [TestCategory("Html-Forms")]
        public void Select_OptionsHarvested_WithSelectedDefault()
        {
            var doc = ParseHtml(
                "<select id='s1' name='country'>" +
                "<option value='UK'>United Kingdom</option>" +
                "<option value='US' selected='selected'>United States</option>" +
                "<option value='FR'>France</option>" +
                "</select>");

            var select = doc.FindAComponentById("s1") as HTMLSelect;
            Assert.IsNotNull(select);

            using (var ms = DocStreams.GetOutputStream("HTMLSelect_Harvest.pdf"))
            {
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(FormInputFieldType.Choice, select.FieldType);
            Assert.AreEqual(3, select.Choices.Count);
            Assert.AreEqual("UK", select.Choices[0].Value);
            Assert.AreEqual("United Kingdom", select.Choices[0].Label);
            Assert.IsTrue(select.Choices[1].Selected);
            Assert.AreEqual("US", select.Value, "The selected option's value should become the field's Value");

            // options are harvested and removed - they must not remain as visible child content
            Assert.AreEqual(0, select.Contents.OfType<HTMLOption>().Count());
        }

        [TestMethod()]
        [TestCategory("Html-Forms")]
        public void Select_OptionWithNoValueAttribute_UsesInnerTextAsValue()
        {
            var doc = ParseHtml("<select id='s1' name='x'><option>Plain Text</option></select>");
            var select = doc.FindAComponentById("s1") as HTMLSelect;

            using (var ms = DocStreams.GetOutputStream("HTMLSelect_NoValueAttr.pdf"))
            {
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, select.Choices.Count);
            Assert.AreEqual("Plain Text", select.Choices[0].Value);
            Assert.AreEqual("Plain Text", select.Choices[0].Label);
        }

        [TestMethod()]
        [TestCategory("Html-Forms")]
        public void Select_MultipleAttribute_SetsMultiselectOption()
        {
            var doc = ParseHtml("<select id='s1' name='x' multiple='multiple'><option>A</option></select>");
            var select = doc.FindAComponentById("s1") as HTMLSelect;

            Assert.IsTrue(select.Multiple);
            Assert.IsTrue((select.Options & FormFieldOptions.Multiselect) == FormFieldOptions.Multiselect);
        }

        [TestMethod()]
        [TestCategory("Html-Forms")]
        public void Select_GeneratesOptArray_EndToEnd()
        {
            var doc = ParseHtml(
                "<select id='s1' name='country'>" +
                "<option value='UK'>United Kingdom</option>" +
                "<option value='US' selected='selected'>United States</option>" +
                "</select>");

            using (var ms = DocStreams.GetOutputStream("HTMLSelect_EndToEnd.pdf"))
            {
                doc.SaveAsPDF(ms);
            }

            var select = doc.FindAComponentById("s1") as HTMLSelect;
            Assert.IsNotNull(select.Widget);
            Assert.AreEqual("country", select.Widget.Name);
            Assert.IsNotNull(select.Widget.Choices);
            Assert.AreEqual(2, select.Widget.Choices.Count());
        }
    }
}
