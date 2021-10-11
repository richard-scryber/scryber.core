using Scryber.Styles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber;

namespace Scryber.Core.UnitTests.Styles
{
    
    
    /// <summary>
    ///This is a test class for PDFStyleDefnTest and is intended
    ///to contain all PDFStyleDefnTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFStyleDefnTest
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
        ///A test for PDFStyleDefn Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void PDFStyleDefnConstructorTest()
        {
            StyleDefn target = new StyleDefn();
            Assert.IsNotNull(target);
            Assert.AreEqual(false, target.HasValues);
            Assert.IsTrue(string.IsNullOrEmpty(target.AppliedClass));
            Assert.IsTrue(string.IsNullOrEmpty(target.AppliedID));
            Assert.IsNull(target.AppliedType);
            Assert.AreEqual(ComponentState.Normal, target.AppliedState);
        }

        /// <summary>
        ///A test for PDFStyleDefn Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void PDFStyleDefnConstructorTest1()
        {
            Type appliedtype = typeof(Scryber.Components.Label);
            string appliedid = "myid";
            string appliedclassname = "myclass";
            StyleDefn target = new StyleDefn(appliedtype, appliedid, appliedclassname);

            Assert.IsNotNull(target);
            Assert.IsFalse(target.HasValues);
            Assert.AreEqual(appliedclassname, target.AppliedClass);
            Assert.AreEqual(appliedid, target.AppliedID);
            Assert.AreEqual(appliedtype, target.AppliedType);
            Assert.AreEqual(ComponentState.Normal, target.AppliedState);
        }


        /// <summary>
        ///A test for IsMatchedTo
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void IsMatchedToTest()
        {
            StyleDefn target = new StyleDefn();

            //Catch All with document
            int priority;

            Scryber.Components.Document doc = new Components.Document();
            bool expected = true;
            bool actual;
            actual = target.IsMatchedTo(doc, ComponentState.Normal, out priority);
            Assert.AreEqual(expected, actual, "Didn't match document on catch all");

            //set up component

            Scryber.Components.Label lbl = new Scryber.Components.Label();
            lbl.StyleClass = "myclass";
            lbl.ID = "myId";

            
            //Catch all should always match

            expected = true;
            actual = target.IsMatchedTo(lbl, ComponentState.Normal, out priority);
            Assert.AreEqual(expected, actual, GetIsMatchedToMessage(1,expected));

            //match applied type
            target.AppliedType = typeof(Scryber.Components.Label);
            expected = true;
            actual = target.IsMatchedTo(lbl, ComponentState.Normal, out priority);
            Assert.AreEqual(expected, actual, GetIsMatchedToMessage(2, expected));

            //match applied type and class
            target.AppliedClass = "myclass";
            expected = true;
            actual = target.IsMatchedTo(lbl, ComponentState.Normal, out priority);
            Assert.AreEqual(expected, actual, GetIsMatchedToMessage(3, expected));

            //match applied type, class and id
            target.AppliedID = "myId";
            expected = true;
            actual = target.IsMatchedTo(lbl, ComponentState.Normal, out priority);
            Assert.AreEqual(expected, actual, GetIsMatchedToMessage(4, expected));

            //match class and id
            target.AppliedType = null;
            expected = true;
            actual = target.IsMatchedTo(lbl, ComponentState.Normal, out priority);
            Assert.AreEqual(expected, actual, GetIsMatchedToMessage(5, expected));

            //match id
            target.AppliedClass = string.Empty;
            expected = true;
            actual = target.IsMatchedTo(lbl, ComponentState.Normal, out priority);
            Assert.AreEqual(expected, actual, GetIsMatchedToMessage(6, expected));

            //match base type and id
            target.AppliedType = typeof(Scryber.Components.SpanBase);
            expected = true;
            actual = target.IsMatchedTo(lbl, ComponentState.Normal, out priority);
            Assert.AreEqual(expected, actual, GetIsMatchedToMessage(7, expected));

            //multiple defined style classes
            lbl.StyleClass = "other myclass";
            target.AppliedClass = "myclass";
            expected = true;
            actual = target.IsMatchedTo(lbl, ComponentState.Normal, out priority);
            Assert.AreEqual(expected, actual, GetIsMatchedToMessage(8, expected));

            //non-matched class, matched type and id
            lbl.StyleClass = "myclass";
            target.AppliedClass = "other";
            expected = false;
            actual = target.IsMatchedTo(lbl, ComponentState.Normal, out priority);
            Assert.AreEqual(expected, actual, GetIsMatchedToMessage(9, expected));

            //non-matched type, matched class and id
            target.AppliedType = typeof(Scryber.Components.TableCell);
            target.AppliedClass = "myclass";
            expected = false;
            actual = target.IsMatchedTo(lbl, ComponentState.Normal, out priority);
            Assert.AreEqual(expected, actual, GetIsMatchedToMessage(10, expected));

            //non-matched id, matched class
            target.AppliedType = null;
            lbl.ID = "otherID";
            expected = false;
            actual = target.IsMatchedTo(lbl, ComponentState.Normal, out priority);
            Assert.AreEqual(expected, actual, GetIsMatchedToMessage(11, expected));

            
        }

        private string GetIsMatchedToMessage(int index, bool expected)
        {
            string message;
            if (expected)
                message = index.ToString() + ": Expected True, was false.";
            else
                message = index.ToString() + ": Expected False, was true.";

            return message;
        }


        /// <summary>
        ///A test for MergeInto
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void MergeIntoTest()
        {
            Scryber.Components.Label lbl = new Scryber.Components.Label();
            lbl.StyleClass = "myclass";
            lbl.ID = "myId";

            StyleDefn target = new StyleDefn();
            target.Background.Color = Scryber.Drawing.StandardColors.Aqua;

            Style style = new Style();
            style.Background.Color = Scryber.Drawing.StandardColors.Blue;

            //matched should have aqua applied, non-matching should stay blue


            //match applied type
            target.AppliedType = typeof(Scryber.Components.Label);
            target.MergeInto(style, lbl, ComponentState.Normal);

            style.Flatten();
            Assert.AreEqual(Scryber.Drawing.StandardColors.Aqua, style.Background.Color);

            //non-matching applied type
            target.AppliedType = typeof(Scryber.Components.TableCell);
            style = new Style();
            style.Background.Color = Scryber.Drawing.StandardColors.Blue;

            target.MergeInto(style, lbl, ComponentState.Normal);
            Assert.AreEqual(Scryber.Drawing.StandardColors.Blue, style.Background.Color);

        }

        /// <summary>
        ///A test for AppliedClass
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void AppliedClassTest()
        {
            StyleDefn target = new StyleDefn();
            string expected = "myclass";
            string actual;
            target.AppliedClass = expected;
            actual = target.AppliedClass;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for AppliedID
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void AppliedIDTest()
        {
            StyleDefn target = new StyleDefn();
            string expected = "myID";
            string actual;
            target.AppliedID = expected;
            actual = target.AppliedID;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for AppliedState
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void AppliedStateTest()
        {
            StyleDefn target = new StyleDefn();
            ComponentState expected = ComponentState.Down;
            ComponentState actual;
            target.AppliedState = expected;
            actual = target.AppliedState;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for AppliedType
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void AppliedTypeTest()
        {
            StyleDefn target = new StyleDefn();
            Type expected = typeof(Scryber.Components.TableCell);
            Type actual;
            target.AppliedType = expected;
            actual = target.AppliedType;
            Assert.AreEqual(expected, actual);
            
        }
    }
}
