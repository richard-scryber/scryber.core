using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber.Styles;

namespace Scryber.Core.UnitTests.Styles
{
    
    
    /// <summary>
    ///This is a test class for PDFShapeStyle and is intended
    ///to contain all PDFShapeStyle Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFShapeStyleTest
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
        ///A test for PDFShapeStyle Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Shape_ConstructorTest()
        {
            PDFShapeStyle target = new PDFShapeStyle();
            Assert.IsNotNull(target);
            Assert.AreEqual(PDFStyleKeys.ShapeItemKey, target.ItemKey);
        }

        
        /// <summary>
        ///A test for VertextCount
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Shape_VertextCountTest()
        {
            PDFShapeStyle target = new PDFShapeStyle(); 
            Assert.AreEqual(4, target.VertexCount);

            target.VertexCount = 14;
            Assert.AreEqual(14, target.VertexCount);

            target.RemoveVertexCount();
            Assert.AreEqual(4, target.VertexCount);
        }

        /// <summary>
        ///A test for VertextStep
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Shape_VertextStepTest()
        {
            PDFShapeStyle target = new PDFShapeStyle();
            Assert.AreEqual(1, target.VertexStep);

            target.VertexStep = 2;
            Assert.AreEqual(2, target.VertexStep);

            target.RemoveVertexStep();
            Assert.AreEqual(1, target.VertexStep);
        }

        /// <summary>
        ///A test for Closed
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Shape_ClosedTest()
        {
            PDFShapeStyle target = new PDFShapeStyle();
            Assert.AreEqual(true, target.Closed, "Default");

            target.Closed = false;
            Assert.AreEqual(false, target.Closed, "Set Value");

            target.RemoveClosed();
            Assert.AreEqual(true, target.Closed, "Removed Value");
        }

        /// <summary>
        ///A test for Rotation
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Shape_RotationTest()
        {
            PDFShapeStyle target = new PDFShapeStyle();
            Assert.AreEqual(0.0, target.Rotation);

            target.Rotation = 90.0;
            Assert.AreEqual(90.0, target.Rotation);

            target.RemoveRotation();
            Assert.AreEqual(0.0, target.Rotation);
        }
    }
}
