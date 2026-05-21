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
    /// Tests for CSS position and sizing parsers: width, height, min-width, min-height,
    /// max-width, max-height, top, right, bottom, left
    /// Covers 10 position/sizing parsers
    /// </summary>
    [TestClass()]
    public class CSSPositionSizingParsers_Tests
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

        #region Width Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Width_ValidUnit_Pt_SetsWidth()
        {
            var parser = new CSSWidthParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "100pt");

            Assert.IsTrue(result);
            Assert.AreEqual(100.0, style.Size.Width.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Width_ValidUnit_Px_SetsWidth()
        {
            var parser = new CSSWidthParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "200px");

            Assert.IsTrue(result);
            // 200px = 200 * 0.75 = 150pt
            Assert.AreEqual(150.0, style.Size.Width.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Width_AutoValue_SetsAuto()
        {
            var parser = new CSSWidthParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "auto");

            Assert.IsTrue(result);
            Assert.AreEqual(Unit.AutoValue, style.Size.Width);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Width_ZeroValue_SetsZero()
        {
            var parser = new CSSWidthParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "0");

            Assert.IsTrue(result);
            Assert.AreEqual(0.0, style.Size.Width.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Width_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSWidthParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid");

            Assert.IsFalse(result);
        }

        #endregion

        #region Height Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Height_ValidUnit_Pt_SetsHeight()
        {
            var parser = new CSSHeightParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "200pt");

            Assert.IsTrue(result);
            Assert.AreEqual(200.0, style.Size.Height.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Height_ValidUnit_Px_SetsHeight()
        {
            var parser = new CSSHeightParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "300px");

            Assert.IsTrue(result);
            // 300px = 300 * 0.75 = 225pt
            Assert.AreEqual(225.0, style.Size.Height.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Height_AutoValue_SetsAuto()
        {
            var parser = new CSSHeightParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "auto");

            Assert.IsTrue(result);
            Assert.AreEqual(Unit.AutoValue, style.Size.Height);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Height_ZeroValue_SetsZero()
        {
            var parser = new CSSHeightParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "0");

            Assert.IsTrue(result);
            Assert.AreEqual(0.0, style.Size.Height.PointsValue, 0.001);
        }

        #endregion

        #region Min-Width Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void MinWidth_ValidUnit_Pt_SetsMinWidth()
        {
            var parser = new CSSMinWidthParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "50pt");

            Assert.IsTrue(result);
            Assert.AreEqual(50.0, style.Size.MinimumWidth.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void MinWidth_ValidUnit_Px_SetsMinWidth()
        {
            var parser = new CSSMinWidthParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "100px");

            Assert.IsTrue(result);
            // 100px = 100 * 0.75 = 75pt
            Assert.AreEqual(75.0, style.Size.MinimumWidth.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void MinWidth_ZeroValue_SetsZero()
        {
            var parser = new CSSMinWidthParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "0");

            Assert.IsTrue(result);
            Assert.AreEqual(0.0, style.Size.MinimumWidth.PointsValue, 0.001);
        }

        #endregion

        #region Min-Height Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void MinHeight_ValidUnit_Pt_SetsMinHeight()
        {
            var parser = new CSSMinHeightParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "75pt");

            Assert.IsTrue(result);
            Assert.AreEqual(75.0, style.Size.MinimumHeight.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void MinHeight_ValidUnit_Px_SetsMinHeight()
        {
            var parser = new CSSMinHeightParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "120px");

            Assert.IsTrue(result);
            // 120px = 120 * 0.75 = 90pt
            Assert.AreEqual(90.0, style.Size.MinimumHeight.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void MinHeight_ZeroValue_SetsZero()
        {
            var parser = new CSSMinHeightParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "0");

            Assert.IsTrue(result);
            Assert.AreEqual(0.0, style.Size.MinimumHeight.PointsValue, 0.001);
        }

        #endregion

        #region Max-Width Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void MaxWidth_ValidUnit_Pt_SetsMaxWidth()
        {
            var parser = new CSSMaxWidthParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "500pt");

            Assert.IsTrue(result);
            Assert.AreEqual(500.0, style.Size.MaximumWidth.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void MaxWidth_ValidUnit_Px_SetsMaxWidth()
        {
            var parser = new CSSMaxWidthParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "600px");

            Assert.IsTrue(result);
            // 600px = 600 * 0.75 = 450pt
            Assert.AreEqual(450.0, style.Size.MaximumWidth.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void MaxWidth_NoneValue_SetsNone()
        {
            var parser = new CSSMaxWidthParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "none");

            Assert.IsTrue(result);
            // 'none' should set an empty or infinite value
        }

        #endregion

        #region Max-Height Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void MaxHeight_ValidUnit_Pt_SetsMaxHeight()
        {
            var parser = new CSSMaxHeightParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "400pt");

            Assert.IsTrue(result);
            Assert.AreEqual(400.0, style.Size.MaximumHeight.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void MaxHeight_ValidUnit_Px_SetsMaxHeight()
        {
            var parser = new CSSMaxHeightParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "800px");

            Assert.IsTrue(result);
            // 800px = 800 * 0.75 = 600pt
            Assert.AreEqual(600.0, style.Size.MaximumHeight.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void MaxHeight_NoneValue_SetsNone()
        {
            var parser = new CSSMaxHeightParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "none");

            Assert.IsTrue(result);
            // 'none' should set an empty or infinite value
        }

        #endregion

        #region Position Top Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Top_ValidUnit_Pt_SetsTop()
        {
            var parser = new CSSTopParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "20pt");

            Assert.IsTrue(result);
            Assert.AreEqual(20.0, style.Position.Y.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Top_ValidUnit_Px_SetsTop()
        {
            var parser = new CSSTopParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "40px");

            Assert.IsTrue(result);
            // 40px = 40 * 0.75 = 30pt
            Assert.AreEqual(30.0, style.Position.Y.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Top_ZeroValue_SetsZero()
        {
            var parser = new CSSTopParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "0");

            Assert.IsTrue(result);
            Assert.AreEqual(0.0, style.Position.Y.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Top_NegativeValue_SetsNegative()
        {
            var parser = new CSSTopParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "-10pt");

            Assert.IsTrue(result);
            Assert.AreEqual(-10.0, style.Position.Y.PointsValue, 0.001);
        }

        #endregion

        #region Position Right Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Right_ValidUnit_Pt_SetsRight()
        {
            var parser = new CSSRightParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "15pt");

            Assert.IsTrue(result);
            Assert.AreEqual(15.0, style.Position.Right.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Right_ValidUnit_Px_SetsRight()
        {
            var parser = new CSSRightParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "32px");

            Assert.IsTrue(result);
            // 32px = 32 * 0.75 = 24pt
            Assert.AreEqual(24.0, style.Position.Right.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Right_ZeroValue_SetsZero()
        {
            var parser = new CSSRightParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "0");

            Assert.IsTrue(result);
            Assert.AreEqual(0.0, style.Position.Right.PointsValue, 0.001);
        }

        #endregion

        #region Position Bottom Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Bottom_ValidUnit_Pt_SetsBottom()
        {
            var parser = new CSSBottomParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "25pt");

            Assert.IsTrue(result);
            Assert.AreEqual(25.0, style.Position.Bottom.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Bottom_ValidUnit_Px_SetsBottom()
        {
            var parser = new CSSBottomParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "48px");

            Assert.IsTrue(result);
            // 48px = 48 * 0.75 = 36pt
            Assert.AreEqual(36.0, style.Position.Bottom.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Bottom_ZeroValue_SetsZero()
        {
            var parser = new CSSBottomParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "0");

            Assert.IsTrue(result);
            Assert.AreEqual(0.0, style.Position.Bottom.PointsValue, 0.001);
        }

        #endregion

        #region Position Left Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Left_ValidUnit_Pt_SetsLeft()
        {
            var parser = new CSSLeftParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "30pt");

            Assert.IsTrue(result);
            Assert.AreEqual(30.0, style.Position.X.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Left_ValidUnit_Px_SetsLeft()
        {
            var parser = new CSSLeftParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "64px");

            Assert.IsTrue(result);
            // 64px = 64 * 0.75 = 48pt
            Assert.AreEqual(48.0, style.Position.X.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Left_ZeroValue_SetsZero()
        {
            var parser = new CSSLeftParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "0");

            Assert.IsTrue(result);
            Assert.AreEqual(0.0, style.Position.X.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Left_NegativeValue_SetsNegative()
        {
            var parser = new CSSLeftParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "-20pt");

            Assert.IsTrue(result);
            Assert.AreEqual(-20.0, style.Position.X.PointsValue, 0.001);
        }

        #endregion

        #region Z-Index Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ZIndex_PositiveInteger_SetsZIndex()
        {
            var parser = new CSSZIndexParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "5");

            Assert.IsTrue(result);
            Assert.AreEqual(5, style.Position.ZIndex);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ZIndex_NegativeInteger_SetsZIndex()
        {
            var parser = new CSSZIndexParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "-1");

            Assert.IsTrue(result);
            Assert.AreEqual(-1, style.Position.ZIndex);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ZIndex_Zero_SetsZero()
        {
            var parser = new CSSZIndexParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "0");

            Assert.IsTrue(result);
            Assert.AreEqual(0, style.Position.ZIndex);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ZIndex_Auto_SetsZero()
        {
            // 'auto' is equivalent to z-index: 0 in our implementation
            var parser = new CSSZIndexParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "auto");

            Assert.IsTrue(result);
            Assert.AreEqual(0, style.Position.ZIndex);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ZIndex_LargePositive_SetsZIndex()
        {
            var parser = new CSSZIndexParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "999");

            Assert.IsTrue(result);
            Assert.AreEqual(999, style.Position.ZIndex);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ZIndex_LargeNegative_SetsZIndex()
        {
            var parser = new CSSZIndexParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "-100");

            Assert.IsTrue(result);
            Assert.AreEqual(-100, style.Position.ZIndex);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ZIndex_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSZIndexParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "abc");

            Assert.IsFalse(result);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ZIndex_WhitespaceOnly_ReturnsFalse()
        {
            // The reader requires at least one non-empty value token; whitespace yields no token.
            var parser = new CSSZIndexParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "   ");

            Assert.IsFalse(result);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ZIndex_DefaultStyle_ReturnsZero()
        {
            // Without setting z-index, the default should be 0
            var style = CreateStyle();
            Assert.AreEqual(0, style.Position.ZIndex);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ZIndex_SetViaStyleItem_CanBeRead()
        {
            // Set via style item property, confirm round-trip
            var style = CreateStyle();
            style.Position.ZIndex = 10;
            Assert.AreEqual(10, style.Position.ZIndex);

            style.Position.ZIndex = -5;
            Assert.AreEqual(-5, style.Position.ZIndex);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ZIndex_RemoveZIndex_ReturnsDefault()
        {
            var style = CreateStyle();
            style.Position.ZIndex = 7;
            Assert.AreEqual(7, style.Position.ZIndex);

            style.Position.RemoveZIndex();
            Assert.AreEqual(0, style.Position.ZIndex);
        }

        #endregion
    }
}
