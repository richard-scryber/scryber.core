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
    /// Tests for CSS flex parsers: flex-direction, flex-wrap, justify-content, align-items,
    /// align-content, align-self, gap, row-gap, flex-grow, flex-shrink, flex-basis, order, flex
    /// </summary>
    [TestClass()]
    public class CSSFlexParsers_Tests
    {
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        #region Helper Methods

        private Style CreateStyle() => new Style();

        private bool ParseValue(CSSStyleValueParser parser, Style style, string cssValue)
        {
            var reader = new CSSStyleItemReader(cssValue);
            return parser.SetStyleValue(style, reader, null);
        }

        #endregion

        // -----------------------------------------------------------------------
        // flex-direction
        // -----------------------------------------------------------------------

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Flex")]
        public void FlexDirection_Row()
        {
            var parser = new CSSFlexDirectionParser();
            var style = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "row"));
            Assert.AreEqual(FlexDirection.Row, style.Flex.Direction);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Flex")]
        public void FlexDirection_Column()
        {
            var parser = new CSSFlexDirectionParser();
            var style = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "column"));
            Assert.AreEqual(FlexDirection.Column, style.Flex.Direction);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Flex")]
        public void FlexDirection_RowReverse()
        {
            var parser = new CSSFlexDirectionParser();
            var style = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "row-reverse"));
            Assert.AreEqual(FlexDirection.RowReverse, style.Flex.Direction);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Flex")]
        public void FlexDirection_ColumnReverse()
        {
            var parser = new CSSFlexDirectionParser();
            var style = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "column-reverse"));
            Assert.AreEqual(FlexDirection.ColumnReverse, style.Flex.Direction);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Flex")]
        public void FlexDirection_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSFlexDirectionParser();
            var style = CreateStyle();
            Assert.IsFalse(ParseValue(parser, style, "diagonal"));
        }

        // -----------------------------------------------------------------------
        // flex-wrap
        // -----------------------------------------------------------------------

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Flex")]
        public void FlexWrap_Nowrap()
        {
            var parser = new CSSFlexWrapParser();
            var style = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "nowrap"));
            Assert.AreEqual(FlexWrap.Nowrap, style.Flex.Wrap);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Flex")]
        public void FlexWrap_Wrap()
        {
            var parser = new CSSFlexWrapParser();
            var style = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "wrap"));
            Assert.AreEqual(FlexWrap.Wrap, style.Flex.Wrap);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Flex")]
        public void FlexWrap_WrapReverse()
        {
            var parser = new CSSFlexWrapParser();
            var style = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "wrap-reverse"));
            Assert.AreEqual(FlexWrap.WrapReverse, style.Flex.Wrap);
        }

        // -----------------------------------------------------------------------
        // justify-content
        // -----------------------------------------------------------------------

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Flex")]
        public void JustifyContent_FlexStart()
        {
            var parser = new CSSJustifyContentParser();
            var style = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "flex-start"));
            Assert.AreEqual(FlexJustify.FlexStart, style.Flex.JustifyContent);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Flex")]
        public void JustifyContent_FlexEnd()
        {
            var parser = new CSSJustifyContentParser();
            var style = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "flex-end"));
            Assert.AreEqual(FlexJustify.FlexEnd, style.Flex.JustifyContent);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Flex")]
        public void JustifyContent_Center()
        {
            var parser = new CSSJustifyContentParser();
            var style = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "center"));
            Assert.AreEqual(FlexJustify.Center, style.Flex.JustifyContent);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Flex")]
        public void JustifyContent_SpaceBetween()
        {
            var parser = new CSSJustifyContentParser();
            var style = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "space-between"));
            Assert.AreEqual(FlexJustify.SpaceBetween, style.Flex.JustifyContent);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Flex")]
        public void JustifyContent_SpaceAround()
        {
            var parser = new CSSJustifyContentParser();
            var style = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "space-around"));
            Assert.AreEqual(FlexJustify.SpaceAround, style.Flex.JustifyContent);
        }

        // -----------------------------------------------------------------------
        // align-items / align-self
        // -----------------------------------------------------------------------

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Flex")]
        public void AlignItems_Stretch()
        {
            var parser = new CSSAlignItemsParser();
            var style = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "stretch"));
            Assert.AreEqual(FlexAlignMode.Stretch, style.Flex.AlignItems);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Flex")]
        public void AlignItems_FlexStart()
        {
            var parser = new CSSAlignItemsParser();
            var style = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "flex-start"));
            Assert.AreEqual(FlexAlignMode.FlexStart, style.Flex.AlignItems);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Flex")]
        public void AlignItems_FlexEnd()
        {
            var parser = new CSSAlignItemsParser();
            var style = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "flex-end"));
            Assert.AreEqual(FlexAlignMode.FlexEnd, style.Flex.AlignItems);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Flex")]
        public void AlignItems_Center()
        {
            var parser = new CSSAlignItemsParser();
            var style = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "center"));
            Assert.AreEqual(FlexAlignMode.Center, style.Flex.AlignItems);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Flex")]
        public void AlignItems_Baseline()
        {
            var parser = new CSSAlignItemsParser();
            var style = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "baseline"));
            Assert.AreEqual(FlexAlignMode.Baseline, style.Flex.AlignItems);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Flex")]
        public void AlignSelf_Auto()
        {
            var parser = new CSSAlignSelfParser();
            var style = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "auto"));
            Assert.AreEqual(FlexAlignMode.Auto, style.Flex.AlignSelf);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Flex")]
        public void AlignSelf_FlexStart()
        {
            var parser = new CSSAlignSelfParser();
            var style = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "flex-start"));
            Assert.AreEqual(FlexAlignMode.FlexStart, style.Flex.AlignSelf);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Flex")]
        public void AlignSelf_Center()
        {
            var parser = new CSSAlignSelfParser();
            var style = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "center"));
            Assert.AreEqual(FlexAlignMode.Center, style.Flex.AlignSelf);
        }

        // -----------------------------------------------------------------------
        // flex-grow / flex-shrink
        // -----------------------------------------------------------------------

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Flex")]
        public void FlexGrow_Value()
        {
            var parser = new CSSFlexGrowParser();
            var style = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "2"));
            Assert.AreEqual(2.0, style.Flex.Grow, 0.001);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Flex")]
        public void FlexShrink_Value()
        {
            var parser = new CSSFlexShrinkParser();
            var style = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "3"));
            Assert.AreEqual(3.0, style.Flex.Shrink, 0.001);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Flex")]
        public void FlexGrow_NegativeClampedToZero()
        {
            var parser = new CSSFlexGrowParser();
            var style = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "-1"));
            Assert.AreEqual(0.0, style.Flex.Grow, 0.001);
        }

        // -----------------------------------------------------------------------
        // flex-basis
        // -----------------------------------------------------------------------

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Flex")]
        public void FlexBasis_Value()
        {
            var parser = new CSSFlexBasisParser();
            var style = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "100pt"));
            Assert.AreEqual(false, style.Flex.BasisAuto);
            Assert.AreEqual(100.0, style.Flex.Basis.PointsValue, 0.1);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Flex")]
        public void FlexBasis_Auto()
        {
            var parser = new CSSFlexBasisParser();
            var style = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "auto"));
            Assert.AreEqual(true, style.Flex.BasisAuto);
        }

        // -----------------------------------------------------------------------
        // order
        // -----------------------------------------------------------------------

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Flex")]
        public void Order_PositiveValue()
        {
            var parser = new CSSFlexOrderParser();
            var style = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "3"));
            Assert.AreEqual(3, style.Flex.Order);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Flex")]
        public void Order_NegativeValue()
        {
            var parser = new CSSFlexOrderParser();
            var style = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "-1"));
            Assert.AreEqual(-1, style.Flex.Order);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Flex")]
        public void Order_Zero()
        {
            var parser = new CSSFlexOrderParser();
            var style = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "0"));
            Assert.AreEqual(0, style.Flex.Order);
        }

        // -----------------------------------------------------------------------
        // gap / row-gap
        // -----------------------------------------------------------------------

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Flex")]
        public void Gap_SingleValue()
        {
            var parser = new CSSGapParser();
            var style = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "10pt"));
            Assert.AreEqual(10.0, style.Flex.Gap.PointsValue, 0.1);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Flex")]
        public void RowGap_Value()
        {
            var parser = new CSSRowGapParser();
            var style = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "5pt"));
            Assert.AreEqual(5.0, style.Flex.RowGap.PointsValue, 0.1);
        }

        // -----------------------------------------------------------------------
        // flex shorthand
        // -----------------------------------------------------------------------

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Flex")]
        public void Flex_Shorthand_OneValue_SetsGrow()
        {
            var parser = new CSSFlexShorthandParser();
            var style = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "2"));
            Assert.AreEqual(2.0, style.Flex.Grow, 0.001);
            Assert.AreEqual(1.0, style.Flex.Shrink, 0.001);
            Assert.AreEqual(false, style.Flex.BasisAuto);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Flex")]
        public void Flex_Shorthand_None_SetsNoGrowNoShrinkAutoBasis()
        {
            var parser = new CSSFlexShorthandParser();
            var style = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "none"));
            Assert.AreEqual(0.0, style.Flex.Grow, 0.001);
            Assert.AreEqual(0.0, style.Flex.Shrink, 0.001);
            Assert.AreEqual(true, style.Flex.BasisAuto);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Flex")]
        public void Flex_Shorthand_Auto_SetsGrow1Shrink1AutoBasis()
        {
            var parser = new CSSFlexShorthandParser();
            var style = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "auto"));
            Assert.AreEqual(1.0, style.Flex.Grow, 0.001);
            Assert.AreEqual(1.0, style.Flex.Shrink, 0.001);
            Assert.AreEqual(true, style.Flex.BasisAuto);
        }
    }
}
