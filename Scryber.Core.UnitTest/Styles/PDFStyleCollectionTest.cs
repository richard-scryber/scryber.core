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
            PDFStyleCollection target = new PDFStyleCollection();
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
            PDFStyleCollection target = new PDFStyleCollection();
            PDFStyleDefn defn = new PDFStyleDefn();
            defn.AppliedType = typeof(Label);
            defn.AppliedClass = "sea";

            defn.Border.Color = PDFColors.Red;
            defn.Border.Width = 10;
            defn.Font.FontFamily = "Helvetica";
            target.Add(defn);

            PDFStyleDefn defn2 = new PDFStyleDefn();
            defn2.AppliedClass = "sea";

            defn2.Border.Color = PDFColors.Gray;
            defn2.Columns.ColumnCount = 3;
            target.Add(defn2);

            PDFStyleDefn defn3 = new PDFStyleDefn();
            defn3.AppliedClass = "other";
            defn3.Border.Width = 20;
            defn3.Stroke.Color = PDFColors.Aqua;
            target.Add(defn3);

            Label lbl = new Label();
            ComponentState state = ComponentState.Normal;

            PDFStyle style = new PDFStyle();
            target.MergeInto(style, lbl, state);
            style.Flatten();
            Assert.IsFalse(style.HasValues);//no style class on the label

            lbl.StyleClass = "sea";
            style = new PDFStyle();
            target.MergeInto(style, lbl, state);
            style.Flatten();

            Assert.AreEqual(PDFColors.Gray, style.Border.Color); //from defn2
            Assert.AreEqual((PDFUnit)10, style.Border.Width); // from defn (defn2 has no width)
            Assert.AreEqual("Helvetica", style.Font.FontFamily); //from defn
            Assert.AreEqual(3, style.Columns.ColumnCount); //from defn2
            Assert.IsFalse(style.IsValueDefined(PDFStyleKeys.StrokeColorKey)); //defn3 does have a stroke, but shoulld not be included



        }

        /// <summary>
        ///A test for Item
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void ItemTest()
        {
            PDFStyleCollection target = new PDFStyleCollection();
            PDFStyleDefn defn = new PDFStyleDefn();
            defn.Border.Color = PDFColors.Red;
            defn.Border.Width = 10;
            target.Add(defn);

            PDFStyleDefn defn2 = new PDFStyleDefn();
            defn2.Border.Color = PDFColors.Gray;
            defn2.Border.Width = 2;
            target.Add(defn2);

            int index = 0;
            PDFStyleBase expected = defn;
            PDFStyleBase actual = target[index];

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
            PDFStyleCollection col = doc.Styles;
            Assert.AreEqual(doc, col.Owner);

            //A non IPDFComponent entry - make sure there are not cast exceptions
            PDFStyle unowned = new PDFStyle();
            col.Add(unowned);

            //An IPDFComponent entry - so should have it's parent value set
            PDFStylesDocument styleDoc = new PDFStylesDocument();
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
            Assert.IsNull(styleDoc.Parent);

            //multiple hierarchy
            PDFStylesDocument inner = new PDFStylesDocument();
            styleDoc.Styles.Add(inner);
            doc.Styles.Add(styleDoc);
            Assert.AreEqual(inner.Parent, styleDoc);
            Assert.AreEqual(other, styleDoc.Parent);

        }
    }
}
