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
                doc.Params["title"] = "Hello World";
                using (var stream = DocStreams.GetOutputStream("HelloWorld.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }
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
                bodyClass = "top"
            };

            using (var doc = Document.ParseDocument(path))
            {
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
            Assert.AreEqual("rgb (255,0,0)", bgColor.ToString()); //Red Background

            var color = pBlock.FullStyle.Fill.Color;
            Assert.AreEqual("rgb (255,255,255)", color);

            //Second page check

            
            body = _layoutcontext.DocumentLayout.AllPages[1];
            Assert.IsNotNull(body);

            pBlock = body.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            pLine = pBlock.Columns[0].Contents[0] as PDFLayoutLine;
            pRun = pLine.Runs[1] as PDFTextRunCharacter; // First is static text

            Assert.AreEqual("This is the content on the next page ", pRun.Characters);

            bgColor = pBlock.FullStyle.Background.Color;
            Assert.AreEqual("rgb (255,0,0)", bgColor.ToString()); //Red Background

            color = pBlock.FullStyle.Fill.Color;
            Assert.AreEqual("rgb (255,255,255)", color);

        }


        [TestMethod()]
        public void LocalAndRemoteImages()
        {
            var imagepath = "https://raw.githubusercontent.com/richard-scryber/scryber.core/master/docs/images/ScyberLogo2_alpha_small.png";
            var client = new System.Net.WebClient();
            var data = client.DownloadData(imagepath);

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
                all[i] = new { Name = "Name " + val.ToString(), Cost = "£" + val + ".00" };
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

                    /*
                    Assert.AreEqual(1, doc.Styles.Count);
                    Assert.IsInstanceOfType(doc.Styles[0], typeof(StyleGroup));
                    var grp = doc.Styles[0] as StyleGroup;

                    Assert.AreEqual(2, grp.Styles.Count);
                    Assert.IsInstanceOfType(grp.Styles[0], typeof(StyleFontFace));

                    ff = grp.Styles[0] as StyleFontFace;

                    Assert.AreEqual("Open Sans", ff.FontFamily.FamilyName);
                    Assert.IsFalse(ff.FontItalic);
                    Assert.IsTrue(ff.FontBold);
                    Assert.IsNotNull(ff.Source);

                    var sel = ff.Source;
                    Assert.AreEqual(FontSourceType.Local, sel.Type);
                    Assert.AreEqual("Open Sans", sel.Source);
                    Assert.AreEqual(FontSourceFormat.Default, sel.Format);

                    Assert.IsNotNull(sel.Next);
                    sel = sel.Next;

                    Assert.AreEqual(FontSourceType.Url, sel.Type);
                    Assert.AreEqual("https://fonts.gstatic.com/s/opensans/v18/mem8YaGs126MiZpBA-U1Ug.ttf", sel.Source);
                    Assert.AreEqual(FontSourceFormat.TrueType, sel.Format);


                    Assert.IsNotNull(sel.Next);
                    sel = sel.Next;

                    Assert.AreEqual(FontSourceType.Url, sel.Type);
                    Assert.AreEqual("https://github.com/google/fonts/blob/master/apache/opensans/OpenSans-Bold.woff", sel.Source);
                    Assert.AreEqual(FontSourceFormat.WOFF, sel.Format);

                    Assert.IsNull(sel.Next);
                    */
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



            

            using (var doc = Document.ParseDocument(path))
            {
                //pass paramters as needed, supporting simple values, arrays or complex classes.
                
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
    }
}
