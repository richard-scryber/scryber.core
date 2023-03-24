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
            Style root = new Style();
            StyleStack target = new StyleStack(root);
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
            Style root = new Style();
            StyleStack target = new StyleStack(root);

            Style one = new Style();
            target.Push(one);

            Style two = new Style();
            target.Push(two);

            StyleStack expected = target;
            StyleStack actual;
            actual = target.Clone();

            //pop them off in sequence
            
            Style popped = expected.Pop();
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
            Style root = new Style();
            root.Background.Color = StandardColors.Red; //Not inherited
            root.Font.FontFamily = (FontSelector)"Symbol"; //Font is inherited
            root.Font.FontSize = 20; //Font is inherited

            StyleStack target = new StyleStack(root);

            Style one = new Style();
            one.Background.FillStyle = FillType.Pattern; //Background is not inherited
            one.Border.Width = 2; //Border is not inherited
            one.Font.FontSize = 48; //Font is inherited - override root
            target.Push(one);

            //last item will always be merged in a full style - actual, not inherited values
            Style two = new Style();
            two.Border.Color = StandardColors.Lime;
            two.Border.Width = 3;
            two.Font.FontItalic = true;
            target.Push(two);

            Label lbl = new Label();
            Style actual = target.GetFullStyle(lbl, Size.Empty, Size.Empty, Size.Empty, Unit.Zero);

            Assert.AreEqual("Symbol", actual.Font.FontFamily.FamilyName); //inherited from root
            Assert.AreEqual((Unit)48, actual.Font.FontSize); //inherited from one
            Assert.AreEqual((Unit)3, actual.Border.Width); //border from two
            Assert.AreEqual(StandardColors.Lime, actual.Border.Color); //border from two
            Assert.AreEqual(true, actual.Font.FontItalic); //font from two
            Assert.AreEqual(Color.Transparent, actual.Background.Color); //not inherited from root
            Assert.AreEqual(FillType.None, actual.Background.FillStyle); //not inherited from one
            
        }

        /// <summary>
        ///A test for GetFullStyle with relative percent values
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void GetFullStyleTest_RelativePercent()
        {
            Style root = new Style();
            root.Background.Color = StandardColors.Red; //Not inherited
            root.Font.FontFamily = (FontSelector)"Symbol"; //Font is inherited
            root.Font.FontSize = Unit.Pt(20); //Font is inherited

            StyleStack target = new StyleStack(root);

            Style one = new Style();
            one.Background.FillStyle = FillType.Pattern; //Background is not inherited
            one.Border.Width = 2; //Border is not inherited
            one.Font.FontSize = Unit.Percent(50); //Font is inherited - calculate from root
            one.Padding.All = Unit.Percent(5); //5 percent padding
            one.Margins.All = Unit.Percent(10); //10 percent margins
            one.Margins.Left = Unit.Percent(15); //15 percent left margin

            target.Push(one);

            Size page = new Size(600, 800);
            Size container = new Size(300, 400);
            Size font = new Size(10, 20);
            Unit rem = new Unit(16);

            Label lbl = new Label();
            Style actual = target.GetFullStyle(lbl, page, container, font, rem);

            

            Assert.AreEqual((Unit)10, actual.Font.FontSize); //50%
            Assert.AreEqual((Unit)2, actual.Border.Width); //absolute
            Assert.AreEqual((Unit)(400.0 * 0.05), actual.Padding.All); //5% of container height (default vertical)
            Assert.AreEqual((Unit)(400.0 * 0.1), actual.Margins.All); //10% of container height (default vertical)
            Assert.AreEqual((Unit)(300.0 * 0.15), actual.Margins.Left); //15% of container width
        }

        /// <summary>
        ///A test for GetFullStyle with relative percent values
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void GetFullStyleTest_RelativeViewWidth()
        {
            //Assert.Inconclusive();

            Style root = new Style();
            root.Background.Color = StandardColors.Red; //Not inherited
            root.Font.FontFamily = (FontSelector)"Symbol"; //Font is inherited
            root.Font.FontSize = Unit.Pt(20); //Font is inherited

            StyleStack target = new StyleStack(root);

            Style one = new Style();
            one.Background.FillStyle = FillType.Pattern; //Background is not inherited
            one.Border.Width = 2; //Border is not inherited
            one.Font.FontSize = Unit.Vw(5); 
            one.Padding.All = Unit.Vw(15);
            one.Margins.All = Unit.Vw(10); 
            one.Margins.Left = Unit.Vw(20);

            target.Push(one);

            Size page = new Size(600, 800);
            Size container = new Size(400, 500);
            Size font = new Size(10, 20);
            Unit rem = new Unit(16);

            Label lbl = new Label();
            Style actual = target.GetFullStyle(lbl, page, container, font, rem);



            Assert.AreEqual((Unit)(600 * 0.05), actual.Font.FontSize); //50%
            Assert.AreEqual((Unit)2, actual.Border.Width); //absolute
            Assert.AreEqual((Unit)(600.0 * 0.15), actual.Padding.All); 
            Assert.AreEqual((Unit)(600.0 * 0.10), actual.Margins.All);
            Assert.AreEqual((Unit)(600.0 * 0.20), actual.Margins.Left); 
        }

        /// <summary>
        ///A test for GetFullStyle with relative percent values
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void GetFullStyleTest_RelativeViewHeight()
        {
            

            Style root = new Style();
            root.Background.Color = StandardColors.Red; //Not inherited
            root.Font.FontFamily = (FontSelector)"Symbol"; //Font is inherited
            root.Font.FontSize = Unit.Pt(20); //Font is inherited

            StyleStack target = new StyleStack(root);

            Style one = new Style();
            one.Background.FillStyle = FillType.Pattern; //Background is not inherited
            one.Border.Width = 2; //Border is not inherited
            one.Font.FontSize = Unit.Vh(6); 
            one.Padding.All = Unit.Vh(16); 
            one.Margins.All = Unit.Vh(11); 
            one.Margins.Left = Unit.Vh(21); 

            target.Push(one);

            Size page = new Size(600, 800);
            Size container = new Size(400, 500);
            Size font = new Size(10, 20);
            Unit rem = new Unit(16);

            Label lbl = new Label();
            Style actual = target.GetFullStyle(lbl, page, container, font, rem);

            Assert.AreEqual((Unit)(800 * 0.06), actual.Font.FontSize); //50%
            Assert.AreEqual((Unit)2, actual.Border.Width); //absolute
            Assert.AreEqual((Unit)(800.0 * 0.16), actual.Padding.All); 
            Assert.AreEqual((Unit)(800.0 * 0.11), actual.Margins.All); 
            Assert.AreEqual((Unit)(800.0 * 0.21), actual.Margins.Left); 
        }

        /// <summary>
        ///A test for GetFullStyle with relative percent values
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void GetFullStyleTest_RelativeViewMin()
        {
            //Assert.Inconclusive();

            Style root = new Style();
            root.Background.Color = StandardColors.Red; //Not inherited
            root.Font.FontFamily = (FontSelector)"Symbol"; //Font is inherited
            root.Font.FontSize = Unit.Pt(20); //Font is inherited

            StyleStack target = new StyleStack(root);

            Style one = new Style();
            one.Background.FillStyle = FillType.Pattern; //Background is not inherited
            one.Border.Width = 2; //Border is not inherited
            one.Font.FontSize = Unit.Vmin(7); 
            one.Padding.All = Unit.Vmin(17); 
            one.Margins.All = Unit.Vmin(12); 
            one.Margins.Left = Unit.Vmin(22); 

            target.Push(one);

            Size page = new Size(600, 800);
            Size container = new Size(400, 500);
            Size font = new Size(10, 20);
            Unit rem = new Unit(16);

            Label lbl = new Label();
            Style actual = target.GetFullStyle(lbl, page, container, font, rem);

            //vmin is 600
            Assert.AreEqual((Unit)(600 * 0.07), actual.Font.FontSize); 
            Assert.AreEqual((Unit)2, actual.Border.Width); //absolute
            Assert.AreEqual((Unit)(600.0 * 0.17), actual.Padding.All);
            Assert.AreEqual((Unit)(600.0 * 0.12), actual.Margins.All); 
            Assert.AreEqual((Unit)(600.0 * 0.22), actual.Margins.Left); 
        }

        /// <summary>
        ///A test for GetFullStyle with relative percent values
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void GetFullStyleTest_RelativeViewMax()
        {
            //Assert.Inconclusive();

            Style root = new Style();
            root.Background.Color = StandardColors.Red; //Not inherited
            root.Font.FontFamily = (FontSelector)"Symbol"; 
            root.Font.FontSize = Unit.Pt(20); 

            StyleStack target = new StyleStack(root);

            Style one = new Style();
            one.Background.FillStyle = FillType.Pattern; //Background is not inherited
            one.Border.Width = 2; //Border is not inherited
            one.Font.FontSize = Unit.Vmax(8); 
            one.Padding.All = Unit.Vmax(18); 
            one.Margins.All = Unit.Vmax(13); 
            one.Margins.Left = Unit.Vmax(23);

            target.Push(one);

            Size page = new Size(600, 800);
            Size container = new Size(400, 500);
            Size font = new Size(10, 20);
            Unit rem = new Unit(16);

            Label lbl = new Label();
            Style actual = target.GetFullStyle(lbl, page, container, font, rem);

            //vmin is 600
            Assert.AreEqual((Unit)(800 * 0.08), actual.Font.FontSize); 
            Assert.AreEqual((Unit)2, actual.Border.Width); //absolute
            Assert.AreEqual((Unit)(800.0 * 0.18), actual.Padding.All); 
            Assert.AreEqual((Unit)(800.0 * 0.13), actual.Margins.All); 
            Assert.AreEqual((Unit)(800.0 * 0.23), actual.Margins.Left); 
        }


        /// <summary>
        ///A test for GetFullStyle with relative percent values
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void GetFullStyleTest_RelativeEmHeight()
        {
            //Assert.Inconclusive();

            Style root = new Style();
            root.Background.Color = StandardColors.Red; //Not inherited
            root.Font.FontFamily = (FontSelector)"Symbol"; //Font is inherited
            root.Font.FontSize = Unit.Pt(20); //Font is inherited

            StyleStack target = new StyleStack(root);

            Style one = new Style();
            one.Background.FillStyle = FillType.Pattern; //Background is not inherited
            one.Border.Width = 2; //Border is not inherited
            one.Font.FontSize = Unit.Em(4); 
            one.Padding.All = Unit.Em(5); 
            one.Margins.All = Unit.Em(3);
            one.Margins.Left = Unit.Em(2);

            target.Push(one);

            Size page = new Size(600, 800);
            Size container = new Size(400, 500);
            Size font = new Size(10, 20);
            Unit rem = new Unit(16);

            Label lbl = new Label();
            Style actual = target.GetFullStyle(lbl, page, container, font, rem);

            //vmin is 600
            Assert.AreEqual((Unit)(20 * 4.0), actual.Font.FontSize); 
            Assert.AreEqual((Unit)2, actual.Border.Width); //absolute
            Assert.AreEqual((Unit)(20 * 5), actual.Padding.All);
            Assert.AreEqual((Unit)(20 * 3), actual.Margins.All); 
            Assert.AreEqual((Unit)(20 * 2), actual.Margins.Left); 
        }


        /// <summary>
        ///A test for GetFullStyle with relative percent values
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void GetFullStyleTest_RelativeExHeight()
        {
            //Assert.Inconclusive();

            Style root = new Style();
            root.Background.Color = StandardColors.Red; //Not inherited
            root.Font.FontFamily = (FontSelector)"Symbol"; //Font is inherited
            root.Font.FontSize = Unit.Pt(20); //Font is inherited

            StyleStack target = new StyleStack(root);

            Style one = new Style();
            one.Background.FillStyle = FillType.Pattern; //Background is not inherited
            one.Border.Width = 2; //Border is not inherited
            one.Font.FontSize = Unit.Ex(8); 
            one.Padding.All = Unit.Ex(10); 
            one.Margins.All = Unit.Ex(6); 
            one.Margins.Left = Unit.Ex(3); 

            target.Push(one);

            Size page = new Size(600, 800);
            Size container = new Size(400, 500);
            Size font = new Size(10, 20);
            Unit rem = new Unit(16);

            Label lbl = new Label();
            Style actual = target.GetFullStyle(lbl, page, container, font, rem);

            //Ex is x char height, approx same as 0 width, so thats what we use.
            Assert.AreEqual((Unit)(10 * 8.0), actual.Font.FontSize);
            Assert.AreEqual((Unit)2, actual.Border.Width); //absolute
            Assert.AreEqual((Unit)(10 * 10), actual.Padding.All); 
            Assert.AreEqual((Unit)(10 * 6), actual.Margins.All); 
            Assert.AreEqual((Unit)(10 * 3), actual.Margins.Left); 
        }

        /// <summary>
        ///A test for GetFullStyle with relative percent values
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void GetFullStyleTest_RelativeChWidth()
        {
            //Assert.Inconclusive();

            Style root = new Style();
            root.Background.Color = StandardColors.Red; //Not inherited
            root.Font.FontFamily = (FontSelector)"Symbol"; //Font is inherited
            root.Font.FontSize = Unit.Pt(20); //Font is inherited

            StyleStack target = new StyleStack(root);

            Style one = new Style();
            one.Background.FillStyle = FillType.Pattern; 
            one.Border.Width = 2; //Border is not inherited
            one.Font.FontSize = Unit.Ex(7); 
            one.Padding.All = Unit.Ex(9); 
            one.Margins.All = Unit.Ex(8); 
            one.Margins.Left = Unit.Ex(4); 

            target.Push(one);

            Size page = new Size(600, 800);
            Size container = new Size(400, 500);
            Size font = new Size(10, 20);
            Unit rem = new Unit(16);

            Label lbl = new Label();
            Style actual = target.GetFullStyle(lbl, page, container, font, rem);

            //ch is zero width - font.Width
            Assert.AreEqual((Unit)(10 * 7.0), actual.Font.FontSize); 
            Assert.AreEqual((Unit)2, actual.Border.Width); //absolute
            Assert.AreEqual((Unit)(10 * 9), actual.Padding.All); 
            Assert.AreEqual((Unit)(10 * 8), actual.Margins.All); 
            Assert.AreEqual((Unit)(10 * 4), actual.Margins.Left); 
        }

        /// <summary>
        ///A test for GetFullStyle with relative percent values
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void GetFullStyleTest_RelativeRemHeight()
        {

            Style root = new Style();
            root.Background.Color = StandardColors.Red; //Not inherited
            root.Font.FontFamily = (FontSelector)"Symbol"; //Font is inherited
            root.Font.FontSize = Unit.Pt(20); //Font is inherited

            StyleStack target = new StyleStack(root);

            Style one = new Style();
            one.Background.FillStyle = FillType.Pattern; //Background is not inherited
            one.Border.Width = 2; //Border is not inherited
            one.Font.FontSize = Unit.RootEm(2); 
            one.Padding.All = Unit.RootEm(3); 
            one.Margins.All = Unit.RootEm(4); 
            one.Margins.Left = Unit.RootEm(5); 

            target.Push(one);

            Size page = new Size(600, 800);
            Size container = new Size(400, 500);
            Size font = new Size(10, 20);
            Unit rem = new Unit(16);

            Label lbl = new Label();
            Style actual = target.GetFullStyle(lbl, page, container, font, rem);

            //ch is zero width - font.Width
            Assert.AreEqual((Unit)(16 * 2), actual.Font.FontSize); 
            Assert.AreEqual((Unit)2, actual.Border.Width); //absolute
            Assert.AreEqual((Unit)(16 * 3), actual.Padding.All); 
            Assert.AreEqual((Unit)(16 * 4), actual.Margins.All); 
            Assert.AreEqual((Unit)(16 * 5), actual.Margins.Left);
        }


        /// <summary>
        ///A test for Pop
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void PopTest()
        {
            Style root = new Style();
            StyleStack target = new StyleStack(root);

            Style one = new Style();
            target.Push(one);

            Style two = new Style();
            target.Push(two);

            Style expected = two;
            Style actual;
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
            Style root = new Style();
            StyleStack target = new StyleStack(root);

            Style one = new Style();
            target.Push(one);

            Style two = new Style();
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
            Style root = new Style();
            StyleStack target = new StyleStack(root);
            Assert.AreEqual(root, target.Current);

            Style one = new Style();
            target.Push(one);
            Assert.AreEqual(one, target.Current);

            Style two = new Style();
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
            Style root = new Style();
            StyleStack target = new StyleStack(root);
            Assert.AreEqual(1, target.Count);

            Style one = new Style();
            target.Push(one);
            Assert.AreEqual(2, target.Count);

            Style two = new Style();
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
