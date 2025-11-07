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

using System.Xml.Schema;
using System.Xml.Linq;
using System.IO;
using Scryber.Generation;
using Scryber.Svg.Components;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using Scryber.PDF.Resources;
using Scryber.Expressive.Functions;
using Scryber.Expressive.Expressions;
using Scryber.Expressive.Exceptions;
using Scryber.Expressive;
using Scryber.PDF.Secure;
using Scryber.Html;
using System.Collections;


namespace Scryber.Core.UnitTests.Html
{
    [TestClass()]
    public class HtmlParsing_Test
    {


        private PDFLayoutContext _layoutcontext;

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

        private void DocumentParsing_Layout(object sender, LayoutEventArgs args)
        {
            _layoutcontext = (PDFLayoutContext)args.Context;
        }

        

        
        

        [TestMethod()]
        public void SimpleDocumentParsing()
        {
            var src = @"<!DOCTYPE html>
                        <html xmlns='http://www.w3.org/1999/xhtml' >
                            <head>
                                <title>Html document title</title>
                            </head>

                            <body class='strong' style='margin:20px;' >
                                <p id='myPara' style='border: solid 1px blue; padding: 5px;' >This is a paragraph of content</p>
                            </body>

                        </html>";

            using (var sr = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent);
                Assert.IsInstanceOfType(doc, typeof(HTMLDocument));

                using (var stream = DocStreams.GetOutputStream("HtmlSimple.pdf"))
                {
                    doc.LayoutComplete += DocumentParsing_Layout;
                    doc.SaveAsPDF(stream);
                }

                
                var body = _layoutcontext.DocumentLayout.AllPages[0].ContentBlock;
                var p = body.Columns[0].Contents[0] as PDFLayoutBlock;
                Assert.AreEqual("Html document title", doc.Info.Title, "Title is not correct");
                

            }
        }

        [TestMethod()]
        public void SimpleDocumentParsing2()
        {
            var src = @"<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <meta charset='utf-8' />
    <title>First Test</title>

    <style >
        body {
            background-color:#CCC;
            padding:20pt;
            font-size:medium;
            margin:0pt;
            font-family: 'Segoe UI', sans-serif;
        }

        h1{
            font-size:30pt;
            font-weight:normal;
        }

    </style>
</head>
<body>
    <div>Above the heading
        <h1>This is my first heading</h1>
        <div>And this is the content below the heading that should flow across multiple lines within the page and flow nicely along those lines.</div>
    </div>
</body>
</html>";

            using (var sr = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent);
                doc.RenderOptions.Compression = OutputCompressionType.None;

                Assert.IsInstanceOfType(doc, typeof(HTMLDocument));

                using (var stream = DocStreams.GetOutputStream("HtmlSimple2.pdf"))
                {
                    doc.LayoutComplete += DocumentParsing_Layout;
                    doc.SaveAsPDF(stream);
                }

                var page = doc.Pages[0] as Page;
                var div = page.Contents[1] as Div;
                var h1 = div.Contents[1] as HTMLHead1;
                Assert.IsNotNull(h1, "No heading found");
                Assert.AreEqual(1, h1.Contents.Count);
                var content = h1.Contents[0];
            }
        }


        [TestMethod()]
        public void LoadHtmlFromSource()
        {
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/HtmlFromSource.html",
                this.TestContext);
            using (var doc = Document.ParseDocument(path))
            {
                var defn = new StyleDefn("h1.border");
                defn.Background.Color = (Color)"#FFA";
                defn.Border.Width = 2;
                defn.Border.Color = StandardColors.Red;
                defn.Border.LineStyle = LineType.Solid;

                doc.Styles.Add(defn);

                using (var stream = DocStreams.GetOutputStream("HtmlFromSource.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }
            }

            Assert.Inconclusive();
        }

        [TestMethod()]
        public void HelloWorld()
        {
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/HelloWorld.xhtml",
                this.TestContext);
            using (var doc = Document.ParseDocument(path))
            {
                
                using (var stream = DocStreams.GetOutputStream("HelloWorld.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }
            }

            Assert.Inconclusive();
        }


        [TestMethod()]
        public void MultipleImageReferences()
        {
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/MultipleImageReferences.html",
                this.TestContext);
            
            using (var doc = Document.ParseDocument(path))
            {

                using (var stream = DocStreams.GetOutputStream("MultipleImageReferences.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

                Assert.AreEqual(2, doc.SharedResources.Count); //Second is the font.

                //Ensure we have one image and it's group.png

                PDFImageXObject one = null;

                if (doc.SharedResources[0] is PDFImageXObject)
                    one = doc.SharedResources[0] as PDFImageXObject;
                else if (doc.SharedResources[1] is PDFImageXObject)
                    one = doc.SharedResources[1] as PDFImageXObject;
                                
                Assert.IsNotNull(one);
                var data = one.ImageData;
                Assert.IsNotNull(data);
                Assert.IsTrue(data.SourcePath.EndsWith("group.png", StringComparison.OrdinalIgnoreCase));
                
            }
        }


        [TestMethod()]
        public void RemoteCssFileLoading()
        {
            var path = "https://raw.githubusercontent.com/richard-scryber/scryber.core/master/Scryber.UnitTest/Content/HTML/CSS/Include.css";
            var src = @"<html xmlns='http://www.w3.org/1999/xhtml' >
                            <head>
                                <title>Html document title</title>
                                <link href='" + path + @"' rel='stylesheet' />
                            </head>

                            <body class='grey' style='margin:20px;' >
                                <p id='myPara' >This is a paragraph of content</p>
                            </body>

                        </html>";

            using (var sr = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent);
                Assert.IsInstanceOfType(doc, typeof(HTMLDocument));

                using (var stream = DocStreams.GetOutputStream("RemoteCssFileLoading.pdf"))
                {
                    doc.LayoutComplete += DocumentParsing_Layout;
                    doc.SaveAsPDF(stream);
                }


                var body = _layoutcontext.DocumentLayout.AllPages[0].PageBlock;
                
                Assert.AreEqual("Html document title", doc.Info.Title, "Title is not correct");

                //This has been loaded from the remote file
                Assert.AreEqual((Color)"#808080", body.FullStyle.Background.Color, "Fill colors do not match");
                

            }
        }

        [TestMethod()]
        public void RemoteCssFileLoadingAsync()
        {
            var path = "https://raw.githubusercontent.com/richard-scryber/scryber.core/master/Scryber.UnitTest/Content/HTML/CSS/Include.css";
            var src = @"<?scryber append-log='true' ?>
<html xmlns='http://www.w3.org/1999/xhtml' >
                            <head>
                                <title>Html document title</title>
                                <link href='" + path + @"' rel='stylesheet' />
                            </head>

                            <body class='grey' style='margin:20px;' >
                                <p id='myPara' >This is a paragraph of content</p>
                            </body>

                        </html>";

            using (var sr = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent);
                Assert.IsInstanceOfType(doc, typeof(HTMLDocument));

                using (var stream = DocStreams.GetOutputStream("RemoteCssFileLoadingAsync.pdf"))
                {
                    doc.LayoutComplete += DocumentParsing_Layout;
                    Task.Run(async () =>
                    {
                        await doc.SaveAsPDFAsync(stream);
                    }).GetAwaiter().GetResult();
                    
                }


                var body = _layoutcontext.DocumentLayout.AllPages[0].PageBlock;

                Assert.AreEqual("Html document title", doc.Info.Title, "Title is not correct");

                //This has been loaded from the remote file
                Assert.AreEqual((Color)"#808080", body.FullStyle.Background.Color, "Fill colors do not match");


            }
        }

        [TestMethod()]
        public void RemoteCssFileLoadingCustomHandlerAsync()
        {
            var path = "https://raw.githubusercontent.com/richard-scryber/scryber.core/master/Scryber.UnitTest/Content/HTML/CSS/Include.css";
            var src = @"<?scryber append-log='true' log-level='Verbose' ?>
<html xmlns='http://www.w3.org/1999/xhtml' >
                            <head>
                                <title>Html document title</title>
                                <link href='" + path + @"' rel='stylesheet' />
                            </head>

                            <body class='grey' style='margin:20px;' >
                                <p id='myPara' >This is a paragraph of content</p>
                            </body>

                        </html>";

            _remotefileRequestCalled = 0;

            using (var sr = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent);
                Assert.IsInstanceOfType(doc, typeof(HTMLDocument));

                using (var stream = DocStreams.GetOutputStream("RemoteCssFileLoadingAsyncCustomLoading.pdf"))
                {
                    
                    doc.RemoteFileRegistered += Doc_RemoteFileRegistered; 
                    doc.LayoutComplete += DocumentParsing_Layout;

                    Task.Run(async () =>
                    {
                        await doc.SaveAsPDFAsync(stream);

                    }).GetAwaiter().GetResult();

                }


                var body = _layoutcontext.DocumentLayout.AllPages[0].PageBlock;

                Assert.AreEqual("Html document title", doc.Info.Title, "Title is not correct");




                Assert.IsTrue(_remotefileRequestCalled == 1, "Remote request was not run");

                //This has been loaded from the remote file
                Assert.AreEqual((Color)"#808080", body.FullStyle.Background.Color, "Fill colors do not match");

            }
        }

        private int _remotefileRequestCalled = 0;

        /// <summary>
        /// This is outside of the main code to return the binary data for a remote file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Doc_RemoteFileRegistered(object sender, RemoteFileRequestEventArgs args)
        {

            _remotefileRequestCalled ++;
            //Flag as executing
            args.Request.StartRequest();
            var doc = args.Request.Owner.Document;
            doc.TraceLog.Add(TraceLevel.Message, "External Code", "Starting the request for the Remote file outside of the main code");

            Task.Run(async () =>
            {
                using var client = new System.Net.Http.HttpClient();

                var data = await client.GetByteArrayAsync(args.Request.FilePath);

                System.Threading.Thread.Sleep(2000);

                var ms = new MemoryStream(data);
                doc.TraceLog.Add(TraceLevel.Message, "External Code", "Request for the Remote file was completed outside of the main code (including wait)");

                args.Request.Callback(args.Request.Owner, args.Request, ms);
                args.Request.CompleteRequest(ms, true);


            });
            
            
        }

        [TestMethod()]
        public void RemoteCssFileLoadingCustomImageHandlerAsync()
        {
            var path = "https://raw.githubusercontent.com/richard-scryber/scryber.core/master/Scryber.UnitTest/Content/HTML/CSS/Include.css";
            var imgsrc = "https://raw.githubusercontent.com/richard-scryber/scryber.core/master/docs/images/ScyberLogo2_alpha_small.png"; //parameter for uniqueness

            var src = @"<?scryber append-log='true' log-level='Verbose' ?>
<html xmlns='http://www.w3.org/1999/xhtml' >
                            <head>
                                <title>Html document title</title>
                                <link href='" + path + @"' rel='stylesheet' />
                            </head>

                            <body class='grey' style='margin:20px;' >
                                <p id='myPara' >This is a paragraph of content with an image below</p>
                                <img src='" + imgsrc + @"' />
                            </body>

                        </html>";

            _remoteImagefileRequestCalled = 0;

            using (var sr = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent);
                Assert.IsInstanceOfType(doc, typeof(HTMLDocument));

                using (var stream = DocStreams.GetOutputStream("RemoteCssFileLoadingAsyncCustomImageLoading.pdf"))
                {
                    doc.CacheProvider = new Scryber.Caching.PDFNoCachingProvider();
                    doc.RemoteFileRegistered += Doc_RemoteFileRegisteredForImages;
                    doc.LayoutComplete += DocumentParsing_Layout;

                    Task.Run(async () =>
                    {
                        await doc.SaveAsPDFAsync(stream);

                    }).GetAwaiter().GetResult();

                }


                var body = _layoutcontext.DocumentLayout.AllPages[0].PageBlock;

                Assert.AreEqual("Html document title", doc.Info.Title, "Title is not correct");




                Assert.IsTrue(_remoteImagefileRequestCalled == 1, "Remote request was not run just once it bwas run " + _remoteImagefileRequestCalled + " times");

                //This has been loaded from the remote file
                Assert.AreEqual((Color)"#808080", body.FullStyle.Background.Color, "Fill colors do not match");
                Assert.AreEqual(2, doc.SharedResources.Count, "The image was not loaded");
                var found = false;

                for(var i = 0; i < doc.SharedResources.Count; i++)
                {
                    var rsrc = doc.SharedResources[i];
                    if (rsrc.ResourceKey == imgsrc)
                    {
                        found = true;
                        var img = rsrc as PDFImageXObject;
                        Assert.IsNotNull(img);
                        //Just check the image size for validation it was loaded
                        var raster = img.ImageData as ImageRasterData;
                        Assert.IsNotNull(raster);
                        
                        Assert.AreEqual(640, raster.PixelWidth, "Unexpected image width for the png");
                        Assert.AreEqual(643, raster.PixelHeight, "Unexpected image height for the png");
                    }
                }

                Assert.IsTrue(found, "The remote image was not found");
            }
        }

        int _remoteImagefileRequestCalled = 0;

        /// <summary>
        /// This is outside of the main code to return the binary data for a remote file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Doc_RemoteFileRegisteredForImages(object sender, RemoteFileRequestEventArgs args)
        {
            if (args.Request.FilePath.EndsWith(".png"))
            {
                _remoteImagefileRequestCalled++;
                //Flag as executing
                args.Request.StartRequest();
                var doc = args.Request.Owner.Document;
                doc.TraceLog.Add(TraceLevel.Message, "External Code", "Starting the request for the Remote image file outside of the main code");

                Task.Run(async () =>
                {
                    using var client = new System.Net.Http.HttpClient();

                    var data = await client.GetByteArrayAsync(args.Request.FilePath);

                    //System.Threading.Thread.Sleep(2000);

                    var ms = new MemoryStream(data);
                    doc.TraceLog.Add(TraceLevel.Message, "External Code", "Request for the Remote file was completed outside of the main code (including wait)");

                    args.Request.Callback(args.Request.Owner, args.Request, ms);
                    args.Request.CompleteRequest(ms, true);


                });

            }
            else
            {
                //Do nothing here and it should be handled internally
            }
        }


        [TestMethod()]
        public void BodyAsASection()
        {
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/bodyheadfoot.html",
                this.TestContext);
            
            using (var doc = Document.ParseDocument(path))
            {
                using (var stream = DocStreams.GetOutputStream("bodyheadfoot.pdf"))
                {
                    doc.LayoutComplete += DocumentParsing_Layout;
                    doc.SaveAsPDF(stream);
                    
                }

                var pg = doc.Pages[0] as Section;
                Assert.IsNotNull(pg.Header);
                Assert.IsNotNull(pg.Footer);

                var body = _layoutcontext.DocumentLayout.AllPages[0];
                Assert.IsNotNull(body.HeaderBlock);
                Assert.IsNotNull(body.FooterBlock);
            }

        }

        [TestMethod()]
        public void PartialParagraphWithContent()
        {

            var str = @"<p xmlns='http://www.w3.org/1999/xhtml' class='{@:model.bodyClass}' style=""{@:model.bodyStyle}"">
        This is the content on the next page with number <num value=""{@:model.number}"" data-format=""£#0.00"" /> and name <label data-content=""{@:model.items[1].Name}"">
    {@:model.items[0].Name}
</label>
    </p>";

            var frag = Document.Parse(new StringReader(str), ParseSourceType.DynamicContent) as Component;
            var model = new
            {
                headerText = "Bound Header",
                footerText = "Bound Footer",
                content = "This is the bound content text",
                bodyStyle = "background-color:red; color:#FFF; padding: 20pt",
                bodyClass = "top",
                number = (Decimal)10.1,
                items = new[]
                {
                    new { Name = "First" },
                    new { Name = "Second"},
                    new { Name = "Third" }
                }
            };

            Document doc = new Document();
            Page pg = new Page();
            pg.Contents.Add(frag);
            doc.Params["model"] = model;


        }

        [TestMethod()]
        public void BodyWithBinding()
        {
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/bodyWithBinding.html",
                this.TestContext);
            
            var model = new
            {
                headerText = "Bound Header",
                footerText = "Bound Footer",
                content = "This is the bound content text",
                bodyStyle = "background-color:red; color:#FFF; padding: 20pt",
                bodyClass = "top",
                number = (Decimal)10.1,
                items = new []
                {
                    new { Name = "First" },
                    new { Name = "Second"},
                    new { Name = "Third" }
                }
            };

            using (var doc = Document.ParseDocument(path))
            {
                doc.ConformanceMode = ParserConformanceMode.Strict;
                using (var stream = DocStreams.GetOutputStream("bodyWithBinding.pdf"))
                {
                    doc.Params["model"] = model;
                    doc.AutoBind = true;
                    doc.LayoutComplete += DocumentParsing_Layout;
                    doc.SaveAsPDF(stream);

                }

                var pg = doc.Pages[0] as Section;
                Assert.IsNotNull(pg.Header);
                Assert.IsNotNull(pg.Footer);
            }

            var body = _layoutcontext.DocumentLayout.AllPages[0];
            Assert.IsNotNull(body.HeaderBlock);
            Assert.IsNotNull(body.FooterBlock);

            // Header content check

            var pgHead = body.HeaderBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            var header = pgHead.Columns[0].Contents[0] as PDFLayoutBlock;
            var pBlock = header.Columns[0].Contents[0] as PDFLayoutBlock;

            var pLine = pBlock.Columns[0].Contents[0] as PDFLayoutLine;
            var pRun = pLine.Runs[1] as PDFTextRunCharacter; // 0 is begin text

            Assert.AreEqual(model.headerText, pRun.Characters);

            // Footer content check

            var pgFoot = body.FooterBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            var footer = pgFoot.Columns[0].Contents[0] as PDFLayoutBlock;
            pBlock = footer.Columns[0].Contents[0] as PDFLayoutBlock;

            pLine = pBlock.Columns[0].Contents[0] as PDFLayoutLine;
            pRun = pLine.Runs[1] as PDFTextRunCharacter; // 0 is begin text

            Assert.AreEqual(model.footerText, pRun.Characters);

            //First page check
            pBlock = body.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            pLine = pBlock.Columns[0].Contents[0] as PDFLayoutLine;
            pRun = pLine.Runs[1] as PDFTextRunCharacter; // First is static text

            Assert.AreEqual(pRun.Characters, "Bound value of ");

            pRun = pLine.Runs[4] as PDFTextRunCharacter;

            Assert.AreEqual(pRun.Characters, model.content);

            var bgColor = pBlock.FullStyle.Background.Color;
            Assert.AreEqual("rgb(255,0,0)", bgColor.ToString()); //Red Background

            var color = pBlock.FullStyle.Fill.Color;
            Assert.AreEqual("rgb(255,255,255)", color.ToString());

            //Second page check

            
            body = _layoutcontext.DocumentLayout.AllPages[1];
            Assert.IsNotNull(body);

            pBlock = body.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            pLine = pBlock.Columns[0].Contents[0] as PDFLayoutLine;
            pRun = pLine.Runs[1] as PDFTextRunCharacter; // First is static text

            Assert.AreEqual("This is the content on the next page with number ", pRun.Characters);

            //TextEnd at 2
            //TextBegin at 3

            var pnum = pLine.Runs[4] as PDFTextRunCharacter;
            Assert.AreEqual((10.10).ToString("£#0.00"), pnum.Characters);

            //TextEnd at 5
            //TextBegin at 6

            var intern = pLine.Runs[7] as PDFTextRunCharacter;
            Assert.AreEqual(" and name ", intern.Characters);

            //TextEnd at 8
            //Inline span begin at 9
            //TextBegin at 10

            var lbl = pLine.Runs[11] as PDFTextRunCharacter;
            Assert.AreEqual("Second", lbl.Characters);

            bgColor = pBlock.FullStyle.Background.Color;
            Assert.AreEqual("rgb(255,0,0)", bgColor.ToString()); //Red Background

            color = pBlock.FullStyle.Fill.Color;
            Assert.AreEqual("rgb(255,255,255)", color.ToString());

            // Performance check
            using (var doc = Document.ParseDocument(path))
            {
                
                using (var stream = DocStreams.GetOutputStream("bodyWithBinding_performance.pdf"))
                {
                    doc.Params["model"] = model;
                    doc.AutoBind = true;
                    doc.SaveAsPDF(stream);

                }
            }

        }

        [TestMethod()]
        public void BodyWithJsonBinding()
        {
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/bodyWithBinding.html",
                this.TestContext);
            
            var modeljson = "{\r\n" +
                "\"headerText\" : \"Bound Header\"," +
                "\"footerText\" : \"Bound Footer\"," +
                "\"content\" : \"This is the bound content text\"," +
                "\"bodyStyle\" : \"background-color:red; color:#FFF; padding: 20pt\"," +
                "\"bodyClass\" : \"top\"," +
                "\"number\" : 10.1," +
                "\"items\" : [" + 
                    "{ \"Name\" : \"First\" }," +
                    "{ \"Name\" : \"Second\"}," +
                    "{ \"Name\" : \"Third\" }" +
                    "]" +
                "}";

            dynamic model = Newtonsoft.Json.JsonConvert.DeserializeObject(modeljson);

            using (var doc = Document.ParseDocument(path))
            {
                using (var stream = DocStreams.GetOutputStream("bodyWithJsonBinding.pdf"))
                {
                    doc.Params["model"] = model;
                    doc.ConformanceMode = ParserConformanceMode.Strict;
                    doc.LayoutComplete += DocumentParsing_Layout;
                    doc.SaveAsPDF(stream);

                }

                var pg = doc.Pages[0] as Section;
                Assert.IsNotNull(pg.Header);
                Assert.IsNotNull(pg.Footer);
            }

            var body = _layoutcontext.DocumentLayout.AllPages[0];
            Assert.IsNotNull(body.HeaderBlock);
            Assert.IsNotNull(body.FooterBlock);

            // Header content check

            var pgHead = body.HeaderBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            var header = pgHead.Columns[0].Contents[0] as PDFLayoutBlock;
            var pBlock = header.Columns[0].Contents[0] as PDFLayoutBlock;

            var pLine = pBlock.Columns[0].Contents[0] as PDFLayoutLine;
            var pRun = pLine.Runs[1] as PDFTextRunCharacter; // 0 is begin text

            Assert.AreEqual(pRun.Characters, model.headerText.ToString());

            // Footer content check

            var pgFoot = body.FooterBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            var footer = pgFoot.Columns[0].Contents[0] as PDFLayoutBlock;
            pBlock = footer.Columns[0].Contents[0] as PDFLayoutBlock;

            pLine = pBlock.Columns[0].Contents[0] as PDFLayoutLine;
            pRun = pLine.Runs[1] as PDFTextRunCharacter; // 0 is begin text

            Assert.AreEqual(pRun.Characters, model.footerText.ToString());

            //First page check
            pBlock = body.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            pLine = pBlock.Columns[0].Contents[0] as PDFLayoutLine;
            pRun = pLine.Runs[1] as PDFTextRunCharacter; // First is static text

            Assert.AreEqual(pRun.Characters, "Bound value of ");

            pRun = pLine.Runs[4] as PDFTextRunCharacter;

            Assert.AreEqual(pRun.Characters, model.content.ToString());

            var bgColor = pBlock.FullStyle.Background.Color;
            Assert.AreEqual("rgb(255,0,0)", bgColor.ToString()); //Red Background

            var color = pBlock.FullStyle.Fill.Color;
            Assert.AreEqual("rgb(255,255,255)", color.ToString());

            //Second page check


            body = _layoutcontext.DocumentLayout.AllPages[1];
            Assert.IsNotNull(body);

            pBlock = body.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            pLine = pBlock.Columns[0].Contents[0] as PDFLayoutLine;
            pRun = pLine.Runs[1] as PDFTextRunCharacter; // First is static text

            Assert.AreEqual("This is the content on the next page with number ", pRun.Characters);

            bgColor = pBlock.FullStyle.Background.Color;
            Assert.AreEqual("rgb(255,0,0)", bgColor.ToString()); //Red Background

            color = pBlock.FullStyle.Fill.Color;
            Assert.AreEqual("rgb(255,255,255)", color.ToString());

        }

        [TestMethod()]
        public void BodyWithExpressionBinding()
        {
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/bodyWithExpressionBinding.html",
                this.TestContext);
            var model = new
            {
                headerText = "Bound Header",
                footerText = "Bound Footer",
                content = "This is the bound content text",
                bodyStyle = "background-color:red; color:#FFF; padding: 20pt",
                bodyClass = "top",
                number = (Decimal)10.1,
                items = new[]
                {
                    new { Name = "First" },
                    new { Name = "Second"},
                    new { Name = "Third" }
                }
            };

            using (var doc = Document.ParseDocument(path))
            {
                //doc.ConformanceMode = ParserConformanceMode.Strict;
                using (var stream = DocStreams.GetOutputStream("bodyWithExpressionBinding.pdf"))
                {
                    doc.Params["model"] = model;
                    doc.AutoBind = true;
                    doc.LayoutComplete += DocumentParsing_Layout;
                    doc.SaveAsPDF(stream);

                }

                var pg = doc.Pages[0] as Section;
                Assert.IsNotNull(pg.Header);
                Assert.IsNotNull(pg.Footer);
            }

            var body = _layoutcontext.DocumentLayout.AllPages[0];
            Assert.IsNotNull(body.HeaderBlock);
            Assert.IsNotNull(body.FooterBlock);

            // Header content check

            var pgHead = body.HeaderBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            var header = pgHead.Columns[0].Contents[0] as PDFLayoutBlock;
            var pBlock = header.Columns[0].Contents[0] as PDFLayoutBlock;

            var pLine = pBlock.Columns[0].Contents[0] as PDFLayoutLine;
            var pRun = pLine.Runs[1] as PDFTextRunCharacter; // 0 is begin text

            Assert.AreEqual(pRun.Characters, model.headerText);

            // Footer content check

            var pgFoot = body.FooterBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            var footer = pgFoot.Columns[0].Contents[0] as PDFLayoutBlock;
            pBlock = footer.Columns[0].Contents[0] as PDFLayoutBlock;

            pLine = pBlock.Columns[0].Contents[0] as PDFLayoutLine;
            pRun = pLine.Runs[1] as PDFTextRunCharacter; // 0 is begin text

            Assert.AreEqual(pRun.Characters, model.footerText);

            //First page check
            pBlock = body.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            pLine = pBlock.Columns[0].Contents[0] as PDFLayoutLine;
            pRun = pLine.Runs[1] as PDFTextRunCharacter; // First is static text

            Assert.AreEqual(pRun.Characters, "Bound value of ");

            pRun = pLine.Runs[4] as PDFTextRunCharacter;

            Assert.AreEqual(pRun.Characters, model.content + " & \"an error in quotes\"");

            var bgColor = pBlock.FullStyle.Background.Color;
            Assert.AreEqual("rgb(255,0,0)", bgColor.ToString()); //Red Background

            var color = pBlock.FullStyle.Fill.Color;
            Assert.AreEqual("rgb(255,255,255)", color.ToString());

            //Second page check


            body = _layoutcontext.DocumentLayout.AllPages[1];
            Assert.IsNotNull(body);

            pBlock = body.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            pLine = pBlock.Columns[0].Contents[0] as PDFLayoutLine;
            pRun = pLine.Runs[1] as PDFTextRunCharacter; // First is static text

            Assert.AreEqual("This is the content on the next page with number ", pRun.Characters);

            //TextEnd at 2
            //TextBegin at 3

            var pnum = pLine.Runs[4] as PDFTextRunCharacter;
            Assert.AreEqual((10.10).ToString("£#0.00"), pnum.Characters);

            //TextEnd at 5
            //TextBegin at 6

            var intern = pLine.Runs[7] as PDFTextRunCharacter;
            Assert.AreEqual(" and name ", intern.Characters);

            //TextEnd at 8
            //Inline span begin at 9
            //TextBegin at 10

            var lbl = pLine.Runs[11] as PDFTextRunCharacter;
            Assert.AreEqual("Second", lbl.Characters);

            bgColor = pBlock.FullStyle.Background.Color;
            Assert.AreEqual("rgb(255,0,0)", bgColor.ToString()); //Red Background

            color = pBlock.FullStyle.Fill.Color;
            Assert.AreEqual("rgb(255,255,255)", color.ToString());
        }



        [TestMethod()]
        public void LocalAndRemoteImages()
        {
            var imagepath = "https://raw.githubusercontent.com/richard-scryber/scryber.core/master/docs/images/ScyberLogo2_alpha_small.png";
            var client = new System.Net.Http.HttpClient();
            var data = client.GetByteArrayAsync(imagepath).Result;
            
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/LocalAndRemoteImages.html",
                this.TestContext);
            
            using (var doc = Document.ParseDocument(path))
            {
                using (var stream = DocStreams.GetOutputStream("LocalAndRemoteImages.pdf"))
                {
                    doc.LayoutComplete += DocumentParsing_Layout;
                    doc.SaveAsPDF(stream);

                }

                Assert.AreEqual(3, doc.SharedResources.Count, "Not all images were loaded");

                var img1Index = 1;
                var img2Index = 2;

                var font = doc.SharedResources[0] as PDFFontResource;
                if (null == font)
                {
                    //switch around as images can be loaded first if using a proxy
                    font = doc.SharedResources[2] as PDFFontResource;
                    img1Index = 0;
                    img2Index = 1;
                }

                Assert.IsNotNull(font, "No font loaded");

                var one = doc.SharedResources[img1Index] as PDFImageXObject; //Should be the local image;
                Assert.IsNotNull(one, "Not an image for the second resouce");
                Assert.IsTrue(one.Registered);
                Assert.IsTrue(one.Source.EndsWith("/HTML/images/ScyberLogo2_alpha_small.png"), "Source one '" + one.Source + "' was not correct for the image");

                var two = doc.SharedResources[img2Index] as PDFImageXObject;
                Assert.IsNotNull(two, "Not an image for the third resource");
                Assert.IsTrue(two.Source.EndsWith("/docs/images/ScyberLogo2_alpha_small.png"), "Source two '" + two.Source + "' was not correct for the second image");


            }

        }

        [TestMethod()]
        public void LocalAndRemoteImagesAsync()
        {
            var imagepath = "https://raw.githubusercontent.com/richard-scryber/scryber.core/master/docs/images/ScyberLogo2_alpha_small.png";
            var client = new System.Net.Http.HttpClient();
            var data = client.GetByteArrayAsync(imagepath).Result;


            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/LocalAndRemoteImages.html",
                this.TestContext);


            using (var doc = Document.ParseDocument(path))
            {
                using (var stream = DocStreams.GetOutputStream("LocalAndRemoteImagesAsync.pdf"))
                {
                    doc.LayoutComplete += DocumentParsing_Layout;
                    Task.Run(async () =>
                    {
                        await doc.SaveAsPDFAsync(stream);
                    }).GetAwaiter().GetResult();

                }

                Assert.AreEqual(3, doc.SharedResources.Count, "Not all images were loaded");

                var img1Index = 1;
                var img2Index = 2;

                var font = doc.SharedResources[0] as PDFFontResource;
                if (null == font)
                {
                    //switch around as images can be loaded first if using a proxy
                    font = doc.SharedResources[2] as PDFFontResource;
                    img1Index = 0;
                    img2Index = 1;
                }

                Assert.IsNotNull(font, "No font loaded");

                var one = doc.SharedResources[img1Index] as PDFImageXObject; //Should be the local image;
                Assert.IsNotNull(one, "Not an image for the second resouce");
                Assert.IsTrue(one.Registered);
                Assert.IsTrue(one.Source.EndsWith("/HTML/images/ScyberLogo2_alpha_small.png"), "Source was not correct for the image");

                var two = doc.SharedResources[img2Index] as PDFImageXObject;
                Assert.IsNotNull(two,"Not an image for the third resource");
                Assert.IsTrue(two.Source.EndsWith("docs/images/ScyberLogo2_alpha_small.png"), "Source was not correct for the second image");




            }

        }

        [TestMethod()]
        public void BodyTemplating()
        {
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/bodytemplating.html",
                this.TestContext);
            
            dynamic[] all = new dynamic[100];
            int total = 0;

            for(var i = 0; i < 100; i++)
            {
                var val = i + 1;
                all[i] = new { Name = "Name " + val.ToString(), Cost = "£" + val + ".00", Style = "" };
                total += val;
            }

            var model = new
            {
                Items = all,
                Total = new
                {
                    Name = "Total",
                    Cost = "£" + total + ".00"
                }
            };

            using (var doc = Document.ParseDocument(path))
            {
                using (var stream = DocStreams.GetOutputStream("bodytemplating.pdf"))
                {
                    doc.Params["model"] = model;
                    doc.LayoutComplete += DocumentParsing_Layout;
                    doc.SaveAsPDF(stream);

                }
                var pg = doc.Pages[0] as Section;
                Assert.IsNotNull(pg.Header);
                Assert.IsNotNull(pg.Footer);

                var body = _layoutcontext.DocumentLayout.AllPages[0];
                Assert.IsNotNull(body.HeaderBlock);
                Assert.IsNotNull(body.FooterBlock);

                var table = doc.FindAComponentById("grid") as TableGrid;
                Assert.IsNotNull(table);
                Assert.AreEqual(2 + model.Items.Length, table.Rows.Count);
            }

        }

        [TestMethod()]
        public void BodyTemplatingWithJson()
        {
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/bodytemplating.html",
                this.TestContext);
            StringBuilder content = new StringBuilder();

            
            int total = 0;
            int count = 100;
            for (var i = 0; i < count; i++)
            {
                var val = i + 1;
                if (i > 0)
                    content.Append(",");

                content.Append("{ \"Name\": \"Name " + val.ToString() + "\", \"Cost\": \"£" + val.ToString() + ".00\", \"Style\": \"\" }\r\n");

                total += val;
            }

            var modelJson = "{\r\n" +
                "\"Items\" : [" + content.ToString() +
                "],\r\n" +
                "\"Total\" : {\r\n" +
                    "\"Name\" : \"Total\",\r\n" +
                    "\"Cost\" : \"£" + total + ".00\"\r\n" +
                    "}\r\n" +
                "}";

            var model = Newtonsoft.Json.JsonConvert.DeserializeObject(modelJson);

            using (var doc = Document.ParseDocument(path))
            {
                using (var stream = DocStreams.GetOutputStream("bodytemplatingWithJson.pdf"))
                {
                    doc.Params["model"] = model;
                    doc.LayoutComplete += DocumentParsing_Layout;
                    doc.SaveAsPDF(stream);

                }
                var pg = doc.Pages[0] as Section;
                Assert.IsNotNull(pg.Header);
                Assert.IsNotNull(pg.Footer);

                var body = _layoutcontext.DocumentLayout.AllPages[0];
                Assert.IsNotNull(body.HeaderBlock);
                Assert.IsNotNull(body.FooterBlock);

                var table = doc.FindAComponentById("grid") as TableGrid;
                Assert.IsNotNull(table);
                Assert.AreEqual(2 + count, table.Rows.Count);
            }

        }

        [TestMethod]
        public void DisplayNoneHidden()
        {
            dynamic[] all = new dynamic[100];
            int total = 0;

            for (var i = 0; i < 100; i++)
            {
                var val = i + 1;
                //Hide every tenth one.
                var vis = (i % 10 == 0) ? "display:none" : "";

                all[i] = new { Name = "Name " + val.ToString(), Cost = "£" + val + ".00", Style = vis };
                if (i % 10 != 0)
                    total += val;
            }

            var model = new
            {
                Items = all,
                Total = new
                {
                    Name = "Total",
                    Cost = "£" + total + ".00"
                }
            };

            
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/displaynone.html",
                this.TestContext);
            
            using (var doc = Document.ParseDocument(path))
            {
                using (var stream = DocStreams.GetOutputStream("htmlDisplayNone.pdf"))
                {
                    doc.Params["model"] = model;
                    doc.LayoutComplete += DocumentParsing_Layout;
                    doc.SaveAsPDF(stream);

                }
                var layout = this._layoutcontext.DocumentLayout;
                var pDiv = layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
                Assert.AreEqual(pDiv.Columns[0].Contents.Count, 2, "There should be only 2 layout items in the set of paragraphs");

                var p1 = pDiv.Columns[0].Contents[0] as PDFLayoutBlock;
                Assert.AreEqual("pshow1", p1.Owner.ID);

                var p2 = pDiv.Columns[0].Contents[1] as PDFLayoutBlock;
                Assert.AreEqual("pshow2", p2.Owner.ID);
            }

        }

        [TestMethod()]
        public void TopAndTailed()
        {
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/topandtailed.html",
                this.TestContext);
            using (var doc = Document.ParseDocument(path))
            {
                doc.Params["title"] = "Title in code";

                using (var stream = DocStreams.GetOutputStream("topandtailed.pdf"))
                {
                    doc.LayoutComplete += DocumentParsing_Layout;
                    doc.SaveAsPDF(stream);

                }

                var pg = doc.Pages[0] as Section;
                Assert.IsNotNull(pg.Header);
                Assert.IsNotNull(pg.Footer);

                var body = _layoutcontext.DocumentLayout.AllPages[0];
                Assert.IsNotNull(body.HeaderBlock);
                Assert.IsNotNull(body.FooterBlock);
            }

        }

        [TestMethod()]
        public void TextDecoration()
        {
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/textdecoration.html",
                this.TestContext);
            
            using (var doc = Document.ParseDocument(path))
            {
                doc.Params["title"] = "Title in code";

                using (var stream = DocStreams.GetOutputStream("textdecoration.pdf"))
                {
                    doc.LayoutComplete += DocumentParsing_Layout;
                    doc.SaveAsPDF(stream);

                }

                var pg = doc.Pages[0] as Section;
                var body = _layoutcontext.DocumentLayout.AllPages[0];

                Assert.Inconclusive("There are no specific tests written for the text decoration");

            }

        }
        
        [TestMethod]
        public void CSSTransformParsing()
        {
            //rotate examples
            string toParse = "rotate(3.142)";
            
            var parsed = CSSTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            var root = parsed.Root;
            var rotate = root as TransformRotateOperation;
            Assert.IsNotNull(rotate);
            
            Assert.AreEqual(3.142, rotate.AngleRadians);
            
            toParse = "rotate(-0.5turn) ";
            parsed = CSSTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            rotate = root as TransformRotateOperation;
            Assert.IsNotNull(rotate);
            
            Assert.AreEqual(-3.142, Math.Round(rotate.AngleRadians, 3));

            toParse = "rotate(180deg)";
            parsed = CSSTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            rotate = root as TransformRotateOperation;
            Assert.IsNotNull(rotate);
            
            Assert.AreEqual(3.142, Math.Round(rotate.AngleRadians, 3));

            toParse = "scale(1)";
            parsed = CSSTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            var scale = root as TransformScaleOperation;
            Assert.IsNotNull(scale);
            
            Assert.AreEqual(1.0, Math.Round(scale.XScaleValue));
            Assert.AreEqual(1.0, Math.Round(scale.YScaleValue));
            
            toParse = "scale(0.7)";
            parsed = CSSTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            scale = root as TransformScaleOperation;
            Assert.IsNotNull(scale);
            
            Assert.AreEqual(0.7, Math.Round(scale.XScaleValue, 5));
            Assert.AreEqual(0.7, Math.Round(scale.YScaleValue, 5));
            
            toParse = "scale(1.3, 0.4)";
            parsed = CSSTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            scale = root as TransformScaleOperation;
            Assert.IsNotNull(scale);
            
            Assert.AreEqual(1.3, Math.Round(scale.XScaleValue, 5));
            Assert.AreEqual(0.4, Math.Round(scale.YScaleValue, 5));
            
            toParse = "scale(-0.5,1)";
            parsed = CSSTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            scale = root as TransformScaleOperation;
            Assert.IsNotNull(scale);
            
            Assert.AreEqual(-0.5, Math.Round(scale.XScaleValue, 5));
            Assert.AreEqual(1.0, Math.Round(scale.YScaleValue, 5));
            
            toParse = "scaleX(0.7)";
            parsed = CSSTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            scale = root as TransformScaleOperation;
            Assert.IsNotNull(scale);
            
            Assert.AreEqual(0.7, Math.Round(scale.XScaleValue, 5));
            Assert.AreEqual(1.0, Math.Round(scale.YScaleValue, 5));
            
            
            toParse = "scaleX(1.3)";
            parsed = CSSTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            scale = root as TransformScaleOperation;
            Assert.IsNotNull(scale);
            
            Assert.AreEqual(1.3, Math.Round(scale.XScaleValue, 5));
            Assert.AreEqual(1.0, Math.Round(scale.YScaleValue, 5));
            
            toParse = "scaleX(-0.5)";
            parsed = CSSTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            scale = root as TransformScaleOperation;
            Assert.IsNotNull(scale);
            
            Assert.AreEqual(-0.5, Math.Round(scale.XScaleValue, 5));
            Assert.AreEqual(1.0, Math.Round(scale.YScaleValue, 5));
            
            toParse = "skew(0)";
            parsed = CSSTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            var skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            Assert.AreEqual(0, Math.Round(skew.XAngleRadians, 5));
            Assert.AreEqual(0, Math.Round(skew.YAngleRadians, 5));
            
            toParse = "skew(15deg, 15deg)";
            parsed = CSSTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            const double Deg2Rad = Math.PI / 180.0;

            Assert.AreEqual(Math.Round(15 * Deg2Rad, 5), Math.Round(skew.XAngleRadians, 5));
            Assert.AreEqual(Math.Round(15 * Deg2Rad, 5), Math.Round(skew.YAngleRadians, 5));

            toParse = "skew(-0.06turn, 18deg)";
            parsed = CSSTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            Assert.AreEqual(Math.Round((-0.06 * 360) * Deg2Rad, 5), Math.Round(skew.XAngleRadians, 5));
            Assert.AreEqual(Math.Round(18 * Deg2Rad, 5), Math.Round(skew.YAngleRadians, 5));
            
            toParse = "skew(.312rad)";
            parsed = CSSTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            Assert.AreEqual(0.312, Math.Round(skew.XAngleRadians, 5));
            Assert.AreEqual(0.312, Math.Round(skew.YAngleRadians, 5));
            
            toParse = "skewX(0)";
            parsed = CSSTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            Assert.AreEqual(0, Math.Round(skew.XAngleRadians, 5));
            Assert.AreEqual(0, Math.Round(skew.YAngleRadians, 5));
            
            toParse = "skewX(-0.6turn)";
            parsed = CSSTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            
            Assert.AreEqual(Math.Round((-0.6 * 360) * Deg2Rad, 5), Math.Round(skew.XAngleRadians, 5));
            Assert.AreEqual(0.0, skew.YAngleRadians);
            
            toParse = "skewX(35deg)";
            parsed = CSSTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            
            Assert.AreEqual(Math.Round(35 * Deg2Rad, 5), Math.Round(skew.XAngleRadians, 5));
            Assert.AreEqual(0.0, skew.YAngleRadians);

            toParse = "skewX(.234rad)";
            parsed = CSSTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            Assert.AreEqual(0.234, Math.Round(skew.XAngleRadians, 5));
            Assert.AreEqual(0, Math.Round(skew.YAngleRadians, 5));
            
            toParse = "skewY(0)";
            parsed = CSSTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            Assert.AreEqual(0, Math.Round(skew.XAngleRadians, 5));
            Assert.AreEqual(0, Math.Round(skew.YAngleRadians, 5));
            
            toParse = "skewY(-0.6turn)";
            parsed = CSSTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            
            Assert.AreEqual(Math.Round((-0.6 * 360) * Deg2Rad, 5), Math.Round(skew.YAngleRadians, 5));
            Assert.AreEqual(0.0, skew.XAngleRadians);
            
            toParse = "skewY(35deg)";
            parsed = CSSTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            
            Assert.AreEqual(Math.Round(35 * Deg2Rad, 5), Math.Round(skew.YAngleRadians, 5));
            Assert.AreEqual(0.0, skew.XAngleRadians);

            toParse = "skewY(.234rad)";
            parsed = CSSTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            Assert.AreEqual(0.234, Math.Round(skew.YAngleRadians, 5));
            Assert.AreEqual(0, Math.Round(skew.XAngleRadians, 5));
            
            toParse = "translate(200px)";
            parsed = CSSTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            var translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);
            

            Assert.AreEqual(Unit.Px(200), translate.XOffset);
            Assert.AreEqual(Unit.Zero, translate.YOffset);

            toParse = "translate(50%)";
            parsed = CSSTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);
            

            Assert.AreEqual(Unit.Percent(50), translate.XOffset);
            Assert.AreEqual(Unit.Zero, translate.YOffset);
            
            toParse = "translate(100pt,200pt)";
            parsed = CSSTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);
            

            Assert.AreEqual(Unit.Pt(100), translate.XOffset);
            Assert.AreEqual(Unit.Pt(200), translate.YOffset);
            
            toParse = "translate(100px,50%)";
            parsed = CSSTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);
            

            Assert.AreEqual(Unit.Px(100), translate.XOffset);
            Assert.AreEqual(Unit.Percent(50), translate.YOffset);
            
            toParse = "translate(-30%, 210.5px)";
            parsed = CSSTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);
            

            Assert.AreEqual(Unit.Percent(-30), translate.XOffset);
            Assert.AreEqual(Unit.Px(210.5), translate.YOffset);
            
            toParse = "translate(30%, -50%)";
            parsed = CSSTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);
            

            Assert.AreEqual(Unit.Percent(30), translate.XOffset);
            Assert.AreEqual(Unit.Percent(-50), translate.YOffset);
            
            toParse = "translateX(0)";
            parsed = CSSTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);

            Assert.AreEqual(Unit.Pt(0), translate.XOffset);
            Assert.AreEqual(Unit.Pt(0), translate.YOffset);
            
            toParse = "translateX(42px)";
            parsed = CSSTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);

            Assert.AreEqual(Unit.Px(42.0), translate.XOffset);
            Assert.AreEqual(Unit.Pt(0), translate.YOffset);
            
            toParse = "translateX(-2.1rem)";
            parsed = CSSTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);

            Assert.AreEqual(Unit.RootEm(-2.1), translate.XOffset);
            Assert.AreEqual(Unit.Pt(0), translate.YOffset);
            
            toParse = "translateX(3ch)";
            parsed = CSSTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);

            Assert.AreEqual(Unit.Ch(3), translate.XOffset);
            Assert.AreEqual(Unit.Pt(0), translate.YOffset);
            
            toParse = "translateY(0)";
            parsed = CSSTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);

            Assert.AreEqual(Unit.Pt(0), translate.XOffset);
            Assert.AreEqual(Unit.Pt(0), translate.YOffset);
            
            toParse = "translateY(42px)";
            parsed = CSSTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);
            
            Assert.AreEqual(Unit.Pt(0), translate.XOffset);
            Assert.AreEqual(Unit.Px(42.0), translate.YOffset);
            
            toParse = "translateY(-2.1rem)";
            parsed = CSSTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);

            Assert.AreEqual(Unit.Pt(0), translate.XOffset);
            Assert.AreEqual(Unit.RootEm(-2.1), translate.YOffset);
            
            toParse = "translateY(3ch)";
            parsed = CSSTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);
            
            Assert.AreEqual(Unit.Pt(0), translate.XOffset);
            Assert.AreEqual(Unit.Ch(3), translate.YOffset);
            
            toParse = "matrix(1, 0, 0, 1, 0, 0)";
            parsed = CSSTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            var matrix = root as Scryber.Drawing.TransformMatrixOperation;
            Assert.IsNotNull(matrix);
            
            Assert.AreEqual(6, matrix.MatrixValues.Length);
            Assert.AreEqual(1.0, matrix.MatrixValues[0]);
            Assert.AreEqual(0.0, matrix.MatrixValues[1]);
            Assert.AreEqual(0.0, matrix.MatrixValues[2]);
            Assert.AreEqual(1.0, matrix.MatrixValues[3]);
            Assert.AreEqual(0.0, matrix.MatrixValues[4]);
            Assert.AreEqual(0.0, matrix.MatrixValues[5]);
            
            toParse = "matrix(0.4, 0, 0.5, 1.2, 60, 10)";
            parsed = CSSTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            matrix = root as Scryber.Drawing.TransformMatrixOperation;
            Assert.IsNotNull(matrix);
            
            Assert.AreEqual(6, matrix.MatrixValues.Length);
            Assert.AreEqual(0.4, matrix.MatrixValues[0]);
            Assert.AreEqual(0.0, matrix.MatrixValues[1]);
            Assert.AreEqual(0.5, matrix.MatrixValues[2]);
            Assert.AreEqual(1.2, matrix.MatrixValues[3]);
            Assert.AreEqual(60.0, matrix.MatrixValues[4]);
            Assert.AreEqual(10.0, matrix.MatrixValues[5]);
            
            toParse = "matrix(0.1, 1, -0.3, 1, 20, 20.2)";
            parsed = CSSTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            matrix = root as Scryber.Drawing.TransformMatrixOperation;
            Assert.IsNotNull(matrix);
            
            Assert.AreEqual(6, matrix.MatrixValues.Length);
            Assert.AreEqual(0.1, matrix.MatrixValues[0]);
            Assert.AreEqual(1.0, matrix.MatrixValues[1]);
            Assert.AreEqual(-0.3, matrix.MatrixValues[2]);
            Assert.AreEqual(1.0, matrix.MatrixValues[3]);
            Assert.AreEqual(20.0, matrix.MatrixValues[4]);
            Assert.AreEqual(20.2, matrix.MatrixValues[5]);
        }

        [TestMethod()]
        public void HtmlIFrameFragments()
        {
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/BodyFraming.html",
                this.TestContext);
            using (var doc = Document.ParseDocument(path))
            {
                var model = new
                {
                    fragmentContent = "Content for the fragment"
                };
                doc.Params["model"] = model;

                using (var stream = DocStreams.GetOutputStream("BodyFraming.pdf"))
                {
                    doc.LayoutComplete += DocumentParsing_Layout;
                    doc.SaveAsPDF(stream);

                }
                var para = doc.FindAComponentById("FrameInner") as Paragraph;
                Assert.IsNotNull(para);

                //Get the second paragraph
                para = doc.FindAComponentById("FrameDynamic") as Paragraph;
                Assert.AreEqual(2, para.Contents.Count);

                //Check that the inner text of the para matches the bound value.
                var span = para.Contents[1] as IPDFTextLiteral;
                Assert.AreEqual(model.fragmentContent, span.Text);
            }

        }

        private class CodeTemplate : ITemplate
        {
            public string Content{
                get;
                private set;
            }

            public CodeTemplate(string content)
            {
                this.Content = content;
            }

            public IEnumerable<IComponent> Instantiate(int index, IComponent owner)
            {
                using (StringReader reader = new StringReader(this.Content))
                    return new[] { Document.Parse(reader, ParseSourceType.DynamicContent) };
            }
        }


        [TestMethod()]
        public void HtmlPlaceholderFragments()
        {
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/BodyWithPlaceholder.html",
                this.TestContext);
            using (var doc = Document.ParseDocument(path))
            {
                var model = new
                {
                    HtmlContent = "<p id='p1'><span style='color: rgb(255,0,0);'>Lorem ipsum dolor sit amet, consectetur</span></p>"
                    + "<p id='p2'><br/></p>"
                    + "<p id='p3' ><span style='color: rgb(0,0 255);' ><span class='ql-cursor'></span>Vivamus sed tincidunt nisi.Vivamus elementum,</span></p>",
                };
                doc.Params["model"] = model;

                using (var stream = DocStreams.GetOutputStream("BodyWithPlaceholder.pdf"))
                {
                    doc.LayoutComplete += DocumentParsing_Layout;
                    doc.SaveAsPDF(stream);

                }


                var ph = doc.FindAComponentById("aPlaceholder") as PlaceHolder;
                Assert.IsNotNull(ph);

                Assert.AreEqual(1, ph.Contents.Count);
                Assert.IsInstanceOfType(ph.Contents[0], typeof(Scryber.Data.TemplateInstance));

                var instance = ph.Contents[0] as Scryber.Data.TemplateInstance;
                Assert.AreEqual(3, instance.Content.Count);

                var p = instance.FindAComponentById("p1");
                Assert.IsNotNull(p);
                p = instance.FindAComponentById("p2");
                Assert.IsNotNull(p);
                p = instance.FindAComponentById("p3");
                Assert.IsNotNull(p);
                
            }

        }



        [TestMethod()]
        public void BodyWithPageNumbers()
        {
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/bodyWithPageNums.html",
                this.TestContext);
            var model = new
            {
                headerText = "Bound Header",
                footerText = "Bound Footer",
                content = "This is the bound content text",
                bodyStyle = "background-color:red; color:#FFF; padding: 20pt",
                bodyClass = "top"
            };

            using (var doc = Document.ParseDocument(path))
            {
                using (var stream = DocStreams.GetOutputStream("bodyWithPageNums.pdf"))
                {
                    doc.Params["model"] = model;
                    doc.AutoBind = true;
                    doc.LayoutComplete += DocumentParsing_Layout;
                    doc.SaveAsPDF(stream);

                }

                var pg = doc.Pages[0] as Section;
                Assert.IsNotNull(pg.Header);
                Assert.IsNotNull(pg.Footer);



                var p2ref = doc.FindAComponentById("secondParaPage") as HTMLPageNumber;
                var p3ref = doc.FindAComponentById("thirdParaPage") as HTMLPageNumber;
                var p3act = doc.FindAComponentById("thirdPageValue") as HTMLPageNumber;

                Assert.AreEqual("2", p2ref.OutputValue, "The P2 reference was not valid");
                Assert.AreEqual("3", p3ref.OutputValue, "The P3 reference was not valid");
                Assert.AreEqual("3", p3act.OutputValue, "The P3 actual was not valid");
            }

        }


        [TestMethod()]
        public void Html5Tags()
        {
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/Html5AllTags.html",
                this.TestContext);
            
            using (var doc = Document.ParseDocument(path))
            {
                var model = new
                {
                    fragmentContent = "Content for the fragment"
                };
                doc.Params["model"] = model;

                using (var stream = DocStreams.GetOutputStream("Html5AllTags.pdf"))
                {
                    doc.LayoutComplete += DocumentParsing_Layout;
                    doc.SaveAsPDF(stream);

                }

            }

            Assert.Inconclusive();

        }

        [TestMethod()]
        public void FontFace()
        {

            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/FontFace.html",
                this.TestContext);
            
            using var doc = Document.ParseDocument(path);
            doc.RenderOptions.Compression = OutputCompressionType.None;

            var model = new
            {
                fragmentContent = "Content for the fragment"
            };
            doc.Params["model"] = model;

            using var stream = DocStreams.GetOutputStream("FontFace.pdf");
            doc.SaveAsPDF(stream);

            //Check the remote style link for Fraunces

            var remote = doc.Styles[0] as StyleGroup;
                    
            Assert.IsNotNull(remote);
            Assert.AreEqual(4, remote.Styles.Count, "4 fonts were NOT loaded from the remote source");

            bool[] checks = new bool[4];
            //Should be Italic 400 and 700 + Regular 400 and 700
            for(var i = 0; i < 4; i++)
            {
                var one = remote.Styles[i] as StyleFontFace;
                Assert.IsNotNull(one);
                Assert.AreEqual("Fraunces", one.FontFamily.ToString(), "The font was not fraunces");

                if(one.FontStyle == Scryber.Drawing.FontStyle.Regular)
                {
                    if (one.FontWeight == 400)
                        checks[0] = true;
                    else if (one.FontWeight == 700)
                        checks[1] = true;
                }
                else if(one.FontStyle == Scryber.Drawing.FontStyle.Italic)
                {
                    if (one.FontWeight == 400)
                        checks[2] = true;
                    else if (one.FontWeight == 700)
                        checks[3] = true;
                }
            }

            Assert.IsTrue(checks[0], "No regular 400 weight");
            Assert.IsTrue(checks[1], "No regular 700 weight");
            Assert.IsTrue(checks[2], "No italic 400 weight");
            Assert.IsTrue(checks[3], "No italic 700 weight");

            //Check the local style groups for 2 fontface and 2 styles

            var inline = doc.Styles[1] as StyleGroup;

            Assert.IsNotNull(inline);
            Assert.AreEqual(4, inline.Styles.Count, "4 styles were not defined locally");

            var ffone = inline.Styles[0] as StyleFontFace;
            Assert.IsNotNull(ffone);
            Assert.AreEqual("Roboto", ffone.FontFamily.ToString(), "Inline @fontface was not Robototo");
            Assert.AreEqual(Scryber.Drawing.FontStyle.Regular, ffone.FontStyle, "Inline @fontface was not regular");
            Assert.AreEqual(900, ffone.FontWeight, "Inline @fontface was not black");
            Assert.IsNotNull(ffone.Source, "No source was set");

            var name = Font.GetFullName(ffone.FontFamily.FamilyName, ffone.FontWeight, ffone.FontStyle);
            var rsrc1 = doc.SharedResources.GetResource(PDFResource.FontDefnResourceType, name);
            Assert.IsNotNull(rsrc1);

            var fftwo = inline.Styles[1] as StyleFontFace;
            Assert.IsNotNull(fftwo);
            Assert.AreEqual("'Open Sans'", fftwo.FontFamily.ToString(), "Inline @fontface was not 'Open Sans'");
            Assert.AreEqual(Scryber.Drawing.FontStyle.Regular, fftwo.FontStyle, "Inline @fontface was not regular");
            Assert.AreEqual(300, fftwo.FontWeight, "Inline @fontface was not light");
            Assert.IsNotNull(fftwo.Source, "No source was set");

            name = Font.GetFullName(fftwo.FontFamily.FamilyName, fftwo.FontWeight, fftwo.FontStyle);
            rsrc1 = doc.SharedResources.GetResource(PDFResource.FontDefnResourceType, name);
            Assert.IsNotNull(rsrc1, "The open sans font was not found");
        }


        [TestMethod()]
        public void FontFaceAveryLocal()
        {

            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/FontFaceAvery.html",
                this.TestContext);
            
            using var doc = Document.ParseDocument(path);
            doc.RenderOptions.Compression = OutputCompressionType.None;

            var model = new
            {
                fragmentContent = "Content for the fragment"
            };
            doc.Params["model"] = model;

            using var stream = DocStreams.GetOutputStream("FontFace_AveryBusiness.pdf");
            doc.SaveAsPDF(stream);

            var style = doc.Styles[0] as StyleGroup;
            Assert.IsNotNull(style);

            Assert.AreEqual(4, style.Styles.Count, "4 fonts were NOT loaded from the remote source");

            
            //Should be AveryBusiness and Open Sans for the first 2
            for (var i = 0; i < 2; i++)
            {
                var one = style.Styles[i] as StyleFontFace;
                Assert.IsNotNull(one);
                if (i == 0)
                {
                    Assert.AreEqual("AveryBusiness", one.FontFamily.ToString(), "The first font was not AveryBusiness");
                    Assert.AreEqual(Scryber.Drawing.FontStyle.Regular, one.FontStyle);
                    Assert.AreEqual(400, one.FontWeight);
                }
                else if (i == 1)
                {
                    Assert.AreEqual("'Open Sans'", one.FontFamily.ToString(), "The second font was not Open Sans");
                    Assert.AreEqual(Scryber.Drawing.FontStyle.Regular, one.FontStyle);
                    Assert.AreEqual(300, one.FontWeight);
                }
            }

            
            
            var name = Font.GetFullName("AveryBusiness", 400, Scryber.Drawing.FontStyle.Regular);
            var rsrc1 = doc.SharedResources.GetResource(PDFResource.FontDefnResourceType, name) as PDFFontResource;
            Assert.IsNotNull(rsrc1);
            Assert.AreEqual("Archive", rsrc1.Definition.WindowsName); //Taken from the font file name table

            
            name = Font.GetFullName("Open Sans", 300, Scryber.Drawing.FontStyle.Regular);
            rsrc1 = doc.SharedResources.GetResource(PDFResource.FontDefnResourceType, name) as PDFFontResource;
            Assert.IsNotNull(rsrc1, "The open sans font was not found");
            Assert.AreEqual("OpenSansLight", rsrc1.Definition.WindowsName); //Taken from the font file name table
        }

        [TestMethod()]
        public void FontFaceBase64()
        {
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/FontFaceBase64.html",
                this.TestContext);
            
            using var doc = Document.ParseDocument(path);
            doc.RenderOptions.Compression = OutputCompressionType.None;

            var model = new
            {
                fragmentContent = "Content for the fragment"
            };
            doc.Params["model"] = model;



            using var stream = DocStreams.GetOutputStream("FontFaceBase64.pdf");
            doc.SaveAsPDF(stream);

            //Check the remote style link for Fraunces

            var remote = doc.Styles[0] as StyleGroup;

            Assert.IsNotNull(remote);
            Assert.AreEqual(4, remote.Styles.Count, "4 fonts were NOT loaded from the remote source");

            bool[] checks = new bool[4];
            //Should be Italic 400 and 700 + Regular 400 and 700
            for (var i = 0; i < 4; i++)
            {
                var one = remote.Styles[i] as StyleFontFace;
                Assert.IsNotNull(one);
                Assert.AreEqual("Fraunces", one.FontFamily.ToString(), "The font was not fraunces");

                if (one.FontStyle == Scryber.Drawing.FontStyle.Regular)
                {
                    if (one.FontWeight == 400)
                        checks[0] = true;
                    else if (one.FontWeight == 700)
                        checks[1] = true;
                }
                else if (one.FontStyle == Scryber.Drawing.FontStyle.Italic)
                {
                    if (one.FontWeight == 400)
                        checks[2] = true;
                    else if (one.FontWeight == 700)
                        checks[3] = true;
                }
            }

            Assert.IsTrue(checks[0], "No regular 400 weight");
            Assert.IsTrue(checks[1], "No regular 700 weight");
            Assert.IsTrue(checks[2], "No italic 400 weight");
            Assert.IsTrue(checks[3], "No italic 700 weight");

            //Check the local style groups for 2 fontface and 2 styles

            var inline = doc.Styles[1] as StyleGroup;

            Assert.IsNotNull(inline);
            Assert.AreEqual(4, inline.Styles.Count, "4 styles were not defined locally");

            var ffone = inline.Styles[0] as StyleFontFace;
            Assert.IsNotNull(ffone);
            Assert.AreEqual("Oswald", ffone.FontFamily.ToString(), "Inline @fontface was not Oswald");
            Assert.AreEqual(Scryber.Drawing.FontStyle.Regular, ffone.FontStyle, "Inline @fontface was not regular");
            Assert.AreEqual(800, ffone.FontWeight, "Inline @fontface was not black");
            Assert.IsNotNull(ffone.Source, "No source was set");


            var name = Font.GetFullName(ffone.FontFamily.FamilyName, ffone.FontWeight, ffone.FontStyle);
            var rsrc1 = doc.SharedResources.GetResource(PDFResource.FontDefnResourceType, name);
            Assert.IsNotNull(rsrc1, "The oswald font was not found in the document");

            var fftwo = inline.Styles[1] as StyleFontFace;
            Assert.IsNotNull(fftwo);
            Assert.AreEqual("'Open Sans'", fftwo.FontFamily.ToString(), "Inline @fontface was not 'Open Sans'");
            Assert.AreEqual(Scryber.Drawing.FontStyle.Regular, fftwo.FontStyle, "Inline @fontface was not regular");
            Assert.AreEqual(300, fftwo.FontWeight, "Inline @fontface was not light");
            Assert.IsNotNull(fftwo.Source, "No source was set");

            name = Font.GetFullName(fftwo.FontFamily.FamilyName, fftwo.FontWeight, fftwo.FontStyle);
            rsrc1 = doc.SharedResources.GetResource(PDFResource.FontDefnResourceType, name);
            Assert.IsNotNull(rsrc1, "The open sans font was not found");
        }

        [TestMethod()]
        public async Task FontFaceAsync()
        {
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/FontFace.html",
                this.TestContext);
            using var doc = Document.ParseDocument(path);
            doc.RenderOptions.Compression = OutputCompressionType.None;

            var model = new
            {
                fragmentContent = "Content for the fragment"
            };
            doc.Params["model"] = model;

            using var stream = DocStreams.GetOutputStream("FontFaceAsync.pdf");
            await doc.SaveAsPDFAsync(stream);

            //Check the remote style link for Fraunces

            var remote = doc.Styles[0] as StyleGroup;

            Assert.IsNotNull(remote);
            Assert.AreEqual(4, remote.Styles.Count, "4 fonts were NOT loaded from the remote source");

            bool[] checks = new bool[4];
            //Should be Italic 400 and 700 + Regular 400 and 700
            for (var i = 0; i < 4; i++)
            {
                var one = remote.Styles[i] as StyleFontFace;
                Assert.IsNotNull(one);
                Assert.AreEqual("Fraunces", one.FontFamily.ToString(), "The font was not fraunces");

                if (one.FontStyle == Scryber.Drawing.FontStyle.Regular)
                {
                    if (one.FontWeight == 400)
                        checks[0] = true;
                    else if (one.FontWeight == 700)
                        checks[1] = true;
                }
                else if (one.FontStyle == Scryber.Drawing.FontStyle.Italic)
                {
                    if (one.FontWeight == 400)
                        checks[2] = true;
                    else if (one.FontWeight == 700)
                        checks[3] = true;
                }
            }

            Assert.IsTrue(checks[0], "No regular 400 weight");
            Assert.IsTrue(checks[1], "No regular 700 weight");
            Assert.IsTrue(checks[2], "No italic 400 weight");
            Assert.IsTrue(checks[3], "No italic 700 weight");

            //Check the local style groups for 2 fontface and 2 styles

            var inline = doc.Styles[1] as StyleGroup;

            Assert.IsNotNull(inline);
            Assert.AreEqual(4, inline.Styles.Count, "4 styles were not defined locally");

            var ffone = inline.Styles[0] as StyleFontFace;
            Assert.IsNotNull(ffone);
            Assert.AreEqual("Roboto", ffone.FontFamily.ToString(), "Inline @fontface was not Roboto");
            Assert.AreEqual(Scryber.Drawing.FontStyle.Regular, ffone.FontStyle, "Inline @fontface was not regular");
            Assert.AreEqual(900, ffone.FontWeight, "Inline @fontface was not black");
            Assert.IsNotNull(ffone.Source, "No source was set");

            var name = Font.GetFullName(ffone.FontFamily.FamilyName, ffone.FontWeight, ffone.FontStyle);
            var rsrc1 = doc.SharedResources.GetResource(PDFResource.FontDefnResourceType, name);
            Assert.IsNotNull(rsrc1);

            var fftwo = inline.Styles[1] as StyleFontFace;
            Assert.IsNotNull(fftwo);
            Assert.AreEqual("'Open Sans'", fftwo.FontFamily.ToString(), "Inline @fontface was not 'Open Sans'");
            Assert.AreEqual(Scryber.Drawing.FontStyle.Regular, fftwo.FontStyle, "Inline @fontface was not regular");
            Assert.AreEqual(300, fftwo.FontWeight, "Inline @fontface was not light");
            Assert.IsNotNull(fftwo.Source, "No source was set");

            name = Font.GetFullName(fftwo.FontFamily.FamilyName, fftwo.FontWeight, fftwo.FontStyle);
            rsrc1 = doc.SharedResources.GetResource(PDFResource.FontDefnResourceType, name);
            Assert.IsNotNull(rsrc1, "The open sans font was not found");
        }


        [TestMethod()]
        public async Task FontFaceBase64Async()
        {
            
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/FontFaceBase64.html",
                this.TestContext);
            
            using var doc = Document.ParseDocument(path);
            doc.RenderOptions.Compression = OutputCompressionType.None;

            var model = new
            {
                fragmentContent = "Content for the fragment"
            };
            doc.Params["model"] = model;



            using var stream = DocStreams.GetOutputStream("FontFaceBase64Async.pdf");
            await doc.SaveAsPDFAsync(stream);

            //Check the remote style link for Fraunces

            var remote = doc.Styles[0] as StyleGroup;

            Assert.IsNotNull(remote);
            Assert.AreEqual(4, remote.Styles.Count, "4 fonts were NOT loaded from the remote source");

            bool[] checks = new bool[4];
            //Should be Italic 400 and 700 + Regular 400 and 700
            for (var i = 0; i < 4; i++)
            {
                var one = remote.Styles[i] as StyleFontFace;
                Assert.IsNotNull(one);
                Assert.AreEqual("Fraunces", one.FontFamily.ToString(), "The font was not fraunces");

                if (one.FontStyle == Scryber.Drawing.FontStyle.Regular)
                {
                    if (one.FontWeight == 400)
                        checks[0] = true;
                    else if (one.FontWeight == 700)
                        checks[1] = true;
                }
                else if (one.FontStyle == Scryber.Drawing.FontStyle.Italic)
                {
                    if (one.FontWeight == 400)
                        checks[2] = true;
                    else if (one.FontWeight == 700)
                        checks[3] = true;
                }
            }

            Assert.IsTrue(checks[0], "No regular 400 weight");
            Assert.IsTrue(checks[1], "No regular 700 weight");
            Assert.IsTrue(checks[2], "No italic 400 weight");
            Assert.IsTrue(checks[3], "No italic 700 weight");

            //Check the local style groups for 2 fontface and 2 styles

            var inline = doc.Styles[1] as StyleGroup;

            Assert.IsNotNull(inline);
            Assert.AreEqual(4, inline.Styles.Count, "4 styles were not defined locally");

            var ffone = inline.Styles[0] as StyleFontFace;
            Assert.IsNotNull(ffone);
            Assert.AreEqual("Oswald", ffone.FontFamily.ToString(), "Inline @fontface was not Oswald");
            Assert.AreEqual(Scryber.Drawing.FontStyle.Regular, ffone.FontStyle, "Inline @fontface was not regular");
            Assert.AreEqual(800, ffone.FontWeight, "Inline @fontface was not black");
            Assert.IsNotNull(ffone.Source, "No source was set");
            

            var name = Font.GetFullName(ffone.FontFamily.FamilyName, ffone.FontWeight, ffone.FontStyle);
            var rsrc1 = doc.SharedResources.GetResource(PDFResource.FontDefnResourceType, name);
            Assert.IsNotNull(rsrc1, "The oswald font was not found in the document");

            var fftwo = inline.Styles[1] as StyleFontFace;
            Assert.IsNotNull(fftwo);
            Assert.AreEqual("'Open Sans'", fftwo.FontFamily.ToString(), "Inline @fontface was not 'Open Sans'");
            Assert.AreEqual(Scryber.Drawing.FontStyle.Regular, fftwo.FontStyle, "Inline @fontface was not regular");
            Assert.AreEqual(300, fftwo.FontWeight, "Inline @fontface was not light");
            Assert.IsNotNull(fftwo.Source, "No source was set");

            name = Font.GetFullName(fftwo.FontFamily.FamilyName, fftwo.FontWeight, fftwo.FontStyle);
            rsrc1 = doc.SharedResources.GetResource(PDFResource.FontDefnResourceType, name);
            Assert.IsNotNull(rsrc1, "The open sans font was not found");
        }

        /// <summary>
        /// Tests that the font face will fallback to the closest weight / style and none
        /// </summary>
        [TestMethod()]
        public void FontFaceWeightFallback()
        {

            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/FontFaceFallback.html",
                this.TestContext);

            using var doc = Document.ParseDocument(path);
            //doc.RenderOptions.Compression = OutputCompressionType.None;
            doc.TraceLog.SetRecordLevel(TraceRecordLevel.Verbose);

            using var stream = DocStreams.GetOutputStream("FontFaceFallback.pdf");
            doc.LayoutComplete += DocumentParsing_Layout;
            doc.SaveAsPDF(stream);

            //Check the remote style link for Fraunces

            var remote = doc.Styles[0] as StyleGroup;

            Assert.IsNotNull(remote);
            Assert.AreEqual(6, remote.Styles.Count, "6 fonts were NOT loaded from the remote source");

            bool[] checks = new bool[6];
            //Should be Italic 100, 400 and 700 + Regular 100, 400 and 700
            for (var i = 0; i < 6; i++)
            {
                var one = remote.Styles[i] as StyleFontFace;
                Assert.IsNotNull(one);
                Assert.AreEqual("Fraunces", one.FontFamily.ToString(), "The font was not fraunces");

                if (one.FontStyle == Scryber.Drawing.FontStyle.Regular)
                {
                    if (one.FontWeight == 400)
                        checks[0] = true;
                    else if (one.FontWeight == 700)
                        checks[1] = true;
                    else if (one.FontWeight == 100)
                        checks[2] = true;
                }
                else if (one.FontStyle == Scryber.Drawing.FontStyle.Italic)
                {
                    if (one.FontWeight == 400)
                        checks[3] = true;
                    else if (one.FontWeight == 700)
                        checks[4] = true;
                    else if (one.FontWeight == 100)
                        checks[5] = true;

                }
            }

            Assert.IsNotNull(_layoutcontext);
            var pg = _layoutcontext.DocumentLayout.AllPages[0];
            var blocks = pg.ContentBlock.Columns[0].Contents;

            var regular = GetFontFallbackRunBegin(blocks[0]);
            var italic = GetFontFallbackRunBegin(blocks[1]);
            var thin = GetFontFallbackRunBegin(blocks[2]);
            var thinfallback = GetFontFallbackRunBegin(blocks[3]);
            var medfallback = GetFontFallbackRunBegin(blocks[4]);
            var medfallbackwithbold = GetFontFallbackRunBegin(blocks[5]);
            var blackfallback = GetFontFallbackRunBegin(blocks[6]);
            var courierfallback = GetFontFallbackRunBegin(blocks[7]);
            var fallbackwithstyle = GetFontFallbackRunBegin(blocks[8]);
            var unicode = GetFontFallbackRunBegin(blocks[9]);
            var uniUnsupported = GetFontFallbackRunBegin(blocks[10]);

            //no fallback
            AssertFontFallback(regular.TextRenderOptions, "Fraunces", 400, false);

            //no fallback italic
            AssertFontFallback(italic.TextRenderOptions, "Fraunces", 400, true);

            //no fallback thin
            AssertFontFallback(thin.TextRenderOptions, "Fraunces", 100, false);

            //fallback from 200 to 100
            AssertFontFallback(thinfallback.TextRenderOptions, "Fraunces", 100, false);

            //fallback from 300 to 400
            AssertFontFallback(medfallback.TextRenderOptions, "Fraunces", 400, false);

            //fallback from 300 to 400
            AssertFontFallback(medfallbackwithbold.TextRenderOptions, "Fraunces", 400, false);

            //fallback from 900 to 700
            AssertFontFallback(blackfallback.TextRenderOptions, "Fraunces", 700, false);

            //fallback from Roboto to courier
            AssertFontFallback(courierfallback.TextRenderOptions, "Courier", 400, false);

            //Roboto not loaded so use sans-serif
            AssertFontFallback(fallbackwithstyle.TextRenderOptions, "Helvetica", 400, false);

            //Use unicode font
            AssertFontFallback(unicode.TextRenderOptions, "Noto Serif Ethiopic", 400, false);

            //Unicode fails to courier
            AssertFontFallback(uniUnsupported.TextRenderOptions, "Noto Sans TC", 400, false);


            //Cache check
            //The previous fonts should be cached and available for re-use in the file.
            
            
            using var doc2 = Document.ParseDocument(path);
            //doc.RenderOptions.Compression = OutputCompressionType.None;
            doc2.TraceLog.SetRecordLevel(TraceRecordLevel.Verbose);

            using var stream2 = DocStreams.GetOutputStream("FontFaceFallbackCached.pdf");
            doc2.SaveAsPDF(stream2);

        }

        private void AssertFontFallback(PDFTextRenderOptions options, string familyName, int weight, bool italic)
        {
            Assert.AreEqual(familyName, options.Font.Resource.Definition.Family);
            Assert.AreEqual(weight, options.Font.Resource.Definition.Weight);
            Assert.AreEqual(italic, options.Font.Resource.Definition.Italic);
        }

        private PDFTextRunBegin GetFontFallbackRunBegin(PDFLayoutItem para)
        {
            var block = (PDFLayoutBlock)para;
            var line = (PDFLayoutLine)block.Columns[0].Contents[0];
            var run = (PDFTextRunBegin)line.Runs[0];

            return run;
        }

        public class ReadMeModel
        {
            public string titlestyle { get; set; }
            public string title { get; set; }
            public ReadMeModelItem[] items { get; set; }
        }

        public class ReadMeModelItem
        {
            public string name { get; set; }
        }


        


        [TestMethod()]
        public void ArticlesWithCountersHtml()
        {
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/ArticleCounters.html",
                this.TestContext);
            using (var doc = Document.ParseDocument(path))
            {
                doc.Params["title"] = "Hello World";

                using (var stream = DocStreams.GetOutputStream("ArticleCounters.pdf"))
                {
                    doc.LayoutComplete += DocumentParsing_Layout;
                    doc.SaveAsPDF(stream);
                }

            }


            Assert.IsNotNull(_layoutcontext);
            var layout = _layoutcontext.DocumentLayout;
            Assert.IsNotNull(layout);

            Assert.AreEqual(1, layout.AllPages.Count);
            Assert.AreEqual(3, layout.AllPages[0].ContentBlock.Columns[0].Contents.Count);

            var article1 = layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(article1);
            Assert.AreEqual("PrimaryArticle", article1.Owner.ID);

            var article2 = layout.AllPages[0].ContentBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(article2);
            Assert.AreEqual("SecondArticle", article2.Owner.ID);

            var divList = layout.AllPages[0].ContentBlock.Columns[0].Contents[2] as PDFLayoutBlock;
            Assert.IsNotNull(divList);
            Assert.AreEqual("list-counters", divList.Owner.ID);

            //first article content

            var headblock = article1.Columns[0].Contents[0] as PDFLayoutBlock;
            AssertHasBeforeContent(headblock, "1: ", "Top h1 of first outer article");

            var innerArticle = article1.Columns[0].Contents[2] as PDFLayoutBlock;
            headblock = innerArticle.Columns[0].Contents[0] as PDFLayoutBlock;
            AssertHasBeforeContent(headblock, "1.1: ", "First inner h2 of first outer article");

            innerArticle = article1.Columns[0].Contents[3] as PDFLayoutBlock;
            headblock = innerArticle.Columns[0].Contents[0] as PDFLayoutBlock;
            AssertHasBeforeContent(headblock, "1.2: ", "Second inner h2 of first outer article");

            //second article content

            headblock = article2.Columns[0].Contents[0] as PDFLayoutBlock;
            AssertHasBeforeContent(headblock, "2: ", "Top h1 of second outer article");

            innerArticle = article2.Columns[0].Contents[2] as PDFLayoutBlock;
            headblock = innerArticle.Columns[0].Contents[0] as PDFLayoutBlock;
            AssertHasBeforeContent(headblock, "2.1: ", "First inner h2 of second outer article");

            innerArticle = article2.Columns[0].Contents[3] as PDFLayoutBlock;
            headblock = innerArticle.Columns[0].Contents[0] as PDFLayoutBlock;
            AssertHasBeforeContent(headblock, "2.2: ", "Second inner h2 of second outer article");

            //ordered list
            var ol = divList.Columns[0].Contents[1] as PDFLayoutBlock;

            var item = ol.Columns[0].Contents[0] as PDFLayoutBlock;
            AssertHasBeforeContent(item, "1 ", "1 ");

            item = ol.Columns[0].Contents[1] as PDFLayoutBlock;
            AssertHasBeforeContent(item, "2 ", "2 ");

            //inner ordered list
            var iol = item.Columns[0].Contents[1] as PDFLayoutBlock;

            item = iol.Columns[0].Contents[0] as PDFLayoutBlock;
            AssertHasBeforeContent(item, "2.1 ", "2.1 ");

            item = iol.Columns[0].Contents[1] as PDFLayoutBlock;
            AssertHasBeforeContent(item, "2.2 ", "2.2 ");

            item = iol.Columns[0].Contents[2] as PDFLayoutBlock;
            AssertHasBeforeContent(item, "2.3 ", "2.3 ");

            //inner inner ordered list
            var iiol = item.Columns[0].Contents[1] as PDFLayoutBlock;

            var inner = iiol.Columns[0].Contents[0] as PDFLayoutBlock;
            AssertHasBeforeContent(inner, "2.3.1 ", "2.3.1 ");

            inner = iiol.Columns[0].Contents[1] as PDFLayoutBlock;
            AssertHasBeforeContent(inner, "2.3.2 ", "2.3.2 ");

            //second inner inner ordered list
            iiol = item.Columns[0].Contents[2] as PDFLayoutBlock;

            inner = iiol.Columns[0].Contents[0] as PDFLayoutBlock;
            AssertHasBeforeContent(inner, "2.3.1 ", "2.3.1 ");

            inner = iiol.Columns[0].Contents[1] as PDFLayoutBlock;
            AssertHasBeforeContent(inner, "2.3.2 ", "2.3.2 ");

            inner = iiol.Columns[0].Contents[2] as PDFLayoutBlock;
            AssertHasBeforeContent(inner, "2.3.3 ", "2.3.2 ");

            //back out to inner

            item = iol.Columns[0].Contents[3] as PDFLayoutBlock;
            AssertHasBeforeContent(item, "2.4 ", "2.4 ");

            //back out to the top
            item = ol.Columns[0].Contents[2] as PDFLayoutBlock;
            AssertHasBeforeContent(item, "3 ", "3 ");

            item = ol.Columns[0].Contents[3] as PDFLayoutBlock;
            AssertHasBeforeContent(item, "4 ", "4 ");

            //and a new list at the bottom
            ol = divList.Columns[0].Contents[2] as PDFLayoutBlock;
            item = ol.Columns[0].Contents[0] as PDFLayoutBlock;
            AssertHasBeforeContent(item, "1 ", "1 ");

            item = ol.Columns[0].Contents[1] as PDFLayoutBlock;
            AssertHasBeforeContent(item, "2 ", "2 ");

        }

        

        private void AssertHasBeforeContent(PDFLayoutBlock itemblock, string pseudoContent, string itemName)
        {
            Assert.IsNotNull(itemblock, "The block for '" + itemName + "' was null");
            Assert.IsTrue(itemblock.Columns[0].Contents.Count > 0, "The block for '" + itemName + "' has no content");
            var line = itemblock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line, "The block for '" + itemName + "' first item was not a line");
            //chars after inlinebegin and textbegin

            var all = "";
            for (var i = 0; i < line.Runs.Count; i++)
            {
                if (line.Runs[1] is PDFLayoutInlineBegin)
                {
                    all = "";
                }
                else if (line.Runs[i] is PDFTextRunCharacter chars)
                {
                    all += chars.Characters;
                }
                else if (line.Runs[i] is PDFLayoutInlineEnd)
                    break;
            }
            Assert.AreEqual(pseudoContent, all, "The pseudo characters for '" + itemName + "' did not match");
        }

        [TestMethod()]
        public void RestrictedHtml()
        {
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/RestrictedHtml.html",
                this.TestContext);

            using (var doc = Document.ParseDocument(path))
            {
                doc.RenderOptions.PasswordProvider = new Scryber.PDF.Secure.DocumentPasswordProvider("Password");
                doc.Params["title"] = "Hello World";

                using (var stream = DocStreams.GetOutputStream("RestrictedHtml.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

            }

            Assert.Inconclusive("Can only check by opening the document");
        }

        [TestMethod()]
        public void RestrictedWithoutPasswordHtml()
        {
            
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/RestrictedHtml.html",
                this.TestContext);
            using (var doc = Document.ParseDocument(path))
            {
                //Need to set this, otherwise the 
                doc.ConformanceMode = ParserConformanceMode.Lax;
                doc.Params["title"] = "Hello World";

                using (var stream = DocStreams.GetOutputStream("RestrictedNoPasswordHtml.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

            }

            Assert.Inconclusive("Can only check by opening the document");
        }

        [TestMethod()]
        public void RestrictedProtectedHtml()
        {
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/RestrictedHtml.html",
                this.TestContext);

            using (var doc = Document.ParseDocument(path))
            {
                doc.RenderOptions.PasswordProvider = new Scryber.PDF.Secure.DocumentPasswordProvider("Password", "Password");
                doc.Params["title"] = "Hello World";
                using (var stream = DocStreams.GetOutputStream("ProtectedHtml.pdf"))
                {
                    doc.PreRender += delegate(object sender, RenderEventArgs args)
                    {
                        var provider = doc.RenderOptions.PasswordProvider;
                        var perms = doc.RenderOptions.Permissions;
                        var writer = doc.RenderOptions.WriterFactory;
                        Assert.IsNotNull(provider, "Password provider was null");
                        Assert.IsTrue(perms.HasRestrictions);
                        Assert.IsTrue(perms.AllowFormFilling);
                        Assert.IsFalse(perms.AllowCopying);
                        Assert.IsInstanceOfType(writer, typeof(PDFSecureWrite14Factory));
                    };
                    
                    doc.SaveAsPDF(stream);
                }

            }

            Assert.Inconclusive("Can only check by opening the document");
        }


        [TestMethod()]
        public void BasePath()
        {
             var src = @"<?scryber append-log=true parser-log=true ?>
                         <html xmlns='http://www.w3.org/1999/xhtml' >
                            <head>
                                <title>Html document title</title>
                                <base href='https://raw.githubusercontent.com/richard-scryber/scryber.core/master/Scryber.UnitTest/Content/HTML/' />
                                <link rel='stylesheet' href='CSS/Include.css' media='print' />
                              </head>
                            <body class='grey' style='margin:20px;' >
                                <p id='myPara' >This is a paragraph of content</p>
                                <img id='myToroid' src='./Images/Toroid24.png' style='width:100pt' />
                                <embed id='myDrawing' src='../HTML/Fragments/MyDrawing.svg' />
                               </body>
                        </html>";

            using (var sr = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent);
                doc.RenderOptions.AllowMissingImages = false; //Will error if the image is not found
                
                Assert.IsInstanceOfType(doc, typeof(HTMLDocument));
                Assert.AreEqual("https://raw.githubusercontent.com/richard-scryber/scryber.core/master/Scryber.UnitTest/Content/HTML/", doc.LoadedSource, "Loaded Source is not correct");
                
                using (var stream = DocStreams.GetOutputStream("DynamicBasePath.pdf"))
                {
                    doc.LayoutComplete += DocumentParsing_Layout;
                    doc.AppendTraceLog = true;
                    doc.SaveAsPDF(stream);
                }
                Assert.AreEqual(1, doc.Styles.Count, "Remote styles were not loaded");
                Assert.IsInstanceOfType(doc.Styles[0], typeof(StyleGroup), "The remote styles is not a group");

                var img = doc.FindAComponentById("myToroid") as Image;
                Assert.IsNotNull(img.XObject,"The image was not loaded from the remote source");

                var embed = doc.FindAComponentById("myDrawing") as SVGCanvas;
                Assert.IsNotNull(embed);
                Assert.AreNotEqual(0, embed.Contents.Count, "Embedded content was not loaded from the source");
                
            }
        }

        [TestMethod()]
        public void XLinqHtmlParsing()
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
            var doc = Document.ParseDocument(html.CreateReader(), string.Empty, ParseSourceType.DynamicContent);

            Assert.IsNotNull(doc);
            Assert.IsInstanceOfType(doc, typeof(HTMLDocument));
            
            using (var stream = DocStreams.GetOutputStream("HelloWorldLinq.pdf"))
                doc.SaveAsPDF(stream);

            Assert.AreEqual("Hello World", doc.Info.Title);
        }

        [TestMethod()]
        public void StringHtmlParsing()
        {
            var src = @"<html xmlns='http://www.w3.org/1999/xhtml' >
                            <head>
                                <title>Hello World</title>
                              </head>
                            <body>
                                <div style='padding: 10px' >Hello World.</div>
                            </body>
                        </html>";

            using (var reader = new StringReader(src))
            {
                var doc = Document.ParseDocument(reader, string.Empty, ParseSourceType.DynamicContent);

                Assert.IsNotNull(doc);
                Assert.IsInstanceOfType(doc, typeof(HTMLDocument));
                
                using (var stream = DocStreams.GetOutputStream("HelloWorldString.pdf"))
                    doc.SaveAsPDF(stream);
                
                Assert.AreEqual("Hello World", doc.Info.Title);
            }
        }

        
        
        private class CustomReferenceResolver
        {

            private int _invocationCount = 0;
            
            public IComponent CustomResolve(string filepath, string xpath, ParserSettings settings)
            {
                if (filepath == "MyTsAndCs")
                {
                    this._invocationCount++;
                    using (var tsAndCs = LoadTermsStream())
                    {
                        //We have our stream so just do the parsing again with the same settings
                        var comp = Document.Parse(filepath, tsAndCs, ParseSourceType.DynamicContent, CustomResolve, settings);
                        return comp;
                    }
                }
                else
                {
                    filepath = System.IO.Path.Combine("C:/", filepath);
                    return Document.Parse(filepath, CustomResolve, settings);
                }
            }

            private StringReader LoadTermsStream()
            {
                return new StringReader("<p xmlns='http://www.w3.org/1999/xhtml'>These are my terms</p>");
            }

            public bool AssertWasInvokedOnce()
            {
                if (this._invocationCount == 1)
                    return true;
                else
                {
                    throw new InvalidOperationException("The custom resolver method was invoked " +
                                                        this._invocationCount + " times - expected 1");
                }
            }
        }

        

        [TestMethod()]
        public void StringParsingWithResolver()
        {
            var src = @"<html xmlns='http://www.w3.org/1999/xhtml' >
                            <head>
                                <title>Hello World</title>
                              </head>
                            <body>
                                <div style='padding: 10px' >Hello World.</div>
                                <embed id='TsAndCs' src='MyTsAndCs' />
                            </body>
                        </html>";

            var resolver = new CustomReferenceResolver();
            
            using (var reader = new StringReader(src))
            {
                var comp = Document.Parse(string.Empty, reader, ParseSourceType.DynamicContent, resolver.CustomResolve);

                Assert.IsNotNull(comp);
                Assert.IsInstanceOfType(comp, typeof(HTMLDocument));
                var doc = comp as HTMLDocument;
                
                using (var stream = DocStreams.GetOutputStream("HelloWorldStringWithResolver.pdf"))
                    doc.SaveAsPDF(stream);
                
                Assert.AreEqual("Hello World", doc.Info.Title);
                resolver.AssertWasInvokedOnce();
            }
        }

        [TestMethod()]
        public void SimpleExpressionBinding()
        {
            var src = @"<!DOCTYPE HTML >
                <html lang='en' xmlns='http://www.w3.org/1999/xhtml' >
                    <head>
                        <title>{{concat('Hello ', model.user.firstname)}}</title>
                    </head>
                    <body>
                        <div style='color: {{theme.color}}; padding: {{theme.space}}; text-align: {{theme.align}}'>
                            {{concat('Hello ',model.user.firstname)}}.
                        </div>
                    </body>
                </html>";
            
            using (var reader = new StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                doc.Params["model"] = new
                {
                    user = new
                    {
                        firstname = "Richard",
                        salutation = "Mr"
                    }
                };
                doc.Params["theme"] = new
                {
                    color = "#FF0000",
                    space = "10pt",
                    align = "center"
                };

                using (var stream = DocStreams.GetOutputStream("SimpleModelExpressionBinding.pdf"))
                {
                    //Before databinding 
                    Assert.IsNull(doc.Info.Title);

                    doc.SaveAsPDF(stream);

                    //After databinding
                    Assert.AreEqual("Hello Richard", doc.Info.Title);
                }

            }
        }

        protected Document GetHelloWorld()
        {
            var doc = new Document();
            doc.Info.Title = "Hello World";

            var page = new Page();
            doc.Pages.Add(page);

            var div = new Div() { Padding = new Thickness(10) };
            page.Contents.Add(div);

            div.Contents.Add(new TextLiteral("Hello World"));

            return doc;
        }


        //
        //Order Items binding
        //


        public class User
        {
            public string Salutation { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }
        }

        public class Order
        {

            public int ID { get; set; }

            public string CurrencyFormat { get; set; }

            public double TaxRate { get; set; }

            public double Total { get; set; }

            public List<OrderItem> Items { get; set; }

            public int PaymentTerms { get; set; }
        }


        public class OrderItem
        {

            public string ItemNo { get; set; }

            public string ItemName { get; set; }

            public double Quantity { get; set; }

            public double ItemPrice { get; set; }

        }


        public class OrderMockService
        {

            public Order GetOrder(int id)
            {
                var order = new Order() { ID = id, CurrencyFormat = "£##0.00", TaxRate = 0.2 };
                order.Items = new List<OrderItem>(){
                    new OrderItem() { ItemNo = "O 12", ItemName = "Widget", Quantity = 2, ItemPrice = 12.5 },
                    new OrderItem() { ItemNo = "O 17", ItemName = "Sprogget", Quantity = 4, ItemPrice = 1.5 },
                    new OrderItem() { ItemNo = "I 13", ItemName = "M10 bolts with a counter clockwise thread on the inner content and a star nut top, tamper proof and locking ring included.", Quantity = 8, ItemPrice = 1.0 }
                };
                order.Total = (2.0 * 12.5) + (4.0 * 1.5) + (8 * 1.0);

                return order;
            }

        }

        [TestMethod]
        public void HTMLOrderItems()
        {
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/OrderItems.html",
                this.TestContext);
            var doc = Document.ParseDocument(path);
            var service = new OrderMockService();
            var user = new User() { Salutation = "Mr", FirstName = "Richard", LastName = "Smith" };
            var order = service.GetOrder(1);
            order.PaymentTerms = 0;

            doc.Params["model"] = new
            {
                user = user,
                order = order
            };

            doc.Params["style"] = new
            {
                rowColor = (Color)"#EEE",
                altColor = (Color)"#DDD",
                dateFormat ="dd MMMM yyyy",
                currencyFormat = "£##0.00"
            };
            
            using (var stream = DocStreams.GetOutputStream("OrderItemsTemplate.pdf"))
                doc.SaveAsPDF(stream);

            Assert.Inconclusive();
        }

        


        [TestMethod]
        public void HTMLCustomFunction()
        {
            

            var src = @"<html xmlns='http://www.w3.org/1999/xhtml' >
                            <head>
                              <title>Page Custom Function</title>
                            </head>
                            <body class='grey' style='margin:20px;' >
                                <p id='paraConstant' >{{ToUpper('test')}}.</p>
                                <p id='paraVariable' >{{ToUpper(model.title)}}.</p>
                                <p id='paraExpression' >{{toupper(concat('hello ', model.title))}}.</p>
                               </body>
                        </html>";

            var data = new
            {
                title = "My title"
            };

            using (var sr = new System.IO.StringReader(src))
            {
                using (var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent))
                {
                    doc.Params["model"] = data;
                    using (var stream = DocStreams.GetOutputStream("UpperCaseExpression.pdf"))
                    {
                        doc.SaveAsPDF(stream);
                    }

                    var pconst = doc.FindAComponentById("paraConstant") as Paragraph;
                    var pvar = doc.FindAComponentById("paraVariable") as Paragraph;
                    var pexpr = doc.FindAComponentById("paraExpression") as Paragraph;

                    Assert.AreEqual("TEST", (pconst.Contents[0] as TextLiteral).Text, "Constant text was not uppercased");
                    Assert.AreEqual("MY TITLE", (pvar.Contents[0] as TextLiteral).Text, "Variable text was not uppercased");
                    Assert.AreEqual("HELLO MY TITLE", (pexpr.Contents[0] as TextLiteral).Text, "Expression text was not uppercased");
                }
            }


        }



        [TestMethod()]
        public void InvalidBackgroundImage()
        {
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/InvalidBackgroundImage.html",
                this.TestContext);
            bool error = false;
            try
            {
                using (var doc = Document.ParseDocument(path))
                {
                    doc.ConformanceMode = ParserConformanceMode.Strict;
                    using (var stream = DocStreams.GetOutputStream("InvalidBackgroundImage.pdf"))
                    {
                        doc.LayoutComplete += DocumentParsing_Layout;
                        doc.SaveAsPDF(stream);
                    }
                }

            }
            catch(Exception)
            {
                error = true;
            }

            Assert.IsTrue(error, "An error was not raised when a background image was not available in Strict mode");


            error = true;
            try
            {
                using (var doc = Document.ParseDocument(path))
                {
                    using (var stream = DocStreams.GetOutputStream("InvalidBackgroundImage.pdf"))
                    {
                        //Set the conformance to lax
                        doc.ConformanceMode = ParserConformanceMode.Lax;

                        doc.LayoutComplete += DocumentParsing_Layout;
                        doc.SaveAsPDF(stream);
                    }
                }
                error = false;

            }
            catch (Exception)
            {
                error = true;
            }

            Assert.IsFalse(error, "An error was raised when a background image was not available in Lax mode");
        }

        [TestMethod]
        public void LargeFileTest()
        {
            
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/LargeFile.html",
                this.TestContext);
            var data = new
            {
                Items = GetListItems(10000)
            };

            using (var doc = Document.ParseDocument(path))
            {
                doc.Params["model"] = data;
                using (var stream = DocStreams.GetOutputStream("LargeFile.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

                var table = doc.FindAComponentById("largeTable") as HTMLTableGrid;

                //Check the row count including the header row
                Assert.AreEqual(10000 + 1, table.Rows.Count, "Number of rows does not match expected count");
            }
        }

        private class ListItem
        {
            public string Key { get; set; }
            public int Value { get; set; }
            public string Row { get; set; }
        }

        private static ListItem[] GetListItems(int count)
        {
            var mocks = new ListItem[count];

            for (int i = 0; i < count; i++)
            {
                ListItem m = new ListItem() { Key = "Item " + i.ToString(), Value = i, Row = (i % 2 == 1) ? "odd" : "even" };
                mocks[i] = m;
            }

            return mocks;
        }



        [TestMethod]
        public void PageNumberingTest()

        {

            var src = @"<html xmlns='http://www.w3.org/1999/xhtml' >
                            <head>
                              <title>Page Numbering</title>
                            </head>
                            <body class='grey' style='margin:20px;' >
                                <p id='myPara' >Page <page/> of <page property='Total' />.</p>
                               </body>
                        </html>";

            var data = new
            {
                Items = GetListItems(10)
            };
            using (var sr = new System.IO.StringReader(src))
            {
                using (var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent))
                {
                    doc.Params["model"] = data;
                    using (var stream = DocStreams.GetOutputStream("PageNumbers.pdf"))
                    {
                        doc.SaveAsPDF(stream);
                    }

                }
            }

            Assert.Inconclusive();
        }

        private PDFLayoutDocument _layout;
        
        private void Gradient_LayoutComplete(object sender, LayoutEventArgs args)
        {
            var context = (PDFLayoutContext)(args.Context);
            _layout = context.DocumentLayout;
        }

        


        [TestMethod]
        public void JeromeTest()
        {

            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/JeroemTest.html",
                this.TestContext);
            var model = new
            {
                repeat = true,
                Items = new[] {
                                new { Type = "Plane"},
                                new { Type = "Ship"},
                                new { Type = "Car"}
                            },
                DateTimeValue = new DateTime(2020, 10, 20)
            };

            using (var sr = new System.IO.StreamReader(path))
            {
                using (var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent))
                {
                    doc.Params["model"] = model;
                    using (var stream = DocStreams.GetOutputStream("JeroemTest2.pdf"))
                    {
                        doc.SaveAsPDF(stream);
                    }
                }
            }

            Assert.Inconclusive();
        }



        [TestMethod]
        public void HahyesTest()
        {
            
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/HahyesTest.html",
                this.TestContext);
            
            using (var sr = new System.IO.StreamReader(path))
            {
                var text = sr.ReadToEnd();

                using (var doc = HtmlParser.Parse(text, path, ParseSourceType.LocalFile))
                {
                    using (var stream = DocStreams.GetOutputStream("HahyesTest2.pdf"))
                    {
                        doc.SaveAsPDF(stream);
                    }
                }

            }

            Assert.Inconclusive();
        }

    }


    public static class HtmlParser
    {
        const string htmlXmlns = "http://www.w3.org/1999/xhtml";

        private static  System.Text.RegularExpressions.Regex restoreHtmlEntities =
            new System.Text.RegularExpressions.Regex("&amp;([^ ]{1,8});");

        public static  Document Parse(string html, string path, ParseSourceType type, bool forceXHTMLNamespace = true)
        {
            //Use agility pack to load the html

            HtmlAgilityPack.HtmlDocument adoc = new HtmlAgilityPack.HtmlDocument();
            adoc.LoadHtml(html);


            //make sure we have the namespace on the html node

            var node = adoc.DocumentNode.SelectSingleNode("//html");
            if (null == node)
            {
                throw new Scryber.PDFParserException("Cannot parse the html content as the root 'html' node could not be found");
            }
            else if (node.Attributes.Contains("xmlns") == false)
                node.Attributes.Add("xmlns", htmlXmlns);
            else
            {
                var xmlns = node.Attributes["xmlns"];
                if (xmlns.Value != htmlXmlns)
                {
                    if (forceXHTMLNamespace)
                        xmlns.Value = htmlXmlns;
                    else
                        throw new Scryber.PDFParserException("Cannot parse the html content as the namespace dose not match the xhtml namespace");
                }
            }

            //output the document as xhtml

            adoc.OptionOutputAsXml = true;
            adoc.OptionPreserveXmlNamespaces = true;
            adoc.OptionWriteEmptyNodes = true;
            adoc.OptionAutoCloseOnEnd = true;

            StringBuilder content = new StringBuilder();
            using (var sw = new StringWriter(content))
            {
                using (var xml = new System.Xml.XmlTextWriter(sw))
                    adoc.Save(xml);
            }

            var xhtml = content.ToString();


            //HtmlAgilityPack will convert html entities such as &nbsp;
            //to &amp;nbsp; so we need to convert back

            var restored = restoreHtmlEntities.Replace(xhtml, (input) => {
                //first is full string second should be the characters between the &amp; and trailing ;
                var val = "&" + input.Groups[1].Value + ";"; 
                return val;
            });

            //finally we can parse the xhtml

            using (var sr = new StringReader(restored))
            {
                var doc = Document.ParseDocument(sr, path, type);

                return doc;
            }

        }
    }
    
}
