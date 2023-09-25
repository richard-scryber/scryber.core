using Scryber.Styles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber;
using Scryber.Drawing;
using Scryber.Components;

namespace Scryber.Core.UnitTests.Styles
{
    
    
    /// <summary>
    ///This is a test class for PDFStyleCollectionTest and is intended
    ///to contain all PDFStyleCollectionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFStyleCollectionTest
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
        ///A test for PDFStyleCollection Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void PDFStyleCollectionConstructorTest()
        {
            StyleCollection target = new StyleCollection();
            Assert.IsNotNull(target);
            Assert.AreEqual(0, target.Count);
        }

        /// <summary>
        ///A test for MergeInto
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void MergeIntoTest()
        {
            StyleCollection target = new StyleCollection();
            StyleDefn defn = new StyleDefn();
            defn.Match = "Label.sea";

            defn.Border.Color = StandardColors.Red;
            defn.Border.Width = 10;
            defn.Font.FontFamily = (FontSelector)"Helvetica";

            target.Add(defn);

            StyleDefn defn2 = new StyleDefn();
            defn2.Match = ".sea"; // lower priority

            defn2.Border.Color = StandardColors.Gray; 
            defn2.Columns.ColumnCount = 3;
            target.Add(defn2);

            StyleDefn defn3 = new StyleDefn();
            defn3.AppliedClass = "other";
            defn3.Border.Width = 20;
            defn3.Stroke.Color = StandardColors.Aqua;
            target.Add(defn3);

            Label lbl = new Label();
            lbl.ElementName = "Label";
            

            Style style = new Style();
            target.MergeInto(style, lbl);
            Assert.IsFalse(style.HasValues);//no style class on the label

            lbl.StyleClass = "sea";
            style = new Style();
            target.MergeInto(style, lbl);

            Assert.AreEqual(StandardColors.Red, style.Border.Color); //from defn as higher priority
            Assert.AreEqual((Unit)10, style.Border.Width); // from defn (defn2 has no width)
            Assert.AreEqual((FontSelector)"Helvetica", style.Font.FontFamily); //from defn
            Assert.AreEqual(3, style.Columns.ColumnCount); //from defn2 (lower priority but not set on defn)
            Assert.IsFalse(style.IsValueDefined(StyleKeys.StrokeColorKey)); //defn3 does have a stroke, but shoulld not be included



        }

        /// <summary>
        ///A test for Item
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void ItemTest()
        {
            StyleCollection target = new StyleCollection();
            StyleDefn defn = new StyleDefn();
            defn.Border.Color = StandardColors.Red;
            defn.Border.Width = 10;
            target.Add(defn);

            StyleDefn defn2 = new StyleDefn();
            defn2.Border.Color = StandardColors.Gray;
            defn2.Border.Width = 2;
            target.Add(defn2);

            int index = 0;
            StyleBase expected = defn;
            StyleBase actual = target[index];

            Assert.AreEqual(expected, actual);

            index = 1;
            expected = defn2;
            actual = target[index];
            Assert.AreEqual(expected, actual);

            expected = defn;
            target[index] = defn;
            actual = target[index];
            Assert.AreEqual(expected, actual);
        }


        /// <summary>
        /// A test for Owner - and the flowing through of parent hierarchy based on that value.
        /// </summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void OwnerTest()
        {
            Document doc = new Document();
            doc.ID = "First Document";

            //doc.Styles should be initialized with the document as it's owner
            StyleCollection col = doc.Styles;
            Assert.AreEqual(doc, col.Owner);

            //A non IPDFComponent entry - make sure there are not cast exceptions
            Style unowned = new Style();
            col.Add(unowned);

            //An IPDFComponent entry - so should have it's parent value set
            StylesDocument styleDoc = new StylesDocument();
            col.Add(styleDoc);

            //parent should automatically be set on the styleDoc from the parent.
            Assert.AreEqual(doc, styleDoc.Parent);

            //Changing the owner should pass through
            Document other = new Document();
            other.ID = "Other Document";
            col.Owner = other;
            Assert.AreEqual(other, styleDoc.Parent);

            //removing the time should not set the owner.
            col.Remove(styleDoc);
            Assert.IsNull(styleDoc.Parent, "The parent was not cleared from the document on removal");

            //multiple hierarchy
            StylesDocument inner = new StylesDocument();
            styleDoc.Styles.Add(inner);
            doc.Styles.Add(styleDoc);
            Assert.AreEqual(inner.Parent, styleDoc);
            Assert.AreEqual(other, styleDoc.Parent);

        }
    }
}
