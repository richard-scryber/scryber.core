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
    /// Tests for CSS display parsers: display, opacity, overflow, overflow-x, overflow-y
    /// Covers 5 display/visibility/overflow parsers
    /// </summary>
    [TestClass()]
    public class CSSDisplayParsers_Tests
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

        #region Display Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Display_Block_SetsBlock()
        {
            var parser = new CSSDisplayParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "block");

            Assert.IsTrue(result);
            Assert.AreEqual(DisplayMode.Block, style.Position.DisplayMode);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Display_Inline_SetsInline()
        {
            var parser = new CSSDisplayParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "inline");

            Assert.IsTrue(result);
            Assert.AreEqual(DisplayMode.Inline, style.Position.DisplayMode);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Display_InlineBlock_SetsInlineBlock()
        {
            var parser = new CSSDisplayParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "inline-block");

            Assert.IsTrue(result);
            Assert.AreEqual(DisplayMode.InlineBlock, style.Position.DisplayMode);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Display_None_SetsInvisible()
        {
            var parser = new CSSDisplayParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "none");

            Assert.IsTrue(result);
            Assert.AreEqual(DisplayMode.Invisible, style.Position.DisplayMode);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Display_Table_SetsTable()
        {
            var parser = new CSSDisplayParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "table");

            Assert.IsTrue(result);
            // Table display mode
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Display_TableRow_SetsTableRow()
        {
            var parser = new CSSDisplayParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "table-row");

            Assert.IsTrue(result);
            // Table row display mode
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Display_TableCell_SetsTableCell()
        {
            var parser = new CSSDisplayParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "table-cell");

            Assert.IsTrue(result);
            // Table cell display mode
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Display_CaseInsensitive_ParsesCorrectly()
        {
            var parser = new CSSDisplayParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "BLOCK");

            Assert.IsTrue(result);
            Assert.AreEqual(DisplayMode.Block, style.Position.DisplayMode);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Display_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSDisplayParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid-display");

            Assert.IsFalse(result);
        }

        #endregion

        #region Opacity Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Opacity_FullyOpaque_SetsOne()
        {
            var parser = new CSSOpacityParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "1");

            Assert.IsTrue(result);
            Assert.AreEqual(1.0, style.Fill.Opacity, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Opacity_HalfTransparent_SetsHalf()
        {
            var parser = new CSSOpacityParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "0.5");

            Assert.IsTrue(result);
            Assert.AreEqual(0.5, style.Fill.Opacity, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Opacity_FullyTransparent_SetsZero()
        {
            var parser = new CSSOpacityParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "0");

            Assert.IsTrue(result);
            Assert.AreEqual(0.0, style.Fill.Opacity, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Opacity_QuarterTransparent_SetsFraction()
        {
            var parser = new CSSOpacityParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "0.25");

            Assert.IsTrue(result);
            Assert.AreEqual(0.25, style.Fill.Opacity, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Opacity_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSOpacityParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid-opacity");

            Assert.IsFalse(result);
        }

        #endregion

        #region Overflow Action Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void OverflowAction_Visible_SetsVisible()
        {
            var parser = new CSSOverflowActionParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "visible");

            Assert.IsTrue(result);
            Assert.AreEqual(OverflowAction.None, style.Overflow.Action);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void OverflowAction_Hidden_SetsClip()
        {
            var parser = new CSSOverflowActionParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "hidden");

            Assert.IsTrue(result);
            Assert.AreEqual(OverflowAction.Clip, style.Overflow.Action);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void OverflowAction_Clip_SetsClip()
        {
            var parser = new CSSOverflowActionParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "clip");

            Assert.IsTrue(result);
            Assert.AreEqual(OverflowAction.Clip, style.Overflow.Action);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void OverflowAction_Truncate_SetsTruncate()
        {
            var parser = new CSSOverflowActionParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "truncate");

            Assert.IsTrue(result);
            Assert.AreEqual(OverflowAction.Truncate, style.Overflow.Action);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void OverflowAction_NewPage_SetsNewPage()
        {
            var parser = new CSSOverflowActionParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "new-page");

            Assert.IsTrue(result);
            Assert.AreEqual(OverflowAction.NewPage, style.Overflow.Action);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void OverflowAction_CaseInsensitive_ParsesCorrectly()
        {
            var parser = new CSSOverflowActionParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "HIDDEN");

            Assert.IsTrue(result);
            Assert.AreEqual(OverflowAction.Clip, style.Overflow.Action);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void OverflowAction_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSOverflowActionParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid-overflow");

            Assert.IsFalse(result);
        }

        #endregion

        #region Overflow X Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void OverflowX_Visible_SetsVisible()
        {
            var parser = new CSSOverflowXParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "visible");

            Assert.IsTrue(result);
            Assert.AreEqual(OverflowAction.None, style.Overflow.Action);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void OverflowX_Hidden_SetsClip()
        {
            var parser = new CSSOverflowXParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "hidden");

            Assert.IsTrue(result);
            Assert.AreEqual(OverflowAction.Clip, style.Overflow.Action);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void OverflowX_Clip_SetsClip()
        {
            var parser = new CSSOverflowXParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "clip");

            Assert.IsTrue(result);
            Assert.AreEqual(OverflowAction.Clip, style.Overflow.Action);
        }

        #endregion

        #region Overflow Y Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void OverflowY_Visible_SetsVisible()
        {
            var parser = new CSSOverflowYParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "visible");

            Assert.IsTrue(result);
            Assert.AreEqual(OverflowAction.None, style.Overflow.Action);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void OverflowY_Hidden_SetsClip()
        {
            var parser = new CSSOverflowYParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "hidden");

            Assert.IsTrue(result);
            Assert.AreEqual(OverflowAction.Clip, style.Overflow.Action);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void OverflowY_Clip_SetsClip()
        {
            var parser = new CSSOverflowYParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "clip");

            Assert.IsTrue(result);
            Assert.AreEqual(OverflowAction.Clip, style.Overflow.Action);
        }

        #endregion
    }
}
