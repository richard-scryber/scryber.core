using Scryber.Styles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Scryber.Core.UnitTests.Styles
{
    //TODO: Transform Styles
    
    
    /// <summary>
    ///This is a test class for PDFTransformStyleTest and is intended
    ///to contain all PDFTransformStyleTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFTransformStyleTest
    {

#if USETRANSFORM

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

#region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
#endregion


        /// <summary>
        ///A test for PDFTransformStyle Constructor
        ///</summary>
        [TestMethod()]
        public void PDFTransformStyleConstructorTest()
        {
            PDFTransformStyle target = new PDFTransformStyle();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for RemoveOffsetH
        ///</summary>
        [TestMethod()]
        public void RemoveOffsetHTest()
        {
            PDFTransformStyle target = new PDFTransformStyle(); // TODO: Initialize to an appropriate value
            target.RemoveOffsetH();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for RemoveOffsetV
        ///</summary>
        [TestMethod()]
        public void RemoveOffsetVTest()
        {
            PDFTransformStyle target = new PDFTransformStyle(); // TODO: Initialize to an appropriate value
            target.RemoveOffsetV();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for RemoveRotate
        ///</summary>
        [TestMethod()]
        public void RemoveRotateTest()
        {
            PDFTransformStyle target = new PDFTransformStyle(); // TODO: Initialize to an appropriate value
            target.RemoveRotate();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for RemoveScaleX
        ///</summary>
        [TestMethod()]
        public void RemoveScaleXTest()
        {
            PDFTransformStyle target = new PDFTransformStyle(); // TODO: Initialize to an appropriate value
            target.RemoveScaleX();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for RemoveScaleY
        ///</summary>
        [TestMethod()]
        public void RemoveScaleYTest()
        {
            PDFTransformStyle target = new PDFTransformStyle(); // TODO: Initialize to an appropriate value
            target.RemoveScaleY();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for RemoveSkewX
        ///</summary>
        [TestMethod()]
        public void RemoveSkewXTest()
        {
            PDFTransformStyle target = new PDFTransformStyle(); // TODO: Initialize to an appropriate value
            target.RemoveSkewX();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for RemoveSkewY
        ///</summary>
        [TestMethod()]
        public void RemoveSkewYTest()
        {
            PDFTransformStyle target = new PDFTransformStyle(); // TODO: Initialize to an appropriate value
            target.RemoveSkewY();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for OffsetH
        ///</summary>
        [TestMethod()]
        public void OffsetHTest()
        {
            PDFTransformStyle target = new PDFTransformStyle(); // TODO: Initialize to an appropriate value
            float expected = 0F; // TODO: Initialize to an appropriate value
            float actual;
            target.OffsetH = expected;
            actual = target.OffsetH;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for OffsetV
        ///</summary>
        [TestMethod()]
        public void OffsetVTest()
        {
            PDFTransformStyle target = new PDFTransformStyle(); // TODO: Initialize to an appropriate value
            float expected = 0F; // TODO: Initialize to an appropriate value
            float actual;
            target.OffsetV = expected;
            actual = target.OffsetV;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Rotate
        ///</summary>
        [TestMethod()]
        public void RotateTest()
        {
            PDFTransformStyle target = new PDFTransformStyle(); // TODO: Initialize to an appropriate value
            float expected = 20F; // TODO: Initialize to an appropriate value
            float actual;
            target.Rotate = expected;
            actual = target.Rotate;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ScaleX
        ///</summary>
        [TestMethod()]
        public void ScaleXTest()
        {
            
            PDFTransformStyle target = new PDFTransformStyle(); // TODO: Initialize to an appropriate value
            float expected = 20F; // TODO: Initialize to an appropriate value
            float actual;
            target.ScaleX = expected;
            actual = target.ScaleX;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ScaleY
        ///</summary>
        [TestMethod()]
        public void ScaleYTest()
        {
            PDFTransformStyle target = new PDFTransformStyle(); // TODO: Initialize to an appropriate value
            float expected = 0F; // TODO: Initialize to an appropriate value
            float actual;
            target.ScaleY = expected;
            actual = target.ScaleY;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for SkewX
        ///</summary>
        [TestMethod()]
        public void SkewXTest()
        {
            PDFTransformStyle target = new PDFTransformStyle(); // TODO: Initialize to an appropriate value
            float expected = 0F; // TODO: Initialize to an appropriate value
            float actual;
            target.SkewX = expected;
            actual = target.SkewX;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for SkewY
        ///</summary>
        [TestMethod()]
        public void SkewYTest()
        {
            PDFTransformStyle target = new PDFTransformStyle(); // TODO: Initialize to an appropriate value
            float expected = 0F; // TODO: Initialize to an appropriate value
            float actual;
            target.SkewY = expected;
            actual = target.SkewY;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
#endif

    }

}
