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

        public Attachment_Tests()
		{
		}

        [TestMethod]
        public void TestIconAttachment()
        {
            var doc = new Document();
            var pg = new Page();
            doc.Pages.Add(pg);
            doc.ConformanceMode = ParserConformanceMode.Strict;

            var attach = new IconAttachment();
            attach.DisplayIcon = AttachmentDisplayIcon.Paperclip;
            attach.Source = "../../../Content/Markdown/Markdown.md";
            attach.Width = 20;
            attach.Height = 20;
            pg.Contents.Add("Before attachment");
            pg.Contents.Add(attach);
            pg.Contents.Add("After attachment");
            using (var sr = DocStreams.GetOutputStream("IconAttachment.pdf"))
            {
                doc.SaveAsPDF(sr);
            }
        }
	}
}

