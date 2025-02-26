using Scryber.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber;
using Scryber.Components;
using Scryber.Svg;
using Scryber.Svg.Components;
using Scryber.Html.Components;
using System.CodeDom;
using System.Xml.Linq;
using Scryber.PDF.Graphics;
using Scryber.PDF.Native;
using Scryber.PDF.Resources;
using Scryber.Svg.Layout;

namespace Scryber.Core.UnitTests.Svg
{
    
    
    /// <summary>
    ///This is a test class for PDFColor_Test and is intended
    ///to contain all PDFColor_Test Unit Tests
    ///</summary>
    [TestClass()]
    public class SVGLinearGradientAngle_Test
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

        [TestMethod()]
        public void SVGLinearGradientCoord_00_45Deg_Origin_Test()
        {
            var rect = new Rect(0, 0, 10, 10);
            var angle = 45.0;

            Point tl;
            Point br;
            
            var result = SVGLinearGradientCalculator.CalculateOptimumCoords(rect, angle, out tl, out br);

            Assert.IsTrue(result);
            Assert.AreEqual(0, tl.X);
            Assert.AreEqual(0, tl.Y);
            Assert.AreEqual(10, br.X);
            Assert.AreEqual(10, br.Y);


        }
        
        [TestMethod()]
        public void SVGLinearGradientCoord_01_45Deg_5ptY_Test()
        {
            var rect = new Rect(0, 5, 10, 10);
            var angle = 45.0;

            Point tl;
            Point br;
            
            var result = SVGLinearGradientCalculator.CalculateOptimumCoords(rect, angle, out tl, out br);

            Assert.IsTrue(result);
            Assert.AreEqual(0, tl.X);
            Assert.AreEqual(5, tl.Y);
            Assert.AreEqual(10, br.X);
            Assert.AreEqual(15, br.Y);


        }
        
        [TestMethod()]
        public void SVGLinearGradientCoord_02_45Deg_5ptX_Test()
        {
            var rect = new Rect(5, 0, 10, 10);
            var angle = 45.0;

            Point tl;
            Point br;
            
            var result = SVGLinearGradientCalculator.CalculateOptimumCoords(rect, angle, out tl, out br);

            Assert.IsTrue(result);
            Assert.AreEqual(5, tl.X);
            Assert.AreEqual(0, tl.Y);
            Assert.AreEqual(15, br.X);
            Assert.AreEqual(10, br.Y);


        }
        
        [TestMethod()]
        public void SVGLinearGradientCoord_03_25Deg_Origin_Test()
        {
            var rect = new Rect(0, 0, 10, 10);
            var angle = 22.5;

            Point tl;
            Point br;
            
            var result = SVGLinearGradientCalculator.CalculateOptimumCoords(rect, angle, out tl, out br);

            Assert.IsTrue(result);
            Assert.AreEqual(0, tl.X);
            Assert.AreEqual(0, tl.Y);
            Assert.AreEqual(12, Math.Round(br.X.PointsValue));
            Assert.AreEqual(5, Math.Round(br.Y.PointsValue));


        }
        
        [TestMethod()]
        public void SVGLinearGradientCoord_04_25Deg_5ptX5ptY_Test()
        {
            var rect = new Rect(5, 5, 10, 10);
            var angle = 22.5;

            Point tl;
            Point br;
            
            var result = SVGLinearGradientCalculator.CalculateOptimumCoords(rect, angle, out tl, out br);

            Assert.IsTrue(result);
            Assert.AreEqual(5, tl.X);
            Assert.AreEqual(5, tl.Y);
            Assert.AreEqual(17.1, Math.Round(br.X.PointsValue, 1));
            Assert.AreEqual(10, Math.Round(br.Y.PointsValue, 1));


        }
        
        [TestMethod()]
        public void SVGLinearGradientCoord_05_0Deg_Origin_Test()
        {
            var rect = new Rect(0, 0, 10, 10);
            var angle = 0;

            Point tl;
            Point br;
            
            var result = SVGLinearGradientCalculator.CalculateOptimumCoords(rect, angle, out tl, out br);

            Assert.IsTrue(result);
            Assert.AreEqual(0, tl.X);
            Assert.AreEqual(0, tl.Y);
            Assert.AreEqual(10, br.X.PointsValue);
            Assert.AreEqual(0, br.Y.PointsValue);


        }
        
        [TestMethod()]
        public void SVGLinearGradientCoord_06_0Deg_5ptX5ptY_Test()
        {
            var rect = new Rect(5, 5, 10, 10);
            var angle = 0;

            Point tl;
            Point br;
            
            var result = SVGLinearGradientCalculator.CalculateOptimumCoords(rect, angle, out tl, out br);

            Assert.IsTrue(result);
            Assert.AreEqual(5, tl.X);
            Assert.AreEqual(5, tl.Y);
            Assert.AreEqual(15, br.X.PointsValue);
            Assert.AreEqual(5, br.Y.PointsValue);


        }
        
        [TestMethod()]
        public void SVGLinearGradientCoord_07_0Deg_M5ptX5ptY_Test()
        {
            var rect = new Rect(-5, 5, 10, 10);
            var angle = 0;

            Point tl;
            Point br;
            
            var result = SVGLinearGradientCalculator.CalculateOptimumCoords(rect, angle, out tl, out br);

            Assert.IsTrue(result);
            Assert.AreEqual(-5, tl.X);
            Assert.AreEqual(5, tl.Y);
            Assert.AreEqual(5, br.X.PointsValue);
            Assert.AreEqual(5, br.Y.PointsValue);


        }
        
        [TestMethod()]
        public void SVGLinearGradientCoord_08_25Deg_M5xM5y_Test()
        {
            var rect = new Rect(-5, -5, 10, 10);
            var angle = 22.5;

            Point tl;
            Point br;
            
            var result = SVGLinearGradientCalculator.CalculateOptimumCoords(rect, angle, out tl, out br);

            Assert.IsTrue(result);
            Assert.AreEqual(-5, tl.X);
            Assert.AreEqual(-5, tl.Y);
            Assert.AreEqual(7, Math.Round(br.X.PointsValue));
            Assert.AreEqual(0, Math.Round(br.Y.PointsValue));
            
        }
        
        [TestMethod()]
        public void SVGLinearGradientCoord_09_90Deg_Origin_Test()
        {
            var rect = new Rect(0, 0, 10, 10);
            var angle = 90;

            Point tl;
            Point br;
            
            var result = SVGLinearGradientCalculator.CalculateOptimumCoords(rect, angle, out tl, out br);

            Assert.IsTrue(result);
            Assert.AreEqual(0, tl.X);
            Assert.AreEqual(0, tl.Y);
            Assert.AreEqual(0, Math.Round(br.X.PointsValue));
            Assert.AreEqual(10, Math.Round(br.Y.PointsValue));
            
        }
        
        
        [TestMethod()]
        public void SVGLinearGradientCoord_10_90Deg_5x5y_Test()
        {
            var rect = new Rect(5, 5, 10, 10);
            var angle = 90;

            Point tl;
            Point br;
            
            var result = SVGLinearGradientCalculator.CalculateOptimumCoords(rect, angle, out tl, out br);

            Assert.IsTrue(result);
            Assert.AreEqual(5, tl.X);
            Assert.AreEqual(5, tl.Y);
            Assert.AreEqual(5, Math.Round(br.X.PointsValue));
            Assert.AreEqual(15, Math.Round(br.Y.PointsValue));
            
        }
        
        [TestMethod()]
        public void SVGLinearGradientCoord_11_90Deg_m5xm5y_Test()
        {
            var rect = new Rect(-5, -5, 10, 10);
            var angle = 90;

            Point tl;
            Point br;
            
            var result = SVGLinearGradientCalculator.CalculateOptimumCoords(rect, angle, out tl, out br);

            Assert.IsTrue(result);
            Assert.AreEqual(-5, tl.X);
            Assert.AreEqual(-5, tl.Y);
            Assert.AreEqual(-5, Math.Round(br.X.PointsValue));
            Assert.AreEqual(5, Math.Round(br.Y.PointsValue));
            
        }
        
        
        
        [TestMethod()]
        public void SVGLinearGradientCoord_12_135Deg_Origin_Test()
        {
            var rect = new Rect(0, 0, 10, 10);
            var angle = 135;

            Point pt1;
            Point pt2;
            
            var result = SVGLinearGradientCalculator.CalculateOptimumCoords(rect, angle, out pt1, out pt2);

            //135 = From bottom right to top left corners
            Assert.IsTrue(result);
            Assert.AreEqual(10, pt1.X);
            Assert.AreEqual(0, pt1.Y);
            Assert.AreEqual(0, pt2.X.PointsValue);
            Assert.AreEqual(10, pt2.Y.PointsValue);
            
        }
        
        [TestMethod()]
        public void SVGLinearGradientCoord_13_135Deg_5x0y_Test()
        {
            var rect = new Rect(5, 0, 10, 10);
            var angle = 135;

            Point pt1;
            Point pt2;
            
            var result = SVGLinearGradientCalculator.CalculateOptimumCoords(rect, angle, out pt1, out pt2);

            //135 = From bottom right to top left corners
            Assert.IsTrue(result);
            Assert.AreEqual(15, pt1.X);
            Assert.AreEqual(0, pt1.Y);
            Assert.AreEqual(5, pt2.X.PointsValue);
            Assert.AreEqual(10, pt2.Y.PointsValue);
            
        }
        
        [TestMethod()]
        public void SVGLinearGradientCoord_15_135Deg_M5xM10y_Test()
        {
            var rect = new Rect(-5,-10, 10, 10);
            var angle = 135;

            Point pt1;
            Point pt2;
            
            var result = SVGLinearGradientCalculator.CalculateOptimumCoords(rect, angle, out pt1, out pt2);

            //135 = From bottom right to top left corners
            Assert.IsTrue(result);
            Assert.AreEqual(5, pt1.X);
            Assert.AreEqual(-10, pt1.Y);
            Assert.AreEqual(-5, pt2.X.PointsValue);
            Assert.AreEqual(0, pt2.Y.PointsValue);
            
        }
        
        [TestMethod()]
        public void SVGLinearGradientCoord_16_115Deg_Origin_Test()
        {
            var rect = new Rect(0,0, 10, 10);
            var angle = 115;

            Point pt1;
            Point pt2;
            
            var result = SVGLinearGradientCalculator.CalculateOptimumCoords(rect, angle, out pt1, out pt2);

            //115 = From bottom right to mid top left
            Assert.IsTrue(result);
            Assert.AreEqual(10, pt1.X);
            Assert.AreEqual(0, pt1.Y);
            Assert.AreEqual(4.4, Math.Round(pt2.X.PointsValue, 1));
            Assert.AreEqual(12.0, Math.Round(pt2.Y.PointsValue, 1));
            
        }
        
        
        [TestMethod()]
        public void SVGLinearGradientCoord_17_115Deg_5x10y_Test()
        {
            var rect = new Rect(5, 10, 10, 10);
            var angle = 115;

            Point pt1;
            Point pt2;
            
            var result = SVGLinearGradientCalculator.CalculateOptimumCoords(rect, angle, out pt1, out pt2);

            //115 = From bottom right to mid top left 
            Assert.IsTrue(result);
            Assert.AreEqual(15, pt1.X);
            Assert.AreEqual(10, pt1.Y);
            Assert.AreEqual(9.4, Math.Round(pt2.X.PointsValue, 1));
            Assert.AreEqual(22.0, Math.Round(pt2.Y.PointsValue, 1));
            
        }
        
        [TestMethod()]
        public void SVGLinearGradientCoord_18_155Deg_Origin_Test()
        {
            var rect = new Rect(0, 0, 10, 10);
            var angle = 155;

            Point pt1;
            Point pt2;
            
            var result = SVGLinearGradientCalculator.CalculateOptimumCoords(rect, angle, out pt1, out pt2);

            //155 = From top right to mid left 
            Assert.IsTrue(result);
            Assert.AreEqual(10, pt1.X);
            Assert.AreEqual(0, pt1.Y);
            Assert.AreEqual(-2.0, Math.Round(pt2.X.PointsValue, 1));
            Assert.AreEqual(5.6, Math.Round(pt2.Y.PointsValue, 1));
            
        }
        
        [TestMethod()]
        public void SVGLinearGradientCoord_19_180Deg_Origin_Test()
        {
            var rect = new Rect(0, 0, 10, 10);
            var angle = 180;

            Point tl;
            Point br;
            
            var result = SVGLinearGradientCalculator.CalculateOptimumCoords(rect, angle, out tl, out br);

            Assert.IsTrue(result);
            Assert.AreEqual(10, tl.X);
            Assert.AreEqual(0, tl.Y);
            Assert.AreEqual(0, Math.Round(br.X.PointsValue));
            Assert.AreEqual(0, Math.Round(br.Y.PointsValue));
            
        }
        
        
        [TestMethod()]
        public void SVGLinearGradientCoord_20_180Deg_5x5y_Test()
        {
            var rect = new Rect(5, 5, 10, 10);
            var angle = 180;

            Point tl;
            Point br;
            
            var result = SVGLinearGradientCalculator.CalculateOptimumCoords(rect, angle, out tl, out br);

            Assert.IsTrue(result);
            Assert.AreEqual(15, tl.X);
            Assert.AreEqual(5, tl.Y);
            Assert.AreEqual(5, Math.Round(br.X.PointsValue));
            Assert.AreEqual(5, Math.Round(br.Y.PointsValue));
            
        }
        
        [TestMethod()]
        public void SVGLinearGradientCoord_21_180Deg_m5xm5y_Test()
        {
            var rect = new Rect(-5, -5, 10, 10);
            var angle = 180;

            Point tl;
            Point br;
            
            var result = SVGLinearGradientCalculator.CalculateOptimumCoords(rect, angle, out tl, out br);

            Assert.IsTrue(result);
            Assert.AreEqual(5, tl.X);
            Assert.AreEqual(-5, tl.Y);
            Assert.AreEqual(-5, Math.Round(br.X.PointsValue));
            Assert.AreEqual(-5, Math.Round(br.Y.PointsValue));
            
        }
        
        [TestMethod()]
        public void SVGLinearGradientCoord_22_225Deg_Origin_Test()
        {
            var rect = new Rect(0, 0, 10, 10);
            var angle = 225;

            Point pt1;
            Point pt2;
            
            var result = SVGLinearGradientCalculator.CalculateOptimumCoords(rect, angle, out pt1, out pt2);

            //225 = From top right to bottom left corners
            Assert.IsTrue(result);
            Assert.AreEqual(10, pt1.X);
            Assert.AreEqual(10, pt1.Y);
            Assert.AreEqual(0, pt2.X.PointsValue);
            Assert.AreEqual(0, pt2.Y.PointsValue);
            
        }
        
        [TestMethod()]
        public void SVGLinearGradientCoord_23_225Deg_5x0y_Test()
        {
            var rect = new Rect(5, 0, 10, 10);
            var angle = 225;

            Point pt1;
            Point pt2;
            
            var result = SVGLinearGradientCalculator.CalculateOptimumCoords(rect, angle, out pt1, out pt2);

            //225 = From top right to bottom left corners
            Assert.IsTrue(result);
            Assert.AreEqual(15, pt1.X);
            Assert.AreEqual(10, pt1.Y);
            Assert.AreEqual(5, pt2.X.PointsValue);
            Assert.AreEqual(0, pt2.Y.PointsValue);
            
        }
        
        [TestMethod()]
        public void SVGLinearGradientCoord_24_225Deg_M5xM10y_Test()
        {
            var rect = new Rect(-5,-10, 10, 10);
            var angle = 225;

            Point pt1;
            Point pt2;
            
            var result = SVGLinearGradientCalculator.CalculateOptimumCoords(rect, angle, out pt1, out pt2);

            //225 = From top right to bottom left corners
            Assert.IsTrue(result);
            Assert.AreEqual(5, pt1.X);
            Assert.AreEqual(0, pt1.Y);
            Assert.AreEqual(-5, pt2.X.PointsValue);
            Assert.AreEqual(-10, pt2.Y.PointsValue);
            
        }
        
        
        [TestMethod()]
        public void SVGLinearGradientCoord_26_205Deg_Origin_Test()
        {
            var rect = new Rect(0,0, 10, 10);
            var angle = 205;

            Point pt1;
            Point pt2;
            
            var result = SVGLinearGradientCalculator.CalculateOptimumCoords(rect, angle, out pt1, out pt2);

            //115 = From top right to mid left
            Assert.IsTrue(result);
            Assert.AreEqual(10, pt1.X);
            Assert.AreEqual(10, pt1.Y);
            Assert.AreEqual(-2.0, Math.Round(pt2.X.PointsValue, 1));
            Assert.AreEqual(4.4, Math.Round(pt2.Y.PointsValue, 1));
            
        }
        
        
        [TestMethod()]
        public void SVGLinearGradientCoord_27_205Deg_5x10y_Test()
        {
            var rect = new Rect(5, 10, 10, 10);
            var angle = 205;

            Point pt1;
            Point pt2;
            
            var result = SVGLinearGradientCalculator.CalculateOptimumCoords(rect, angle, out pt1, out pt2);

            //115 = From top right to mid left 
            Assert.IsTrue(result);
            Assert.AreEqual(15, pt1.X);
            Assert.AreEqual(20, pt1.Y);
            Assert.AreEqual(3.0, Math.Round(pt2.X.PointsValue, 1));
            Assert.AreEqual(14.4, Math.Round(pt2.Y.PointsValue, 1));
            
        }
        
        [TestMethod()]
        public void SVGLinearGradientCoord_28_245Deg_Origin_Test()
        {
            var rect = new Rect(0, 0, 10, 10);
            var angle = 245;

            Point pt1;
            Point pt2;
            
            var result = SVGLinearGradientCalculator.CalculateOptimumCoords(rect, angle, out pt1, out pt2);

            //155 = From top right to mid left 
            Assert.IsTrue(result);
            Assert.AreEqual(10, pt1.X);
            Assert.AreEqual(10, pt1.Y);
            Assert.AreEqual(4.4, Math.Round(pt2.X.PointsValue, 1));
            Assert.AreEqual(-2.0, Math.Round(pt2.Y.PointsValue, 1));
            
        }

        [TestMethod()]
        public void SVGLinearGradientCoord_29_270Deg_Origin_Test()
        {
            var rect = new Rect(0, 0, 10, 10);
            var angle = 270;

            Point tl;
            Point br;
            
            var result = SVGLinearGradientCalculator.CalculateOptimumCoords(rect, angle, out tl, out br);

            Assert.IsTrue(result);
            Assert.AreEqual(0, tl.X);
            Assert.AreEqual(10, tl.Y);
            Assert.AreEqual(0, Math.Round(br.X.PointsValue));
            Assert.AreEqual(0, Math.Round(br.Y.PointsValue));
            
        }
        
        
        [TestMethod()]
        public void SVGLinearGradientCoord_30_270Deg_5x5y_Test()
        {
            var rect = new Rect(5, 5, 10, 10);
            var angle = 270;

            Point tl;
            Point br;
            
            var result = SVGLinearGradientCalculator.CalculateOptimumCoords(rect, angle, out tl, out br);

            Assert.IsTrue(result);
            Assert.AreEqual(5, tl.X);
            Assert.AreEqual(15, tl.Y);
            Assert.AreEqual(5, Math.Round(br.X.PointsValue));
            Assert.AreEqual(5, Math.Round(br.Y.PointsValue));
            
        }
        
        [TestMethod()]
        public void SVGLinearGradientCoord_31_270Deg_m5xm5y_Test()
        {
            var rect = new Rect(-5, -5, 10, 10);
            var angle = 270;

            Point tl;
            Point br;
            
            var result = SVGLinearGradientCalculator.CalculateOptimumCoords(rect, angle, out tl, out br);

            Assert.IsTrue(result);
            Assert.AreEqual(-5, tl.X);
            Assert.AreEqual(5, tl.Y);
            Assert.AreEqual(-5, Math.Round(br.X.PointsValue));
            Assert.AreEqual(-5, Math.Round(br.Y.PointsValue));
            
        }
        
        
        [TestMethod()]
        public void SVGLinearGradientCoord_32_315Deg_Origin_Test()
        {
            var rect = new Rect(0, 0, 10, 10);
            var angle = 315;

            Point pt1;
            Point pt2;
            
            var result = SVGLinearGradientCalculator.CalculateOptimumCoords(rect, angle, out pt1, out pt2);

            //315 = From top left to bottom right corners
            Assert.IsTrue(result);
            Assert.AreEqual(0, pt1.X);
            Assert.AreEqual(10, pt1.Y);
            Assert.AreEqual(10, pt2.X.PointsValue);
            Assert.AreEqual(0, pt2.Y.PointsValue);
            
        }
        
        [TestMethod()]
        public void SVGLinearGradientCoord_33_315Deg_5x0y_Test()
        {
            var rect = new Rect(5, 0, 10, 10);
            var angle = 315;

            Point pt1;
            Point pt2;
            
            var result = SVGLinearGradientCalculator.CalculateOptimumCoords(rect, angle, out pt1, out pt2);

            //315 = From top left to bottom right corners
            Assert.IsTrue(result);
            Assert.AreEqual(5, pt1.X);
            Assert.AreEqual(10, pt1.Y);
            Assert.AreEqual(15, pt2.X.PointsValue);
            Assert.AreEqual(0, pt2.Y.PointsValue);
            
        }
        
        [TestMethod()]
        public void SVGLinearGradientCoord_34_315Deg_M5xM10y_Test()
        {
            var rect = new Rect(-5,-10, 10, 10);
            var angle = 315;

            Point pt1;
            Point pt2;
            
            var result = SVGLinearGradientCalculator.CalculateOptimumCoords(rect, angle, out pt1, out pt2);

            //315 = From top left to bottom right corners
            Assert.IsTrue(result);
            Assert.AreEqual(-5, pt1.X);
            Assert.AreEqual(0, pt1.Y);
            Assert.AreEqual(5, pt2.X.PointsValue);
            Assert.AreEqual(-10, pt2.Y.PointsValue);
            
        }
        
        
        [TestMethod()]
        public void SVGLinearGradientCoord_36_285Deg_Origin_Test()
        {
            var rect = new Rect(0, 0, 10, 10);
            var angle = 285;

            Point pt1;
            Point pt2;
            
            var result = SVGLinearGradientCalculator.CalculateOptimumCoords(rect, angle, out pt1, out pt2);

            //285 = From top left to mid bottom
            Assert.IsTrue(result);
            Assert.AreEqual(0, pt1.X);
            Assert.AreEqual(10, pt1.Y);
            Assert.AreEqual(3.2, Math.Round(pt2.X.PointsValue, 1));
            Assert.AreEqual(-1.8, Math.Round(pt2.Y.PointsValue, 1));

        }
        
        
        [TestMethod()]
        public void SVGLinearGradientCoord_37_285Deg_5x10y_Test()
        {
            var rect = new Rect(5, 10, 10, 10);
            var angle = 285;

            Point pt1;
            Point pt2;
            
            var result = SVGLinearGradientCalculator.CalculateOptimumCoords(rect, angle, out pt1, out pt2);

            //285 = From top right to mid bottom 
            Assert.IsTrue(result);
            Assert.AreEqual(5, pt1.X);
            Assert.AreEqual(20, pt1.Y);
            Assert.AreEqual(8.2, Math.Round(pt2.X.PointsValue, 1));
            Assert.AreEqual(8.2, Math.Round(pt2.Y.PointsValue, 1));
            
        }
        
        [TestMethod()]
        public void SVGLinearGradientCoord_38_335Deg_Origin_Test()
        {
            var rect = new Rect(0, 0, 10, 10);
            var angle = 335;

            Point pt1;
            Point pt2;
            
            var result = SVGLinearGradientCalculator.CalculateOptimumCoords(rect, angle, out pt1, out pt2);

            //155 = From top left to mid right 
            Assert.IsTrue(result);
            Assert.AreEqual(0, pt1.X);
            Assert.AreEqual(10, pt1.Y);
            Assert.AreEqual(12.0, Math.Round(pt2.X.PointsValue, 1));
            Assert.AreEqual(4.4, Math.Round(pt2.Y.PointsValue, 1));
            
        }
        
        [TestMethod()]
        public void SVGLinearGradientCoord_39_360Deg_Origin_Test()
        {
            var rect = new Rect(0, 0, 10, 10);
            var angle = 360;

            Point tl;
            Point br;
            
            var result = SVGLinearGradientCalculator.CalculateOptimumCoords(rect, angle, out tl, out br);

            Assert.IsTrue(result);
            Assert.AreEqual(0, tl.X);
            Assert.AreEqual(0, tl.Y);
            Assert.AreEqual(10, Math.Round(br.X.PointsValue));
            Assert.AreEqual(0, Math.Round(br.Y.PointsValue));
            
        }
        
        
        [TestMethod()]
        public void SVGLinearGradientCoord_40_360Deg_5x5y_Test()
        {
            var rect = new Rect(5, 5, 10, 10);
            var angle = 360;

            Point tl;
            Point br;
            
            var result = SVGLinearGradientCalculator.CalculateOptimumCoords(rect, angle, out tl, out br);

            Assert.IsTrue(result);
            Assert.AreEqual(5, tl.X);
            Assert.AreEqual(5, tl.Y);
            Assert.AreEqual(15, Math.Round(br.X.PointsValue));
            Assert.AreEqual(5, Math.Round(br.Y.PointsValue));
            
        }
        
        [TestMethod()]
        public void SVGLinearGradientCoord_41_360Deg_m5xm5y_Test()
        {
            var rect = new Rect(-5, -5, 10, 10);
            var angle = 360;

            Point tl;
            Point br;
            
            var result = SVGLinearGradientCalculator.CalculateOptimumCoords(rect, angle, out tl, out br);

            Assert.IsTrue(result);
            Assert.AreEqual(-5, tl.X);
            Assert.AreEqual(-5, tl.Y);
            Assert.AreEqual(5, Math.Round(br.X.PointsValue));
            Assert.AreEqual(-5, Math.Round(br.Y.PointsValue));
            
        }
        
        
        [TestMethod()]
        public void SVGLinearGradientCoord_42_675Deg_5x0y_Test()
        {
            var rect = new Rect(5, 0, 10, 10);
            var angle = 315 + 360;

            Point pt1;
            Point pt2;
            
            var result = SVGLinearGradientCalculator.CalculateOptimumCoords(rect, angle, out pt1, out pt2);

            //675 = 315 = From top left to bottom right corners
            Assert.IsTrue(result);
            Assert.AreEqual(5, pt1.X);
            Assert.AreEqual(10, pt1.Y);
            Assert.AreEqual(15, pt2.X.PointsValue);
            Assert.AreEqual(0, pt2.Y.PointsValue);
            
        }
        
        [TestMethod()]
        public void SVGLinearGradientCoord_43_M45Deg_M5xM10y_Test()
        {
            var rect = new Rect(-5,-10, 10, 10);
            var angle = 315 - 360;

            Point pt1;
            Point pt2;
            
            var result = SVGLinearGradientCalculator.CalculateOptimumCoords(rect, angle, out pt1, out pt2);

            //315 = From top left to bottom right corners
            Assert.IsTrue(result);
            Assert.AreEqual(-5, pt1.X);
            Assert.AreEqual(0, pt1.Y);
            Assert.AreEqual(5, pt2.X.PointsValue);
            Assert.AreEqual(-10, pt2.Y.PointsValue);
            
        }

        
        [TestMethod()]
        public void SVGLinearGradientCoord_44_45Deg_Wide_Test()
        {
            var rect = new Rect(0, 0, 30, 10);
            var angle = 45;

            Point pt1;
            Point pt2;
            
            var result = SVGLinearGradientCalculator.CalculateOptimumCoords(rect, angle, out pt1, out pt2);

            //315 = From top left to bottom right corners
            Assert.IsTrue(result);
            Assert.AreEqual(0, pt1.X);
            Assert.AreEqual(0, pt1.Y);
            Assert.AreEqual(20, pt2.X.PointsValue);
            Assert.AreEqual(20, pt2.Y.PointsValue);
            
        }
        
        [TestMethod()]
        public void SVGLinearGradientCoord_45_45Deg_Tall_Test()
        {
            var rect = new Rect(0, 0, 10, 40);
            var angle = 45;

            Point pt1;
            Point pt2;
            
            var result = SVGLinearGradientCalculator.CalculateOptimumCoords(rect, angle, out pt1, out pt2);

            //315 = From top left to bottom right corners
            Assert.IsTrue(result);
            Assert.AreEqual(0, pt1.X);
            Assert.AreEqual(0, pt1.Y);
            Assert.AreEqual(25, pt2.X.PointsValue);
            Assert.AreEqual(25, pt2.Y.PointsValue);
            
        }

    }
}
