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
    /// Tests for CSS background parsers: background-color, background-image, background-repeat,
    /// background-position, background-size, and shorthand
    /// Covers 8 parsers for background properties
    /// </summary>
    [TestClass()]
    public class CSSBackgroundParsers_Tests
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

        #region Background Color Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BackgroundColor_HexColor_SetsColor()
        {
            var parser = new CSSBackgroundColorParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "#FF0000");

            Assert.IsTrue(result);
            Assert.AreEqual(Color.FromRGB(255, 0, 0), style.Background.Color);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BackgroundColor_NamedColor_SetsColor()
        {
            var parser = new CSSBackgroundColorParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "blue");

            Assert.IsTrue(result);
            Assert.AreEqual(Color.FromRGB(0, 0, 255), style.Background.Color);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BackgroundColor_RgbColor_SetsColor()
        {
            var parser = new CSSBackgroundColorParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "rgb(128, 64, 192)");

            Assert.IsTrue(result);
            Assert.AreEqual(Color.FromRGB(128, 64, 192), style.Background.Color);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BackgroundColor_RgbaColor_SetsColorAndOpacity()
        {
            var parser = new CSSBackgroundColorParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "rgba(255, 128, 0, 0.7)");

            Assert.IsTrue(result);
            Assert.AreEqual(Color.FromRGB(255, 128, 0), style.Background.Color);
            Assert.AreEqual(0.7, style.Background.Opacity, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BackgroundColor_Transparent_SetsTransparent()
        {
            var parser = new CSSBackgroundColorParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "transparent");

            Assert.IsTrue(result);
            Assert.AreEqual(Color.Transparent, style.Background.Color);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BackgroundColor_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSBackgroundColorParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "not-a-color");

            Assert.IsFalse(result);
        }

        #endregion

        #region Background Image Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BackgroundImage_UrlValue_SetsImageSource()
        {
            var parser = new CSSBackgroundImageParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "url(image.jpg)");

            Assert.IsTrue(result);
            Assert.AreEqual("image.jpg", style.Background.ImageSource);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BackgroundImage_UrlWithQuotes_SetsImageSource()
        {
            var parser = new CSSBackgroundImageParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "url('image.png')");

            Assert.IsTrue(result);
            Assert.AreEqual("image.png", style.Background.ImageSource);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BackgroundImage_UrlWithDoubleQuotes_SetsImageSource()
        {
            var parser = new CSSBackgroundImageParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "url(\"background.gif\")");

            Assert.IsTrue(result);
            Assert.AreEqual("background.gif", style.Background.ImageSource);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BackgroundImage_None_SetsNoImage()
        {
            var parser = new CSSBackgroundImageParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "none");

            Assert.IsTrue(result);
            Assert.IsTrue(string.IsNullOrEmpty(style.Background.ImageSource));
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BackgroundImage_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSBackgroundImageParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid-image");

            Assert.IsFalse(result);
        }

        #endregion

        #region Background Repeat Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BackgroundRepeat_Repeat_SetsRepeatBoth()
        {
            var parser = new CSSBackgroundRepeatParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "repeat");

            Assert.IsTrue(result);
            Assert.AreEqual(PatternRepeat.RepeatBoth, style.Background.PatternRepeat);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BackgroundRepeat_RepeatX_SetsRepeatX()
        {
            var parser = new CSSBackgroundRepeatParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "repeat-x");

            Assert.IsTrue(result);
            Assert.AreEqual(PatternRepeat.RepeatX, style.Background.PatternRepeat);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BackgroundRepeat_RepeatY_SetsRepeatY()
        {
            var parser = new CSSBackgroundRepeatParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "repeat-y");

            Assert.IsTrue(result);
            Assert.AreEqual(PatternRepeat.RepeatY, style.Background.PatternRepeat);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BackgroundRepeat_NoRepeat_SetsNone()
        {
            var parser = new CSSBackgroundRepeatParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "no-repeat");

            Assert.IsTrue(result);
            Assert.AreEqual(PatternRepeat.None, style.Background.PatternRepeat);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BackgroundRepeat_CaseInsensitive_ParsesCorrectly()
        {
            var parser = new CSSBackgroundRepeatParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "REPEAT-X");

            Assert.IsTrue(result);
            Assert.AreEqual(PatternRepeat.RepeatX, style.Background.PatternRepeat);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BackgroundRepeat_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSBackgroundRepeatParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid-repeat");

            Assert.IsFalse(result);
        }

        #endregion

        #region Background Position Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BackgroundPositionX_ValidUnit_Pt_SetsPosition()
        {
            var parser = new CSSBackgroundPositionXParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "10pt");

            Assert.IsTrue(result);
            Assert.AreEqual(10.0, style.Background.PatternXPosition.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BackgroundPositionX_ValidUnit_Px_SetsPosition()
        {
            var parser = new CSSBackgroundPositionXParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "20px");

            Assert.IsTrue(result);
            // 20px = 20 * 0.75 = 15pt
            Assert.AreEqual(15.0, style.Background.PatternXPosition.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BackgroundPositionX_ZeroValue_SetsZero()
        {
            var parser = new CSSBackgroundPositionXParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "0");

            Assert.IsTrue(result);
            Assert.AreEqual(0.0, style.Background.PatternXPosition.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BackgroundPositionY_ValidUnit_Pt_SetsPosition()
        {
            var parser = new CSSBackgroundPositionYParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "15pt");

            Assert.IsTrue(result);
            Assert.AreEqual(15.0, style.Background.PatternYPosition.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BackgroundPositionY_ValidUnit_Px_SetsPosition()
        {
            var parser = new CSSBackgroundPositionYParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "24px");

            Assert.IsTrue(result);
            // 24px = 24 * 0.75 = 18pt
            Assert.AreEqual(18.0, style.Background.PatternYPosition.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BackgroundPositionY_NegativeValue_SetsNegative()
        {
            var parser = new CSSBackgroundPositionYParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "-5pt");

            Assert.IsTrue(result);
            Assert.AreEqual(-5.0, style.Background.PatternYPosition.PointsValue, 0.001);
        }

        #endregion

        #region Background Size Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BackgroundSize_ValidUnit_Pt_SetsSize()
        {
            var parser = new CSSBackgroundSizeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "100pt");

            Assert.IsTrue(result);
            Assert.AreEqual(100.0, style.Background.PatternXSize.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BackgroundSize_ValidUnit_Px_SetsSize()
        {
            var parser = new CSSBackgroundSizeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "200px");

            Assert.IsTrue(result);
            // 200px = 200 * 0.75 = 150pt
            Assert.AreEqual(150.0, style.Background.PatternXSize.PointsValue, 0.001);
        }
        
        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BackgroundSize_ValidUnit_Px_SetsBothSize()
        {
            var parser = new CSSBackgroundSizeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "200px 100px");

            Assert.IsTrue(result);
            // 200px = 200 * 0.75 = 150pt
            Assert.AreEqual(150.0, style.Background.PatternXSize.PointsValue, 0.001);
            // 210px = 100 * 0.75 = 75pt
            Assert.AreEqual(75.0, style.Background.PatternYSize.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BackgroundSize_ZeroValue_SetsZero()
        {
            var parser = new CSSBackgroundSizeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "0");

            Assert.IsTrue(result);
            Assert.AreEqual(0.0, style.Background.PatternXSize.PointsValue, 0.001);
        }

        #endregion
    }
}
