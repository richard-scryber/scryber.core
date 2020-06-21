using Scryber.Styles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber;
using Scryber.Drawing;
using Scryber.Components;

namespace Scryber.Core.UnitTests.Styles
{
    
    
    /// <summary>
    ///This is a test class for PDFStylesDocumentTest and is intended
    ///to contain all PDFStylesDocumentTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFStylesDocumentTest
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
        ///A test for PDFStylesDocument Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void PDFStylesDocumentConstructorTest()
        {
            PDFStylesDocument target = new PDFStylesDocument();
            Assert.IsNotNull(target);
            
        }

        /// <summary>
        ///A test for PDFStylesDocument Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void PDFStylesDocumentConstructorTest1()
        {
            PDFObjectType type = (PDFObjectType)"0000";
            PDFStylesDocument target = new PDFStylesDocument(type);
            Assert.AreEqual(type, target.Type);
        }


        /// <summary>
        ///A test for Dispose
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void DisposeTest()
        {
            PDFStylesDocument target = new PDFStylesDocument(); 
            target.Dispose();
            //Just make sure it doesn't throw an exception
        }


        private bool isInitialized;

        void target_Initialized(object sender, EventArgs e)
        {
            isInitialized = true;
        }

        /// <summary>
        ///A test for Init
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void InitTest()
        {
            PDFStylesDocument target = new PDFStylesDocument(); // TODO: Initialize to an appropriate value
            PDFInitContext context = new PDFInitContext(new PDFItemCollection(null), 
                new Logging.DoNothingTraceLog(TraceRecordLevel.Off), new PDFPerformanceMonitor(true));
            target.Initialized += target_Initialized;
            
            isInitialized = false;
            target.Init(context);
            //Set to true by the event handler
            Assert.IsTrue(isInitialized);
        }

       

        /// <summary>
        ///A test for MapPath
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void MapPathTest()
        {

            //Relative file path
            
            PDFStylesDocument target = new PDFStylesDocument();
            string path = @"..\Images\MyImage.png";
            char separator = System.IO.Path.DirectorySeparatorChar;
            string root = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string expected = System.IO.Path.Combine(root, "Images", "MyImage.png");
            string src = System.IO.Path.Combine(root, "PDFs", "MyStyles.pdfx");

            target.LoadedSource = src;

            string actual;
            actual = target.MapPath(path);
             Assert.AreEqual(expected.ToLower(), actual.ToLower());
            
            //http url

            path = @"../Images/MyImage.png";
            expected = @"http://www.scryber.co.uk/Documents/Images/MyImage.png";
            src = @"http://www.scryber.co.uk/Documents/PDFs/MyStyles.psfx";
            target.LoadedSource = src;

            actual = target.MapPath(path);
            Assert.AreEqual(expected.ToLower(), actual.ToLower());

            //http url - same directory

            path = @"MyImage.png";
            expected = @"http://www.scryber.co.uk/Documents/PDFs/MyImage.png";
            src = @"http://www.scryber.co.uk/Documents/PDFs/MyStyles.psfx";
            target.LoadedSource = src;

            actual = target.MapPath(path);
            Assert.AreEqual(expected.ToLower(), actual.ToLower());

            

            // image file path not relative

            path = root + separator + "Images"  + separator + "MyImage.png";
            expected = path;
            src = root + "Documents" + separator + "PDFs" + separator + "MyStyles.pdfx";
            target.LoadedSource = src;

            actual = target.MapPath(path);
            Assert.AreEqual(expected.ToLower(), actual.ToLower());

            // image web path not relative

            path = @"http://www.scryber.co.uk/Images/MyImage.png";
            expected = @"http://www.scryber.co.uk/Images/MyImage.png";
            src = @"http://www.scryber.co.uk/Documents/PDFs/MyStyles.psfx";
            target.LoadedSource = src;

            actual = target.MapPath(path);
            Assert.AreEqual(expected.ToLower(), actual.ToLower());

        }


        /// <summary>
        ///A test for MapPath using full urls
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void MapPathTest2()
        {
            // web url test

            PDFStylesDocument target = new PDFStylesDocument();
            target.LoadedSource = @"http://www.scryber.co.uk/Documents/PDFs/MyStyles.psfx";
            string source = @"/Documents/Images/MyImage.png";
            bool isfile = false;
            bool isfileExpected = false;
            string expected = @"http://www.scryber.co.uk/Documents/Images/MyImage.png";

            string actual;
            actual = target.MapPath(source, out isfile);
            Assert.AreEqual(isfileExpected, isfile);
            Assert.AreEqual(expected, actual);

            target.LoadedSource = @"http://192.1680.1:443/Documents/PDFs/MyStyles.psfx";
            source = @"/Images/MyImage.png";
            isfile = false;
            isfileExpected = false;
            expected = @"http://192.1680.1:443/Images/MyImage.png";

            actual = target.MapPath(source, out isfile);
            Assert.AreEqual(isfileExpected, isfile);
            Assert.AreEqual(expected, actual);

        }

        private void InitStylesDocument(PDFStylesDocument target)
        {
            PDFStyleDefn defn = new PDFStyleDefn();
            defn.AppliedType = typeof(PDFLabel);
            defn.AppliedClass = "sea";

            defn.Border.Color = PDFColors.Red;
            defn.Border.Width = 10;
            defn.Font.FontFamily = "Helvetica";
            target.Styles.Add(defn);

            PDFStyleDefn defn2 = new PDFStyleDefn();
            defn2.AppliedClass = "sea";

            defn2.Border.Color = PDFColors.Gray;
            defn2.Columns.ColumnCount = 3;
            target.Styles.Add(defn2);

            PDFStyleDefn defn3 = new PDFStyleDefn();
            defn3.AppliedClass = "other";
            defn3.Border.Width = 20;
            defn3.Stroke.Color = PDFColors.Aqua;
            target.Styles.Add(defn3);
        }


        /// <summary>
        ///A test for MergeInto
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void MergeIntoTest()
        {
            string classname = "sea";
            PDFStylesDocument target = new PDFStylesDocument();

            // same class, same type = applied
            PDFStyleDefn defn = new PDFStyleDefn();
            defn.AppliedType = typeof(PDFLabel);
            defn.AppliedClass = classname;

            defn.Border.Color = PDFColors.Red;
            defn.Border.Width = 10;
            defn.Font.FontFamily = "Helvetica";
            target.Styles.Add(defn);

            // same class no type = applied
            PDFStyleDefn defn2 = new PDFStyleDefn();
            defn2.AppliedClass = classname;

            defn2.Border.Color = PDFColors.Gray;
            defn2.Columns.ColumnCount = 3;
            target.Styles.Add(defn2);

            // different class = not applied
            PDFStyleDefn defn3 = new PDFStyleDefn();
            defn3.AppliedClass = "other";
            defn3.Border.Width = 20;
            defn3.Stroke.Color = PDFColors.Aqua;
            target.Styles.Add(defn3);

            //same class but different type = not applied
            PDFStyleDefn defn4 = new PDFStyleDefn();
            defn4.AppliedClass = classname;
            defn4.AppliedType = typeof(PDFImage);
            defn4.Font.FontFamily = "Symbol";
            target.Styles.Add(defn4);


            

            PDFStyle actual = new PDFStyle();
            PDFLabel lbl = new PDFLabel();
            lbl.StyleClass = classname;
            target.MergeInto(actual, lbl, ComponentState.Normal);
            actual.Flatten();

            Assert.AreEqual(PDFColors.Gray, actual.Border.Color);
            Assert.AreEqual((PDFUnit)10, actual.Border.Width);
            Assert.AreEqual(3, actual.Columns.ColumnCount);
            Assert.AreEqual("Helvetica", actual.Font.FontFamily);
        }

        /// <summary>
        ///A test for Document
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void DocumentTest()
        {
            PDFStylesDocument target = new PDFStylesDocument();
            PDFDocument root = new PDFDocument();
            root.Styles.Add(target);

            IPDFDocument actual;
            actual = target.Document;
            Assert.AreEqual(root, actual);
        }

        /// <summary>
        ///A test for ID
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void IDTest()
        {
            PDFStylesDocument target = new PDFStylesDocument();
            string expected = "MyStylesDocument";
            string actual;
            target.ID = expected;
            actual = target.ID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for LoadedSource
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void LoadedSourceTest()
        {
            PDFStylesDocument target = new PDFStylesDocument();
            string expected = @"C:\Documents\PDFs\MyStyles.psfx";
            string actual;
            target.LoadedSource = expected;
            actual = target.LoadedSource;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Parent
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void ParentTest()
        {
            PDFStylesDocument target = new PDFStylesDocument();
            PDFDocument expected = new PDFDocument();
            expected.Styles.Add(target);
            target.Parent = expected;

            IPDFComponent actual = target.Parent;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Styles
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void StylesTest()
        {
            PDFStylesDocument target = new PDFStylesDocument();
            PDFStyleCollection expected = target.Styles;
            Assert.IsNotNull(expected, "Styles collection is not initialized");
            Assert.AreEqual(target, expected.Owner, "Check the owner is set");

            expected = new PDFStyleCollection();
            target.Styles = expected;

            PDFStyleCollection actual = target.Styles;
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(actual.Owner, target, "Check the owner is reset");
           
        }
    }
}
