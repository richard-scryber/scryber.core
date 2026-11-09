using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.Core.UnitTests.Styles
{
    [TestClass()]
    public class PDFFlexStyleTest
    {
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        // -----------------------------------------------------------------------
        // Constructor
        // -----------------------------------------------------------------------

        [TestMethod()]
        [TestCategory("Style Values")]
        public void Flex_ConstructorTest()
        {
            var target = new FlexStyle();
            Assert.IsNotNull(target);
            Assert.AreEqual(StyleKeys.FlexItemKey, target.ItemKey);
        }

        // -----------------------------------------------------------------------
        // Direction
        // -----------------------------------------------------------------------

        [TestMethod()]
        [TestCategory("Style Values")]
        public void Flex_DirectionTest()
        {
            var target = new FlexStyle();

            Assert.AreEqual(FlexDirection.Row, target.Direction, "Default should be Row");

            target.Direction = FlexDirection.Column;
            Assert.AreEqual(FlexDirection.Column, target.Direction);

            target.Direction = FlexDirection.RowReverse;
            Assert.AreEqual(FlexDirection.RowReverse, target.Direction);

            target.RemoveDirection();
            Assert.AreEqual(FlexDirection.Row, target.Direction, "After remove should revert to default Row");
        }

        // -----------------------------------------------------------------------
        // Wrap
        // -----------------------------------------------------------------------

        [TestMethod()]
        [TestCategory("Style Values")]
        public void Flex_WrapTest()
        {
            var target = new FlexStyle();

            Assert.AreEqual(FlexWrap.Nowrap, target.Wrap, "Default should be Nowrap");

            target.Wrap = FlexWrap.Wrap;
            Assert.AreEqual(FlexWrap.Wrap, target.Wrap);

            target.Wrap = FlexWrap.WrapReverse;
            Assert.AreEqual(FlexWrap.WrapReverse, target.Wrap);

            target.RemoveWrap();
            Assert.AreEqual(FlexWrap.Nowrap, target.Wrap, "After remove should revert to default Nowrap");
        }

        // -----------------------------------------------------------------------
        // JustifyContent
        // -----------------------------------------------------------------------

        [TestMethod()]
        [TestCategory("Style Values")]
        public void Flex_JustifyContentTest()
        {
            var target = new FlexStyle();

            Assert.AreEqual(FlexJustify.FlexStart, target.JustifyContent, "Default should be FlexStart");

            target.JustifyContent = FlexJustify.Center;
            Assert.AreEqual(FlexJustify.Center, target.JustifyContent);

            target.JustifyContent = FlexJustify.SpaceBetween;
            Assert.AreEqual(FlexJustify.SpaceBetween, target.JustifyContent);

            target.JustifyContent = FlexJustify.SpaceAround;
            Assert.AreEqual(FlexJustify.SpaceAround, target.JustifyContent);

            target.JustifyContent = FlexJustify.FlexEnd;
            Assert.AreEqual(FlexJustify.FlexEnd, target.JustifyContent);

            target.RemoveJustifyContent();
            Assert.AreEqual(FlexJustify.FlexStart, target.JustifyContent, "After remove should revert to FlexStart");
        }

        // -----------------------------------------------------------------------
        // AlignItems
        // -----------------------------------------------------------------------

        [TestMethod()]
        [TestCategory("Style Values")]
        public void Flex_AlignItemsTest()
        {
            var target = new FlexStyle();

            Assert.AreEqual(FlexAlignMode.Stretch, target.AlignItems, "Default should be Stretch");

            target.AlignItems = FlexAlignMode.FlexStart;
            Assert.AreEqual(FlexAlignMode.FlexStart, target.AlignItems);

            target.AlignItems = FlexAlignMode.Center;
            Assert.AreEqual(FlexAlignMode.Center, target.AlignItems);

            target.AlignItems = FlexAlignMode.FlexEnd;
            Assert.AreEqual(FlexAlignMode.FlexEnd, target.AlignItems);

            target.AlignItems = FlexAlignMode.Baseline;
            Assert.AreEqual(FlexAlignMode.Baseline, target.AlignItems);

            target.RemoveAlignItems();
            Assert.AreEqual(FlexAlignMode.Stretch, target.AlignItems, "After remove should revert to Stretch");
        }

        // -----------------------------------------------------------------------
        // AlignContent
        // -----------------------------------------------------------------------

        [TestMethod()]
        [TestCategory("Style Values")]
        public void Flex_AlignContentTest()
        {
            var target = new FlexStyle();

            Assert.AreEqual(FlexAlignMode.Stretch, target.AlignContent, "Default should be Stretch");

            target.AlignContent = FlexAlignMode.Center;
            Assert.AreEqual(FlexAlignMode.Center, target.AlignContent);

            target.AlignContent = FlexAlignMode.FlexEnd;
            Assert.AreEqual(FlexAlignMode.FlexEnd, target.AlignContent);

            target.RemoveAlignContent();
            Assert.AreEqual(FlexAlignMode.Stretch, target.AlignContent, "After remove should revert to Stretch");
        }

        // -----------------------------------------------------------------------
        // AlignSelf
        // -----------------------------------------------------------------------

        [TestMethod()]
        [TestCategory("Style Values")]
        public void Flex_AlignSelfTest()
        {
            var target = new FlexStyle();

            Assert.AreEqual(FlexAlignMode.Auto, target.AlignSelf, "Default should be Auto");

            target.AlignSelf = FlexAlignMode.FlexStart;
            Assert.AreEqual(FlexAlignMode.FlexStart, target.AlignSelf);

            target.AlignSelf = FlexAlignMode.Center;
            Assert.AreEqual(FlexAlignMode.Center, target.AlignSelf);

            target.AlignSelf = FlexAlignMode.Stretch;
            Assert.AreEqual(FlexAlignMode.Stretch, target.AlignSelf);

            target.RemoveAlignSelf();
            Assert.AreEqual(FlexAlignMode.Auto, target.AlignSelf, "After remove should revert to Auto");
        }

        // -----------------------------------------------------------------------
        // Gap
        // -----------------------------------------------------------------------

        [TestMethod()]
        [TestCategory("Style Values")]
        public void Flex_GapTest()
        {
            var target = new FlexStyle();

            Assert.AreEqual(Unit.Zero, target.Gap, "Default Gap should be zero");

            target.Gap = new Unit(10, PageUnits.Points);
            Assert.AreEqual(10.0, target.Gap.PointsValue, 0.01);

            target.Gap = new Unit(20, PageUnits.Points);
            Assert.AreEqual(20.0, target.Gap.PointsValue, 0.01);

            target.RemoveGap();
            Assert.AreEqual(Unit.Zero, target.Gap, "After remove Gap should return to zero");
        }

        // -----------------------------------------------------------------------
        // RowGap
        // -----------------------------------------------------------------------

        [TestMethod()]
        [TestCategory("Style Values")]
        public void Flex_RowGapTest()
        {
            var target = new FlexStyle();

            Assert.AreEqual(Unit.Zero, target.RowGap, "Default RowGap should be zero");

            target.RowGap = new Unit(5, PageUnits.Points);
            Assert.AreEqual(5.0, target.RowGap.PointsValue, 0.01);

            target.RemoveRowGap();
            Assert.AreEqual(Unit.Zero, target.RowGap, "After remove RowGap should return to zero");
        }

        // -----------------------------------------------------------------------
        // ColumnGap
        // -----------------------------------------------------------------------

        [TestMethod()]
        [TestCategory("Style Values")]
        public void Flex_ColumnGapTest()
        {
            var target = new FlexStyle();

            Assert.AreEqual(Unit.Zero, target.ColumnGap, "Default ColumnGap should be zero");

            target.ColumnGap = new Unit(15, PageUnits.Points);
            Assert.AreEqual(15.0, target.ColumnGap.PointsValue, 0.01);

            target.RemoveColumnGap();
            Assert.AreEqual(Unit.Zero, target.ColumnGap, "After remove ColumnGap should return to zero");
        }

        // -----------------------------------------------------------------------
        // Grow
        // -----------------------------------------------------------------------

        [TestMethod()]
        [TestCategory("Style Values")]
        public void Flex_GrowTest()
        {
            var target = new FlexStyle();

            Assert.AreEqual(0.0, target.Grow, 0.001, "Default Grow should be 0");

            target.Grow = 1.0;
            Assert.AreEqual(1.0, target.Grow, 0.001);

            target.Grow = 2.5;
            Assert.AreEqual(2.5, target.Grow, 0.001);

            target.RemoveGrow();
            Assert.AreEqual(0.0, target.Grow, 0.001, "After remove Grow should return to 0");
        }

        // -----------------------------------------------------------------------
        // Shrink
        // -----------------------------------------------------------------------

        [TestMethod()]
        [TestCategory("Style Values")]
        public void Flex_ShrinkTest()
        {
            var target = new FlexStyle();

            Assert.AreEqual(1.0, target.Shrink, 0.001, "Default Shrink should be 1");

            target.Shrink = 0.0;
            Assert.AreEqual(0.0, target.Shrink, 0.001);

            target.Shrink = 3.0;
            Assert.AreEqual(3.0, target.Shrink, 0.001);

            target.RemoveShrink();
            Assert.AreEqual(1.0, target.Shrink, 0.001, "After remove Shrink should return to 1");
        }

        // -----------------------------------------------------------------------
        // Basis / BasisAuto
        // -----------------------------------------------------------------------

        [TestMethod()]
        [TestCategory("Style Values")]
        public void Flex_BasisTest()
        {
            var target = new FlexStyle();

            Assert.AreEqual(Unit.Zero, target.Basis, "Default Basis should be zero");
            Assert.IsFalse(target.BasisAuto, "Default BasisAuto should be false");

            target.Basis = new Unit(100, PageUnits.Points);
            Assert.AreEqual(100.0, target.Basis.PointsValue, 0.01);

            target.RemoveBasis();
            Assert.AreEqual(Unit.Zero, target.Basis, "After remove Basis should return to zero");
        }

        [TestMethod()]
        [TestCategory("Style Values")]
        public void Flex_BasisAutoTest()
        {
            var target = new FlexStyle();

            Assert.IsFalse(target.BasisAuto, "Default BasisAuto should be false");

            target.BasisAuto = true;
            Assert.IsTrue(target.BasisAuto);

            target.BasisAuto = false;
            Assert.IsFalse(target.BasisAuto);

            target.RemoveBasisAuto();
            Assert.IsFalse(target.BasisAuto, "After remove BasisAuto should return to false");
        }

        // -----------------------------------------------------------------------
        // Order
        // -----------------------------------------------------------------------

        [TestMethod()]
        [TestCategory("Style Values")]
        public void Flex_OrderTest()
        {
            var target = new FlexStyle();

            Assert.AreEqual(0, target.Order, "Default Order should be 0");

            target.Order = 3;
            Assert.AreEqual(3, target.Order);

            target.Order = -1;
            Assert.AreEqual(-1, target.Order);

            target.RemoveOrder();
            Assert.AreEqual(0, target.Order, "After remove Order should return to 0");
        }

        // -----------------------------------------------------------------------
        // Style.Flex integration — values survive round-trip through Style container
        // -----------------------------------------------------------------------

        [TestMethod()]
        [TestCategory("Style Values")]
        public void Flex_StyleIntegrationTest()
        {
            var style = new Style();

            style.Flex.Direction = FlexDirection.Column;
            style.Flex.Wrap = FlexWrap.Wrap;
            style.Flex.JustifyContent = FlexJustify.Center;
            style.Flex.AlignItems = FlexAlignMode.FlexStart;
            style.Flex.Gap = new Unit(8, PageUnits.Points);
            style.Flex.Grow = 1.0;
            style.Flex.Order = 2;

            Assert.AreEqual(FlexDirection.Column,      style.Flex.Direction);
            Assert.AreEqual(FlexWrap.Wrap,             style.Flex.Wrap);
            Assert.AreEqual(FlexJustify.Center,        style.Flex.JustifyContent);
            Assert.AreEqual(FlexAlignMode.FlexStart,   style.Flex.AlignItems);
            Assert.AreEqual(8.0, style.Flex.Gap.PointsValue, 0.01);
            Assert.AreEqual(1.0, style.Flex.Grow,      0.001);
            Assert.AreEqual(2,   style.Flex.Order);
        }
    }
}
