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
    /// Tests for CSS spacing parsers: margin and padding properties
    /// Covers 16 parsers: margin (8) and padding (8) with shorthand and individual sides
    /// </summary>
    [TestClass()]
    public class CSSSpacingParsers_Tests
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
            // Create a reader directly from the CSS value
            var reader = new CSSStyleItemReader(cssValue);
            return reader;
        }

        private bool ParseValue(CSSStyleValueParser parser, Style style, string cssValue)
        {
            var reader = CreateReader(cssValue);
            return parser.SetStyleValue(style, reader, null);
        }

        #endregion

        #region Margin Top Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void MarginsTop_ValidUnit_Pt_SetsCorrectly()
        {
            var parser = new CSSMarginsTopParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "20pt");

            Assert.IsTrue(result);
            Assert.AreEqual(20.0, style.Margins.Top.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void MarginsTop_ValidUnit_Px_SetsCorrectly()
        {
            var parser = new CSSMarginsTopParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "16px");

            Assert.IsTrue(result);
            // 16px = 16 * 0.75 = 12pt
            Assert.AreEqual(12.0, style.Margins.Top.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void MarginsTop_AutoValue_SetsAuto()
        {
            var parser = new CSSMarginsTopParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "auto");

            Assert.IsTrue(result);
            Assert.AreEqual(Unit.AutoValue, style.Margins.Top);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void MarginsTop_ZeroValue_SetsZero()
        {
            var parser = new CSSMarginsTopParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "0");

            Assert.IsTrue(result);
            Assert.AreEqual(0.0, style.Margins.Top.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void MarginsTop_NegativeValue_SetsNegative()
        {
            var parser = new CSSMarginsTopParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "-10pt");

            Assert.IsTrue(result);
            Assert.AreEqual(-10.0, style.Margins.Top.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void MarginsTop_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSMarginsTopParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid");

            Assert.IsFalse(result);
        }

        #endregion

        #region Margin Right Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void MarginsRight_ValidUnit_Mm_SetsCorrectly()
        {
            var parser = new CSSMarginsRightParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "10mm");

            Assert.IsTrue(result);
            // 10mm ≈ 28.35pt
            Assert.AreEqual(28.35, style.Margins.Right.PointsValue, 0.1);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void MarginsRight_AutoValue_SetsAuto()
        {
            var parser = new CSSMarginsRightParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "auto");

            Assert.IsTrue(result);
            Assert.AreEqual(Unit.AutoValue, style.Margins.Right);
        }

        #endregion

        #region Margin Bottom Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void MarginsBottom_ValidUnit_Cm_SetsCorrectly()
        {
            var parser = new CSSMarginsBottomParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "2cm");

            Assert.IsTrue(result);
            // 2cm ≈ 56.69pt
            Assert.AreEqual(56.69, style.Margins.Bottom.PointsValue, 0.1);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void MarginsBottom_AutoValue_SetsAuto()
        {
            var parser = new CSSMarginsBottomParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "auto");

            Assert.IsTrue(result);
            Assert.AreEqual(Unit.AutoValue, style.Margins.Bottom);
        }

        #endregion

        #region Margin Left Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void MarginsLeft_ValidUnit_In_SetsCorrectly()
        {
            var parser = new CSSMarginsLeftParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "1in");

            Assert.IsTrue(result);
            Assert.AreEqual(72.0, style.Margins.Left.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void MarginsLeft_AutoValue_SetsAuto()
        {
            var parser = new CSSMarginsLeftParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "auto");

            Assert.IsTrue(result);
            Assert.AreEqual(Unit.AutoValue, style.Margins.Left);
        }

        #endregion

        #region Margin All Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void MarginsAll_SingleValue_SetsAllSides()
        {
            var parser = new CSSMarginsAllParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "15pt");

            Assert.IsTrue(result);
            Assert.AreEqual(15.0, style.Margins.Top.PointsValue, 0.001);
            Assert.AreEqual(15.0, style.Margins.Right.PointsValue, 0.001);
            Assert.AreEqual(15.0, style.Margins.Bottom.PointsValue, 0.001);
            Assert.AreEqual(15.0, style.Margins.Left.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void MarginsAll_TwoValues_SetsTopBottomAndLeftRight()
        {
            var parser = new CSSMarginsAllParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "10pt 20pt");

            Assert.IsTrue(result);
            Assert.AreEqual(10.0, style.Margins.Top.PointsValue, 0.001);
            Assert.AreEqual(20.0, style.Margins.Right.PointsValue, 0.001);
            Assert.AreEqual(10.0, style.Margins.Bottom.PointsValue, 0.001);
            Assert.AreEqual(20.0, style.Margins.Left.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void MarginsAll_ThreeValues_SetsTopLeftRightBottom()
        {
            var parser = new CSSMarginsAllParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "10pt 20pt 30pt");

            Assert.IsTrue(result);
            Assert.AreEqual(10.0, style.Margins.Top.PointsValue, 0.001);
            Assert.AreEqual(20.0, style.Margins.Right.PointsValue, 0.001);
            Assert.AreEqual(30.0, style.Margins.Bottom.PointsValue, 0.001);
            Assert.AreEqual(20.0, style.Margins.Left.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void MarginsAll_FourValues_SetsAllSidesIndividually()
        {
            var parser = new CSSMarginsAllParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "10pt 20pt 30pt 40pt");

            Assert.IsTrue(result);
            Assert.AreEqual(10.0, style.Margins.Top.PointsValue, 0.001);
            Assert.AreEqual(20.0, style.Margins.Right.PointsValue, 0.001);
            Assert.AreEqual(30.0, style.Margins.Bottom.PointsValue, 0.001);
            Assert.AreEqual(40.0, style.Margins.Left.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void MarginsAll_MixedAutoAndUnits_SetsBoth()
        {
            var parser = new CSSMarginsAllParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "auto 10pt");

            Assert.IsTrue(result);
            Assert.AreEqual(Unit.AutoValue, style.Margins.Top);
            Assert.AreEqual(10.0, style.Margins.Right.PointsValue, 0.001);
            Assert.AreEqual(Unit.AutoValue, style.Margins.Bottom);
            Assert.AreEqual(10.0, style.Margins.Left.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void MarginsAll_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSMarginsAllParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid");

            Assert.IsFalse(result);
        }

        #endregion

        #region Margin Inline Parsers Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void MarginsInlineStart_ValidUnit_SetsCorrectly()
        {
            var parser = new CSSMarginsInlineStartParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "25pt");

            Assert.IsTrue(result);
            Assert.AreEqual(25.0, style.Margins.Left.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void MarginsInlineEnd_ValidUnit_SetsCorrectly()
        {
            var parser = new CSSMarginsInlineEndParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "30pt");

            Assert.IsTrue(result);
            Assert.AreEqual(30.0, style.Margins.Right.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void MarginsInlineBoth_SingleValue_SetsBothSides()
        {
            var parser = new CSSMarginsInlineBothParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "12pt");

            Assert.IsTrue(result);
            Assert.AreEqual(12.0, style.Margins.Left.PointsValue, 0.001);
            Assert.AreEqual(12.0, style.Margins.Right.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void MarginsInlineBoth_TwoValues_SetsStartAndEnd()
        {
            var parser = new CSSMarginsInlineBothParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "10pt 20pt");

            Assert.IsTrue(result);
            Assert.AreEqual(10.0, style.Margins.Left.PointsValue, 0.001);
            Assert.AreEqual(20.0, style.Margins.Right.PointsValue, 0.001);
        }

        #endregion

        #region Padding Top Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PaddingTop_ValidUnit_Pt_SetsCorrectly()
        {
            var parser = new CSSPaddingTopParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "15pt");

            Assert.IsTrue(result);
            Assert.AreEqual(15.0, style.Padding.Top.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PaddingTop_ValidUnit_Px_SetsCorrectly()
        {
            var parser = new CSSPaddingTopParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "24px");

            Assert.IsTrue(result);
            // 24px = 24 * 0.75 = 18pt
            Assert.AreEqual(18.0, style.Padding.Top.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PaddingTop_ZeroValue_SetsZero()
        {
            var parser = new CSSPaddingTopParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "0");

            Assert.IsTrue(result);
            Assert.AreEqual(0.0, style.Padding.Top.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PaddingTop_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSPaddingTopParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid");

            Assert.IsFalse(result);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PaddingTop_AutoValue_ReturnsFalse()
        {
            // Padding does not support 'auto'
            var parser = new CSSPaddingTopParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "auto");

            Assert.IsFalse(result);
        }

        #endregion

        #region Padding Right Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PaddingRight_ValidUnit_Mm_SetsCorrectly()
        {
            var parser = new CSSPaddingRightParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "5mm");

            Assert.IsTrue(result);
            // 5mm ≈ 14.17pt
            Assert.AreEqual(14.17, style.Padding.Right.PointsValue, 0.1);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PaddingRight_ZeroValue_SetsZero()
        {
            var parser = new CSSPaddingRightParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "0");

            Assert.IsTrue(result);
            Assert.AreEqual(0.0, style.Padding.Right.PointsValue, 0.001);
        }

        #endregion

        #region Padding Bottom Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PaddingBottom_ValidUnit_Cm_SetsCorrectly()
        {
            var parser = new CSSPaddingBottomParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "1cm");

            Assert.IsTrue(result);
            // 1cm ≈ 28.35pt
            Assert.AreEqual(28.35, style.Padding.Bottom.PointsValue, 0.1);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PaddingBottom_ZeroValue_SetsZero()
        {
            var parser = new CSSPaddingBottomParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "0");

            Assert.IsTrue(result);
            Assert.AreEqual(0.0, style.Padding.Bottom.PointsValue, 0.001);
        }

        #endregion

        #region Padding Left Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PaddingLeft_ValidUnit_In_SetsCorrectly()
        {
            var parser = new CSSPaddingLeftParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "0.5in");

            Assert.IsTrue(result);
            Assert.AreEqual(36.0, style.Padding.Left.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PaddingLeft_ZeroValue_SetsZero()
        {
            var parser = new CSSPaddingLeftParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "0");

            Assert.IsTrue(result);
            Assert.AreEqual(0.0, style.Padding.Left.PointsValue, 0.001);
        }

        #endregion

        #region Padding All Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PaddingAll_SingleValue_SetsAllSides()
        {
            var parser = new CSSPaddingAllParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "12pt");

            Assert.IsTrue(result);
            Assert.AreEqual(12.0, style.Padding.Top.PointsValue, 0.001);
            Assert.AreEqual(12.0, style.Padding.Right.PointsValue, 0.001);
            Assert.AreEqual(12.0, style.Padding.Bottom.PointsValue, 0.001);
            Assert.AreEqual(12.0, style.Padding.Left.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PaddingAll_TwoValues_SetsTopBottomAndLeftRight()
        {
            var parser = new CSSPaddingAllParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "8pt 16pt");

            Assert.IsTrue(result);
            Assert.AreEqual(8.0, style.Padding.Top.PointsValue, 0.001);
            Assert.AreEqual(16.0, style.Padding.Right.PointsValue, 0.001);
            Assert.AreEqual(8.0, style.Padding.Bottom.PointsValue, 0.001);
            Assert.AreEqual(16.0, style.Padding.Left.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PaddingAll_ThreeValues_SetsTopLeftRightBottom()
        {
            var parser = new CSSPaddingAllParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "5pt 10pt 15pt");

            Assert.IsTrue(result);
            Assert.AreEqual(5.0, style.Padding.Top.PointsValue, 0.001);
            Assert.AreEqual(10.0, style.Padding.Right.PointsValue, 0.001);
            Assert.AreEqual(15.0, style.Padding.Bottom.PointsValue, 0.001);
            Assert.AreEqual(10.0, style.Padding.Left.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PaddingAll_FourValues_SetsAllSidesIndividually()
        {
            var parser = new CSSPaddingAllParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "5pt 10pt 15pt 20pt");

            Assert.IsTrue(result);
            Assert.AreEqual(5.0, style.Padding.Top.PointsValue, 0.001);
            Assert.AreEqual(10.0, style.Padding.Right.PointsValue, 0.001);
            Assert.AreEqual(15.0, style.Padding.Bottom.PointsValue, 0.001);
            Assert.AreEqual(20.0, style.Padding.Left.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PaddingAll_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSPaddingAllParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid");

            Assert.IsFalse(result);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PaddingAll_WithAuto_ReturnsFalse()
        {
            // Padding does not support 'auto'
            var parser = new CSSPaddingAllParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "auto");

            Assert.IsFalse(result);
        }

        #endregion

        #region Padding Inline Parsers Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PaddingInlineStart_ValidUnit_SetsCorrectly()
        {
            var parser = new CSSPaddingInlineStartParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "18pt");

            Assert.IsTrue(result);
            Assert.AreEqual(18.0, style.Padding.Left.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PaddingInlineEnd_ValidUnit_SetsCorrectly()
        {
            var parser = new CSSPaddingInlineEndParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "22pt");

            Assert.IsTrue(result);
            Assert.AreEqual(22.0, style.Padding.Right.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PaddingInlineBoth_SingleValue_SetsBothSides()
        {
            var parser = new CSSPaddingInlineBothParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "14pt");

            Assert.IsTrue(result);
            Assert.AreEqual(14.0, style.Padding.Left.PointsValue, 0.001);
            Assert.AreEqual(14.0, style.Padding.Right.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PaddingInlineBoth_TwoValues_SetsStartAndEnd()
        {
            var parser = new CSSPaddingInlineBothParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "12pt 16pt");

            Assert.IsTrue(result);
            Assert.AreEqual(12.0, style.Padding.Left.PointsValue, 0.001);
            Assert.AreEqual(16.0, style.Padding.Right.PointsValue, 0.001);
        }

        #endregion
    }
}
