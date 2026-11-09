using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Drawing;
using Scryber.Styles;
using Scryber.Styles.Parsing;
using Scryber.Styles.Parsing.Typed;

namespace Scryber.Core.UnitTests.Html.CSSParsers
{
    /// <summary>
    /// Tests for CSS global keyword values: initial, inherit, unset, revert.
    ///
    /// Documented behaviour: properties are ignored (parser returns false, style unchanged)
    /// when one of these values is encountered. Two parsers have explicit handling for 'initial'
    /// as a documented exception: CSSOverflowXParser and CSSPageBreakInsideParser.
    /// </summary>
    [TestClass()]
    public class CSSGlobalValues_Tests
    {
        private Style CreateStyle() => new Style();

        private bool ParseValue(CSSStyleValueParser parser, Style style, string cssValue)
        {
            var reader = new CSSStyleItemReader(cssValue);
            return parser.SetStyleValue(style, reader, null);
        }

        // -----------------------------------------------------------------------
        // Section 1: Enum parsers — all four global keywords ignored
        // -----------------------------------------------------------------------

        #region TextAlign — global keywords return false

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-GlobalValues")]
        public void TextAlign_Initial_ReturnsFalse()
        {
            var parser = new CSSTextAlignParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "initial");

            Assert.IsFalse(result);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-GlobalValues")]
        public void TextAlign_Inherit_ReturnsFalse()
        {
            var parser = new CSSTextAlignParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "inherit");

            Assert.IsFalse(result);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-GlobalValues")]
        public void TextAlign_Unset_ReturnsFalse()
        {
            var parser = new CSSTextAlignParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "unset");

            Assert.IsFalse(result);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-GlobalValues")]
        public void TextAlign_Revert_ReturnsFalse()
        {
            var parser = new CSSTextAlignParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "revert");

            Assert.IsFalse(result);
        }

        #endregion

        #region PositionMode — global keywords return false

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-GlobalValues")]
        public void PositionMode_Initial_ReturnsFalse()
        {
            var parser = new CSSPositionModeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "initial");

            Assert.IsFalse(result);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-GlobalValues")]
        public void PositionMode_Inherit_ReturnsFalse()
        {
            var parser = new CSSPositionModeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "inherit");

            Assert.IsFalse(result);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-GlobalValues")]
        public void PositionMode_Unset_ReturnsFalse()
        {
            var parser = new CSSPositionModeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "unset");

            Assert.IsFalse(result);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-GlobalValues")]
        public void PositionMode_Revert_ReturnsFalse()
        {
            var parser = new CSSPositionModeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "revert");

            Assert.IsFalse(result);
        }

        #endregion

        // -----------------------------------------------------------------------
        // Section 2: Unit parsers — all four global keywords ignored
        // -----------------------------------------------------------------------

        #region Margins — global keywords return false

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-GlobalValues")]
        public void Margin_Initial_ReturnsFalse()
        {
            var parser = new CSSMarginsAllParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "initial");

            Assert.IsFalse(result);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-GlobalValues")]
        public void Margin_Inherit_ReturnsFalse()
        {
            var parser = new CSSMarginsAllParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "inherit");

            Assert.IsFalse(result);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-GlobalValues")]
        public void Margin_Unset_ReturnsFalse()
        {
            var parser = new CSSMarginsAllParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "unset");

            Assert.IsFalse(result);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-GlobalValues")]
        public void Margin_Revert_ReturnsFalse()
        {
            var parser = new CSSMarginsAllParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "revert");

            Assert.IsFalse(result);
        }

        #endregion

        // -----------------------------------------------------------------------
        // Section 3: Existing values are preserved when a global keyword fails
        // -----------------------------------------------------------------------

        #region Value preservation

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-GlobalValues")]
        public void TextAlign_InitialAfterCenter_PreservesCenter()
        {
            var parser = new CSSTextAlignParser();
            var style = CreateStyle();

            ParseValue(parser, style, "center");
            Assert.AreEqual(HorizontalAlignment.Center, style.Position.HAlign);

            var result = ParseValue(parser, style, "initial");

            Assert.IsFalse(result);
            Assert.AreEqual(HorizontalAlignment.Center, style.Position.HAlign, "existing text-align value should be unchanged");
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-GlobalValues")]
        public void Margin_InheritAfter20pt_Preserves20pt()
        {
            var parser = new CSSMarginsAllParser();
            var style = CreateStyle();

            ParseValue(parser, style, "20pt");
            Assert.AreEqual(20.0, style.Margins.Top.PointsValue, 0.001);

            var result = ParseValue(parser, style, "inherit");

            Assert.IsFalse(result);
            Assert.AreEqual(20.0, style.Margins.Top.PointsValue, 0.001, "existing margin value should be unchanged");
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-GlobalValues")]
        public void PositionMode_UnsetAfterAbsolute_PreservesAbsolute()
        {
            var parser = new CSSPositionModeParser();
            var style = CreateStyle();

            ParseValue(parser, style, "absolute");
            Assert.AreEqual(PositionMode.Absolute, style.Position.PositionMode);

            var result = ParseValue(parser, style, "unset");

            Assert.IsFalse(result);
            Assert.AreEqual(PositionMode.Absolute, style.Position.PositionMode, "existing position value should be unchanged");
        }

        #endregion

        // -----------------------------------------------------------------------
        // Section 4: overflow-x — 'initial' treated as 'visible' (removes clip)
        //            'inherit', 'unset', 'revert' not handled, return false
        // -----------------------------------------------------------------------

        #region OverflowX special case

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-GlobalValues")]
        public void OverflowX_InitialAfterHidden_RemovesClip()
        {
            var parser = new CSSOverflowXParser();
            var style = CreateStyle();

            ParseValue(parser, style, "hidden");
            Assert.IsTrue(style.IsValueDefined(StyleKeys.ClipLeftKey), "clip should be set after hidden");

            var result = ParseValue(parser, style, "initial");

            Assert.IsTrue(result, "overflow-x: initial should return true");
            Assert.IsFalse(style.IsValueDefined(StyleKeys.ClipLeftKey), "clip should be removed after initial");
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-GlobalValues")]
        public void OverflowX_Inherit_ReturnsFalse()
        {
            var parser = new CSSOverflowXParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "inherit");

            Assert.IsFalse(result);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-GlobalValues")]
        public void OverflowX_Unset_ReturnsFalse()
        {
            var parser = new CSSOverflowXParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "unset");

            Assert.IsFalse(result);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-GlobalValues")]
        public void OverflowX_Revert_ReturnsFalse()
        {
            var parser = new CSSOverflowXParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "revert");

            Assert.IsFalse(result);
        }

        #endregion

        // -----------------------------------------------------------------------
        // Section 5: page-break-inside — 'initial' explicitly maps to Truncate/Never
        //            'inherit', 'unset', 'revert' return false
        // -----------------------------------------------------------------------

        #region PageBreakInside special case

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-GlobalValues")]
        public void PageBreakInside_Initial_SetsTruncateNever()
        {
            // 'initial' maps to Truncate/Never — documented special case in CSSBreakInsideParser
            var parser = new CSSPageBreakInsideParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "initial");

            Assert.IsTrue(result);
            Assert.AreEqual(OverflowSplit.Never, style.Overflow.Split);
            Assert.AreEqual(OverflowAction.Truncate, style.Overflow.Action);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-GlobalValues")]
        public void PageBreakInside_Inherit_ReturnsFalse()
        {
            var parser = new CSSPageBreakInsideParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "inherit");

            Assert.IsFalse(result);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-GlobalValues")]
        public void PageBreakInside_Unset_ReturnsFalse()
        {
            var parser = new CSSPageBreakInsideParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "unset");

            Assert.IsFalse(result);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-GlobalValues")]
        public void PageBreakInside_Revert_ReturnsFalse()
        {
            var parser = new CSSPageBreakInsideParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "revert");

            Assert.IsFalse(result);
        }

        #endregion

        // -----------------------------------------------------------------------
        // Section 6: Full CSS block — ignored global keyword doesn't prevent
        //            subsequent properties in the same rule from being applied
        // -----------------------------------------------------------------------

        #region Full block parsing

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-GlobalValues")]
        public void CSSBlock_InitialBackgroundColor_IgnoredButColorWhiteApplied()
        {
            var css = @".element {
    background-color: initial;
    color: white;
}";
            var parser = new CSSStyleParser(css, null);
            var col = new StyleCollection();
            foreach (var style in parser)
                col.Add(style);

            Assert.AreEqual(1, col.Count, "one rule should be parsed");
            var rule = col[0] as StyleDefn;
            Assert.IsNotNull(rule);

            // background-color: initial was ignored — key should not be set
            Assert.IsFalse(rule.IsValueDefined(StyleKeys.BgColorKey), "background-color: initial should be ignored");

            // color: white should be applied
            Assert.IsTrue(rule.IsValueDefined(StyleKeys.FillColorKey), "color: white should be applied");
            Assert.AreEqual(StandardColors.White, rule.GetValue(StyleKeys.FillColorKey, StandardColors.Transparent));
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-GlobalValues")]
        public void CSSBlock_InheritColor_IgnoredButBackgroundColorApplied()
        {
            // Verify the same: a global keyword first, a real value second — both directions
            var css = @".element {
    color: inherit;
    background-color: #336699;
}";
            var parser = new CSSStyleParser(css, null);
            var col = new StyleCollection();
            foreach (var style in parser)
                col.Add(style);

            Assert.AreEqual(1, col.Count, "one rule should be parsed");
            var rule = col[0] as StyleDefn;
            Assert.IsNotNull(rule);

            // color: inherit was ignored
            Assert.IsFalse(rule.IsValueDefined(StyleKeys.FillColorKey), "color: inherit should be ignored");

            // background-color should be applied
            Assert.IsTrue(rule.IsValueDefined(StyleKeys.BgColorKey), "background-color should be applied");
            Assert.AreEqual((Color)"#336699", rule.GetValue(StyleKeys.BgColorKey, StandardColors.Transparent));
        }

        #endregion
    }
}
