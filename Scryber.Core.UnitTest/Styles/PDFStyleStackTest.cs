using Scryber.Styles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber;
using Scryber.Drawing;
using Scryber.Components;

namespace Scryber.Core.UnitTests.Styles
{
    
    
    /// <summary>
    ///This is a test class for PDFStyleStackTest and is intended
    ///to contain all PDFStyleStackTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFStyleStackTest
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
        ///A test for PDFStyleStack Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void PDFStyleStackConstructorTest()
        {
            PDFStyle root = new PDFStyle();
            PDFStyleStack target = new PDFStyleStack(root);
            Assert.IsNotNull(target);
            Assert.IsNotNull(target.Current);
            Assert.AreEqual(root, target.Current);
        }

        /// <summary>
        ///A test for Clone
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void CloneTest()
        {
            PDFStyle root = new PDFStyle();
            PDFStyleStack target = new PDFStyleStack(root);

            PDFStyle one = new PDFStyle();
            target.Push(one);

            PDFStyle two = new PDFStyle();
            target.Push(two);

            PDFStyleStack expected = target;
            PDFStyleStack actual;
            actual = target.Clone();

            //pop them off in sequence
            
            PDFStyle popped = expected.Pop();
            Assert.AreEqual(popped, actual.Pop());
            
            popped = expected.Pop();
            Assert.AreEqual(popped, actual.Pop());
            
            popped = expected.Pop();
            Assert.AreEqual(popped, actual.Pop());
        }

        /// <summary>
        ///A test for GetFullStyle
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void GetFullStyleTest()
        {
            PDFStyle root = new PDFStyle();
            root.Background.Color = PDFColors.Red; //Not inherited
            root.Font.FontFamily = "Symbol"; //Font is inherited
            root.Font.FontSize = 20; //Font is inherited

            PDFStyleStack target = new PDFStyleStack(root);

            PDFStyle one = new PDFStyle();
            one.Background.FillStyle = FillStyle.Pattern; //Background is not inherited
            one.Border.Width = 2; //Border is not inherited
            one.Font.FontSize = 48; //Font is inherited - override root
            target.Push(one);

            //last item will always be merged in a full style - actual, not inherited values
            PDFStyle two = new PDFStyle();
            two.Border.Color = PDFColors.Lime;
            two.Border.Width = 3;
            two.Font.FontItalic = true;
            target.Push(two);

            Label lbl = new Label();
            PDFStyle actual = target.GetFullStyle(lbl);

            Assert.AreEqual("Symbol", actual.Font.FontFamily); //inherited from root
            Assert.AreEqual((PDFUnit)48, actual.Font.FontSize); //inherited from one
            Assert.AreEqual((PDFUnit)3, actual.Border.Width); //border from two
            Assert.AreEqual(PDFColors.Lime, actual.Border.Color); //border from two
            Assert.AreEqual(true, actual.Font.FontItalic); //font from two
            Assert.AreEqual(PDFColor.Transparent, actual.Background.Color); //not inherited from root
            Assert.AreEqual(FillStyle.None, actual.Background.FillStyle); //not inherited from one
            
        }

        /// <summary>
        ///A test for Pop
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void PopTest()
        {
            PDFStyle root = new PDFStyle();
            PDFStyleStack target = new PDFStyleStack(root);

            PDFStyle one = new PDFStyle();
            target.Push(one);

            PDFStyle two = new PDFStyle();
            target.Push(two);

            PDFStyle expected = two;
            PDFStyle actual;
            actual = target.Pop();
            Assert.AreEqual(expected, actual);

            expected = one;
            actual = target.Pop();
            Assert.AreEqual(expected, actual);

            expected = root;
            actual = target.Pop();
            Assert.AreEqual(expected, actual);

            try
            {
                actual = target.Pop();
                throw new ArgumentException("Stack should be empty");
            }
            catch (InvalidOperationException)
            {
                //Successfully thrown empty stack exception
            }
        }


        /// <summary>
        ///A test for Push
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void PushTest()
        {
            PDFStyle root = new PDFStyle();
            PDFStyleStack target = new PDFStyleStack(root);

            PDFStyle one = new PDFStyle();
            target.Push(one);

            PDFStyle two = new PDFStyle();
            target.Push(two);

            Assert.AreEqual(3, target.Count);

        }

        /// <summary>
        ///A test for Current
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void CurrentTest()
        {
            PDFStyle root = new PDFStyle();
            PDFStyleStack target = new PDFStyleStack(root);
            Assert.AreEqual(root, target.Current);

            PDFStyle one = new PDFStyle();
            target.Push(one);
            Assert.AreEqual(one, target.Current);

            PDFStyle two = new PDFStyle();
            target.Push(two);
            Assert.AreEqual(two, target.Current);

        }

        /// <summary>
        ///A test for Count
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void CountTest()
        {
            PDFStyle root = new PDFStyle();
            PDFStyleStack target = new PDFStyleStack(root);
            Assert.AreEqual(1, target.Count);

            PDFStyle one = new PDFStyle();
            target.Push(one);
            Assert.AreEqual(2, target.Count);

            PDFStyle two = new PDFStyle();
            target.Push(two);
            Assert.AreEqual(3, target.Count);

            target.Pop();
            Assert.AreEqual(2, target.Count);

            target.Pop();
            Assert.AreEqual(1, target.Count);

            target.Pop();
            Assert.AreEqual(0, target.Count);

        }
    }
}
