using Scryber.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Scryber.Core.UnitTests.Drawing
{
    
    
    /// <summary>
    ///This is a test class for PDFColors_Test and is intended
    ///to contain all PDFColors_Test Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFColors_Test
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
        ///A test for FromName
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void FromName_Test()
        {
            string name = "white";
            PDFColor expected = PDFColors.White;
            PDFColor actual;
            actual = PDFColors.FromName(name);
            Assert.AreEqual(expected, actual);

            name = "yellow";
            expected = PDFColors.Yellow;
            actual = PDFColors.FromName(name);
            Assert.AreEqual(expected, actual);

            name = "teal";
            expected = PDFColors.Teal;
            actual = PDFColors.FromName(name);
            Assert.AreEqual(expected, actual);

            name = "silver";
            expected = PDFColors.Silver;
            actual = PDFColors.FromName(name);
            Assert.AreEqual(expected, actual);

            name = "red";
            expected = PDFColors.Red;
            actual = PDFColors.FromName(name);
            Assert.AreEqual(expected, actual);

            name = "purple";
            expected = PDFColors.Purple;
            actual = PDFColors.FromName(name);
            Assert.AreEqual(expected, actual);

            name = "olive";
            expected = PDFColors.Olive;
            actual = PDFColors.FromName(name);
            Assert.AreEqual(expected, actual);

            name = "navy";
            expected = PDFColors.Navy;
            actual = PDFColors.FromName(name);
            Assert.AreEqual(expected, actual);

            name = "maroon";
            expected = PDFColors.Maroon;
            actual = PDFColors.FromName(name);
            Assert.AreEqual(expected, actual);

            name = "lime";
            expected = PDFColors.Lime;
            actual = PDFColors.FromName(name);
            Assert.AreEqual(expected, actual);

            name = "green";
            expected = PDFColors.Green;
            actual = PDFColors.FromName(name);
            Assert.AreEqual(expected, actual);

            name = "gray";
            expected = PDFColors.Gray;
            actual = PDFColors.FromName(name);
            Assert.AreEqual(expected, actual);

            name = "fuchsia";
            expected = PDFColors.Fuchsia;
            actual = PDFColors.FromName(name);
            Assert.AreEqual(expected, actual);

            name = "blue";
            expected = PDFColors.Blue;
            actual = PDFColors.FromName(name);
            Assert.AreEqual(expected, actual);

            name = "black";
            expected = PDFColors.Black;
            actual = PDFColors.FromName(name);
            Assert.AreEqual(expected, actual);

            name = "aqua";
            expected = PDFColors.Aqua;
            actual = PDFColors.FromName(name);
            Assert.AreEqual(expected, actual);

            name = "BLACK";
            expected = PDFColors.Black;
            actual = PDFColors.FromName(name);
            Assert.AreEqual(expected, actual);

            name = "BlacK";
            expected = PDFColors.Black;
            actual = PDFColors.FromName(name);
            Assert.AreEqual(expected, actual);

            try
            {
                name = "Not A color";
                actual = PDFColors.FromName(name);
                throw new NotSupportedException("Should not get this far - throw exception with unknown name");
            }
            catch (ArgumentOutOfRangeException)
            {
                TestContext.WriteLine("Caught the expected expection");
            }


        }

    }
}
