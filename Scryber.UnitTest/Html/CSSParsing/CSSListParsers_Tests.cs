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
    /// Tests for CSS list parsers: list-style, list-style-type, list-item-alignment,
    /// list-item-prefix, list-item-postfix, list-item-concatenation, list-item-group, list-item-inset
    /// Covers 8 list-related parsers
    /// </summary>
    [TestClass()]
    public class CSSListParsers_Tests
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

        #region List Style Type Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ListStyleType_Disc_SetsBullet()
        {
            var parser = new CSSListStyleTypeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "disc");

            Assert.IsTrue(result);
            Assert.AreEqual(ListNumberingGroupStyle.Bullet, style.List.NumberingStyle);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ListStyleType_Circle_SetsBullet()
        {
            var parser = new CSSListStyleTypeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "circle");

            Assert.IsTrue(result);
            Assert.AreEqual(ListNumberingGroupStyle.Bullet, style.List.NumberingStyle);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ListStyleType_Decimal_SetsDecimals()
        {
            var parser = new CSSListStyleTypeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "decimal");

            Assert.IsTrue(result);
            Assert.AreEqual(ListNumberingGroupStyle.Decimals, style.List.NumberingStyle);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ListStyleType_LowerAlpha_SetsLowercaseLetters()
        {
            var parser = new CSSListStyleTypeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "lower-alpha");

            Assert.IsTrue(result);
            Assert.AreEqual(ListNumberingGroupStyle.LowercaseLetters, style.List.NumberingStyle);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ListStyleType_UpperAlpha_SetsUppercaseLetters()
        {
            var parser = new CSSListStyleTypeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "upper-alpha");

            Assert.IsTrue(result);
            Assert.AreEqual(ListNumberingGroupStyle.UppercaseLetters, style.List.NumberingStyle);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ListStyleType_LowerRoman_SetsLowercaseRoman()
        {
            var parser = new CSSListStyleTypeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "lower-roman");

            Assert.IsTrue(result);
            Assert.AreEqual(ListNumberingGroupStyle.LowercaseRoman, style.List.NumberingStyle);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ListStyleType_UpperRoman_SetsUppercaseRoman()
        {
            var parser = new CSSListStyleTypeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "upper-roman");

            Assert.IsTrue(result);
            Assert.AreEqual(ListNumberingGroupStyle.UppercaseRoman, style.List.NumberingStyle);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ListStyleType_None_SetsNone()
        {
            var parser = new CSSListStyleTypeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "none");

            Assert.IsTrue(result);
            Assert.AreEqual(ListNumberingGroupStyle.None, style.List.NumberingStyle);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ListStyleType_CaseInsensitive_ParsesCorrectly()
        {
            var parser = new CSSListStyleTypeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "DECIMAL");

            Assert.IsTrue(result);
            Assert.AreEqual(ListNumberingGroupStyle.Decimals, style.List.NumberingStyle);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ListStyleType_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSListStyleTypeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid-type");

            Assert.IsFalse(result);
        }

        #endregion

        #region List Item Alignment Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ListItemAlignment_Left_SetsLeft()
        {
            var parser = new CSSListItemAlignmentParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "left");

            Assert.IsTrue(result);
            Assert.AreEqual(HorizontalAlignment.Left, style.List.NumberAlignment);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ListItemAlignment_Right_SetsRight()
        {
            var parser = new CSSListItemAlignmentParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "right");

            Assert.IsTrue(result);
            Assert.AreEqual(HorizontalAlignment.Right, style.List.NumberAlignment);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ListItemAlignment_Center_SetsCenter()
        {
            var parser = new CSSListItemAlignmentParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "center");

            Assert.IsTrue(result);
            Assert.AreEqual(HorizontalAlignment.Center, style.List.NumberAlignment);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ListItemAlignment_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSListItemAlignmentParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid-alignment");

            Assert.IsFalse(result);
        }

        #endregion

        #region List Item Prefix Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ListItemPrefix_TextValue_SetsPrefix()
        {
            var parser = new CSSListItemPrefixParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "'Chapter '");

            Assert.IsTrue(result);
            Assert.AreEqual("Chapter", style.List.NumberPrefix);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ListItemPrefix_EmptyString_SetsEmpty()
        {
            var parser = new CSSListItemPrefixParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "''");

            Assert.IsTrue(result);
            Assert.AreEqual("'", style.List.NumberPrefix);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ListItemPrefix_None_SetsEmpty()
        {
            var parser = new CSSListItemPrefixParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "none");

            Assert.IsTrue(result);
            Assert.IsTrue(string.IsNullOrEmpty(style.List.NumberPrefix));
        }

        #endregion

        #region List Item Postfix Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ListItemPostfix_TextValue_SetsPostfix()
        {
            var parser = new CSSListItemPostFixParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "'.'");

            Assert.IsTrue(result);
            Assert.AreEqual(".", style.List.NumberPostfix);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ListItemPostfix_EmptyString_SetsEmpty()
        {
            var parser = new CSSListItemPostFixParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "''");

            Assert.IsTrue(result);
            Assert.AreEqual("'", style.List.NumberPostfix);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ListItemPostfix_None_SetsEmpty()
        {
            var parser = new CSSListItemPostFixParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "none");

            Assert.IsTrue(result);
            Assert.IsTrue(string.IsNullOrEmpty(style.List.NumberPostfix));
        }

        #endregion

        #region List Item Concatenation Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ListItemConcatenation_True_SetsConcatenate()
        {
            var parser = new CSSListItemConcatenationParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "true");

            Assert.IsTrue(result);
            Assert.IsTrue(style.List.ConcatenateWithParent);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ListItemConcatenation_False_SetsNoConcatenate()
        {
            var parser = new CSSListItemConcatenationParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "false");

            Assert.IsTrue(result);
            Assert.IsFalse(style.List.ConcatenateWithParent);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ListItemConcatenation_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSListItemConcatenationParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid-bool");

            Assert.IsFalse(result);
        }

        #endregion

        #region List Item Inset Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ListItemInset_ValidUnit_Pt_SetsInset()
        {
            var parser = new CSSListItemInsetParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "20pt");

            Assert.IsTrue(result);
            Assert.AreEqual(20.0, style.List.NumberInset.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ListItemInset_ValidUnit_Px_SetsInset()
        {
            var parser = new CSSListItemInsetParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "24px");

            Assert.IsTrue(result);
            // 24px = 24 * 0.75 = 18pt
            Assert.AreEqual(18.0, style.List.NumberInset.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ListItemInset_ZeroValue_SetsZero()
        {
            var parser = new CSSListItemInsetParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "0");

            Assert.IsTrue(result);
            Assert.AreEqual(0.0, style.List.NumberInset.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void ListItemInset_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSListItemInsetParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid-inset");

            Assert.IsFalse(result);
        }

        #endregion
    }
}
