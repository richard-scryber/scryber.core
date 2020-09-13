using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber.Drawing;
using Scryber.Native;
using Scryber.Styles.Selectors;
using Scryber.Styles;
using Scryber.Components;

namespace Scryber.Core.UnitTests.Styles
{
    [TestClass()]
    public class PDFStyleMatcherPriorityTest
    {

        #region public TestContext TestContext

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

        #endregion


        [TestMethod]
        public void StylePrioritySingle_Test()
        {

            PDFStyleMatcher matcher = "doc:Div";
            Assert.AreEqual(1, matcher.Selector.Priority, "Element on it's own should be 1");

            matcher = ".red";
            Assert.AreEqual(2, matcher.Selector.Priority, "Class on it's own should be 2");

            matcher = ".red.blue";
            Assert.AreEqual(3, matcher.Selector.Priority, "multiple Classes on their own should be 3");

            matcher = ".red.blue.green";
            Assert.AreEqual(4, matcher.Selector.Priority, "3 Classes on their own should be 4");

            matcher = "#id";
            Assert.AreEqual(5, matcher.Selector.Priority, "ID on it's own should be 5");
        }

        [TestMethod]
        public void StylePriorityMultiple_Test()
        {

            PDFStyleMatcher matcher = "doc:Div.red";
            Assert.AreEqual(3, matcher.Selector.Priority, "Element and class should be 3");

            
            matcher = "doc:Div.red.blue";
            Assert.AreEqual(4, matcher.Selector.Priority, "Element and multiple Classes should be 4");

            matcher = "#id.red.blue";
            Assert.AreEqual(8, matcher.Selector.Priority, "ID with classes should be 8");
        }

        [TestMethod]
        public void StylePriorityCompound_Test()
        {

            PDFStyleMatcher matcher = "doc:Div .red";
            Assert.AreEqual(21, matcher.Selector.Priority, "Element and class should be 21");


            matcher = "doc:Div .red.blue";
            Assert.AreEqual(31, matcher.Selector.Priority, "Element and multiple Classes should be 31");

            matcher = "#id .red.blue";
            Assert.AreEqual(35, matcher.Selector.Priority, "ID with classes should be 35");

            matcher = "doc:Div .red.blue #id";
            Assert.AreEqual(531, matcher.Selector.Priority, "Element, multiple Classes and id should be 531");

            matcher = "#id .red.blue doc:Div";
            Assert.AreEqual(135, matcher.Selector.Priority, "ID, classes and element should be 135");

            matcher = "#id doc:Div.red.blue doc:Div.red";
            Assert.AreEqual(345, matcher.Selector.Priority, "ID, element with classes and element with classes should be 345");

            matcher = "#id doc:Div.red.blue doc:Div.red .green";
            Assert.AreEqual(2345, matcher.Selector.Priority, "ID, element with classes, element with classes and class should be 2345");
        }

        [TestMethod]
        public void StylePriorityCompoundParent_Test()
        {

            //Using a direct parent - we double that selector priority

            PDFStyleMatcher matcher = "doc:Div > .red";
            Assert.AreEqual(41, matcher.Selector.Priority, "Element and class should be 41 (20*2+ 1)");


            matcher = "doc:Div > .red.blue";
            Assert.AreEqual(61, matcher.Selector.Priority, "Element and multiple Classes should be 61 (20*3 + 1)");

            matcher = "#id > .red.blue";
            Assert.AreEqual(65, matcher.Selector.Priority, "ID with child classes should be 65");

            matcher = "doc:Div .red.blue > #id";
            Assert.AreEqual(1031, matcher.Selector.Priority, "Element, multiple Classes and child id should be 1031 ((200 * 5) + (10 * 3) + 1)");

        }

    }
}
