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
using Scryber.PDF.Native;

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
                  "<title>Parse Single Frame With Inner Content</title>" +
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
                  "<title>Parse Single Frame With Mapped Path</title>" +
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

    

    [TestMethod]
    public void ParseSingleFrameWithTenInnerPages()
    {
        const string ExpressionsPDFPath =
            "https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/master/docs/images/Crib%20Sheet%20-%20Expressions.pdf";
        const int startIndex = 11;
        const int pageCount = 10;
        PDFObjectRef firstPageRef = new PDFObjectRef(125, 0);
        var src = "<html>" +
                  "<head>" +
                  "<title>Parse Single Frame With Ten Inner Pages</title>" +
                  "</head>" +
                  "<frameset>" +
                  "<frame src='" + ExpressionsPDFPath + "' data-page-start='" + startIndex + "' data-page-count='" + pageCount + "' />" +
                  "</frameset>" +
                  "</html>";
        using (var stream = new StringReader(src))
        {
            var doc = Document.ParseHtmlDocument(stream, ParseSourceType.DynamicContent) as HTMLDocument;

            Assert.IsNotNull(doc);
            
            Assert.IsNull(doc.Body);
            Assert.IsNotNull(doc.Frameset);

            Assert.IsNotNull(doc.Frameset.Frames);
            Assert.AreEqual(1, doc.Frameset.Frames.Count);

            var frame = doc.Frameset.Frames[0];
            Assert.IsNotNull(frame);
            Assert.AreEqual(ExpressionsPDFPath, frame.RemoteSource);

            Assert.AreEqual(startIndex, frame.PageStartIndex);
            Assert.AreEqual(pageCount, frame.PageInsertCount);

            var path = frame.MapPath(frame.RemoteSource);
            Assert.AreEqual( ExpressionsPDFPath, path);
            
            using (var sr = DocStreams.GetOutputStream("Frameset_Pages_11_to_21.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.SaveAsPDF(sr);

                sr.Position = 0;

                PDFFile file = PDFFile.Load(sr, doc.TraceLog);
                Assert.IsNotNull(file);
                Assert.IsNotNull(file.PageTree);
                
                var tree = file.AssertGetContent(file.PageTree) as PDFDictionary;
                Assert.IsNotNull(tree);
                
                var array = tree["Kids"] as PDFArray;
                Assert.IsNotNull(array);
                
                Assert.AreEqual(pageCount, array.Count);
                
                var first = array[0] as PDFObjectRef;
                Assert.IsNotNull(first);
                
                Assert.AreEqual(firstPageRef.Number, first.Number);
                Assert.AreEqual(firstPageRef.Generation, first.Generation);
            }

        }
    }
    
    [TestMethod]
    public void ParseSingleFrameWithFinalPages()
    {
        const string ExpressionsPDFPath =
            "https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/master/docs/images/Crib%20Sheet%20-%20Expressions.pdf";
        const int startIndex = 110;
        const int pageCount = 141 - startIndex;
        
        PDFObjectRef firstPageRef = new PDFObjectRef(895, 0);
        var src = "<html>" +
                  "<head>" +
                  "<title>Parse Single Frame With Final Pages</title>" +
                  "</head>" +
                  "<frameset>" +
                  "<frame src='" + ExpressionsPDFPath + "' data-page-start='" + startIndex + "' />" + //default page count should run to the end of the document
                  "</frameset>" +
                  "</html>";
        using (var stream = new StringReader(src))
        {
            var doc = Document.ParseHtmlDocument(stream, ParseSourceType.DynamicContent) as HTMLDocument;

            Assert.IsNotNull(doc);
            
            Assert.IsNull(doc.Body);
            Assert.IsNotNull(doc.Frameset);

            Assert.IsNotNull(doc.Frameset.Frames);
            Assert.AreEqual(1, doc.Frameset.Frames.Count);

            var frame = doc.Frameset.Frames[0];
            Assert.IsNotNull(frame);
            Assert.AreEqual(ExpressionsPDFPath, frame.RemoteSource);

            Assert.AreEqual(startIndex, frame.PageStartIndex);
            Assert.AreEqual(int.MaxValue, frame.PageInsertCount);

            var path = frame.MapPath(frame.RemoteSource);
            Assert.AreEqual( ExpressionsPDFPath, path);
            
            using (var sr = DocStreams.GetOutputStream("Frameset_last_31_pages.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.SaveAsPDF(sr);

                sr.Position = 0;

                PDFFile file = PDFFile.Load(sr, doc.TraceLog);
                Assert.IsNotNull(file);
                Assert.IsNotNull(file.PageTree);
                
                var tree = file.AssertGetContent(file.PageTree) as PDFDictionary;
                Assert.IsNotNull(tree);
                
                var array = tree["Kids"] as PDFArray;
                Assert.IsNotNull(array);
                
                Assert.AreEqual(pageCount, array.Count);
                
                var first = array[0] as PDFObjectRef;
                Assert.IsNotNull(first);
                
                Assert.AreEqual(firstPageRef.Number, first.Number);
                Assert.AreEqual(firstPageRef.Generation, first.Generation);
            }

        }
    }

    [TestMethod]
    public void ParseSingleFrameWithFirstPages()
    {
        const string ExpressionsPDFPath =
            "https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/master/docs/images/Crib%20Sheet%20-%20Expressions.pdf";
        const int startIndex = 0;
        const int pageCount = 21;

        PDFObjectRef firstPageRef = new PDFObjectRef(3, 0);
        PDFObjectRef lastPageRef = new PDFObjectRef(224, 0);
        
        var src = "<html>" +
                  "<head>" +
                  "<title>Parse Single Frame With First Pages</title>" +
                  "</head>" +
                  "<frameset>" +
                  "<frame src='" + ExpressionsPDFPath + "' data-page-count='" + pageCount +
                  "' />" + //default page start should be zero
                  "</frameset>" +
                  "</html>";
        using (var stream = new StringReader(src))
        {
            var doc = Document.ParseHtmlDocument(stream, ParseSourceType.DynamicContent) as HTMLDocument;

            Assert.IsNotNull(doc);
            
            Assert.IsNull(doc.Body);
            Assert.IsNotNull(doc.Frameset);

            Assert.IsNotNull(doc.Frameset.Frames);
            Assert.AreEqual(1, doc.Frameset.Frames.Count);

            var frame = doc.Frameset.Frames[0];
            Assert.IsNotNull(frame);
            Assert.AreEqual(ExpressionsPDFPath, frame.RemoteSource);

            Assert.AreEqual(0, frame.PageStartIndex);
            Assert.AreEqual(pageCount, frame.PageInsertCount);

            var path = frame.MapPath(frame.RemoteSource);
            Assert.AreEqual(ExpressionsPDFPath, path);

            using (var sr = DocStreams.GetOutputStream("Frameset_first_21_pages.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.SaveAsPDF(sr);

                sr.Position = 0;

                PDFFile file = PDFFile.Load(sr, doc.TraceLog);
                Assert.IsNotNull(file);
                Assert.IsNotNull(file.PageTree);

                var tree = file.AssertGetContent(file.PageTree) as PDFDictionary;
                Assert.IsNotNull(tree);

                var array = tree["Kids"] as PDFArray;
                Assert.IsNotNull(array);

                Assert.AreEqual(pageCount, array.Count);

                var first = array[0] as PDFObjectRef;
                Assert.IsNotNull(first);

                Assert.AreEqual(firstPageRef.Number, first.Number);
                Assert.AreEqual(firstPageRef.Generation, first.Generation);

                var last = array[array.Count - 1] as PDFObjectRef;
                Assert.IsNotNull(last);
                
                Assert.AreEqual(lastPageRef.Number, last.Number);
                Assert.AreEqual(lastPageRef.Generation, last.Generation);
            }

        }
    }
    
    [TestMethod]
    public void ParseTwoFramesWithSectionsOfSameDocument()
    {
        const string ExpressionsPDFPath =
            "https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/master/docs/images/Crib%20Sheet%20-%20Expressions.pdf";

        const int firstStartIndex = 0;
        const int secondStartIndex = 100;
        const int firstPageCount = 21;
        const int secondPageCount = 11;

        PDFObjectRef firstfirstPageRef = new PDFObjectRef(3, 0);
        PDFObjectRef firstLastPageRef = new PDFObjectRef(224, 0);
        PDFObjectRef lastfirstPageRef = new PDFObjectRef(835, 0);
        PDFObjectRef lastlastPageRef = new PDFObjectRef(895, 0);
        
        var src = "<html>" +
                  "<head>" +
                  "<title>Parse Two Frames With Sections Of The Same Document</title>" +
                  "</head>" +
                  "<frameset>" +
                  "<frame src='" + ExpressionsPDFPath + "' data-page-start='" + firstStartIndex + "' data-page-count='" + firstPageCount + "' />" +
                  "<frame src='" + ExpressionsPDFPath + "' data-page-start='" + secondStartIndex + "' data-page-count='" + secondPageCount + "' />" +
                  "</frameset>" +
                  "</html>";
        using (var stream = new StringReader(src))
        {
            var doc = Document.ParseHtmlDocument(stream, ParseSourceType.DynamicContent) as HTMLDocument;

            Assert.IsNotNull(doc);
            
            Assert.IsNull(doc.Body);
            Assert.IsNotNull(doc.Frameset);

            Assert.IsNotNull(doc.Frameset.Frames);
            Assert.AreEqual(2, doc.Frameset.Frames.Count);

            var frame = doc.Frameset.Frames[0];
            Assert.IsNotNull(frame);
            Assert.AreEqual(ExpressionsPDFPath, frame.RemoteSource);

            Assert.AreEqual(firstStartIndex, frame.PageStartIndex);
            Assert.AreEqual(firstPageCount, frame.PageInsertCount);
            
            frame = doc.Frameset.Frames[1];
            Assert.IsNotNull(frame);
            Assert.AreEqual(ExpressionsPDFPath, frame.RemoteSource);

            Assert.AreEqual(secondStartIndex, frame.PageStartIndex);
            Assert.AreEqual(secondPageCount, frame.PageInsertCount);

            var path = frame.MapPath(frame.RemoteSource);
            Assert.AreEqual(ExpressionsPDFPath, path);

            using (var sr = DocStreams.GetOutputStream("Frameset_twoSections_sameDoc.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.SaveAsPDF(sr);

                sr.Position = 0;

                PDFFile file = PDFFile.Load(sr, doc.TraceLog);
                Assert.IsNotNull(file);
                Assert.IsNotNull(file.PageTree);

                var tree = file.AssertGetContent(file.PageTree) as PDFDictionary;
                Assert.IsNotNull(tree);

                var array = tree["Kids"] as PDFArray;
                Assert.IsNotNull(array);

                Assert.AreEqual(firstPageCount + secondPageCount, array.Count);

                var first = array[0] as PDFObjectRef;
                Assert.IsNotNull(first);

                Assert.AreEqual(firstfirstPageRef.Number, first.Number);
                Assert.AreEqual(firstfirstPageRef.Generation, first.Generation);
                
                first = array[firstPageCount - 1] as PDFObjectRef;
                Assert.IsNotNull(first);

                Assert.AreEqual(firstLastPageRef.Number, first.Number);
                Assert.AreEqual(firstLastPageRef.Generation, first.Generation);

                var last = array[firstPageCount] as PDFObjectRef;
                Assert.IsNotNull(last);
                
                Assert.AreEqual(lastfirstPageRef.Number, last.Number);
                Assert.AreEqual(lastfirstPageRef.Generation, last.Generation);
                
                last = array[array.Count - 1] as PDFObjectRef;
                Assert.IsNotNull(last);
                
                Assert.AreEqual(lastlastPageRef.Number, last.Number);
                Assert.AreEqual(lastlastPageRef.Generation, last.Generation);
            }

        }
    }
    
    
    [TestMethod]
    public void ParseTwoFramesWithFirstPagesAndRemoteTemplate()
    {
        const string ExpressionsPDFPath =
            "https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/master/docs/images/Crib%20Sheet%20-%20Expressions.pdf";
        const string RemoteTemplatePath = 
            "https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/master/Scryber.UnitTest/Content/HTML/HelloWorld.xhtml";
        const int startIndex = 0;
        const int pageCount = 21;

        PDFObjectRef firstPageRef = new PDFObjectRef(3, 0);
        PDFObjectRef penultimatePageRef = new PDFObjectRef(224, 0);
        PDFObjectRef lastPageRef = new PDFObjectRef(1340, 0);
        
        var src = "<html>" +
                  "<head>" +
                  "<title>Parse Single Frame With First Pages</title>" +
                  "</head>" +
                  "<frameset>" +
                  "<frame src='" + ExpressionsPDFPath + "' data-page-count='" + pageCount +
                  "' />" + //default page start should be zero
                  "<frame src='" + RemoteTemplatePath +  "' />" + //Add all the pages from the template after the others
                  "</frameset>" +
                  "</html>";
        using (var stream = new StringReader(src))
        {
            var doc = Document.ParseHtmlDocument(stream, ParseSourceType.DynamicContent) as HTMLDocument;

            Assert.IsNotNull(doc);
            
            Assert.IsNull(doc.Body);
            Assert.IsNotNull(doc.Frameset);

            Assert.IsNotNull(doc.Frameset.Frames);
            Assert.AreEqual(2, doc.Frameset.Frames.Count);

            var frame = doc.Frameset.Frames[0];
            Assert.IsNotNull(frame);
            Assert.AreEqual(ExpressionsPDFPath, frame.RemoteSource);

            Assert.AreEqual(0, frame.PageStartIndex);
            Assert.AreEqual(pageCount, frame.PageInsertCount);

            var path = frame.MapPath(frame.RemoteSource);
            Assert.AreEqual(ExpressionsPDFPath, path);
            
            frame = doc.Frameset.Frames[1];
            Assert.IsNotNull(frame);
            Assert.AreEqual(RemoteTemplatePath, frame.RemoteSource);

            Assert.AreEqual(0, frame.PageStartIndex);
            Assert.AreEqual(int.MaxValue, frame.PageInsertCount);

            path = frame.MapPath(frame.RemoteSource);
            Assert.AreEqual(RemoteTemplatePath, path);

            using (var sr = DocStreams.GetOutputStream("Frameset_21_pages_1_template.pdf"))
            {
                doc.Params["title"] = "Document title from the outer frameset.";
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.SaveAsPDF(sr);

                sr.Position = 0;

                PDFFile file = PDFFile.Load(sr, doc.TraceLog);
                Assert.IsNotNull(file);
                Assert.IsNotNull(file.PageTree);

                var tree = file.AssertGetContent(file.PageTree) as PDFDictionary;
                Assert.IsNotNull(tree);

                var array = tree["Kids"] as PDFArray;
                Assert.IsNotNull(array);

                Assert.AreEqual(pageCount + 1, array.Count); //Add one for the inserted template

                var first = array[0] as PDFObjectRef;
                Assert.IsNotNull(first);

                Assert.AreEqual(firstPageRef.Number, first.Number);
                Assert.AreEqual(firstPageRef.Generation, first.Generation);

                var penultimate = array[array.Count - 2] as PDFObjectRef;
                Assert.IsNotNull(penultimate);
                
                Assert.AreEqual(penultimatePageRef.Number, penultimate.Number);
                Assert.AreEqual(penultimatePageRef.Generation, penultimate.Generation);
                
                var last = array[array.Count - 1] as PDFObjectRef;
                Assert.IsNotNull(last);
                
                Assert.AreEqual(lastPageRef.Number, last.Number);
                Assert.AreEqual(lastPageRef.Generation, last.Generation);
            }

        }
    }

}
