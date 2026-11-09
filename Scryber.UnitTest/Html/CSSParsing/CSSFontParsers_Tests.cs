using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Styles;
using Scryber.Styles.Parsing;
using Scryber.Styles.Parsing.Typed;
using Scryber.Drawing;
using Scryber.Html;
using Scryber.PDF;

namespace Scryber.Core.UnitTests.Html.CSSParsers
{
    /// <summary>
    /// Tests for CSS font parsers: font-family, font-size, font-weight, font-style,
    /// line-height, and related font properties
    /// Covers 9 font parsers
    /// </summary>
    [TestClass()]
    public class CSSFontParsers_Tests
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

        #region Font Family Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void FontFamily_SingleFont_SetsFamily()
        {
            var parser = new CSSFontFamilyParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "Arial");

            Assert.IsTrue(result);
            Assert.IsNotNull(style.Font.FontFamily);
            Assert.AreEqual("Arial", style.Font.FontFamily.FamilyName);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void FontFamily_MultipleFonts_SetsChain()
        {
            var parser = new CSSFontFamilyParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "Helvetica, Arial, sans-serif");

            Assert.IsTrue(result);
            Assert.IsNotNull(style.Font.FontFamily);
            Assert.AreEqual("Helvetica", style.Font.FontFamily.FamilyName);
            Assert.IsNotNull(style.Font.FontFamily.Next);
            Assert.AreEqual("Arial", style.Font.FontFamily.Next.FamilyName);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void FontFamily_QuotedFont_SetsFamily()
        {
            
            var parser = new CSSStyleItemAllParser();
            var style = CreateStyle();
            var reader = new CSSStyleItemReader("font-family: 'Times New Roman'");
            var result = parser.SetStyleValue(style, reader, null);
            
            Assert.IsTrue(result);
            Assert.IsNotNull(style.Font.FontFamily);
            Assert.AreEqual("Times New Roman", style.Font.FontFamily.FamilyName);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void FontFamily_DoubleQuotedFont_SetsFamily()
        {
            var parser = new CSSStyleItemAllParser();
            var style = CreateStyle();
            var reader = new CSSStyleItemReader("font-family: \"Courier New\"");
            var result = parser.SetStyleValue(style, reader, null);

            Assert.IsTrue(result);
            Assert.IsNotNull(style.Font.FontFamily);
            Assert.AreEqual("Courier New", style.Font.FontFamily.FamilyName);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void FontFamily_GenericFamily_SetsFamily()
        {
            var parser = new CSSFontFamilyParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "sans-serif");

            Assert.IsTrue(result);
            Assert.IsNotNull(style.Font.FontFamily);
            Assert.AreEqual("sans-serif", style.Font.FontFamily.FamilyName);
        }

        #endregion

        #region Font Size Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void FontSize_ValidUnit_Pt_SetsSize()
        {
            var parser = new CSSFontSizeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "12pt");

            Assert.IsTrue(result);
            Assert.AreEqual(12.0, style.Font.FontSize.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void FontSize_ValidUnit_Px_SetsSize()
        {
            var parser = new CSSFontSizeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "16px");

            Assert.IsTrue(result);
            // 16px = 16 * 0.75 = 12pt
            Assert.AreEqual(12.0, style.Font.FontSize.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void FontSize_KeywordSmall_SetsSmall()
        {
            var parser = new CSSFontSizeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "small");

            Assert.IsTrue(result);
            Assert.AreEqual(10.0, style.Font.FontSize.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void FontSize_KeywordMedium_SetsMedium()
        {
            var parser = new CSSFontSizeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "medium");

            Assert.IsTrue(result);
            Assert.AreEqual(12.0, style.Font.FontSize.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void FontSize_KeywordLarge_SetsLarge()
        {
            var parser = new CSSFontSizeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "large");

            Assert.IsTrue(result);
            Assert.AreEqual(16.0, style.Font.FontSize.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void FontSize_KeywordXSmall_SetsXSmall()
        {
            var parser = new CSSFontSizeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "x-small");

            Assert.IsTrue(result);
            Assert.AreEqual(8.0, style.Font.FontSize.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void FontSize_KeywordXXLarge_SetsXXLarge()
        {
            var parser = new CSSFontSizeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "xx-large");

            Assert.IsTrue(result);
            Assert.AreEqual(32.0, style.Font.FontSize.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void FontSize_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSFontSizeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid-size");

            Assert.IsFalse(result);
        }

        #endregion

        #region Font Weight Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void FontWeight_KeywordNormal_SetsRegular()
        {
            var parser = new CSSFontWeightParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "normal");

            Assert.IsTrue(result);
            Assert.AreEqual(FontWeights.Regular, style.Font.FontWeight);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void FontWeight_KeywordBold_SetsBold()
        {
            var parser = new CSSFontWeightParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "bold");

            Assert.IsTrue(result);
            Assert.AreEqual(FontWeights.Bold, style.Font.FontWeight);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void FontWeight_KeywordLighter_SetsLight()
        {
            var parser = new CSSFontWeightParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "lighter");

            Assert.IsTrue(result);
            Assert.AreEqual(FontWeights.Light, style.Font.FontWeight);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void FontWeight_KeywordBolder_SetsExtraBold()
        {
            var parser = new CSSFontWeightParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "bolder");

            Assert.IsTrue(result);
            Assert.AreEqual(FontWeights.ExtraBold, style.Font.FontWeight);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void FontWeight_Numeric100_SetsThin()
        {
            var parser = new CSSFontWeightParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "100");

            Assert.IsTrue(result);
            Assert.AreEqual(100, style.Font.FontWeight);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void FontWeight_Numeric400_SetsRegular()
        {
            var parser = new CSSFontWeightParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "400");

            Assert.IsTrue(result);
            Assert.AreEqual(400, style.Font.FontWeight);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void FontWeight_Numeric700_SetsBold()
        {
            var parser = new CSSFontWeightParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "700");

            Assert.IsTrue(result);
            Assert.AreEqual(700, style.Font.FontWeight);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void FontWeight_Numeric900_SetsBlack()
        {
            var parser = new CSSFontWeightParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "900");

            Assert.IsTrue(result);
            Assert.AreEqual(900, style.Font.FontWeight);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void FontWeight_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSFontWeightParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid-weight");

            Assert.IsFalse(result);
        }

        #endregion

        #region Font Style Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void FontStyle_Normal_SetsRegular()
        {
            var parser = new CSSFontStyleParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "normal");

            Assert.IsTrue(result);
            Assert.AreEqual(Scryber.Drawing.FontStyle.Regular, style.Font.FontFaceStyle);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void FontStyle_Italic_SetsItalic()
        {
            var parser = new CSSFontStyleParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "italic");

            Assert.IsTrue(result);
            Assert.AreEqual(Scryber.Drawing.FontStyle.Italic, style.Font.FontFaceStyle);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void FontStyle_Oblique_SetsOblique()
        {
            var parser = new CSSFontStyleParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "oblique");

            Assert.IsTrue(result);
            // Oblique is mapped to Italic in Scryber
            Assert.AreEqual(Scryber.Drawing.FontStyle.Oblique, style.Font.FontFaceStyle);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void FontStyle_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSFontStyleParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid-style");

            Assert.IsFalse(result);
        }

        #endregion

        #region Font Line Height Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void LineHeight_ValidUnit_Pt_SetsHeight()
        {
            var parser = new CSSFontLineHeightParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "18pt");

            Assert.IsTrue(result);
            Assert.AreEqual(18.0, style.Text.Leading.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void LineHeight_ValidUnit_Px_SetsHeight()
        {
            var parser = new CSSFontLineHeightParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "24px");

            Assert.IsTrue(result);
            // 24px = 24 * 0.75 = 18pt
            Assert.AreEqual(18.0, style.Text.Leading.PointsValue, 0.001);
        }
        
        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void LineHeight_NoUnit_SetsHeight_Em()
        {
            var parser = new CSSFontLineHeightParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "1.2");

            Assert.IsTrue(result);
            Assert.AreEqual(1.2, style.Text.Leading.Value, 0.001);
            Assert.AreEqual(PageUnits.EMHeight, style.Text.Leading.Units);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void LineHeight_ZeroValue_SetsZero()
        {
            var parser = new CSSFontLineHeightParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "0");

            Assert.IsTrue(result);
            Assert.AreEqual(0.0, style.Text.Leading.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void LineHeight_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSFontLineHeightParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid-height");

            Assert.IsFalse(result);
        }

        #endregion
    }
}
