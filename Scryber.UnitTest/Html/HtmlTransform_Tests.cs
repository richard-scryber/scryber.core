using System;
using System.IO;
using Scryber.Drawing;
using Scryber.Components;
using Scryber.PDF.Resources;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.PDF;


namespace Scryber.Core.UnitTests.Html
{
    [TestClass]
    public class HtmlTransform_Tests
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

        const int RoundingPrecision = 4;

        private void AssertPointsAreEqual(Point p1, Point pt2, string message)
        {
            var x1 = Math.Round(p1.X.PointsValue, RoundingPrecision);
            var y1 = Math.Round(p1.Y.PointsValue, RoundingPrecision);
            var x2 = Math.Round(pt2.X.PointsValue, RoundingPrecision);
            var y2 = Math.Round(pt2.Y.PointsValue, RoundingPrecision);

            if (x1 != x2)
                throw new AssertFailedException( "X value of pt1 (" + x1 + "," + y1 + ") not equal to pt2 (" + x2 + "," + y2 + ") - " + message);
            if (y1 != y2)
                throw new AssertFailedException("Y value of pt1 (" + x1 + "," + y1 + ") not equal to pt2 (" + x2 + "," + y2 + ") - " + message);
        }

        [TestMethod]
        public void Transform_MatrixRotate_Test()
        {

            var matrix = new PDFTransformationMatrix();

            //90 degrees with single unit size origin rect.
            matrix.SetRotationDeg(90);

            var ptA = new Point(0, 1);
            var ptB = new Point(1, 1);
            var ptC = new Point(1, 0);
            var ptD = new Point(0, 0);

            var ptA1 = matrix.TransformPoint(ptA);
            var ptB1 = matrix.TransformPoint(ptB);
            var ptC1 = matrix.TransformPoint(ptC);
            var ptD1 = matrix.TransformPoint(ptD);

            AssertPointsAreEqual(new Point(-1, 0), ptA1 , "Point A not transformed");
            AssertPointsAreEqual(new Point(-1, 1), ptB1, "Point B not transformed");
            AssertPointsAreEqual(new Point(0, 1), ptC1, "Point C not transformed");
            AssertPointsAreEqual(new Point(0, 0), ptD1, "Point D not transformed");


            matrix = new PDFTransformationMatrix();

            //45 degrees with a set of points offset by 10
            matrix.SetRotationDeg(45);

            ptA = new Point(10, 20);
            ptB = new Point(20, 20);
            ptC = new Point(20, 10);
            ptD = new Point(10, 10);

            ptA1 = matrix.TransformPoint(ptA);
            ptB1 = matrix.TransformPoint(ptB);
            ptC1 = matrix.TransformPoint(ptC);
            ptD1 = matrix.TransformPoint(ptD);

            AssertPointsAreEqual(new Point(-7.0711, 21.2132), ptA1, "Point A not transformed");
            AssertPointsAreEqual(new Point(0.0000, 28.2843), ptB1, "Point B not transformed");
            AssertPointsAreEqual(new Point(7.0711, 21.2132), ptC1, "Point C not transformed");
            AssertPointsAreEqual(new Point(0.0000, 14.1421), ptD1, "Point D not transformed");
        }

        [TestMethod]
        public void Transform_MatrixTranslate_Test()
        {

            var matrix = new PDFTransformationMatrix();

            matrix.SetTranslation(20, 10);

            var ptA = new Point(0, 1);
            var ptB = new Point(1, 1);
            var ptC = new Point(1, 0);
            var ptD = new Point(0, 0);

            var ptA1 = matrix.TransformPoint(ptA);
            var ptB1 = matrix.TransformPoint(ptB);
            var ptC1 = matrix.TransformPoint(ptC);
            var ptD1 = matrix.TransformPoint(ptD);

            AssertPointsAreEqual(new Point(20, 11), ptA1, "Point A not transformed");
            AssertPointsAreEqual(new Point(21, 11), ptB1, "Point B not transformed");
            AssertPointsAreEqual(new Point(21, 10), ptC1, "Point C not transformed");
            AssertPointsAreEqual(new Point(20, 10), ptD1, "Point D not transformed");


            matrix = new PDFTransformationMatrix();
            matrix.SetTranslation(14, 10);

            ptA = new Point(7, 9);
            ptB = new Point(9, 9);
            ptC = new Point(9, 7);
            ptD = new Point(7, 7);

            ptA1 = matrix.TransformPoint(ptA);
            ptB1 = matrix.TransformPoint(ptB);
            ptC1 = matrix.TransformPoint(ptC);
            ptD1 = matrix.TransformPoint(ptD);

            AssertPointsAreEqual(new Point(21, 19), ptA1, "Point A not transformed");
            AssertPointsAreEqual(new Point(23, 19), ptB1, "Point B not transformed");
            AssertPointsAreEqual(new Point(23, 17), ptC1, "Point C not transformed");
            AssertPointsAreEqual(new Point(21, 17), ptD1, "Point D not transformed");
        }


        [TestMethod]
        public void Transform_MatrixSkew_Test()
        {

            var matrix = new PDFTransformationMatrix();

            matrix.SetSkew(2, 0);

            var ptA = new Point(0, 1);
            var ptB = new Point(1, 1);
            var ptC = new Point(1, 0);
            var ptD = new Point(0, 0);

            var ptA1 = matrix.TransformPoint(ptA);
            var ptB1 = matrix.TransformPoint(ptB);
            var ptC1 = matrix.TransformPoint(ptC);
            var ptD1 = matrix.TransformPoint(ptD);

            AssertPointsAreEqual(new Point(2, 1), ptA1, "Point A not transformed");
            AssertPointsAreEqual(new Point(3, 1), ptB1, "Point B not transformed");
            AssertPointsAreEqual(new Point(1, 0), ptC1, "Point C not transformed");
            AssertPointsAreEqual(new Point(0, 0), ptD1, "Point D not transformed");


            matrix = new PDFTransformationMatrix();
            matrix.SetSkew(9, 6);

            ptA = new Point(7, 9);
            ptB = new Point(9, 9);
            ptC = new Point(9, 7);
            ptD = new Point(7, 7);

            ptA1 = matrix.TransformPoint(ptA);
            ptB1 = matrix.TransformPoint(ptB);
            ptC1 = matrix.TransformPoint(ptC);
            ptD1 = matrix.TransformPoint(ptD);

            AssertPointsAreEqual(new Point(88, 51), ptA1, "Point A not transformed");
            AssertPointsAreEqual(new Point(90, 63), ptB1, "Point B not transformed");
            AssertPointsAreEqual(new Point(72, 61), ptC1, "Point C not transformed");
            AssertPointsAreEqual(new Point(70, 49), ptD1, "Point D not transformed");
        }

        [TestMethod]
        public void Transform_MatrixScale_Test()
        {

            var matrix = new PDFTransformationMatrix();

            matrix.SetScale(2, 3);

            var ptA = new Point(0, 1);
            var ptB = new Point(1, 1);
            var ptC = new Point(1, 0);
            var ptD = new Point(0, 0);

            var ptA1 = matrix.TransformPoint(ptA);
            var ptB1 = matrix.TransformPoint(ptB);
            var ptC1 = matrix.TransformPoint(ptC);
            var ptD1 = matrix.TransformPoint(ptD);

            AssertPointsAreEqual(new Point(0, 3), ptA1, "Point A not transformed");
            AssertPointsAreEqual(new Point(2, 3), ptB1, "Point B not transformed");
            AssertPointsAreEqual(new Point(2, 0), ptC1, "Point C not transformed");
            AssertPointsAreEqual(new Point(0, 0), ptD1, "Point D not transformed");


            matrix = new PDFTransformationMatrix();
            matrix.SetScale(3, 1);

            ptA = new Point(7, 9);
            ptB = new Point(9, 9);
            ptC = new Point(9, 7);
            ptD = new Point(7, 7);

            ptA1 = matrix.TransformPoint(ptA);
            ptB1 = matrix.TransformPoint(ptB);
            ptC1 = matrix.TransformPoint(ptC);
            ptD1 = matrix.TransformPoint(ptD);

            AssertPointsAreEqual(new Point(21, 9), ptA1, "Point A not transformed");
            AssertPointsAreEqual(new Point(27, 9), ptB1, "Point B not transformed");
            AssertPointsAreEqual(new Point(27, 7), ptC1, "Point C not transformed");
            AssertPointsAreEqual(new Point(21, 7), ptD1, "Point D not transformed");
        }

        [TestMethod]
        public void Transform_MatrixMultiple_Test()
        {

            var matrix = new PDFTransformationMatrix();

            matrix.SetTranslation(4, 2);
            matrix.SetRotationDeg(45);
            matrix.SetSkew(4, 2);

            var ptA = new Point(0, 1);
            var ptB = new Point(1, 1);
            var ptC = new Point(1, 0);
            var ptD = new Point(0, 0);

            var ptA1 = matrix.TransformPoint(ptA);
            var ptB1 = matrix.TransformPoint(ptB);
            var ptC1 = matrix.TransformPoint(ptC);
            var ptD1 = matrix.TransformPoint(ptD);

            AssertPointsAreEqual(new Point(6.1213, 5.5355), ptA1, "Point A not transformed");
            AssertPointsAreEqual(new Point(5.4142, 7.6569), ptB1, "Point B not transformed");
            AssertPointsAreEqual(new Point(3.2929, 4.1213), ptC1, "Point C not transformed");
            AssertPointsAreEqual(new Point(4.0000, 2.0000), ptD1, "Point D not transformed");


            //matrix = new PDFTransformationMatrix();
            //matrix.SetTranslation(14, 10);

            //ptA = new Point(7, 9);
            //ptB = new Point(9, 9);
            //ptC = new Point(9, 7);
            //ptD = new Point(7, 7);

            //ptA1 = matrix.TransformPoint(ptA);
            //ptB1 = matrix.TransformPoint(ptB);
            //ptC1 = matrix.TransformPoint(ptC);
            //ptD1 = matrix.TransformPoint(ptD);

            //AssertPointsAreEqual(new Point(21, 19), ptA1, "Point A not transformed");
            //AssertPointsAreEqual(new Point(23, 19), ptB1, "Point B not transformed");
            //AssertPointsAreEqual(new Point(23, 17), ptC1, "Point C not transformed");
            //AssertPointsAreEqual(new Point(21, 17), ptD1, "Point D not transformed");
        }



        /// <summary>
        /// An image in the content with a full file path
        /// </summary>
        [TestMethod]
        public void Transform_SingleRotate_Test()
        {
            
            var html = @"<?scryber append-log='true' log-level='Diagnostic' parser-log='true' ?>
<html xmlns='http://www.w3.org/1999/xhtml' >
<body style='padding:20pt;' >
    <div style='width: 100pt; left: 0pt; top: 0pt; background-color:#ddd; height:200pt; position: absolute; border: solid 2px red;' >Content of the div</div>
    <div style='width: 100pt; left: 0pt; top: 0pt; background-color:#ddd; height:200pt; transform: rotate(-10deg); position: absolute; border: solid 4px blue;' >Content of the div <div style='top: 70pt;height:30pt; background-color:blue'></div> </div>
    <div style='width: 100pt; left: 0pt; top: 0pt; background-color:#ddd; height:200pt; transform: rotate(-20deg); position: absolute; border: solid 2px red;' >Content of the div <div style='height:30pt; background-color:blue'></div> </div>
    <div style='width: 100pt; left: 0pt; top: 0pt; background-color:#ddd; height:200pt; transform: rotate(-30deg); position: absolute; border: solid 2px red;' >Content of the div<div style='height:30pt; background-color:blue'></div> </div>
    <div style='width: 100pt; left: 0pt; top: 0pt; background-color:#ddd; height:200pt; transform: rotate(-40deg); position: absolute; border: solid 2px red;' >Content of the div<div style='height:30pt; background-color:blue'></div> </div>
    <div style='width: 100pt; left: 0pt; top: 0pt; background-color:#ddd; height:200pt; transform: rotate(-50deg); position: absolute; border: solid 2px red;' >Content of the div<div style='height:30pt; background-color:blue'></div> </div>
    <div style='width: 100pt; left: 0pt; top: 0pt; background-color:#ddd; height:200pt; transform: rotate(-60deg); position: absolute; border: solid 2px red;' >Content of the div<div style='height:30pt; background-color:blue'></div> </div>
    <div style='width: 100pt; left: 0pt; top: 0pt; background-color:#ddd; height:200pt; transform: rotate(-70deg); position: absolute; border: solid 2px red;' >Content of the div<div style='height:30pt; background-color:blue'></div> </div>
</body>
</html>";

            using (var sr = new System.IO.StringReader(html))
            {
                using (var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent))
                {
                    doc.RenderOptions.Compression = OutputCompressionType.None;
                    using (var stream = DocStreams.GetOutputStream("Transform_SingleRotate.pdf"))
                    {
                        doc.SaveAsPDF(stream);
                    }

                }
            }
        }
        
        
        
        
    }
}