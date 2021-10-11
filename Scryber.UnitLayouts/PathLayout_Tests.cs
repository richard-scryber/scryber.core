using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.PDF.Layout;
using Scryber.PDF;


namespace Scryber.UnitLayouts
{
    [TestClass()]
    public class PathLayout_Tests
    {
        public PathLayout_Tests()
        {
        }

        //Makes sure the shapes are aligned to the baseline, unless there is a vertical setting for v-align.
        [TestMethod()]
        public void LineAlignmentTest()
        {
            var doc = new Document();
            var page = new Page();
            doc.Pages.Add(page);

            page.Contents.Add(new TextLiteral("Before"));

            var path = new Line() { PositionMode = Scryber.Drawing.PositionMode.Inline };
            page.Contents.Add(path);

            
            doc.LayoutComplete += Doc_LayoutComplete;

            using (var ms = new System.IO.MemoryStream())
                doc.SaveAsPDF(ms);

            Assert.Inconclusive("Not implemented as padding allows lines to move down to the baseline");
        }


        private void Doc_LayoutComplete(object sender, LayoutEventArgs args)
        {
            var pg = args.Context.GetLayout<PDFLayoutDocument>().AllPages[0];
            var reg = pg.ContentBlock.Columns[0];
            var line = reg.Contents[0] as PDFLayoutLine;
            var runs = line.Runs;

            var offset = line.BaseLineOffset;

            //BeginText, TextRun, EndText, Line
            var path = runs[3] as PDFLayoutComponentRun;

        }
    }
}
