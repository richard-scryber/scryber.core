using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Styles;
using Scryber.Drawing;
using System.Xml.Linq;
using System.IO;

namespace Scryber.UnitSamples
{
    [TestClass]
    public class OverviewSamples : SampleBase
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
        public void SimpleParsing()
        {
            var path = GetTemplatePath("Overview", "SimpleParsing.html");

            using (var doc = Document.ParseDocument(path))
            {
                using (var stream = GetOutputStream("Overview", "SimpleParsing.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

            }
        }

        [TestMethod]
        public void XLinqParsing()
        {

            XNamespace ns = "http://www.w3.org/1999/xhtml";

            var html = new XElement(ns + "html",
                new XElement(ns + "head",
                    new XElement(ns + "title",
                        new XText("Hello World"))
                    ),
                new XElement(ns + "body",
                    new XElement(ns + "div",
                        new XAttribute("style", "padding:10px"),
                        new XText("Hello World."))
                    )
                );

            using (var reader = html.CreateReader())
            {
                //passing an empty string to the path as we don't have images or other references to load
                using (var doc = Document.ParseDocument(reader, string.Empty, ParseSourceType.DynamicContent))
                {
                    using (var stream = GetOutputStream("Overview", "XLinqParsing.pdf"))
                    {
                        doc.SaveAsPDF(stream);
                    }
                }
            }
        }


        [TestMethod]
        public void StringParsing()
        {
            var title = "Hello World";
            var src = @"<html xmlns='http://www.w3.org/1999/xhtml' >
                    <head>
                        <title>" + title + @"</title>
                    </head>
                    <body>
                        <div style='padding: 10px' >" + title + @".</div>
                    </body>
                </html>";

            using (var reader = new StringReader(src))
            {
                using (var doc = Document.ParseDocument(reader, string.Empty, ParseSourceType.DynamicContent))
                {
                    using (var stream = GetOutputStream("Overview", "StringParsing.pdf"))
                    {
                        doc.SaveAsPDF(stream);
                    }
                }
            }
        }


        protected Document GetHelloWorld()
        {
            var doc = new Document();
            doc.Info.Title = "Hello World";

            var page = new Page();
            doc.Pages.Add(page);

            var div = new Div() { Padding = new PDFThickness(10) };
            page.Contents.Add(div);

            div.Contents.Add(new TextLiteral("Hello World"));

            return doc;
        }


        [TestMethod]
        public void DocumentInCode()
        {

            using (var doc = GetHelloWorld())
            {
                using (var stream = GetOutputStream("Overview", "CodedDocument.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }
            }
        }

        [TestMethod()]
        public void EmbedContent()
        {
            var path = GetTemplatePath("Overview", "EmbeddedContent.html");

            using (var doc = Document.ParseDocument(path))
            {
                using (var stream = GetOutputStream("Overview", "EmbeddedContent.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

            }
        }
    }
}
