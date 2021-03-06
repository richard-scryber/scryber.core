﻿using Scryber.Styles;
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
            StylesDocument target = new StylesDocument();
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
            StylesDocument target = new StylesDocument(type);
            Assert.AreEqual(type, target.Type);
        }


        /// <summary>
        ///A test for Dispose
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void DisposeTest()
        {
            StylesDocument target = new StylesDocument(); 
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
            StylesDocument target = new StylesDocument(); // TODO: Initialize to an appropriate value
            PDFInitContext context = new PDFInitContext(new PDFItemCollection(null), 
                new Logging.DoNothingTraceLog(TraceRecordLevel.Off), new PDFPerformanceMonitor(true), null);
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
            
            StylesDocument target = new StylesDocument();
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

            StylesDocument target = new StylesDocument();
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

        private void InitStylesDocument(StylesDocument target)
        {
            StyleDefn defn = new StyleDefn();
            defn.AppliedType = typeof(Label);
            defn.AppliedClass = "sea";

            defn.Border.Color = PDFColors.Red;
            defn.Border.Width = 10;
            defn.Font.FontFamily = (PDFFontSelector)"Helvetica";
            target.Styles.Add(defn);

            StyleDefn defn2 = new StyleDefn();
            defn2.AppliedClass = "sea";

            defn2.Border.Color = PDFColors.Gray;
            defn2.Columns.ColumnCount = 3;
            target.Styles.Add(defn2);

            StyleDefn defn3 = new StyleDefn();
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
            StylesDocument target = new StylesDocument();

            // same class, same type = applied
            StyleDefn defn = new StyleDefn();
            defn.AppliedType = typeof(Label);
            defn.AppliedClass = classname;

            defn.Border.Color = PDFColors.Red;
            defn.Border.Width = 10;
            defn.Font.FontFamily = (PDFFontSelector)"Helvetica";
            target.Styles.Add(defn);

            // same class no type = applied (lower priority)
            StyleDefn defn2 = new StyleDefn();
            defn2.AppliedClass = classname;

            defn2.Border.Color = PDFColors.Gray;
            defn2.Columns.ColumnCount = 3;
            target.Styles.Add(defn2);

            // different class = not applied
            StyleDefn defn3 = new StyleDefn();
            defn3.AppliedClass = "other";
            defn3.Border.Width = 20;
            defn3.Stroke.Color = PDFColors.Aqua;
            target.Styles.Add(defn3);

            //same class but different type = not applied
            StyleDefn defn4 = new StyleDefn();
            defn4.AppliedClass = classname;
            defn4.AppliedType = typeof(Image);
            defn4.Font.FontFamily = (PDFFontSelector)"Symbol";
            target.Styles.Add(defn4);


            

            Style actual = new Style();
            Label lbl = new Label();
            lbl.StyleClass = classname;
            target.MergeInto(actual, lbl, ComponentState.Normal);
            actual.Flatten();

            Assert.AreEqual(PDFColors.Red, actual.Border.Color); //from defn (higher priority than defn2)
            Assert.AreEqual((PDFUnit)10, actual.Border.Width); // from defn
            Assert.AreEqual(3, actual.Columns.ColumnCount); //from defn2 
            Assert.AreEqual((PDFFontSelector)"Helvetica", actual.Font.FontFamily);
        }

        /// <summary>
        ///A test for Document
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void DocumentTest()
        {
            StylesDocument target = new StylesDocument();
            Document root = new Document();
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
            StylesDocument target = new StylesDocument();
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
            StylesDocument target = new StylesDocument();
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
            StylesDocument target = new StylesDocument();
            Document expected = new Document();
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
            StylesDocument target = new StylesDocument();
            StyleCollection expected = target.Styles;
            Assert.IsNotNull(expected, "Styles collection is not initialized");
            Assert.AreEqual(target, expected.Owner, "Check the owner is set");

            expected = new StyleCollection();
            target.Styles = expected;

            StyleCollection actual = target.Styles;
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(actual.Owner, target, "Check the owner is reset");
           
        }
    }
}
