using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber.Drawing;
using Scryber.PDF.Native;
using Scryber.Styles.Selectors;
using Scryber.Styles;
using Scryber.Components;

namespace Scryber.Core.UnitTests.Styles
{
    [TestClass()]
    public class PDFStyleMatcherTest
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


        [TestMethod()]
        public void StyleMatcherSimpleParsing_Test()
        {
            string test = ".red";

            var parsed = StyleMatcher.Parse(test);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Selector);
            Assert.AreEqual(".red", parsed.Selector.AppliedClass.ToString(), "Class Test failed");
            Assert.IsNull(parsed.Selector.AppliedID, "Class Test failed");
            Assert.IsNull(parsed.Selector.AppliedElement, "Class Test failed");
            Assert.IsNull(parsed.Selector.Ancestor, "Class Test failed");
            Assert.IsFalse(parsed.Selector.HasAncestor, "Class Test failed");
            Assert.AreEqual(parsed.Selector.Placement, StylePlacement.Any, "Class Test failed");

            Assert.AreEqual(test, parsed.ToString(), "ToString did not match");

            test = "#NewComponentID";

            parsed = StyleMatcher.Parse(test);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Selector);
            Assert.AreEqual("NewComponentID", parsed.Selector.AppliedID,"ID Test failed");
            Assert.IsNull(parsed.Selector.AppliedClass, "ID Test failed");
            Assert.IsNull(parsed.Selector.AppliedElement, "ID Test failed");
            Assert.IsNull(parsed.Selector.Ancestor, "ID Test failed");
            Assert.IsFalse(parsed.Selector.HasAncestor, "ID Test failed");
            Assert.AreEqual(parsed.Selector.Placement, StylePlacement.Any, "ID Test failed");

            Assert.AreEqual(test, parsed.ToString(), "ToString did not match");

            test = "doc:Component";

            parsed = StyleMatcher.Parse(test);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Selector);
            Assert.AreEqual("doc:Component", parsed.Selector.AppliedElement, "Type Test failed");
            Assert.IsNull(parsed.Selector.AppliedClass, "Type Test failed");
            Assert.IsNull(parsed.Selector.AppliedID, "Type Test failed");
            Assert.IsNull(parsed.Selector.Ancestor, "Type Test failed");
            Assert.IsFalse(parsed.Selector.HasAncestor, "Type Test failed");
            Assert.AreEqual(parsed.Selector.Placement, StylePlacement.Any, "Type Test failed");

            Assert.AreEqual(test, parsed.ToString(), "ToString did not match");
        }

        [TestMethod()]
        public void StyleMatcherDualParsing_Test()
        {
            string test = "doc:Table.red";

            var parsed = StyleMatcher.Parse(test);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Selector);
            Assert.AreEqual(".red", parsed.Selector.AppliedClass.ToString(), "Type.Class Test failed");
            Assert.IsNull(parsed.Selector.AppliedID, "Type.Class Test failed");
            Assert.AreEqual("doc:Table", parsed.Selector.AppliedElement, "Type.Class Test failed");
            Assert.IsNull(parsed.Selector.Ancestor, "Type.Class Test failed");
            Assert.IsFalse(parsed.Selector.HasAncestor, "Type.Class Test failed");
            Assert.AreEqual(parsed.Selector.Placement, StylePlacement.Any, "Type.Class Test failed");

            Assert.AreEqual(test, parsed.ToString(), "ToString did not match");

            test = "doc:Div#NewComponentID";

            parsed = StyleMatcher.Parse(test);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Selector);
            Assert.AreEqual("NewComponentID", parsed.Selector.AppliedID, "Type.ID Test failed");
            Assert.IsNull(parsed.Selector.AppliedClass, "Type.ID Test failed");
            Assert.AreEqual("doc:Div",parsed.Selector.AppliedElement, "Type.ID Test failed");
            Assert.IsNull(parsed.Selector.Ancestor, "Type.ID Test failed");
            Assert.IsFalse(parsed.Selector.HasAncestor, "Type.ID Test failed");
            Assert.AreEqual(parsed.Selector.Placement, StylePlacement.Any, "Type.ID Test failed");

            Assert.AreEqual(test, parsed.ToString(), "ToString did not match");

            test = ".red#ComponentID";

            parsed = StyleMatcher.Parse(test);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Selector);
            Assert.IsNull(parsed.Selector.AppliedElement, "Class.Type Test failed");
            Assert.AreEqual(".red",parsed.Selector.AppliedClass.ToString(), "Class.Type Test failed");
            Assert.AreEqual("ComponentID",parsed.Selector.AppliedID, "Class.Type Test failed");
            Assert.IsNull(parsed.Selector.Ancestor, "Class.Type Test failed");
            Assert.IsFalse(parsed.Selector.HasAncestor, "Class.Type Test failed");
            Assert.AreEqual(parsed.Selector.Placement, StylePlacement.Any, "Class.Type Test failed");

            Assert.AreEqual(test, parsed.ToString(), "ToString did not match");

            test = ".red.blue";

            parsed = StyleMatcher.Parse(test);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Selector);
            Assert.IsNull(parsed.Selector.AppliedElement, "Class.Class Test failed");
            Assert.IsNull(parsed.Selector.AppliedID, "Class.Class Test failed");
            Assert.AreEqual(".red.blue", parsed.Selector.AppliedClass.ToString(), "Class.Class Test failed");

            //Check the class matcher values - stored in reverse order
            Assert.AreEqual("blue", parsed.Selector.AppliedClass.ClassName);
            Assert.IsNotNull(parsed.Selector.AppliedClass.AndClass);
            Assert.AreEqual("red", parsed.Selector.AppliedClass.AndClass.ClassName);
            Assert.IsNull(parsed.Selector.AppliedClass.AndClass.AndClass);


            Assert.AreEqual(test, parsed.ToString(), "ToString did not match");
        }

        /// <summary>
        /// Tests the simple multiple 
        /// </summary>
        [TestMethod]
        public void StyleMatcherSimpleAncestor_Test()
        {
            string test = ".blue .red";

            var parsed = StyleMatcher.Parse(test);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Selector);

            //Because these are parsed in reverse order, then they need to be
            Assert.AreEqual(".red", parsed.Selector.AppliedClass.ToString(), ".Class .Class Test failed");
            Assert.IsNull(parsed.Selector.AppliedID, ".Class .Class Test failed");
            Assert.IsNull(parsed.Selector.AppliedElement, ".Class .Class Test failed");

            Assert.IsNotNull(parsed.Selector.Ancestor, ".Class.Class Test failed");
            Assert.IsTrue(parsed.Selector.HasAncestor, ".Class .Class Test failed");
            Assert.AreEqual(parsed.Selector.Placement, StylePlacement.Any, ".Class .Class Test failed");


            Assert.AreEqual(".blue", parsed.Selector.Ancestor.AppliedClass.ToString(), ".Class .Class Test failed");
            Assert.IsNull(parsed.Selector.Ancestor.AppliedID, ".Class .Class Test failed");
            Assert.IsNull(parsed.Selector.Ancestor.AppliedElement, ".Class .Class Test failed");
            Assert.AreEqual(parsed.Selector.Ancestor.Placement, StylePlacement.Any, ".Class .Class Test failed");

            Assert.AreEqual(test, parsed.ToString(), "ToString did not match");

            test = "doc:Table.red doc:Cell.blue";

            parsed = StyleMatcher.Parse(test);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Selector);

            //Because these are parsed in reverse order, then they need to be
            Assert.AreEqual(".blue", parsed.Selector.AppliedClass.ToString(), "Element.Class Element.Class Test failed");
            Assert.IsNull(parsed.Selector.AppliedID, "Element.Class Element.Class Test failed");
            Assert.AreEqual("doc:Cell", parsed.Selector.AppliedElement, "Element.Class Element.Class Test failed");

            Assert.IsNotNull(parsed.Selector.Ancestor, "Element.ClassElement.Class Test failed");
            Assert.IsTrue(parsed.Selector.HasAncestor, "Element.Class Element.Class Test failed");
            Assert.AreEqual(parsed.Selector.Placement, StylePlacement.Any, "Element.Class Element.Class Test failed");


            Assert.AreEqual(".red", parsed.Selector.Ancestor.AppliedClass.ToString(), "Element.Class Element.Class Test failed");
            Assert.IsNull(parsed.Selector.Ancestor.AppliedID, "Element.Class Element.Class Test failed");
            Assert.AreEqual("doc:Table",parsed.Selector.Ancestor.AppliedElement, "Element.Class Element.Class Test failed");
            Assert.AreEqual(parsed.Selector.Ancestor.Placement, StylePlacement.Any, "Element.Class Element.Class Test failed");

            Assert.AreEqual(test, parsed.ToString(), "ToString did not match");

            test = "doc:Table.red.green doc:Cell.blue";

            parsed = StyleMatcher.Parse(test);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Selector);

            //Because these are parsed in reverse order, then they need to be
            Assert.AreEqual(".blue", parsed.Selector.AppliedClass.ToString(), "Element.Class.Class Element.Class Test failed");
            Assert.IsNull(parsed.Selector.AppliedID, "Element.Class.Class Element.Class Test failed");
            Assert.AreEqual("doc:Cell", parsed.Selector.AppliedElement, "Element.Class.Class Element.Class Test failed");

            Assert.IsNotNull(parsed.Selector.Ancestor, "Element.Class.Class Element.Class Test failed");
            Assert.IsTrue(parsed.Selector.HasAncestor, "Element.Class.Class Element.Class Test failed");
            Assert.AreEqual(parsed.Selector.Placement, StylePlacement.Any, "Element.Class.Class Element.Class Test failed");


            Assert.AreEqual(".red.green", parsed.Selector.Ancestor.AppliedClass.ToString(), "Element.Class.Class Element.Class Test failed");
            Assert.AreEqual("green", parsed.Selector.Ancestor.AppliedClass.ClassName, "Element.Class.Class Element.Class Test failed");
            Assert.AreEqual("red", parsed.Selector.Ancestor.AppliedClass.AndClass.ClassName, "Element.Class.Class Element.Class Test failed");

            Assert.IsNull(parsed.Selector.Ancestor.AppliedID, "Element.Class.Class Element.Class Test failed");
            Assert.AreEqual("doc:Table", parsed.Selector.Ancestor.AppliedElement, "Element.Class.Class Element.Class Test failed");
            Assert.AreEqual(parsed.Selector.Ancestor.Placement, StylePlacement.Any, "Element.Class.Class Element.Class Test failed");

            Assert.AreEqual(test, parsed.ToString(), "ToString did not match");
        }


        /// <summary>
        /// Tests the Parent selector in css, the > separator
        /// </summary>
        [TestMethod]
        public void StyleMatcherParentAncestor_Test()
        {
            string test = ".blue > .red";

            var parsed = StyleMatcher.Parse(test);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Selector);

            //Because these are parsed in reverse order, then they need to be
            Assert.AreEqual(".red", parsed.Selector.AppliedClass.ToString(), ".Class .Class Test failed");
            Assert.IsNull(parsed.Selector.AppliedID, ".Class .Class Test failed");
            Assert.IsNull(parsed.Selector.AppliedElement, ".Class .Class Test failed");

            Assert.IsNotNull(parsed.Selector.Ancestor, ".Class.Class Test failed");
            Assert.IsTrue(parsed.Selector.HasAncestor, ".Class .Class Test failed");
            Assert.AreEqual(parsed.Selector.Placement, StylePlacement.Any, ".Class .Class Test failed");


            Assert.AreEqual(".blue", parsed.Selector.Ancestor.AppliedClass.ToString(), ".Class .Class Test failed");
            Assert.IsNull(parsed.Selector.Ancestor.AppliedID, ".Class .Class Test failed");
            Assert.IsNull(parsed.Selector.Ancestor.AppliedElement, ".Class .Class Test failed");
            Assert.AreEqual(parsed.Selector.Ancestor.Placement, StylePlacement.DirectParent, ".Class .Class Test failed");

            Assert.AreEqual(test, parsed.ToString(), "ToString did not match");

            test = "doc:Page.red > doc:Div#MyDiv";

            parsed = StyleMatcher.Parse(test);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Selector);

            //Because these are parsed in reverse order, then they need to be
            Assert.IsNull(parsed.Selector.AppliedClass, "Element.Class > Element.ID Test failed");
            Assert.AreEqual("MyDiv", parsed.Selector.AppliedID, "Element.Class > Element.ID Test failed");
            Assert.AreEqual("doc:Div", parsed.Selector.AppliedElement, "Element.Class > Element.ID Test failed");

            Assert.IsNotNull(parsed.Selector.Ancestor, "Element.Class > Element.ID Test failed");
            Assert.IsTrue(parsed.Selector.HasAncestor, "Element.Class > Element.ID Test failed");
            Assert.AreEqual(parsed.Selector.Placement, StylePlacement.Any, "Element.Class > Element.ID Test failed");


            Assert.AreEqual(".red", parsed.Selector.Ancestor.AppliedClass.ToString(), "Element.Class > Element.ID Test failed");
            Assert.IsNull(parsed.Selector.Ancestor.AppliedID, "Element.Class > Element.ID Test failed");
            Assert.AreEqual("doc:Page", parsed.Selector.Ancestor.AppliedElement, "Element.Class > Element.ID Test failed");
            Assert.AreEqual(parsed.Selector.Ancestor.Placement, StylePlacement.DirectParent, "Element.Class > Element.ID Test failed");

            Assert.AreEqual(test, parsed.ToString(), "ToString did not match");

            test = "doc:Page.red > doc:Table doc:Cell.alt.green doc:Date";

            parsed = StyleMatcher.Parse(test);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Selector);

            StyleSelector match = parsed.Selector;

            //Because these are parsed in reverse order, then they need to be read in that order

            //doc:Date
            Assert.IsNull(match.AppliedClass, "Element.Class > Element Element.Class Element Test failed");
            Assert.IsNull(match.AppliedID, "Element.Class > Element Element.Class Element Test failed");
            Assert.AreEqual("doc:Date", match.AppliedElement, "Element.Class > Element Element.Class Element Test failed");

            Assert.IsNotNull(match.Ancestor, "Element.Class > Element Element.Class Element Test failed");
            Assert.IsTrue(match.HasAncestor, "Element.Class > Element Element.Class Element Test failed");
            Assert.AreEqual(match.Placement, StylePlacement.Any, "Element.Class > Element Element.Class Element Test failed");

            //doc:Cell.alt
            match = match.Ancestor;
            Assert.AreEqual(".alt.green", match.AppliedClass.ToString(), "Element.Class > Element Element.Class Element Test failed");
            Assert.IsNull(match.AppliedID, "Element.Class > Element Element.Class Element Test failed");
            Assert.AreEqual("doc:Cell", match.AppliedElement, "Element.Class > Element Element.Class Element Test failed");

            Assert.IsNotNull(match.Ancestor, "Element.Class > Element Element.Class Element Test failed");
            Assert.IsTrue(match.HasAncestor, "Element.Class > Element Element.Class Element Test failed");
            Assert.AreEqual(match.Placement, StylePlacement.Any, "Element.Class > Element Element.Class Element Test failed");

            //doc.Table
            match = match.Ancestor;
            Assert.IsNull(match.AppliedClass, "Element.Class > Element Element.Class Element Test failed");
            Assert.IsNull(match.AppliedID, "Element.Class > Element Element.Class Element Test failed");
            Assert.AreEqual("doc:Table", match.AppliedElement, "Element.Class > Element Element.Class Element Test failed");

            Assert.IsNotNull(match.Ancestor, "Element.Class > Element Element.Class Element Test failed");
            Assert.IsTrue(match.HasAncestor, "Element.Class > Element Element.Class Element Test failed");
            Assert.AreEqual(match.Placement, StylePlacement.Any, "Element.Class > Element Element.Class Element Test failed");

            //doc:Page.red >
            match = match.Ancestor;
            Assert.AreEqual(".red", match.AppliedClass.ToString(), "Element.Class > Element Element.Class Element Test failed");
            Assert.IsNull(match.AppliedID, "Element.Class > Element Element.Class Element Test failed");
            Assert.AreEqual("doc:Page", match.AppliedElement, "Element.Class > Element Element.Class Element Test failed");
            Assert.AreEqual(match.Placement, StylePlacement.DirectParent, "Element.Class > Element Element.Class Element Test failed");

            Assert.AreEqual(test, parsed.ToString(), "ToString did not match");
        }


        /// <summary>
        /// Tests the definition of multiple styles in a single string (separated by a comma)
        /// </summary>
        [TestMethod()]
        public void StyleMatcherMultipleParsing_Test()
        {
            string test = "doc:Table.red, doc:Cell.red";

            var parsed = StyleMatcher.Parse(test);

            Assert.IsInstanceOfType(parsed, typeof(StyleMultipleMatcher));
            Assert.AreEqual(test, parsed.ToString(), "ToString did not match");

            var multi = (StyleMultipleMatcher)parsed;

            Assert.IsNotNull(multi);
            Assert.IsNotNull(multi.Selector);
            Assert.IsNotNull(multi.Next);

            Assert.AreEqual(".red", parsed.Selector.AppliedClass.ToString(), "Type.Class Test failed");
            Assert.IsNull(parsed.Selector.AppliedID, "Type.Class Test failed");
            Assert.AreEqual("doc:Cell", parsed.Selector.AppliedElement, "Type.Class Test failed");
            Assert.IsNull(parsed.Selector.Ancestor, "Type.Class Test failed");
            Assert.IsFalse(parsed.Selector.HasAncestor, "Type.Class Test failed");
            Assert.AreEqual(parsed.Selector.Placement, StylePlacement.Any, "Type.Class Test failed");

            parsed = multi.Next;

            Assert.AreEqual(".red", parsed.Selector.AppliedClass.ToString(), "Type.Class Test failed");
            Assert.IsNull(parsed.Selector.AppliedID, "Type.Class Test failed");
            Assert.AreEqual("doc:Table", parsed.Selector.AppliedElement, "Type.Class Test failed");
            Assert.IsNull(parsed.Selector.Ancestor, "Type.Class Test failed");
            Assert.IsFalse(parsed.Selector.HasAncestor, "Type.Class Test failed");
            Assert.AreEqual(parsed.Selector.Placement, StylePlacement.Any, "Type.Class Test failed");

            //3 with last complex

            test = "doc:Table.blue, doc:Cell.red, doc:Page.red.green > doc:Div#MyDiv";

            parsed = StyleMatcher.Parse(test);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Selector);
            Assert.AreEqual(test, parsed.ToString(), "ToString did not match");



            //Because these are parsed in reverse order, then they need to be tested in reverse order

            Assert.IsNull(parsed.Selector.AppliedClass, "First Tripple style Test failed");
            Assert.AreEqual("MyDiv", parsed.Selector.AppliedID, "First Tripple style Test failed");
            Assert.AreEqual("doc:Div", parsed.Selector.AppliedElement, "First Tripple style Test failed");

            Assert.IsNotNull(parsed.Selector.Ancestor, "First Tripple style Test failed");
            Assert.IsTrue(parsed.Selector.HasAncestor, "First Tripple style Test failed");
            Assert.AreEqual(parsed.Selector.Placement, StylePlacement.Any, "First Tripple style Test failed");


            Assert.AreEqual(".red.green", parsed.Selector.Ancestor.AppliedClass.ToString(), "First Tripple style Test failed");
            Assert.IsNull(parsed.Selector.Ancestor.AppliedID, "First Tripple style Test failed");
            Assert.AreEqual("doc:Page", parsed.Selector.Ancestor.AppliedElement, "First Tripple style Test failed");
            Assert.AreEqual(parsed.Selector.Ancestor.Placement, StylePlacement.DirectParent, "First Tripple style Test failed");

            //Move to the red cell
            Assert.IsInstanceOfType(parsed, typeof(StyleMultipleMatcher));
            multi = (StyleMultipleMatcher)parsed;
            parsed = multi.Next;

            Assert.AreEqual(".red", parsed.Selector.AppliedClass.ToString(), "Second Tripple style Test failed");
            Assert.IsNull(parsed.Selector.AppliedID, "Second Tripple style Test failed");
            Assert.AreEqual("doc:Cell", parsed.Selector.AppliedElement, "Second Tripple style Test failed");
            Assert.IsNull(parsed.Selector.Ancestor, "Second Tripple style Test failed");
            Assert.IsFalse(parsed.Selector.HasAncestor, "Second Tripple style Test failed");
            Assert.AreEqual(parsed.Selector.Placement, StylePlacement.Any, "Second Tripple style Test failed");

            //Move to the blue table
            Assert.IsInstanceOfType(parsed, typeof(StyleMultipleMatcher));
            multi = (StyleMultipleMatcher)parsed;
            parsed = multi.Next;

            Assert.AreEqual(".blue", parsed.Selector.AppliedClass.ToString(), "Third Tripple style Test failed");
            Assert.IsNull(parsed.Selector.AppliedID, "Third Tripple style Test failed");
            Assert.AreEqual("doc:Table", parsed.Selector.AppliedElement, "Third Tripple style Test failed");
            Assert.IsNull(parsed.Selector.Ancestor, "Third Tripple style Test failed");
            Assert.IsFalse(parsed.Selector.HasAncestor, "Third Tripple style Test failed");
            Assert.AreEqual(parsed.Selector.Placement, StylePlacement.Any, "Third Tripple style Test failed");


        }
        
        [TestMethod]
        public void StyleMatcherConversion_Test()
        {
            StyleMatcher matcher;
            string test;

            test = ".red";
            matcher = test;

            Assert.AreEqual(test, matcher.ToString(), "Conversion and back ToString did not match");

            test = ".red, doc:Div#mydiv > doc:Table > doc:Cell.red";
            matcher = test;

            Assert.AreEqual(test, matcher.ToString(), "Complex conversion and back ToString did not match");
        }

        [TestMethod]
        public void StyleMatcherMultiple_Test()
        {
            StyleMatcher red = ".red";

            Div div = new Div();
            div.StyleClass = "red";
            int priority;

            bool result = red.IsMatchedTo(div, ComponentState.Normal, out priority);
            Assert.IsTrue(result, "Did not match on the applied class of " + div.StyleClass);

            div.StyleClass = "red bordered";

            result = red.IsMatchedTo(div, ComponentState.Normal, out priority);
            Assert.IsTrue(result, "Did not match on the applied class of " + div.StyleClass);

            div.StyleClass = "bordered red";

            result = red.IsMatchedTo(div, ComponentState.Normal, out priority);
            Assert.IsTrue(result, "Did not match on the applied class of " + div.StyleClass);

            div.StyleClass = "redish red";

            result = red.IsMatchedTo(div, ComponentState.Normal, out priority);
            Assert.IsTrue(result, "Did not match on the applied class of " + div.StyleClass);

            div.StyleClass = "redish red bordered";

            result = red.IsMatchedTo(div, ComponentState.Normal, out priority);
            Assert.IsTrue(result, "Did not match on the applied class of " + div.StyleClass);

            div.StyleClass = "redish reder bordered";

            result = red.IsMatchedTo(div, ComponentState.Normal, out priority);
            Assert.IsFalse(result, "Should not have matched on the applied class of " + div.StyleClass);

            div.StyleClass = "redred red";

            result = red.IsMatchedTo(div, ComponentState.Normal, out priority);
            Assert.IsTrue(result, "Did not match on the applied class of " + div.StyleClass);

        }

    }
}
