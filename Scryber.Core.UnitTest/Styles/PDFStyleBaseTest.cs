using Scryber.Styles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber;

namespace Scryber.Core.UnitTests.Styles
{
    
    
    /// <summary>
    ///This is a test class for PDFStyleBaseTest and is intended
    ///to contain all PDFStyleBaseTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFStyleBaseTest
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
        /// Creates a new style with the Background and 
        /// </summary>
        /// <returns></returns>
        internal virtual PDFStyleDefn CreatePDFStyleBase()
        {
            PDFStyleDefn style = new PDFStyleDefn();
            style.AppliedClass = "sea";
            style.AppliedID = "mylabel";
            style.AppliedType = typeof(Components.PDFLabel);

            style.Background.Color = Scryber.Drawing.PDFColors.Aqua;

            style.Border.Width = 4;
            style.Border.Color = Scryber.Drawing.PDFColors.Blue;

            Assert.IsTrue(style.HasValues);
            return style;
        }

        private PDFStyleDefn CreateAlternateStyle()
        {
            PDFStyleDefn style = new PDFStyleDefn();
            style.Padding.All = 10;
            style.Background.Color = Scryber.Drawing.PDFColors.Yellow;
            style.Background.FillStyle = Scryber.Drawing.FillStyle.Pattern;
            return style;
        }

        #region public void DataBindTest()

        private bool bg_bound;
        private bool fill_binding;
        private bool style_bound;
        private bool style_binding;

        /// <summary>
        ///A test for DataBind
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void DataBindTest()
        {
            PDFStyle target = CreatePDFStyleBase();
            PDFDataContext context = new PDFDataContext(new PDFItemCollection(null), new Logging.DoNothingTraceLog(TraceRecordLevel.Off), new PDFPerformanceMonitor(true));

            bg_bound = false;
            fill_binding = false;
            style_binding = false;
            style_bound = false;

            target.DataBinding += target_DataBinding;
            target.DataBound += target_DataBound;

            //Make sure the inner items are bound too.
            target.Background.DataBound += Background_DataBound;
            target.Fill.DataBinding += Fill_DataBinding;

            target.DataBind(context);

            Assert.IsTrue(style_binding);
            Assert.IsTrue(style_bound);
            Assert.IsTrue(bg_bound);
            Assert.IsTrue(fill_binding);
        }

        void Fill_DataBinding(object sender, PDFDataBindEventArgs e)
        {
            fill_binding = true;
        }

        void Background_DataBound(object sender, PDFDataBindEventArgs e)
        {
            bg_bound = true;
        }

        void target_DataBound(object sender, PDFDataBindEventArgs e)
        {
            style_bound = true;
        }

        void target_DataBinding(object sender, PDFDataBindEventArgs e)
        {
            style_binding = true;
        }

        #endregion



        

        /// <summary>
        ///A test for MergeInto
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void MergeIntoTest()
        {
            PDFStyleDefn target = CreatePDFStyleBase();
            PDFStyleDefn style = CreateAlternateStyle();

            //
            // label with matching class name - should be merged
            //

            Scryber.Components.PDFLabel lbl = new Components.PDFLabel();
            lbl.StyleClass = "sea";
            lbl.ID = "mylabel";
            ComponentState state = ComponentState.Down;

            target.MergeInto(style, lbl, state);
            style.Flatten(); //must flatten the style after a merge

            Assert.AreEqual(style.Padding.All, (Scryber.Drawing.PDFUnit)10); // part of style
            Assert.AreEqual(style.Background.Color, Scryber.Drawing.PDFColors.Aqua); // should have been replaced by targets color
            Assert.AreEqual(style.Background.FillStyle, Scryber.Drawing.FillStyle.Pattern); //from the target - not replaced by the change of color
            Assert.IsTrue(style.IsValueDefined(PDFStyleKeys.BorderColorKey));
            Assert.AreEqual(style.Border.Width, (Scryber.Drawing.PDFUnit)4); //from the target - not originally in style
            Assert.AreEqual(style.Border.Color, Scryber.Drawing.PDFColors.Blue); //from the target - not originally in style

            //No style class so should not be merged.
            
            target = CreatePDFStyleBase();
            style = CreateAlternateStyle();

            lbl = new Components.PDFLabel();
            lbl.ID = "mylabel";
            target.MergeInto(style, lbl, state);
            Assert.AreEqual(style.Padding.All, (Scryber.Drawing.PDFUnit)10); 
            Assert.AreEqual(style.Background.Color, Scryber.Drawing.PDFColors.Yellow); 
            Assert.AreEqual(style.Background.FillStyle, Scryber.Drawing.FillStyle.Pattern);
            Assert.IsFalse(style.IsValueDefined(PDFStyleKeys.BorderColorKey));

            //Different ID so should not be merged.
            
            target = CreatePDFStyleBase();
            style = CreateAlternateStyle();

            lbl = new Components.PDFLabel();
            lbl.StyleClass = "sea";
            lbl.ID = "anotherlabel";
            target.MergeInto(style, lbl, state);
            Assert.AreEqual(style.Padding.All, (Scryber.Drawing.PDFUnit)10); 
            Assert.AreEqual(style.Background.Color, Scryber.Drawing.PDFColors.Yellow);
            Assert.AreEqual(style.Background.FillStyle, Scryber.Drawing.FillStyle.Pattern);
            Assert.IsFalse(style.IsValueDefined(PDFStyleKeys.BorderColorKey));

            // different type

            Components.PDFImage img = new Components.PDFImage();
            img.StyleClass = "sea";
            img.ID = "mylabel";
            target.MergeInto(style, img, state);
            Assert.AreEqual(style.Padding.All, (Scryber.Drawing.PDFUnit)10);
            Assert.AreEqual(style.Background.Color, Scryber.Drawing.PDFColors.Yellow);
            Assert.AreEqual(style.Background.FillStyle, Scryber.Drawing.FillStyle.Pattern);
            Assert.IsFalse(style.IsValueDefined(PDFStyleKeys.BorderColorKey));
        }
    }
}
