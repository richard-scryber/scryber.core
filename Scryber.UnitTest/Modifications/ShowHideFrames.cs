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

[TestClass]
public class ShowHideFrames
{
    
    [TestMethod]
    public void Frameset_40_ParseThreeFramesOneHidden()
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
        const int lastSetCount = 0;

        PDFObjectRef firstPageRef = new PDFObjectRef(1359, 0);
        PDFObjectRef lastFirstPageRef = new PDFObjectRef(1379, 0);

        PDFObjectRef firstTemplatePageRef = new PDFObjectRef(1380, 0);
        PDFObjectRef lastTemplatePageRef = new PDFObjectRef(1381, 0);
        
        //PDFObjectRef first2ndSectionPageRef = new PDFObjectRef(1382, 0);
        //PDFObjectRef last2ndSectionPageRef = new PDFObjectRef(1392, 0);
        
        var src = "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                  "<head>" +
                  "<title>Parse Single Frame With First Pages</title>" +
                  "</head>" +
                  "<frameset>" +
                  "<frame src='" + ExpressionsPDFPath + "' data-page-count='" + pageCount + "' />" + //default page start should be zero
                  "<frame>" + //Add the content within the frame
                  TemplateContent +
                  "</frame>" +
                  "<frame hidden='hidden' src='" + ExpressionsPDFPath + "' data-page-start='" + lastSetStart + "' />" + //default page start should be zero
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
            
            Assert.IsFalse(frame.Visible); //third frame is not visible.

            path = frame.MapPath(frame.RemoteSource);
            Assert.AreEqual(ExpressionsPDFPath, path);
            

            using (var sr = DocStreams.GetOutputStream("Frameset_40_3FramesOneHidden.pdf"))
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
    public void Frameset_41_ParseThreeFramesTwoHidden()
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
        const int pageCount = 0;
        const int lastSetStart = 130;
        const int lastSetCount = 11;

        //PDFObjectRef firstPageRef = new PDFObjectRef(1359, 0);
        //PDFObjectRef lastFirstPageRef = new PDFObjectRef(1379, 0);

        //PDFObjectRef firstTemplatePageRef = new PDFObjectRef(1380, 0);
        //PDFObjectRef lastTemplatePageRef = new PDFObjectRef(1381, 0);
        
        PDFObjectRef first2ndSectionPageRef = new PDFObjectRef(1340, 0);
        PDFObjectRef last2ndSectionPageRef = new PDFObjectRef(1350, 0);
        
        var src = "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                  "<head>" +
                  "<title>Parse Single Frame With First Pages</title>" +
                  "</head>" +
                  "<frameset>" +
                  "<frame hidden='hidden' src='" + ExpressionsPDFPath + "' data-page-count='" + pageCount + "' />" + //default page start should be zero
                  "<frame hidden='hidden' >" + //Add the content within the frame
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
            
            Assert.IsFalse(frame.Visible);

            var path = frame.MapPath(frame.RemoteSource);
            Assert.AreEqual(ExpressionsPDFPath, path);
            
            frame = doc.Frameset.Frames[1];
            Assert.IsNotNull(frame);
            Assert.IsNull(frame.RemoteSource);
            Assert.IsNotNull(frame.InnerHtml);

            var innerdoc = frame.InnerHtml;
            

            Assert.AreEqual(0, frame.PageStartIndex);
            Assert.AreEqual(int.MaxValue, frame.PageInsertCount);

            Assert.IsFalse(frame.Visible);

            frame = doc.Frameset.Frames[2];
            Assert.IsNotNull(frame);
            Assert.AreEqual(ExpressionsPDFPath, frame.RemoteSource);

            Assert.AreEqual(lastSetStart, frame.PageStartIndex);
            Assert.AreEqual(int.MaxValue, frame.PageInsertCount);

            path = frame.MapPath(frame.RemoteSource);
            Assert.AreEqual(ExpressionsPDFPath, path);
            
            Assert.IsTrue(frame.Visible);
            

            using (var sr = DocStreams.GetOutputStream("Frameset_41_ParseThreeFramesTwoHidden.pdf"))
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

                Assert.AreEqual(lastSetCount, array.Count); //Add two for the inserted template

                var first = array[0] as PDFObjectRef;
                Assert.IsNotNull(first);

                
                //first page of the second pdf section
                var fstLastRef = array[0] as PDFObjectRef;
                Assert.IsNotNull(fstLastRef);
                
                Assert.AreEqual(first2ndSectionPageRef.Number, fstLastRef.Number);
                Assert.AreEqual(first2ndSectionPageRef.Generation, fstLastRef.Generation);
                
                
                //last page of the second pdf section
                var lastLastRef = array[array.Count-1] as PDFObjectRef;
                Assert.IsNotNull(lastLastRef);
                
                Assert.AreEqual(last2ndSectionPageRef.Number, lastLastRef.Number);
                Assert.AreEqual(last2ndSectionPageRef.Generation, lastLastRef.Generation);
                
                
                //Nothing should be loaded for the inline frame.
                Assert.AreEqual(0, innerdoc.SharedResources.Count);
                
            }

        }
    }

    [TestMethod]
    public void Frameset_42_ThreeFramesAllHidden()
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
            
            //explicitly hidden #0
            frame.Hidden = "hidden";
            
            frame = doc.Frameset.Frames[1];
            Assert.IsNotNull(frame);
            Assert.IsNull(frame.RemoteSource);
            Assert.IsNotNull(frame.InnerHtml);

            var innerdoc = frame.InnerHtml;
            

            Assert.AreEqual(0, frame.PageStartIndex);
            Assert.AreEqual(int.MaxValue, frame.PageInsertCount);

            //explicitly hidden #1
            frame.Hidden = "hidden";

            frame = doc.Frameset.Frames[2];
            Assert.IsNotNull(frame);
            Assert.AreEqual(ExpressionsPDFPath, frame.RemoteSource);

            Assert.AreEqual(lastSetStart, frame.PageStartIndex);
            Assert.AreEqual(int.MaxValue, frame.PageInsertCount);

            path = frame.MapPath(frame.RemoteSource);
            Assert.AreEqual(ExpressionsPDFPath, path);
            
            //explicitly hidden #2
            frame.Hidden = "hidden";
            

            using (var sr = DocStreams.GetOutputStream("Frameset_42_ThreeFramesAllHidden.pdf"))
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

                Assert.AreEqual(0, array.Count); //No pages

            }

        }
    }
    
    [TestMethod]
    public void Frameset_43_ThreeFramesAllHiddenOneUnHidden()
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
        const int pageCount = 0;
        const int lastSetStart = 130;
        const int lastSetCount = 0;

        //PDFObjectRef firstPageRef = new PDFObjectRef(1359, 0);
        //PDFObjectRef lastFirstPageRef = new PDFObjectRef(1379, 0);

        PDFObjectRef firstTemplatePageRef = new PDFObjectRef(22, 0);
        PDFObjectRef lastTemplatePageRef = new PDFObjectRef(23, 0);
        
        // PDFObjectRef first2ndSectionPageRef = new PDFObjectRef(1382, 0);
        // PDFObjectRef last2ndSectionPageRef = new PDFObjectRef(1392, 0);
        
        var src = "<html xmlns='http://www.w3.org/1999/xhtml'>" +
                  "<head>" +
                  "<title>Parse Single Frame With First Pages</title>" +
                  "</head>" +
                  "<frameset>" +
                  "<frame hidden='hidden' src='" + ExpressionsPDFPath + "' data-page-count='" + pageCount + "' />" + //default page start should be zero
                  "<frame hidden='hidden'>" + //Add the content within the frame
                  TemplateContent +
                  "</frame>" +
                  "<frame hidden='hidden' src='" + ExpressionsPDFPath + "' data-page-start='" + lastSetStart + "' />" + //default page start should be zero
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
            
            Assert.IsFalse(frame.Visible); //Initially Hidden
            
            frame = doc.Frameset.Frames[1];
            Assert.IsNotNull(frame);
            Assert.IsNull(frame.RemoteSource);
            Assert.IsNotNull(frame.InnerHtml);

            var innerdoc = frame.InnerHtml;
            

            Assert.AreEqual(0, frame.PageStartIndex);
            Assert.AreEqual(int.MaxValue, frame.PageInsertCount);

            Assert.IsFalse(frame.Visible); //Initially Hidden

            frame = doc.Frameset.Frames[2];
            Assert.IsNotNull(frame);
            Assert.AreEqual(ExpressionsPDFPath, frame.RemoteSource);

            Assert.AreEqual(lastSetStart, frame.PageStartIndex);
            Assert.AreEqual(int.MaxValue, frame.PageInsertCount);

            path = frame.MapPath(frame.RemoteSource);
            Assert.AreEqual(ExpressionsPDFPath, path);
            
            Assert.IsFalse(frame.Visible); //Initially Hidden


            doc.Frameset.Frames[1].Visible = true; //UnHide the middle template
            

            using (var sr = DocStreams.GetOutputStream("Frameset_43_ThreeFramesAllHiddenOneUnHidden.pdf"))
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

                

                var pgIndex = pageCount; //first template page
                var templateRef = array[pgIndex] as PDFObjectRef;
                Assert.IsNotNull(templateRef);
                
                Assert.AreEqual(firstTemplatePageRef.Number, templateRef.Number);
                Assert.AreEqual(firstTemplatePageRef.Generation, templateRef.Generation);

                pgIndex++; //second template page
                templateRef = array[pgIndex] as PDFObjectRef;
                Assert.IsNotNull(templateRef);
                
                Assert.AreEqual(lastTemplatePageRef.Number, templateRef.Number);
                Assert.AreEqual(lastTemplatePageRef.Generation, templateRef.Generation);

                
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
}