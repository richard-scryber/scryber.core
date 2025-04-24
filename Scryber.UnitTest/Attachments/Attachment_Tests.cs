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

        private PDF.Layout.PDFLayoutDocument _layout;
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

            using (var sr = DocStreams.GetOutputStream("TestIconAttachment.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.LayoutComplete += DocOnLayoutComplete;
                doc.SaveAsPDF(sr);
            }
            
            
            
        }

        private void DocOnLayoutComplete(object sender, LayoutEventArgs args)
        {
            this._layout = (args.Context as PDFLayoutContext).DocumentLayout;
        }

        [TestMethod]
        public void AttachmentInTemplateAttachment()
        {
            var path = "../../Scryber.UnitTest/Content/HTML/AttachmentsWithIcon.html";
            path = DocStreams.AssertGetContentPath(path, TestContext);
            
            var doc = Document.ParseDocument(path);
            var pg = doc.Pages[0];
            pg.Style.OverlayGrid.ShowGrid = true;
            pg.Style.OverlayGrid.GridColor = StandardColors.Aqua;
            pg.Style.OverlayGrid.GridOpacity = 0.5;
            pg.Style.OverlayGrid.GridSpacing = 10;
            pg.Style.OverlayGrid.GridMajorCount = 5;

            using (var sr = DocStreams.GetOutputStream("AttachmentWithIcon.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.LayoutComplete += DocOnLayoutComplete;
                doc.SaveAsPDF(sr);
            }
            
            Assert.IsNotNull(this._layout);
            Assert.AreEqual(3, this._layout.TotalPageCount);
            var artefacts = _layout.Artefacts;
            
            //Embedded File reference
            
            Assert.AreEqual(3, artefacts.Count);
            var found = artefacts.TryGetCollection("EmbeddedFiles", out var embedded);
            Assert.IsTrue(found);
            Assert.IsNotNull(embedded);
            PDFEmbeddedAttachmentDictionary dictionary = embedded as PDFEmbeddedAttachmentDictionary;
            Assert.IsNotNull(dictionary);
            Assert.AreEqual(1, dictionary.Count);
            
            var one = dictionary.First();
            
            Assert.IsNotNull(one);
            Assert.AreEqual("hBdy1_landscapeAttachment", one.Key);
            var embed = one.Value;
            Assert.IsNotNull(embed);
            Assert.IsNotNull(embed.FileData);
            Assert.IsTrue(embed.FullFilePath.EndsWith("landscape.jpg"));
            Assert.AreEqual("./Images/landscape.jpg", embed.Description);
            
            //Page 1

            var lpg = this._layout.AllPages[0];
            Assert.IsNotNull(lpg);

            var pgArtefacts = lpg.Artefacts;
            Assert.IsNotNull(pgArtefacts);
            Assert.AreEqual(1, pgArtefacts.Count);
            found = pgArtefacts.TryGetCollection("Annots", out var col);
            
            Assert.IsTrue(found);
            Assert.IsNotNull(col);

            var annots = col as PDFAnnotationCollection;
            Assert.IsNotNull(annots);
            Assert.AreEqual(10, annots.Count);

            for (var i = 0; i < annots.Count; i++)
            {
                var annot = annots[i];
                Assert.IsNotNull(annot);
                var attach = annot as PDFAttachmentAnnotationEntry;
                Assert.IsNotNull(attach);
                
                Assert.IsNotNull(attach.Attachment);
                Assert.IsNotNull(attach.AttachmentFileSpec);
                Assert.AreEqual(embed, attach.AttachmentFileSpec);
                
                if (i % 2 == 0)
                {
                    //HTMLObject annotation
                    Assert.IsInstanceOfType(attach.LinkedFrom, typeof(HTMLObject));
                }
                else
                {
                    //HTMLLink annotation with inner text literal
                    Assert.IsInstanceOfType(attach.LinkedFrom, typeof(TextLiteral));
                }
            }
            
            //Page 2
            
            lpg = this._layout.AllPages[1];
            Assert.IsNotNull(lpg);

            pgArtefacts = lpg.Artefacts;
            Assert.IsNotNull(pgArtefacts);
            Assert.AreEqual(1, pgArtefacts.Count);
            found = pgArtefacts.TryGetCollection("Annots", out var col2);
            
            Assert.IsTrue(found);
            Assert.IsNotNull(col);

            annots = col2 as PDFAnnotationCollection;
            Assert.IsNotNull(annots);
            Assert.AreEqual(10, annots.Count);

            for (var i = 0; i < annots.Count; i++)
            {
                var annot = annots[i];
                Assert.IsNotNull(annot);
                var attach = annot as PDFAttachmentAnnotationEntry;
                Assert.IsNotNull(attach);
                
                Assert.IsNotNull(attach.Attachment);
                Assert.IsNotNull(attach.AttachmentFileSpec);
                Assert.AreEqual(embed, attach.AttachmentFileSpec);
                
                if (i % 2 == 0)
                {
                    //HTMLObject annotation
                    Assert.IsInstanceOfType(attach.LinkedFrom, typeof(HTMLObject));
                }
                else
                {
                    //HTMLLink annotation with inner text literal
                    Assert.IsInstanceOfType(attach.LinkedFrom, typeof(TextLiteral));
                }
            }
            
            //Page 3
            
            lpg = this._layout.AllPages[2];
            Assert.IsNotNull(lpg);

            pgArtefacts = lpg.Artefacts;
            Assert.IsNotNull(pgArtefacts);
            Assert.AreEqual(1, pgArtefacts.Count);
            found = pgArtefacts.TryGetCollection("Annots", out var col3);
            
            Assert.IsTrue(found);
            Assert.IsNotNull(col);

            annots = col3 as PDFAnnotationCollection;
            Assert.IsNotNull(annots);
            Assert.AreEqual(5, annots.Count);

            var types = new []
            {
                typeof(HTMLObject),
                typeof(TextLiteral),
                typeof(HTMLImage),
                typeof(TextLiteral),
                typeof(TextLiteral),
            };

            for (var i = 0; i < annots.Count; i++)
            {
                var annot = annots[i];
                Assert.IsNotNull(annot);
                var attach = annot as PDFAttachmentAnnotationEntry;
                Assert.IsNotNull(attach);
                
                Assert.IsNotNull(attach.Attachment);
                Assert.IsNotNull(attach.AttachmentFileSpec);
                Assert.AreEqual(embed, attach.AttachmentFileSpec);
                
                Assert.AreEqual(types[i], attach.LinkedFrom.GetType(), "Type failed at index " + i);
                //Could check types, but we know there are 8, so assumed good.
            }
        }
	}
}

