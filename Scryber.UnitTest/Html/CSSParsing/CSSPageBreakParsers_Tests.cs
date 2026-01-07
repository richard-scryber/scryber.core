using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Styles;
using Scryber.Styles.Parsing;
using Scryber.Styles.Parsing.Typed;
using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Core.UnitTests.Html.CSSParsers
{
    /// <summary>
    /// Tests for CSS page and break parsers: page-break-before, page-break-after,
    /// page-break-inside, break-before-after, break-inside, page-size, page-name
    /// Covers 7 page/break-related parsers
    /// </summary>
    [TestClass()]
    public class CSSPageBreakParsers_Tests
    {
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        #region Helper Methods

        private Style CreateStyle()
        {
            return new Style();
        }

        private CSSStyleItemReader CreateReader(string cssValue)
        {
            var reader = new CSSStyleItemReader(cssValue);
            return reader;
        }

        private bool ParseValue(CSSStyleValueParser parser, Style style, string cssValue)
        {
            var reader = CreateReader(cssValue);
            return parser.SetStyleValue(style, reader, null);
        }

        #endregion

        #region Page Break Before Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PageBreakBefore_Always_SetsBreak()
        {
            var parser = new CSSPageBreakBeforeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "always");

            Assert.IsTrue(result);
            Assert.IsTrue(style.PageStyle.BreakBefore);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PageBreakBefore_Avoid_SetsNoBreak()
        {
            var parser = new CSSPageBreakBeforeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "avoid");

            Assert.IsTrue(result);
            Assert.IsFalse(style.PageStyle.BreakBefore);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PageBreakBefore_Left_SetsBreak()
        {
            var parser = new CSSPageBreakBeforeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "left");

            Assert.IsTrue(result);
            Assert.IsTrue(style.PageStyle.BreakBefore);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PageBreakBefore_Right_SetsBreak()
        {
            var parser = new CSSPageBreakBeforeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "right");

            Assert.IsTrue(result);
            Assert.IsTrue(style.PageStyle.BreakBefore);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PageBreakBefore_Column_SetsBreak()
        {
            var parser = new CSSPageBreakBeforeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "column");

            Assert.IsTrue(result);
            Assert.IsTrue(style.PageStyle.BreakBefore);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PageBreakBefore_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSPageBreakBeforeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid-break");

            Assert.IsFalse(result);
        }

        #endregion

        #region Page Break After Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PageBreakAfter_Always_SetsBreak()
        {
            var parser = new CSSPageBreakAfterParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "always");

            Assert.IsTrue(result);
            Assert.IsTrue(style.PageStyle.BreakAfter);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PageBreakAfter_Avoid_SetsNoBreak()
        {
            var parser = new CSSPageBreakAfterParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "avoid");

            Assert.IsTrue(result);
            Assert.IsFalse(style.PageStyle.BreakAfter);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PageBreakAfter_Left_SetsBreak()
        {
            var parser = new CSSPageBreakAfterParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "left");

            Assert.IsTrue(result);
            Assert.IsTrue(style.PageStyle.BreakAfter);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PageBreakAfter_Right_SetsBreak()
        {
            var parser = new CSSPageBreakAfterParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "right");

            Assert.IsTrue(result);
            Assert.IsTrue(style.PageStyle.BreakAfter);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PageBreakAfter_Column_SetsBreak()
        {
            var parser = new CSSPageBreakAfterParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "column");

            Assert.IsTrue(result);
            Assert.IsTrue(style.PageStyle.BreakAfter);
        }

        #endregion

        #region Page Break Inside Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PageBreakInside_Auto_SetsAuto()
        {
            var parser = new CSSPageBreakInsideParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "auto");

            Assert.IsTrue(result);
            Assert.AreEqual(OverflowSplit.Any, style.Overflow.Split);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PageBreakInside_Avoid_SetsAvoid()
        {
            var parser = new CSSPageBreakInsideParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "avoid");

            Assert.IsTrue(result);
            Assert.AreEqual(OverflowSplit.Never, style.Overflow.Split);
            Assert.AreEqual(OverflowAction.NewPage, style.Overflow.Action);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PageBreakInside_Initial_SetsInitial()
        {
            var parser = new CSSPageBreakInsideParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "initial");

            Assert.IsTrue(result);
            Assert.AreEqual(OverflowSplit.Never, style.Overflow.Split);
            Assert.AreEqual(OverflowAction.Truncate, style.Overflow.Action);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PageBreakInside_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSPageBreakInsideParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid-break");

            Assert.IsFalse(result);
        }

        #endregion

        #region Page Size Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PageSize_A4_SetsA4()
        {
            var parser = new CSSPageSizeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "A4");

            Assert.IsTrue(result);
            Assert.AreEqual(PaperSize.A4, style.PageStyle.PaperSize);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PageSize_Letter_SetsLetter()
        {
            var parser = new CSSPageSizeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "Letter");

            Assert.IsTrue(result);
            Assert.AreEqual(PaperSize.Letter, style.PageStyle.PaperSize);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PageSize_A4Portrait_SetsA4AndPortrait()
        {
            var parser = new CSSPageSizeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "A4 Portrait");

            Assert.IsTrue(result);
            Assert.AreEqual(PaperSize.A4, style.PageStyle.PaperSize);
            Assert.AreEqual(PaperOrientation.Portrait, style.PageStyle.PaperOrientation);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PageSize_LetterLandscape_SetsLetterAndLandscape()
        {
            var parser = new CSSPageSizeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "Letter Landscape");

            Assert.IsTrue(result);
            Assert.AreEqual(PaperSize.Letter, style.PageStyle.PaperSize);
            Assert.AreEqual(PaperOrientation.Landscape, style.PageStyle.PaperOrientation);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PageSize_OrientationOnly_SetsOrientation()
        {
            var parser = new CSSPageSizeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "Landscape");

            Assert.IsTrue(result);
            Assert.AreEqual(PaperOrientation.Landscape, style.PageStyle.PaperOrientation);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PageSize_CustomWidthHeight_SetsDimensions()
        {
            var parser = new CSSPageSizeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "300pt 400pt");

            Assert.IsTrue(result);
            Assert.AreEqual(300.0, style.PageStyle.Width.PointsValue, 0.001);
            Assert.AreEqual(400.0, style.PageStyle.Height.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PageSize_WidthOnly_SetsWidth()
        {
            var parser = new CSSPageSizeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "300pt");

            Assert.IsTrue(result);
            Assert.AreEqual(300.0, style.PageStyle.Width.PointsValue, 0.001);
        }

        #endregion

        #region Page Name Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PageName_ValidName_SetsName()
        {
            var parser = new CSSPageNameParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "cover");

            Assert.IsTrue(result);
            Assert.AreEqual("cover", style.PageStyle.PageNameGroup);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PageName_AnotherName_SetsName()
        {
            var parser = new CSSPageNameParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "chapter-1");

            Assert.IsTrue(result);
            Assert.AreEqual("chapter-1", style.PageStyle.PageNameGroup);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PageName_EmptyString_ReturnsFalse()
        {
            var parser = new CSSPageNameParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "");

            Assert.IsFalse(result);
        }

        #endregion
    }
}
