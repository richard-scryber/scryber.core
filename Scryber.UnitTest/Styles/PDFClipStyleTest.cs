using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.Core.UnitTests.Styles
{
    
    
    /// <summary>
    ///This is a test class for PDFClipStyleTest and is intended
    ///to contain all PDFClipStyleTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFClipStyleTest
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
        ///A test for PDFClipStyle Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void ClipStyle_ConstructorTest()
        {
            ClipStyle target = new ClipStyle();
            Assert.IsNotNull(target);
            Assert.AreEqual(StyleKeys.ClipItemKey,target.ItemKey);
        }


        /// <summary>
        ///A test for GetThickness
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Clip_TryGetThicknessTest()
        {
            ClipStyle target = new ClipStyle();

            Thickness actual;
            Thickness expected = Thickness.Empty();
            bool result = target.TryGetThickness(out actual);
            Assert.IsFalse(result);
            Assert.AreEqual(expected, actual);

            target.All = 12;
            expected = new Thickness(12);

            result = target.TryGetThickness(out actual);
            Assert.IsTrue(result);
            Assert.AreEqual(expected, actual);

            target.Left = 13;
            target.Right = 14;
            target.Top = 15;
            target.Bottom = 16;
            expected = new Thickness(15, 14, 16, 13);

            result = target.TryGetThickness(out actual);
            Assert.IsTrue(result);
            Assert.AreEqual(expected, actual);

            target.RemoveAllValues();
            expected = Thickness.Empty();
            result = target.TryGetThickness(out actual);
            Assert.IsFalse(result);
            Assert.AreEqual(expected, actual);
        }



        /// <summary>
        ///A test for SetClip
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Clip_SetThicknessTest()
        {
            ClipStyle target = new ClipStyle();
            target.All = 12;
            target.Left = 13;
            target.Right = 14;
            target.Top = 15;
            target.Bottom = 16;

            Thickness thickness = new Thickness(21, 22, 23, 24); //T,R,B,L
            target.SetThickness(thickness);

            Assert.AreEqual((Unit)21, target.Top);
            Assert.AreEqual((Unit)24, target.Left);
            Assert.AreEqual((Unit)23, target.Bottom);
            Assert.AreEqual((Unit)22, target.Right);
        }

        /// <summary>
        ///A test for All
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Clip_AllTest()
        {
            ClipStyle target = new ClipStyle();
            Assert.AreEqual(Unit.Zero, target.All);

            target.All = 20;
            Assert.AreEqual((Unit)20, target.All);

            //These properties fall back to all
            Assert.AreEqual((Unit)20, target.Left);
            Assert.AreEqual((Unit)20, target.Top);
            Assert.AreEqual((Unit)20, target.Bottom);
            Assert.AreEqual((Unit)20, target.Right);

            //Modify one and make sure the rest as still set
            target.Bottom = 300;

            Assert.AreEqual((Unit)20, target.All);
            Assert.AreEqual((Unit)20, target.Left);
            Assert.AreEqual((Unit)20, target.Top);
            Assert.AreEqual((Unit)300, target.Bottom); //different
            Assert.AreEqual((Unit)20, target.Right);

            target.RemoveAll();
            Assert.AreEqual(Unit.Zero, target.All);

            Assert.AreEqual(Unit.Zero, target.Left);
            Assert.AreEqual(Unit.Zero, target.Top);
            Assert.AreEqual((Unit)300, target.Bottom); //retained the separate value
            Assert.AreEqual(Unit.Zero, target.Right);
        }

        /// <summary>
        ///A test for Bottom
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Clip_BottomTest()
        {
            ClipStyle target = new ClipStyle();
            Assert.AreEqual(Unit.Zero, target.Bottom);

            target.Bottom = 20;
            Assert.AreEqual((Unit)20, target.Bottom);

            target.RemoveBottom();
            Assert.AreEqual(Unit.Zero, target.Bottom);
        }

        /// <summary>
        ///A test for Left
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Clip_LeftTest()
        {
            ClipStyle target = new ClipStyle();
            Assert.AreEqual(Unit.Zero, target.Left);

            target.Left = 20;
            Assert.AreEqual((Unit)20, target.Left);

            target.RemoveLeft();
            Assert.AreEqual(Unit.Zero, target.Left);
        }

        /// <summary>
        ///A test for Right
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Clip_RightTest()
        {
            ClipStyle target = new ClipStyle();
            Assert.AreEqual(Unit.Zero, target.Right);

            target.Right = 20;
            Assert.AreEqual((Unit)20, target.Right);

            target.RemoveRight();
            Assert.AreEqual(Unit.Zero, target.Right);
        }

        /// <summary>
        ///A test for Top
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Clip_TopTest()
        {
            ClipStyle target = new ClipStyle();
            Assert.AreEqual(Unit.Zero, target.Top);

            target.Top = 20;
            Assert.AreEqual((Unit)20, target.Top);

            target.RemoveTop();
            Assert.AreEqual(Unit.Zero, target.Top);
        }

    }
}
