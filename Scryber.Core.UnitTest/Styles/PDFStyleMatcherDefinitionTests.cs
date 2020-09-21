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
    public class PDFStyleMatcherDefinitionTest
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
        public void SimpleIsMatch_Test()
        {
            int priority;
            var defn = new StyleDefn();
            defn.Match = PDFStyleMatcher.Parse(".red");


            var div = new Div();
            div.StyleClass = "red";

            var result = defn.IsMatchedTo(div, out priority);

            Assert.IsTrue(result, "The definition did not match the component");
        }

        [TestMethod]
        public void DualIsMatch_Test()
        {
            int priority;
            var defn = new StyleDefn();
            defn.Match = PDFStyleMatcher.Parse("doc:Div.red");


            var div = new Div();
            div.ID = "MyDiv";
            div.StyleClass = "red";
            div.ElementName = "doc:Div";

            var result = defn.IsMatchedTo(div, out priority);

            Assert.IsTrue(result, "The definition did not match the component");
        }



        [TestMethod]
        public void MultiClassIsMatch_Test()
        {
            int priority;

            var defn = new StyleDefn();
            defn.Match = "doc:Div.red";


            var div = new Div();

            div.StyleClass = "red blue";
            div.ElementName = "doc:Div";
            var result = defn.IsMatchedTo(div, out priority);
            Assert.IsTrue(result, "The definition did not match the component");

            div.StyleClass = "blue red";
            result = defn.IsMatchedTo(div, out priority);
            Assert.IsTrue(result, "The definition did not match the component");

            div.StyleClass = "blue red green";
            result = defn.IsMatchedTo(div, out priority);
            Assert.IsTrue(result, "The definition did not match the component");

        }

        [TestMethod]
        public void MultiClassIsNotMatch_Test()
        {
            int priority;

            var defn = new StyleDefn();
            defn.Match = "doc:Div.red";


            var div = new Div();
            div.ElementName = "doc:Div";

            div.StyleClass = "blue-red green";
            var result = defn.IsMatchedTo(div, out priority);
            Assert.IsFalse(result, "The definition matched the component");

            div.StyleClass = "blue red-green";
            result = defn.IsMatchedTo(div, out priority);
            Assert.IsFalse(result, "The definition matched the component");

            div.StyleClass = "blue-red";
            result = defn.IsMatchedTo(div, out priority);
            Assert.IsFalse(result, "The definition matched the component");

            div.StyleClass = "red-green";
            result = defn.IsMatchedTo(div, out priority);
            Assert.IsFalse(result, "The definition matched the component");

            div.StyleClass = "blue-red-green";
            result = defn.IsMatchedTo(div, out priority);
            Assert.IsFalse(result, "The definition matched the component");


        }

        [TestMethod]
        public void NestedClassMatch_Test()
        {
            int priority;

            var defn = new StyleDefn();
            defn.Match = ".red .blue";

            var div1 = new Div();
            div1.StyleClass = "red";

            var div2 = new Div();
            div2.StyleClass = "blue";
            div1.Contents.Add(div2);

            var result = defn.IsMatchedTo(div2, out priority);
            Assert.IsTrue(result, "The inner div was not matched as expected");


            //Switch the parent to .green and it should not match

            div1.StyleClass = "green";
            result = defn.IsMatchedTo(div2, out priority);
            Assert.IsFalse(result, "The inner div was matched - not as expected");
        }

        [TestMethod]
        public void DeepNestedClassMatch_Test()
        {
            int priority;

            var defn = new StyleDefn();
            defn.Match = "doc:Div#MyDiv doc:Para.blue";

            var div1 = new Div();
            div1.ElementName = "doc:Div";
            div1.ID = "MyDiv";
            div1.StyleClass = "red";

            var div2 = new Div();
            div2.ElementName = "doc:Div";
            div2.StyleClass = "blue";
            div1.Contents.Add(div2);

            var para = new Paragraph();
            para.ElementName = "doc:Para";
            para.StyleClass = "blue";
            div2.Contents.Add(para);

            var result = defn.IsMatchedTo(para, out priority);
            Assert.IsTrue(result, "The inner para was not matched as expected");

            //Switch the parent to .green and it should not match

            div1.StyleClass = "green";
            result = defn.IsMatchedTo(div2, out priority);
            Assert.IsFalse(result, "The inner para was matched - not as expected");
        }

        [TestMethod]
        public void DirectNestedClassMatch_Test()
        {
            int priority;
            var defn = new StyleDefn();
            defn.Match = "doc:Div.green > doc:Para.blue";

            var div1 = new Div();
            div1.ElementName = "doc:Div";
            div1.StyleClass = "red";

            var div2 = new Div();
            div2.ElementName = "doc:Div";
            div2.StyleClass = "green";
            div1.Contents.Add(div2);

            var para = new Paragraph();
            para.ElementName = "doc:Para";
            para.StyleClass = "blue";
            div2.Contents.Add(para);

            var result = defn.IsMatchedTo(para, out priority);
            Assert.IsTrue(result, "The inner para was not matched as expected");


            //Switch the parent to .red and parents parent to .green and it should not match
            //As not a direct parent

            div1.StyleClass = "green";
            div2.StyleClass = "red";

            result = defn.IsMatchedTo(div2, out priority);
            Assert.IsFalse(result, "The inner para was matched - not as expected");
        }


        [TestMethod]
        public void NestedDualClassMatch_Test()
        {
            int priority;

            var defn = new StyleDefn();
            defn.Match = "doc:Div.green > .blue.green";

            var div1 = new Div();
            div1.ElementName = "doc:Div";
            div1.StyleClass = "red";

            var div2 = new Div();
            div2.ElementName = "doc:Div";
            div2.StyleClass = "green";
            div1.Contents.Add(div2);

            var para = new Paragraph();
            para.ElementName = "doc:Para";
            para.StyleClass = "blue green";
            div2.Contents.Add(para);

            var result = defn.IsMatchedTo(para, out priority);
            Assert.IsTrue(result, "The inner para was not matched as expected");


            //Switch the parent to .red and parents parent to .green and it should not match
            //As not a direct parent

            div1.StyleClass = "green";
            div2.StyleClass = "red";

            result = defn.IsMatchedTo(div2, out priority);
            Assert.IsFalse(result, "The inner para was matched - not as expected");

            //Green back as direct parent, but only one class on the para

            div2.StyleClass = "green";
            para.StyleClass = "blue";

            result = defn.IsMatchedTo(div2, out priority);
            Assert.IsFalse(result, "The inner para was matched - not as expected");
        }

        [TestMethod]
        public void NestedMultipleClassMatch_Test()
        {
            int priority = 0;

            var defn = new StyleDefn();
            defn.Match = "doc:Div.green > doc:Para.green.red, doc:Div.red > doc:Para.blue.green";

            var div1 = new Div();
            div1.ElementName = "doc:Div";
            div1.StyleClass = "red";

            var div2 = new Div();
            div2.ElementName = "doc:Div";
            div2.StyleClass = "green";
            div1.Contents.Add(div2);

            var para = new Paragraph();
            para.ElementName = "doc:Para";
            para.StyleClass = "green red";
            div2.Contents.Add(para);

            //red > bulue and green
            var result = defn.IsMatchedTo(para, out priority);
            Assert.IsTrue(result, "The inner para was not matched as expected");


            //Switch the parent to .red and parents parent to .green and it should match on the second selector
            //As not a direct parent (classes reveresd order)

            div2.StyleClass = "red";
            para.StyleClass = "green blue";

            result = defn.IsMatchedTo(para, out priority);
            Assert.IsTrue(result, "The inner para was not matched as expected");

            //red back as direct parent, but only one class on the para

            div2.StyleClass = "red";
            para.StyleClass = "green";

            result = defn.IsMatchedTo(para, out priority);
            Assert.IsFalse(result, "The inner para was matched - not as expected");
        }


    }
}
