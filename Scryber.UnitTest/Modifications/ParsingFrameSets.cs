using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Html.Components;
using Scryber.Styles;
using Scryber.Drawing;
using Scryber.Styles.Parsing;

using Scryber.PDF.Layout;
using Scryber.PDF;

using System.Diagnostics;
using System.IO;
using Scryber.Text;
using Scryber.PDF.Resources;

namespace Scryber.Core.UnitTests.Modifications;

[TestClass()]
public class ParsingFrameSets_Test
{
    private TestContext testContextInstance;

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext
    {
        get { return testContextInstance; }
        set { testContextInstance = value; }
    }

    [TestMethod]
    public void ParseSingleFrame()
    {
        var src = "<html>" +
                  "<head>" +
                  "<title>With Frame</title>" +
                  "</head>" +
                  "<frameset>" +
                  "<frame src='./content.pdf' data-page-start='1' data-page-count='2' />" +
                  "</frameset>" +
                  "</html>";
        using (var stream = new StringReader(src))
        {
            var doc = Document.ParseHtmlDocument(stream, ParseSourceType.DynamicContent) as HTMLDocument;

            Assert.IsNotNull(doc);
            Assert.AreEqual("With Frame", doc.Head.Title);

            Assert.IsNull(doc.Body);
            Assert.IsNotNull(doc.Frameset);

            Assert.IsNotNull(doc.Frameset.Frames);
            Assert.AreEqual(1, doc.Frameset.Frames.Count);

            var frame = doc.Frameset.Frames[0];
            Assert.IsNotNull(frame);
            Assert.AreEqual("./content.pdf", frame.RemoteSource);

            Assert.AreEqual(1, frame.PageStartIndex);
            Assert.AreEqual(2, frame.PageInsertCount);
        }


    }

    [TestMethod]
    public void ParseSingleFrameAndBodyInvalid()
    {
        var src = "<?scryber parser-mode='strict' ?>" +
                  "<html>" +
                  "<head>" +
                  "<title>With Frame</title>" +
                  "</head>" +
                  "<frameset>" +
                  "<frame src='./content.pdf' />" +
                  "</frameset>" +
                  "<body>This will throw an error</body>" +
                  "</html>";
        using (var stream = new StringReader(src))
        {
            var caught = false;
            try
            {
                var doc = Document.ParseHtmlDocument(stream, ParseSourceType.DynamicContent) as HTMLDocument;
            }
            catch (PDFParserException)
            {
                caught = true;
            }

            Assert.IsTrue(caught, "No exception was raised for both frameset and body.");
        }

        src = "<?scryber parser-mode='strict' ?>" +
              "<html>" +
              "<head>" +
              "<title>With Frame</title>" +
              "</head>" +
              "<body>This will be set, but frameset will throw an error.</body>" +
              "<frameset>" +
              "<frame src='./content.pdf' />" +
              "</frameset>" +
              "</html>";

        using (var stream = new StringReader(src))
        {
            var caught = false;
            try
            {
                var doc = Document.ParseHtmlDocument(stream, ParseSourceType.DynamicContent) as HTMLDocument;
            }
            catch (PDFParserException)
            {
                caught = true;
            }

            Assert.IsTrue(caught, "No exception was raised for both frameset and body.");
        }

    }

    [TestMethod]
    public void ParseSingleFrameWithInnerContent()
    {
        var src = "<?scryber parser-mode='strict' ?>" +
                  "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                  "<head>" +
                  "<title>With Frame</title>" +
                  "</head>" +
                  "<frameset>" +
                  "<frame>" +
                  "<html><body><h1>This is the inner heading</h1></body></html>" +
                  "</frame>" +
                  "</frameset>" +
                  "</html>";
        using (var stream = new StringReader(src))
        {
            var doc = Document.ParseDocument(stream, ParseSourceType.DynamicContent) as HTMLDocument;

            Assert.IsNotNull(doc);
            Assert.AreEqual("With Frame", doc.Head.Title);

            Assert.IsNull(doc.Body);
            Assert.IsNotNull(doc.Frameset);

            Assert.IsNotNull(doc.Frameset.Frames);
            Assert.AreEqual(1, doc.Frameset.Frames.Count);

            var frame = doc.Frameset.Frames[0];
            Assert.IsNotNull(frame);

            var innerDoc = frame.InnerHtml;
            Assert.IsNotNull(innerDoc);


            Assert.IsNotNull(innerDoc.Body);
            Assert.AreEqual(1, innerDoc.Body.Contents.Count);

            var h1 = innerDoc.Body.Contents[0] as Head1;
            Assert.IsNotNull(h1);
            Assert.AreEqual(1, h1.Contents.Count);
            var lit = h1.Contents[0] as TextLiteral;
            Assert.IsNotNull(lit);
            Assert.AreEqual("This is the inner heading", lit.Text);
        }


    }

    [TestMethod]
    public void ParseSingleFrameWithMappedPath()
    {
        var src = "<html>" +
                  "<head>" +
                  "<title>With Frame</title>" +
                  "<base href='https://github.com/richard-scryber/scryber.core/tree/94c4b84820d6d239c8c56cbec01a62beddc943e9/docs/images/' />" +
                  "</head>" +
                  "<frameset>" +
                  "<frame src='./content.pdf' data-page-start='1' data-page-count='2' />" +
                  "</frameset>" +
                  "</html>";
        using (var stream = new StringReader(src))
        {
            var doc = Document.ParseHtmlDocument(stream, ParseSourceType.DynamicContent) as HTMLDocument;

            Assert.IsNotNull(doc);
            Assert.AreEqual("With Frame", doc.Head.Title);

            Assert.IsNull(doc.Body);
            Assert.IsNotNull(doc.Frameset);

            Assert.IsNotNull(doc.Frameset.Frames);
            Assert.AreEqual(1, doc.Frameset.Frames.Count);

            var frame = doc.Frameset.Frames[0];
            Assert.IsNotNull(frame);
            Assert.AreEqual("./content.pdf", frame.RemoteSource);

            Assert.AreEqual(1, frame.PageStartIndex);
            Assert.AreEqual(2, frame.PageInsertCount);

            var path = frame.MapPath(frame.RemoteSource);
            Assert.AreEqual(
                "https://github.com/richard-scryber/scryber.core/tree/94c4b84820d6d239c8c56cbec01a62beddc943e9/docs/images/content.pdf",
                path);

        }
    }

    private const string ExpressionsPDFPath =
        "https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/master/docs/images/Crib%20Sheet%20-%20Expressions.pdf";


    [TestMethod]
    public void ParseSingleFrameWithTwoPages()
    {
        var src = "<html>" +
                  "<head>" +
                  "<title>With Frame</title>" +
                  "</head>" +
                  "<frameset>" +
                  "<frame src='" + ExpressionsPDFPath + "' data-page-start='1' data-page-count='2' />" +
                  "</frameset>" +
                  "</html>";
        using (var stream = new StringReader(src))
        {
            var doc = Document.ParseHtmlDocument(stream, ParseSourceType.DynamicContent) as HTMLDocument;

            Assert.IsNotNull(doc);
            Assert.AreEqual("With Frame", doc.Head.Title);

            Assert.IsNull(doc.Body);
            Assert.IsNotNull(doc.Frameset);

            Assert.IsNotNull(doc.Frameset.Frames);
            Assert.AreEqual(1, doc.Frameset.Frames.Count);

            var frame = doc.Frameset.Frames[0];
            Assert.IsNotNull(frame);
            Assert.AreEqual(ExpressionsPDFPath, frame.RemoteSource);

            Assert.AreEqual(1, frame.PageStartIndex);
            Assert.AreEqual(2, frame.PageInsertCount);

            var path = frame.MapPath(frame.RemoteSource);
            Assert.AreEqual( ExpressionsPDFPath, path);
            
            using (var sr = DocStreams.GetOutputStream("Frameset_2_pages.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.SaveAsPDF(sr);
            }

        }
    }

}
