using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Html.Components;
using Scryber.Html;
using System.Threading.Tasks;
using Scryber.PDF;
using Scryber.Drawing;

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

        /// <summary>
        /// Executes the generation of a PDF from a template using the timers
        /// </summary>
        [TestMethod()]
        public void RemoteFileLoadingDirect()
        {
            var cssPath = "https://raw.githubusercontent.com/richard-scryber/scryber.core/master/Scryber.UnitTest/Content/HTML/CSS/Include.css";
            var imgPath = "https://raw.githubusercontent.com/richard-scryber/scryber.core/master/Scryber.UnitTest/Content/HTML/Images/group.png";

            var src = @"<?scryber append-log='true' ?>
<html xmlns='http://www.w3.org/1999/xhtml' >
                            <head>
                                <title>Html document title</title>
                                <link href='" + cssPath + @"' rel='stylesheet' />
                            </head>

                            <body class='grey' style='margin:20px;' >
                                <p id='myPara' >This is a paragraph of content</p>
                                <img src='" + imgPath + @"' />
                            </body>

                        </html>";

            using (var sr = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent);
                Assert.IsInstanceOfType(doc, typeof(HTMLDocument));

                using (var stream = DocStreams.GetOutputStream("RemoteFileLoadingDirect.pdf"))
                {
                    doc.LayoutComplete += DocumentParsing_Layout;
                    doc.SaveAsPDF(stream);

                }


                var body = _layoutcontext.DocumentLayout.AllPages[0].ContentBlock;

                Assert.AreEqual("Html document title", doc.Info.Title, "Title is not correct");

                //This has been loaded from the remote file
                Assert.AreEqual((Color)"#808080", body.FullStyle.Background.Color, "Fill colors do not match");

                var rsrc = doc.SharedResources;
                Assert.AreEqual(2, rsrc.Count);

                var img = (Scryber.PDF.Resources.PDFImageXObject)rsrc[0];

                Assert.IsNotNull(img);
                Assert.AreEqual(img.ImageData.SourcePath, imgPath);
            }
        }

        /// <summary>
        /// Executes the generation of a PDF from a template using await tasks
        /// </summary>
        [TestMethod()]
        public void RemoteFileLoadingAsync()
        {
            var cssPath = "https://raw.githubusercontent.com/richard-scryber/scryber.core/master/Scryber.UnitTest/Content/HTML/CSS/Include.css";
            var imgPath = "https://raw.githubusercontent.com/richard-scryber/scryber.core/master/Scryber.UnitTest/Content/HTML/Images/group.png";

            var src = @"<?scryber append-log='true' ?>
<html xmlns='http://www.w3.org/1999/xhtml' >
                            <head>
                                <title>Html document title</title>
                                <link href='" + cssPath + @"' rel='stylesheet' />
                            </head>

                            <body class='grey' style='margin:20px;' >
                                <p id='myPara' >This is a paragraph of content</p>
                                <img src='" + imgPath + @"' />
                            </body>

                        </html>";

            using (var sr = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent);
                Assert.IsInstanceOfType(doc, typeof(HTMLDocument));

                using (var stream = DocStreams.GetOutputStream("RemoteFileLoadingAsync.pdf"))
                {
                    doc.LayoutComplete += DocumentParsing_Layout;
                    Task.Run(async () =>
                    {
                        await doc.SaveAsPDFAsync(stream);
                    }).GetAwaiter().GetResult();

                }


                var body = _layoutcontext.DocumentLayout.AllPages[0].ContentBlock;

                Assert.AreEqual("Html document title", doc.Info.Title, "Title is not correct");

                //This has been loaded from the remote file
                Assert.AreEqual((Color)"#808080", body.FullStyle.Background.Color, "Fill colors do not match");

                var rsrc = doc.SharedResources;
                Assert.AreEqual(2, rsrc.Count);

                var img = (Scryber.PDF.Resources.PDFImageXObject)rsrc[0];

                Assert.IsNotNull(img);
                Assert.AreEqual(img.ImageData.SourcePath, imgPath);
            }
        }

        /// <summary>
        /// Executes the generation of a PDF from a template using the timers
        /// </summary>
        [TestMethod()]
        public void RemoteFileLoadingTimer()
        {
            var cssPath = "https://raw.githubusercontent.com/richard-scryber/scryber.core/master/Scryber.UnitTest/Content/HTML/CSS/Include.css";
            var imgPath = "https://raw.githubusercontent.com/richard-scryber/scryber.core/master/Scryber.UnitTest/Content/HTML/Images/group.png";

            var src = @"<?scryber append-log='true' ?>
<html xmlns='http://www.w3.org/1999/xhtml' >
                            <head>
                                <title>Html document title</title>
                                <link href='" + cssPath + @"' rel='stylesheet' />
                            </head>

                            <body class='grey' style='margin:20px;' >
                                <p id='myPara' >This is a paragraph of content</p>
                                <img src='" + imgPath + @"' />
                            </body>

                        </html>";

            using (var sr = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent);
                Assert.IsInstanceOfType(doc, typeof(HTMLDocument));

                using (var stream = DocStreams.GetOutputStream("RemoteFileLoadingTimer.pdf"))
                {
                    doc.LayoutComplete += DocumentParsing_Layout;
                    Task.Run(async () =>
                    {
                        bool complete = false;

                        doc.SaveAsPDFTimer(stream, (status) =>
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


                var body = _layoutcontext.DocumentLayout.AllPages[0].ContentBlock;

                Assert.AreEqual("Html document title", doc.Info.Title, "Title is not correct");

                //This has been loaded from the remote file
                Assert.AreEqual((Color)"#808080", body.FullStyle.Background.Color, "Fill colors do not match");

                var rsrc = doc.SharedResources;
                Assert.AreEqual(2, rsrc.Count);

                var img = (Scryber.PDF.Resources.PDFImageXObject)rsrc[0];

                Assert.IsNotNull(img);
                Assert.AreEqual(img.ImageData.SourcePath, imgPath);
            }
        }
    }
}
