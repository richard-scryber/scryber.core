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
        ///A test for PDFTransformationMatrix Constructor
        ///</summary>
        [TestMethod()]
        public void PDFTransformationMatrixConstructor_Test()
        {
            PDFTransformationMatrix target = new PDFTransformationMatrix();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for PDFTransformationMatrix Constructor
        ///</summary>
        [TestMethod()]
        public void PDFTransformationMatrixConstructor_Test1()
        {
            float offsetX = 0F; // TODO: Initialize to an appropriate value
            float offsetY = 0F; // TODO: Initialize to an appropriate value
            float angle = 0F; // TODO: Initialize to an appropriate value
            float scaleX = 0F; // TODO: Initialize to an appropriate value
            float scaleY = 0F; // TODO: Initialize to an appropriate value
            PDFTransformationMatrix target = new PDFTransformationMatrix(offsetX, offsetY, angle, scaleX, scaleY);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        

        

        /// <summary>
        ///A test for Components
        ///</summary>
        [TestMethod()]
        public void Components_Test()
        {
            //PDFTransformationMatrix target = new PDFTransformationMatrix(); // TODO: Initialize to an appropriate value
            //double[] actual;
            //actual = target.Components;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
