using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Drawing;
using Scryber.Styles;
using Scryber.Styles.Parsing;
using Scryber.Components;

namespace Scryber.Core.UnitTests.Html.CSSParsers
{
    /// <summary>
    /// Tests for the CSS !important flag: detection/stripping at the reader level,
    /// the MakeImportant/IsImportant programmatic API, and cascade resolution.
    /// </summary>
    [TestClass()]
    public class CSSImportant_Tests
    {
        // -----------------------------------------------------------------------
        // Section 1: CSSStyleItemReader detection/stripping
        // -----------------------------------------------------------------------

        #region Reader-level detection

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Important")]
        public void Reader_SingleValue_Important_IsDetectedAndStripped()
        {
            var reader = new CSSStyleItemReader("color: red !important;");
            Assert.IsTrue(reader.ReadNextAttributeName());
            Assert.AreEqual("color", reader.CurrentAttribute);

            Assert.IsTrue(reader.ReadNextValue(';', ignoreWhiteSpace: true));
            Assert.AreEqual("red", reader.CurrentTextValue.TrimEnd());
            Assert.IsTrue(reader.IsImportantAttribute);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Important")]
        public void Reader_SingleValue_NoImportant_IsUnaffected()
        {
            var reader = new CSSStyleItemReader("color: red;");
            Assert.IsTrue(reader.ReadNextAttributeName());
            Assert.IsTrue(reader.ReadNextValue(';', ignoreWhiteSpace: true));

            Assert.AreEqual("red", reader.CurrentTextValue.TrimEnd());
            Assert.IsFalse(reader.IsImportantAttribute);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Important")]
        public void Reader_Important_CaseInsensitiveAndWhitespaceTolerant()
        {
            var reader = new CSSStyleItemReader("color: red   !   IMPORTANT ;");
            Assert.IsTrue(reader.ReadNextAttributeName());
            Assert.IsTrue(reader.ReadNextValue(';', ignoreWhiteSpace: true));

            Assert.AreEqual("red", reader.CurrentTextValue.TrimEnd());
            Assert.IsTrue(reader.IsImportantAttribute);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Important")]
        public void Reader_ShorthandTokens_LastTokenIsImportant_NotIncludedInValue()
        {
            //Simulates a shorthand parser reading space-separated tokens one at a time.
            var reader = new CSSStyleItemReader("margin: 1pt 2pt 3pt 4pt !important;");
            Assert.IsTrue(reader.ReadNextAttributeName());
            Assert.AreEqual("margin", reader.CurrentAttribute);

            Assert.IsTrue(reader.ReadNextValue(' ', ';'));
            Assert.AreEqual("1pt", reader.CurrentTextValue);

            Assert.IsTrue(reader.ReadNextValue(' ', ';'));
            Assert.AreEqual("2pt", reader.CurrentTextValue);

            Assert.IsTrue(reader.ReadNextValue(' ', ';'));
            Assert.AreEqual("3pt", reader.CurrentTextValue);

            Assert.IsTrue(reader.ReadNextValue(' ', ';'));
            Assert.AreEqual("4pt", reader.CurrentTextValue);

            //No further tokens should be readable - !important must not appear as a value token.
            Assert.IsFalse(reader.ReadNextValue(' ', ';'));
            Assert.IsTrue(reader.IsImportantAttribute);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Important")]
        public void Reader_MultipleDeclarations_OnlyOneImportant_AdvancesCorrectly()
        {
            var reader = new CSSStyleItemReader("color: red !important; background-color: blue; font-weight: bold !important;");

            Assert.IsTrue(reader.ReadNextAttributeName());
            Assert.AreEqual("color", reader.CurrentAttribute);
            Assert.IsTrue(reader.ReadNextValue(';', ignoreWhiteSpace: true));
            Assert.AreEqual("red", reader.CurrentTextValue.TrimEnd());
            Assert.IsTrue(reader.IsImportantAttribute);

            Assert.IsTrue(reader.MoveToNextAttribute());
            Assert.IsTrue(reader.ReadNextAttributeName());
            Assert.AreEqual("background-color", reader.CurrentAttribute);
            Assert.IsTrue(reader.ReadNextValue(';', ignoreWhiteSpace: true));
            Assert.AreEqual("blue", reader.CurrentTextValue.TrimEnd());
            Assert.IsFalse(reader.IsImportantAttribute);

            Assert.IsTrue(reader.MoveToNextAttribute());
            Assert.IsTrue(reader.ReadNextAttributeName());
            Assert.AreEqual("font-weight", reader.CurrentAttribute);
            Assert.IsTrue(reader.ReadNextValue(';', ignoreWhiteSpace: true));
            Assert.AreEqual("bold", reader.CurrentTextValue.TrimEnd());
            Assert.IsTrue(reader.IsImportantAttribute);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Important")]
        public void Reader_ValueWithParenthesesAndImportant_IsHandledCorrectly()
        {
            var reader = new CSSStyleItemReader("background-image: url('image.png') !important;");
            Assert.IsTrue(reader.ReadNextAttributeName());
            Assert.IsTrue(reader.ReadNextValue(';', ignoreWhiteSpace: true));

            Assert.AreEqual("url('image.png')", reader.CurrentTextValue.TrimEnd());
            Assert.IsTrue(reader.IsImportantAttribute);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Important")]
        public void Reader_BangWithoutImportantWord_IsNotTreatedAsImportant()
        {
            //Not real CSS, but should degrade safely rather than mis-detecting.
            var reader = new CSSStyleItemReader("content: 'hello!';");
            Assert.IsTrue(reader.ReadNextAttributeName());
            Assert.IsTrue(reader.ReadNextValue(';', ignoreWhiteSpace: true));

            Assert.AreEqual("'hello!'", reader.CurrentTextValue.TrimEnd());
            Assert.IsFalse(reader.IsImportantAttribute);
        }

        #endregion

        // -----------------------------------------------------------------------
        // Section 2: MakeImportant / IsImportant programmatic API
        // -----------------------------------------------------------------------

        #region Programmatic API

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Important")]
        public void MakeImportant_SetsFlagOnExistingValue()
        {
            var style = new Style();
            style.SetValue(StyleKeys.FillColorKey, (Color)"#FF0000");

            Assert.IsFalse(style.IsImportant(StyleKeys.FillColorKey));

            style.MakeImportant(StyleKeys.FillColorKey);

            Assert.IsTrue(style.IsImportant(StyleKeys.FillColorKey));
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Important")]
        public void MakeImportant_CanClearFlag()
        {
            var style = new Style();
            style.SetValue(StyleKeys.FillColorKey, (Color)"#FF0000");
            style.MakeImportant(StyleKeys.FillColorKey);
            Assert.IsTrue(style.IsImportant(StyleKeys.FillColorKey));

            style.MakeImportant(StyleKeys.FillColorKey, false);

            Assert.IsFalse(style.IsImportant(StyleKeys.FillColorKey));
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Important")]
        public void MakeImportant_NoOpOnUnsetKey()
        {
            var style = new Style();

            //Should not throw, and should not create a value for the key.
            style.MakeImportant(StyleKeys.FillColorKey);

            Assert.IsFalse(style.IsImportant(StyleKeys.FillColorKey));
            Assert.IsFalse(style.IsValueDefined(StyleKeys.FillColorKey));
        }

        #endregion

        // -----------------------------------------------------------------------
        // Section 3: Cascade resolution (end to end)
        // -----------------------------------------------------------------------

        #region Cascade

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Important")]
        public void Cascade_ImportantLowSpecificity_BeatsNonImportantHighSpecificity()
        {
            var src = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<head>
    <style>
        .red { color: #FF0000 !important; }
        body#pg.red { color: #00FF00; }
    </style>
</head>
<body id='pg' class='red'>
    Content
</body>
</html>";

            using (var ms = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(ms, ParseSourceType.DynamicContent);
                using (var stream = DocStreams.GetOutputStream("CSSImportant_LowSpecificityWins.pdf"))
                    doc.SaveAsPDF(stream);

                var pg = doc.FindAComponentById("pg") as Page;
                var arrange = pg.GetFirstArrangement() as ComponentMultiArrangement;
                arrange = arrange.NextArrangement;
                var style = arrange.FullStyle;

                Assert.AreEqual((Color)"#FF0000", style.Fill.Color, "The !important, lower specificity rule should win");
            }
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Important")]
        public void Cascade_Important_BeatsNonImportantInlineStyle()
        {
            var src = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<head>
    <style>
        .red { color: #FF0000 !important; }
    </style>
</head>
<body id='pg' class='red' style='color: #00FF00;'>
    Content
</body>
</html>";

            using (var ms = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(ms, ParseSourceType.DynamicContent);
                using (var stream = DocStreams.GetOutputStream("CSSImportant_BeatsInline.pdf"))
                    doc.SaveAsPDF(stream);

                var pg = doc.FindAComponentById("pg") as Page;
                var arrange = pg.GetFirstArrangement() as ComponentMultiArrangement;
                arrange = arrange.NextArrangement;
                var style = arrange.FullStyle;

                Assert.AreEqual((Color)"#FF0000", style.Fill.Color, "The !important stylesheet rule should beat the non-important inline style");
            }
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Important")]
        public void Cascade_TwoImportantRules_HigherSpecificityWins()
        {
            var src = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<head>
    <style>
        .red { color: #FF0000 !important; }
        body#pg.red { color: #00FF00 !important; }
    </style>
</head>
<body id='pg' class='red'>
    Content
</body>
</html>";

            using (var ms = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(ms, ParseSourceType.DynamicContent);
                using (var stream = DocStreams.GetOutputStream("CSSImportant_HigherSpecificityWinsAmongImportant.pdf"))
                    doc.SaveAsPDF(stream);

                var pg = doc.FindAComponentById("pg") as Page;
                var arrange = pg.GetFirstArrangement() as ComponentMultiArrangement;
                arrange = arrange.NextArrangement;
                var style = arrange.FullStyle;

                Assert.AreEqual((Color)"#00FF00", style.Fill.Color, "Among two !important rules, the higher specificity one should still win");
            }
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Important")]
        public void Cascade_NonImportant_NormalSpecificityStillApplies()
        {
            //Regression: without !important anywhere, behaviour should be unchanged from before this work.
            var src = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<head>
    <style>
        .red { color: #FF0000; }
        body#pg.red { color: #00FF00; }
    </style>
</head>
<body id='pg' class='red'>
    Content
</body>
</html>";

            using (var ms = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(ms, ParseSourceType.DynamicContent);
                using (var stream = DocStreams.GetOutputStream("CSSImportant_RegressionNoImportant.pdf"))
                    doc.SaveAsPDF(stream);

                var pg = doc.FindAComponentById("pg") as Page;
                var arrange = pg.GetFirstArrangement() as ComponentMultiArrangement;
                arrange = arrange.NextArrangement;
                var style = arrange.FullStyle;

                Assert.AreEqual((Color)"#00FF00", style.Fill.Color, "Without !important, higher specificity should win as before");
            }
        }

        #endregion
    }
}
