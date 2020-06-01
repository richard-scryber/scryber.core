using Scryber.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Scryber.Core.UnitTests.Drawing
{
    
    
    /// <summary>
    ///This is a test class for PDFFontMetrics_Test and is intended
    ///to contain all PDFFontMetrics_Test Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFFontMetrics_Test
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
        ///A test for PDFFontMetrics Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Fonts")]
        public void PDFFontMetricsConstructor_Test()
        {
            double emheight = 12.7;
            double ascent = 120.34;
            double descent = 12.9;
            double lineheight = 13.6;
            PDFFontMetrics target = new PDFFontMetrics(emheight, ascent, descent, lineheight);
            

            Assert.IsNotNull(target);
            Assert.AreEqual(emheight, target.EmHeight);
            Assert.AreEqual(ascent, target.Ascent);
            Assert.AreEqual(descent, target.Descent);
            Assert.AreEqual(lineheight, target.LineHeight);
            Assert.AreEqual(lineheight - (ascent + descent), target.LineSpacing);
        }

    }
}
