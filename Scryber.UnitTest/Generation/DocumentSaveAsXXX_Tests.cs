using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Html.Components;
using Scryber.Html;
using System.Threading.Tasks;
using Scryber.PDF;
using Scryber.Drawing;
using Scryber.PDF.Resources;

namespace Scryber.Core.UnitTests.Generation
{
    [TestClass]
    public class DocumentSaveAs_Tests
    {
        PDFLayoutContext _layoutcontext;

        public DocumentSaveAs_Tests()
        {
        }

        private void DocumentParsing_Layout(object sender, LayoutEventArgs args)
        {
            _layoutcontext = (PDFLayoutContext)args.Context;
        }

        const string cssPath = "https://raw.githubusercontent.com/richard-scryber/scryber.core/master/Scryber.UnitTest/Content/HTML/CSS/Include.css";
        const string imgPath = "https://raw.githubusercontent.com/richard-scryber/scryber.core/master/Scryber.UnitTest/Content/HTML/Images/group.png";
        const string fontPath = "https://fonts.gstatic.com/s/poppins/v20/pxiDyp8kv8JHgFVrJJLm21llEA.ttf";

        const string docSrcBase = @"<?scryber append-log='true' ?>
                        <html xmlns='http://www.w3.org/1999/xhtml' >
                            <head>
                                <title>{{TITLE}}</title>
                                <link href='" + cssPath + @"' rel='stylesheet' />
                                <style>
                                    @font-face {
                                      font-family: 'Poppins';
                                      font-style: italic;
                                      font-weight: 300;
                                      src: url(" + fontPath + @") format('truetype');
                                    }

                                     .title {
                                      font-family: Poppins;
                                      font-weight: 300;
                                      color: silver;
                                     }
                                </style>
                            </head>

                            <body class='grey' style='margin:20px;' >
                                <h2 class='title' >The Title in Poppins</h2>
                                <p id='myPara' >This is a paragraph of content</p>
                                <img src='" + imgPath + @"' />
                            </body>

                        </html>";


        public string DocSource(string title)
        {
            var src = docSrcBase;
            src = src.Replace("{{TITLE}}", title);
            return src;
        }


        

        public void AssertDocumentLayout(string title)
        {
            Assert.IsNotNull(_layoutcontext);

            var layout = _layoutcontext.DocumentLayout;
            var doc = _layoutcontext.Document as Scryber.Components.Document;

            Assert.IsNotNull(layout);

            Assert.AreEqual(title, doc.Info.Title);
            

            //This has been loaded from the remote file
            var body = _layoutcontext.DocumentLayout.AllPages[0].PageBlock;
            Assert.AreEqual((Color)"#808080", body.FullStyle.Background.Color, "Fill colors do not match");


            Assert.AreEqual(3, doc.SharedResources.Count);

            PDFImageXObject img = null;
            PDFFontResource gill = null;
            PDFFontResource poppins = null;

            foreach(var rsrc in doc.SharedResources)
            {
                if (rsrc.ResourceType == PDFResource.XObjectResourceType)
                    img = (PDFImageXObject)rsrc;
                else if (rsrc.ResourceType == PDFResource.FontDefnResourceType)
                {
                    var font = (PDFFontResource)rsrc;
                    if (font.FontName == "Poppins,Italic")
                        poppins = font;
                    else if (font.FontName == "Gill Sans")
                        gill = font;
                    else
                        throw new System.Exception("Unknown font " + font.FontName);
                }
                else
                    throw new System.Exception("Unexpected resource " + rsrc.ResourceType);
            }

            

            Assert.IsNotNull(img);
            Assert.IsNotNull(gill);
            Assert.IsNotNull(poppins);
            Assert.AreEqual(img.ImageData.SourcePath, imgPath);
        }

        /// <summary>
        /// Executes the generation of a PDF from a template using the timers
        /// </summary>
        [TestMethod()]
        public void RemoteFileLoadingDirect()
        {

            string title = "Remote Direct";
            var src = DocSource(title);

            using (var sr = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent);
                doc.CacheProvider = new Scryber.Caching.PDFNoCachingProvider();
                doc.TraceLog.SetRecordLevel(TraceRecordLevel.Verbose);

                Assert.IsInstanceOfType(doc, typeof(HTMLDocument));

                using (var stream = DocStreams.GetOutputStream("RemoteFileLoadingDirect.pdf"))
                {
                    doc.LayoutComplete += DocumentParsing_Layout;
                    doc.SaveAsPDF(stream);

                }


                AssertDocumentLayout(title);

                
            }
        }

        /// <summary>
        /// Executes the generation of a PDF from a template using await tasks
        /// </summary>
        [TestMethod()]
        public void RemoteFileLoadingAsync()
        {
            string title = "Remote Async";
            var src = DocSource(title);
            using (var sr = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent);
                doc.CacheProvider = new Scryber.Caching.PDFNoCachingProvider();
                doc.TraceLog.SetRecordLevel(TraceRecordLevel.Verbose);

                Assert.IsInstanceOfType(doc, typeof(HTMLDocument));

                using (var stream = DocStreams.GetOutputStream("RemoteFileLoadingAsync.pdf"))
                {
                    doc.LayoutComplete += DocumentParsing_Layout;
                    Task.Run(async () =>
                    {
                        await doc.SaveAsPDFAsync(stream);
                    }).GetAwaiter().GetResult();

                }


                AssertDocumentLayout(title);
            }
        }

        /// <summary>
        /// Executes the generation of a PDF from a template using the timers
        /// </summary>
        [TestMethod()]
        public void RemoteFileLoadingTimer()
        {
            string title = "Remote Timed";
            var src = DocSource(title);

            using (var sr = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent);
                doc.CacheProvider = new Scryber.Caching.PDFNoCachingProvider();
                doc.TraceLog.SetRecordLevel(TraceRecordLevel.Verbose);

                Assert.IsInstanceOfType(doc, typeof(HTMLDocument));

                using (var stream = DocStreams.GetOutputStream("RemoteFileLoadingTimer.pdf"))
                {
                    doc.LayoutComplete += DocumentParsing_Layout;
                    Task.Run(async () =>
                    {
                        bool complete = false;

                        doc.SaveAsPDF(stream, (status) =>
                        {
                            if (status.Success)
                                complete = true;
                        });

                        int count = 0;
                        while (!complete)
                        {
                            await Task.Delay(500);
                            count++;

                            if (count > 20)
                                throw new System.Exception("Took far too long. Check network settings");
                        }

                    }).GetAwaiter().GetResult();

                }


                AssertDocumentLayout(title);

            }
        }

        private const string SampleHtml = "<html>\n\n<head>\n  <title>Hello world document\n  </title>\n  <link href=\"https://fonts.googleapis.com/css2?family=Comme:wght@100&display=swap\" rel=\"stylesheet\">\n  <style>\n    /* @font-face {\n      font-family: 'Poppins';\n      font-style: italic;\n      font-weight: 300;\n      src: url(https://fonts.gstatic.com/s/poppins/v20/pxiDyp8kv8JHgFVrJJLm21llEA.ttf) format('truetype');\n    } */\n\n    body {\n      padding: 20px;\n    }\n\n    h2,\n    p {\n      padding: 20px;\n    }\n\n    .title {\n      font-family: Helvetica;\n      font-weight: 300;\n      color: silver;\n      height: 300pt;\n      text-align: right;\n      vertical-align: bottom;\n      font-size: 60px;\n    }\n  </style>\n</head>\n\n<body>\n  <h2 class=\"title\" style=\"font-style:italic; padding: 30px;\">Only the Station Wagons.</h2>\n  <table style='width:100%; margin:40pt; font-size: 12pt'>\n    <tr>\n      <td>Color</td>\n      <td>Capacity</td>\n    </tr>\n    <template data-bind=\"{{selectWhere(items, .type == 'station wagon')}}\">\n      <tr>\n        <td>{{.color}}</td>\n        <td>{{.capacity}}</td>\n      </tr>\n    </template>\n  </table>\n</body>\n\n</html>";


        [TestMethod]
        public void HtmlLoadingDirect()
        {
            string title = "Remote Direct";
            var src = SampleHtml;

            using (var sr = new System.IO.StringReader(src))
            {
                var doc = Document.ParseHtmlDocument(sr, ParseSourceType.DynamicContent);
                doc.CacheProvider = new Scryber.Caching.PDFNoCachingProvider();
                doc.TraceLog.SetRecordLevel(TraceRecordLevel.Verbose);

                Assert.IsInstanceOfType(doc, typeof(HTMLDocument));

                using (var stream = DocStreams.GetOutputStream("HTMLFileLoadingDirect.pdf"))
                {
                    doc.LayoutComplete += DocumentParsing_Layout;
                    doc.SaveAsPDF(stream);

                }

            }
        }
    }
}
