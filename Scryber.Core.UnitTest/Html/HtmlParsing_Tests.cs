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
            var src = @"<html xmlns='http://www.w3.org/1999/xhtml' >
                            <head>
                                <title>Html document title</title>
                                <style>
                                    .strong { font-weight: bold; color: #880088; }
                                    body.strong p { background-color: #F8F; }
                                </style>
                            </head>

                            <body class='strong' style='margin:20px;' >
                                <p id='myPara' style='border: solid 1px blue; padding: 5px;' >This is a &quot;paragraph&quot; of content</p>
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
                Assert.AreEqual(Scryber.Drawing.PDFColor.Parse("#880088"), body.FullStyle.Fill.Color, "Fill colors do not match");
                Assert.AreEqual(Scryber.Drawing.PDFColor.Parse("#F8F"), p.FullStyle.Background.Color, "Background color has not been applied");
                Assert.AreEqual(Scryber.Drawing.PDFColor.Parse("blue"), p.FullStyle.Border.Color, "Inline Border Color not correct");

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
            font-family: 'Zapf Dingbats';
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
            path = System.IO.Path.Combine(path, "../../../Content/sample.html");

            using (var doc = Document.ParseDocument(path))
            {
                using (var stream = DocStreams.GetOutputStream("HtmlFromSource.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }
            }

        }

        [TestMethod()]
        public void RemoteCssFileLoading()
        {
            var path = "https://raw.githubusercontent.com/richard-scryber/scryber.core/master/Scryber.Core.UnitTest/Content/Include.css";
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

                using (var stream = DocStreams.GetOutputStream("HtmlRemoteCSS.pdf"))
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
    }
}
