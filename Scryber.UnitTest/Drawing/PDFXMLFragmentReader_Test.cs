using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Text;

namespace Scryber.Core.UnitTests.Drawing
{
    /// <summary>
    /// Summary description for XMLFragmentReader_Test
    /// </summary>
    [TestClass]
    public class PDFXMLFragmentReader_Test
    {
        public PDFXMLFragmentReader_Test()
        {
            //
            // TODO: Add constructor logic here
            //
        }

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
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion


        //
        // Test 0
        //

        private const string TestXml0 = @"This is some text that may not
                close every thing or have every thing in 
                a single line</span>";

        private static PDFTextOp[] MatchXml0 = new PDFTextOp[] { new PDFTextDrawOp("This is some text that may not close every thing or have every thing in a single line"), new PDFTextClassOp("", false) };


        [TestMethod]
        [TestCategory("Parser")]
        public void XMLFragment_TestStringXML0()
        {
            PDFXMLFragmentParser parser = new PDFXMLFragmentParser();
            List<Scryber.Text.PDFTextOp> all = parser.Parse(TestXml0, false);

            for (int i = 0; i < all.Count; i++)
            {
                TestContext.WriteLine("{0} Operation: '{1}'", i, all[i]);
                Assert.AreEqual(all[i], MatchXml0[i]);
            }

        }


        //
        // Test 1
        //

        private const string TestXml1 = @"This is some text that may not
                <span class='myclass' >close every thing or have every thing in a
                single line</span>";

        private static PDFTextOp[] MatchXml1 = new PDFTextOp[] { new PDFTextDrawOp("This is some text that may not "), 
                                                                 new PDFTextClassOp("myclass",true), 
                                                                 new PDFTextDrawOp("close every thing or have every thing in a single line"),
                                                                 new PDFTextClassOp("myclass", false) };

        [TestMethod]
        [TestCategory("Parser")]
        public void XMLFragment_TestStringXML1()
        {
            PDFXMLFragmentParser parser = new PDFXMLFragmentParser();
            List<Scryber.Text.PDFTextOp> all = parser.Parse(TestXml1, false);

            for (int i = 0; i < all.Count; i++)
            {
                TestContext.WriteLine("{0} Operation: '{1}'", i, all[i]);
                Assert.AreEqual(all[i], MatchXml1[i]);
            }

        }


        //
        // Test 2
        //

        private const string TestXml2 = @"This is some text <b>in bold<i> and italic</i> that may not &lt;&gt;<br/>
                close every thing or <span class='myclass' style='font-weight:bold;' ><span class='another'>have every thing in a </span>
                single line</span>";

        private static PDFTextOp[] MatchXml2 = new PDFTextOp[] { new PDFTextDrawOp("This is some text "), 
                                                                 new PDFTextFontOp("b",true),
                                                                 new PDFTextDrawOp("in bold"),
                                                                 new PDFTextFontOp("i",true),
                                                                 new PDFTextDrawOp(" and italic"),
                                                                 new PDFTextFontOp("i", false),
                                                                 new PDFTextDrawOp(" that may not <>"),
                                                                 new PDFTextNewLineOp(),
                                                                 new PDFTextDrawOp(" close every thing or "),
                                                                 new PDFTextClassOp("myclass",true),
                                                                 new PDFTextClassOp("another",true),
                                                                 new PDFTextDrawOp("have every thing in a "),
                                                                 new PDFTextClassOp("another",false),
                                                                 new PDFTextDrawOp(" single line"),
                                                                 new PDFTextClassOp("myclass", false) };

        [TestMethod]
        [TestCategory("Parser")]
        public void XMLFragment_TestStringXML2()
        {
            PDFXMLFragmentParser parser = new PDFXMLFragmentParser();
            List<Scryber.Text.PDFTextOp> all = parser.Parse(TestXml2, false);

            for (int i = 0; i < all.Count; i++)
            {
                TestContext.WriteLine("{0} Operation: '{1}'", i, all[i]);
                Assert.AreEqual(MatchXml2[i], all[i]);
            }

        }

        //
        // Test 3
        //

        private const string TestXml3 = @"This is some text <b>in bold <i>and italic</i></b> that may not
                 <span class='myclass' >close every thing or </span>
                 <span class='another'>have every thing in a </span>
                 single line";

        private static PDFTextOp[] MatchXml3 = new PDFTextOp[] { new PDFTextDrawOp("This is some text "), 
                                                                 new PDFTextFontOp("b",true),
                                                                 new PDFTextDrawOp("in bold "),
                                                                 new PDFTextFontOp("i",true),
                                                                 new PDFTextDrawOp("and italic"),
                                                                 new PDFTextFontOp("i", false),
                                                                 new PDFTextFontOp("b",false),
                                                                 new PDFTextDrawOp(" that may not "),
                                                                 new PDFTextClassOp("myclass",true),
                                                                 new PDFTextDrawOp("close every thing or "),
                                                                 new PDFTextClassOp("myclass", false),
                                                                 new PDFTextDrawOp(" "),
                                                                 new PDFTextClassOp("another",true),
                                                                 new PDFTextDrawOp("have every thing in a "),
                                                                 new PDFTextClassOp("another",false),
                                                                 new PDFTextDrawOp(" single line") };

        [TestMethod]
        [TestCategory("Parser")]
        public void XMLFragment_TestStringXML3()
        {
            PDFXMLFragmentParser parser = new PDFXMLFragmentParser();
            List<Scryber.Text.PDFTextOp> all = parser.Parse(TestXml3, false);

            for (int i = 0; i < all.Count; i++)
            {
                TestContext.WriteLine("{0} Operation: '{1}'", i, all[i]);
                Assert.AreEqual(MatchXml3[i], all[i]);
            }

        }

        [TestMethod()]
        [TestCategory("Parser")]
        public void XMLFragment_TestReaderXML3()
        {
            PDFXMLFragmentReader reader = new PDFXMLFragmentReader(TestXml3, false, null);
            int index = 0;

            while (reader.Read())
            {
                Assert.AreEqual(MatchXml3[index], reader.Value);
                Assert.AreEqual(MatchXml3[index].OpType, reader.OpType);
                index++;
            }
            Assert.IsTrue(reader.EOF);

        }

        [TestMethod()]
        [TestCategory("Parser")]
        public void XMLFragment_TestReaderXML3WithReset()
        {
            PDFXMLFragmentReader reader = new PDFXMLFragmentReader(TestXml3, false, null);
            int index = 0;
            bool hasreset = false;

            while (reader.Read())
            {
                Assert.AreEqual(MatchXml3[index], reader.Value);
                Assert.AreEqual(MatchXml3[index].OpType, reader.OpType);
                index++;
                if (index == 5 && hasreset == false)
                {
                    hasreset = true;
                    index = 0;
                    reader.Reset();
                }
            }
            Assert.IsTrue(reader.EOF);

        }


        //
        // Test 4
        //

        private const string TestXml4 = @"<span class='myclass'>This is some text that may not
                close every thing or have every thing in
                a single line</span>";

        private static PDFTextOp[] MatchXml4 = new PDFTextOp[] { new PDFTextClassOp("myclass",true),
                                                                 new PDFTextDrawOp("This is some text that may not close every thing or have every thing in a single line"), 
                                                                 new PDFTextClassOp("myclass", false) };

        [TestMethod]
        [TestCategory("Parser")]
        public void XMLFragment_TestStringXML4()
        {
            PDFXMLFragmentParser parser = new PDFXMLFragmentParser();
            List<Scryber.Text.PDFTextOp> all = parser.Parse(TestXml4, false);

            for (int i = 0; i < all.Count; i++)
            {
                TestContext.WriteLine("{0} Operation: '{1}'", i, all[i]);
                Assert.AreEqual(MatchXml4[i], all[i]);
            }

        }

        [TestMethod()]
        [TestCategory("Parser")]
        public void XMLFragment_TestReaderXML4()
        {
            PDFXMLFragmentReader reader = new PDFXMLFragmentReader(TestXml4, false, null);
            int index = 0;
            while (reader.Read())
            {
                Assert.AreEqual(MatchXml4[index], reader.Value);
                Assert.AreEqual(MatchXml4[index].OpType, reader.OpType);
                index++;
            }
            Assert.IsTrue(reader.EOF);

        }

        //
        // Test 5
        //

        private const string TestXml5 = @"This is some text that may not
                close every thing or have every thing in
                a single line
                ";

        private static PDFTextOp[] MatchXml5 = new PDFTextOp[] { new PDFTextDrawOp("This is some text that may not close every thing or have every thing in a single line ") };

        [TestMethod]
        [TestCategory("Parser")]
        public void XMLFragment_TestStringXML5()
        {
            PDFXMLFragmentParser parser = new PDFXMLFragmentParser();
            List<Scryber.Text.PDFTextOp> all = parser.Parse(TestXml5, false);

            for (int i = 0; i < all.Count; i++)
            {
                TestContext.WriteLine("{0} Operation: '{1}'", i, all[i]);
                Assert.AreEqual(MatchXml5[i], all[i]);
            }

        }


        [TestMethod()]
        [TestCategory("Parser")]
        public void XMLFragment_TestReaderXML5()
        {
            PDFXMLFragmentReader reader = new PDFXMLFragmentReader(TestXml5, false, null);
            int index = 0;
            while (reader.Read())
            {
                Assert.AreEqual(MatchXml5[index], reader.Value);
                Assert.AreEqual(MatchXml5[index].OpType, reader.OpType);
                index++;
            }
            Assert.IsTrue(reader.EOF);

        }


        //
        // Test 6 - Invalid Xml 
        //

        private const string TestXml6_1 = @"This is some text <b>in bold <i>and italic</i></b> that may not
                 <span class='myclass' close every thing or </span>
                 <span class='another'>have every thing in a </span>
                 single line";

        private const string TestXml6_2 = @"This is some text <b>in bold <i>and italic</i></b> that may not
                 <span class='myclass' >close every thing or </span>
                 <span class='another'>have every thing in a </span
                 single line";


        [TestMethod]
        [TestCategory("Parser")]
        public void XMLFragment_TestStringXML6()
        {
            PDFXMLFragmentParser parser = new PDFXMLFragmentParser();
            try
            {
                List<Scryber.Text.PDFTextOp> all = parser.Parse(TestXml6_1, false);
                throw new ArgumentException("Required exception not thrown");
            }
            catch (PDFXmlFormatException ex)
            {
                TestContext.WriteLine("Successfully caught the exception from invalid xml :" + ex.Message);
            }


            parser = new PDFXMLFragmentParser();
            try
            {
                List<Scryber.Text.PDFTextOp> all = parser.Parse(TestXml6_2, false);
                throw new ArgumentException("Required exception not thrown");
            }
            catch (PDFXmlFormatException ex)
            {
                TestContext.WriteLine("Successfully caught the exception from invalid xml : " + ex.Message);
            }



        }


        [TestMethod()]
        [TestCategory("Parser")]
        public void XMLFragment_TestReaderXML6()
        {
            PDFXMLFragmentReader reader = null;
            try
            {
                reader = new PDFXMLFragmentReader(TestXml6_2, false, null);
                throw new ArgumentException("Required excpetion not thrown");
            }
            catch (PDFXmlFormatException ex)
            {
                TestContext.WriteLine("Successfully caught the exception from invalid xml : " + ex.Message);
            }


            if (reader != null)
            {
                try
                {
                    reader.Read();
                    throw new ArgumentException("Required exception not thrown");
                }
                catch (PDFXmlFormatException ex)
                {
                    TestContext.WriteLine("Successfully caught the exception from invalid xml : " + ex.Message);
                }
            }

        }

    }
}
