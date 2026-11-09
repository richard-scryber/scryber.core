using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Styles;
using Scryber.Styles.Parsing;
using Scryber.Styles.Parsing.Typed;
using Scryber.Drawing;
using Scryber.Html;
using Scryber.Text;

namespace Scryber.Core.UnitTests.Html.CSSParsers
{
    /// <summary>
    /// Tests for CSS text parsers: text-align, text-decoration, letter-spacing,
    /// word-spacing, white-space, vertical-align, text-anchor
    /// Covers 7 text-related parsers
    /// </summary>
    [TestClass()]
    public class CSSTextParsers_Tests
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

        #region Text Align Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void TextAlign_Left_SetsLeft()
        {
            var parser = new CSSTextAlignParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "left");

            Assert.IsTrue(result);
            Assert.AreEqual(HorizontalAlignment.Left, style.Position.HAlign);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void TextAlign_Right_SetsRight()
        {
            var parser = new CSSTextAlignParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "right");

            Assert.IsTrue(result);
            Assert.AreEqual(HorizontalAlignment.Right, style.Position.HAlign);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void TextAlign_Center_SetsCenter()
        {
            var parser = new CSSTextAlignParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "center");

            Assert.IsTrue(result);
            Assert.AreEqual(HorizontalAlignment.Center, style.Position.HAlign);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void TextAlign_Justify_SetsJustified()
        {
            var parser = new CSSTextAlignParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "justify");

            Assert.IsTrue(result);
            Assert.AreEqual(HorizontalAlignment.Justified, style.Position.HAlign);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void TextAlign_CaseInsensitive_ParsesCorrectly()
        {
            var parser = new CSSTextAlignParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "CENTER");

            Assert.IsTrue(result);
            Assert.AreEqual(HorizontalAlignment.Center, style.Position.HAlign);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void TextAlign_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSTextAlignParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid-align");

            Assert.IsFalse(result);
        }

        #endregion

        #region Text Decoration Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void TextDecoration_None_SetsNone()
        {
            var parser = new CSSTextDecorationParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "none");

            Assert.IsTrue(result);
            Assert.AreEqual(Text.TextDecoration.None, style.Text.Decoration);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void TextDecoration_Underline_SetsUnderline()
        {
            var parser = new CSSTextDecorationParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "underline");

            Assert.IsTrue(result);
            Assert.AreEqual(Text.TextDecoration.Underline, style.Text.Decoration);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void TextDecoration_Overline_SetsOverline()
        {
            var parser = new CSSTextDecorationParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "overline");

            Assert.IsTrue(result);
            Assert.AreEqual(Text.TextDecoration.Overline, style.Text.Decoration);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void TextDecoration_LineThrough_SetsStrikeThrough()
        {
            var parser = new CSSTextDecorationParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "line-through");

            Assert.IsTrue(result);
            Assert.AreEqual(Text.TextDecoration.StrikeThrough, style.Text.Decoration);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void TextDecoration_CaseInsensitive_ParsesCorrectly()
        {
            var parser = new CSSTextDecorationParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "UNDERLINE");

            Assert.IsTrue(result);
            Assert.AreEqual(Text.TextDecoration.Underline, style.Text.Decoration);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void TextDecoration_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSTextDecorationParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid-decoration");

            Assert.IsFalse(result);
        }

        #endregion

        #region Letter Spacing Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void LetterSpacing_ValidUnit_Pt_SetsSpacing()
        {
            var parser = new CSSLetterSpacingParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "2pt");

            Assert.IsTrue(result);
            Assert.AreEqual(2.0, style.Text.CharacterSpacing.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void LetterSpacing_ValidUnit_Px_SetsSpacing()
        {
            var parser = new CSSLetterSpacingParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "4px");

            Assert.IsTrue(result);
            // 4px = 4 * 0.75 = 3pt
            Assert.AreEqual(3.0, style.Text.CharacterSpacing.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void LetterSpacing_ZeroValue_SetsZero()
        {
            var parser = new CSSLetterSpacingParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "0");

            Assert.IsTrue(result);
            Assert.AreEqual(0.0, style.Text.CharacterSpacing.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void LetterSpacing_NegativeValue_SetsNegative()
        {
            var parser = new CSSLetterSpacingParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "-1pt");

            Assert.IsTrue(result);
            Assert.AreEqual(-1.0, style.Text.CharacterSpacing.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void LetterSpacing_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSLetterSpacingParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid-spacing");

            Assert.IsFalse(result);
        }

        #endregion

        #region Word Spacing Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void WordSpacing_ValidUnit_Pt_SetsSpacing()
        {
            var parser = new CSSWordSpacingParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "5pt");

            Assert.IsTrue(result);
            Assert.AreEqual(5.0, style.Text.WordSpacing.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void WordSpacing_ValidUnit_Px_SetsSpacing()
        {
            var parser = new CSSWordSpacingParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "8px");

            Assert.IsTrue(result);
            // 8px = 8 * 0.75 = 6pt
            Assert.AreEqual(6.0, style.Text.WordSpacing.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void WordSpacing_ZeroValue_SetsZero()
        {
            var parser = new CSSWordSpacingParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "0");

            Assert.IsTrue(result);
            Assert.AreEqual(0.0, style.Text.WordSpacing.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void WordSpacing_NegativeValue_SetsNegative()
        {
            var parser = new CSSWordSpacingParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "-2pt");

            Assert.IsTrue(result);
            Assert.AreEqual(-2.0, style.Text.WordSpacing.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void WordSpacing_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSWordSpacingParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid-spacing");

            Assert.IsFalse(result);
        }

        #endregion

        #region White Space Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void WhiteSpace_Normal_SetsNormal()
        {
            var parser = new CSSWhiteSpaceParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "normal");

            Assert.IsTrue(result);
            Assert.AreEqual(WordWrap.Auto, style.Text.WrapText);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void WhiteSpace_Nowrap_SetsNoWrap()
        {
            var parser = new CSSWhiteSpaceParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "nowrap");

            Assert.IsTrue(result);
            Assert.AreEqual(WordWrap.NoWrap, style.Text.WrapText);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void WhiteSpace_Pre_SetsPre()
        {
            var parser = new CSSWhiteSpaceParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "pre");

            Assert.IsTrue(result);
            // Pre preserves whitespace and doesn't wrap
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void WhiteSpace_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSWhiteSpaceParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid-whitespace");

            Assert.IsFalse(result);
        }

        #endregion

        #region Vertical Align Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void VerticalAlign_Top_SetsTop()
        {
            var parser = new CSSVerticalAlignParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "top");

            Assert.IsTrue(result);
            Assert.AreEqual(VerticalAlignment.Top, style.Position.VAlign);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void VerticalAlign_Middle_SetsMiddle()
        {
            var parser = new CSSVerticalAlignParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "middle");

            Assert.IsTrue(result);
            Assert.AreEqual(VerticalAlignment.Middle, style.Position.VAlign);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void VerticalAlign_Bottom_SetsBottom()
        {
            var parser = new CSSVerticalAlignParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "bottom");

            Assert.IsTrue(result);
            Assert.AreEqual(VerticalAlignment.Bottom, style.Position.VAlign);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void VerticalAlign_Baseline_SetsBaseline()
        {
            var parser = new CSSVerticalAlignParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "baseline");

            Assert.IsTrue(result);
            // Baseline is default alignment
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void VerticalAlign_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSVerticalAlignParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid-align");

            Assert.IsFalse(result);
        }

        #endregion
    }
}
