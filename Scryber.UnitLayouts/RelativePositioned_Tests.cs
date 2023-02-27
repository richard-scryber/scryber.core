using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.PDF.Layout;
using Scryber.PDF;
using Scryber.Drawing;

namespace Scryber.UnitLayouts
{
    [TestClass()]
    public class RelativePositioned_Tests
    {
        const string TestCategoryName = "Layout";

        PDFLayoutDocument layout;

        /// <summary>
        /// Event handler that sets the layout instance variable, after the layout has completed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Doc_LayoutComplete(object sender, LayoutEventArgs args)
        {
            layout = args.Context.GetLayout<PDFLayoutDocument>();
        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockPercentRelativeToPage()
        {

            Document doc = new Document();
            Section section = new Section();
            section.Width = 600;
            section.Height = 800;
            section.FontSize = 20;
            section.TextLeading = 25;
            doc.Pages.Add(section);

            Div relative = new Div() {
                Height = new Unit(50, PageUnits.Percent),
                Width = new Unit(50, PageUnits.Percent),
                BorderWidth = 1, BorderColor = Drawing.StandardColors.Red };

            relative.Contents.Add(new TextLiteral("50% width and height"));
            section.Contents.Add(relative);

            

            using (var ms = DocStreams.GetOutputStream("RelativePositioned_BlockToPage.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);
            var block = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            Assert.AreEqual(600 / 2.0, block.Width);
            Assert.AreEqual(800 / 2.0, block.Height);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockPercentRelativeToContainer()
        {

            Document doc = new Document();
            Section section = new Section();
            section.Width = 600;
            section.Height = 800;
            section.FontSize = 20;
            section.TextLeading = 25;
            section.Padding = 10;
            section.BackgroundColor = StandardColors.Aqua;

            doc.Pages.Add(section);

            Div wrapper = new Div()
            {
                Margins = 10,
                Padding = 10,
                BorderWidth = 1,
                BorderColor = StandardColors.Blue
            };

            section.Contents.Add(wrapper);
            Div relative = new Div()
            {
                Height = new Unit(25, PageUnits.Percent),
                Width = new Unit(50, PageUnits.Percent),
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red
            };
            wrapper.Contents.Add(relative);

            relative.Contents.Add(new TextLiteral("50% width and 25% height with margins"));
            

            using (var ms = DocStreams.GetOutputStream("RelativePositioned_BlockToContainer.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);
            var wrapperBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(580, wrapperBlock.Width); //page - margins

            Assert.AreEqual(1, wrapperBlock.Columns.Length);
            Assert.AreEqual(1, wrapperBlock.Columns[0].Contents.Count);

            var relativeBlock = wrapperBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(relativeBlock);

            Unit expectedWidth = (580 - 40) / 2.0;
            Unit expectedHeight = (800 - 60) / 4.0;
            Assert.AreEqual(expectedWidth, relativeBlock.Width, "Widths did not match");
            Assert.AreEqual(expectedHeight, relativeBlock.Height, "Heights did not match");
        }


    }
}
