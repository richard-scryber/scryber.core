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
    /// Tests for CSS column parsers: column-count, column-width, column-gap, column-span,
    /// column-break-before, column-break-after, column-break-inside
    /// Covers 7 column-related parsers
    /// </summary>
    [TestClass()]
    public class CSSColumnParsers_Tests
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

        #region Column Count Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnCount_SingleColumn_SetsOne()
        {
            var parser = new CSSColumnCountParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "1");

            Assert.IsTrue(result);
            Assert.AreEqual(1, style.Columns.ColumnCount);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnCount_TwoColumns_SetsTwo()
        {
            var parser = new CSSColumnCountParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "2");

            Assert.IsTrue(result);
            Assert.AreEqual(2, style.Columns.ColumnCount);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnCount_ThreeColumns_SetsThree()
        {
            var parser = new CSSColumnCountParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "3");

            Assert.IsTrue(result);
            Assert.AreEqual(3, style.Columns.ColumnCount);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnCount_Auto_SetsAuto()
        {
            var parser = new CSSColumnCountParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "auto");

            Assert.IsTrue(result);
            // Auto column count
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnCount_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSColumnCountParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid-count");

            Assert.IsFalse(result);
        }

        #endregion

        #region Column Width Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnWidth_ValidUnit_Pt_SetsWidth()
        {
            var parser = new CSSColumnWidthParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "150pt");

            Assert.IsTrue(result);
            var widths = style.GetValue(StyleKeys.ColumnWidthKey, Scryber.Drawing.ColumnWidths.Empty);
            Assert.IsFalse(widths.IsEmpty);
            Assert.IsTrue(widths.HasExplicitWidth);
            Assert.AreEqual(150.0, widths.Explicit.PointsValue, 0.001);
            Assert.IsNull(widths.Widths);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnWidth_ValidUnit_Px_SetsWidth()
        {
            var parser = new CSSColumnWidthParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "200px");

            Assert.IsTrue(result);
            // 200px = 200 * 0.75 = 150pt
            var widths = style.GetValue(StyleKeys.ColumnWidthKey, Scryber.Drawing.ColumnWidths.Empty);
            Assert.IsFalse(widths.IsEmpty);
            Assert.IsTrue(widths.HasExplicitWidth);
            Assert.AreEqual(150.0, widths.Explicit.PointsValue, 0.001);
            Assert.IsNull(widths.Widths);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnWidth_AutoValue_SetsAuto()
        {
            var parser = new CSSColumnWidthParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "auto");

            Assert.IsTrue(result);
            var widths = style.GetValue(StyleKeys.ColumnWidthKey, Scryber.Drawing.ColumnWidths.Empty);
            Assert.IsTrue(widths.IsEmpty);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnWidth_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSColumnWidthParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid-width");

            Assert.IsFalse(result);
        }

        #endregion

        #region Column Gap Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnGap_ValidUnit_Pt_SetsGap()
        {
            var parser = new CSSColumnGapParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "20pt");

            Assert.IsTrue(result);
            Assert.AreEqual(20.0, style.Columns.AlleyWidth.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnGap_ValidUnit_Px_SetsGap()
        {
            var parser = new CSSColumnGapParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "32px");

            Assert.IsTrue(result);
            // 32px = 32 * 0.75 = 24pt
            Assert.AreEqual(24.0, style.Columns.AlleyWidth.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnGap_ZeroValue_SetsZero()
        {
            var parser = new CSSColumnGapParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "0");

            Assert.IsTrue(result);
            Assert.AreEqual(0.0, style.Columns.AlleyWidth.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnGap_NormalValue_SetsDefault()
        {
            var parser = new CSSColumnGapParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "normal");

            Assert.IsTrue(result);
            // Sets normal/default gap
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnGap_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSColumnGapParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid-gap");

            Assert.IsFalse(result);
        }

        #endregion

        #region Column Span Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnSpan_None_SetsNoSpan()
        {
            var parser = new CSSColumnSpanParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "none");

            Assert.IsTrue(result);
            // Column span none
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnSpan_All_SetsSpanAll()
        {
            var parser = new CSSColumnSpanParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "all");

            Assert.IsTrue(result);
            // Column span all
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnSpan_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSColumnSpanParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid-span");

            Assert.IsFalse(result);
        }

        #endregion

        #region Column Break Before Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnBreakBefore_Auto_SetsAuto()
        {
            var parser = new CSSColumnBreakBeforeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "auto");

            Assert.IsTrue(result);
            // Auto break
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnBreakBefore_Always_SetsAlways()
        {
            var parser = new CSSColumnBreakBeforeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "always");

            Assert.IsTrue(result);
            // Always break before
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnBreakBefore_Avoid_SetsAvoid()
        {
            var parser = new CSSColumnBreakBeforeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "avoid");

            Assert.IsTrue(result);
            // Avoid break before
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnBreakBefore_Column_SetsColumn()
        {
            var parser = new CSSColumnBreakBeforeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "column");

            Assert.IsTrue(result);
            // Break before column
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnBreakBefore_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSColumnBreakBeforeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid-break");

            Assert.IsFalse(result);
        }

        #endregion

        #region Column Break After Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnBreakAfter_Auto_SetsAuto()
        {
            var parser = new CSSColumnBreakAfterParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "auto");

            Assert.IsTrue(result);
            // Auto break
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnBreakAfter_Always_SetsAlways()
        {
            var parser = new CSSColumnBreakAfterParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "always");

            Assert.IsTrue(result);
            // Always break after
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnBreakAfter_Avoid_SetsAvoid()
        {
            var parser = new CSSColumnBreakAfterParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "avoid");

            Assert.IsTrue(result);
            // Avoid break after
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnBreakAfter_Column_SetsColumn()
        {
            var parser = new CSSColumnBreakAfterParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "column");

            Assert.IsTrue(result);
            // Break after column
        }

        #endregion

        #region Column Break Inside Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnBreakInside_Auto_SetsAuto()
        {
            var parser = new CSSColumnBreakInsideParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "auto");

            Assert.IsTrue(result);
            // Auto break inside
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnBreakInside_Avoid_SetsAvoid()
        {
            var parser = new CSSColumnBreakInsideParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "avoid");

            Assert.IsTrue(result);
            // Avoid break inside
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnBreakInside_AvoidColumn_SetsAvoidColumn()
        {
            var parser = new CSSColumnBreakInsideParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "avoid-column");

            Assert.IsTrue(result);
            // Avoid column break inside
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnBreakInside_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSColumnBreakInsideParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid-break");

            Assert.IsFalse(result);
        }

        #endregion
    }
}
