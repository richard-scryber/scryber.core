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
using Scryber.PDF;


namespace Scryber.Core.UnitTests.Attachments
{
	[TestClass]
	public class Attachment_Tests
	{
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

        public Attachment_Tests()
		{
		}

        [TestMethod]
        public void TestIconAttachment()
        {
            var path = "../../Scryber.UnitTest/Content/Markdown/Markdown.md";
            path = DocStreams.AssertGetContentPath(path, TestContext);
            var doc = new Document();
            var pg = new Page();
            doc.Pages.Add(pg);
            doc.ConformanceMode = ParserConformanceMode.Strict;

            var attach = new IconAttachment();
            attach.DisplayIcon = AttachmentDisplayIcon.PushPin;
            attach.Source = path;
            attach.Width = 20;
            attach.Height = 20;
            attach.Padding = new Thickness(10);
            attach.ID = "markdown";

            pg.Contents.Add("Before attachment");
            pg.Contents.Add(attach);
            pg.Contents.Add("After attachment");
            
            

            pg.Contents.Add(new Div() {
                ID = "wrapper",
                Contents =
                {
                    new Link() { Destination = "#markdown", Action = LinkAction.Destination, Contents = {
                            new TextLiteral("Link To Markdown")
                        }
                    }

                }

            });

            using (var sr = DocStreams.GetOutputStream("IconAttachment.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                
                doc.SaveAsPDF(sr);
            }
        }
        
        [TestMethod]
        public void AttachmentInTemplateAttachment()
        {
            var path = "../../Scryber.UnitTest/Content/HTML/Attachments.html";
            path = DocStreams.AssertGetContentPath(path, TestContext);
            
            var doc = Document.ParseDocument(path);
            var pg = doc.Pages[0];
            pg.Style.OverlayGrid.ShowGrid = true;
            pg.Style.OverlayGrid.GridColor = StandardColors.Aqua;
            pg.Style.OverlayGrid.GridOpacity = 0.5;
            pg.Style.OverlayGrid.GridSpacing = 10;
            pg.Style.OverlayGrid.GridMajorCount = 5;

            using (var sr = DocStreams.GetOutputStream("AttachmentInTemplate.pdf"))
            {
                doc.SaveAsPDF(sr);
            }
        }
	}
}

