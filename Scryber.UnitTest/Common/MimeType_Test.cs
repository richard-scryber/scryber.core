using Scryber.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber;
using Scryber.Components;
using System.CodeDom;
using Scryber.PDF.Native;

namespace Scryber.Core.UnitTests.Common
{
    
    
    /// <summary>
    ///This is a test class for SVG XObjects so the drawing and the offsets at the component level are based on Top Left to Bottom Right,
    /// and a translated to the PDF Bottom Left to Top Right at render time.
    ///</summary>
    [TestClass()]
    public class MimeType_Test
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





        /// <summary>
        ///A test for MimeType Constructor with a simple root
        ///</summary>
        [TestMethod()]
        [TestCategory("Common")]
        public void MimeTypeConstructor_Test()
        {
            string expected = "text";
            MimeType mime = new MimeType(expected);


            Assert.IsTrue(mime.IsValid);
            Assert.AreEqual(expected, mime.ToString());
            Assert.AreEqual(expected, mime.Root);
            Assert.IsTrue(string.IsNullOrEmpty(mime.Base));
            Assert.IsTrue(string.IsNullOrEmpty(mime.Content));

        }

        /// <summary>
        ///A test for MimeType Constructor with a simple root/content
        ///</summary>
        [TestMethod()]
        [TestCategory("Common")]
        public void MimeTypeConstructor2_Test()
        {
            string expected = "text/html";
            MimeType mime = new MimeType(expected);


            Assert.IsTrue(mime.IsValid);
            Assert.AreEqual(expected, mime.ToString());
            Assert.AreEqual("text", mime.Root);
            Assert.IsTrue(string.IsNullOrEmpty(mime.Base));
            Assert.AreEqual("html", mime.Content);

        }

        /// <summary>
        ///A test for MimeType Constructor with a simple root/content
        ///</summary>
        [TestMethod()]
        [TestCategory("Common")]
        public void MimeTypeConstructor3_Test()
        {
            string expected = "application/xhtml+xml";
            MimeType mime = new MimeType(expected);


            Assert.IsTrue(mime.IsValid);
            Assert.AreEqual(expected, mime.ToString());
            Assert.AreEqual("application", mime.Root);
            Assert.AreEqual("xml", mime.Base);
            Assert.AreEqual("xhtml", mime.Content);

        }


        /// <summary>
        ///A test for MimeType Constructor with a simple root/content
        ///</summary>
        [TestMethod()]
        [TestCategory("Common")]
        public void MimeTypeEquals_Test()
        {
            bool expected = true;
            MimeType mime = new MimeType("application/xhtml+xml");

            var actual = mime.Equals(new MimeType("application/xhtml+xml"));
            Assert.AreEqual(expected, actual);

            Assert.AreEqual("application", mime.Root);
            Assert.AreEqual("xml", mime.Base);
            Assert.AreEqual("xhtml", mime.Content);

            mime = new MimeType("text/xhtml+xml");
            expected = false;
            actual = mime.Equals(new MimeType("application/xhtml+xml"));

            Assert.AreEqual(expected, actual);

            Assert.AreEqual("text", mime.Root);
            Assert.AreEqual("xml", mime.Base);
            Assert.AreEqual("xhtml", mime.Content);

        }


        /// <summary>
        ///A test for MimeType Constructor with a simple root/content
        ///</summary>
        [TestMethod()]
        [TestCategory("Common")]
        public void MimeTypeTryParse_Test()
        {
            bool expected = true;
            var type = "application/xhtml+xml";

            MimeType mime;
            bool actual = MimeType.TryParse(type, out mime);

            Assert.AreEqual(expected, actual);

            actual = mime.Equals(new MimeType(type));
            Assert.AreEqual(expected, actual);

            Assert.AreEqual("application", mime.Root);
            Assert.AreEqual("xml", mime.Base);
            Assert.AreEqual("xhtml", mime.Content);


            type = "text/html";

            actual = MimeType.TryParse(type, out mime);

            Assert.AreEqual(expected, actual);

            actual = mime.Equals(new MimeType(type));
            Assert.AreEqual(expected, actual);

            Assert.AreEqual("text", mime.Root);
            Assert.IsNull(mime.Base);
            Assert.AreEqual("html", mime.Content);

            type = null;

            actual = MimeType.TryParse(type, out mime);
            expected = false;
            Assert.AreEqual(expected, actual);

            Assert.IsNull(mime);
        }

        //
        // Qualified
        //


        /// <summary>
        ///A test for MimeType Constructor with a simple root
        ///</summary>
        [TestMethod()]
        [TestCategory("Common")]
        public void QualifiedMimeTypeConstructor_Test()
        {
            string expected = "text";
            QualifiedMimeType mime = new QualifiedMimeType(expected, null, null);


            Assert.IsTrue(mime.IsValid);
            Assert.AreEqual(expected, mime.ToString());
            Assert.AreEqual(expected, mime.Root);
            Assert.IsTrue(string.IsNullOrEmpty(mime.Base));
            Assert.IsTrue(string.IsNullOrEmpty(mime.Content));
            Assert.IsTrue(string.IsNullOrEmpty(mime.Charset));
            Assert.IsTrue(string.IsNullOrEmpty(mime.Variant));


        }

        /// <summary>
        ///A test for MimeType Constructor with a simple root/content
        ///</summary>
        [TestMethod()]
        [TestCategory("Common")]
        public void QualifiedMimeTypeConstructor2_Test()
        {
            string expected = "text/html";
            string expectedCharset = "UTF-8";
            QualifiedMimeType mime = new QualifiedMimeType(expected, expectedCharset, null);


            Assert.IsTrue(mime.IsValid);
            Assert.AreEqual(expected + "; charset=" + expectedCharset, mime.ToString());
            Assert.AreEqual("text", mime.Root);
            Assert.IsTrue(string.IsNullOrEmpty(mime.Base));
            Assert.AreEqual("html", mime.Content);
            Assert.AreEqual(expectedCharset, mime.Charset);
            Assert.IsTrue(string.IsNullOrEmpty(mime.Variant));

        }

        /// <summary>
        ///A test for MimeType Constructor with a simple root/content
        ///</summary>
        [TestMethod()]
        [TestCategory("Common")]
        public void QualifiedMimeTypeConstructor3_Test()
        {
            string expected = "text/html";
            string expectedCharset = "UTF-8";
            string expectedVariant = "Original";

            QualifiedMimeType mime = new QualifiedMimeType(expected, expectedCharset, expectedVariant);


            Assert.IsTrue(mime.IsValid);
            Assert.AreEqual(expected + "; charset=" + expectedCharset + "; variant=" + expectedVariant,
                mime.ToString());

            Assert.AreEqual("text", mime.Root);
            Assert.IsTrue(string.IsNullOrEmpty(mime.Base));
            Assert.AreEqual("html", mime.Content);
            Assert.AreEqual(expectedCharset, mime.Charset);
            Assert.AreEqual(expectedVariant, mime.Variant);

        }

        /// <summary>
        ///A test for MimeType Constructor with a simple root/content
        ///</summary>
        [TestMethod()]
        [TestCategory("Common")]
        public void QualifiedMimeTypeTryParse_Test()
        {
            bool expected = true;
            var type = "application/xhtml+xml; charset=UTF-8";

            MimeType mime;
            bool actual = MimeType.TryParse(type, out mime);

            Assert.AreEqual(expected, actual);
            Assert.IsInstanceOfType(mime, typeof(QualifiedMimeType));
            var qualified = (QualifiedMimeType)mime;



            Assert.AreEqual("application", qualified.Root);
            Assert.AreEqual("xml", qualified.Base);
            Assert.AreEqual("xhtml", qualified.Content);
            Assert.AreEqual("UTF-8", qualified.Charset);
            Assert.IsTrue(string.IsNullOrEmpty(qualified.Variant));

            type = "application/xhtml+xml; charset=UTF-8; variant=Scryber";

            actual = MimeType.TryParse(type, out mime);

            Assert.AreEqual(expected, actual);
            Assert.IsInstanceOfType(mime, typeof(QualifiedMimeType));
            qualified = (QualifiedMimeType)mime;

            Assert.AreEqual("application", mime.Root);
            Assert.AreEqual("xml", qualified.Base);
            Assert.AreEqual("xhtml", qualified.Content);
            Assert.AreEqual("UTF-8", qualified.Charset);
            Assert.AreEqual("Scryber", qualified.Variant);

            type = "application/xhtml+xml; variant=Scryber";

            actual = MimeType.TryParse(type, out mime);

            Assert.AreEqual(expected, actual);
            Assert.IsInstanceOfType(mime, typeof(QualifiedMimeType));
            qualified = (QualifiedMimeType)mime;

            Assert.AreEqual("application", mime.Root);
            Assert.AreEqual("xml", qualified.Base);
            Assert.AreEqual("xhtml", qualified.Content);
            Assert.IsTrue(String.IsNullOrEmpty(qualified.Charset));
            Assert.AreEqual("Scryber", qualified.Variant);
        }


    }
}
