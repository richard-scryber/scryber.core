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
        public void Select_OptionsBoundViaEachHelper_AreHarvested()
        {
            var src = @"<html><body>
                <select id='s1' name='country'>
                {{#each model.countries}}
                    <option value='{{this.code}}'>{{this.name}}</option>
                {{/each}}
                </select>
                </body></html>";

            var doc = Document.ParseHtmlDocument(new StringReader(src));
            doc.Params["model"] = new
            {
                countries = new[]
                {
                    new { code = "UK", name = "United Kingdom" },
                    new { code = "US", name = "United States" },
                    new { code = "FR", name = "France" }
                }
            };

            var select = doc.FindAComponentById("s1") as HTMLSelect;
            Assert.IsNotNull(select);

            using (var ms = DocStreams.GetOutputStream("HTMLSelect_EachBound.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(3, select.Choices.Count);
            Assert.AreEqual("UK", select.Choices[0].Value);
            Assert.AreEqual("United Kingdom", select.Choices[0].Label);
            Assert.AreEqual("US", select.Choices[1].Value);
            Assert.AreEqual("FR", select.Choices[2].Value);
        }
        
        [TestMethod()]
        [TestCategory("Html-Forms")]
        public void SelectMultiple_OptionsBoundViaEachHelper_AreHarvested()
        {
            var src = @"<html><body>
                <select id='s1' name='country' multiple='multiple' value='{{model.countries[1]}}' size='{{count(model.countries)}}'>
                {{#each model.countries}}
                    <option value='{{this.code}}'>{{this.name}}</option>
                {{/each}}
                </select>
                </body></html>";

            var doc = Document.ParseHtmlDocument(new StringReader(src));
            doc.Params["model"] = new
            {
                countries = new[]
                {
                    new { code = "UK", name = "United Kingdom" },
                    new { code = "US", name = "United States" },
                    new { code = "FR", name = "France" },
                    new { code="DK", name = "Germany"},
                    new { code="ES", name = "Spain"},
                    
                }
            };

            var select = doc.FindAComponentById("s1") as HTMLSelect;
            Assert.IsNotNull(select);

            using (var ms = DocStreams.GetOutputStream("HTMLSelectMultiple_EachBound.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(5, select.Choices.Count);
            Assert.AreEqual("UK", select.Choices[0].Value);
            Assert.AreEqual("United Kingdom", select.Choices[0].Label);
            Assert.AreEqual("US", select.Choices[1].Value);
            Assert.AreEqual("FR", select.Choices[2].Value);
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
            Assert.IsFalse((select.Options & FormFieldOptions.Combo) == FormFieldOptions.Combo, "A multiple select is a list box, not a combo/dropdown");
        }

        [TestMethod()]
        [TestCategory("Html-Forms")]
        public void Select_WithoutMultiple_DefaultsToComboBox()
        {
            // Without /Ff Combo, a PDF Choice field defaults to a list box that draws every /Opt
            // entry stacked within its own rect - a plain <select> should render as a dropdown
            // (a combo box) showing only the current value, matching real HTML behaviour.
            var doc = ParseHtml("<select id='s1' name='x'><option>A</option><option>B</option></select>");
            var select = doc.FindAComponentById("s1") as HTMLSelect;

            Assert.IsFalse(select.Multiple);
            Assert.IsTrue((select.Options & FormFieldOptions.Combo) == FormFieldOptions.Combo);
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
        

        [TestMethod()]
        public void Button_WithHoverActiveCss_DistinctAP()
        {
            var src = "<html><head>" +
                      "<style>" +
                      "button { background-color: #aaf; border-radius:4pt;}" +
                      "button:hover{background-color:#ff0000;} " +
                      "button:active{background-color:#0000ff; color:white;}" +
                      "</style>" +
                      "</head>" +
                      "<body>" +
                      "Line before<br/>" +
                      "Before Input<input type='checkbox' />Before button" +
                      "<button id='b1' name='go' type='submit'>Save</button>" +
                      "After Button<br/>" +
                      "Under Button" +
                      "</body></html>";
            var doc = Document.ParseHtmlDocument(new StringReader(src));

            using var stream = DocStreams.GetOutputStream("ButtonStateCheck.pdf");
            doc.RenderOptions.Compression = OutputCompressionType.None;
            doc.SaveAsPDF(stream);
            stream.Flush();
        }
        
        [TestMethod()]
        public void Button_WithHoverActiveCssInForm_DistinctAP()
        {
            var src = "<html><head>" +
                      "<style>" +
                      "button { background-color: #aaf; border-radius:0pt; margin: 0pt 5pt;} " +
                      "button:active {background-color:#aa00aa; color: white;} " +
                      "button:hover {background-color:#0000ff; color: white;} " +
                      "</style>" +
                      "</head>" +
                      "<body style='margin:10pt;'>" +
                      "<div style='border: solid 1pt lime; padding: 10pt;'>Above the form</div>" +
                      "<form action=\"http://localhost:5188/Home/Submit\" method=\"post\" style=''>Above<br/>Above 2<br/>" +
                      "Before button<button id='b1' name='go' type='submit'>Save</button><span>After Button</span>" +
                      "</form>" +
                      "</body></html>";
            var doc = Document.ParseHtmlDocument(new StringReader(src));

            using var stream = DocStreams.GetOutputStream("ButtonStateCheck_submit.pdf");
            doc.RenderOptions.Compression = OutputCompressionType.None;
            doc.AppendTraceLog = true;
            doc.SaveAsPDF(stream);
            stream.Flush();
        }
        
        [TestMethod()]
        public void TextBox_CssInForm_DistinctAP()
        {
            var src = "<html><head>" +
                      "<style>" +
                      "input {margin: 2pt 0pt; padding: 4pt; border-radius:4pt; display: inline-block; background-color: #eee;}" +
                      "input.required {border-color: #f00;}" +
                      "button { background-color: #aaf; border-radius:4pt; margin: 5pt}" +
                      "button:hover, button:active {background-color:#0000ff; margin: 7pt 3pt 3pt 7pt; } " +
                      "</style>" +
                      "</head>" +
                      "<body style='padding: 0pt; margin:10pt;'>" +
                      "<form action=\"https://localhost:7094/Home/Submit\" method=\"post\">Above<br/>Before " +
                      "<input type='text' name='name' value='Hello' /> After<br/>" +
                      "<input type='text' class='required' name='second' value='Second' /> <span>After Button</span>"  +
                      "</form>" +
                      "</body></html>";
            var doc = Document.ParseHtmlDocument(new StringReader(src));

            using var stream = DocStreams.GetOutputStream("TextBox_CssInForm_DistinctAP.pdf");
            doc.RenderOptions.Compression = OutputCompressionType.None;
            doc.SaveAsPDF(stream);
            stream.Flush();
        }
        
        [TestMethod()]
        public void Select_CssInForm_DistinctAP()
        {
            var src = "<html><head>" +
                      "<style>" +
                      "select {margin: 2pt 0pt; padding: 4pt; border-radius:4pt; display: inline-block; background-color: #eee;}" +
                      "select.required {border-color: #f00;}" +
                      "button { background-color: #aaf; border-radius:4pt; margin: 5pt}" +
                      "button:hover, button:active {background-color:#0000ff; margin: 7pt 3pt 3pt 7pt; } " +
                      "</style>" +
                      "</head>" +
                      "<body style='padding: 0pt; margin:10pt;'>" +
                      "<form action=\"https://localhost:7094/Home/Submit\" method=\"post\">Above<br/>Before " +
                      "<select name='name' >" +
                        "<option selected='selected' value='Hello'>Hello</option>" +
                        "<option value='Second'>Second</option>" +
                        "<option value='Third'>Third</option>" +
                      "</select> After<br/>" +
                      "</form>" +
                      "</body></html>";
            var doc = Document.ParseHtmlDocument(new StringReader(src));

            using var stream = DocStreams.GetOutputStream("Select_CssInForm_DistinctAP.pdf");
            doc.RenderOptions.Compression = OutputCompressionType.None;
            doc.SaveAsPDF(stream);
            stream.Flush();
        }
        
        [TestMethod()]
        public void TextArea_CssInForm_DistinctAP()
        {
            var src = "<html><head>" +
                      "<style>" +
                      "textarea {margin: 2pt 0pt; padding: 4pt; border-radius:4pt; display: inline-block; background-color: #eee; font-size:12pt;}" +
                      "textarea.required {border-color: #f00;}" +
                      "button { background-color: #aaf; border-radius:4pt; margin: 5pt}" +
                      "button:hover, button:active {background-color:#0000ff; margin: 7pt 3pt 3pt 7pt; } " +
                      "</style>" +
                      "</head>" +
                      "<body style='padding: 0pt; margin:10pt;'>" +
                      "<form action=\"https://localhost:7094/Home/Submit\" method=\"post\">Above<br/>Before " +
                      "<textarea rows='3' cols='20' >This is some long\r\n" +
                      "text in the text area</textarea>" +
                      " After<br/>" +
                      "</form>" +
                      "<span>After Form</span>" +
                      "</body></html>";
            var doc = Document.ParseHtmlDocument(new StringReader(src));

            using var stream = DocStreams.GetOutputStream("TextArea_CssInForm_DistinctAP.pdf");
            doc.RenderOptions.Compression = OutputCompressionType.None;
            doc.SaveAsPDF(stream);
            stream.Flush();
        }
        
        [TestMethod()]
        public void CheckBox_CssInForm_DistinctAP()
        {
            var src = "<html><head>" +
                      "<style>" +
                      // "input { background-color: #f99; " +
                      //  "margin-left:5pt; margin-right: 5pt; }" +
                      //  "input:active {background-color:#00F; color: white; } " +
                      "</style>" +
                      "</head>" +
                      "<body style='padding: 0pt; margin:10pt;'>" +
                      "<div style='border: solid 1pt lime; padding: 10pt;'>Above the form</div>" +
                      "<form style=''>Above<br/>Above 2<br/>" + 
                      //"Before <input type='text' value='Input' size='10' /> Not Selected " +
                      "Before<input type='checkbox' name='group' value='test' checked='checked' />Selected" +
                      //"<button id='b1' name='go' type='submit'>Save</button>After" +
                       "<input type='radio' name='other' value='not_test' checked='checked' />Not Test" +
                       "<input type='radio' name='other' value='test' />Test<br/>" +
                      "After line" +
                      "</form>" +
                      "<span>After Form</span>" +
                      "</body></html>";
            var doc = Document.ParseHtmlDocument(new StringReader(src));

            using var stream = DocStreams.GetOutputStream("Checkbox_CssInForm_DistinctAP.pdf");
            //doc.RenderOptions.Compression = OutputCompressionType.None;
            doc.SaveAsPDF(stream);
            stream.Flush();
        }
        
        [TestMethod()]
        public void CheckBox_CssInFormInline_DistinctAP()
        {
            var src = "<html><head>" +
                      "<style>" +
                       "input { background-color: #f99;}" +
                      //  "margin-left:5pt; margin-right: 5pt; }" +
                      //  "input:active {background-color:#00F; color: white; } " +
                      "</style>" +
                      "</head>" +
                      "<body style='padding: 0pt; margin:10pt;'>" +
                      "<div style='border: solid 1pt lime; padding: 10pt;'>Above the form</div>" +
                      "<form style=''>Above<br/>Above 2<br/>" + 
                      //"Before <input type='text' value='Input' size='10' /> Not Selected " +
                      "Before<input style='display:inline; margin: 10pt;' type='checkbox' name='group' value='test' checked='checked' />Selected" +
                      //"<button id='b1' name='go' type='submit'>Save</button>After" +
                      "<input type='radio' name='other' value='not_test' checked='checked' />Not Test" +
                      "<input type='radio' name='other' value='test' />Test<br/>" +
                      "After line" +
                      "</form>" +
                      "<span>After Form</span>" +
                      "</body></html>";
            var doc = Document.ParseHtmlDocument(new StringReader(src));

            using var stream = DocStreams.GetOutputStream("Checkbox_CssInFormInline_DistinctAP.pdf");
            //doc.RenderOptions.Compression = OutputCompressionType.None;
            doc.SaveAsPDF(stream);
            stream.Flush();
        }
        
        [TestMethod()]
        public void CheckBox_CssInFormBlock_DistinctAP()
        {
            var src = "<html><head>" +
                      "<style>" +
                      "input { background-color: #f99;}" +
                      //  "margin-left:5pt; margin-right: 5pt; }" +
                      //  "input:active {background-color:#00F; color: white; } " +
                      "</style>" +
                      "</head>" +
                      "<body style='padding: 0pt; margin:10pt;'>" +
                      "<div style='border: solid 1pt lime; padding: 10pt;'>Above the form</div>" +
                      "<form style=''>Above<br/>Above 2<br/>" + 
                      //"Before <input type='text' value='Input' size='10' /> Not Selected " +
                      "Before<input style='display:block; margin: 10pt;' type='checkbox' name='group' value='test' checked='checked' />Selected" +
                      //"<button id='b1' name='go' type='submit'>Save</button>After" +
                      "<input type='radio' name='other' value='not_test' checked='checked' />Not Test" +
                      "<input type='radio' name='other' value='test' />Test<br/>" +
                      "After line" +
                      "</form>" +
                      "<span>After Form</span>" +
                      "</body></html>";
            var doc = Document.ParseHtmlDocument(new StringReader(src));

            using var stream = DocStreams.GetOutputStream("Checkbox_CssInFormBlock_DistinctAP.pdf");
            //doc.RenderOptions.Compression = OutputCompressionType.None;
            doc.SaveAsPDF(stream);
            stream.Flush();
        }
        
        [TestMethod()]
        public void CheckBox_CssInFormRelative_DistinctAP()
        {
            var src = "<html><head>" +
                      "<style>" +
                      "input { background-color: #f99;" +
                        "margin-left:5pt; margin-right: 5pt; }" +
                        "input:active {background-color:#00F; color: white; } " +
                      "</style>" +
                      "</head>" +
                      "<body style='padding: 0pt; margin:10pt;'>" +
                      "<div style='border: solid 1pt lime; padding: 10pt;'>Above the form</div>" +
                      "<form style=''>Above<br/>Above 2<br/>" + 
                      //"Before <input type='text' value='Input' size='10' /> Not Selected " +
                      "Before<input style='margin: 0pt;' type='checkbox' name='group' value='test' checked='checked' />Selected" +
                      //"<button id='b1' name='go' type='submit'>Save</button>After" +
                      "<input type='radio' name='other' value='not_test' checked='checked' />Not Test" +
                      "<input type='radio' name='other' value='test' />Test<br/>" +
                      "After line" +
                      "</form>" +
                      "<span>After Form</span>" +
                      "</body></html>";
            var doc = Document.ParseHtmlDocument(new StringReader(src));

            using var stream = DocStreams.GetOutputStream("Checkbox_CssInFormRelative_DistinctAP.pdf");
            //doc.RenderOptions.Compression = OutputCompressionType.None;
            doc.SaveAsPDF(stream);
            stream.Flush();
        }
        
        [TestMethod()]
        public void CheckBox_CssInFormAbsolute_DistinctAP()
        {
            var src = "<html><head>" +
                      "<style>" +
                      "input { background-color: #f99;}" +
                      //  "margin-left:5pt; margin-right: 5pt; }" +
                      "input:active {background-color:#00F; color: white; } " +
                      "</style>" +
                      "</head>" +
                      "<body style='padding: 0pt; margin:10pt;'>" +
                      "<div style='border: solid 1pt lime; padding: 10pt;'>Above the form</div>" +
                      "<form style=''>Above<br/>Above 2<br/>" + 
                      //"Before <input type='text' value='Input' size='10' /> Not Selected " +
                      "Before<input style='position:absolute; top: 20pt; left: 30pt; margin: 10pt;' type='checkbox' name='group' value='test' checked='checked' />Selected" +
                      //"<button id='b1' name='go' type='submit'>Save</button>After" +
                      //"<input type='radio' name='other' value='not_test' checked='checked' />Not Test" +
                      //"<input type='radio' name='other' value='test' />Test<br/>" +
                      "After line" +
                      "</form>" +
                      "<span>After Form</span>" +
                      "</body></html>";
            var doc = Document.ParseHtmlDocument(new StringReader(src));

            using var stream = DocStreams.GetOutputStream("Checkbox_CssInFormAbsolute_DistinctAP.pdf");
            //doc.RenderOptions.Compression = OutputCompressionType.None;
            doc.SaveAsPDF(stream);
            stream.Flush();
        }
        
        [TestMethod()]
        public void CheckBox_CssInFormFixed_DistinctAP()
        {
            var src = "<html><head>" +
                      "<style>" +
                      "input { background-color: #f99;}" +
                      //  "margin-left:5pt; margin-right: 5pt; }" +
                      "input:active {background-color:#00F; color: white; } " +
                      "</style>" +
                      "</head>" +
                      "<body style='padding: 0pt; margin:10pt;'>" +
                      "<div style='border: solid 1pt lime; padding: 10pt;'>Above the form</div>" +
                      "<form style=''>Above<br/>Above 2<br/>" + 
                      //"Before <input type='text' value='Input' size='10' /> Not Selected " +
                      "Before<input style='position:fixed; top: 0pt; left: 0pt; margin: 0pt;' type='checkbox' name='group' value='test' checked='checked' />Selected" +
                      //"<button id='b1' name='go' type='submit'>Save</button>After" +
                      //"<input type='radio' name='other' value='not_test' checked='checked' />Not Test" +
                      //"<input type='radio' name='other' value='test' />Test<br/>" +
                      "<span style='position:fixed; right: 0pt; top: 10pt;'>After line</span>" +
                      "</form>" +
                      "<span>After Form</span>" +
                      "</body></html>";
            var doc = Document.ParseHtmlDocument(new StringReader(src));

            using var stream = DocStreams.GetOutputStream("Checkbox_CssInFormFixed_DistinctAP.pdf");
            //doc.RenderOptions.Compression = OutputCompressionType.None;
            doc.SaveAsPDF(stream);
            stream.Flush();
        }
    }
}
