using Scryber.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Scryber.Core.UnitTests.Drawing
{
    [TestClass]
    public class PDFColumnWidth_Test
    {

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        public PDFColumnWidth_Test()
        {
        }

        [TestMethod]
        public void ColumnWidthConstructors()
        {
            var widths = new ColumnWidths();
            Assert.IsTrue(widths.IsEmpty);
            Assert.IsFalse(widths.HasExplicitWidth);
            Assert.AreEqual(PDFUnit.Empty, widths.Explicit);
            Assert.IsNull(widths.Widths);
        }

        [TestMethod]
        public void ColumnWidthWidthsContrsuctors()
        {
            var widths = new ColumnWidths(new double[] { 0.3, 0.4, 0.3 });
            Assert.IsFalse(widths.IsEmpty);
            Assert.IsFalse(widths.HasExplicitWidth);
            Assert.AreEqual(PDFUnit.Empty, widths.Explicit);

            Assert.IsNotNull(widths.Widths);
            Assert.AreEqual(3, widths.Widths.Length);
            Assert.AreEqual(0.3, widths.Widths[0]);
            Assert.AreEqual(0.4, widths.Widths[1]);
            Assert.AreEqual(0.3, widths.Widths[2]);
        }

        [TestMethod]
        public void ColumnWidthExplicitContrsuctors()
        {
            var widths = new ColumnWidths(new PDFUnit(200, PageUnits.Points));
            Assert.IsFalse(widths.IsEmpty);
            Assert.IsTrue(widths.HasExplicitWidth);

            Assert.IsNull(widths.Widths);
            Assert.AreEqual(new PDFUnit(200, PageUnits.Points), widths.Explicit);
        }

        [TestMethod]
        public void ColumnWidthWidthsParse()
        {
            var str = "0.3 0.3 0.4";
            var widths = ColumnWidths.Parse(str);

            Assert.IsFalse(widths.IsEmpty);
            Assert.IsFalse(widths.HasExplicitWidth);
            Assert.AreEqual(PDFUnit.Empty, widths.Explicit);

            Assert.IsNotNull(widths.Widths);
            Assert.AreEqual(3, widths.Widths.Length);
            Assert.AreEqual(0.3, widths.Widths[0]);
            Assert.AreEqual(0.3, widths.Widths[1]);
            Assert.AreEqual(0.4, widths.Widths[2]);

        }

        [TestMethod]
        public void ColumnWidthPercentParse()
        {
            var str = "40% 30% 30%";
            var widths = ColumnWidths.Parse(str);

            Assert.IsFalse(widths.IsEmpty);
            Assert.IsFalse(widths.HasExplicitWidth);
            Assert.AreEqual(PDFUnit.Empty, widths.Explicit);

            Assert.IsNotNull(widths.Widths);
            Assert.AreEqual(3, widths.Widths.Length);
            Assert.AreEqual(0.4, widths.Widths[0]);
            Assert.AreEqual(0.3, widths.Widths[1]);
            Assert.AreEqual(0.3, widths.Widths[2]);
        }

        [TestMethod]
        public void ColumnWidthExplicitParse()
        {
            var str = "300pt";
            var widths = ColumnWidths.Parse(str);
            Assert.IsFalse(widths.IsEmpty);
            Assert.IsTrue(widths.HasExplicitWidth);

            Assert.IsNull(widths.Widths);
            Assert.AreEqual(new PDFUnit(300, PageUnits.Points), widths.Explicit);
        }


        [TestMethod]
        public void ColumnWidthWithVariableParse()
        {
            var str = "40% * 30%";
            var widths = ColumnWidths.Parse(str);

            Assert.IsFalse(widths.IsEmpty);
            Assert.IsFalse(widths.HasExplicitWidth);
            Assert.AreEqual(PDFUnit.Empty, widths.Explicit);

            Assert.IsNotNull(widths.Widths);
            Assert.AreEqual(3, widths.Widths.Length);
            Assert.AreEqual(0.4, widths.Widths[0]);
            Assert.AreEqual(ColumnWidths.UndefinedWidth, widths.Widths[1]);
            Assert.AreEqual(0.3, widths.Widths[2]);
        }


        [TestMethod]
        public void ColumnWidthExplicitCalculate()
        {
            var str = "300pt";
            var widths = ColumnWidths.Parse(str);
            Assert.IsFalse(widths.IsEmpty);
            Assert.IsTrue(widths.HasExplicitWidth);

            int count;
            var calc = widths.GetExplicitColumnWidths(950, 10, out count);

            Assert.AreEqual(3, count);
            Assert.AreEqual(310, (int)calc[0].PointsValue);
            Assert.AreEqual(310, (int)calc[1].PointsValue);
            Assert.AreEqual(310, (int)calc[2].PointsValue);

            calc = widths.GetExplicitColumnWidths(750, 10, out count);

            Assert.AreEqual(2, count);

            // 370 + 10 + 370 = 750
            Assert.AreEqual(370, (int)calc[0].PointsValue);
            Assert.AreEqual(370, (int)calc[1].PointsValue);

            calc = widths.GetExplicitColumnWidths(250, 10, out count);
            //Smaller than the desired size so it's just 1
            Assert.AreEqual(1, count);
            Assert.AreEqual(250, (int)calc[0].PointsValue);

            calc = widths.GetExplicitColumnWidths(550, 10, out count);
            //Smaller than the available size for 2 so it's just 1
            Assert.AreEqual(1, count);
            Assert.AreEqual(550, (int)calc[0].PointsValue);


            calc = widths.GetExplicitColumnWidths(1890, 10, out count);
            //Just a big number
            Assert.AreEqual(6, count);

            Assert.AreEqual(306, (int)calc[0].PointsValue);
            Assert.AreEqual(306, (int)calc[1].PointsValue);
            Assert.AreEqual(306, (int)calc[2].PointsValue);

            // Reverse check to make sure the values add up to the orginal
            // with the alley being 1 less than the column count.

            var max = (10 * (count-1)) + (calc[0] * count);
            Assert.AreEqual((int)max.PointsValue, 1890);
        }


        [TestMethod]
        public void ColumnWidthWithVariableCaclulate()
        {
            var str = "40% * 30%";
            var widths = ColumnWidths.Parse(str);


            var calc = widths.GetPercentColumnWidths(1030, 10, 4);
            Assert.AreEqual(4, calc.Length);
            Assert.AreEqual(400, calc[0].PointsValue);
            Assert.AreEqual(300, calc[2].PointsValue);

            Assert.AreEqual(150, calc[1].PointsValue);
            Assert.AreEqual(150, calc[3].PointsValue);


            calc = widths.GetPercentColumnWidths(1020, 10, 3);
            Assert.AreEqual(3, calc.Length);
            Assert.AreEqual(400, calc[0].PointsValue);
            Assert.AreEqual(300, calc[2].PointsValue);

            Assert.AreEqual(300, calc[1].PointsValue);

            calc = widths.GetPercentColumnWidths(1010, 10, 2);
            Assert.AreEqual(2, calc.Length);
            Assert.AreEqual(400, calc[0].PointsValue);

            Assert.AreEqual(600, calc[1].PointsValue);

            str = "0.2 0.5 * 0.1";
            widths = ColumnWidths.Parse(str);

            calc = widths.GetPercentColumnWidths(1030, 10, 4);
            Assert.AreEqual(4, calc.Length);
            Assert.AreEqual(200, calc[0].PointsValue);
            Assert.AreEqual(500, calc[1].PointsValue);
            Assert.AreEqual(200, calc[2].PointsValue);
            Assert.AreEqual(100, calc[3].PointsValue);
        }

    }
}
