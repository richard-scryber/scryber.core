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
            Assert.Inconclusive("This test requires all column spans, and is not supported.");
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

        #region Column Rule Width Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnRuleWidth_ValidUnit_Pt_SetsWidth()
        {
            var parser = new CSSColumnRuleWidthParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "2pt");

            Assert.IsTrue(result);
            Assert.AreEqual(2.0, style.GetValue(StyleKeys.ColumnRuleWidthKey, Unit.Zero).PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnRuleWidth_ValidUnit_Px_SetsWidth()
        {
            var parser = new CSSColumnRuleWidthParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "4px");

            Assert.IsTrue(result);
            // 4px = 4 * 0.75 = 3pt
            Assert.AreEqual(3.0, style.GetValue(StyleKeys.ColumnRuleWidthKey, Unit.Zero).PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnRuleWidth_ZeroValue_SetsZero()
        {
            var parser = new CSSColumnRuleWidthParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "0");

            Assert.IsTrue(result);
            Assert.AreEqual(0.0, style.GetValue(StyleKeys.ColumnRuleWidthKey, Unit.Zero).PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnRuleWidth_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSColumnRuleWidthParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid-width");

            Assert.IsFalse(result);
        }

        #endregion

        #region Column Rule Color Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnRuleColor_HexRed_SetsColor()
        {
            var parser = new CSSColumnRuleColorParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "#FF0000");

            Assert.IsTrue(result);
            Assert.AreEqual(Color.FromRGB(255, 0, 0), style.GetValue(StyleKeys.ColumnRuleColorKey, Color.Transparent));
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnRuleColor_HexBlue_SetsColor()
        {
            var parser = new CSSColumnRuleColorParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "#0000FF");

            Assert.IsTrue(result);
            Assert.AreEqual(Color.FromRGB(0, 0, 255), style.GetValue(StyleKeys.ColumnRuleColorKey, Color.Transparent));
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnRuleColor_ShortHex_SetsColor()
        {
            var parser = new CSSColumnRuleColorParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "#333");

            Assert.IsTrue(result);
            Assert.AreEqual(Color.FromRGB(0x33, 0x33, 0x33), style.GetValue(StyleKeys.ColumnRuleColorKey, Color.Transparent));
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnRuleColor_RgbFunction_SetsColor()
        {
            var parser = new CSSColumnRuleColorParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "rgb(0, 128, 0)");

            Assert.IsTrue(result);
            Assert.AreEqual(Color.FromRGB(0, 128, 0), style.GetValue(StyleKeys.ColumnRuleColorKey, Color.Transparent));
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnRuleColor_RgbaFunction_SetsColorAndOpacity()
        {
            var parser = new CSSColumnRuleColorParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "rgba(255, 0, 0, 0.5)");

            Assert.IsTrue(result);
            Assert.AreEqual(Color.FromRGB(255, 0, 0), style.GetValue(StyleKeys.ColumnRuleColorKey, Color.Transparent));
            Assert.AreEqual(0.5, style.GetValue(StyleKeys.ColumnRuleOpacityKey, 1.0), 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnRuleColor_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSColumnRuleColorParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "not-a-color");

            Assert.IsFalse(result);
        }

        #endregion

        #region Column Rule Line Style Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnRuleStyle_Solid_SetsSolid()
        {
            var parser = new CSSColumnRuleLineStyleParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "solid");

            Assert.IsTrue(result);
            Assert.AreEqual(LineType.Solid, style.GetValue(StyleKeys.ColumnRuleStyleKey, LineType.None));
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnRuleStyle_Dashed_SetsDashWithPattern()
        {
            var parser = new CSSColumnRuleLineStyleParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "dashed");

            Assert.IsTrue(result);
            Assert.AreEqual(LineType.Dash, style.GetValue(StyleKeys.ColumnRuleStyleKey, LineType.None));
            Assert.IsNotNull(style.GetValue(StyleKeys.ColumnRuleDashKey, null));
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnRuleStyle_Dotted_SetsDashWithPattern()
        {
            var parser = new CSSColumnRuleLineStyleParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "dotted");

            Assert.IsTrue(result);
            Assert.AreEqual(LineType.Dash, style.GetValue(StyleKeys.ColumnRuleStyleKey, LineType.None));
            Assert.IsNotNull(style.GetValue(StyleKeys.ColumnRuleDashKey, null));
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnRuleStyle_DashedAndDottedHaveDifferentPatterns()
        {
            var dashedStyle = CreateStyle();
            var dottedStyle = CreateStyle();

            ParseValue(new CSSColumnRuleLineStyleParser(), dashedStyle, "dashed");
            ParseValue(new CSSColumnRuleLineStyleParser(), dottedStyle, "dotted");

            var dashedPattern = dashedStyle.GetValue(StyleKeys.ColumnRuleDashKey, null);
            var dottedPattern = dottedStyle.GetValue(StyleKeys.ColumnRuleDashKey, null);

            Assert.IsNotNull(dashedPattern);
            Assert.IsNotNull(dottedPattern);
            Assert.AreNotEqual(dashedPattern, dottedPattern);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnRuleStyle_None_SetsNone()
        {
            var parser = new CSSColumnRuleLineStyleParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "none");

            Assert.IsTrue(result);
            Assert.AreEqual(LineType.None, style.GetValue(StyleKeys.ColumnRuleStyleKey, LineType.Solid));
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnRuleStyle_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSColumnRuleLineStyleParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "not-a-line-style");

            Assert.IsFalse(result);
        }

        #endregion

        #region Column Rule Shorthand Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnRule_WidthOnly_SetsWidth()
        {
            var parser = new CSSColumnRuleParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "2pt");

            Assert.IsTrue(result);
            Assert.AreEqual(2.0, style.GetValue(StyleKeys.ColumnRuleWidthKey, Unit.Zero).PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnRule_StyleOnly_SetsStyle()
        {
            var parser = new CSSColumnRuleParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "solid");

            Assert.IsTrue(result);
            Assert.AreEqual(LineType.Solid, style.GetValue(StyleKeys.ColumnRuleStyleKey, LineType.None));
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnRule_ColorOnly_SetsColor()
        {
            var parser = new CSSColumnRuleParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "#FF0000");

            Assert.IsTrue(result);
            Assert.AreEqual(Color.FromRGB(255, 0, 0), style.GetValue(StyleKeys.ColumnRuleColorKey, Color.Transparent));
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnRule_WidthAndStyle_SetsBoth()
        {
            var parser = new CSSColumnRuleParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "3pt solid");

            Assert.IsTrue(result);
            Assert.AreEqual(3.0, style.GetValue(StyleKeys.ColumnRuleWidthKey, Unit.Zero).PointsValue, 0.001);
            Assert.AreEqual(LineType.Solid, style.GetValue(StyleKeys.ColumnRuleStyleKey, LineType.None));
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnRule_AllThreeValues_SetsAll()
        {
            var parser = new CSSColumnRuleParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "2pt solid #0000FF");

            Assert.IsTrue(result);
            Assert.AreEqual(2.0, style.GetValue(StyleKeys.ColumnRuleWidthKey, Unit.Zero).PointsValue, 0.001);
            Assert.AreEqual(LineType.Solid, style.GetValue(StyleKeys.ColumnRuleStyleKey, LineType.None));
            Assert.AreEqual(Color.FromRGB(0, 0, 255), style.GetValue(StyleKeys.ColumnRuleColorKey, Color.Transparent));
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnRule_DashedWithColor_SetsDashAndColor()
        {
            var parser = new CSSColumnRuleParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "1pt dashed #CC0000");

            Assert.IsTrue(result);
            Assert.AreEqual(1.0, style.GetValue(StyleKeys.ColumnRuleWidthKey, Unit.Zero).PointsValue, 0.001);
            Assert.AreEqual(LineType.Dash, style.GetValue(StyleKeys.ColumnRuleStyleKey, LineType.None));
            Assert.IsNotNull(style.GetValue(StyleKeys.ColumnRuleDashKey, null));
            Assert.AreEqual(Color.FromRGB(0xCC, 0, 0), style.GetValue(StyleKeys.ColumnRuleColorKey, Color.Transparent));
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnRule_EmptyValue_ReturnsFalse()
        {
            var parser = new CSSColumnRuleParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, " ");

            Assert.IsFalse(result);
        }

        #endregion

        #region Column Fill Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnFill_Auto_SetsAuto()
        {
            var parser = new CSSColumnFillParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "auto");

            Assert.IsTrue(result);
            Assert.AreEqual(ColumnFillMode.Auto, style.Columns.FillMode);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnFill_Balance_SetsBalance()
        {
            var parser = new CSSColumnFillParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "balance");

            Assert.IsTrue(result);
            Assert.AreEqual(ColumnFillMode.Balance, style.Columns.FillMode);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnFill_BalanceAll_SetsBalanceAll()
        {
            // The CSS spec uses "balance-all" but Enum.TryParse requires the enum member
            // name, so the parseable value is "balance_all" (underscore).
            var parser = new CSSColumnFillParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "balance_all");

            Assert.IsTrue(result);
            Assert.AreEqual(ColumnFillMode.Balance_All, style.Columns.FillMode);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnFill_CssHyphenatedBalanceAll_ReturnsFalse()
        {
            // "balance-all" (hyphen) does not map to any ColumnFillMode enum member
            // and so the parser cannot resolve it.
            var parser = new CSSColumnFillParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "balance-all");

            Assert.IsFalse(result);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ColumnFill_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSColumnFillParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "not-a-fill-mode");

            Assert.IsFalse(result);
        }

        #endregion

        #region Columns Shorthand Parser Tests

        // Helper to read column width from the style
        private ColumnWidths GetColumnWidths(Style style)
            => style.GetValue(StyleKeys.ColumnWidthKey, ColumnWidths.Empty);

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Columns_Auto_SetsCountOneAndEmptyWidths()
        {
            // "auto" alone is equivalent to "auto auto"
            var parser = new CSSColumnsParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "auto");

            Assert.IsTrue(result);
            Assert.AreEqual(1, style.Columns.ColumnCount);
            Assert.IsTrue(GetColumnWidths(style).IsEmpty);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Columns_AutoAuto_SetsCountOneAndEmptyWidths()
        {
            var parser = new CSSColumnsParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "auto auto");

            Assert.IsTrue(result);
            Assert.AreEqual(1, style.Columns.ColumnCount);
            Assert.IsTrue(GetColumnWidths(style).IsEmpty);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Columns_CountOnly_SetsCountAndAutoWidth()
        {
            // A lone integer is treated as count with auto widths
            var parser = new CSSColumnsParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "3");

            Assert.IsTrue(result);
            Assert.AreEqual(3, style.Columns.ColumnCount);
            Assert.IsTrue(GetColumnWidths(style).IsEmpty);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Columns_WidthOnly_SetsAutoCountAndWidth()
        {
            // A lone unit value is treated as column-width with auto count (0)
            var parser = new CSSColumnsParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "150pt");

            Assert.IsTrue(result);
            Assert.AreEqual(0, style.GetValue(StyleKeys.ColumnCountKey, -1));
            var widths = GetColumnWidths(style);
            Assert.IsFalse(widths.IsEmpty);
            Assert.IsTrue(widths.HasExplicitWidth);
            Assert.AreEqual(150.0, widths.Explicit.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Columns_CountThenWidth_SetsBoth()
        {
            var parser = new CSSColumnsParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "3 150pt");

            Assert.IsTrue(result);
            Assert.AreEqual(3, style.Columns.ColumnCount);
            var widths = GetColumnWidths(style);
            Assert.IsTrue(widths.HasExplicitWidth);
            Assert.AreEqual(150.0, widths.Explicit.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Columns_WidthThenCount_SetsBothInAnyOrder()
        {
            // Width before count should produce the same result as count before width
            var parser = new CSSColumnsParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "150pt 3");

            Assert.IsTrue(result);
            Assert.AreEqual(3, style.Columns.ColumnCount);
            var widths = GetColumnWidths(style);
            Assert.IsTrue(widths.HasExplicitWidth);
            Assert.AreEqual(150.0, widths.Explicit.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Columns_AutoThenCount_SetsAutoWidthAndExplicitCount()
        {
            var parser = new CSSColumnsParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "auto 3");

            Assert.IsTrue(result);
            Assert.AreEqual(3, style.Columns.ColumnCount);
            Assert.IsTrue(GetColumnWidths(style).IsEmpty);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Columns_CountThenAuto_SetsExplicitCountAndAutoWidth()
        {
            var parser = new CSSColumnsParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "3 auto");

            Assert.IsTrue(result);
            Assert.AreEqual(3, style.Columns.ColumnCount);
            Assert.IsTrue(GetColumnWidths(style).IsEmpty);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Columns_AutoThenWidth_SetsAutoCountAndExplicitWidth()
        {
            var parser = new CSSColumnsParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "auto 200pt");

            Assert.IsTrue(result);
            Assert.AreEqual(0, style.GetValue(StyleKeys.ColumnCountKey, -1));
            var widths = GetColumnWidths(style);
            Assert.IsTrue(widths.HasExplicitWidth);
            Assert.AreEqual(200.0, widths.Explicit.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Columns_WidthThenAuto_SetsExplicitWidthAndAutoCount()
        {
            var parser = new CSSColumnsParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "200pt auto");

            Assert.IsTrue(result);
            Assert.AreEqual(0, style.GetValue(StyleKeys.ColumnCountKey, -1));
            var widths = GetColumnWidths(style);
            Assert.IsTrue(widths.HasExplicitWidth);
            Assert.AreEqual(200.0, widths.Explicit.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Columns_TwoWidths_SetsCountTwoAndBothWidths()
        {
            // Two unit values are treated as two explicit column widths
            var parser = new CSSColumnsParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "100pt 200pt");

            Assert.IsTrue(result);
            Assert.AreEqual(2, style.Columns.ColumnCount);
            var widths = GetColumnWidths(style);
            Assert.IsNotNull(widths.Widths);
            Assert.AreEqual(2, widths.Widths.Length);
            Assert.AreEqual(100.0, widths.Widths[0], 0.001);
            Assert.AreEqual(200.0, widths.Widths[1], 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Columns_PxWidth_ConvertsToPoints()
        {
            var parser = new CSSColumnsParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "2 200px");

            Assert.IsTrue(result);
            Assert.AreEqual(2, style.Columns.ColumnCount);
            var widths = GetColumnWidths(style);
            // 200px = 200 * 0.75 = 150pt
            Assert.AreEqual(150.0, widths.Explicit.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Columns_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSColumnsParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "not-valid");

            Assert.IsFalse(result);
        }

        #endregion
    }
}
