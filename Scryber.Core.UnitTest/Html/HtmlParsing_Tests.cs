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

using Scryber.Layout;
using System.Xml.Schema;
using System.Xml.Linq;
using System.IO;
using Scryber.Generation;
using Scryber.Svg.Components;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using Scryber.Resources;
using NuGet.Frameworks;
using Scryber.Expressive.Functions;
using Scryber.Expressive.Expressions;
using Scryber.Expressive.Exceptions;
using Scryber.Expressive;

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

        private void SimpleDocumentParsing_Layout(object sender, PDFLayoutEventArgs args)
        {
            _layoutcontext = args.Context;
        }

        [TestMethod]
        public void DataImageFactory_Test()
        {
            var html = @"<html xmlns='http://www.w3.org/1999/xhtml' ><body style='padding:20pt;' >
                    <img src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAUAAAAFCAYAAACNbyblAAAAHElEQVQI12P4//8/w38GIAXDIBKE0DHxgljNBAAO9TXL0Y4OHwAAAABJRU5ErkJggg==' alt='Red dot' />
                    </body></html>";

            using (var sr = new System.IO.StringReader(html))
            {
                using (var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent))
                {
                    using (var stream = DocStreams.GetOutputStream("DataImage.pdf"))
                    {
                        doc.SaveAsPDF(stream);


                        Assert.AreEqual(1, doc.SharedResources.Count);
                        var one = doc.SharedResources[0];
                        Assert.IsInstanceOfType(one, typeof(PDFImageXObject));
                    }

                }
            }
        }

        [TestMethod]
        public void DataImageAsBackgroundTest()
        {
            var html = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<head><style>
    .bgimg{
        background-image: url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAUAAAAFCAYAAACNbyblAAAAHElEQVQI12P4//8/w38GIAXDIBKE0DHxgljNBAAO9TXL0Y4OHwAAAABJRU5ErkJggg==');
        margin: 20pt;
    }
</style></head>
<body style='padding:20pt;' >
                    <img src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAUAAAAFCAYAAACNbyblAAAAHElEQVQI12P4//8/w38GIAXDIBKE0DHxgljNBAAO9TXL0Y4OHwAAAABJRU5ErkJggg==' alt='Red dot' />
    <p class='bgimg' style='width:140pt; height:100pt'>Content</p>
</body></html>";

            using (var sr = new System.IO.StringReader(html))
            {
                using (var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent))
                {
                    using (var stream = DocStreams.GetOutputStream("DataImageAsBackground.pdf"))
                    {
                        doc.SaveAsPDF(stream);

                        Assert.AreEqual(3, doc.SharedResources.Count); //Third is the font.

                        //As we have 2 data images we do not check for equality
                        var one = doc.SharedResources[0];
                        Assert.IsInstanceOfType(one, typeof(PDFImageXObject));

                        var two = doc.SharedResources[1];
                        Assert.IsInstanceOfType(one, typeof(PDFImageXObject));
                    }

                }
            }
        }

        [TestMethod]
        public void DataImageWithWhiteSpaceTest()
        {
            var html = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<head><style>
    .bgimg{
        background-image: url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAUAAAAFCAYAAACNbyblAAAAHElEQVQI12P
                                                     4//8/w38GIAXDIBKE0DHxgljNBAAO9TXL0Y4OHwAAAABJRU5ErkJggg==');
        margin: 20pt;
    }
</style></head>
<body style='padding:20pt;' >
                    <img src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAUAAAAF
                                    CAYAAACNbyblAAAAHElEQVQI12P4//8/w38GIAXDIBKE0DHx
                                    gljNBAAO9TXL0Y4OHwAAAABJRU5ErkJggg==' alt='Red dot' />
    <p class='bgimg' style='width:140pt; height:100pt'>Content</p>
</body></html>";

            using (var sr = new System.IO.StringReader(html))
            {
                using (var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent))
                {
                    using (var stream = DocStreams.GetOutputStream("DataImageWithWhitespace.pdf"))
                    {
                        doc.SaveAsPDF(stream);

                        Assert.AreEqual(3, doc.SharedResources.Count); //Third is the font.

                        //As we have 2 data images we do not check for equality
                        var one = doc.SharedResources[0];
                        Assert.IsInstanceOfType(one, typeof(PDFImageXObject));

                        var two = doc.SharedResources[1];
                        Assert.IsInstanceOfType(one, typeof(PDFImageXObject));
                    }

                }
            }
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
                    doc.LayoutComplete += SimpleDocumentParsing_Layout;
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
            font-family: 'Segoe UI';
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
                    doc.LayoutComplete += SimpleDocumentParsing_Layout;
                    doc.SaveAsPDF(stream);
                }

                var page = doc.Pages[0] as Page;
                var div = page.Contents[0] as Div;
                var h1 = div.Contents[1] as HTMLHead1;
                Assert.IsNotNull(h1, "No heading found");
                Assert.AreEqual(1, h1.Contents.Count);
                var content = h1.Contents[0];
            }
        }


        [TestMethod()]
        public void LoadHtmlFromSource()
        {
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/HtmlFromSource.html");

            using (var doc = Document.ParseDocument(path))
            {
                var defn = new StyleDefn("h1.border");
                defn.Background.Color = (PDFColor)"#FFA";
                defn.Border.Width = 2;
                defn.Border.Color = PDFColors.Red;
                defn.Border.LineStyle = LineType.Solid;

                doc.Styles.Add(defn);
                
                    
                
                using (var stream = DocStreams.GetOutputStream("HtmlFromSource.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }
            }

        }

        [TestMethod()]
        public void HelloWorld()
        {
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/HelloWorld.html");

            using (var doc = Document.ParseDocument(path))
            {
                
                using (var stream = DocStreams.GetOutputStream("HelloWorld.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }
            }
        }

        [TestMethod()]
        public void MultipleImageReferences()
        {
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/MultipleImageReferences.html");

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
            var path = "https://raw.githubusercontent.com/richard-scryber/scryber.core/master/Scryber.Core.UnitTest/Content/HTML/CSS/Include.css";
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
                    doc.LayoutComplete += SimpleDocumentParsing_Layout;
                    doc.SaveAsPDF(stream);
                }


                var body = _layoutcontext.DocumentLayout.AllPages[0].ContentBlock;
                
                Assert.AreEqual("Html document title", doc.Info.Title, "Title is not correct");

                //This has been loaded from the remote file
                Assert.AreEqual((PDFColor)"#808080", body.FullStyle.Background.Color, "Fill colors do not match");
                

            }
        }

        [TestMethod()]
        public void RemoteCssFileLoadingAsync()
        {
            var path = "https://raw.githubusercontent.com/richard-scryber/scryber.core/master/Scryber.Core.UnitTest/Content/HTML/CSS/Include.css";
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
                    doc.LayoutComplete += SimpleDocumentParsing_Layout;
                    Task.Run(async () =>
                    {
                        await doc.SaveAsPDFAsync(stream);
                    }).GetAwaiter().GetResult();
                    
                }


                var body = _layoutcontext.DocumentLayout.AllPages[0].ContentBlock;

                Assert.AreEqual("Html document title", doc.Info.Title, "Title is not correct");

                //This has been loaded from the remote file
                Assert.AreEqual((PDFColor)"#808080", body.FullStyle.Background.Color, "Fill colors do not match");


            }
        }


        [TestMethod()]
        public void BodyAsASection()
        {
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/bodyheadfoot.html");

            using (var doc = Document.ParseDocument(path))
            {
                using (var stream = DocStreams.GetOutputStream("bodyheadfoot.pdf"))
                {
                    doc.LayoutComplete += SimpleDocumentParsing_Layout;
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
        public void BodyWithBinding()
        {
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/bodyWithBinding.html");

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
                    doc.LayoutComplete += SimpleDocumentParsing_Layout;
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

            Assert.AreEqual(pRun.Characters, model.content);

            var bgColor = pBlock.FullStyle.Background.Color;
            Assert.AreEqual("rgb(255,0,0)", bgColor.ToString()); //Red Background

            var color = pBlock.FullStyle.Fill.Color;
            Assert.AreEqual("rgb(255,255,255)", color);

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
            Assert.AreEqual("£10.10", pnum.Characters);

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
            Assert.AreEqual("rgb(255,255,255)", color);

            // Performance check
            using (var doc = Document.ParseDocument(path))
            {
                
                using (var stream = DocStreams.GetOutputStream("bodyWithBinding.pdf"))
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
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/bodyWithBinding.html");
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
                    doc.LayoutComplete += SimpleDocumentParsing_Layout;
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
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/bodyWithExpressionBinding.html");

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
                    doc.LayoutComplete += SimpleDocumentParsing_Layout;
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
            Assert.AreEqual("rgb(255,255,255)", color);

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
            Assert.AreEqual("£10.10", pnum.Characters);

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
            Assert.AreEqual("rgb(255,255,255)", color);
        }



        [TestMethod()]
        public void LocalAndRemoteImages()
        {
            var imagepath = "https://raw.githubusercontent.com/richard-scryber/scryber.core/master/docs/images/ScyberLogo2_alpha_small.png";
            var client = new System.Net.Http.HttpClient();
            var data = client.GetByteArrayAsync(imagepath).Result;

            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/LocalAndRemoteImages.html");

            Assert.IsTrue(System.IO.File.Exists(path));

            using (var doc = Document.ParseDocument(path))
            {
                using (var stream = DocStreams.GetOutputStream("LocalAndRemoteImages.pdf"))
                {
                    doc.LayoutComplete += SimpleDocumentParsing_Layout;
                    doc.SaveAsPDF(stream);

                }

                Assert.AreEqual(3, doc.SharedResources.Count, "Not all images were loaded");

                var zero = doc.SharedResources[0] as PDFFontResource;
                Assert.IsNotNull(zero);

                var one = doc.SharedResources[1] as PDFImageXObject; //Should be the local image;
                Assert.IsNotNull(one, "Not an image for the second resouce");
                Assert.IsTrue(one.Registered);
                Assert.IsTrue(one.Source.EndsWith("/HTML/images/ScyberLogo2_alpha_small.png"), "Source was not correct for the image");

                var two = doc.SharedResources[2] as PDFImageXObject;
                Assert.IsNotNull(two, "Not an image for the third resource");
                Assert.IsTrue(two.Source.EndsWith("docs/images/ScyberLogo2_alpha_small.png"), "Source was not correct for the second image");


            }

        }

        [TestMethod()]
        public void LocalAndRemoteImagesAsync()
        {
            var imagepath = "https://raw.githubusercontent.com/richard-scryber/scryber.core/master/docs/images/ScyberLogo2_alpha_small.png";
            var client = new System.Net.Http.HttpClient();
            var data = client.GetByteArrayAsync(imagepath).Result;

            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/LocalAndRemoteImages.html");

            Assert.IsTrue(System.IO.File.Exists(path));

            using (var doc = Document.ParseDocument(path))
            {
                using (var stream = DocStreams.GetOutputStream("LocalAndRemoteImagesAsync.pdf"))
                {
                    doc.LayoutComplete += SimpleDocumentParsing_Layout;
                    Task.Run(async () =>
                    {
                        await doc.SaveAsPDFAsync(stream);
                    }).GetAwaiter().GetResult();

                }

                Assert.AreEqual(3, doc.SharedResources.Count, "Not all images were loaded");

                var zero = doc.SharedResources[0] as PDFFontResource;
                Assert.IsNotNull(zero);

                var one = doc.SharedResources[1] as PDFImageXObject; //Should be the local image;
                Assert.IsNotNull(one, "Not an image for the second resouce");
                Assert.IsTrue(one.Registered);
                Assert.IsTrue(one.Source.EndsWith("/HTML/images/ScyberLogo2_alpha_small.png"), "Source was not correct for the image");

                var two = doc.SharedResources[2] as PDFImageXObject;
                Assert.IsNotNull(two,"Not an image for the third resource");
                Assert.IsTrue(two.Source.EndsWith("docs/images/ScyberLogo2_alpha_small.png"), "Source was not correct for the second image");




            }

        }

        [TestMethod()]
        public void BodyTemplating()
        {
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/bodytemplating.html");

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
                    doc.LayoutComplete += SimpleDocumentParsing_Layout;
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
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/bodytemplating.html");

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
                    doc.LayoutComplete += SimpleDocumentParsing_Layout;
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


            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/displaynone.html");

            using (var doc = Document.ParseDocument(path))
            {
                using (var stream = DocStreams.GetOutputStream("htmlDisplayNone.pdf"))
                {
                    doc.Params["model"] = model;
                    doc.LayoutComplete += SimpleDocumentParsing_Layout;
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
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/topandtailed.html");

            using (var doc = Document.ParseDocument(path))
            {
                doc.Params["title"] = "Title in code";

                using (var stream = DocStreams.GetOutputStream("topandtailed.pdf"))
                {
                    doc.LayoutComplete += SimpleDocumentParsing_Layout;
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
        public void BordersAndSides()
        {
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/BorderSides.html");

            using (var doc = Document.ParseDocument(path))
            {
                using (var stream = DocStreams.GetOutputStream("BorderSides.pdf"))
                {
                    doc.LayoutComplete += SimpleDocumentParsing_Layout;
                    doc.SaveAsPDF(stream);
                }
            }
        }

        [TestMethod()]
        public void HtmlIFrameFragments()
        {
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/BodyFraming.html");

            using (var doc = Document.ParseDocument(path))
            {
                var model = new
                {
                    fragmentContent = "Content for the fragment"
                };
                doc.Params["model"] = model;

                using (var stream = DocStreams.GetOutputStream("BodyFraming.pdf"))
                {
                    doc.LayoutComplete += SimpleDocumentParsing_Layout;
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

        [TestMethod()]
        public void HtmlLinksLocalAndRemote()
        {
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/LinksLocalAndRemote.html");

            using (var doc = Document.ParseDocument(path))
            {
                var model = new
                {
                    fragmentContent = "Content for the fragment"
                };
                doc.Params["model"] = model;

                using (var stream = DocStreams.GetOutputStream("LinksLocalAndRemote.pdf"))
                {
                    doc.LayoutComplete += SimpleDocumentParsing_Layout;
                    doc.SaveAsPDF(stream);

                }
                
            }

        }


        [TestMethod()]
        public void BodyWithPageNumbers()
        {
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/bodyWithPageNums.html");

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
                    doc.LayoutComplete += SimpleDocumentParsing_Layout;
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
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/Html5AllTags.html");

            using (var doc = Document.ParseDocument(path))
            {
                var model = new
                {
                    fragmentContent = "Content for the fragment"
                };
                doc.Params["model"] = model;

                using (var stream = DocStreams.GetOutputStream("Html5AllTags.pdf"))
                {
                    doc.LayoutComplete += SimpleDocumentParsing_Layout;
                    doc.SaveAsPDF(stream);

                }

            }

        }

        [TestMethod()]
        public void FontFace()
        {
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/FontFace.html");
            StyleFontFace ff;

            using (var doc = Document.ParseDocument(path))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;

                var model = new
                {
                    fragmentContent = "Content for the fragment"
                };
                doc.Params["model"] = model;

                using (var stream = DocStreams.GetOutputStream("FontFace.pdf"))
                {
                    doc.LayoutComplete += SimpleDocumentParsing_Layout;
                    doc.SaveAsPDF(stream);

                }

            }

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
        public void READMESample()
        {
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/READMESample.html");

            //pass paramters as needed, supporting arrays or complex classes.
            var items = new[]
            {
                new { name = "First item" },
                new { name = "Second item" },
                new { name = "Third item" },
            };

            var model = new{
                titlestyle = "color:#ff6347",
                title = "Hello from scryber",
                items = items
            };

            using (var doc = Document.ParseDocument(path))
            {
                //pass paramters as needed, supporting simple values, arrays or complex classes.
                doc.Params["author"] = "Scryber Engine";
                doc.Params["model"] = model;
                using (var stream = DocStreams.GetOutputStream("READMESample.pdf"))
                {
                    
                    doc.SaveAsPDF(stream);
                }

            }

            using (var doc = Document.ParseDocument(path))
            {
                //pass paramters as needed, supporting simple values, arrays or complex classes.
                doc.Params["author"] = "Scryber Engine";
                doc.Params["model"] = model;
                using (var stream = DocStreams.GetOutputStream("READMESample2.pdf"))
                {

                    doc.SaveAsPDF(stream);
                }

            }
        }

        [TestMethod()]
        public void DocumentationOutput()
        {
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/documentation.html");
            Document doc;

            using (doc = Document.ParseDocument(path))
            {
                using (var stream = DocStreams.GetOutputStream("documentation.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }
            }

            
        }

        

        [TestMethod()]
        public void BodyWithLongContent()
        {
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/bodyWithLongContent.html");



            using (var doc = Document.ParseDocument(path))
            {
                //pass paramters as needed, supporting simple values, arrays or complex classes.

                using (var stream = DocStreams.GetOutputStream("bodyWithLongContent.pdf"))
                {

                    doc.SaveAsPDF(stream);
                }

            }
        }

        [TestMethod()]
        public void BodyWithMultipleColumns()
        {
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/bodyWithMultipleColumns.html");



            using (var doc = Document.ParseDocument(path))
            {
                //pass paramters as needed, supporting simple values, arrays or complex classes.

                using (var stream = DocStreams.GetOutputStream("bodyWithMultipleColumns.pdf"))
                {

                    doc.SaveAsPDF(stream);
                }

            }
        }

        [TestMethod()]
        public void AbsolutelyPositioned()
        {
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/HtmlAbsolutePositioned.html");

            

            using (var doc = Document.ParseDocument(path))
            {
                //pass paramters as needed, supporting simple values, arrays or complex classes.

                using (var stream = DocStreams.GetOutputStream("HtmlAbsolutePositioned.pdf"))
                {

                    doc.SaveAsPDF(stream);
                }

            }
        }

        [TestMethod()]
        public void FloatLeft()
        {
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/FloatLeft.html");

            using (var doc = Document.ParseDocument(path))
            {
                var style = doc.Pages[0].Style;
                //style.OverlayGrid.ShowGrid = true;
                style.OverlayGrid.GridSpacing = 10;

                using (var stream = DocStreams.GetOutputStream("FloatLeft.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }
            }
        }

        [TestMethod()]
        public void FloatRight()
        {
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/FloatRight.html");

            using (var doc = Document.ParseDocument(path))
            {
                var style = doc.Pages[0].Style;
                //style.OverlayGrid.ShowGrid = true;
                style.OverlayGrid.GridSpacing = 10;

                using (var stream = DocStreams.GetOutputStream("FloatRight.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }
            }

        }

        [TestMethod()]
        public void FloatMixed()
        {
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/FloatMixed.html");

            using (var doc = Document.ParseDocument(path))
            {
                doc.ConformanceMode = ParserConformanceMode.Strict;
                //pass paramters as needed, supporting simple values, arrays or complex classes.
                var style = doc.Pages[0].Style;
                //style.OverlayGrid.ShowGrid = true;
                style.OverlayGrid.GridSpacing = 10;

                using (var stream = DocStreams.GetOutputStream("FloatMixed.pdf"))
                {

                    doc.SaveAsPDF(stream);
                }

            }
        }

        [TestMethod()]
        public void RestrictedHtml()
        {
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/RestrictedHtml.html");

            using (var doc = Document.ParseDocument(path))
            {
                doc.PasswordProvider = new Scryber.Secure.DocumentPasswordProvider("Password");
                doc.Params["title"] = "Hello World";

                using (var stream = DocStreams.GetOutputStream("RestrictedHtml.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

            }
        }

        [TestMethod()]
        public void RestrictedWithoutPasswordHtml()
        {
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/RestrictedHtml.html");

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
        }

        [TestMethod()]
        public void RestrictedProtectedHtml()
        {
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/RestrictedHtml.html");

            using (var doc = Document.ParseDocument(path))
            {
                doc.PasswordProvider = new Scryber.Secure.DocumentPasswordProvider("Password", "Password");
                doc.Params["title"] = "Hello World";
                using (var stream = DocStreams.GetOutputStream("ProtectedHtml.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

            }
        }


        [TestMethod()]
        public void BasePath()
        {
            //var path = "https://raw.githubusercontent.com/richard-scryber/scryber.core/master/Scryber.Core.UnitTest/Content/HTML/Images/Toroid24.png";

            var src = @"<html xmlns='http://www.w3.org/1999/xhtml' >
                            <head>
                                <title>Html document title</title>
                                <base href='https://raw.githubusercontent.com/richard-scryber/scryber.core/master/Scryber.Core.UnitTest/Content/HTML/' />
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
                Assert.AreEqual("https://raw.githubusercontent.com/richard-scryber/scryber.core/master/Scryber.Core.UnitTest/Content/HTML/", doc.LoadedSource, "Loaded Source is not correct");
                
                using (var stream = DocStreams.GetOutputStream("DynamicBasePath.pdf"))
                {
                    doc.LayoutComplete += SimpleDocumentParsing_Layout;
                    doc.SaveAsPDF(stream);
                }
                Assert.AreEqual(1, doc.Styles.Count, "Remote styles were not loaded");
                Assert.IsInstanceOfType(doc.Styles[0], typeof(StyleGroup), "The remote styles is not a group");

                var img = doc.FindAComponentById("myToroid") as Image;
                Assert.IsNotNull(img.XObject,"The image was not loaded from the remote source");

                var embed = doc.FindAComponentById("myDrawing") as SVGCanvas;
                Assert.IsNotNull(embed);
                Assert.AreNotEqual(0, embed.Contents.Count, "SVG drawing was not loaded from the source");
                
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

                Assert.AreEqual("Hello World", doc.Info.Title);
            }
        }

        private StringReader LoadTermsStream()
        {
            return new StringReader("<p xmlns='http://www.w3.org/1999/xhtml'>These are my terms</p>");
        }

        private IPDFComponent CustomResolve(string filepath, string xpath, PDFGeneratorSettings settings)
        {
            if (filepath == "MyTsAndCs")
            {
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

            using (var reader = new StringReader(src))
            {
                var comp = Document.Parse(string.Empty, reader, ParseSourceType.DynamicContent, CustomResolve);

                Assert.IsNotNull(comp);
                Assert.IsInstanceOfType(comp, typeof(HTMLDocument));
                var doc = comp as HTMLDocument;
                Assert.AreEqual("Hello World", doc.Info.Title);

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

            var div = new Div() { Padding = new PDFThickness(10) };
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
            var doc = Document.ParseDocument("../../../Content/HTML/OrderItems.html");
            var service = new OrderMockService();
            var user = new User() { Salutation = "Mr", FirstName = "Richard", LastName = "Smith" };
            var order = service.GetOrder(1);
            order.PaymentTerms = 0;

            doc.Params["model"] = new
            {
                user = user,
                order = order
            };

            var grid = doc.FindAComponentById("orders") as TableGrid;
            //Properties directly on the visual component.
            grid.BackgroundColor = "#EEE";
            //Using the style property
            grid.Style.Margins.Right = 20;
            grid.Style.Margins.Left = 20;

            //Using style keys
            var pay = doc.FindAComponentById("payNow") as Div;
            pay.Style.SetValue(StyleKeys.BorderStyleKey, LineType.Dash);
            pay.Style.SetValue(StyleKeys.BorderDashKey, PDFDashes.LongDash);

            //A new style to the document
            StyleDefn style = new StyleDefn("#terms div#payNow");
            style.Border.Width = 2;
            doc.Styles.Add(style);

            using (var stream = DocStreams.GetOutputStream("OrderItemsTemplate.pdf"))
                doc.SaveAsPDF(stream);
        }

        private class ToUpperFunction : IFunction
        {
            public string Name { get { return "ToUpper"; } }

            public object Evaluate(IExpression[] param, IDictionary<string, object> vars, Expressive.Context context)
            {
                if (null == param && param.Length < 1)
                    throw new ExpressiveException("Invalid arguments for the ToUpper expression");

                object one = param[0].Evaluate(vars);

                if (null == one)
                    return null;
                else
                    return one.ToString().ToUpper();

            }
        }

        static HtmlParsing_Test()
        {
            //We do this early so we can make sure it is in the collection.
            Scryber.Binding.BindingCalcExpressionFactory.RegisterFunction(new ToUpperFunction());
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
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/InvalidBackgroundImage.html");
            bool error = false;
            try
            {
                using (var doc = Document.ParseDocument(path))
                {
                    doc.ConformanceMode = ParserConformanceMode.Strict;
                    using (var stream = DocStreams.GetOutputStream("InvalidBackgroundImage.pdf"))
                    {
                        doc.LayoutComplete += SimpleDocumentParsing_Layout;
                        doc.SaveAsPDF(stream);
                    }
                }

            }
            catch(Exception ex)
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

                        doc.LayoutComplete += SimpleDocumentParsing_Layout;
                        doc.SaveAsPDF(stream);
                    }
                }
                error = false;

            }
            catch (Exception ex)
            {
                error = true;
            }

            Assert.IsFalse(error, "An error was raised when a background image was not available in Lax mode");
        }

        [TestMethod]
        public void LargeFileTest()
        {

            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/LargeFile.html");

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
        }

        private PDFLayoutDocument _layout;

        [TestMethod]
        public void LinearGradientTest()
        {
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/LinearGradients.html");

            using (var sr = new System.IO.StreamReader(path))
            {
                using (var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent))
                {
                    using (var stream = DocStreams.GetOutputStream("LinearGradient.pdf"))
                    {
                        doc.LayoutComplete += Gradient_LayoutComplete;
                        doc.SaveAsPDF(stream);
                    }

                    var rg = new PDFColor[] { PDFColors.Red, PDFColors.Green };
                    var rgby = new PDFColor[] { PDFColors.Red, PDFColors.Green, PDFColors.Blue, PDFColors.Yellow };
                    var ryg = new PDFColor[] { PDFColors.Red, PDFColors.Yellow, PDFColors.Green };

                    Assert.IsNotNull(_layout);
                    var pg = _layout.AllPages[0];

                    var resources = pg.Resources;
                    Assert.AreEqual(2, resources.Types.Count);

                    var patterns = resources.Types["Pattern"];
                    Assert.IsNotNull(patterns);
                    Assert.AreEqual(9, patterns.Count);

                    
                    ValidateLinearGradient(patterns[0] as PDFLinearShadingPattern, rg, (double)GradientAngle.Bottom);
                    ValidateLinearGradient(patterns[1] as PDFLinearShadingPattern, rg, (double)GradientAngle.Bottom_Left);
                    ValidateLinearGradient(patterns[2] as PDFLinearShadingPattern, rg, (double)GradientAngle.Left);
                    ValidateLinearGradient(patterns[3] as PDFLinearShadingPattern, rg, (double)GradientAngle.Top_Left);
                    ValidateLinearGradient(patterns[4] as PDFLinearShadingPattern, rg, (double)GradientAngle.Top);
                    ValidateLinearGradient(patterns[5] as PDFLinearShadingPattern, rg, (double)GradientAngle.Top_Right);
                    ValidateLinearGradient(patterns[6] as PDFLinearShadingPattern, rg, (double)GradientAngle.Right);
                    ValidateLinearGradient(patterns[7] as PDFLinearShadingPattern, rgby, (double)GradientAngle.Bottom_Right);
                    ValidateLinearGradient(patterns[8] as PDFLinearShadingPattern, ryg, (double)GradientAngle.Bottom, true);
                }
            }

            
        }

        private static void ValidateLinearGradient(PDFLinearShadingPattern one, PDFColor[] cols, double angle, bool repeating = false)
        {
            Assert.IsTrue(one.PatternType == PatternType.ShadingPattern);
            Assert.IsTrue(one.Registered);
            Assert.IsNotNull(one.Descriptor);
            Assert.AreEqual(cols.Length, one.Descriptor.Colors.Count);
            Assert.AreEqual(angle, one.Descriptor.Angle);
            Assert.AreEqual(repeating, one.Descriptor.Repeating, "Repeating flag does not match");
            Assert.AreEqual(GradientType.Linear, one.Descriptor.GradientType);
            for (int i = 0; i < cols.Length; i++)
            {
                Assert.AreEqual(cols[i], one.Descriptor.Colors[i].Color, "Color '" + i + "' does not match '" + cols[i].ToString() + "'");
            }
        }

        private void Gradient_LayoutComplete(object sender, PDFLayoutEventArgs args)
        {
            _layout = args.Context.DocumentLayout;
        }



        [TestMethod]
        public void RadialGradientTest()
        {
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/RadialGradients.html");

            using (var sr = new System.IO.StreamReader(path))
            {
                using (var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent))
                {
                    using (var stream = DocStreams.GetOutputStream("RadialGradient.pdf"))
                    {
                        doc.LayoutComplete += Gradient_LayoutComplete;
                        doc.SaveAsPDF(stream);
                    }

                    
                    var rg = new PDFColor[] { PDFColors.Red, PDFColors.Green };
                    var rgby = new PDFColor[] { PDFColors.Red, PDFColors.Green, PDFColors.Blue, PDFColors.Yellow };
                    var ryg = new PDFColor[] { PDFColors.Red, PDFColors.Yellow, PDFColors.Green };

                    Assert.IsNotNull(_layout);
                    var pg = _layout.AllPages[0];

                    //var resources = pg.Resources;
                    //Assert.AreEqual(2, resources.Types.Count);

                    //var patterns = resources.Types["Pattern"];
                    //Assert.IsNotNull(patterns);
                    //Assert.AreEqual(9, patterns.Count);

                }
            }


        }


        [TestMethod]
        public void JeromeTest()
        {
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/JeroemTest.html");
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


        }
        
    }
}
