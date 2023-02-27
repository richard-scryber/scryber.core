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
        public void BlockPercentFullToPage()
        {

            Document doc = new Document();
            Section section = new Section();
            section.Width = 600;
            section.Height = 800;
            section.FontSize = 20;
            section.TextLeading = 25;
            doc.Pages.Add(section);

            Div relative = new Div()
            {
                Height = new Unit(100, PageUnits.Percent),
                Width = new Unit(100, PageUnits.Percent),
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red
            };

            relative.Contents.Add(new TextLiteral("100% width and height"));
            section.Contents.Add(relative);



            using (var ms = DocStreams.GetOutputStream("RelativePositioned_FullToPage.pdf"))
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

            Assert.AreEqual(600, block.Width);
            Assert.AreEqual(800, block.Height);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockPercentFullToPageMargins()
        {

            Document doc = new Document();
            Section section = new Section();
            section.Width = 600;
            section.Height = 800;
            section.FontSize = 20;
            section.TextLeading = 25;
            section.Margins = new Thickness(20);

            doc.Pages.Add(section);

            Div relative = new Div()
            {
                Height = new Unit(100, PageUnits.Percent),
                Width = new Unit(100, PageUnits.Percent),
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red
            };

            relative.Contents.Add(new TextLiteral("100% width and height"));
            section.Contents.Add(relative);



            using (var ms = DocStreams.GetOutputStream("RelativePositioned_FullToPageMargins.pdf"))
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

            Assert.AreEqual(600 - 40, block.Width);
            Assert.AreEqual(800 - 40, block.Height);
        }


        /// <summary>
        /// Tests the overflowing of the content for 100vh and 100vw with margins - this is as per browsers
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockViewPortFullToPageMargins()
        {

            Document doc = new Document();
            Section section = new Section();
            section.Width = 600;
            section.Height = 800;
            section.FontSize = 20;
            section.TextLeading = 25;
            section.Margins = new Thickness(20);

            doc.Pages.Add(section);

            Div relative = new Div()
            {
                Height = new Unit(100, PageUnits.ViewPortHeight),
                Width = new Unit(100, PageUnits.ViewPortWidth),
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red
            };

            relative.Contents.Add(new TextLiteral("100% viewport width and height"));
            section.Contents.Add(relative);



            using (var ms = DocStreams.GetOutputStream("RelativePositioned_FullToPageMargins.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(2, layout.AllPages.Count);

            //Should overflow because 100vh > page height with margins
            Assert.AreEqual(0, layout.AllPages[0].ContentBlock.Columns[0].Contents.Count);

            var pg = layout.AllPages[1];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);
            var block = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            Assert.AreEqual(600, block.Width);
            Assert.AreEqual(800, block.Height);
        }


        /// <summary>
        /// Tests the the content for 100vh and 100vw with margins to be clipped
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockViewPortFullToPageMarginsClipped()
        {

            Document doc = new Document();
            Section section = new Section();
            section.Width = 600;
            section.Height = 800;
            section.FontSize = 20;
            section.TextLeading = 25;
            section.OverflowAction = OverflowAction.Clip;
            section.Margins = new Thickness(20);

            doc.Pages.Add(section);

            Div relative = new Div()
            {
                Height = new Unit(100, PageUnits.ViewPortHeight),
                Width = new Unit(100, PageUnits.ViewPortWidth),
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red
            };

            relative.Contents.Add(new TextLiteral("100% viewport width and height"));
            section.Contents.Add(relative);



            using (var ms = DocStreams.GetOutputStream("RelativePositioned_FullClippedToPageMargins.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            //Clipped so it should all be on the first page
            Assert.AreEqual(1, layout.AllPages.Count);

            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);
            var block = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            Assert.AreEqual(600, block.Width);
            Assert.AreEqual(800, block.Height);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockViewPortWidthAndHeightRelativeToPage()
        {

            Document doc = new Document();
            Section section = new Section();
            section.Width = 600;
            section.Height = 800;
            section.FontSize = 20;
            section.TextLeading = 25;
            doc.Pages.Add(section);

            Div relative = new Div()
            {
                Height = new Unit(20, PageUnits.ViewPortHeight),
                Width = new Unit(25, PageUnits.ViewPortWidth),
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red
            };

            relative.Contents.Add(new TextLiteral("20% width and 25% height from page size"));
            section.Contents.Add(relative);



            using (var ms = DocStreams.GetOutputStream("RelativePositionedViewPort_BlockToPage.pdf"))
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

            Unit expectedWidth = (600) / 4.0;
            Unit expectedHeight = (800) / 5.0;

            Assert.AreEqual(expectedWidth, block.Width, "Widths did not match");
            Assert.AreEqual(expectedHeight, block.Height, "Heights did not match");
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockViewPortMaxRelativeToPage()
        {

            Document doc = new Document();
            Section section = new Section();
            section.Width = 600;
            section.Height = 800;
            section.FontSize = 20;
            section.TextLeading = 25;
            doc.Pages.Add(section);

            Div relative = new Div()
            {
                Height = new Unit(20, PageUnits.ViewPortMax),
                Width = new Unit(25, PageUnits.ViewPortMax),
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red
            };

            relative.Contents.Add(new TextLiteral("20% width and 25% height from page max"));
            section.Contents.Add(relative);



            using (var ms = DocStreams.GetOutputStream("RelativePositionedViewPortMax_BlockToPage.pdf"))
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

            Unit expectedWidth = (800) / 4.0;
            Unit expectedHeight = (800) / 5.0;

            Assert.AreEqual(expectedWidth, block.Width, "Widths did not match");
            Assert.AreEqual(expectedHeight, block.Height, "Heights did not match");
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockViewPortMinRelativeToPage()
        {

            Document doc = new Document();
            Section section = new Section();
            section.Width = 600;
            section.Height = 800;
            section.FontSize = 20;
            section.TextLeading = 25;
            doc.Pages.Add(section);

            Div relative = new Div()
            {
                Height = new Unit(20, PageUnits.ViewPortMin),
                Width = new Unit(25, PageUnits.ViewPortMin),
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red
            };

            relative.Contents.Add(new TextLiteral("20% width and 25% height from page min"));
            section.Contents.Add(relative);



            using (var ms = DocStreams.GetOutputStream("RelativePositionedViewPortMin_BlockToPage.pdf"))
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

            Unit expectedWidth = (600) / 4.0;
            Unit expectedHeight = (600) / 5.0;

            Assert.AreEqual(expectedWidth, block.Width, "Widths did not match");
            Assert.AreEqual(expectedHeight, block.Height, "Heights did not match");
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockPercentRelativeToPageMargins()
        {

            Document doc = new Document();
            Page section = new Page();
            section.Style.PageStyle.Width = 600;
            section.Style.PageStyle.Height = 800;
            section.FontSize = 20;
            section.TextLeading = 25;
            section.Margins = new Thickness(new Unit(10, PageUnits.Percent));
            section.BorderWidth = 1;
            section.Style.OverlayGrid.ShowGrid = true;
            section.Style.OverlayGrid.GridSpacing = 60;
            doc.Pages.Add(section);

            Div relative = new Div()
            {
                Height = new Unit(50, PageUnits.Percent),
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red
            };

            relative.Contents.Add(new TextLiteral("full width and 50% height in margins"));
            section.Contents.Add(relative);



            using (var ms = DocStreams.GetOutputStream("RelativePositioned_BlockToPageWithMargins.pdf"))
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

            Unit expectedWidth = (600.0)  -  ((600.0 / 10.0) * 2.0); //page width - 10% margins either side
            Unit expectedHeight = ((800.0) - ((800.0 / 10.0) * 2.0)) / 2.0; //50% of (page height - 10% margins)
            
            Assert.AreEqual(expectedWidth.PointsValue, block.Width.PointsValue, "Widths did not match");
            Assert.AreEqual(expectedHeight.PointsValue, block.Height.PointsValue, "Heights did not match");
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockPercentRelativeToContainer()
        {

            Document doc = new Document();
            Section section = new Section();
            section.Style.PageStyle.Width = 600;
            section.Style.PageStyle.Height = 800;
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

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockPercentRelativeToSizedContainer()
        {

            Document doc = new Document();
            Section section = new Section();
            section.Style.PageStyle.Width = 600;
            section.Style.PageStyle.Height = 800;
            section.FontSize = 20;
            section.TextLeading = 25;
            //section.Padding = 10;
            section.BackgroundColor = StandardColors.Silver;
            section.VerticalAlignment = VerticalAlignment.Middle;
            section.HorizontalAlignment = HorizontalAlignment.Center;

            doc.Pages.Add(section);

            Div wrapper = new Div()
            {
                ID = "wrapper",
                Width = new Unit(50, PageUnits.Percent),
                Height = new Unit(50, PageUnits.Percent),
                BorderWidth = 1,
                BorderColor = StandardColors.Blue,
                VerticalAlignment = VerticalAlignment.Middle,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            section.Contents.Add(wrapper);
            Div relative = new Div()
            {
                ID = "inner_relative",
                Height = new Unit(50, PageUnits.Percent),
                Width = new Unit(50, PageUnits.Percent),
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red,
                VerticalAlignment = VerticalAlignment.Middle,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            wrapper.Contents.Add(relative);

            relative.Contents.Add(new TextLiteral("50% width and 25% height with margins"));


            using (var ms = DocStreams.GetOutputStream("RelativePositioned_BlockToSizedContainer.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Unit parentBoundsWidth = 600;
            Unit parentBoundsHeight = 800;

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);
            var wrapperBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            

            Assert.AreEqual(1, wrapperBlock.Columns.Length);
            Assert.AreEqual(1, wrapperBlock.Columns[0].Contents.Count);

            Unit expectedWidth = 600 / 2.0;
            Unit expectedHeight = 800 / 2.0;
            Unit expectedX = (parentBoundsWidth - expectedWidth) / 2;
            Unit expectedY = (parentBoundsHeight - expectedHeight) / 2;

            Assert.AreEqual(expectedWidth, wrapperBlock.Width, "Widths did not match on wrapper");
            Assert.AreEqual(expectedHeight, wrapperBlock.Height, "Heights did not match on wrapper");
            Assert.AreEqual(expectedX, wrapperBlock.TotalBounds.X, "X Value did not match on wrapper");
            Assert.AreEqual(expectedY, wrapperBlock.TotalBounds.Y, "Y Value did not match on wrapper");

            var relativeBlock = wrapperBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(relativeBlock);

            parentBoundsWidth = expectedWidth;
            parentBoundsHeight = expectedHeight;

            expectedWidth = expectedWidth / 2.0;
            expectedHeight = expectedHeight / 2.0;

            expectedX = (parentBoundsWidth - expectedWidth) / 2;
            expectedY = (parentBoundsHeight - expectedHeight) / 2;

            Assert.AreEqual(expectedWidth, relativeBlock.Width, "Widths did not match on inner");
            Assert.AreEqual(expectedHeight, relativeBlock.Height, "Heights did not match on inner");
            Assert.AreEqual(expectedX, relativeBlock.TotalBounds.X, "X Value did not match on inner");
            Assert.AreEqual(expectedY, relativeBlock.TotalBounds.Y, "Y Value did not match on inner");
        }
    }
}
