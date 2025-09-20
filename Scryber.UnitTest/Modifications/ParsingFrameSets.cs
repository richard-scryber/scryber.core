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
using Scryber.Modifications;
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

    //
    // sections of a single document
    //
    
    [TestMethod]
    public void Frameset_01_ParseSingleFrame()
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
    public void Frameset_02_ParseSingleFrameAndBodyInvalid()
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
    public void Frameset_03_ParseSingleFrameWithInnerContent()
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
    public void Frameset_04_ParseSingleFrameWithMappedPath()
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
    public void Frameset_05_ParseSingleFrameWithTenInnerPages()
    {
        const string ExpressionsPDFPath =
            "https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/master/docs/images/Crib%20Sheet%20-%20Expressions.pdf";
        const int startIndex = 11;
        const int pageCount = 10;
        PDFObjectRef firstPageRef = new PDFObjectRef(1340, 0);
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
    public void Frameset_06_ParseSingleFrameWithFinalPages()
    {
        const string ExpressionsPDFPath =
            "https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/master/docs/images/Crib%20Sheet%20-%20Expressions.pdf";
        const int startIndex = 110;
        const int pageCount = 141 - startIndex;
        
        PDFObjectRef firstPageRef = new PDFObjectRef(1340, 0);
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
    public void Frameset_07_ParseSingleFrameWithFirstPages()
    {
        const string ExpressionsPDFPath =
            "https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/master/docs/images/Crib%20Sheet%20-%20Expressions.pdf";
        const int startIndex = 0;
        const int pageCount = 21;

        PDFObjectRef firstPageRef = new PDFObjectRef(1340, 0);
        PDFObjectRef lastPageRef = new PDFObjectRef(1360, 0);
        
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
    
    
    //
    // 2 documents merged
    //
    
    [TestMethod]
    public void Frameset_10_ParseTwoFramesWithSectionsOfSameDocument()
    {
        const string ExpressionsPDFPath =
            "https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/master/docs/images/Crib%20Sheet%20-%20Expressions.pdf";

        const int firstStartIndex = 0;
        const int secondStartIndex = 100;
        const int firstPageCount = 21;
        const int secondPageCount = 11;

        PDFObjectRef firstfirstPageRef = new PDFObjectRef(1340, 0);
        PDFObjectRef firstLastPageRef = new PDFObjectRef(1360, 0);
        PDFObjectRef lastfirstPageRef = new PDFObjectRef(1361, 0);
        PDFObjectRef lastlastPageRef = new PDFObjectRef(1371, 0);
        
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
                
                //Check the frame references - should be only 1 even though there are 2 frames
                Assert.IsNotNull(doc.Frameset.RootReference);
                Assert.IsNotNull(doc.Frameset.DependantReferences);
                Assert.AreEqual(0, doc.Frameset.DependantReferences.Count);
            }

        }
    }
    
    
    [TestMethod]
    public void Frameset_11_ParseTwoFramesWithFirstPagesAndRemoteTemplate()
    {
        const string ExpressionsPDFPath =
            "https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/master/docs/images/Crib%20Sheet%20-%20Expressions.pdf";
        const string RemoteTemplatePath = 
            "https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/master/Scryber.UnitTest/Content/HTML/HelloWorld.xhtml";
        const int startIndex = 0;
        const int pageCount = 21;

        PDFObjectRef firstPageRef = new PDFObjectRef(1350, 0);
        PDFObjectRef penultimatePageRef = new PDFObjectRef(1370, 0);
        PDFObjectRef lastPageRef = new PDFObjectRef(1371, 0);
        
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

    [TestMethod]
    public void Frameset_12_ParseTwoFramesWithFirstPagesAndInlineTemplate()
    {
        const string ExpressionsPDFPath =
            "https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/master/docs/images/Crib%20Sheet%20-%20Expressions.pdf";
        const string TemplateContent = "<html id='inner'>\n" +
                                             "<head>\n    " +
                                             "<title>{@:title}</title>\n" + //We set the base in the template to load  the relative image.
                                             "<base href='https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/master/Scryber.UnitTest/Content/HTML' />\n" +
                                             "</head>\n" +
                                             "<body id='innerBody'>\n" +
                                             "    <div id='div1' title='{@:title}' style='padding:10px; page-break-after: always'>{@:title}</div>\n" +
                                             "    <a href='#div1' id='div2' style=\"padding:10pt\" >&lt;&lsquo;Inside the template&rsquo;&gt;</a>\n" +
                                             "    <img id='img1' src=\"https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/master/Scryber.UnitTest/Content/HTML/Images/group.png\" style=\"width:100pt; padding:10px\" />\n" +
                                             "</body>\n" +
                                             "</html>";
        const int startIndex = 0;
        const int pageCount = 21;

        PDFObjectRef firstPageRef = new PDFObjectRef(1359, 0);
        PDFObjectRef lastFirstPageRef = new PDFObjectRef(1379, 0);

        PDFObjectRef firstLastPageRef = new PDFObjectRef(1380, 0);
        PDFObjectRef lastLastPageRef = new PDFObjectRef(1381, 0);
        
        var src = "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                  "<head>" +
                  "<title>Parse Single Frame With First Pages</title>" +
                  "</head>" +
                  "<frameset>" +
                  "<frame src='" + ExpressionsPDFPath + "' data-page-count='" + pageCount + "' />" + //default page start should be zero
                  "<frame>" + //Add the content within the frame
                  TemplateContent +
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
            Assert.IsNull(frame.RemoteSource);
            Assert.IsNotNull(frame.InnerHtml);

            var innerdoc = frame.InnerHtml;
            

            Assert.AreEqual(0, frame.PageStartIndex);
            Assert.AreEqual(int.MaxValue, frame.PageInsertCount);
            

            using (var sr = DocStreams.GetOutputStream("Frameset_21_pages_1_inline_template.pdf"))
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

                Assert.AreEqual(pageCount + 2, array.Count); //Add two for the inserted template

                var first = array[0] as PDFObjectRef;
                Assert.IsNotNull(first);

                Assert.AreEqual(firstPageRef.Number, first.Number);
                Assert.AreEqual(firstPageRef.Generation, first.Generation);

                var penultimate = array[array.Count - 3] as PDFObjectRef; // -3 = last page of the first doc
                Assert.IsNotNull(penultimate);
                
                Assert.AreEqual(lastFirstPageRef.Number, penultimate.Number);
                Assert.AreEqual(lastFirstPageRef.Generation, penultimate.Generation);
                
                var last = array[array.Count - 2] as PDFObjectRef;
                Assert.IsNotNull(last);
                
                Assert.AreEqual(firstLastPageRef.Number, last.Number);
                Assert.AreEqual(firstLastPageRef.Generation, last.Generation);
                
                last = array[array.Count - 1] as PDFObjectRef;
                Assert.IsNotNull(last);
                
                Assert.AreEqual(lastLastPageRef.Number, last.Number);
                Assert.AreEqual(lastLastPageRef.Generation, last.Generation);
                
                //Check the remote image is loaded.
                Assert.AreEqual(2, innerdoc.SharedResources.Count);
                var img = innerdoc.SharedResources[0] as PDFImageXObject;
                Assert.IsNotNull(img);
                Assert.AreNotEqual(Size.Empty, img.GetImageSize());
                Assert.AreEqual(198, img.GetImageSize().Width.PointsValue);
                Assert.AreEqual(171, img.GetImageSize().Height.PointsValue);
                
                //Check the bound text
                var div = innerdoc.FindAComponentById("div1") as Div;
                Assert.IsNotNull(div);
                Assert.AreEqual(1, div.Contents.Count);
                var lit = div.Contents[0] as TextLiteral;
                Assert.IsNotNull(lit);
                Assert.AreEqual("Document title from the outer frameset.", lit.Text);
            }

        }
    }
    
    [TestMethod]
    public void Frameset_13_ParseThreeFramesWithFirstPagesInlineTemplateAndLastPages()
    {
        const string ExpressionsPDFPath =
            "https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/master/docs/images/Crib%20Sheet%20-%20Expressions.pdf";
        const string TemplateContent = "<html id='inner'>\n" +
                                             "<head>\n    " +
                                             "<title>{@:title}</title>\n" +
                                             "</head>\n" +
                                             "<body id='innerBody'>\n" +
                                             "    <div id='div1' title='{@:title}' style='padding:10px; page-break-after: always'>{@:title}</div>\n" +
                                             "    <a href='#div1' id='div2' style=\"padding:10pt\" >&lt;&lsquo;Inside the template&rsquo;&gt;</a>\n" +
                                             "    <img id='img1' src=\"https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/master/Scryber.UnitTest/Content/HTML/Images/group.png\" style=\"width:100pt; padding:10px\" />\n" +
                                             "</body>\n" +
                                             "</html>";
        const int startIndex = 0;
        const int pageCount = 21;
        const int lastSetStart = 130;
        const int lastSetCount = 11;

        PDFObjectRef firstPageRef = new PDFObjectRef(1359, 0);
        PDFObjectRef lastFirstPageRef = new PDFObjectRef(1379, 0);

        PDFObjectRef firstTemplatePageRef = new PDFObjectRef(1380, 0);
        PDFObjectRef lastTemplatePageRef = new PDFObjectRef(1381, 0);
        
        PDFObjectRef first2ndSectionPageRef = new PDFObjectRef(1382, 0);
        PDFObjectRef last2ndSectionPageRef = new PDFObjectRef(1392, 0);
        
        var src = "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                  "<head>" +
                  "<title>Parse Single Frame With First Pages</title>" +
                  "</head>" +
                  "<frameset>" +
                  "<frame src='" + ExpressionsPDFPath + "' data-page-count='" + pageCount + "' />" + //default page start should be zero
                  "<frame>" + //Add the content within the frame
                  TemplateContent +
                  "</frame>" +
                  "<frame src='" + ExpressionsPDFPath + "' data-page-start='" + lastSetStart + "' />" + //default page start should be zero
                  "</frameset>" +
                  "</html>";
        using (var stream = new StringReader(src))
        {
            var doc = Document.ParseDocument(stream, ParseSourceType.DynamicContent) as HTMLDocument;

            Assert.IsNotNull(doc);
            
            Assert.IsNull(doc.Body);
            Assert.IsNotNull(doc.Frameset);

            Assert.IsNotNull(doc.Frameset.Frames);
            Assert.AreEqual(3, doc.Frameset.Frames.Count);

            var frame = doc.Frameset.Frames[0];
            Assert.IsNotNull(frame);
            Assert.AreEqual(ExpressionsPDFPath, frame.RemoteSource);

            Assert.AreEqual(0, frame.PageStartIndex);
            Assert.AreEqual(pageCount, frame.PageInsertCount);

            var path = frame.MapPath(frame.RemoteSource);
            Assert.AreEqual(ExpressionsPDFPath, path);
            
            frame = doc.Frameset.Frames[1];
            Assert.IsNotNull(frame);
            Assert.IsNull(frame.RemoteSource);
            Assert.IsNotNull(frame.InnerHtml);

            var innerdoc = frame.InnerHtml;
            

            Assert.AreEqual(0, frame.PageStartIndex);
            Assert.AreEqual(int.MaxValue, frame.PageInsertCount);


            frame = doc.Frameset.Frames[2];
            Assert.IsNotNull(frame);
            Assert.AreEqual(ExpressionsPDFPath, frame.RemoteSource);

            Assert.AreEqual(lastSetStart, frame.PageStartIndex);
            Assert.AreEqual(int.MaxValue, frame.PageInsertCount);

            path = frame.MapPath(frame.RemoteSource);
            Assert.AreEqual(ExpressionsPDFPath, path);
            

            using (var sr = DocStreams.GetOutputStream("Frameset_21_pages_2_inline_template_14_pages.pdf"))
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

                Assert.AreEqual(pageCount + 2 + lastSetCount, array.Count); //Add two for the inserted template

                var first = array[0] as PDFObjectRef;
                Assert.IsNotNull(first);

                Assert.AreEqual(firstPageRef.Number, first.Number);
                Assert.AreEqual(firstPageRef.Generation, first.Generation);

                var pgIndex = pageCount - 1;
                
                //last page of the first section
                var penultimate = array[pgIndex] as PDFObjectRef; 
                Assert.IsNotNull(penultimate);
                
                Assert.AreEqual(lastFirstPageRef.Number, penultimate.Number);
                Assert.AreEqual(lastFirstPageRef.Generation, penultimate.Generation);

                pgIndex = pageCount; //first template page
                var templateRef = array[pgIndex] as PDFObjectRef;
                Assert.IsNotNull(templateRef);
                
                Assert.AreEqual(firstTemplatePageRef.Number, templateRef.Number);
                Assert.AreEqual(firstTemplatePageRef.Generation, templateRef.Generation);

                pgIndex++; //second template page
                templateRef = array[pgIndex] as PDFObjectRef;
                Assert.IsNotNull(templateRef);
                
                Assert.AreEqual(lastTemplatePageRef.Number, templateRef.Number);
                Assert.AreEqual(lastTemplatePageRef.Generation, templateRef.Generation);

                pgIndex = pageCount + 2;
                //first page of the second pdf section
                var fstLastRef = array[pgIndex] as PDFObjectRef;
                Assert.IsNotNull(fstLastRef);
                
                Assert.AreEqual(first2ndSectionPageRef.Number, fstLastRef.Number);
                Assert.AreEqual(first2ndSectionPageRef.Generation, fstLastRef.Generation);
                
                
                //last page of the second pdf section
                var lastLastRef = array[array.Count-1] as PDFObjectRef;
                Assert.IsNotNull(lastLastRef);
                
                Assert.AreEqual(last2ndSectionPageRef.Number, lastLastRef.Number);
                Assert.AreEqual(last2ndSectionPageRef.Generation, lastLastRef.Generation);
                
                
                //Check the remote image is loaded.
                Assert.AreEqual(2, innerdoc.SharedResources.Count);
                var img = innerdoc.SharedResources[0] as PDFImageXObject;
                Assert.IsNotNull(img);
                Assert.AreNotEqual(Size.Empty, img.GetImageSize());
                Assert.AreEqual(198, img.GetImageSize().Width.PointsValue);
                Assert.AreEqual(171, img.GetImageSize().Height.PointsValue);
                
                //Check the bound text
                var div = innerdoc.FindAComponentById("div1") as Div;
                Assert.IsNotNull(div);
                Assert.AreEqual(1, div.Contents.Count);
                var lit = div.Contents[0] as TextLiteral;
                Assert.IsNotNull(lit);
                Assert.AreEqual("Document title from the outer frameset.", lit.Text);
            }

        }
    }

    [TestMethod]
    public void Frameset_14_ParseFourFramesWithFirstPagesRemoteOneMiddleAndLastInline()
    {
        const string ExpressionsPDFPath =
            "https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/master/docs/images/Crib%20Sheet%20-%20Expressions.pdf";
        const string TemplatePath = 
            "https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/modifications/Scryber.UnitTest/Content/HTML/HelloWorld2Page.xhtml";
        
        const int startIndex = 0;
        const int pageCount = 21;
        const int lastSetStart = 130;
        const int lastSetCount = 11;

        PDFObjectRef firstPageRef = new PDFObjectRef(1353, 0);
        PDFObjectRef lastFirstPageRef = new PDFObjectRef(1373, 0);

        PDFObjectRef firstTemplatePageRef = new PDFObjectRef(1374, 0);
        
        PDFObjectRef first2ndSectionPageRef = new PDFObjectRef(1375, 0);
        PDFObjectRef last2ndSectionPageRef = new PDFObjectRef(1385, 0);
        
        PDFObjectRef lastTemplatePageRef = new PDFObjectRef(1386, 0);
        
        var src = "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                  "<head>" +
                  "<title>Parse Single Frame With First Pages</title>" +
                  "</head>" +
                  "<frameset>" +
                  "<frame src='" + ExpressionsPDFPath + "' data-page-count='" + pageCount + "' />" + //default page start should be zero
                  "<frame src='" + TemplatePath + "' data-page-count='1' />" + //First page from the remote source
                  "<frame src='" + ExpressionsPDFPath + "' data-page-start='" + lastSetStart + "' data-page-count='" + lastSetCount + "' />" + //default page start should be zero
                  "<frame src='" + TemplatePath + "' data-page-start='1' />" + //Last page from the remote source
                  "</frameset>" +
                  "</html>";
        
        using (var stream = new StringReader(src))
        {
            var doc = Document.ParseDocument(stream, ParseSourceType.DynamicContent) as HTMLDocument;

            Assert.IsNotNull(doc);
            
            Assert.IsNull(doc.Body);
            Assert.IsNotNull(doc.Frameset);

            Assert.IsNotNull(doc.Frameset.Frames);
            Assert.AreEqual(4, doc.Frameset.Frames.Count);

            var frame = doc.Frameset.Frames[0];
            Assert.IsNotNull(frame);
            Assert.AreEqual(ExpressionsPDFPath, frame.RemoteSource);
            Assert.AreEqual(0, frame.PageStartIndex);
            Assert.AreEqual(pageCount, frame.PageInsertCount);

            var path = frame.MapPath(frame.RemoteSource);
            Assert.AreEqual(ExpressionsPDFPath, path);
            
            frame = doc.Frameset.Frames[1];
            Assert.IsNotNull(frame);
            Assert.IsNotNull(frame.RemoteSource);
            Assert.AreEqual(TemplatePath, frame.RemoteSource);
            Assert.AreEqual(0, frame.PageStartIndex);
            Assert.AreEqual(1, frame.PageInsertCount);

            frame = doc.Frameset.Frames[2];
            Assert.IsNotNull(frame);
            Assert.AreEqual(ExpressionsPDFPath, frame.RemoteSource);
            Assert.AreEqual(130, frame.PageStartIndex);
            Assert.AreEqual(lastSetCount, frame.PageInsertCount);
            
            frame = doc.Frameset.Frames[3];
            Assert.IsNotNull(frame);
            Assert.IsNotNull(frame.RemoteSource);
            Assert.AreEqual(TemplatePath, frame.RemoteSource);
            Assert.AreEqual(1, frame.PageStartIndex);
            Assert.AreEqual(int.MaxValue, frame.PageInsertCount);

            

            using (var sr = DocStreams.GetOutputStream("Frameset_14_21_pdf_1_remote_11_pdf_1_remote.pdf"))
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

                Assert.AreEqual(pageCount + 2 + lastSetCount, array.Count); //Add two for the inserted template

                var first = array[0] as PDFObjectRef;
                Assert.IsNotNull(first);

                Assert.AreEqual(firstPageRef.Number, first.Number);
                Assert.AreEqual(firstPageRef.Generation, first.Generation);

                var pgIndex = pageCount - 1;
                
                //last page of the first section
                var penultimate = array[pgIndex] as PDFObjectRef; 
                Assert.IsNotNull(penultimate);
                
                Assert.AreEqual(lastFirstPageRef.Number, penultimate.Number);
                Assert.AreEqual(lastFirstPageRef.Generation, penultimate.Generation);

                pgIndex = pageCount; //first template page
                var templateRef = array[pgIndex] as PDFObjectRef;
                Assert.IsNotNull(templateRef);
                
                Assert.AreEqual(firstTemplatePageRef.Number, templateRef.Number);
                Assert.AreEqual(firstTemplatePageRef.Generation, templateRef.Generation);

                
                pgIndex = pageCount + 1;
                //first page of the second pdf section
                var fstLastRef = array[pgIndex] as PDFObjectRef;
                Assert.IsNotNull(fstLastRef);
                
                Assert.AreEqual(first2ndSectionPageRef.Number, fstLastRef.Number);
                Assert.AreEqual(first2ndSectionPageRef.Generation, fstLastRef.Generation);
                
                
                //last page of the second pdf section
                var lastLastRef = array[array.Count-2] as PDFObjectRef;
                Assert.IsNotNull(lastLastRef);
                
                Assert.AreEqual(last2ndSectionPageRef.Number, lastLastRef.Number);
                Assert.AreEqual(last2ndSectionPageRef.Generation, lastLastRef.Generation);
                
                //final template page
                templateRef = array[array.Count - 1] as PDFObjectRef;
                Assert.IsNotNull(templateRef);
                
                Assert.AreEqual(lastTemplatePageRef.Number, templateRef.Number);
                Assert.AreEqual(lastTemplatePageRef.Generation, templateRef.Generation);
                
                var innerRef = doc.Frameset.DependantReferences[0] as FrameTemplateFileReference;
                Assert.IsNotNull(innerRef);
                
                //Check the remote image is loaded.
                var innerdoc = innerRef.ParsedDocument;
                Assert.AreEqual(2, innerdoc.SharedResources.Count);
                var img = innerdoc.SharedResources[0] as PDFImageXObject;
                Assert.IsNotNull(img);
                Assert.AreNotEqual(Size.Empty, img.GetImageSize());
                Assert.AreEqual(198, img.GetImageSize().Width.PointsValue);
                Assert.AreEqual(171, img.GetImageSize().Height.PointsValue);
                
                //Check the bound text
                var div = innerdoc.FindAComponentById("div1") as Div;
                Assert.IsNotNull(div);
                Assert.AreEqual(1, div.Contents.Count);
                var lit = div.Contents[0] as TextLiteral;
                Assert.IsNotNull(lit);
                Assert.AreEqual("Document title from the outer frameset.", lit.Text);
            }

        }
    }
    
    
    [TestMethod]
    public void Frameset_15_SmallSubset()
    {
        const string ExpressionsPDFPath =
            "https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/master/docs/images/Crib%20Sheet%20-%20Expressions.pdf";
        const string TemplatePath = 
            "https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/modifications/Scryber.UnitTest/Content/HTML/HelloWorld2Page.xhtml";
        
        const int startIndex = 20;
        const int pageCount = 5;
        const int lastSetStart = 137;
        const int lastSetCount = 4;

        PDFObjectRef firstPageRef = new PDFObjectRef(1353, 0);
        PDFObjectRef lastFirstPageRef = new PDFObjectRef(1357, 0);

        PDFObjectRef firstTemplatePageRef = new PDFObjectRef(1358, 0);
        
        PDFObjectRef first2ndSectionPageRef = new PDFObjectRef(1359, 0);
        PDFObjectRef last2ndSectionPageRef = new PDFObjectRef(1362, 0);
        
        PDFObjectRef lastTemplatePageRef = new PDFObjectRef(1363, 0);
        
        var src = "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                  "<head>" +
                  "<title>Parse Single Frame With First Pages</title>" +
                  "</head>" +
                  "<frameset>" +
                  "<frame src='" + ExpressionsPDFPath + "' data-page-start='"+ startIndex + "' data-page-count='" + pageCount + "' />" + //default page start should be zero
                  "<frame src='" + TemplatePath + "' data-page-count='1' />" + //First page from the remote source
                  "<frame src='" + ExpressionsPDFPath + "' data-page-start='" + lastSetStart + "' data-page-count='" + lastSetCount + "' />" + //default page start should be zero
                  "<frame src='" + TemplatePath + "' data-page-start='1' />" + //Last page from the remote source
                  "</frameset>" +
                  "</html>";
        
        using (var stream = new StringReader(src))
        {
            var doc = Document.ParseDocument(stream, ParseSourceType.DynamicContent) as HTMLDocument;

            Assert.IsNotNull(doc);
            
            Assert.IsNull(doc.Body);
            Assert.IsNotNull(doc.Frameset);

            Assert.IsNotNull(doc.Frameset.Frames);
            Assert.AreEqual(4, doc.Frameset.Frames.Count);

            var frame = doc.Frameset.Frames[0];
            Assert.IsNotNull(frame);
            Assert.AreEqual(ExpressionsPDFPath, frame.RemoteSource);
            Assert.AreEqual(startIndex, frame.PageStartIndex);
            Assert.AreEqual(pageCount, frame.PageInsertCount);

            var path = frame.MapPath(frame.RemoteSource);
            Assert.AreEqual(ExpressionsPDFPath, path);
            
            frame = doc.Frameset.Frames[1];
            Assert.IsNotNull(frame);
            Assert.IsNotNull(frame.RemoteSource);
            Assert.AreEqual(TemplatePath, frame.RemoteSource);
            Assert.AreEqual(0, frame.PageStartIndex);
            Assert.AreEqual(1, frame.PageInsertCount);

            frame = doc.Frameset.Frames[2];
            Assert.IsNotNull(frame);
            Assert.AreEqual(ExpressionsPDFPath, frame.RemoteSource);
            Assert.AreEqual(lastSetStart, frame.PageStartIndex);
            Assert.AreEqual(lastSetCount, frame.PageInsertCount);
            
            frame = doc.Frameset.Frames[3];
            Assert.IsNotNull(frame);
            Assert.IsNotNull(frame.RemoteSource);
            Assert.AreEqual(TemplatePath, frame.RemoteSource);
            Assert.AreEqual(1, frame.PageStartIndex);
            Assert.AreEqual(int.MaxValue, frame.PageInsertCount);
            

            using (var sr = DocStreams.GetOutputStream("Frameset_15_SmallSet.pdf"))
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

                Assert.AreEqual(pageCount + 2 + lastSetCount, array.Count); //Add two for the inserted template

                var first = array[0] as PDFObjectRef;
                Assert.IsNotNull(first);

                Assert.AreEqual(firstPageRef.Number, first.Number);
                Assert.AreEqual(firstPageRef.Generation, first.Generation);

                var pgIndex = pageCount - 1;
                
                //last page of the first section
                var penultimate = array[pgIndex] as PDFObjectRef; 
                Assert.IsNotNull(penultimate);
                
                Assert.AreEqual(lastFirstPageRef.Number, penultimate.Number);
                Assert.AreEqual(lastFirstPageRef.Generation, penultimate.Generation);

                pgIndex = pageCount; //first template page
                var templateRef = array[pgIndex] as PDFObjectRef;
                Assert.IsNotNull(templateRef);
                
                Assert.AreEqual(firstTemplatePageRef.Number, templateRef.Number);
                Assert.AreEqual(firstTemplatePageRef.Generation, templateRef.Generation);

                
                pgIndex = pageCount + 1;
                //first page of the second pdf section
                var fstLastRef = array[pgIndex] as PDFObjectRef;
                Assert.IsNotNull(fstLastRef);
                
                Assert.AreEqual(first2ndSectionPageRef.Number, fstLastRef.Number);
                Assert.AreEqual(first2ndSectionPageRef.Generation, fstLastRef.Generation);
                
                
                //last page of the second pdf section
                var lastLastRef = array[array.Count-2] as PDFObjectRef;
                Assert.IsNotNull(lastLastRef);
                
                Assert.AreEqual(last2ndSectionPageRef.Number, lastLastRef.Number);
                Assert.AreEqual(last2ndSectionPageRef.Generation, lastLastRef.Generation);
                
                //final template page
                templateRef = array[array.Count - 1] as PDFObjectRef;
                Assert.IsNotNull(templateRef);
                
                Assert.AreEqual(lastTemplatePageRef.Number, templateRef.Number);
                Assert.AreEqual(lastTemplatePageRef.Generation, templateRef.Generation);
                
                var innerRef = doc.Frameset.DependantReferences[0] as FrameTemplateFileReference;
                Assert.IsNotNull(innerRef);
                
                //Check the remote image is loaded.
                var innerdoc = innerRef.ParsedDocument;
                Assert.AreEqual(2, innerdoc.SharedResources.Count);
                var img = innerdoc.SharedResources[0] as PDFImageXObject;
                Assert.IsNotNull(img);
                Assert.AreNotEqual(Size.Empty, img.GetImageSize());
                Assert.AreEqual(198, img.GetImageSize().Width.PointsValue);
                Assert.AreEqual(171, img.GetImageSize().Height.PointsValue);
                
                //Check the bound text
                var div = innerdoc.FindAComponentById("div1") as Div;
                Assert.IsNotNull(div);
                Assert.AreEqual(1, div.Contents.Count);
                var lit = div.Contents[0] as TextLiteral;
                Assert.IsNotNull(lit);
                Assert.AreEqual("Document title from the outer frameset.", lit.Text);
                
                //Check the references - 2 only
                
                var refs = doc.Frameset.RootReference;
                Assert.IsNotNull(refs);
                Assert.AreEqual(refs.FileType, FrameFileType.DirectPDF);
                Assert.AreEqual(ExpressionsPDFPath, refs.FullPath);
            
                Assert.AreEqual(1, doc.Frameset.DependantReferences.Count);
                refs = doc.Frameset.DependantReferences[0];
                Assert.IsNotNull(refs);
                Assert.AreEqual(refs.FileType, FrameFileType.ReferencedTemplate);
                Assert.AreEqual(TemplatePath, refs.FullPath);
            }

        }
    }
    
    //
    // 3 documents merged
    // 


    [TestMethod]
    public void Frameset_20_ParseThreeInlineTemplates()
    {
        const string FirstTemplate ="<html id='inner1'>\n" +
                                    "<head>\n    " +
                                    "<title>{{concat(title, \"- Document 1\")}}</title>\n" +
                                    "</head>\n" +
                                    "<body id='inner1Body'>\n" +
                                    "    <div id='div1' title='{{concat(title, \"- Document 1\")}}' style='padding:10px;'>{@:title}</div>\n" +
                                    "    <a href='#div1' id='div2' style=\"padding:10pt\" >&lt;&lsquo;Inside the first template&rsquo;&gt;</a>\n" +
                                    "    <img id='img1' src=\"https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/master/Scryber.UnitTest/Content/HTML/Images/group.png\" style=\"width:100pt; padding:10px\" />\n" +
                                    "</body>\n" +
                                    "</html>";
        
        const string SecondTemplate = "<html id='inner2'>\n" +
                                      "<head>\n    " +
                                      "<title>{{concat(title, \"- Document 2\")}}</title>\n" +
                                      "</head>\n" +
                                      "<body id='inner2Body'>\n" +
                                      "    <div id='div1' title='{{concat(title, \"- Document 2\")}}' style='padding:10px;'>{@:title}</div>\n" +
                                      "    <a href='#div1' id='div2' style=\"padding:10pt\" >&lt;&lsquo;Inside the second template&rsquo;&gt;</a>\n" +
                                      "    <img id='img1' src=\"https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/master/Scryber.UnitTest/Content/HTML/Images/group.png\" style=\"width:100pt; padding:10px\" />\n" +
                                      "</body>\n" +
                                      "</html>";
        
        const string ThirdTemplate = "<html id='inner3'>\n" +
                                      "<head>\n    " +
                                      "<title>{{concat(title, \"- Document 3\")}}</title>\n" +
                                      "</head>\n" +
                                      "<body id='inner3Body'>\n" +
                                      "    <div id='div1' title='{{concat(title, \"- Document 3\")}}' style='padding:10px;'>{@:title}</div>\n" +
                                      "    <a href='#div1' id='div2' style=\"padding:10pt\" >&lt;&lsquo;Inside the third template&rsquo;&gt;</a>\n" +
                                      "    <img id='img1' src=\"https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/master/Scryber.UnitTest/Content/HTML/Images/group.png\" style=\"width:100pt; padding:10px\" />\n" +
                                      "</body>\n" +
                                      "</html>";
        
        const int startIndex = 0;
        const int pageCount = 3;
        
        var src = "<html xmlns='http://www.w3.org/1999/xhtml' title='Outer'>" +
                  "<head>" +
                  "<title>Parse 3 inline templates</title>" +
                  "</head>" +
                  "<frameset title='Frameset'>" +
                  "<frame title='First'>" + FirstTemplate + "</frame>" + 
                  "<frame title='Second'>" + SecondTemplate + "</frame>" + 
                  "<frame title='Third'>" + ThirdTemplate + "</frame>" + 
                  "</frameset>" +
                  "</html>";

        using (var stream = new StringReader(src))
        {
            var doc = Document.ParseDocument(stream, ParseSourceType.DynamicContent) as HTMLDocument;

            Assert.IsNotNull(doc);

            Assert.IsNull(doc.Body);
            Assert.IsNotNull(doc.Frameset);

            Assert.IsNotNull(doc.Frameset.Frames);
            Assert.AreEqual(3, doc.Frameset.Frames.Count);

            var frame = doc.Frameset.Frames[0];
            Assert.IsNotNull(frame);
            Assert.IsNull(frame.RemoteSource);
            Assert.IsNotNull(frame.InnerHtml);
            Assert.AreEqual(0, frame.PageStartIndex);
            Assert.AreEqual(int.MaxValue, frame.PageInsertCount);
            

            frame = doc.Frameset.Frames[1];
            Assert.IsNotNull(frame);
            Assert.IsNull(frame.RemoteSource);
            Assert.IsNotNull(frame.InnerHtml);
            Assert.AreEqual(0, frame.PageStartIndex);
            Assert.AreEqual(int.MaxValue, frame.PageInsertCount);

            frame = doc.Frameset.Frames[2];
            Assert.IsNotNull(frame);
            Assert.IsNull(frame.RemoteSource);
            Assert.IsNotNull(frame.InnerHtml);
            Assert.AreEqual(0, frame.PageStartIndex);
            Assert.AreEqual(int.MaxValue, frame.PageInsertCount);
            



            using (var sr = DocStreams.GetOutputStream("Frameset_20_ThreeInlineTemplates.pdf"))
            {
                doc.Params["title"] = "Document title from the outer frameset.";
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.SaveAsPDF(sr);

                sr.Position = 0;

                var file = PDFFile.Load(sr, doc.TraceLog);
                
                Assert.IsNotNull(file);
                Assert.IsNotNull(file.PageTree);

                var tree = file.AssertGetContent(file.PageTree) as PDFDictionary;
                Assert.IsNotNull(tree);

                var array = tree["Kids"] as PDFArray;
                Assert.IsNotNull(array);

                Assert.AreEqual(pageCount, array.Count);
            }
        }
    }

   
    [TestMethod]
    public void Frameset_21_ParseMultipleWithMixedOutputOverflowWarning()
    {
        const string ExpressionsPDFPath =
            "https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/master/docs/images/Crib%20Sheet%20-%20Expressions.pdf";
        const string TemplatePath = 
            "https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/modifications/Scryber.UnitTest/Content/HTML/HelloWorld2Page.xhtml";
        
        const int startIndex = 0;
        const int pageCount = 21;
        const int lastSetStart = 140;
        const int lastSetCount = 11;

        PDFObjectRef firstPageRef = new PDFObjectRef(1353, 0);
        PDFObjectRef lastFirstPageRef = new PDFObjectRef(1373, 0);

        PDFObjectRef firstTemplatePageRef = new PDFObjectRef(1374, 0);
        
        PDFObjectRef first2ndSectionPageRef = new PDFObjectRef(1375, 0);
        PDFObjectRef last2ndSectionPageRef = new PDFObjectRef(1385, 0);
        
        PDFObjectRef lastTemplatePageRef = new PDFObjectRef(1386, 0);
        
        var src = "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                  "<head>" +
                  "<title>Parse Single Frame With First Pages</title>" +
                  "</head>" +
                  "<frameset>" +
                  "<frame src='" + ExpressionsPDFPath + "' data-page-count='" + pageCount + "' />" + //default page start should be zero
                  "<frame src='" + TemplatePath + "' data-page-count='1' />" + //First page from the remote source
                  "<frame src='" + ExpressionsPDFPath + "' data-page-start='" + lastSetStart + "' data-page-count='" + lastSetCount + "' />" + //MORE pages than available
                  "<frame src='" + TemplatePath + "' data-page-start='10' />" + //starts AFTER the end of the document
                  "</frameset>" +
                  "</html>";
        
        
        using (var stream = new StringReader(src))
        {
            var doc = Document.ParseDocument(stream, ParseSourceType.DynamicContent) as HTMLDocument;

            Assert.IsNotNull(doc);

            Assert.IsNull(doc.Body);
            Assert.IsNotNull(doc.Frameset);

            Assert.IsNotNull(doc.Frameset.Frames);
            Assert.AreEqual(4, doc.Frameset.Frames.Count);

            var frame = doc.Frameset.Frames[0];
            Assert.IsNotNull(frame);
            Assert.AreEqual(ExpressionsPDFPath, frame.RemoteSource);
            Assert.IsNull(frame.InnerHtml);
            Assert.AreEqual(0, frame.PageStartIndex);
            Assert.AreEqual(pageCount, frame.PageInsertCount);
            

            frame = doc.Frameset.Frames[1];
            Assert.IsNotNull(frame);
            Assert.AreEqual(TemplatePath, frame.RemoteSource);
            Assert.IsNull(frame.InnerHtml);
            Assert.AreEqual(0, frame.PageStartIndex);
            Assert.AreEqual(1, frame.PageInsertCount);

            frame = doc.Frameset.Frames[2];
            Assert.IsNotNull(frame);
            Assert.AreEqual(ExpressionsPDFPath, frame.RemoteSource);
            Assert.IsNull(frame.InnerHtml);
            Assert.AreEqual(140, frame.PageStartIndex);
            Assert.AreEqual(11, frame.PageInsertCount);
            
            frame = doc.Frameset.Frames[3];
            Assert.IsNotNull(frame);
            Assert.AreEqual(TemplatePath, frame.RemoteSource);
            Assert.IsNull(frame.InnerHtml);
            Assert.AreEqual(10, frame.PageStartIndex);
            Assert.AreEqual(int.MaxValue, frame.PageInsertCount);
            



            using (var sr = DocStreams.GetOutputStream("Frameset_21_ParseMultipleWithMixedOutputOverflowWarning.pdf"))
            {
                doc.Params["title"] = "Document title from the outer frameset.";
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.AppendTraceLog = false;
                doc.SaveAsPDF(sr);

                sr.Position = 0;

                var file = PDFFile.Load(sr, doc.TraceLog);
                
                Assert.IsNotNull(file);
                Assert.IsNotNull(file.PageTree);

                var tree = file.AssertGetContent(file.PageTree) as PDFDictionary;
                Assert.IsNotNull(tree);

                var array = tree["Kids"] as PDFArray;
                Assert.IsNotNull(array);

                Assert.AreEqual(pageCount + 1 + 1, array.Count); //Add two for the first template and 1 for the last available page in the expressions document.
            }
        }
    }
    
    
    [TestMethod]
    public void Frameset_22_ParseMultipleWithMixedOutputOverflowError()
    {
        const string ExpressionsPDFPath =
            "https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/master/docs/images/Crib%20Sheet%20-%20Expressions.pdf";
        const string TemplatePath = 
            "https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/modifications/Scryber.UnitTest/Content/HTML/HelloWorld2Page.xhtml";
        
        const int startIndex = 0;
        const int pageCount = 21;
        const int lastSetStart = 140;
        const int lastSetCount = 11;

        PDFObjectRef firstPageRef = new PDFObjectRef(1353, 0);
        PDFObjectRef lastFirstPageRef = new PDFObjectRef(1373, 0);

        PDFObjectRef firstTemplatePageRef = new PDFObjectRef(1374, 0);
        
        PDFObjectRef first2ndSectionPageRef = new PDFObjectRef(1375, 0);
        PDFObjectRef last2ndSectionPageRef = new PDFObjectRef(1385, 0);
        
        PDFObjectRef lastTemplatePageRef = new PDFObjectRef(1386, 0);
        
        var src = "<?scryber parser-mode=strict ?>" +
                  "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                  "<head>" +
                  "<title>Parse Single Frame With First Pages</title>" +
                  "</head>" +
                  "<frameset>" +
                  "<frame src='" + ExpressionsPDFPath + "' data-page-count='" + pageCount + "' />" + //default page start should be zero
                  "<frame src='" + TemplatePath + "' data-page-count='1' />" + //First page from the remote source
                  "<frame src='" + ExpressionsPDFPath + "' data-page-start='" + lastSetStart + "' data-page-count='" + lastSetCount + "' />" + //MORE pages than available
                  "<frame src='" + TemplatePath + "' data-page-start='10' />" + //starts AFTER the end of the document
                  "</frameset>" +
                  "</html>";
        
        
        using (var stream = new StringReader(src))
        {
            var doc = Document.ParseDocument(stream, ParseSourceType.DynamicContent) as HTMLDocument;
            doc.ConformanceMode = ParserConformanceMode.Strict;
            Assert.IsNotNull(doc);

            Assert.IsNull(doc.Body);
            Assert.IsNotNull(doc.Frameset);

            Assert.IsNotNull(doc.Frameset.Frames);
            Assert.AreEqual(4, doc.Frameset.Frames.Count);

            
            var frame = doc.Frameset.Frames[0];
            Assert.IsNotNull(frame);
            Assert.AreEqual(ExpressionsPDFPath, frame.RemoteSource);
            Assert.IsNull(frame.InnerHtml);
            Assert.AreEqual(0, frame.PageStartIndex);
            Assert.AreEqual(pageCount, frame.PageInsertCount);
            

            frame = doc.Frameset.Frames[1];
            Assert.IsNotNull(frame);
            Assert.AreEqual(TemplatePath, frame.RemoteSource);
            Assert.IsNull(frame.InnerHtml);
            Assert.AreEqual(0, frame.PageStartIndex);
            Assert.AreEqual(1, frame.PageInsertCount);

            frame = doc.Frameset.Frames[2];
            Assert.IsNotNull(frame);
            Assert.AreEqual(ExpressionsPDFPath, frame.RemoteSource);
            Assert.IsNull(frame.InnerHtml);
            Assert.AreEqual(140, frame.PageStartIndex);
            Assert.AreEqual(11, frame.PageInsertCount);
            
            frame = doc.Frameset.Frames[3];
            Assert.IsNotNull(frame);
            Assert.AreEqual(TemplatePath, frame.RemoteSource);
            Assert.IsNull(frame.InnerHtml);
            Assert.AreEqual(10, frame.PageStartIndex);
            Assert.AreEqual(int.MaxValue, frame.PageInsertCount);




            using (var sr = DocStreams.GetOutputStream("Frameset_22_ParseMultipleWithMixedOutputOverflowError.pdf"))
            {
                doc.Params["title"] = "Document title from the outer frameset.";
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.AppendTraceLog = false;

                bool caught = false;

                try
                {
                    //strict mode SHOULD throw a layout exception.
                    doc.SaveAsPDF(sr);
                }
                catch (PDFLayoutException)
                {
                    caught = true;
                }

            }
        }
    }

    [TestMethod]
    public async Task Frameset_30_AsyncParseRemotePDF()
    {
        const string ExpressionsPDFPath =
            "https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/master/docs/images/Crib%20Sheet%20-%20Expressions.pdf";
        const string TemplatePath = 
            "https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/modifications/Scryber.UnitTest/Content/HTML/HelloWorld2Page.xhtml";

        const string Template = "<html id='inner1'>\n" +
                                     "<head>\n    " +
                                     "<title>{{concat(title, \"- Document 1\")}}</title>\n" +
                                     "</head>\n" +
                                     "<body id='inner1Body'>\n" +
                                     "    <div id='div1' title='{{concat(title, \"- Document 1\")}}' style='padding:10px;'>{@:title}</div>\n" +
                                     "    <a href='#div1' id='div2' style=\"padding:10pt\" >&lt;&lsquo;Inside the first template&rsquo;&gt;</a>\n" +
                                     "    <img id='img1' src=\"https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/master/Scryber.UnitTest/Content/HTML/Images/group.png\" style=\"width:100pt; padding:10px\" />\n" +
                                     "</body>\n" +
                                     "</html>";

        const int startIndex = 0;
        const int pageCount = 3;

        var src = "<html xmlns='http://www.w3.org/1999/xhtml' title='Outer'>" +
                  "<head>" +
                  "<title>Parse 3 inline templates</title>" +
                  "</head>" +
                  "<frameset title='Frameset'>" +
                  "<frame src='" + ExpressionsPDFPath + "' data-page-count='" + pageCount + "' title='First'></frame>" +
                  // "<frame src='" + TemplatePath + "' title='Second'></frame>" +
                  // "<frame title='Third'>" + Template + "</frame>" +
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
            Assert.IsNotNull(frame.RemoteSource);
            Assert.IsNull(frame.InnerHtml);
            Assert.AreEqual(0, frame.PageStartIndex);
            Assert.AreEqual(pageCount, frame.PageInsertCount);


            // frame = doc.Frameset.Frames[1];
            // Assert.IsNotNull(frame);
            // Assert.IsNotNull(frame.RemoteSource);
            // Assert.IsNull(frame.InnerHtml);
            // Assert.AreEqual(0, frame.PageStartIndex);
            // Assert.AreEqual(int.MaxValue, frame.PageInsertCount);

            // frame = doc.Frameset.Frames[1];
            // Assert.IsNotNull(frame);
            // Assert.IsNull(frame.RemoteSource);
            // Assert.IsNotNull(frame.InnerHtml);
            // Assert.AreEqual(0, frame.PageStartIndex);
            // Assert.AreEqual(int.MaxValue, frame.PageInsertCount);




            using (var sr = DocStreams.GetOutputStream("Frameset_30_AsyncRemotePDF.pdf"))
            {
                doc.Params["title"] = "Document title from the outer frameset.";
                doc.RenderOptions.Compression = OutputCompressionType.None;
                
                // run asynchronously
                await doc.SaveAsPDFAsync(sr);

                sr.Position = 0;

                var file = PDFFile.Load(sr, doc.TraceLog);

                Assert.IsNotNull(file);
                Assert.IsNotNull(file.PageTree);

                var tree = file.AssertGetContent(file.PageTree) as PDFDictionary;
                Assert.IsNotNull(tree);

                var array = tree["Kids"] as PDFArray;
                Assert.IsNotNull(array);

                Assert.AreEqual(pageCount, array.Count);
            }

        }
    }
    
     [TestMethod]
    public async Task Frameset_31_AsyncParseRemotePDFAndRemoteTemplate()
    {
        const string ExpressionsPDFPath =
            "https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/master/docs/images/Crib%20Sheet%20-%20Expressions.pdf";
        const string TemplatePath = 
            "https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/modifications/Scryber.UnitTest/Content/HTML/HelloWorld2Page.xhtml";

        const string Template = "<html id='inner1'>\n" +
                                     "<head>\n    " +
                                     "<title>{{concat(title, \"- Document 1\")}}</title>\n" +
                                     "</head>\n" +
                                     "<body id='inner1Body'>\n" +
                                     "    <div id='div1' title='{{concat(title, \"- Document 1\")}}' style='padding:10px;'>{@:title}</div>\n" +
                                     "    <a href='#div1' id='div2' style=\"padding:10pt\" >&lt;&lsquo;Inside the first template&rsquo;&gt;</a>\n" +
                                     "    <img id='img1' src=\"https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/master/Scryber.UnitTest/Content/HTML/Images/group.png\" style=\"width:100pt; padding:10px\" />\n" +
                                     "</body>\n" +
                                     "</html>";

        const int startIndex = 0;
        const int pageCount = 4;

        var src = "<html xmlns='http://www.w3.org/1999/xhtml' title='Outer'>" +
                  "<head>" +
                  "<title>Parse 2 inline templates</title>" +
                  "</head>" +
                  "<frameset title='Frameset'>" +
                  "<frame src='" + ExpressionsPDFPath + "' data-page-count='" + pageCount + "' title='First'></frame>" + 
                  "<frame src='" + TemplatePath + "' title='Second'></frame>" +
                  // "<frame title='Third'>" + Template + "</frame>" +
                  "</frameset>" +
                  "</html>";

        using (var stream = new StringReader(src))
        {
            var doc = Document.ParseDocument(stream, ParseSourceType.DynamicContent) as HTMLDocument;

            Assert.IsNotNull(doc);

            Assert.IsNull(doc.Body);
            Assert.IsNotNull(doc.Frameset);

            Assert.IsNotNull(doc.Frameset.Frames);
            Assert.AreEqual(2, doc.Frameset.Frames.Count);

            var frame = doc.Frameset.Frames[0];
            Assert.IsNotNull(frame);
            Assert.IsNotNull(frame.RemoteSource);
            Assert.IsNull(frame.InnerHtml);
            Assert.AreEqual(0, frame.PageStartIndex);
            Assert.AreEqual(pageCount, frame.PageInsertCount);


            frame = doc.Frameset.Frames[1];
            Assert.IsNotNull(frame);
            Assert.IsNotNull(frame.RemoteSource);
            Assert.IsNull(frame.InnerHtml);
            Assert.AreEqual(0, frame.PageStartIndex);
            Assert.AreEqual(int.MaxValue, frame.PageInsertCount);

            // frame = doc.Frameset.Frames[1];
            // Assert.IsNotNull(frame);
            // Assert.IsNull(frame.RemoteSource);
            // Assert.IsNotNull(frame.InnerHtml);
            // Assert.AreEqual(0, frame.PageStartIndex);
            // Assert.AreEqual(int.MaxValue, frame.PageInsertCount);




            using (var sr = DocStreams.GetOutputStream("Frameset_31_AsyncRemotePDFAndTemplate.pdf"))
            {
                doc.Params["title"] = "Document title from the outer frameset.";
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.AppendTraceLog = false;
                
                // run asynchronously
                await doc.SaveAsPDFAsync(sr);

                sr.Position = 0;

                var file = PDFFile.Load(sr, doc.TraceLog);

                Assert.IsNotNull(file);
                Assert.IsNotNull(file.PageTree);

                var tree = file.AssertGetContent(file.PageTree) as PDFDictionary;
                Assert.IsNotNull(tree);

                var array = tree["Kids"] as PDFArray;
                Assert.IsNotNull(array);

                Assert.AreEqual(pageCount + 2, array.Count); //Add the extra 2 pages from the original template.
            }

        }
    }
    
    
     [TestMethod]
    public async Task Frameset_32_AsyncParseRemotePDFAndMultipleRemoteTemplates()
    {
        const string ExpressionsPDFPath =
            "https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/master/docs/images/Crib%20Sheet%20-%20Expressions.pdf";
        const string TemplatePath = 
            "https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/modifications/Scryber.UnitTest/Content/HTML/HelloWorld2Page.xhtml";

        const string Template = "<html id='inner1'>\n" +
                                     "<head>\n    " +
                                     "<title>{{concat(title, \"- Document 2\")}}</title>\n" +
                                     "</head>\n" +
                                     "<body id='inner1Body'>\n" +
                                     "    <div id='div1' title='{{concat(title, \"- Document 2\")}}' style='padding:10px;'>{@:title}</div>\n" +
                                     "    <a href='#div1' id='div2' style=\"padding:10pt\" >&lt;&lsquo;Inside the first template&rsquo;&gt;</a>\n" +
                                     "    <img id='img1' src=\"https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/master/Scryber.UnitTest/Content/HTML/Images/group.png\" style=\"width:100pt; padding:10px\" />\n" +
                                     "</body>\n" +
                                     "</html>";

        const int startIndex = 0;
        const int pageCount = 5;
        const int extraFrameCount = 2;

        var src = "<html xmlns='http://www.w3.org/1999/xhtml' title='Outer'>" +
                  "<head>" +
                  "<title>Parse 3 inline templates</title>" +
                  "</head>" +
                  "<frameset title='Frameset'>" +
                  "<frame src='" + ExpressionsPDFPath + "' data-page-count='" + pageCount + "' title='First'></frame>" + 
                  "<frame src='" + TemplatePath + "' title='Second'></frame>" + 
                  // "<frame title='Third'>" + Template + "</frame>" +
                  "</frameset>" +
                  "</html>";

        using (var stream = new StringReader(src))
        {
            var doc = Document.ParseDocument(stream, ParseSourceType.DynamicContent) as HTMLDocument;
            doc.AppendTraceLog = false;
            //doc.TraceLog.SetRecordLevel(TraceRecordLevel.Verbose);
            doc.ConformanceMode = ParserConformanceMode.Lax;
            
            Assert.IsNotNull(doc);

            Assert.IsNull(doc.Body);
            Assert.IsNotNull(doc.Frameset);

            Assert.IsNotNull(doc.Frameset.Frames);
            Assert.AreEqual(2, doc.Frameset.Frames.Count);

            for (var i = 0; i < extraFrameCount; i++)
            {
                var newFrame = new HTMLFrame();
                newFrame.RemoteSource = TemplatePath + "?vers=" + i.ToString();
                doc.Frameset.Frames.Add(newFrame);
            }
            

            var frame = doc.Frameset.Frames[0];
            Assert.IsNotNull(frame);
            Assert.IsNotNull(frame.RemoteSource);
            Assert.IsNull(frame.InnerHtml);
            Assert.AreEqual(0, frame.PageStartIndex);
            Assert.AreEqual(pageCount, frame.PageInsertCount);


            frame = doc.Frameset.Frames[1];
            Assert.IsNotNull(frame);
            Assert.IsNotNull(frame.RemoteSource);
            Assert.IsNull(frame.InnerHtml);
            Assert.AreEqual(0, frame.PageStartIndex);
            Assert.AreEqual(int.MaxValue, frame.PageInsertCount);

            // frame = doc.Frameset.Frames[1];
            // Assert.IsNotNull(frame);
            // Assert.IsNull(frame.RemoteSource);
            // Assert.IsNotNull(frame.InnerHtml);
            // Assert.AreEqual(0, frame.PageStartIndex);
            // Assert.AreEqual(int.MaxValue, frame.PageInsertCount);




            using (var sr = DocStreams.GetOutputStream("Frameset_32_AsyncRemotePDFAndMultipleTemplates.pdf"))
            {
                doc.Params["title"] = "Document title from the frameset.";
                doc.RenderOptions.Compression = OutputCompressionType.None;
                
                // run asynchronously
                await doc.SaveAsPDFAsync(sr);

                sr.Position = 0;

                var file = PDFFile.Load(sr, doc.TraceLog);

                Assert.IsNotNull(file);
                Assert.IsNotNull(file.PageTree);

                var tree = file.AssertGetContent(file.PageTree) as PDFDictionary;
                Assert.IsNotNull(tree);

                var array = tree["Kids"] as PDFArray;
                Assert.IsNotNull(array);

                
                var totalPages = pageCount + 2 + (extraFrameCount * 2);//Add the extra 2 pages and the original template and the extra frames counts.
                Assert.AreEqual(totalPages, array.Count); 
            }

        }
    }
    
    
    
    /// <summary>
    /// Checks that the same document added twice is repeated and does not cause an issue.
    /// </summary>
     [TestMethod]
    public async Task Frameset_33_AsyncParseRemotePDFAndDuplicateRemoteTemplates()
    {
        const string ExpressionsPDFPath =
            "https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/master/docs/images/Crib%20Sheet%20-%20Expressions.pdf";
        const string TemplatePath = 
            "https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/modifications/Scryber.UnitTest/Content/HTML/HelloWorld2Page.xhtml";

        const string Template = "<html id='inner1'>\n" +
                                     "<head>\n    " +
                                     "<title>{{concat(title, \"- Document 2\")}}</title>\n" +
                                     "</head>\n" +
                                     "<body id='inner1Body'>\n" +
                                     "    <div id='div1' title='{{concat(title, \"- Document 2\")}}' style='padding:10px;'>{@:title}</div>\n" +
                                     "    <a href='#div1' id='div2' style=\"padding:10pt\" >&lt;&lsquo;Inside the first template&rsquo;&gt;</a>\n" +
                                     "    <img id='img1' src=\"https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/master/Scryber.UnitTest/Content/HTML/Images/group.png\" style=\"width:100pt; padding:10px\" />\n" +
                                     "</body>\n" +
                                     "</html>";

        const int startIndex = 0;
        const int pageCount = 5;
        const int extraFrameCount = 2;

        var src = "<html xmlns='http://www.w3.org/1999/xhtml' title='Outer'>" +
                  "<head>" +
                  "<title>Parse 3 inline templates</title>" +
                  "</head>" +
                  "<frameset title='Frameset'>" +
                  "<frame src='" + ExpressionsPDFPath + "' data-page-count='" + pageCount + "' title='First'></frame>" + 
                  "<frame src='" + TemplatePath + "' title='Second'></frame>" + 
                  // "<frame title='Third'>" + Template + "</frame>" +
                  "</frameset>" +
                  "</html>";

        using (var stream = new StringReader(src))
        {
            var doc = Document.ParseDocument(stream, ParseSourceType.DynamicContent) as HTMLDocument;
            doc.AppendTraceLog = false;
            //doc.TraceLog.SetRecordLevel(TraceRecordLevel.Verbose);
            doc.ConformanceMode = ParserConformanceMode.Lax;
            
            Assert.IsNotNull(doc);

            Assert.IsNull(doc.Body);
            Assert.IsNotNull(doc.Frameset);

            Assert.IsNotNull(doc.Frameset.Frames);
            Assert.AreEqual(2, doc.Frameset.Frames.Count);

            for (var i = 0; i < extraFrameCount; i++)
            {
                var newFrame = new HTMLFrame();
                newFrame.RemoteSource = TemplatePath;
                doc.Frameset.Frames.Add(newFrame);
            }
            

            var frame = doc.Frameset.Frames[0];
            Assert.IsNotNull(frame);
            Assert.IsNotNull(frame.RemoteSource);
            Assert.IsNull(frame.InnerHtml);
            Assert.AreEqual(0, frame.PageStartIndex);
            Assert.AreEqual(pageCount, frame.PageInsertCount);


            frame = doc.Frameset.Frames[1];
            Assert.IsNotNull(frame);
            Assert.IsNotNull(frame.RemoteSource);
            Assert.IsNull(frame.InnerHtml);
            Assert.AreEqual(0, frame.PageStartIndex);
            Assert.AreEqual(int.MaxValue, frame.PageInsertCount);

            // frame = doc.Frameset.Frames[1];
            // Assert.IsNotNull(frame);
            // Assert.IsNull(frame.RemoteSource);
            // Assert.IsNotNull(frame.InnerHtml);
            // Assert.AreEqual(0, frame.PageStartIndex);
            // Assert.AreEqual(int.MaxValue, frame.PageInsertCount);




            using (var sr = DocStreams.GetOutputStream("Frameset_33_AsyncRemotePDFAndDuplicateTemplates.pdf"))
            {
                doc.Params["title"] = "Document title from the frameset.";
                doc.RenderOptions.Compression = OutputCompressionType.None;
                
                // run asynchronously
                await doc.SaveAsPDFAsync(sr);

                sr.Position = 0;

                var file = PDFFile.Load(sr, doc.TraceLog);

                Assert.IsNotNull(file);
                Assert.IsNotNull(file.PageTree);

                var tree = file.AssertGetContent(file.PageTree) as PDFDictionary;
                Assert.IsNotNull(tree);

                var array = tree["Kids"] as PDFArray;
                Assert.IsNotNull(array);

                
                var totalPages = pageCount + 2 + (extraFrameCount * 2);//Add the extra 2 pages and the original template and the extra frames counts.
                Assert.AreEqual(totalPages, array.Count); 
            }

        }
    }
    
    
    /// <summary>
    /// Checks inline templates within a frameset document running asynchronously
    /// </summary>
     [TestMethod]
    public async Task Frameset_34_AsyncParseRemotePDFAndInlineTemplates()
    {
        const string ExpressionsPDFPath =
            "https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/master/docs/images/Crib%20Sheet%20-%20Expressions.pdf";
        const string TemplatePath = 
            "https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/modifications/Scryber.UnitTest/Content/HTML/HelloWorld2Page.xhtml";

        const string Template = "<html id='inner1'>\n" +
                                     "<head>\n    " +
                                     "<title>{{concat(title, \"- Document 2\")}}</title>\n" +
                                     "</head>\n" +
                                     "<body id='inner1Body'>\n" +
                                     "    <div id='div1' title='{{concat(title, \"- Document 2\")}}' style='padding:10px;'>{@:title}</div>\n" +
                                     "    <a href='#div1' id='div2' style=\"padding:10pt\" >&lt;&lsquo;Inside the first template&rsquo;&gt;</a>\n" +
                                     "    <img id='img1' src=\"https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/master/Scryber.UnitTest/Content/HTML/Images/group.png\" style=\"width:100pt; padding:10px\" />\n" +
                                     "</body>\n" +
                                     "</html>";

        const int startIndex = 0;
        const int pageCount = 5;
        const int extraFrameCount = 0;

        var src = "<html xmlns='http://www.w3.org/1999/xhtml' title='Outer'>" +
                  "<head>" +
                  "<title>Parse 3 inline templates</title>" +
                  "</head>" +
                  "<frameset title='Frameset'>" +
                  "<frame src='" + ExpressionsPDFPath + "' data-page-count='" + pageCount + "' title='First'></frame>" + 
                  //"<frame src='" + TemplatePath + "' title='Second'></frame>" + 
                  "<frame title='Third'>" + Template + "</frame>" +
                  "</frameset>" +
                  "</html>";

        using (var stream = new StringReader(src))
        {
            var doc = Document.ParseDocument(stream, ParseSourceType.DynamicContent) as HTMLDocument;
            doc.AppendTraceLog = false;
            //doc.TraceLog.SetRecordLevel(TraceRecordLevel.Verbose);
            doc.ConformanceMode = ParserConformanceMode.Lax;
            
            Assert.IsNotNull(doc);

            Assert.IsNull(doc.Body);
            Assert.IsNotNull(doc.Frameset);

            Assert.IsNotNull(doc.Frameset.Frames);
            Assert.AreEqual(2, doc.Frameset.Frames.Count);

            for (var i = 0; i < extraFrameCount; i++)
            {
                var newFrame = new HTMLFrame();
                newFrame.RemoteSource = TemplatePath;
                doc.Frameset.Frames.Add(newFrame);
            }
            

            var frame = doc.Frameset.Frames[0];
            Assert.IsNotNull(frame);
            Assert.IsNotNull(frame.RemoteSource);
            Assert.IsNull(frame.InnerHtml);
            Assert.AreEqual(0, frame.PageStartIndex);
            Assert.AreEqual(pageCount, frame.PageInsertCount);


            // frame = doc.Frameset.Frames[1];
            // Assert.IsNotNull(frame);
            // Assert.IsNotNull(frame.RemoteSource);
            // Assert.IsNull(frame.InnerHtml);
            // Assert.AreEqual(0, frame.PageStartIndex);
            // Assert.AreEqual(int.MaxValue, frame.PageInsertCount);

            frame = doc.Frameset.Frames[1];
            Assert.IsNotNull(frame);
            Assert.IsNull(frame.RemoteSource);
            Assert.IsNotNull(frame.InnerHtml);
            Assert.AreEqual(0, frame.PageStartIndex);
            Assert.AreEqual(int.MaxValue, frame.PageInsertCount);




            using (var sr = DocStreams.GetOutputStream("Frameset_34_AsyncRemotePDFAndInlineTemplates.pdf"))
            {
                doc.Params["title"] = "Document title from the frameset.";
                doc.RenderOptions.Compression = OutputCompressionType.None;
                
                // run asynchronously
                await doc.SaveAsPDFAsync(sr);

                sr.Position = 0;

                var file = PDFFile.Load(sr, doc.TraceLog);

                Assert.IsNotNull(file);
                Assert.IsNotNull(file.PageTree);

                var tree = file.AssertGetContent(file.PageTree) as PDFDictionary;
                Assert.IsNotNull(tree);

                var array = tree["Kids"] as PDFArray;
                Assert.IsNotNull(array);

                
                var totalPages = pageCount + 1 + (extraFrameCount * 2);//Add the extra page and the original template and the extra frames counts.
                Assert.AreEqual(totalPages, array.Count); 
            }

        }
    }
    
    
    
    /// <summary>
    /// Checks inline templates within a frameset document running asynchronously
    /// </summary>
     [TestMethod]
    public async Task Frameset_35_AsyncParseMultipleInlineTemplatesAndRemotePDF()
    {
        const string ExpressionsPDFPath =
            "https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/master/docs/images/Crib%20Sheet%20-%20Expressions.pdf";
        const string TemplatePath = 
            "https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/modifications/Scryber.UnitTest/Content/HTML/HelloWorld2Page.xhtml";

        const string Template = "<html id='inner1'>\n" +
                                     "<head>\n    " +
                                     "<title>{{concat(title, \"- Document 2\")}}</title>\n" +
                                     "</head>\n" +
                                     "<body id='inner1Body'>\n" +
                                     "    <div id='div1' title='{{concat(title, \"- Document 2\")}}' style='padding:10px;'>{@:title}</div>\n" +
                                     "    <a href='#div1' id='div2' style=\"padding:10pt\" >&lt;&lsquo;Inside the first template&rsquo;&gt;</a>\n" +
                                     "    <img id='img1' src=\"https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/master/Scryber.UnitTest/Content/HTML/Images/group.png\" style=\"width:100pt; padding:10px\" />\n" +
                                     "</body>\n" +
                                     "</html>";
        
        const string Template2 = "<html id='inner2'>\n" +
                                "<head>\n    " +
                                "<title>{{concat(title, \"- Document 2\")}}</title>\n" +
                                "</head>\n" +
                                "<body id='inner1Body'>\n" +
                                "    <div id='div1' title='{{concat(title, \"- Document 3\")}}' style='padding:10px; page-break-after: always;'>{@:title}</div>\n" +
                                "    <a href='#div1' id='div2' style=\"padding:10pt\" >&lt;&lsquo;Inside the second template&rsquo;&gt;</a>\n" +
                                "    <img id='img1' src=\"https://raw.githubusercontent.com/richard-scryber/scryber.core/refs/heads/master/Scryber.UnitTest/Content/HTML/Images/group.png\" style=\"width:100pt; padding:10px\" />\n" +
                                "</body>\n" +
                                "</html>";

        const int startIndex = 0;
        const int pageCount = 5;
        const int extraFrameCount = 0;

        var src = "<html xmlns='http://www.w3.org/1999/xhtml' title='Outer'>" +
                  "<head>" +
                  "<title>Parse 3 inline templates</title>" +
                  "</head>" +
                  "<frameset title='Frameset'>" +
                  //"<frame src='" + TemplatePath + "' title='Second'></frame>" + 
                  "<frame title='Second'>" + Template + "</frame>" +
                  "<frame title='Third'>" + Template2 + "</frame>" +
                  "<frame src='" + ExpressionsPDFPath + "' data-page-count='" + pageCount + "' title='First'></frame>" + 
                  "</frameset>" +
                  "</html>";

        using (var stream = new StringReader(src))
        {
            var doc = Document.ParseDocument(stream, ParseSourceType.DynamicContent) as HTMLDocument;
            doc.AppendTraceLog = false;
            //doc.TraceLog.SetRecordLevel(TraceRecordLevel.Verbose);
            doc.ConformanceMode = ParserConformanceMode.Lax;
            
            Assert.IsNotNull(doc);

            Assert.IsNull(doc.Body);
            Assert.IsNotNull(doc.Frameset);

            Assert.IsNotNull(doc.Frameset.Frames);
            Assert.AreEqual(3, doc.Frameset.Frames.Count);

            for (var i = 0; i < extraFrameCount; i++)
            {
                var newFrame = new HTMLFrame();
                newFrame.RemoteSource = TemplatePath;
                doc.Frameset.Frames.Add(newFrame);
            }
            
            var frame = doc.Frameset.Frames[1];
            Assert.IsNotNull(frame);
            Assert.IsNull(frame.RemoteSource);
            Assert.IsNotNull(frame.InnerHtml);
            Assert.AreEqual(0, frame.PageStartIndex);
            Assert.AreEqual(int.MaxValue, frame.PageInsertCount);
            
            frame = doc.Frameset.Frames[1];
            Assert.IsNotNull(frame);
            Assert.IsNull(frame.RemoteSource);
            Assert.IsNotNull(frame.InnerHtml);
            Assert.AreEqual(0, frame.PageStartIndex);
            Assert.AreEqual(int.MaxValue, frame.PageInsertCount);

            frame = doc.Frameset.Frames[2];
            Assert.IsNotNull(frame);
            Assert.IsNotNull(frame.RemoteSource);
            Assert.IsNull(frame.InnerHtml);
            Assert.AreEqual(0, frame.PageStartIndex);
            Assert.AreEqual(pageCount, frame.PageInsertCount);


            // frame = doc.Frameset.Frames[1];
            // Assert.IsNotNull(frame);
            // Assert.IsNotNull(frame.RemoteSource);
            // Assert.IsNull(frame.InnerHtml);
            // Assert.AreEqual(0, frame.PageStartIndex);
            // Assert.AreEqual(int.MaxValue, frame.PageInsertCount);

            




            using (var sr = DocStreams.GetOutputStream("Frameset_35_AsyncParseMultipleInlineTemplatesAndRemotePDF.pdf"))
            {
                doc.Params["title"] = "Document title from the frameset.";
                doc.RenderOptions.Compression = OutputCompressionType.None;
                
                // run asynchronously
                await doc.SaveAsPDFAsync(sr);

                sr.Position = 0;

                var file = PDFFile.Load(sr, doc.TraceLog);

                Assert.IsNotNull(file);
                Assert.IsNotNull(file.PageTree);

                var tree = file.AssertGetContent(file.PageTree) as PDFDictionary;
                Assert.IsNotNull(tree);

                var array = tree["Kids"] as PDFArray;
                Assert.IsNotNull(array);

                
                var totalPages = pageCount + 1 + 2;//Add the first template and 2 for the second template and then the pages from the original pdf.
                Assert.AreEqual(totalPages, array.Count); 
            }

        }
    }
}
