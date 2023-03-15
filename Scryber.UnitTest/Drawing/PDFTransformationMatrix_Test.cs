using Scryber.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Scryber.Core.UnitTests.Drawing
{
    
    
    /// <summary>
    ///This is a test class for PDFTransformationMatrix_Test and is intended
    ///to contain all PDFTransformationMatrix_Test Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFTransformationMatrix_Test
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
                throw new AssertFailedException("X value of pt1 (" + x1 + "," + y1 + ") not equal to pt2 (" + x2 + "," + y2 + ") - " + message);
            if (y1 != y2)
                throw new AssertFailedException("Y value of pt1 (" + x1 + "," + y1 + ") not equal to pt2 (" + x2 + "," + y2 + ") - " + message);
        }


        /// <summary>
        ///A test for PDFTransformationMatrix Constructor
        ///</summary>
        [TestMethod()]
        public void PDFTransformationMatrixConstructor_Test()
        {
            PDFTransformationMatrix target = new PDFTransformationMatrix();
            Assert.IsTrue(target.IsIdentity, "Empty matrix is not identity");
        }

        /// <summary>
        ///A test for PDFTransformationMatrix Constructor
        ///</summary>
        [TestMethod()]
        public void PDFTransformationMatrixConstructor_Test1()
        {
            float offsetX = 5F; // Set translation X
            float offsetY = 20F; // Set Translation Y
            float angle = 0.5F; // Set rotation angle radians
            float scaleX = 2.0F; // Set the x scale
            float scaleY = 4.0F; //Set the y scale
            PDFTransformationMatrix target = new PDFTransformationMatrix(offsetX, offsetY, angle, scaleX, scaleY);
            Assert.IsFalse(target.IsIdentity, "Explicit matrix should not be identity");

            PDFTransformationMatrix exact = new PDFTransformationMatrix();
            exact.SetTranslation(5F, 20F);
            exact.SetRotation(0.5F);
            exact.SetScale(2.0F, 4.0F);

            var actual = target.Components;
            var expected = exact.Components;

            for(var i = 0; i < 6; i ++)
            {
                Assert.AreEqual(Math.Round(expected[i], 4), Math.Round(actual[i], 4), "Values " + i + " did not match " + expected[i] + " vs " + actual[i]);
            }
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

            AssertPointsAreEqual(new Point(-1, 0), ptA1, "Point A not transformed");
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


            
        }
    }
}
