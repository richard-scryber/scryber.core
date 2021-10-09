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
    public class StandardColors_Test
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
            Color expected = StandardColors.White;
            Color actual;
            actual = StandardColors.FromName(name);
            Assert.AreEqual(expected, actual);

            name = "yellow";
            expected = StandardColors.Yellow;
            actual = StandardColors.FromName(name);
            Assert.AreEqual(expected, actual);

            name = "teal";
            expected = StandardColors.Teal;
            actual = StandardColors.FromName(name);
            Assert.AreEqual(expected, actual);

            name = "silver";
            expected = StandardColors.Silver;
            actual = StandardColors.FromName(name);
            Assert.AreEqual(expected, actual);

            name = "red";
            expected = StandardColors.Red;
            actual = StandardColors.FromName(name);
            Assert.AreEqual(expected, actual);

            name = "purple";
            expected = StandardColors.Purple;
            actual = StandardColors.FromName(name);
            Assert.AreEqual(expected, actual);

            name = "olive";
            expected = StandardColors.Olive;
            actual = StandardColors.FromName(name);
            Assert.AreEqual(expected, actual);

            name = "navy";
            expected = StandardColors.Navy;
            actual = StandardColors.FromName(name);
            Assert.AreEqual(expected, actual);

            name = "maroon";
            expected = StandardColors.Maroon;
            actual = StandardColors.FromName(name);
            Assert.AreEqual(expected, actual);

            name = "lime";
            expected = StandardColors.Lime;
            actual = StandardColors.FromName(name);
            Assert.AreEqual(expected, actual);

            name = "green";
            expected = StandardColors.Green;
            actual = StandardColors.FromName(name);
            Assert.AreEqual(expected, actual);

            name = "gray";
            expected = StandardColors.Gray;
            actual = StandardColors.FromName(name);
            Assert.AreEqual(expected, actual);

            name = "fuchsia";
            expected = StandardColors.Fuchsia;
            actual = StandardColors.FromName(name);
            Assert.AreEqual(expected, actual);

            name = "blue";
            expected = StandardColors.Blue;
            actual = StandardColors.FromName(name);
            Assert.AreEqual(expected, actual);

            name = "black";
            expected = StandardColors.Black;
            actual = StandardColors.FromName(name);
            Assert.AreEqual(expected, actual);

            name = "aqua";
            expected = StandardColors.Aqua;
            actual = StandardColors.FromName(name);
            Assert.AreEqual(expected, actual);

            name = "BLACK";
            expected = StandardColors.Black;
            actual = StandardColors.FromName(name);
            Assert.AreEqual(expected, actual);

            name = "BlacK";
            expected = StandardColors.Black;
            actual = StandardColors.FromName(name);
            Assert.AreEqual(expected, actual);

            try
            {
                name = "Not A color";
                actual = StandardColors.FromName(name);
                throw new NotSupportedException("Should not get this far - throw exception with unknown name");
            }
            catch (ArgumentOutOfRangeException)
            {
                TestContext.WriteLine("Caught the expected expection");
            }


        }

    }
}
