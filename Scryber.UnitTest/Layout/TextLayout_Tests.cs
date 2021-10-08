using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.PDF.Layout;
using Scryber.PDF;
using Scryber.Drawing;
using System.IO;

namespace Scryber.Core.UnitTests.Layout
{
    [TestClass]
    public class TextLayout_Tests
    {
        public TextLayout_Tests()
        {
        }

        PDFLayoutDocument layout;

        private void Doc_LayoutComplete(object sender, PDFLayoutEventArgs args)
        {
            this.layout = args.Context.DocumentLayout;
        }

        [TestMethod()]
        public void JustASingleLiteral()
        {
            var doc = new Document();
            var pg = new Page();

            pg.Margins = new PDFThickness(10);
            pg.BackgroundColor = new PDFColor(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            doc.Pages.Add(pg);
            pg.Contents.Add(new TextLiteral("This is a text run that should flow over multiple lines in the page with a default line height"));


            doc.RenderOptions.Compression = OutputCompressionType.None;
            SaveAsPDFAndText(doc, "Text_SingleLiteral");
            

            Assert.IsNotNull(layout, "The layout was not saved from the event");

            PDFLayoutLine first = layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutLine;
            PDFLayoutLine second = layout.AllPages[0].ContentBlock.Columns[0].Contents[1] as PDFLayoutLine;



        }

        [TestMethod()]
        public void ALiteralAfterABlock()
        {
            var doc = new Document();
            var pg = new Page();

            pg.Margins = new PDFThickness(10);
            pg.BackgroundColor = new PDFColor(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            doc.Pages.Add(pg);

            var div = new Div();
            div.Contents.Add(new TextLiteral("Inner Content"));
            div.BorderColor = new PDFColor(200, 255, 255);
            div.Padding = new PDFThickness(10);
            div.Height = 100;
            pg.Contents.Add(div);

            pg.Contents.Add(new TextLiteral("This is a text run that should flow over multiple lines in the page with a default line height"));

            doc.RenderOptions.Compression = OutputCompressionType.None;
            this.SaveAsPDFAndText(doc, "Text_LiteralAfterABlock");

            Assert.IsNotNull(layout, "The layout was not saved from the event");
            PDFLayoutRegion region = layout.AllPages[0].ContentBlock.Columns[0];

            PDFLayoutBlock innerblock = region.Contents[0] as PDFLayoutBlock;
            PDFLayoutLine first = region.Contents[1] as PDFLayoutLine;
            PDFLayoutLine second = region.Contents[2] as PDFLayoutLine;



        }

        private void SaveAsPDFAndText(Document doc, string stem)
        {
            using(var ms = new MemoryStream())
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);

                using (var stream = DocStreams.GetOutputStream(stem + ".pdf"))
                {
                    ms.Position = 0;
                    ms.CopyTo(stream);
                }

                using (var stream = DocStreams.GetOutputStream(stem + ".text"))
                {
                    ms.Position = 0;
                    ms.CopyTo(stream);
                }
            }
        }
    }
}
