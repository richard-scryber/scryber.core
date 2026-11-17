using Scryber.Styles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber;
using Scryber.Drawing;
using Scryber.Components;
using System.Linq;
using System.Collections.Generic;
using Scryber.Html.Components;

namespace Scryber.Core.UnitTests.Styles
{
    /// <summary>
    /// Tests for the StyleIndexTree 
    /// </summary>
	[TestClass()]
    public class PDFStyleIndexTest
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



        public PDFStyleIndexTest()
        {
        }

        //
        // single values
        //

        [TestMethod()]
        public void StyleIndex_AddStyleClassDefinition_Test()
        {
            var cName = "className";

            //Add a single definition with a selector of a classname and check it is returned from the index for a matching component
            var defn = new StyleDefn("." + cName);
            var tree = new StyleRootIndexTree();

            tree.AddStyle(defn);

            var lbl = new Label() { StyleClass = cName };

            //Make sure it is matched
            var found = tree.GetTopMatched(lbl).ToList();
            Assert.IsNotNull(found);
            Assert.AreEqual(1, found.Count);
            Assert.AreEqual(defn, found[0]);

            lbl = new Label() { StyleClass = "not-" + cName };

            //Make sure it does not match

            found = tree.GetTopMatched(lbl).ToList();
            Assert.IsNotNull(found);
            Assert.AreEqual(0, found.Count);
        }

        [TestMethod()]
        public void StyleIndex_AddStyleIDDefinition_Test()
        {
            //Add a single definition with a selector of an id and check it is returned from the index for a matching component
            var id = "myId";
            var defn = new StyleDefn("#" + id);
            var tree = new StyleRootIndexTree();

            tree.AddStyle(defn);

            var lbl = new Label();
            lbl.ID = id;

            var found = tree.GetTopMatched(lbl).ToList();

            Assert.IsNotNull(found);
            Assert.AreEqual(1, found.Count);
            Assert.AreEqual(defn, found[0]);

            lbl = new Label();
            lbl.ID = "not-" + id;

            //Make sure it does not match

            found = tree.GetTopMatched(lbl).ToList();
            Assert.IsNotNull(found);
            Assert.AreEqual(0, found.Count);

        }

        [TestMethod()]
        public void StyleIndex_AddStyleElementDefinition_Test()
        {
            //Add a single definition with a selector of an id and check it is returned from the index for a matching component
            var ele = "label";
            var defn = new StyleDefn(ele);
            var tree = new StyleRootIndexTree();

            tree.AddStyle(defn);

            var lbl = new Label();
            lbl.ElementName = ele;

            var found = tree.GetTopMatched(lbl).ToList();

            Assert.IsNotNull(found);
            Assert.AreEqual(1, found.Count);
            Assert.AreEqual(defn, found[0]);

            lbl = new Label();
            lbl.ElementName = "not-" + ele;

            //Make sure it does not match

            found = tree.GetTopMatched(lbl).ToList();
            Assert.IsNotNull(found);
            Assert.AreEqual(0, found.Count);

        }

        //
        // no values
        //

        [TestMethod()]
        public void StyleIndex_AddStyleIndex_NO_Id_Definition_Test()
        {
            //Add a single definition with a selector of an id and check it is not returned from the index for a component with nothing set
            var id = "myId";
            var defn = new StyleDefn("#" + id);
            var tree = new StyleRootIndexTree();

            tree.AddStyle(defn);

            var lbl = new Label();

            //Make sure it does not match

            var found = tree.GetTopMatched(lbl).ToList();
            Assert.IsNotNull(found);
            Assert.AreEqual(0, found.Count);

        }

        [TestMethod()]
        public void StyleIndex_AddStyleIndex_NO_Element_Definition_Test()
        {
            //Add a single definition with a selector of an element and check it is not returned from the index for a component with nothing set
            var ele = "label";
            var defn = new StyleDefn(ele);
            var tree = new StyleRootIndexTree();

            tree.AddStyle(defn);

            var lbl = new Label();

            //Make sure it does not match

            var found = tree.GetTopMatched(lbl).ToList();
            Assert.IsNotNull(found);
            Assert.AreEqual(0, found.Count);

        }

        [TestMethod()]
        public void StyleIndex_AddStyleIndex_NO_Class_Definition_Test()
        {
            //Add a single definition with a selector of a class and check it is not returned from the index for a component with nothing set
            var cls = "className";
            var defn = new StyleDefn("." + cls);
            var tree = new StyleRootIndexTree();

            tree.AddStyle(defn);

            var lbl = new Label();

            //Make sure it does not match

            var found = tree.GetTopMatched(lbl).ToList();
            Assert.IsNotNull(found);
            Assert.AreEqual(0, found.Count);

        }

        //
        // double values
        //

        [TestMethod()]
        public void StyleIndex_AddStyleElementAndClassDefinition_Test()
        {
            //Add a single definition with a selector of an element + class (label.className) and check it is returned from the index for a matching component
            var ele = "label";
            var cls = "className";

            var defn = new StyleDefn(ele + "." + cls);
            var tree = new StyleRootIndexTree();

            tree.AddStyle(defn);

            var lbl = new Label();
            lbl.ElementName = ele;
            lbl.StyleClass = cls;

            var found = tree.GetTopMatched(lbl).ToList();

            Assert.IsNotNull(found);
            Assert.AreEqual(1, found.Count);
            Assert.AreEqual(defn, found[0]);

            lbl = new Label();
            lbl.ElementName = "not-" + ele;
            lbl.StyleClass = cls;

            //Make sure it does not match on the element name

            found = tree.GetTopMatched(lbl).ToList();
            Assert.IsNotNull(found);
            Assert.AreEqual(0, found.Count);

            lbl = new Label();
            lbl.ElementName = ele;
            lbl.StyleClass = "not-" + cls;

            //Make sure it does not match on the class

            found = tree.GetTopMatched(lbl).ToList();
            Assert.IsNotNull(found);
            Assert.AreEqual(0, found.Count);

        }

        [TestMethod()]
        public void StyleIndex_AddStyleElementAndIDDefinition_Test()
        {
            //Add a single definition with a selector of an id + class (#myId.className) and check it is returned from the index for a matching component
            var id = "myId";
            var ele = "label";

            var defn = new StyleDefn(ele + "#" + id);
            var tree = new StyleRootIndexTree();

            tree.AddStyle(defn);

            var lbl = new Label();
            lbl.ID = id;
            lbl.ElementName = ele;

            var found = tree.GetTopMatched(lbl).ToList();

            Assert.IsNotNull(found);
            Assert.AreEqual(1, found.Count);
            Assert.AreEqual(defn, found[0]);

            lbl = new Label();
            lbl.ID = "not-" + id;
            lbl.ElementName = ele;

            //Make sure it does not match on the id

            found = tree.GetTopMatched(lbl).ToList();
            Assert.IsNotNull(found);
            Assert.AreEqual(0, found.Count);

            lbl = new Label();
            lbl.ID = id;
            lbl.ElementName = "not-" + ele;

            //Make sure it does not match on the class

            found = tree.GetTopMatched(lbl).ToList();
            Assert.IsNotNull(found);
            Assert.AreEqual(0, found.Count);

        }

        [TestMethod()]
        public void StyleIndex_AddStyleIDAndClassDefinition_Test()
        {
            //Add a single definition with a selector of an id + class (#myId.className) and check it is returned from the index for a matching component
            var id = "myId";
            var cls = "className";

            var defn = new StyleDefn("#" + id + "." + cls);
            var tree = new StyleRootIndexTree();

            tree.AddStyle(defn);

            var lbl = new Label();
            lbl.ID = id;
            lbl.StyleClass = cls;

            var found = tree.GetTopMatched(lbl).ToList();

            Assert.IsNotNull(found);
            Assert.AreEqual(1, found.Count);
            Assert.AreEqual(defn, found[0]);

            lbl = new Label();
            lbl.ID = "not-" + id;
            lbl.StyleClass = cls;

            //Make sure it does not match on the id

            found = tree.GetTopMatched(lbl).ToList();
            Assert.IsNotNull(found);
            Assert.AreEqual(0, found.Count);

            lbl = new Label();
            lbl.ID = id;
            lbl.StyleClass = "not-" + cls;

            //Make sure it does not match on the class

            found = tree.GetTopMatched(lbl).ToList();
            Assert.IsNotNull(found);
            Assert.AreEqual(0, found.Count);

        }

        //
        // all three set
        //

        [TestMethod()]
        public void StyleIndex_AddStyleIDAndElementAndClassDefinition_Test()
        {
            //Add a single definition with a selector of an id + class (#myId.className) and check it is returned from the index for a matching component
            var id = "myId";
            var cls = "className";
            var ele = "label";

            var defn = new StyleDefn(ele + "#" + id + "." + cls);
            var tree = new StyleRootIndexTree();

            tree.AddStyle(defn);

            var lbl = new Label();
            lbl.ID = id;
            lbl.ElementName = ele;
            lbl.StyleClass = cls;

            var found = tree.GetTopMatched(lbl).ToList();

            Assert.IsNotNull(found);
            Assert.AreEqual(1, found.Count);
            Assert.AreEqual(defn, found[0]);

            lbl = new Label();
            lbl.ID = "not-" + id;
            lbl.ElementName = ele;
            lbl.StyleClass = cls;

            //Make sure it does not match on the id

            found = tree.GetTopMatched(lbl).ToList();
            Assert.IsNotNull(found);
            Assert.AreEqual(0, found.Count);

            lbl = new Label();
            lbl.ID = id;
            lbl.ElementName = ele;
            lbl.StyleClass = "not-" + cls;

            //Make sure it does not match on the class

            found = tree.GetTopMatched(lbl).ToList();
            Assert.IsNotNull(found);
            Assert.AreEqual(0, found.Count);

            lbl = new Label();
            lbl.ID = id;
            lbl.ElementName = "not-" + ele;
            lbl.StyleClass = cls;

            //Make sure it does not match on the class

            found = tree.GetTopMatched(lbl).ToList();
            Assert.IsNotNull(found);
            Assert.AreEqual(0, found.Count);

        }


        //
        // two class names (any order)
        //

        [TestMethod()]
        public void StyleIndex_AddStyleMultipleClassDefinition_Test()
        {
            //Add a single definition with a selector of a double class (.class1.class2) and check it is returned from the index for a matching component
            var cls = "class1";
            var cls2 = "class2";

            var defn = new StyleDefn("." + cls + "." + cls2);
            var tree = new StyleRootIndexTree();

            tree.AddStyle(defn);

            
            var lbl = new Label();
            lbl.StyleClass = cls + " " + cls2;

            var found = tree.GetTopMatched(lbl).ToList();

            Assert.IsNotNull(found);
            Assert.AreEqual(1, found.Count);
            Assert.AreEqual(defn, found[0]);

            //reverse names and confirm

            lbl = new Label();
            lbl.StyleClass = cls2 + " " + cls;

            found = tree.GetTopMatched(lbl).ToList();

            Assert.IsNotNull(found);
            Assert.AreEqual(1, found.Count);
            Assert.AreEqual(defn, found[0]);

            lbl = new Label();
            lbl.StyleClass = "not-" + cls + " " + cls2;

            //Make sure it does not match on the not-class1

            found = tree.GetTopMatched(lbl).ToList();
            Assert.IsNotNull(found);
            Assert.AreEqual(0, found.Count);

            lbl = new Label();
            lbl.StyleClass = "not-" + cls2 + " " + cls;

            //Make sure it does not match on the reversed not-class2

            found = tree.GetTopMatched(lbl).ToList();
            Assert.IsNotNull(found);
            Assert.AreEqual(0, found.Count);

        }

        [TestMethod()]
        public void StyleIndex_AddStyleMultipleClassWithNameDefinition_Test()
        {
            //Add a single definition with a selector of a double class (.class1.class2) and check it is returned from the index for a matching component
            var cls = "class1";
            var cls2 = "class2";
            var ele = "label";

            var defn = new StyleDefn(ele + "." + cls + "." + cls2);
            var tree = new StyleRootIndexTree();

            tree.AddStyle(defn);


            var lbl = new Label();
            lbl.ElementName = ele;
            lbl.StyleClass = cls + " " + cls2;

            var found = tree.GetTopMatched(lbl).ToList();

            Assert.IsNotNull(found);
            Assert.AreEqual(1, found.Count);
            Assert.AreEqual(defn, found[0]);

            //reverse names and confirm

            lbl = new Label();
            lbl.ElementName = ele;
            lbl.StyleClass = cls2 + " " + cls;

            found = tree.GetTopMatched(lbl).ToList();

            Assert.IsNotNull(found);
            Assert.AreEqual(1, found.Count);
            Assert.AreEqual(defn, found[0]);

            lbl = new Label();
            lbl.ElementName = ele;
            lbl.StyleClass = "not-" + cls + " " + cls2;

            //Make sure it does not match on the not-class1

            found = tree.GetTopMatched(lbl).ToList();
            Assert.IsNotNull(found);
            Assert.AreEqual(0, found.Count);

            lbl = new Label();
            lbl.ElementName = ele;
            lbl.StyleClass = "not-" + cls2 + " " + cls;

            //Make sure it does not match on the reversed not-class2

            found = tree.GetTopMatched(lbl).ToList();
            Assert.IsNotNull(found);
            Assert.AreEqual(0, found.Count);

            lbl = new Label();
            lbl.ElementName = "not-" + ele;
            lbl.StyleClass =  cls2 + " " + cls;

            //Make sure it does not match on the not-label

            found = tree.GetTopMatched(lbl).ToList();
            Assert.IsNotNull(found);
            Assert.AreEqual(0, found.Count);

        }

        [TestMethod()]
        public void StyleIndex_AddStyleMultipleClassWithIDDefinition_Test()
        {
            //Add a single definition with a selector of a double class (.class1.class2) and check it is returned from the index for a matching component
            var cls = "class1";
            var cls2 = "class2";
            var id = "myId";

            var defn = new StyleDefn("#" + id + "." + cls + "." + cls2);
            var tree = new StyleRootIndexTree();

            tree.AddStyle(defn);


            var lbl = new Label();
            lbl.ID = id;
            lbl.StyleClass = cls + " " + cls2;

            var found = tree.GetTopMatched(lbl).ToList();

            Assert.IsNotNull(found);
            Assert.AreEqual(1, found.Count);
            Assert.AreEqual(defn, found[0]);

            //reverse names and confirm

            lbl = new Label();
            lbl.ID = id;
            lbl.StyleClass = cls2 + " " + cls;

            found = tree.GetTopMatched(lbl).ToList();

            Assert.IsNotNull(found);
            Assert.AreEqual(1, found.Count);
            Assert.AreEqual(defn, found[0]);

            lbl = new Label();
            lbl.ID = id;
            lbl.StyleClass = "not-" + cls + " " + cls2;

            //Make sure it does not match on the not-class1

            found = tree.GetTopMatched(lbl).ToList();
            Assert.IsNotNull(found);
            Assert.AreEqual(0, found.Count);

            lbl = new Label();
            lbl.ID = id;
            lbl.StyleClass = "not-" + cls2 + " " + cls;

            //Make sure it does not match on the reversed not-class2

            found = tree.GetTopMatched(lbl).ToList();
            Assert.IsNotNull(found);
            Assert.AreEqual(0, found.Count);


            lbl = new Label();
            lbl.ID = id;
            lbl.StyleClass = cls2 + " not-" + cls;

            //Make sure it does not match on the reversed not-class

            found = tree.GetTopMatched(lbl).ToList();
            Assert.IsNotNull(found);
            Assert.AreEqual(0, found.Count);

            lbl = new Label();
            lbl.ID = "not-" + id;
            lbl.StyleClass = cls2 + " " + cls;

            //Make sure it does not match on the not-myId

            found = tree.GetTopMatched(lbl).ToList();
            Assert.IsNotNull(found);
            Assert.AreEqual(0, found.Count);

        }

        //
        // matching on the last selector of the definition
        //

        [TestMethod]
        public void StyleIndex_AddSecondarySelector()
        {
            var className = "className1";
            var className2 = "className2";

            //double class name
            var defn = new StyleDefn("." + className + ", ." + className2);

            var tree = new StyleRootIndexTree();

            tree.AddStyle(defn);


            var lbl = new Label();
            lbl.StyleClass = className;

            var found = tree.GetTopMatched(lbl).ToList();

            Assert.IsNotNull(found);
            Assert.AreEqual(1, found.Count);
            Assert.AreEqual(defn, found[0]);

            lbl = new Label();
            lbl.StyleClass = className2;

            found = tree.GetTopMatched(lbl).ToList();

            Assert.IsNotNull(found);
            Assert.AreEqual(1, found.Count);
            Assert.AreEqual(defn, found[0]);

        }

        //
        // root styles
        //

        [TestMethod]
        public void StyleIndex_AddRootStyleSelector()
        {

            var root = ":root";
            var defn = new StyleDefn(root);

            var tree = new StyleRootIndexTree();
            tree.AddStyle(defn);

            var lbl = new Label();

            var found = tree.GetTopMatched(lbl).ToList();

            Assert.IsNotNull(found);
            Assert.AreEqual(0, found.Count); //not found for label

            var doc = new HTMLDocument();

            found = tree.GetTopMatched(doc).ToList();

            Assert.IsNotNull(found);
            Assert.AreEqual(1, found.Count);

            Assert.AreEqual(defn, found[0]); //found for document

            //add another defn for a class
            var className = "clsName";
            var classDefn = new StyleDefn("." + className);
            tree.AddStyle(classDefn);

            //apply the classes to the document and label
            lbl.StyleClass = className;
            doc.StyleClass = className;


            found = tree.GetTopMatched(lbl).ToList();
            Assert.IsNotNull(found);
            Assert.AreEqual(1, found.Count); //found for label
            Assert.AreEqual(classDefn, found[0]); //it is the class defn
            

            found = tree.GetTopMatched(doc).ToList();

            Assert.IsNotNull(found);
            Assert.AreEqual(2, found.Count);
            Assert.AreEqual(defn, found[0]); //found :root for document
            Assert.AreEqual(classDefn, found[1]); //found .clsName for document


        }

        //
        // catch all matcher
        //

        [TestMethod]
        public void StyleIndex_AddCatchAllStyleSelector()
        {
            var catchAll = "*";
            var defn = new StyleDefn(catchAll);

            var tree = new StyleRootIndexTree();
            tree.AddStyle(defn);

            var lbl = new Label();

            var found = tree.GetTopMatched(lbl).ToList();

            Assert.IsNotNull(found);
            Assert.AreEqual(1, found.Count); //found for label
            Assert.AreEqual(defn, found[0]);

            var doc = new HTMLDocument();

            found = tree.GetTopMatched(doc).ToList();
            Assert.IsNotNull(found);
            Assert.AreEqual(1, found.Count); //found for doc
            Assert.AreEqual(defn, found[0]);


            //Add the namespace qualified catch all
            catchAll = "*|*";
            var nsdefn = new StyleDefn(catchAll);

            tree.AddStyle(nsdefn);

            found = tree.GetTopMatched(lbl).ToList();

            Assert.IsNotNull(found);
            Assert.AreEqual(2, found.Count); //found both for label
            Assert.AreEqual(defn, found[0]);
            Assert.AreEqual(nsdefn, found[1]);

            doc = new HTMLDocument();

            found = tree.GetTopMatched(doc).ToList();
            Assert.IsNotNull(found);
            Assert.AreEqual(2, found.Count); //found both for doc
            Assert.AreEqual(defn, found[0]);
            Assert.AreEqual(nsdefn, found[1]);

        }

        [TestMethod]
        public void StyleIndex_AddInnerCatchAllStyleSelector()
        {
            
            //
            // .wrapper > *
            //

            var innerCatchAll = ".wrapper > *";
            var defn = new StyleDefn(innerCatchAll);

            var tree = new StyleRootIndexTree();
            tree.AddStyle(defn);

            var lbl = new Label();

            var found = tree.GetTopMatched(lbl).ToList();

            Assert.IsNotNull(found);
            Assert.AreEqual(1, found.Count); //found for label based on the last *
            Assert.AreEqual(defn, found[0]);
            //but not matched
            int priority;
            Assert.IsFalse(defn.IsMatchedTo(lbl, ComponentState.Normal, out priority));


            //Add the wrapper class as a parent and it should now match.
            var div = new Div();
            div.StyleClass = "wrapper";

            div.Contents.Add(lbl);

            found = tree.GetTopMatched(lbl).ToList();

            Assert.IsNotNull(found);
            Assert.AreEqual(1, found.Count); //found for label based on the last *
            Assert.AreEqual(defn, found[0]);

            var matched = defn.IsMatchedTo(lbl, ComponentState.Normal, out priority);
            Assert.IsTrue(matched);

            //
            //.wrapper > *|*
            //

            innerCatchAll = ".wrapper > *|*";
            defn = new StyleDefn(innerCatchAll);

            tree = new StyleRootIndexTree();
            tree.AddStyle(defn);

            lbl = new Label();

            found = tree.GetTopMatched(lbl).ToList();

            Assert.IsNotNull(found);
            Assert.AreEqual(1, found.Count); //found for label based on the last *
            Assert.AreEqual(defn, found[0]);
            //but not matched
           
            Assert.IsFalse(defn.IsMatchedTo(lbl, ComponentState.Normal, out priority));


            //Add the wrapper class as a parent and it should now match.
            div = new Div();
            div.StyleClass = "wrapper";

            div.Contents.Add(lbl);

            found = tree.GetTopMatched(lbl).ToList();

            Assert.IsNotNull(found);
            Assert.AreEqual(1, found.Count); //found for label based on the last *
            Assert.AreEqual(defn, found[0]);

            Assert.IsTrue(defn.IsMatchedTo(lbl, ComponentState.Normal, out priority));

            //
            //.wrapper > *.className
            //

            innerCatchAll = ".wrapper > *.class1";
            defn = new StyleDefn(innerCatchAll);

            tree = new StyleRootIndexTree();
            tree.AddStyle(defn);

            lbl = new Label();

            found = tree.GetTopMatched(lbl).ToList();
            Assert.IsNotNull(found);
            Assert.AreEqual(0, found.Count); //NOT found for label based on the last *.class1


            lbl.StyleClass = "class1";

            found = tree.GetTopMatched(lbl).ToList();
            Assert.IsNotNull(found);
            Assert.AreEqual(1, found.Count); //found for label based on the last *
            Assert.AreEqual(defn, found[0]);

            //but not matched

            Assert.IsFalse(defn.IsMatchedTo(lbl, ComponentState.Normal, out priority));

            
            //
            //Add the wrapper class as a parent and it should now match.
            //

            div = new Div();
            div.StyleClass = "wrapper";

            div.Contents.Add(lbl);

            found = tree.GetTopMatched(lbl).ToList();

            Assert.IsNotNull(found);
            Assert.AreEqual(1, found.Count); //found for label based on the last *
            Assert.AreEqual(defn, found[0]);

            Assert.IsTrue(defn.IsMatchedTo(lbl, ComponentState.Normal, out priority));

            //False check on the direct ancestor div.wrapper -> div -> *.class1 should not be a full match

            div.StyleClass = "";

            var div2 = new Div();
            div2.Contents.Add(div);
            div2.StyleClass = "wrapper";

            found = tree.GetTopMatched(lbl).ToList();
            Assert.IsNotNull(found);
            Assert.AreEqual(1, found.Count); //found for label based on the last *
            Assert.AreEqual(defn, found[0]);

            //but not matched

            Assert.IsFalse(defn.IsMatchedTo(lbl, ComponentState.Normal, out priority));



            //with *.className we should just ignore the *

            lbl = new Label();
            div = new Div();

            defn = new StyleDefn("*.className");
            

            tree = new StyleRootIndexTree();
            tree.AddStyle(defn);

            found = tree.GetTopMatched(lbl).ToList();
            Assert.IsNotNull(found);
            Assert.AreEqual(0, found.Count);

            found = tree.GetTopMatched(div).ToList();
            Assert.IsNotNull(found);
            Assert.AreEqual(0, found.Count);

            lbl.StyleClass = "className";

            found = tree.GetTopMatched(lbl).ToList();
            Assert.IsNotNull(found);
            Assert.AreEqual(1, found.Count);
            Assert.AreEqual(defn, found[0]);

            div.StyleClass = "className";

            found = tree.GetTopMatched(div).ToList();
            Assert.IsNotNull(found);
            Assert.AreEqual(1, found.Count);
            Assert.AreEqual(defn, found[0]);

            lbl.StyleClass = "className2";
            found = tree.GetTopMatched(lbl).ToList();
            Assert.IsNotNull(found);
            Assert.AreEqual(0, found.Count);

        }




        //
        // multiple combined selectors
        //

        //.wrapper > .class1, .wrapper > label
        //div > label, div.classname label.classname
        //div > label > .classname


        //.wrapper > .class1, .wapper > *|*.class2,
        //.wrapper > *.class1, .wapper > *|*
        //.wrapper *, .wrapper > *.class3



        //ENSURE SUPPORT FOR AT RULES

        //
        // page styles
        //

        [TestMethod]
        public void StyleIndex_AddPageStyleSelector()
        {
            var tree = new StyleRootIndexTree();

            //@page { margin-left: 16pt }
            var pg = new StylePageGroup(new Scryber.Styles.Selectors.PageMatcher());
            pg.Margins.Left = 16;
            Assert.IsTrue(tree.AddStyle(pg));

            //@page wide { width: 500pt }
            var wide = new StylePageGroup(Scryber.Styles.Selectors.PageMatcher.Parse("wide"));
            wide.Size.Width = 500;
            Assert.IsTrue(tree.AddStyle(wide));

            //page styles are matched against the document's '@page' component, which is not a page
            var pageComponent = new Label() { ElementName = "@page" };
            var found = tree.GetTopMatched(pageComponent).ToList();
            Assert.AreEqual(2, found.Count);
            Assert.AreEqual(pg, found[0]);
            Assert.AreEqual(wide, found[1]);

            var style = new Style();
            foreach (var item in found)
                item.MergeInto(style, pageComponent);
            Assert.AreEqual((Unit)16, style.GetValue(StyleKeys.MarginsLeftKey, Unit.Zero));
            Assert.IsFalse(style.IsValueDefined(StyleKeys.SizeWidthKey), "The named page group should not match an unnamed page");

            //a named page
            var namedComponent = new Label() { ElementName = "@page", StyleClass = "wide" };
            style = new Style();
            foreach (var item in tree.GetTopMatched(namedComponent))
                item.MergeInto(style, namedComponent);
            Assert.AreEqual((Unit)16, style.GetValue(StyleKeys.MarginsLeftKey, Unit.Zero));
            Assert.AreEqual((Unit)500, style.GetValue(StyleKeys.SizeWidthKey, Unit.Zero));

            //any other component is given the page groups (as in a loop over all the styles), but they do not match it
            var label = new Label() { StyleClass = "wide" };
            style = new Style();
            foreach (var item in tree.GetTopMatched(label))
                item.MergeInto(style, label);
            Assert.AreEqual(0, style.ValueCount, "The page styles should not apply to a component");
        }



        //
        // media matcher
        //

        [TestMethod]
        public void StyleIndex_AddMediaStyleSelector()
        {
            var tree = new StyleRootIndexTree();

            //@media print { .cls { width: 10pt } }
            var print = new StyleMediaGroup(Scryber.Styles.Selectors.MediaMatcher.Parse("print"));
            var printed = new StyleDefn(".cls");
            printed.Size.Width = 10;
            print.Styles.Add(printed);
            Assert.IsTrue(tree.AddStyle(print), "The print media styles should be added");

            //@media screen { .cls { height: 20pt } }
            var screen = new StyleMediaGroup(Scryber.Styles.Selectors.MediaMatcher.Parse("screen"));
            var screened = new StyleDefn(".cls");
            screened.Size.Height = 20;
            screen.Styles.Add(screened);
            Assert.IsFalse(tree.AddStyle(screen), "The screen media styles should not be added");

            var label = new Label() { StyleClass = "cls" };
            var found = tree.GetTopMatched(label).ToList();
            Assert.AreEqual(1, found.Count);
            Assert.AreEqual(printed, found[0], "The inner style of the matched media group should be found");

            var style = new Style();
            foreach (var item in found)
                item.MergeInto(style, label);
            Assert.AreEqual((Unit)10, style.GetValue(StyleKeys.SizeWidthKey, Unit.Zero));
            Assert.IsFalse(style.IsValueDefined(StyleKeys.SizeHeightKey), "The screen media style should not be applied");

            //and not to another class
            Assert.AreEqual(0, tree.GetTopMatched(new Label() { StyleClass = "other" }).Count());
        }

        //
        // font face
        //

        [TestMethod]
        public void StyleIndex_AddFontFaceStyleSelector()
        {
            var tree = new StyleRootIndexTree();

            //A font face is registered with the document resources, and never merged into a component's style
            var ff = new StyleFontFace();
            ff.FontFamily = (FontSelector)"Test Font";
            Assert.IsFalse(tree.AddStyle(ff), "The font face should not be added to the index");

            var defn = new StyleDefn(".cls");
            defn.Size.Width = 10;
            Assert.IsTrue(tree.AddStyle(defn));

            var label = new Label() { StyleClass = "cls" };
            var found = tree.GetTopMatched(label).ToList();
            Assert.AreEqual(1, found.Count);
            Assert.AreEqual(defn, found[0]);

            Assert.AreEqual(0, tree.GetTopMatched(new Label()).Count(), "The font face should not be returned for any component");
        }

        //
        // checking the speed to make sure this is all worth it, before going to far - close to a 15 to 20 fold improvement is generally good enough (1.5 seconds down to 100 milliseconds) :-)
        //

        [TestMethod()]
        public void StyleIndex_ManyStyleClassDefinitions_Test()
        {
            //A loop over every style (as a collection of 30 or fewer does) against the index, for the same lookups.
            //The index should give the same single value, and be much faster.
            var cName = "className";
            var tree = new StyleRootIndexTree();
            var all = new List<StyleDefn>();

            const int TotalCount = 10000;
            const int LookupCount = 200;

            for (var i = 0; i < TotalCount; i++)
            {
                var defn = new StyleDefn("." + cName + i);
                defn.Size.Width = i;
                all.Add(defn);
                tree.AddStyle(defn);
            }

            var rnd = new Random(42);
            var lookups = new List<Label>();
            var expected = new List<int>();
            for (var i = 0; i < LookupCount; i++)
            {
                var next = rnd.Next(TotalCount);
                lookups.Add(new Label() { StyleClass = cName + next });
                expected.Add(next);
            }

            var style = new Style();
            var stopWatch = System.Diagnostics.Stopwatch.StartNew();

            //Every style in turn
            for (var i = 0; i < lookups.Count; i++)
            {
                style.Clear();
                foreach (var defn in all)
                    defn.MergeInto(style, lookups[i]);

                Assert.AreEqual(1, style.ValueCount, "Loop value count");
                Assert.AreEqual((Unit)expected[i], style.GetValue(StyleKeys.SizeWidthKey, Unit.Zero), "Loop width");
            }

            stopWatch.Stop();
            var elapsedLoop = stopWatch.Elapsed;
            testContextInstance.WriteLine("Loop over " + TotalCount.ToString("#,###") + " styles for " + LookupCount + " components = " + elapsedLoop.TotalMilliseconds + "ms");

            stopWatch.Restart();

            //The index
            for (var i = 0; i < lookups.Count; i++)
            {
                style.Clear();
                foreach (var found in tree.GetTopMatched(lookups[i]))
                    found.MergeInto(style, lookups[i]);

                Assert.AreEqual(1, style.ValueCount, "Index value count");
                Assert.AreEqual((Unit)expected[i], style.GetValue(StyleKeys.SizeWidthKey, Unit.Zero), "Index width");
            }

            stopWatch.Stop();
            var elapsedIndex = stopWatch.Elapsed;
            testContextInstance.WriteLine("Index of " + TotalCount.ToString("#,###") + " styles for " + LookupCount + " components = " + elapsedIndex.TotalMilliseconds + "ms");

            //a big margin, so the test does not depend on the load of the machine
            Assert.IsTrue(elapsedIndex.Ticks * 10 < elapsedLoop.Ticks, "The index was not at least 10 times faster");
        }

        


    }
}

